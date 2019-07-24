using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WrapFinanceModel : BasicModel
    {
        public long Id { get; set; }

        public long HouseId { get; set; }
        public long HouseKeeper { get; set; }
        public long storeid { get; set; }
        public int  HouseType { get; set; }

        public string Trader { get; set; }

        public int Type { get; set; }

        public string CostName { get; set; }

        public decimal Amount { get; set; }

        public string PayType { get; set; }

        public DateTime TradingDate { get; set; }

        public string Remark { get; set; }
        public string Transaoctor { get; set; }

        public long TrandId { get; set; }

        public string HouseName { get; set; }

        public  string PayMentNumber { get; set; }

        public long CompanyId { get; set; }

        [NotMapped]
        public string CityName { get; set; }

        [NotMapped]
        public string AreaName { get; set; }

        [NotMapped]
        public string CellName { get; set; }
        [NotMapped]
        public string CellNames { get; set; }
        [NotMapped]
        public string[] arrCellNames { get; set; }
    }
    public  class FinanceModel : BasicModel
    {
        public long Id { get; set; }

        public long HouseId { get; set; }

        public string Trader { get; set; }

        public int Type { get; set; }
        public long CompanyId { get; set; }


        public string CostName { get; set; }

        public int source { get; set; }

        public decimal Amount { get; set; }

        public string PayType { get; set; }

        public DateTime TradingDate { get; set; }

        public string Remark { get; set; }

        [NotMapped]
        public string Content { get; set; }
        [NotMapped]
        public int HouseType { get; set; }

        [NotMapped]
        public string CityName { get; set; }

        [NotMapped]
        public string AreaName { get; set; }

        [NotMapped]
        public DateTime BeginTime { get; set; }


        [NotMapped]
        public DateTime EndTime { get; set; }

        [NotMapped]
        public string CellName { get; set; }

        [NotMapped]
        public string CellNames { get; set; }
        [NotMapped]
        public string[] arrCellNames { get; set; }

        public long TrandId { get; set; }
        public string PayMentNumber { get; set; }

        public string Transaoctor { get; set; }
    }
}
