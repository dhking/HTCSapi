using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Models;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 将选定文件夹以及文件列表或者数据库文件或者数据库语句
    /// 生成标准版本资源文件 
    /// </summary>
    public class VersionBuildCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "version-build";
            }
        }

        public override CommandResult Execute(CommandContext context)
        {
            LocalVersion version = context.GetParam<LocalVersion>("localVersion");
            CommandResult cResult = null;
            List<string> resources = context.GetParam<List<string>>("resources");

            if (version == null)
            {
                cResult = new CommandResult(ResultStatus.InValid, "无法生成资源文件，请提供基础版本(localVersion)");
            }
            else if (resources == null || resources.Count <= 0)
            {
                cResult = new CommandResult(ResultStatus.InValid, "没有需要打包的资源文件(resources)，无法生成版本资源");
            }
            else
            {
                cResult = BuildVersion(resources, version);
            }

            return cResult;
        }

        private CommandResult BuildVersion(List<string> resources, LocalVersion version)
        {
            CommandResult cResult = null;

            try
            {
                string v = "V" + DateTime.Now.ToString("yyyyMMddHHmmsss");
                version.PartVersionDIR = Utils.IOUtil.CombineDIR(version.TopPartVersionDIR, v);

                AppVersion appVersion = new AppVersion();
                Utils.SetupUtil setup = new Utils.SetupUtil(version);
                appVersion.VersionRAR = setup.Setup(resources);
                appVersion.Version = v;
                cResult = new CommandResult(ResultStatus.Success, "创建成功");
                cResult.Rows = appVersion;
            }
            catch (Exception ex)
            {
                cResult = new CommandResult(ResultStatus.Error, ex.ToString());
            }

            return cResult;
        }
    }
}
