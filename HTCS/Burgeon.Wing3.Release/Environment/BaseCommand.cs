using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.StepLog;

namespace Burgeon.Wing3.Release.Environment
{
    /// <summary>
    /// Wing.20 项目发布  命令对象基类
    /// </summary>
    public class BaseCommand : ICommand
    {
        private readonly ReleaseUserContext user = new ReleaseUserContext();

        /// <summary>
        /// 获取当前版本发布程序 环境对象
        /// </summary>
        public ResourceEnvironment Environment
        {
            get
            {
                return ResourceEnvironment.Environment;
            }
        }

        public ReleaseUserContext User
        {
            get
            {
                return user;
            }
        }

        /// <summary>
        ///获取当前命令名称
        /// </summary>
        public virtual string Command
        {
            get { return "base"; }
        }

        private ILogger _logger;

        /// <summary>
        /// 获取回话日志对象
        /// </summary>
        public virtual ILogger Logger
        {
            get
            {
                return _logger;
            }
        }

        /// <summary>
        /// 版本资源文件校验器
        /// </summary>
        public virtual Security.ISecurity ResourceSecurity
        {
            get
            {
                return new Security.VersionResourceSecurity();
            }
        }

        public BaseCommand()
        {
            _logger = new VersionSessionLogger();
        }

        public virtual bool Authrize()
        {
            return user.Validate();
        }

        public virtual CommandResult Execute(CommandContext context)
        {
            return new CommandResult(0, "执行完毕");
        }
    }
}
