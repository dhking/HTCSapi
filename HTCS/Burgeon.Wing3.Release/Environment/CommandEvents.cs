using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment
{
    public delegate void BeginExecuteCommand(object sender, BeginExecuteCommandArgs args);

    public delegate void EndExecuteCommand(object sender, EndExecuteCommandArgs args);

    public delegate void ErrorExecuteCommand(object sender, ErrorExecuteCommandArgs args);

    /// <summary>
    /// 命令开始执行事件参数
    /// </summary>
    public class BeginExecuteCommandArgs
    {
        public BeginExecuteCommandArgs(CommandContext context)
        {
            this.Context = context;
        }

        public CommandContext Context { get; private set; }
    }

    /// <summary>
    /// 命令执行完毕事件参数
    /// </summary>
    public class EndExecuteCommandArgs
    {
        public EndExecuteCommandArgs(CommandContext context, CommandResult cResult)
        {
            this.Context = context;
            this.Result = cResult;
        }

        public CommandContext Context { get; private set; }

        public CommandResult Result { get; private set; }
    }

    public class ErrorExecuteCommandArgs
    {
        public ErrorExecuteCommandArgs(Exception ex, CommandContext context)
        {
            this.Error = ex;
            this.Context = context;
        }

        public CommandContext Context { get; private set; }

        public Exception Error { get; private set; }
    }
}
