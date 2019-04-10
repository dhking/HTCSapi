using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using Burgeon.Wing3.Release.Models;
using Burgeon.Wing3.Release.StepLog;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 接收AwakenSendResourceCommand 的版本唤醒信息
    /// 并且自主下载版本文件进行 并且唤醒升级命令 进行升级
    /// </summary>
    public class AwakenReceiveResourceCommand : BaseCommand
    {
        private ILogger log = null;

        public override ILogger Logger
        {
            get
            {
                return log;
            }
        }

        public override string Command
        {
            get
            {
                return "awaken-receive";
            }
        }

        public override bool Authrize()
        {
            return true;//默认不验证
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult cResult = this.GetVersion();

            if (cResult.Status == ResultStatus.Success)
            {
                AppVersion version = cResult.ReadAs<AppVersion>();

                CommandContext vcontext = new CommandContext();
                vcontext["version"] = version;
                cResult = new VersionUpgradeCommand().Execute(vcontext);
            }

            return cResult;
        }

        private CommandResult GetVersion()
        {
            CommandResult cResult = null;

            try
            {
                HttpRequest request = HttpContext.Current.Request;

                AppVersion version = Newtonsoft.Json.JsonConvert.DeserializeObject<AppVersion>(request.Form["version"]);
                cResult = version == null ? new CommandResult(ResultStatus.Error, "没有获取到版本信息") : new CommandResult(ResultStatus.Success, "接收成功,准备升级...", version);
            }
            catch (Exception ex)
            {
                cResult = new CommandResult(ResultStatus.Error, ex.ToString());
            }

            return cResult;
        }
    }
}
