using Burgeon.Wing3.Release.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// Cmd.exe辅助程序类
    /// </summary>
    public class CmdUtil
    {
        /// <summary>
        /// 获取bin下tools文件夹下工具
        /// </summary>
        /// <param name="tool"></param>
        /// <returns></returns>
        public static string GetTool(string tool, string dir = "Tools")
        {
            string baseDIR = AppDomain.CurrentDomain.RelativeSearchPath;
            if (string.IsNullOrEmpty(baseDIR))
            {
                baseDIR = AppDomain.CurrentDomain.BaseDirectory;
            }
            return Utils.IOUtil.CombineDIR(baseDIR, dir, tool);
        }

        /// <summary>
        /// 开启一个进程 并且返回执行结果
        /// </summary>
        /// <returns></returns>
        public static ProcessResult ExecuteCmd(string args = null)
        {
            ProcessResult pResult = null;
            try
            {
                Process process = new Process();
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = "Cmd.exe";
                pInfo.UseShellExecute = false;
                pInfo.CreateNoWindow = true;
                pInfo.RedirectStandardInput = true;
                pInfo.RedirectStandardOutput = true;
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

        /// <summary>
        /// 执行.sql脚本 使用默认cmd.exe
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pass">密码</param>
        /// <param name="dataBase">数据库名</param>
        /// <param name="servername">服务器</param>
        /// <param name="fileName">sql文件名(完整路径)</param>
        ///<param name="exception">异常消息</param>
        public static string ExecuteSqlfilecmdExe(string userName, string pass, string dataBase, string servername, string fileName, out Exception exception)
        {
            string output = string.Empty;
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;//不弹出窗体
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            exception = null;
            //string Path = Application.StartupPath.ToString();
            string Parameter = string.Format("osql.exe -U {0} -P {1} -S  {2} -d {3} -i {4}", userName, pass, servername, dataBase, fileName);
            try
            {
                //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                p.Start();
                p.StandardInput.WriteLine(Parameter);
                p.StandardInput.WriteLine("exit");
                p.StandardInput.WriteLine("exit");
                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();
            }
            catch (Exception e)
            {
                exception = e;
            }
            return output;
        }

        /// <summary>
        /// 执行.sql脚本 (测试时有问题)
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pawd">密码</param>
        /// <param name="dataBase">数据库名</param>
        /// <param name="serverUrl">服务器</param>
        /// <param name="fileName">sql文件名(完整路径)</param>
        public static void ExecuteSQLFile(string userName, string pawd, string dataBase, string serverUrl, string fileName)
        {
            try
            {
                System.Diagnostics.Process pr = new System.Diagnostics.Process();
                pr.StartInfo.FileName = "osql.exe";
                pr.StartInfo.Arguments = string.Format("-U {0} -P {1} -d {2} -s {3} -i {4}.sql", userName, pawd, dataBase, serverUrl, fileName);// "-U sa -P sa -d master -s 127.0.0.1 -i Sql.sql";
                pr.StartInfo.UseShellExecute = false;
                pr.StartInfo.RedirectStandardOutput = true;  //重定向输出
                pr.StartInfo.CreateNoWindow = true;////不弹出窗体
                pr.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;//隐藏输出窗口
                pr.Start();
                System.IO.StreamReader sr = pr.StandardOutput;
                Console.WriteLine(sr.ReadToEnd());
                pr.WaitForExit();
                pr.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 启动指定应用程序 并且不等待
        /// </summary>
        /// <returns></returns>
        public static ProcessResult ExecuteAppNoWait(string file, string args = "")
        {
            ProcessResult pResult = null;
            try
            {
                Process process = new Process();
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = file;
                pInfo.UseShellExecute = false;
                pInfo.CreateNoWindow = true;
                pInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(file);
                pInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo = pInfo;
                process.Start();
                pResult = new ProcessResult(string.Format("已启动程序{0}", file));
            }
            catch (Exception e)
            {
                pResult = new ProcessResult(e);
            }
            return pResult;
        }

        /// <summary>
        /// 指定指定的bat命令
        /// </summary>
        /// <returns></returns>
        public static ProcessResult ExecuteBat(string file)
        {
            ProcessResult pResult = null;
            try
            {
                Process process = new Process();
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = file;
                pInfo.UseShellExecute = false;
                pInfo.CreateNoWindow = true;
                pInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(file);
                pInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo = pInfo;
                process.Start();
                process.WaitForExit();
                pResult = new ProcessResult("执行完毕");
            }
            catch (Exception e)
            {
                pResult = new ProcessResult(e);
            }
            return pResult;
        }
    }
}
