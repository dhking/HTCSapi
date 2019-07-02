using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.House
{
    public  class HousePendent:BasicModel
    {
        public long ID { get; set; }
        public int sign { get; set; }
        public long storeid { get; set; }
        public long HouseKeeper { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Huxing { get; set; }
        public decimal Measure { get; set; }
        public string Orientation { get; set; }
        public long CompanyId { get; set; }
        public decimal costprice { get; set; }
        public string Remarks { get; set; }
        public long ParentRoomid { get; set; }
        public string PrivateTeshe { get; set; }
        public string PrivatePeibei { get; set; }
        public string PrivateImage { get;set;}
        [NotMapped]
        public string PublicImage { get; set; }
        public int Status { get; set; }
        public DateTime RecentTime { get; set; }
        public int IsRm { get; set; }

        public int isyccontract { get; set; }
        public string LoclId { get; set; }

        public long FloorId { get; set; }

        public string Electricid { get; set; }

        [NotMapped]
        public string housekeepername { get; set; }
        [NotMapped]
        public string housekeeperphone { get; set; }

        [NotMapped]
        public string store { get; set; }
        public string uuid { get; set; }

        

        public int iscuizu { get; set; }

        [NotMapped]
        public long Idletime { get; set; }
        [NotMapped]
        public int isbind { get; set; }
        [NotMapped]
        public string Adress { get; set; }

        [NotMapped]
        public string Cellname { get; set; }
        [NotMapped]
        public List<peipei> listpeibei { get; set; }
        [NotMapped]
        public List<T_Tese> listtese { get; set; }
        
        public int shi { get; set; }
        public int ting { get; set; }
        public int wei { get; set; }
        //永昌
        //public decimal yeara { get; set; }
        //public decimal yearb { get; set; }
        //public decimal yearc { get; set; }
        //public decimal yeard { get; set; }
        //public decimal yeare { get; set; }
    }
   
    public class WrapHousePendent : BasicModel
    {

        public long ID { get; set; }
        public long  storeid { get; set; }
        public int sign { get; set; }
        public int constatus { get; set; }
        public long HouseKeeper { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }


        public string teantname { get; set; }
        public decimal recent { get; set; }
        public DateTime? endtime { get; set; }

        public string Huxing { get; set; }
        public decimal Measure { get; set; }
        public string Orientation { get; set; }
        public long CompanyId { get; set; }
        public decimal costprice { get; set; }
        public string Remarks { get; set; }
        public long ParentRoomid { get; set; }
        public string PrivateTeshe { get; set; }
        public string PrivatePeibei { get; set; }
        public string PrivateImage { get; set; }


        [NotMapped]
        public string PublicImage { get; set; }
        public int Status { get; set; }
        public DateTime RecentTime { get; set; }
        public int IsRm { get; set; }

        public int isyccontract { get; set; }
        public string LoclId { get; set; }
        public long FloorId { get; set; }
        public string Electricid { get; set; }

        [NotMapped]
        public string housekeepername { get; set; }
        [NotMapped]
        public string housekeeperphone { get; set; }
        public string uuid { get; set; }



        public int iscuizu { get; set; }

        [NotMapped]
        public long Idletime { get; set; }
       
        [NotMapped]
        public string Adress { get; set; }

        [NotMapped]
        public string Cellname { get; set; }

        public long sort { get; set; }

        public int shi { get; set; }
        public int ting { get; set; }
        public int wei { get; set; }
    }
    public class Floorco
    {
        public int Floorcount { get; set; }

        public long FloorId { get; set; }
    }

    public class Fxing : BasicModel
    {
        public long  Id { get; set; }

        public int shi { get; set; }

        public int ting { get; set; }

        public int wei { get; set; }

        public string image { get; set; }

        public string name { get; set; }

        public int measure { get; set; }

        public long houseid { get; set; }
    }

    public class wrapHousePendent
    {
        public List<HousePendent> HousePendent { get; set; }

        public housesstatic hstatic { get; set; }
    }
    public class housesstatic
    {
        public long count { get; set; }

        public long Vacantcount { get; set; }


        public long rentedcount { get; set; }
    }
    
}
