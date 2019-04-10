using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp
{
    /// <summary>
    /// 日记类型
    /// </summary>
    public enum UtilLogType
    {
        /// <summary>
        /// 
        /// </summary>
        Info,
        /// <summary>
        /// 
        /// </summary>
        Warn,
        /// <summary>
        /// 
        /// </summary>
        Error
    }
    /// <summary>
    /// 日记操作类
    /// </summary>
    public class LogUtil
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly static log4net.ILog logErr = log4net.LogManager.GetLogger("logger.Error");
        /// <summary>
        /// 
        /// </summary>
        private readonly static log4net.ILog logInfo = log4net.LogManager.GetLogger("logger.Info");
        /// <summary>
        /// 
        /// </summary>
        private readonly static log4net.ILog logWarn = log4net.LogManager.GetLogger("logger.Warn");

        /// <summary>
        /// 
        /// </summary>
        static LogUtil()
        {
            String baseDir = AppDomain.CurrentDomain.BaseDirectory;
            String filePath = "";
            if (!String.IsNullOrWhiteSpace(baseDir) && baseDir.LastIndexOf('\\') == baseDir.Length - 1)
            {
                filePath = baseDir + "log4net.config";
            }
            else
            {
                filePath = baseDir + @"\log4net.config";
            }
            FileInfo fileInfo = new FileInfo(filePath);
            XmlConfigurator.ConfigureAndWatch(fileInfo);
        }

        /// <summary>
        /// 输出Log到Log文件中
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        private static void Write(String msg, UtilLogType logType)
        {
            switch (logType)
            {
                case UtilLogType.Info:
                    logInfo.Info(msg);
                    break;
                case UtilLogType.Warn:
                    logWarn.Warn(msg);
                    break;
                case UtilLogType.Error:
                    logErr.Error(msg);
                    break;
                default:
                    logInfo.Info(msg);
                    break;
            }
        }
        /// <summary>
        /// 输出Log到Log文件中
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        /// <param name="ex"></param>
        private static void Write(String msg, UtilLogType logType, Exception ex)
        {
            switch (logType)
            {
                case UtilLogType.Info:
                    logInfo.Info(msg, ex);
                    break;
                case UtilLogType.Warn:
                    logWarn.Warn(msg, ex);
                    break;
                case UtilLogType.Error:
                    logErr.Error(msg, ex);
                    break;
                default:
                    logInfo.Info(msg, ex);
                    break;
            }
        }

        /// <summary>
        /// 输出Log信息，属于Info日记
        /// </summary>
        /// <param name="msg">Log信息内容</param>
        public static void WriteLog(String msg)
        {
            Write(msg, UtilLogType.Info);
        }
        /// <summary>
        /// 输出Log信息,带异常参数，属于Info日记
        /// </summary>
        /// <param name="msg">Log信息内容</param>
        /// <param name="ex">异常</param>
        public static void WriteLog(String msg, Exception ex)
        {
            Write(msg, UtilLogType.Error, ex);
        }
        /// <summary>
        /// 输出Log信息，属于Info日记
        /// </summary>
        /// <param name="msg">Log信息内容</param>
        /// <param name="logType">Log类型</param>
        public static void WriteLog(String msg, UtilLogType logType)
        {
            Write(msg, logType);
        }
        /// <summary>
        /// 输出Log信息,带异常参数，属于Info日记
        /// </summary>
        /// <param name="msg">Log信息内容</param>
        /// <param name="logType">Log类型</param>
        /// <param name="ex">异常</param>
        public static void WriteLog(String msg, UtilLogType logType, Exception ex)
        {
            Write(msg, logType, ex);
        }

    }
}
