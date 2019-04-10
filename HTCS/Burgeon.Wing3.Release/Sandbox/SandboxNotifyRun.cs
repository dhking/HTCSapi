using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Sandbox
{
    public class SandboxNotifyRun
    {
        /// <summary>
        /// 通知指定版本升级启动程序
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool Notify(string version, string exefile, string versionfile)
        {
            SandboxRunNotifyBag bag = new SandboxRunNotifyBag(version, Utils.EncryptUtil.Md5(Utils.ManagementUtil.GetCpuIdentifier()), exefile, versionfile);
            return bag.Save();
        }

        /// <summary>
        /// 启动升级程序
        /// </summary>
        /// <returns></returns>
        public static void RunNotify(SandboxRunNotifyBag bag)
        {
            //从沙箱环境升级 解决当升级包中包含升级程序时会起冲突
            Utils.SandboxRunUtil.Run(bag.ExeFile, new string[] { bag.VersionFile, Utils.CmdUtil.GetTool("") });
        }
    }
}
