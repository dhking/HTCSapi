using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 发布结束 命令
    /// </summary>
    public class VersionPublishFinalCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "publish-final";
            }
        }

        public override bool Authrize()
        {
            return true;
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult cResult = null;

            List<string> companies = new List<string>();
            string ids = context.GetParam<string>("ids");
            companies.AddRange(ids.Split(','));

            string detail = context.GetParam<string>("detail");
            string message = context.GetParam<string>("message");
            string version = context.GetParam<string>("version");
            string userid = context.GetParam<string>("userid");

            if (companies != null)
            {
                int id = 0;
                foreach (string c in companies)
                {
                    int.TryParse(c, out id);
                    if (id > 0)
                    {
                        Final(id, message, detail, version, int.Parse(userid));
                    }
                }
            }

            cResult = new CommandResult(ResultStatus.Success, "版本发布已成功结束");
            return cResult;
        }

        private void Final(int company, string message, string detail, string version, long userid)
        {
            ReleaseUser user = ReleaseUserManager.Instance.GetUserById(userid);
            if (user != null)
            {
                SQLite.SQLiteORMAccessor.RunSQL(string.Format("UPDATE Publishing SET STATUS=100 WHERE CompanyId={0}", company));
                SQLite.SQLiteORMAccessor.CreateOrUpdate<Models.VersionLog>(CreateFinalLog(company, message, detail, version, user));
            }
        }

        private Models.VersionLog CreateFinalLog(long company, string message, string detail, string version, ReleaseUser user)
        {

            Models.VersionLog log = new Models.VersionLog();
            log.CompanyId = company;
            log.Type = 200;
            log.Detail = detail;
            log.Version = version;
            log.UserId = user.UserId;
            log.UserName = user.UserName;
            log.Message = message;
            return log;
        }
    }
}
