using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.House
{

    public class houresources : BasicModel
    {
        public string Name { get; set; }
        public long AreaCode { get; set; }
        public long Id { get; set; }
        public long storeid { get; set; }
        public int HouseType { get; set; }
        public long CompanyId { get; set; }
        public string CellName { get; set; }
        public long HouseKeeper { get; set; }
        public string AreaName { get; set; }
        public string CityName { get; set; }
        public string Adress { get; set; }
    }

    public class HouseQueryfgy : BasicModel
    {
        public long Id { get; set; }

        public long HouseKeeper { get; set; }

      
        public string Name { get; set; }

        public int RecentType { get; set; }

        public string AreaName { get; set; }

        public string CityName { get; set; }

        public int City { get; set; }

        public int Area { get; set; }

        public string Adress { get; set; }

        public string CellName { get; set; }

        public string LocalId { get; set; }

        public int Status { get; set; }

        public long CompanyId { get; set; }

        public string electricid { get; set; }

        public long ParentRoomid { get; set; }

        public string uuid { get; set; }
    }
    public class wrapdistribution
    {
        public List<long> checkdata { get; set; }

        public List<distributionHouseQuery> data { get; set; }

    }
    public class distributionHouseQuery : BasicModel
    {
        public long id { get; set; }
        public long   HouseKeeper { get; set; }
        public long storeid { get; set; }
        public string  storename { get; set; }
        public string HouseKeeperName { get; set; }
        public string Name { get; set; }
        public int RecentType { get; set; }
        public string AreaName { get; set; }
        public string CityName { get; set; } 
        public string Adress { get; set; }
        public string CellName { get; set; }
        public long CompanyId { get; set; }
        [NotMapped]
        public bool LAY_CHECKED { get; set; }
    }
    public   class HouseQuery : BasicModel
    {
        public long Id { get; set; }
        public long storeid { get; set; }
        public long HouseKeeper { get; set; }
        public string Name { get; set; }

        public int RecentType { get; set; }
        
        public string AreaName { get; set; }

        public string CityName { get; set; }
        public int City { get; set; }
        public int Area { get; set; }

        public string Adress { get; set; }

        public string CellName { get; set; }

        public string LocalId { get; set; }

        public int Status { get; set; }

        public long CompanyId { get; set; }

        public string electricid { get; set; }

        public long ParentRoomid { get; set; }
        
        public string uuid { get; set; }

        public int nowfloor { get; set; }
        public int allfloor { get; set; }

        public int measure { get; set; }

        public int isyccontract { get; set; }
    }
    public class HouseLockQuery : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }

        public int RecentType { get; set; }

        public string AreaName { get; set; }

        public int iscuizu { get; set; }
        public string RoomName { get; set; }

        public string CityName { get; set; }
        public int City { get; set; }
        public int Area { get; set; }

        public int Status { get; set; }
        public string Adress { get; set; }

        public string CellName { get; set; }

        public string LocalId { get; set; }

        public string ElecId { get; set; }


        public string UuId { get; set; }

        public string Room { get; set; }

        public long ParentId { get; set; }
    }
}
