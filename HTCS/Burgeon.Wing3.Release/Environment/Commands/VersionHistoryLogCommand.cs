using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 获取指定公司指定版本升级历史日志
    /// </summary>
    public class VersionHistoryLogCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "version-log-history";
            }
        }

        public override CommandResult Execute(CommandContext context)
        {
            int company = context.GetParam<int>("company");
            string version = context.GetParam<string>("version");

            List<Models.VersionLog> histories = SQLite.SQLiteORMAccessor.Select<Models.VersionLog>()
                .Where(SqliteORM.Where.And(SqliteORM.Where.Equal("Version", version), SqliteORM.Where.Equal("CompanyId", company), SqliteORM.Where.LessThan("Status", "200")))
                .OrderBy<Models.VersionLog, long>(m => m.Id).ToList();

            return new CommandResult(ResultStatus.Success, "获取成功", histories);
        }
    }
}
