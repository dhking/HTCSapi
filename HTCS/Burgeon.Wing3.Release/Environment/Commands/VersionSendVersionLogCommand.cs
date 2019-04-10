using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Http;
using Burgeon.Wing3.Release.Utils;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 发送版本升级日志命令
    /// </summary>
    public class VersionSendVersionLogCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "send-version-log";
            }
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult cResult = null;

            try
            {
                int cpy = context.GetParam<int>("cpy");
                string message = context.GetParam<string>("message");
                string mainHost = context.GetParam<string>("main");
                string version = context.GetParam<string>("version");
                string type = context.GetParam<string>("type");
                string userid = context.GetParam<string>("userid");

                if (string.IsNullOrWhiteSpace(mainHost))
                {
                    mainHost = "g.burgeon.cn";
                }
                if (string.IsNullOrWhiteSpace(message) || cpy <= 0)
                {
                    cResult = new CommandResult(ResultStatus.InValid, "参数不合法");
                }

                string uRL = this.Environment.GetCommandURL(mainHost, "receive-log");

                VersionRequest request = new VersionRequest(uRL, RequestType.POST);
                //发送版本信息
                request.SetPostParam("cpy", cpy.ToString());
                request.SetPostParam("msg", message);
                request.SetPostParam("type", type);
                request.SetPostParam("userid", userid);
                request.SetPostParam("version", version);
                request.SetPostParam("m", context.GetParam<string>("m"));

                VersionResponse response = request.Request();

                cResult = response.CommandResult;
                cResult.SubMessage = request.URL;

            }
            catch (Exception ex)
            {
                cResult = new CommandResult(ResultStatus.Error, ex);
            }

            return cResult;
        }
    }
}
