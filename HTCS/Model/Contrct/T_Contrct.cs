using Model.House;
using Model.TENANT;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Contrct
{
    public   class T_Contrct: BasicModel
    {
        public long Id { get; set; }

        public long userid { get; set; }
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Type { get; set; }

        public int PinLv { get; set; }
        [NotMapped]
        public int issendmessage { get; set; }
        public int onlinesign { get; set; }
        public int FirstPay { get; set; }
        public int DepositType { get; set; }
        public decimal Recent { get; set; }

        public int PayType { get; set; }

        public decimal DayRecnet { get; set; }

        public decimal Deposit { get; set; }

        public string Remark { get; set; }

        public string Enclosure { get; set; }
        public DateTime CreateTime { get; set; }
        public string  CreatePerson { get; set; }
        public int Status { get; set; }
        public long HouseId { get; set; }
        public int  HouseType { get; set; }
        public long TeantId { get; set; }
        public int BeforeDay { get; set; }

        public int Recivetype { get; set; }

        public long CompanyId { get; set; }
        //已收
        public string  RecivedType { get; set; }

        public decimal RecivedAmount { get; set; }

        public string  RecivedAccount { get; set; }

        public string adress { get; set; }

        public string treatname { get; set; }

        public DateTime outtime { get; set; }
        [NotMapped]

        public decimal value { get; set; }

        [NotMapped]
        public List<T_Otherfee> Otherfee { get; set; }
        [NotMapped]
        public List<T_Otherfee> Yajin { get; set; }
        [NotMapped]
        public T_Teant Teant { get; set; }

        //收款人

        public string payee { get; set; }

        public string accounts { get; set; }

        public string bank { get; set; }

        public int eleccontract { get; set; }
    }
    public class WrapContract: BasicModel
    {
        public long Id { get; set; }
        public int eleccontract { get; set; }
        public long storeid { get; set; }
        public int onlinesign { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public int issign { get; set; }
     
        public int Status { get; set; }
        public int[] arrStatus { get; set; }
        [NotMapped]
        public string CellNames { get; set; }
        [NotMapped]
        public string[] arrCellNames { get; set; }
        public string CellName { get; set; }
        public string Document { get; set; }
        public string CellName1 { get; set; }
        public long BuildingNumber { get; set; }
        public int RoomId { get; set; }
        public string HouseName { get; set; }
        public decimal Deposit { get; set; }
        public decimal DayRecnet { get; set; }
        public decimal Recent { get; set; }
        public int Pinlv { get; set; }
        public long  HouseKeeper { get; set; }
        public int DepositType { get; set; }
        public string Phone { get; set; }
        public string CityName { get; set; }
        public string AreaName { get; set; }
        public string LockId { get; set; }
        public string Remark { get; set; }
        public long HouseId { get; set; }
        public long TeantId { get; set; }
        public int HouseType { get; set; }
        public int Recivetype { get; set; }
        public string Enclosure { get; set; }

        public string CreatePerson { get; set; }

        public DateTime CreateTime { get; set; }
        public int BeforeDay { get; set; }

        public string RecivedType { get; set; }

        public decimal RecivedAmount { get; set; }

        public string form { get; set; }

        public string RecivedAccount { get; set; }

        public int iskaimen { get; set; }


        public long CompanyId { get; set; }
        //收款人

        public string payee { get; set; }

        public string accounts { get; set; }

        public string bank { get; set; }
        //楼层
        public int floor { get; set; }
        //总高
        public int allfloor { get; set; }
        //面积
        public int mesure { get; set; }
        //
        //城市筛选条件
        [NotMapped]
        public int City { get; set; }
        //城市筛选条件
        [NotMapped]
        public int Area { get; set; }
        //搜索内容
        [NotMapped]
        public string  Content { get; set; }
        [NotMapped]
        public DateTime tBeginTime { get; set; }
        [NotMapped]
        public DateTime tEndTime { get; set; }
        //搜索类别
        [NotMapped]
        public int  Type { get; set; }
        [NotMapped]
        public List<T_Otherfee> Otherfee { get; set; }
        [NotMapped]
        public List<T_Otherfee> Yajin { get; set; }
        [NotMapped]
        public T_Teant Teant { get; set; }
        [NotMapped]
        public List<HouseLockQuery> HouseLock { get; set; }
    }
    //抄表模型
    public  class chaobiao:BasicModel
    {
        public long Id { get; set; }
        public long storeid { get; set; }
        public long teantid { get; set; }
        public int pinlv { get; set; }
        public string housename { get; set; }
        public long CompanyId { get; set; }
        public long houseid { get; set; }

        public string adress { get; set; }

        public string treatname { get; set; }

        public string project { get; set; }

        public string remark { get; set; }

        public DateTime lasttime { get; set; }

        public int lastdushu { get; set; }

        public int isyajin { get; set; }
        public int mobject { get; set; }

        public DateTime time { get; set; }



        public DateTime receivetime { get; set; }
        public int dushu { get; set; }

        public decimal price { get; set; }

        public int type { get; set; }

        //0不分摊1分摊
        public int isfentan { get; set; }

        public string name { get; set; }

        public DateTime begintime { get; set; }


        public DateTime endtime { get; set; }


        public int HouseType { get; set; }
    }
}
