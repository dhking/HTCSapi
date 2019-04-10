using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Models;
using System.IO;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// 文件压缩文件 与解压文件
    /// </summary>
    public class RarUtil
    {
        /// <summary> 
        /// 利用 WinRAR 进行解压缩 
        /// </summary> 
        /// <param name="targetPath">文件解压后的存放路径(绝对路径 不需要包含后缀名 只需要指定存放目录即可)</param> 
        /// <param name="srcPath">要解压的文件的绝对路径</param> 
        ///<param name="exception">异常消息对象</param>
        /// <returns>true 或 false。解压缩成功返回 true，反之，false。</returns> 
        public static ProcessResult UnRAR(string targetPath, string srcPath)
        {
            ProcessResult pResult = null;

            try
            {
                //自动创建目标目录
                IOUtil.AutoCreateDIR(targetPath);
                //解压缩命令，相当于在要压缩文件(rarName)上点右键->WinRAR->解压到当前文件夹 
                string cmd = string.Format("x \"{0}\" \"{1}\" -y", Path.GetFileName(srcPath), targetPath);
                Process process = new Process();
                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.FileName = CmdUtil.GetTool("WinRAR.exe");
                startinfo.Arguments = cmd;
                startinfo.WindowStyle = ProcessWindowStyle.Hidden;
                startinfo.WorkingDirectory = Path.GetDirectoryName(srcPath);
                process.StartInfo = startinfo;
                process.Start();
                process.WaitForExit();
                process.Close();
                pResult = new ProcessResult();
            }
            catch (Exception ex)
            {
                pResult = new ProcessResult(ex);
            }
            return pResult;
        }

        /// <summary> 
        /// 利用 WinRAR 进行压缩 ,具体查看 http://www.okbase.net/doc/details/2582
        /// </summary> 
        /// <param name="path">将要被压缩的文件夹 或者 文件</param> 
        /// <param name="rarPath">压缩后的 .rar 的存放目录（绝对路径）</param> 
        /// <param name="rarName">压缩文件的名称（包括后缀）</param> 
        /// <param name="xfileName">排除文件夹里所有文件，以及文件本身；多个文件直接以；隔开 如：bin ；obj</param> 
        ///<param name="delete">是否删除已存在的压缩包</param>
        /// <returns>true 或 false。压缩成功返回 true，反之，false。</returns> 
        public static ProcessResult RARXFM(string path, string rarPath, string rarName, string xfileName = "", bool isDelete = true)
        {
            ProcessResult pResult = null;

            try
            {
                if (isDelete)
                {
                    IOUtil.Delete(IOUtil.CombineDIR(rarPath, rarName));
                }

                string targetrarPath = IOUtil.AutoCreateDIR(rarPath);//存放路径不存在 创建
                //压缩命令，相当于在要压缩的文件夹(path)上点右键->WinRAR->添加到压缩文件->输入压缩文件名(rarName) -ibck
                string cmd = string.Format("a {0} \"{1}\" -ep1 -o+ -inul -r", rarName, path);

                //-x*\\bin\\* -x*\\bin -x*\\obj\\* -x*\\obj     ---排除bin,obj文件夹里所有文件，以及文件本身
                if (!string.IsNullOrEmpty(xfileName))
                {
                    string[] arrarlist = xfileName.Split(';');
                    string arrtr = string.Empty;
                    foreach (string str in arrarlist)
                    {
                        arrtr += string.Format(" -x*\\{0}\\* -x*\\{0} ", str);
                    }
                    cmd = cmd + arrtr;
                }

                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.FileName =Utils.CmdUtil.GetTool("WinRAR.exe");
                startinfo.Arguments = cmd;                          //设置命令参数 
                startinfo.WindowStyle = ProcessWindowStyle.Hidden;  //隐藏 WinRAR 窗口 
                startinfo.WorkingDirectory = rarPath;
                Process process = new Process();
                process.StartInfo = startinfo;
                process.Start();
                process.WaitForExit(); //无限期等待进程 winrar.exe 退出 
                process.Close();
                pResult = new ProcessResult();
            }
            catch (Exception ex)
            {
                pResult = new ProcessResult(ex);
            }
            return pResult;
        }
    }
}
