using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Globalization;
using System.Diagnostics;

using Burgeon.Wing3.Release.Models;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// 进程辅助工具
    /// </summary>
    public class ProcessUtil
    {
        /// <summary>
        /// 关闭指定id的进程
        /// </summary>
        /// <param name="processid"></param>
        /// <returns></returns>
        public static bool KillProcess(string processid)
        {
            ProcessResult p = CmdUtil.ExecuteCmd(string.Format("tskill {0}", processid));
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
    }
}
