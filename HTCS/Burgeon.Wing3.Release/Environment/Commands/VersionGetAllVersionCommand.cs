using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Indecies;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 获取所有版本命令
    /// </summary>
    public class VersionGetAllVersionCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "all-versions";
            }
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult cResult = null;

            try
            {
                cResult = new CommandResult(ResultStatus.Success, "获取成功", VersionSeach.Create().GetVersions());
            }
            catch (Exception ex)
            {
                cResult = new CommandResult(ResultStatus.Error, ex);
            }

            return cResult;
        }
    }
}
