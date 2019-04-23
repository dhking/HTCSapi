using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class SignVersion
    {
        public string filename { get; set; }
        public string no { get; set; }
        public string name { get; set; }
        public string id_card_no { get; set; }
        public string moblie { get; set; }

        public string user_code { get; set; }
        public string sign_type { get; set; }
        public long  ContractId { get; set; }


        public string certificate { get; set; }
        public string address { get; set; }
        public string contact { get; set; }


        public string mobile { get; set; }
        public string signature_type { get; set; }

    }
    public class Signresult
    {
        public string code { get; set; }

        public string msg { get; set; }
    }
}
