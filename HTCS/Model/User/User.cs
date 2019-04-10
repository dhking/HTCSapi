using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.User
{
    public class UserChecker
    {
        public string appkey { get; set; }
        public string appsecret { get; set; }
    }
    public class ZKUser : BasicModel
    {
        public long Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
      
    }
    public class BanBen : BasicModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public decimal banben { get; set; }
        public string Cont { get; set; }
        public string Url { get; set; }

    }
    public class feedback : BasicModel
    {
        public long id { get; set; }

        public int Type { get; set; }
        public long CompanyId { get; set; }
        public string content { get; set; }

        public long userid { get; set; }

        public string phone { get; set; }


        public DateTime createtime { get; set; }
    }
}
