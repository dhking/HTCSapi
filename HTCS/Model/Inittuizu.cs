using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class tuzuReq
    {
        public long contractid { get; set; }

        public DateTime tuizutime { get; set; }

        public int tktype { get; set; }
    }
    public class TuizuZhu : BasicModel
    {
        public long Id { get; set; }
        
        public string result { get; set; }

        public long CompanyId { get; set; }
        public string Account { get; set; }

        public string subbranch { get; set; }
        public DateTime TuifangTime { get; set; }

        public int IsKongzhi { get; set; }

        public decimal Amount { get; set; }

        public string BankName { get; set; }
       
        public string AmountName { get; set; }

        public DateTime fukuanTime { get; set; }

        public int TkType { get; set; }

      

        public string PayType { get; set; }

        public string BanLi { get; set; }

        public string Bank { get; set; }

        public string Pingzheng { get; set; }

        public long ContractId { get; set; }

        public int FukuanType { get; set; }


        public int Status { get; set; }
        public int  Type{ get; set; }


        public DateTime  time { get; set; }
        [NotMapped]
        public List<Tuizu> list { get; set; }
        [NotMapped]
        public List<Tuizu> test { get; set; }
        [NotMapped]
        public List<peipei> listpeibei { get; set; }
    }
    public  class Inittuizu:BasicModel
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }

        public decimal Amount { get; set; }

        public int Type { get; set; }

        public string  TkType { get; set; }

        public decimal Price { get; set; }

        public int IsPayBill { get; set; }
        
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Explain { get; set; }

     
        public string Name { get; set; }
        
        public string Code { get; set; }
        [NotMapped]
        public DateTime Tuizutime { get; set; }
    }
    public class Tuizu : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public decimal Amount { get; set; }

        public decimal Price { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Type { get; set; }


        public int Status { get; set; }

        public string Explain { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
        
        public long zhuId { get; set; }
    }
}
