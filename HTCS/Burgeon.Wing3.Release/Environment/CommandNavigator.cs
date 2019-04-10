using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment
{
    /// <summary>
    /// 命令导航工具
    /// </summary>
    public class CommandNavigator
    {
        static CommandNavigator()
        {
            Configure();
        }

        private Dictionary<string, BaseCommand> _commands = null;

        private static CommandNavigator navigator = null;

        private CommandNavigator()
        {
            _commands = new Dictionary<string, BaseCommand>();
        }

        /// <summary>
        /// 开始执行命令事件
        /// </summary>
        public event BeginExecuteCommand BeginExecute;

        /// <summary>
        /// 命令执行完毕 事件
        /// </summary>
        public event EndExecuteCommand EndExecute;

        /// <summary>
        /// 命令执行出现错误 事件
        /// </summary>
        public event ErrorExecuteCommand ExecuteError;

        /// <summary>
        /// 默认命令导航实例
        /// </summary>
        public static CommandNavigator Default
        {
            get
            {
                if (navigator == null)
                {
                    navigator = new CommandNavigator();
                }
                return navigator;
            }
        }

        /// <summary>
        /// 指定指定命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual CommandResult Execute(string command, CommandContext context)
        {
            CommandResult cResult = null;

            BaseCommand cmd = this.GetCommand(command);
            if (cmd == null)
            {
                cResult = new CommandResult(ResultStatus.Error, string.Format("不存在当前处理操作:{0}", command));
            }
            try
            {
                this.OnBeginExecuteCommand(context);

                if (cmd.Authrize())
                {
                    cResult = cmd.Execute(context);
                }
                else
                {
                    cResult = new CommandResult(ResultStatus.NoAurthrize, "您没有权限操作此命令");
                }

                this.OnEndExecuteCommand(cResult, context);
            }
            catch (Exception ex)
            {
                cResult = new CommandResult(ResultStatus.Error, ex);
                this.OnErrorExecuteCommand(context, ex);
            }

            return cResult;
        }

        protected virtual void OnBeginExecuteCommand(CommandContext context)
        {
            if (this.BeginExecute != null)
            {
                this.BeginExecute(this, new BeginExecuteCommandArgs(context));
            }
        }

        protected virtual void OnEndExecuteCommand(CommandResult result, CommandContext context)
        {
            if (this.EndExecute != null)
            {
                this.EndExecute(this, new EndExecuteCommandArgs(context, result));
            }
        }

        protected virtual void OnErrorExecuteCommand(CommandContext context, Exception ex)
        {
            if (this.ExecuteError != null)
            {
                this.ExecuteError(this, new ErrorExecuteCommandArgs(ex, context));
            }
        }

        /// <summary>
        /// 添加一个命令组件 如果存在相同命令名称的组件
        /// 则不会进行替换或者添加
        /// </summary>
        /// <param name="cmd"></param>
        public void AddCommand(BaseCommand cmd)
        {
            if (!this.Contains(cmd.Command))
            {
                this._commands.Add(cmd.Command, cmd);
            }
        }

        /// <summary>
        /// 移除指定命令的命令组件
        /// </summary>
        /// <param name="cmd"></param>
        public void RemoveCommand(string cmd)
        {
            if (this.Contains(cmd))
            {
                this._commands.Remove(cmd);
            }
        }

        /// <summary>
        /// 是否包含指定名称的命令组件
        /// </summary>
        /// <param name="cmd"></param>
        public bool Contains(string cmd)
        {
            return this._commands.ContainsKey(cmd);
        }

        /// <summary>
        /// 根据指定命令获取对应的命令组件实例
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private BaseCommand GetCommand(string cmd)
        {
            if (this.Contains(cmd))
            {
                return this._commands[cmd];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 注册当前程序集中相关命令
        /// </summary>
        public static void Configure()
        {
            bool isHost = Environment.ResourceEnvironment.Environment.IsHost;

            if (isHost)
            {
                Default.AddCommand(new Commands.AwakenReceiveResourceCommand());
                Default.AddCommand(new Commands.AwakenSendResourceCommand());
                Default.AddCommand(new Commands.VersionGetAllVersionCommand());
                Default.AddCommand(new Commands.VersionSearchResourceCommand());
                Default.AddCommand(new Commands.VersionBuildCommand());
                Default.AddCommand(new Commands.VersionUpgradeCommand());
                Default.AddCommand(new Commands.VersionProcessPublishCommand());
                Default.AddCommand(new Commands.VersionPublishCommand());
                Default.AddCommand(new Commands.VersionLogCommand());
                Default.AddCommand(new Commands.VersionRemoveVersionCommand());
                Default.AddCommand(new Commands.VersionPublishFinalCommand());
                Default.AddCommand(new Commands.VersionHistoryLogCommand());
            }
            else
            {
                Default.AddCommand(new Commands.AwakenReceiveResourceCommand());
                Default.AddCommand(new Commands.AwakenSendResourceCommand());
                Default.AddCommand(new Commands.VersionBuildCommand());
                Default.AddCommand(new Commands.VersionUpgradeCommand());
                Default.AddCommand(new Commands.VersionSendVersionLogCommand());
                Default.AddCommand(new Commands.VersionSendPublishFinalCommand());
            }
        }
    }
}
