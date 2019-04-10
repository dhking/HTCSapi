using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Models
{
    [SqliteORM.Table]
    public class Publishing
    {
        public Publishing()
        {
            this.Date = DateTime.Now.ToString();
        }

        [SqliteORM.PrimaryKey(AutoIncrement = true)]
        public long Id { get; set; }

        [SqliteORM.Field]
        public string CompanyId { get; set; }

        [SqliteORM.Field]
        public string CompanyName { get; set; }

        [SqliteORM.Field]
        public string Host { get; set; }

        [SqliteORM.Field]
        public string Version { get; set; }

        [SqliteORM.Field]
        public long Status { get; set; }

        [SqliteORM.Field]
        public long UserId { get; set; }

        [SqliteORM.Field]
        public string UserName { get; set; }

        [SqliteORM.Field]
        public string Date { get; set; }
    }
}
