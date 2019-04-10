using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Burgeon.Wing3.Release.Models;

namespace Burgeon.Wing3.Release.Utils
{
    public class SandboxRunUtil
    {
        /// <summary>
        /// 在沙箱环境运行指定Net程序
        /// </summary>
        public static void Run(string file, string[] copys, bool notRun = false)
        {
            if (!System.IO.File.Exists(file))
            {
                throw new ArgumentException(string.Format("文件({0}) 不存在", file));
            }

            string sandboxDIR = IOUtil.AutoCreateDIR(IOUtil.CombineDIR(Environment.ResourceEnvironment.Environment.ClientSandboxDIR, "Run"));
            string run = IOUtil.CombineDIR(sandboxDIR, System.IO.Path.GetFileName(file));
            string[] references = GetReferences(file);
            StopSandboxRuning(run);
            CopyToSandbox(file, sandboxDIR);
            CopyFiles(references, sandboxDIR);
            CopyFiles(copys, sandboxDIR);

            if (System.IO.File.Exists(run))
            {
                //启动升级程序
                ProcessResult pResult = Utils.CmdUtil.ExecuteAppNoWait(run, "");
                if (pResult.IsError)
                {
                    Sandbox.SandboxRunLog.Error(pResult.Exception.ToString());
                }
                else
                {
                    Sandbox.SandboxRunLog.Log("沙箱升级程序已启动，这个过程会有点久，请耐心等候...");
                }
            }
            else
            {
                Sandbox.SandboxRunLog.Error("沙箱环境找不到升级程序");
            }
        }

        private static void StopSandboxRuning(string file)
        {
            Utils.Killer.KillProcess(new string[] { file });
        }

        public static string[] GetReferences(string file)
        {
            string baseDIR = Utils.IOUtil.GetNameOfDIR(file);
            if (string.IsNullOrEmpty(baseDIR))
            {
                baseDIR = AppDomain.CurrentDomain.BaseDirectory;
            }
            List<string> allReferences = new List<string>();
            GetReferences(baseDIR, file, allReferences);
            return allReferences.ToArray();
        }

        public static void GetReferences(string baseDIR, string file, List<string> allReferences)
        {
            if (Utils.IOUtil.ExistsFile(file))
            {
                Assembly assembly = Assembly.Load(System.IO.File.ReadAllBytes(file));
                string[] references = assembly.GetReferencedAssemblies().Select(m => GetNonGacAssemblyFullPath(m.Name, baseDIR)).Where(m => m != null).ToArray();
                foreach (string refe in references)
                {
                    if (!allReferences.Contains(refe))
                    {
                        allReferences.Add(refe);
                        GetReferences(baseDIR, refe, allReferences);
                    }
                }
            }
        }

        public static string GetNonGacAssemblyFullPath(string name, string baseDIR)
        {
            string dll = Utils.IOUtil.CombineDIR(baseDIR, name);
            if (Utils.IOUtil.ExistsFile(dll + ".dll"))
            {
                return dll + ".dll";
            }
            else if (Utils.IOUtil.ExistsFile(dll + ".exe"))
            {
                return dll + ".exe";
            }
            else
            {
                return null;
            }
        }

        public static void CopyFiles(string[] files, string sandboxDIR)
        {
            if (files != null)
            {
                foreach (string refe in files)
                {
                    CopyToSandbox(refe, sandboxDIR);
                }
            }
        }

        public static void CopyToSandbox(string file, string sandboxDIR)
        {
            //如果是目录
            if (Utils.IOUtil.ExistsDIR(file))
            {
                System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(file);

                Utils.IOUtil.CopyDirectory(file, IOUtil.CombineDIR(sandboxDIR, info.Name));
            }
            else if (Utils.IOUtil.ExistsFile(file))
            {
                string sandboxResource = IOUtil.CombineDIR(sandboxDIR, System.IO.Path.GetFileName(file));
                System.IO.File.Copy(file, sandboxResource, true);
            }
        }
    }
}
