using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Sandbox
{
    public class SandboxRunLog
    {
        private static string file = Utils.IOUtil.CombineDIR(Environment.ResourceEnvironment.Environment.ClientSandboxLogDIR, "log.log");

        private static string listener = Utils.IOUtil.CombineDIR(Environment.ResourceEnvironment.Environment.ClientSandboxLogDIR, "listener.log");

        public static string ErrorLogPath = Utils.IOUtil.CombineDIR(Environment.ResourceEnvironment.Environment.ClientSandboxLogDIR, "error.log");

        /// <summary>
        /// 写出启动升级沙箱程序日志
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            System.IO.File.WriteAllText(file, message);
        }

        /// <summary>
        /// 写出启动升级沙箱程序错误日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            System.IO.File.WriteAllText(ErrorLogPath, message);
        }

        /// <summary>
        /// 写出监听启动沙箱程序启动器异常日志
        /// </summary>
        /// <param name="message"></param>
        public static void ListenerErrorLog(string message)
        {
            System.IO.File.WriteAllText(listener, message);
        }
    }
}
