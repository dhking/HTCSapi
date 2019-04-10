using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Model.Bill
{
    
    public  class T_BillList: BasicModel
    {
        public long    Id { get; set; }
        public long    BillId { get; set; }
        public string  BillType { get; set; }
        public int BillStage { get; set; }
        [DefaultValue(0)]
        public decimal Amount { get; set; }
        public decimal ReciveAmount { get; set; }
        public decimal RecivedAmount { get; set; }
        public string Explain { get; set; }
        public string BillCode { get; set; }
        public long CompanyId { get; set; }
    }
}
