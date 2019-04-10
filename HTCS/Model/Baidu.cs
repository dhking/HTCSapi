using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class Baidu
    {
        public int expires_in { get; set; }

        public string access_token { get; set; }
    }
    public class Idcard
    {
        public long UserId { get; set; }
        public string id_card_side { get; set; }

        public string image { get; set; }

        public string fimage { get; set; }
        public string detect_risk { get; set; }

        public string RealName { get; set; }

        public string Mobile { get; set; }
    }
    public class returnidcard
    {
        public string error_code { get; set; }

        public string error_msg { get; set; }

        public string log_id { get; set; }

        public string direction { get; set; }

        public string image_status { get; set; }

        public string idcard_type { get; set; }

        public string imagepath { get; set; }

        public words_result words_result { get; set; }
    }
    public class words_result
    {
        public baiduwrap 住址 { get; set; }

        public baiduwrap 公民身份号码 { get; set; }

        public baiduwrap 出生 { get; set; }

        public baiduwrap 姓名 { get; set; }

        public baiduwrap 民族 { get; set; }

        public baiduwrap 失效日期 { get; set; }

        public baiduwrap 性别 { get; set; }
    }
    public class baiduwrap
    {
        public string words { get; set; }
    }  
}
