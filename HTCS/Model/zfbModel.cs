using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{


    public class accountbank : BasicModel
    {
        public long Id { get; set; }

        public int type { get; set; }
        [NotMapped]
        public string yzm { get; set; }
        [NotMapped]
       public string Mobile { get; set; }
        public string accountname { get; set; }

        public string account { get; set; }

        public string bank { get; set; }

        public string zhbank { get; set; }

        public long CompanyId { get; set; }

    }
    public class T_PayMentAcount:BasicModel
    {
        public long Id { get; set; }
        public string app_id { get; set; }

        public string public_key { get; set; }

        public string private_key { get; set; }

        public string public_key_zf { get; set; }

        public long CompanyId { get; set; }

        public string reflash_token { get; set; }

        public string redirect_uri { get; set; }
        public int Type { get; set; }
    }
    public class zfbzz
    {
        public string out_biz_no { get; set; }

        public string payee_type { get; set; }

        public string payee_account { get; set; }

        public string amount { get; set; }

        public string payer_show_name { get; set; }

        public string payee_real_name { get; set; }

        public string remark { get; set; }

    }

   
}
