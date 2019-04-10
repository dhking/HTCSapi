using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 移除指定版本文件 命令
    /// </summary>
    public class VersionRemoveVersionCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "delete-version";
            }
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult cResult = null;

            string version = context.GetParam<string>("version");

            string path = Utils.IOUtil.CombineDIR(this.Environment.TopBuilVerionDIR, string.Format("{0}.rar", version));

            if (!Utils.IOUtil.ExistsFile(path))
            {
                cResult = new CommandResult(ResultStatus.InValid, string.Format("不存在版本{0}", version));
            }
            else
            {
                Utils.IOUtil.Delete(path);
                cResult = new CommandResult(ResultStatus.Success, "删除成功");
            }

            return cResult;
        }
    }
}
