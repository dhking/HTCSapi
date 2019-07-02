using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Bill
{
    public  class T_Bill: BasicModel
    {
        public long Id { get; set; }
        public long TeantId { get; set; }
        public int stage { get; set; }
        public int? islock { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime  EndTime { get; set; }
        public long HouseId { get; set; }
        public int? HouseType{ get; set; }
        public DateTime? PayTime { get; set; }
        public string PayType { get; set; }
        public string  CreatePerson { get; set; }
        public string  TranSactor { get; set; }
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }
        public string Voucher { get; set; }
        public string sign { get; set; }
        public long ContractId { get; set; }
        public int PayStatus { get; set; }

        public int Object{ get; set; }
        
        public DateTime ShouldReceive { get; set; }
        public decimal Amount { get; set; }
        public string Explain { get; set; }
        public string Liushui { get; set; }

        public int  BillType { get; set; }
        //收款人
        public string payee { get; set; }

        public string accounts { get; set; }

        public string bank { get; set; }

        public string subbranch { get; set; }

        public int type { get; set; }
        [NotMapped]
        public int Status { get; set; }
        [NotMapped]
        public List<T_BillList> list { get; set; }
        [NotMapped]
        public  List<T_BillList> deletebilllist { get; set; }

        public long CompanyId { get; set; }

        public string name { get; set; }
    }
    public class MessageReq
    {
        public string Temp { get; set; }
        public string Phone { get; set; }
        public string SignName { get; set; }
        public string TemplateCode { get; set; }
        public long CompanyId { get; set; }
        public int Type { get; set; }
    }
    public class yzRequest
    {
        public long CompanyId { get; set; }
        public string Phone { get; set; }
        public string Temp { get; set; }
        public string SignName { get; set; }
        public string name { get; set; }
        public string TemplateCode { get; set; }
        public int Type { get; set; }

        public string yzm { get; set; }
    }
}
