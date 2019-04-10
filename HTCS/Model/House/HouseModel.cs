using Model.Base;
using Model.House;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   
    public class HouseTip:PagingModel
    {
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public int HouseType { get; set; }
        public long HouseId { get; set; }
        public int Status { get; set; }
        public string CityName { get; set; }

        public string AreaName { get; set; }

        public int AreaCode { get; set; }
        public long? TeantId { get; set; }

        public string UserName { get; set; }
        public string Adress { get; set; }
        public string UserPhone { get; set; }

        public int iscuizu { get; set; }
    }
    public class ParaTip : PagingModel
    {
        public string Name { get; set; }
        public int Type { get; set; }
    }
   
    public class Excel
    {
        public string search { get; set; }
        public string filename { get; set; }
        public string access_token { get; set; }
    }
    public partial class HouseModel : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
    
        public long storeid { get; set; }
   
        public string Adress { get; set; }
        public long BuildingNumber { get; set; }
        public int Unit { get; set; }
        public int Province { get; set; }
        public int City { get; set; }
        public int sign { get; set; }
        public int RecrntType { get; set; }
        public string RoomId { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string AreamName { get; set; }
        
        public string CellName { get; set; }
       
        public int Status { get; set; }
        //0未租1已出租
        
        public DateTime CreateTime { get; set; }
        public string CreatePerson { get; set; }
        public DateTime DeleteTime { get; set; }
        public string DeletePerson { get; set; }
       
       
        public int ShiNumber { get; set; }
        public int TingNumber { get; set; }

        public int WeiNumber { get; set; }
        public int NowFloor { get; set; }
        public int AllFloor { get; set; }
        public string Orientation { get; set; }
        public string PublicPeibei { get; set; }
        [JsonProperty("PublicTeshe")]
        public string PublicTeshe { get; set; }

        public string PublicImg { get; set; }
        public string Fangguanyuan { get; set; }
        public decimal Price { get; set; }
        public int Area { get; set; }
        public int Measure { get; set; }
        public string BusinessArea { get; set; }
        public DateTime Renttime { get; set; }
        public int IsRm { get; set; }

        public int iscuizu { get; set; }
        
        public string LocalId { get; set; }

        public string Remarks { get; set; }


        public string Electricid { get; set; }

        public string uuid { get; set; }
        [NotMapped]
        public long  housecount { get; set; }
        [NotMapped]
        public string  store { get; set; }
        public decimal budget { get; set; }

        public decimal costprice { get; set; }
        public int term { get; set; }

        public decimal profit { get; set; }

        [NotMapped]
        public string Content { get; set; }
        [NotMapped]
        public long floorcount { get; set; }
        [NotMapped]
        public string Huxing { get; set; }
        [NotMapped]
        public List<peipei> listpeibei { get; set; }
        [NotMapped]
        public List<T_Tese> listtese { get; set; }
        [NotMapped]
        public List<T_Floor> floor { get; set; }
        [NotMapped]
        [DefaultValue(true)]
        public bool GroupBy { get; set; }

        [NotMapped]
        public int Group { get; set; }

        public long HouseKeeper { get; set; }
        [NotMapped]
        public string  housekeepername { get; set; }
        [NotMapped]
        public string housekeeperphone { get; set; }
        [NotMapped]
        public long Idletime { get; set; }

        public int rentfree { get; set; }

        
    }
    public class T_V_HouseModel:BasicModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CellName { get; set; }
        public long BuildingNumber { get; set; }
        public int RoomId { get; set; }
        public int Renttype { get; set; }
      
    }

    public class peipei : BasicModel
    {
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public long Id { get; set; }
        public int Type { get; set; }
        public long HouseId { get; set; }
        //是否选中 0未选中 1选中
        [NotMapped]
        public int isCheck { get; set; }
    }
    public class Wrapdudong
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<T_Floor> floors { get; set; }
    }
    public  class T_Floor : BasicModel
    {
        public long Id { get; set; }

        public int Floor { get; set; }

        public long ParentId { get; set; }
        [NotMapped]

        public string tongji { get; set; }
        [NotMapped]
        public int housecount { get; set; }

        [NotMapped]

        public List<HousePendent> list { get; set; }
    }


    

}
