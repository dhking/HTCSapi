using SqliteORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 版本发布进度监听命令
    /// </summary>
    public class VersionProcessPublishCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "publish-process";
            }
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult cResult = null;

            try
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                List<Models.VersionLog> logs = SQLite.SQLiteORMAccessor.Select<Models.VersionLog>()
                    .Where(Where.And(Where.Equal("UserId", this.User.UserId), Where.Equal("Status", 0)))
                    .OrderBy(m => m.Id).ToList();

                if (logs.Count > 0)
                {
                    long id = logs.Max<Models.VersionLog>(m => m.Id);
                    SQLite.SQLiteORMAccessor.RunSQL(string.Format("UPDATE VersionLog SET STATUS=100 WHERE ID<={0}", id));
                }


                cResult = new CommandResult(ResultStatus.Success, "获取成功", logs);
            }
            catch (Exception ex)
            {
                cResult = new CommandResult(ResultStatus.Error, ex);
            }

            return cResult;
        }
    }
}
