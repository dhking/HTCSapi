using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.StepLog
{
    /// <summary>
    /// 内存日志记录类
    /// </summary>
    public class MemoryCacheLogger : ILogger
    {
        private StringBuilder logWarns = null;

        private StringBuilder logDebugs = null;

        private StringBuilder logErrors = null;

        public MemoryCacheLogger()
        {
            logWarns = new StringBuilder();
            logDebugs = new StringBuilder();
            logErrors = new StringBuilder();
        }

        public void LogWarn(string message)
        {
            logWarns.Append(message);
        }

        public void LogDebug(string message)
        {
            logDebugs.Append(message);
        }

        public void LogError(string message)
        {
            logErrors.Append(message);
        }

        public void LogError(Exception ex)
        {
            logErrors.Append(ex.ToString());
        }

        private string LogFormat(string message)
        {
            return string.Format("{0}:\r\n{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message);
        }
    }
}
