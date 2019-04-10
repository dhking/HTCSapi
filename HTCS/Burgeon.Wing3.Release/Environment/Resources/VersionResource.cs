using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Burgeon.Wing3.Release.Environment.Commands;

namespace Burgeon.Wing3.Release.Environment
{
    /// <summary>
    /// Wing2.0 项目发布 版本文件承载类
    /// </summary>
    public abstract class VersionResource
    {
        public virtual ICommand SendCommandHandler
        {
            get
            {
                return new AwakenSendResourceCommand();
            }
        }

        public virtual ICommand ReceiveCommandHandler
        {
            get
            {
                return new AwakenReceiveResourceCommand();
            }
        }

        /// <summary>
        /// 发送当前资源文件到客户端
        /// </summary>
        /// <param name="client">客户端信息</param>
        public virtual void Send(Client client, Version version)
        {
            CommandContext context = new CommandContext();
            context["Client"] = client;
            context["Version"] = version;
            this.SendCommandHandler.Execute(context);
        }

        /// <summary>
        /// 客户端用于接受指定版本资源文件
        /// </summary>
        /// <param name="postfile"></param>
        public virtual void Received()
        {
            CommandContext context = new CommandContext();
            this.ReceiveCommandHandler.Execute(context);
        }
    }
}
