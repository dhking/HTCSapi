using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Burgeon.Wing3.Release.Sandbox
{
    /// <summary>
    /// 沙箱升级程序运行通知包
    /// </summary>
    public class SandboxRunNotifyBag
    {
        public const string notifyfile = "version.upgrade";

        private static string file = Utils.IOUtil.CombineDIR(Environment.ResourceEnvironment.Environment.ClientSandboxNotifyDIR, notifyfile);

        public SandboxRunNotifyBag() { }

        public SandboxRunNotifyBag(string version, string cpu, string exefile, string versionfile)
        {
            this.Version = version;
            this.Passport = cpu;
            this.ExeFile = exefile;
            this.VersionFile = versionfile;
        }

        /// <summary>
        /// 获取或者设置当前升级程序位置
        /// </summary>
        public string ExeFile { get; set; }

        /// <summary>
        /// 获取或者设置当前升级程序元数据文件位置
        /// </summary>
        public string VersionFile { get; set; }

        /// <summary>
        /// 当前要升级的版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 当前包所在的服务器的CPU唯一编号 进行过md5编码的值
        /// </summary>
        public string Passport { get; set; }

        /// <summary>
        /// 验证当前通知包是否合法
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            return string.Equals(this.Passport, Utils.EncryptUtil.Md5(Utils.ManagementUtil.GetCpuIdentifier()));
        }

        /// <summary>
        /// 生成当前版本升级启动通知文件
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            return Utils.XmlDesSerializeUtil.Serialize<SandboxRunNotifyBag>(this, file);
        }

        /// <summary>
        /// 读取升级通知文件
        /// </summary>
        /// <returns></returns>
        public static SandboxRunNotifyBag Read()
        {
            return Utils.XmlDesSerializeUtil.DeSerialize<SandboxRunNotifyBag>(file);
        }
    }
}
