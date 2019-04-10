using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Burgeon.Wing3.Release
{
    /// <summary>
    /// 发布用户管理
    /// </summary>
    public class ReleaseUserManager
    {
        private static ReleaseUserManager container = null;

        private List<ReleaseUser> Users = null;

        private Dictionary<string, ReleaseUser> DUsers = null;

        private ReleaseUserManager()
        {
            DUsers = new Dictionary<string, ReleaseUser>(StringComparer.InvariantCultureIgnoreCase);
            Users = new List<ReleaseUser>();
            this.InitUsers();
        }

        public IEnumerable<ReleaseUser> AllUsers
        {
            get
            {
                return Users.AsEnumerable();
            }
        }

        public static ReleaseUserManager Instance
        {
            get
            {
                if (container == null)
                {
                    container = new ReleaseUserManager();
                }
                return container;
            }
        }

        public ReleaseUser GetUserById(long id)
        {
            if (DUsers.ContainsKey(id.ToString()))
            {
                return DUsers[id.ToString()];
            }
            else
            {
                return null;
            }
        }

        public void UpdateLogin(long id, string code)
        {
            ReleaseUser user = this.GetUserById(id);
            if (user != null)
            {
                user.DynamicCode = code;
                SQLite.SQLiteORMAccessor.CreateOrUpdate<ReleaseUser>(user);
            }
        }

        private void InitUsers()
        {
            try
            {
                this.Users = SQLite.SQLiteORMAccessor.Select<ReleaseUser>().ToList();

            }
            catch (Exception ex)
            {

            }
            if (this.Users == null || this.Users.Count <= 0)
            {
                string password = Utils.EncryptUtil.Md5("Burgeon123");
                SQLite.SQLiteORMAccessor.CreateOrUpdate(new ReleaseUser("boris", "18621801935",password));
                SQLite.SQLiteORMAccessor.CreateOrUpdate(new ReleaseUser("王波", "13262637926", password));
                SQLite.SQLiteORMAccessor.CreateOrUpdate(new ReleaseUser("周碧文", "13524849487", password));
                this.Users = SQLite.SQLiteORMAccessor.Select<ReleaseUser>().ToList();
            }

            this.DUsers = this.Users.ToDictionary<ReleaseUser, string>(m => m.UserId.ToString());
        }
    }
}
