using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public   class HouseRz : BasicModel
    {
        public long Id { get; set; }
        public DateTime createtime { get; set; }
        public long createperson { get; set; }
        public long houseid { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        [NotMapped]
        public string house { get; set; }

        [NotMapped]
        public DateTime BeginTime { get; set; }

        [NotMapped]
        public DateTime EndTime { get; set; }

        public long companyid { get; set; }
    }
    public class WrapHouseRz : BasicModel
    {
        public long Id { get; set; }
        public DateTime createtime { get; set; }
        public long createperson { get; set; }
        public string create{ get; set; }
        public long houseid { get; set; }
        public string House { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public long companyid { get; set; }
    }
}
