using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WrapFinanceModel : BasicModel
    {
        public long Id { get; set; }

        public long HouseId { get; set; }

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
    }
    public  class FinanceModel : BasicModel
    {
        public long Id { get; set; }

        public long HouseId { get; set; }

        public string Trader { get; set; }

        public int Type { get; set; }
        public long CompanyId { get; set; }


        public string CostName { get; set; }

        public decimal Amount { get; set; }

        public string PayType { get; set; }

        public DateTime TradingDate { get; set; }

        public string Remark { get; set; }


        public long TrandId { get; set; }
        public string PayMentNumber { get; set; }

        public string Transaoctor { get; set; }
    }
}
