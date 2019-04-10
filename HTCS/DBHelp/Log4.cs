using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp
{
    public class LogService :ILogService
    {
        public ILog logger;
        private bool isConfigured = false;

        public LogService()
        {
            if (!isConfigured)
            {
                log4net.Config.XmlConfigurator.Configure();
                logger = LogManager.GetLogger(typeof(LogService));

            }
        }

        public ILog Logger()
        {
            return logger;
        }

        public void logInfo(string message)
        {
            logger.Info(message);
        }



        public void LogWarn(string message)
        {
            logger.Warn(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void logException(Exception e, string message = "")
        {

            StringBuilder preamble = new StringBuilder();
            preamble.Append(message);
            preamble.Append("\r\n 异常:");
            preamble.Append(e.Message);

            preamble.Append("\r\n");

            StackTrace st = new StackTrace(true);
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                MethodBase mb = sf.GetMethod();
                if (mb != null && mb.ReflectedType != null)
                {
                    string mname = mb.ReflectedType.FullName;
                    preamble.Append("at method:");
                    preamble.Append(mname);
                    preamble.Append("::");
                    preamble.Append(sf.GetMethod().Name);
                    preamble.Append(" @");
                    preamble.Append(sf.GetFileLineNumber());
                    preamble.Append(" +file:");
                    preamble.Append(sf.GetFileName());
                    preamble.Append("\r\n");
                }
            }

            preamble.Append("\r\n");

            logger.Fatal(preamble, e);
        }

    }
}
