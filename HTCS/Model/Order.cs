using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class wrapOrder : BasicModel
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public double minamount { get; set; }
        public double maxamount { get; set; }
        public string Content { get; set; }
        public int ispt { get; set; }
        public double Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public long UserId { get; set; }
        public long HouseId { get; set; }
        public string message { get; set; }
        public string Pone { get; set; }
        public string zkName { get; set; }
        public string House { get; set; }
        public int Status { get; set; }
        [NotMapped]
        public string openid { get; set; }
        public int value { get; set; }

        public double ServiceCharge { get; set; }
        public double realamount { get; set; }
        public int isaddacountt { get; set; }

        public decimal Price { get; set; }

        public string liushui { get; set; }

        public string zftype { get; set; }

        public long CompanyId { get; set; }
        [NotMapped]
        public List<T_OrderList> orderlist { get; set; }
    }
    public class Order : BasicModel
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public int ispt { get; set; }
        public double Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public long UserId { get; set; }
        public long HouseId { get; set; }
        public string message { get; set; }

        public string House { get; set; }
        public int Status { get; set; }
        [NotMapped]
        public string openid { get; set; }
        public int value { get; set; }

        public int isaddacountt { get; set; }

        public decimal Price { get; set; }

        public string liushui { get; set; }

        public string zftype { get; set; }

        public long CompanyId { get; set; }
        [NotMapped]
        public List<T_OrderList> orderlist { get; set; }
    }
    public class T_OrderList : BasicModel
    {
        public long Id { get; set; }

        public long orderid { get; set; }

        public decimal amount { get; set; }

        public long billid { get; set; }

        public string name { get; set; }

        public long CompanyId { get; set; }
    }
}
