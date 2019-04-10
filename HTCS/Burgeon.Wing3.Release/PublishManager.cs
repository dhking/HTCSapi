using Burgeon.Wing3.Release.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release
{
    /// <summary>
    /// 用于获取正在发布的服务器列表
    /// </summary>
    public class PublishManager
    {
        private static PublishManager instance = null;

        private PublishManager()
        {

        }

        public static PublishManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PublishManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// 获取正在发布的客户列表
        /// </summary>
        /// <returns></returns>
        public List<Models.Publishing> GetPublishings()
        {
            try
            {
                ClearHistories();
                return SQLite.SQLiteORMAccessor.Select<Models.Publishing>().Where(m => m.Status == 0).ToList();
            }
            catch
            {
                return null;
            }
        }

        public void ClearHistories()
        {
            try
            {
                string date = DateTime.Now.AddDays(-5).ToString();
                SQLite.SQLiteORMAccessor.Delete<Models.VersionLog>(SqliteORM.Where.LessThan("Date", date));
                SQLite.SQLiteORMAccessor.Delete<Models.Publishing>(SqliteORM.Where.LessThan("Date", date));
            }
            finally
            {

            }
        }

        /// <summary>
        /// 添加一个发布项
        /// </summary>
        /// <param name="client"></param>
        public void CreateOrUpdatePubling(Models.Publishing publishing)
        {
            SQLite.SQLiteORMAccessor.CreateOrUpdate<Models.Publishing>(publishing);
        }

        public void DeletePublishing(Models.Publishing publishing)
        {
            SQLite.SQLiteORMAccessor.Delete<Models.Publishing>(publishing);
        }

        public void DeletePublishing(string company)
        {
            SQLite.SQLiteORMAccessor.Delete<Models.Publishing>(SqliteORM.Where.Equal("CompanyId", company));
        }

        public void AddPublishLog(int cpy, int type, string version, string message, string detail, string user, long userid)
        {
            Models.VersionLog log = new Models.VersionLog();
            log.Message = message;
            log.Type = type;
            log.Detail = detail;
            log.CompanyId = cpy;
            log.UserId = userid;
            log.UserName = user;
            log.Version = version;
            SQLite.SQLiteORMAccessor.CreateOrUpdate<Models.VersionLog>(log);
        }

        /// <summary>
        /// 获取一个值指示当前客户是否正在升级中
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool IsPublishing(Client client)
        {
            return SQLite.SQLiteORMAccessor.Select<Models.Publishing>().Where(m => m.CompanyId == client.Id.ToString() && m.Status == 0).Count() > 0;
        }
    }
}
