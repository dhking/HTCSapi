using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.StepLog
{
    /// <summary>
    /// Wing2.0 项目发布 日志接口
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message"></param>
        void LogWarn(string message);

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="message"></param>
        void LogDebug(string message);

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message"></param>
        void LogError(string message);

        /// <summary>
        /// 记录错误异常消息
        /// </summary>
        /// <param name="ex"></param>
        void LogError(Exception ex);
    }
}
