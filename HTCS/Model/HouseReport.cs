using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class HouseReport : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long vacant { get; set; }

        public long profit { get; set; }

        public DateTime updatetime { get; set; }

        public string cellname { get; set; }

        public int recenttype { get; set; }
        [NotMapped]
        public string[] cellnames { get; set; }
        [NotMapped]
        public string[] citynames { get; set; }
    }
    public class HouseReportList : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        [NotMapped]
        public long storeid { get; set; }
        public long housereportid { get; set; }

        public long vacant { get; set; }

        public decimal profit { get; set; }

        public DateTime updatetime { get; set; }

        public long houseid { get; set; }

    }
    public class WrapHouseReportList : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
    
        public long storeid { get; set; }

        public long housereportid { get; set; }

        public long vacant { get; set; }

        public decimal profit { get; set; }

        public DateTime updatetime { get; set; }

        public long houseid { get; set; }

        public string HouseName { get; set; }

    }
}
