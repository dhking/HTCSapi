using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Utils;

namespace Burgeon.Wing3.Release.Environment
{
    /// <summary>
    /// 项目 版本资源文件环境
    /// </summary>
    public class ResourceEnvironment
    {
        private static ResourceEnvironment environment;

        /// <summary>
        /// 版本资源环境参数实例
        /// </summary>
        public static ResourceEnvironment Environment
        {
            get
            {
                if (environment == null)
                {
                    environment = new ResourceEnvironment();
                }
                return environment;
            }
        }

        private ResourceEnvironment()
        {

        }

        /// <summary>
        /// 判断当前程序是否为发布主服务器
        /// </summary>
        public bool IsHost
        {
            get
            {
                return Utils.ConfigurationUtil.GetAppsetting("ReleaseHost", "N") == "Y";
            }
        }

        /// <summary>
        /// 获取当前升级环境的客户程序信息
        /// </summary>
        public Client Client
        {
            get
            {
                return new Client();
            }
        }

        /// <summary>
        /// 获取当前升级主服务器地址
        /// </summary>
        /// <returns></returns>
        public string GetMainHostURL()
        {
            return Utils.ConfigurationUtil.GetAppsetting("MAIN", "g.burgeon.cn:8085");
        }

        /// <summary>
        /// 获取指定命令的接口访问地址
        /// </summary>
        /// <returns></returns>
        public string GetCommandURL(string host, string command)
        {
            return string.Format("{0}/{1}?command={2}", host, ConfigurationUtil.GetAppsetting("R-REST", "release/command"), command);
        }

        /// <summary>
        /// 获取临时版本库部分版本文件存放位置绝对路径
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetClientPartVersionSandboxFile(string name)
        {
            return Utils.IOUtil.CombineDIR(ClientSandboxPartVersionDIR, name);
        }

        /// <summary>
        /// 获取临时版本库 不完整版本 发布目录
        /// </summary>
        public string ClientSandboxPartVersionDIR
        {
            get
            {
                return Utils.ConfigurationUtil.GetAppsetting("SandBox_PartDIR", IOUtil.AutoCreateDIR(IOUtil.CombineDIR(ClientSandboxDIR, "PartVersions")));
            }
        }

        /// <summary>
        /// 获取临时版本库主目录
        /// </summary>
        public string ClientSandboxDIR
        {
            get
            {
                return Utils.ConfigurationUtil.GetAppsetting("SandBox_DIR", IOUtil.AutoCreateDIR(IOUtil.CombineDIR(AppDomainParent, "Sandbox")));
            }
        }

        /// <summary>
        /// 版本升级启动通知目录
        /// </summary>
        public string ClientSandboxNotifyDIR
        {
            get
            {
                return Utils.IOUtil.AutoCreateDIR(IOUtil.CombineDIR(ClientSandboxDIR, "Notify"));
            }
        }

        public string ClientSandboxLogDIR
        {
            get
            {
                return Utils.IOUtil.AutoCreateDIR(IOUtil.CombineDIR(ClientSandboxDIR, "Log"));
            }
        }

        /// <summary>
        /// 获取当前运行程序父级目录绝对路径
        /// </summary>
        public static string AppDomainParent
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ClientBaseDIR))
                {
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                    return dir.Parent.FullName;
                }
                else
                {
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(ClientBaseDIR);
                    return dir.FullName;
                }
            }
        }

        public static string ClientBaseDIR
        {
            get;
            set;
        }

        /// <summary>
        /// 获取当前设置的顶级版本库目录
        /// </summary>
        public string TopVersionDIR
        {
            get
            {
                string dir = IOUtil.AutoCreateDIR(ConfigurationUtil.GetAppsetting("TopDIR"));
                if (!IOUtil.ExistsDIR(dir))
                {
                    throw new Exception("没有设置顶级版本目录(TopDIR)");
                }
                return dir;
            }
        }

        /// <summary>
        /// 获取当前设置存放创建版本的目录
        /// </summary>
        public string TopBuilVerionDIR
        {
            get
            {
                string dir = IOUtil.AutoCreateDIR(IOUtil.CombineDIR(TopVersionDIR, "PartVersions"));
                return dir;
            }
        }
    }
}
