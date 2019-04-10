using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment
{
    /// <summary>
    /// Wing2.0 项目发布 执行命令接口
    /// </summary>
    public interface ICommand
    {
       
         /// <summary>
        /// 执行当前命令
        /// </summary>
        /// <param name="context">发布命令上下文</param>
        /// <returns>命令执行结果 CommandResult</returns>
        CommandResult Execute(CommandContext context);
    }
}
