using Model.House;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class HouseQueryMapping : BaseEntityTypeMap<HouseQuery>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_V_QUERYHOUSE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.storeid).HasColumnName("STOREID");
            Property(m => m.HouseKeeper).HasColumnName("HOUSEKEEPER");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.RecentType).HasColumnName("RENTTYPE");
            Property(m => m.AreaName).HasColumnName("AREANAME");
            Property(m => m.City).HasColumnName("CITY");
            Property(m => m.Area).HasColumnName("AREA");
            Property(m => m.CellName).HasColumnName("CELLNAME");
            Property(m => m.Adress).HasColumnName("ADRESS");
            Property(m => m.CityName).HasColumnName("CITYNAME");
            Property(m => m.LocalId).HasColumnName("LOCALID");
            Property(m => m.Status).HasColumnName("STATUS");
            Property(m => m.electricid).HasColumnName("ELECTRICID");
            Property(m => m.uuid).HasColumnName("UUID");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
            Property(m => m.ParentRoomid).HasColumnName("PARENTROOMID");
         

        }
    }

    public class HouseQueryMapping1 : BaseEntityTypeMap<HouseQueryfgy>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_V_QUERYHOUSEFGY");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.HouseKeeper).HasColumnName("HOUSEKEEPER");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.RecentType).HasColumnName("RENTTYPE");
            Property(m => m.AreaName).HasColumnName("AREANAME");
            Property(m => m.City).HasColumnName("CITY");
            Property(m => m.Area).HasColumnName("AREA");
            Property(m => m.CellName).HasColumnName("CELLNAME");
            Property(m => m.Adress).HasColumnName("ADRESS");
            Property(m => m.CityName).HasColumnName("CITYNAME");
            Property(m => m.LocalId).HasColumnName("LOCALID");
            Property(m => m.Status).HasColumnName("STATUS");
            Property(m => m.electricid).HasColumnName("ELECTRICID");
            Property(m => m.uuid).HasColumnName("UUID");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
            Property(m => m.ParentRoomid).HasColumnName("PARENTROOMID");

        }
    }

    public class houresourcesMapping : BaseEntityTypeMap<houresources>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_V_HOURESOURCES");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.HouseType).HasColumnName("RENTTYPE");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.AreaCode).HasColumnName("AREA");
            Property(m => m.CellName).HasColumnName("CELLNAME");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class HouseQueryLockMapping : BaseEntityTypeMap<HouseLockQuery>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_V_QUERYHOUSELOCK");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.RecentType).HasColumnName("RENTTYPE");
            Property(m => m.AreaName).HasColumnName("AREANAME");
            Property(m => m.City).HasColumnName("CITY");
            Property(m => m.Area).HasColumnName("AREA");
            Property(m => m.CellName).HasColumnName("CELLNAME");
            Property(m => m.Adress).HasColumnName("ADRESS");
            Property(m => m.CityName).HasColumnName("CITYNAME");
            Property(m => m.LocalId).HasColumnName("LOCALID");
            Property(m => m.ElecId).HasColumnName("ELECTRICID");
            Property(m => m.ElecId).HasColumnName("ELECTRICID");
            Property(m => m.ParentId).HasColumnName("PARENTROOMID");
            Property(m => m.RoomName).HasColumnName("ROOMNAME");
            Property(m => m.Status).HasColumnName("STATUS");
            Property(m => m.iscuizu).HasColumnName("ISCUIZU");
            Property(m => m.UuId).HasColumnName("UUID");
            
            Property(m => m.Room).HasColumnName("ROOM");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
         
    }
    }
}
