using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Bill
{
    public  class T_WrapBill:BasicModel
    {
        [NotMapped]
        public string CellNames { get; set; }
        [NotMapped]
        public string[] arrCellNames { get; set; }
        public int HouseType { get; set; }
        public int islock { get; set; }
        public int Object { get; set; }
        public long Id { get; set; }
        public long HouseKeeper { get; set; }
        public string CityName { get; set; }
        public string AreaName { get; set; }
        public long storeid { get; set; }
        public decimal Amount { get; set; }
        [NotMapped]
        public string  Phone { get; set; }
        public long TeantId { get; set; }
        public string  TeantName { get; set; }
        public string RecentName { get; set; }
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
        public string  House{ get; set; }
        public long CompanyId { get; set; }
        public string sign { get; set; }
        public DateTime? PayTime { get; set; }
        public string  PayType { get; set; }
        public string CreatePerson { get; set; }
        public string TranSactor { get; set; }
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }
        public string Voucher { get; set; }
        public int BillType { get; set; }
        public string Liushui { get; set; }
        public long ContractId { get; set; }
        public int stage { get; set; }
        public long HouseId { get; set; }
        //0正序排序1反序排序
        [NotMapped]
        public int? OrderbyTime { get; set; }
        //0待处理1已处理2已逾期
        [NotMapped]
        public int Status { get; set; }
        //城市筛选条件
        [NotMapped]
        public int? City { get; set; }
        //城市筛选条件
        [NotMapped]
        public int? Area { get; set; }

        [NotMapped]
        public string Content { get; set; }
        public int PayStatus { get; set; }
        public DateTime? ShouldReceive { get; set; }
        public string CellName { get; set; }
        public long? BuildingNumber { get; set; }
        public int? RoomId { get; set; }
        public string HouseName { get; set; }
        public long  Day { get; set; }
        
        public List<T_BillList> list { get; set; }
        public int type { get; set; }
        //收款人
        public string payee { get; set; }

        public string accounts { get; set; }

        public string bank { get; set; }

        public string subbranch { get; set; }

      
    }
}
