using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 记录版本日志命令
    /// </summary>
    public class VersionLogCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "receive-log";
            }
        }

        public override bool Authrize()
        {
            return true;
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult cResult = null;

            try
            {
                int cpy = context.GetParam<int>("cpy");
                string userid = context.GetParam<string>("userid");
                string multi = context.GetParam<string>("m");
                string message = context.GetParam<string>("msg");
                string version = context.GetParam<string>("version");
                string type = context.GetParam<string>("type");

                if (cpy > 0 && !string.IsNullOrWhiteSpace(message))
                {
                    this.LogMessages(message, multi, cpy, type, userid, version);
                    cResult = new CommandResult(ResultStatus.Success, "记录成功");
                }
                else
                {
                    cResult = new CommandResult(ResultStatus.Error, "参数不合法");
                }
            }
            catch (Exception ex)
            {
                cResult = new CommandResult(ResultStatus.Error, ex);
            }

            return cResult;
        }

        private void LogMessages(string message, string m, int cpy, string type, string userid, string version)
        {
            userid = string.IsNullOrWhiteSpace(userid) ? this.User.UserId : userid;
            StepLog.VersionRealLogger log = new StepLog.VersionRealLogger(cpy, userid, this.User.UserName, version);

            if (m == "multi")
            {
                string[] strs = message.Split('\n');
                foreach (string s in strs)
                {
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        this.LogMessage(s, type, userid, log);
                    }
                }
            }
            else
            {
                this.LogMessage(message, type, userid, log);
            }
        }

        private void LogMessage(string message, string type, string userid, StepLog.VersionRealLogger log)
        {
            if (type == "99")
            {
                log.LogError(message);
            }
            else if (type == "100")
            {
                log.Log(100, message);
            }
            else
            {
                log.LogDebug(message);
            }
        }
    }
}
