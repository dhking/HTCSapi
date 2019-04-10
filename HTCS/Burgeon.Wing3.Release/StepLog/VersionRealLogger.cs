using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Burgeon.Wing3.Release.StepLog
{
    /// <summary>
    /// 升级实时日志 写入本地文件
    /// </summary>
    public class VersionRealLogger : ILogger
    {
        private int CompanyId = 0;
        private string Version = "";
        private string UserId = "";
        private string UserName = "";

        /// <summary>
        /// 创建一个实时IO 日志记录对象
        /// </summary>
        public VersionRealLogger(int company, string userid, string username, string version)
        {
            this.CompanyId = company;
            this.UserId = userid;
            this.Version = version;
            this.UserName = ReleaseUserManager.Instance.GetUserById(int.Parse(this.UserId)).UserName;
        }

        public void LogWarn(string message)
        {
            this.Log(2, message);
        }

        public void LogDebug(string message)
        {
            this.Log(1, message);
        }

        public void LogError(string message)
        {
            this.Log(99, message);
        }

        public void LogError(Exception ex)
        {
            this.Log(99, ex.Message, ex.ToString());
        }

        public void Log(int type, string message, string subMessage = "")
        {
            Models.VersionLog log = new Models.VersionLog();
            log.Type = type;
            log.Message = message;
            log.Detail = subMessage;
            log.CompanyId = CompanyId;
            log.UserId = int.Parse(UserId);
            log.UserName = UserName;
            log.Version = this.Version;
            SQLite.SQLiteORMAccessor.CreateOrUpdate<Models.VersionLog>(log);
        }
    }
}
