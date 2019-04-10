using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 批量发布版本命令
    /// </summary>
    public class VersionPublishCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "version-publish";
            }
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult cResult = null;

            try
            {
                cResult = Publish(context);
            }
            catch (Exception ex)
            {
                cResult = new CommandResult(ResultStatus.Error, ex);
            }

            return cResult;
        }

        public CommandResult Publish(CommandContext context)
        {
            CommandResult cResult = null;

            //获取当前需要发布的所有客户资料
            List<Client> clients = context.GetParam<List<Client>>("companies");
            //获取当前需要发布的版本信息
            Models.AppVersion version = context.GetParam<Models.AppVersion>("version");

            if (clients == null || clients.Count <= 0)
            {
                cResult = new CommandResult(ResultStatus.InValid, "请至少选择一个客户进行版本发布");
            }
            else if (version == null || string.IsNullOrWhiteSpace(version.Version))
            {
                cResult = new CommandResult(ResultStatus.InValid, "请提供版本信息，注意版本号必须提供");
            }
            else
            {
                version.VersionRAR = Utils.IOUtil.CombineDIR(this.Environment.TopBuilVerionDIR, string.Format(version.Version) + ".rar");

                if (!Utils.IOUtil.ExistsFile(version.VersionRAR))
                {
                    cResult = new CommandResult(ResultStatus.InValid, string.Format("版本{0}不存在", version.VersionRAR));
                }
                else
                {
                    cResult = Publish(clients, version);
                }
            }

            return cResult;
        }

        private CommandResult Publish(List<Client> clients, Models.AppVersion version)
        {
            foreach (Client c in clients)
            {
                Publish(c, version);
            }

            return new CommandResult(ResultStatus.Success, "发布已启动");
        }

        private void Publish(Client client, Models.AppVersion version)
        {
            if (!PublishManager.Instance.IsPublishing(client))
            {
                Models.Publishing publishing = new Models.Publishing();
                publishing.Status = 0;
                publishing.CompanyId = client.Id.ToString();
                publishing.CompanyName = client.COMPANY_NAME;
                publishing.Host = client.COMPANY_WEB;
                publishing.Version = version.Version;
                publishing.UserId = int.Parse(this.User.UserId);
                publishing.UserName = this.User.UserName;
                SQLite.SQLiteORMAccessor.Delete<Models.VersionLog>(SqliteORM.Where.And(SqliteORM.Where.Equal("Version", version.Version), SqliteORM.Where.Equal("CompanyId", client.Id)));
                SQLite.SQLiteORMAccessor.RunSQL(string.Format("UPDATE VersionLog SET STATUS=200 WHERE CompanyId={0} AND STATUS=100", client.Id));
                PublishManager.Instance.CreateOrUpdatePubling(publishing);
                try
                {
                    AwakenSendResourceCommand command = new AwakenSendResourceCommand();
                    CommandContext context = new CommandContext();
                    context["Client"] = client;
                    context["Version"] = version;
                    command.Execute(context);
                }
                catch (Exception ex)
                {
                    SQLite.SQLiteORMAccessor.RunSQL(string.Format("UPDATE Publishing SET STATUS=100 WHERE CompanyId={0}", client.Id));
                    PublishManager.Instance.AddPublishLog(client.Id, 99, version.Version, ex.Message, ex.ToString(), this.User.UserName, int.Parse(this.User.UserId));
                }
            }
        }
    }
}
