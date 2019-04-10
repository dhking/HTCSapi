using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public class WrapRenovation : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long houseid { get; set; }

        public string  HouseName { get; set; }

        public decimal budget { get; set; }

        public DateTime createtime { get; set; }

        public string createperson { get; set; }

        public List<TRenovationList> list { get; set; }
    }
    public  class Renovation : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long houseid { get; set; }

        public decimal budget { get; set; }


        public int term { get; set; }

        public DateTime createtime { get; set; }

        public string createperson { get; set; }
        [NotMapped]
        public List<TRenovationList> list { get; set; }

    }

    public class TRenovationList : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string process { get; set; }

        public decimal amount { get; set; }

        public DateTime time { get; set; }

        public int status { get; set; }

        public string reson { get; set; }

        public string bank { get; set; }

        public string detailedlist { get; set; }

        public string scene { get; set; }

        public long parentid { get; set; }

        public string createperson { get; set; }
    }
}
