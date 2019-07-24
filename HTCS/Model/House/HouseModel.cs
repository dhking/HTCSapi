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
    /// <summary>
    /// 具有经纬度
    /// </summary>
    public interface IHasLngAndLat
    {
        /// <summary>
        /// 经度
        /// </summary>
        double Lng { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        double Lat { get; set; }
    }
    /// <summary>
    /// 带距离的数据
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DataWithDistance<TEntity>
    {
        /// <summary>
        /// 距离（km）
        /// </summary>
        public double distance { get; set; }
        /// <summary>
        /// 实体数据
        /// </summary>
        public TEntity Entity { get; set; }
    }
    public class HouseTip:PagingModel
    {
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public int HouseType { get; set; }
        public long HouseId { get; set; }
        public int Status { get; set; }
        public string CityName { get; set; }

        public string AreaName { get; set; }
        public long storeid { get; set; }

        public long HouseKeeper { get; set; }
        
        public int AreaCode { get; set; }
        public long? TeantId { get; set; }

        public int isyccontract { get; set; }
        
        public string UserName { get; set; }
        public string Adress { get; set; }
        public string UserPhone { get; set; }

        public int iscuizu { get; set; }
    }
    public class ParaTip : PagingModel
    {
        public string Name { get; set; }
        public  long CompanyId { get; set; }
        public int Type { get; set; }
    }
   
    public class Excel
    {
        public string search { get; set; }
        public string filename { get; set; }

        public long Id { get; set; }
        public string access_token { get; set; }
    }

    public class WrapCellName
    {
        public string CellName { get; set; }

        public int housecount { get; set; }

        public List<WrapHouseModel1> listhouse { get; set; }
    }
    public partial class WrapHouseModel1 : BasicModel
    {
        public string teantname { get; set; }
        public decimal recent { get; set; }
        public DateTime? endtime { get; set; }


        public long Id { get; set; }

        public double  distince { get; set; }
        public long CompanyId { get; set; }
   
        public long storeid { get; set; }

        public string Title { get; set; }
        public string Adress { get; set; }
        public string BuildingNumber { get; set; }
        public int Unit { get; set; }
        public int Province { get; set; }
        public int City { get; set; }
        public int sign { get; set; }
        public int constatus { get; set; }
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
        public decimal Measure { get; set; }
        public string BusinessArea { get; set; }
        public DateTime Renttime { get; set; }
        public int IsRm { get; set; }

        public int iscuizu { get; set; }

        public string LocalId { get; set; }

        public string Remarks { get; set; }


        public string Electricid { get; set; }

        public string uuid { get; set; }
        [NotMapped]
        public long housecount { get; set; }
        [NotMapped]
        public string store { get; set; }
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
        public string housekeepername { get; set; }
        [NotMapped]
        public string housekeeperphone { get; set; }
        [NotMapped]
        public long Idletime { get; set; }

        public int rentfree { get; set; }

        public int isyccontract { get; set; }

        public double Lng{ get; set; }

        public double Lat { get; set; }

    }
    public partial class HouseModel : BasicModel
    {
        public long Id { get; set; }
        [NotMapped]
        public double radius { get; set; }
        [NotMapped]
        public string storename { get; set; }

    
        public long CompanyId { get; set; }
    
        public long storeid { get; set; }

        [NotMapped]
        public string location { get; set; }

        public string Adress { get; set; }
        public string BuildingNumber { get; set; }
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
        [NotMapped]
        //0所有1有图片2
        public int ishaveimg { get; set; }
        public string PublicImg { get; set; }
        public string Fangguanyuan { get; set; }
        public decimal Price { get; set; }
        public int Area { get; set; }
        public decimal Measure { get; set; }
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
        public List<Fxing> FangXing { get; set; }
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

        public int isyccontract { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }
        [NotMapped]
        public string CellNames { get; set; }
        [NotMapped]
        public string[] arrCellNames { get; set; }
        //永昌
        //public decimal yeara { get; set; }
        //public decimal yearb { get; set; }
        //public decimal yearc { get; set; }
        //public decimal yeard { get; set; }
        //public decimal yeare { get; set; }
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
        public long HouseKeeper { get; set; }
        [NotMapped]
        public string tongji { get; set; }
        [NotMapped]
        public int housecount { get; set; }

        [NotMapped]

        public List<WrapHousePendent> list { get; set; }
    }


    

}
