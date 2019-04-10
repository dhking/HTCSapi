using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;

using Burgeon.Wing3.Release.Models;
using System.Globalization;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// 进程终结者
    /// </summary>
    public class Killer
    {
        /// <summary>
        /// 删除指定目录下指定文件进程
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool KillProcess(string[] files)
        {
            bool result = false;

            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    string dir = p.MainModule.FileName;
                    if (files.Count(m => string.Equals(m, dir, StringComparison.InvariantCultureIgnoreCase)) > 0)
                    {
                        result = KillProcess(p.Id.ToString());
                    }
                }
                catch (System.Exception exp)
                {

                }
            }

            return result;
        }

        /// <summary>
        /// 关闭指定id的进程
        /// </summary>
        /// <param name="processid"></param>
        /// <returns></returns>
        public static bool KillProcess(string processid)
        {
            ProcessResult p = ExecuteCmd(string.Format("tskill {0}", processid));
            return p.IsError;
        }

        /// <summary>
        /// 关闭指定id进程的所有子进程
        /// </summary>
        /// <returns></returns>
        public static bool KillProcessAndChildren(int processid)
        {
            Process[] procs = Process.GetProcesses();
            for (int i = 0; i < procs.Length; i++)
            {
                if (IsChildProcessOfParentId(procs[i].Id, processid))
                {
                    KillProcessAndChildren(procs[i].Id);
                }
            }

            try
            {
                Process myProc = Process.GetProcessById(processid);
                myProc.Kill();
            }
            catch (ArgumentException)
            {
                ;
            }

            return true;
        }

        /// <summary>
        /// 关闭指定id的进程所在的进程树 （慎用）
        /// </summary>
        /// <param name="processid"></param>
        /// <returns></returns>
        public static bool KillTreeProcess(int processid)
        {
            Process[] procs = Process.GetProcesses();
            for (int i = 0; i < procs.Length; i++)
            {
                if (GetParentProcessId(procs[i].Id) == processid)
                    KillTreeProcess(procs[i].Id);
            }

            try
            {
                Process myProc = Process.GetProcessById(processid);
                myProc.Close();
            }
            catch (ArgumentException)
            {
                ;
            }

            return true;
        }

        private static bool IsChildProcessOfParentId(int Id, int parent)
        {
            bool isChild = false;

            using (ManagementObject mo = new ManagementObject("win32_process.handle='" + Id.ToString(CultureInfo.InvariantCulture) + "'"))
            {
                try
                {
                    mo.Get();
                }
                catch (ManagementException)
                {
                    return false;
                }
                isChild = Convert.ToInt32(mo["ParentProcessId"], CultureInfo.InvariantCulture) == parent;
            }
            return isChild;
        }

        private static int GetParentProcessId(int Id)
        {
            int parentPid = 0;
            using (ManagementObject mo = new ManagementObject("win32_process.handle='" + Id.ToString(CultureInfo.InvariantCulture) + "'"))
            {
                try
                {
                    mo.Get();
                }
                catch (ManagementException)
                {
                    return -1;
                }
                parentPid = Convert.ToInt32(mo["ParentProcessId"], CultureInfo.InvariantCulture);
            }
            return parentPid;
        }

        /// <summary>
        /// 开启一个进程 并且返回执行结果
        /// </summary>
        /// <param name="args">执行当前进程 初始化时需要的命令参数</param>
        /// <param name="ex">进程执行异常</param>
        /// <returns></returns>
        public static ProcessResult ExecuteCmd(string args = null)
        {
            ProcessResult pResult = null;
            try
            {
                Process process = new Process();
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = "Cmd.exe";
                pInfo.Arguments = args;
                pInfo.UseShellExecute = false;
                pInfo.CreateNoWindow = true;
                pInfo.RedirectStandardOutput = true;
                pInfo.RedirectStandardInput = true;
                pInfo.RedirectStandardError = true;
                pInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo = pInfo;
                process.Start();
                process.StandardInput.WriteLine(args);
                process.StandardInput.Close();
                process.WaitForExit();
                string error = process.StandardError.ReadToEnd();
                string output = process.StandardOutput.ReadToEnd();
                process.Close();
                if (string.IsNullOrWhiteSpace(error))
                {
                    pResult = new ProcessResult(output);
                }
                else
                {
                    pResult = new ProcessResult(output, new Exception(error));
                }
            }
            catch (Exception e)
            {
                pResult = new ProcessResult(e);
            }
            return pResult;
        }
    }
}
