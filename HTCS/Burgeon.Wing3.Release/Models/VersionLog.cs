using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Models
{
    /// <summary>
    /// 版本发布日志实体类
    /// </summary>
    [SqliteORM.Table]
    public class VersionLog
    {
        public VersionLog()
        {
            this.Date = DateTime.Now.ToString();
            this.Status = 0;
        }

        [SqliteORM.PrimaryKey(AutoIncrement = true)]
        public long Id { get; set; }

        [SqliteORM.Field]
        public string Message { get; set; }

        [SqliteORM.Field]
        public string Detail { get; set; }

        [SqliteORM.Field]
        public long Type { get; set; }

        [SqliteORM.Field]
        public long CompanyId { get; set; }

        [SqliteORM.Field]
        public long UserId { get; set; }

        [SqliteORM.Field]
        public string UserName { get; set; }

        [SqliteORM.Field]
        public long Status { get; set; }

        [SqliteORM.Field]
        public string Date { get; set; }

        [SqliteORM.Field]
        public string Version { get; set; }
    }
}
