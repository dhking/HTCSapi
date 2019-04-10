using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Indecies;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    class VersionSearchResourceCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "search";
            }
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult cResult = null;

            try
            {
                string path = context.GetParam<string>("path");
                string filter = context.GetParam<string>("filter");
                cResult = new CommandResult(ResultStatus.Success, "搜索成功", VersionSeach.Create().SeachResource(path, filter));
            }
            catch (Exception ex)
            {
                cResult = new CommandResult(ResultStatus.Error, ex);
            }

            return cResult;
        }
    }
}
