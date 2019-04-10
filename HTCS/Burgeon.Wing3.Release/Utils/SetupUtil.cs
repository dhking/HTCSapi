using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Models;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// 部分版本setup.bat生成工具
    /// </summary>
    public class SetupUtil
    {
        private StringBuilder installBuilder = null;

        private StringBuilder unInstallBuilder = null;

        private StringBuilder localBuilder = null;

        private LocalVersion localVersion = null;

        private string destinationBaseDIR = null;

        public SetupUtil(LocalVersion version, string destinationBaseDir = "%BaseDIR%")
        {
            installBuilder = new StringBuilder();
            unInstallBuilder = new StringBuilder();
            localBuilder = new StringBuilder();
            localVersion = version;
            destinationBaseDIR = destinationBaseDir;
            if (version.VersionDIR.IndexOf(":") < 0)
            {
                version.VersionDIR = Utils.IOUtil.CombineDIR(version.TopDIR, version.VersionDIR.TrimStart('\\'));
            }
        }

        /// <summary>
        /// 创建发布与取消发布bat命令
        /// </summary>
        /// <param name="resources">资源文件</param>
        public string Setup(List<string> resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }

            this.SetupStart();

            foreach (string resource in resources)
            {
                if (resource.IndexOf(":") < 0)
                {
                    this.Setup(IOUtil.CombineDIR(localVersion.TopDIR, resource.TrimStart('\\')));
                }
                else
                {
                    this.Setup(resource);
                }
            }

            return this.SetupEnd();
        }

        private void Setup(string resource)
        {
            if (IOUtil.ExistsFile(resource))
            {
                this.AutoSetup(resource);
            }
            else if (IOUtil.ExistsDIR(resource))
            {
                this.SetupDIR(resource);
            }
        }

        private void AutoSetup(string resource)
        {
            if (this.IsSql(resource))
            {
                this.SetupSQL(resource);
            }
            else
            {
                this.SetupFile(resource);
            }
        }

        private void SetupSQL(string sqlfile)
        {
            string rel = IOUtil.TrimBaseDIR(sqlfile, localVersion.VersionDIR);
            if (!rel.StartsWith("\\"))
            {
                rel = "\\" + rel;
            }
            string source = string.Format(@".\DB{0}", rel);

            localBuilder.AppendLine(VersionBatchCommandUtil.XCopyFile(sqlfile, source));
        }

        private void SetupDIR(string dir)
        {
            string rel = IOUtil.TrimBaseDIR(dir, localVersion.VersionDIR);
            if (!rel.StartsWith("\\"))
            {
                rel = "\\" + rel;
            }
            string source = string.Format(".{0}", rel);
            string destination = string.Format("{0}{1}", destinationBaseDIR, rel);
            string bakDestination = string.Format(@".\Cache{0}", rel);

            //1.安装
            //先备份目录
            installBuilder.AppendLine(VersionBatchCommandUtil.XCopyDIR(destination, bakDestination));
            //发布目录
            installBuilder.AppendLine(VersionBatchCommandUtil.XCopyDIR(source, destination));

            //2.卸载
            //将备份目录覆盖 发布目录
            unInstallBuilder.AppendLine(VersionBatchCommandUtil.XCopyDIR(bakDestination, destination));
            //删除备份目录
            unInstallBuilder.AppendLine(VersionBatchCommandUtil.Rd(bakDestination));

            //3.本地版本创建
            localBuilder.AppendLine(VersionBatchCommandUtil.XCopyDIR(dir, source));
        }

        private void SetupFile(string file)
        {
            string rel = IOUtil.TrimBaseDIR(file, localVersion.VersionDIR);
            string source = string.Format(".{0}", rel);
            if (!rel.StartsWith("\\"))
            {
                rel = "\\" + rel;
            }
            string destination = string.Format("{0}{1}", destinationBaseDIR, rel);

            string bakDestination = string.Format(@".\Cache{0}", rel);

            //1.安装
            //先备份文件
            installBuilder.AppendLine(VersionBatchCommandUtil.XCopyFile(destination, bakDestination));
            //发布文件
            installBuilder.AppendLine(VersionBatchCommandUtil.XCopyFile(source, destination));

            //2.卸载
            //将备份文件覆盖发布文件
            unInstallBuilder.AppendLine(VersionBatchCommandUtil.Del(destination));
            unInstallBuilder.AppendLine(VersionBatchCommandUtil.Move(bakDestination, destination));

            //3.本地版本创建
            localBuilder.AppendLine(VersionBatchCommandUtil.XCopyFile(file, source));
        }

        private bool IsSql(string file)
        {
            return string.Equals(System.IO.Path.GetExtension(file), "sql", StringComparison.InvariantCultureIgnoreCase);
        }

        private void SetupStart()
        {
            installBuilder.Clear();
            unInstallBuilder.Clear();
        }

        private string SetupEnd()
        {
            string rarPath = null;
            string pVersionDIR = localVersion.PartVersionDIR;

            IOUtil.AutoCreateDIR(pVersionDIR);

            installBuilder.AppendLine(VersionBatchCommandUtil.Move(@".\setup.bat", @".\setup.bat.runed"));
            unInstallBuilder.AppendLine(VersionBatchCommandUtil.Move(@".\setup.bat.runed", @".\setup.bat"));

            //安装bat命令文件 路径
            string setupfile = Utils.IOUtil.CombineDIR(pVersionDIR, "setup.bat");
            //卸载bat命令文件 路径
            string unsetupfile = Utils.IOUtil.CombineDIR(pVersionDIR, "unsetup.bat");
            //本地版本准备bat命令 路径
            string localbuild = Utils.IOUtil.CombineDIR(pVersionDIR, "local.bat");

            //生成安装bat命令文件
            Utils.IOUtil.SaveFile(setupfile, installBuilder.ToString());
            //生成卸载bat命令文件
            Utils.IOUtil.SaveFile(unsetupfile, unInstallBuilder.ToString());
            //生成本地版本准备bat命令
            Utils.IOUtil.SaveFile(localbuild, localBuilder.ToString());

            //开始准备本地版本文件
            Models.ProcessResult pResult = Utils.CmdUtil.ExecuteBat(localbuild);
            if (pResult.IsError)
            {
                throw pResult.Exception;
            }
            else
            {
                Utils.IOUtil.Delete(localbuild);
                string rarName = Utils.IOUtil.GetFileName(pVersionDIR) + ".rar";
                string dir = localVersion.TopPartVersionDIR;
                //开始打包生成版本文件
                pResult = Utils.RarUtil.RARXFM(pVersionDIR, dir, rarName);
                Utils.IOUtil.Delete(pVersionDIR);
                rarPath = Utils.IOUtil.CombineDIR(dir, rarName);
            }

            if (pResult.IsError)
            {
                throw pResult.Exception;
            }

            return rarPath;
        }
    }
}
