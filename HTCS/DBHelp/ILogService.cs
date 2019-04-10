using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp
{
    public interface ILogService
    {

        ILog Logger();

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <param name="message"></param>
        void logException(Exception e, string message = "");

        void logInfo(string message);

        void LogWarn(string message);

        void LogError(string message);
    }
}
