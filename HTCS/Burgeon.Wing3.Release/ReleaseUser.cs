using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release
{
    [SqliteORM.Table]
    public class ReleaseUser
    {
        public ReleaseUser() { }

        public ReleaseUser(string name, string mobile, string password)
        {
            this.UserName = name;
            this.Mobile = mobile;
            this.Password = password;
        }

        [SqliteORM.PrimaryKey(AutoIncrement = true)]
        public long UserId { get; set; }

        [SqliteORM.Field]
        public string UserName { get; set; }

        [SqliteORM.Field]
        public string DynamicCode { get; set; }

        [SqliteORM.Field]
        public string Password { get; set; }

        [SqliteORM.Field]
        public string Mobile { get; set; }
    }
}
