using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Http;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 客户端设置发布完毕 命令
    /// </summary>
    public class VersionSendPublishFinalCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "send-publish-final";
            }
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult cResult = null;

            try
            {
                int cpy = context.GetParam<int>("company");
                string mainHost = context.GetParam<string>("host");
                string userid = context.GetParam<string>("userid");
                string message = context.GetParam<string>("message");
                string detail = context.GetParam<string>("detail");
                if (string.IsNullOrWhiteSpace(mainHost))
                {
                    mainHost = "g.burgeon.cn";
                }

                string uRL = this.Environment.GetCommandURL(mainHost, "publish-final");
                VersionRequest request = new VersionRequest(uRL, RequestType.POST);
                //发送版本信息
                request.SetPostParam("ids", cpy.ToString());
                request.SetPostParam("message", message);
                request.SetPostParam("detail", detail);
                request.SetPostParam("userid", userid);

                VersionResponse response = request.Request();

                cResult = response.CommandResult;

            }
            catch (Exception ex)
            {
                cResult = new CommandResult(ResultStatus.Error, ex);
            }

            return cResult;
        }
    }
}
