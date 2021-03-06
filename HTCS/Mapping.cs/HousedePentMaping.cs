﻿using Model;
using Model.House;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public  class HousedePentMaping:BaseEntityTypeMap<HousePendent>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.ID);

            Property(m => m.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_HOURESOURCES_PENDENT");
            Property(m => m.ID).HasColumnName("ID");
            Property(m => m.HouseKeeper).HasColumnName("HOUSEKEEPER");
            Property(m => m.storeid).HasColumnName("STOREID");
            Property(m => m.Price).HasColumnName("PRICE");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Huxing).HasColumnName("HUXING");
            Property(m => m.Orientation).HasColumnName("ORIENTATION");
            Property(m => m.Measure).HasColumnName("MEASURE");
            Property(m => m.costprice).HasColumnName("COSTPRICE");
            Property(m => m.ParentRoomid).HasColumnName("PARENTROOMID");
            Property(m => m.Remarks).HasColumnName("REMARKS");
            Property(m => m.PrivateImage).HasColumnName("PEIVATEIMAGE");
            Property(m => m.isyccontract).HasColumnName("ISYCCONTRACT");
            Property(m => m.PrivatePeibei).HasColumnName("PEIBEI");
            Property(m => m.PrivateTeshe).HasColumnName("TESHE");

            Property(m => m.Status).HasColumnName("STATUS");
            Property(m => m.RecentTime).HasColumnName("RECENTTIME");
            Property(m => m.IsRm).HasColumnName("ISRM");
            Property(m => m.LoclId).HasColumnName("LOCALID");
            Property(m => m.FloorId).HasColumnName("FLOORID");
            Property(m => m.Electricid).HasColumnName("ELECTRICID");
            Property(m => m.uuid).HasColumnName("UUID");
            Property(m => m.iscuizu).HasColumnName("ISCUIZU");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
            Property(m => m.sign).HasColumnName("SIGN");

            Property(m => m.shi).HasColumnName("SHI");
            Property(m => m.ting).HasColumnName("TING");
            Property(m => m.wei).HasColumnName("WEI");
            //永昌
            //Property(m => m.yeara).HasColumnName("YEARA");

            //Property(m => m.yearb).HasColumnName("YEARB");

            //Property(m => m.yearc).HasColumnName("YEARC");

            //Property(m => m.yeard).HasColumnName("YEARD");

            //Property(m => m.yeare).HasColumnName("YEARE");

        }
    }

    public class FloorMaping : BaseEntityTypeMap<T_Floor>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_FLOOR");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Floor).HasColumnName("FLOOR");
            Property(m => m.ParentId).HasColumnName("PARENTID");
           
        }
    }
    public class FxingMaping : BaseEntityTypeMap<Fxing>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_FXING");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.shi).HasColumnName("SHI");
            Property(m => m.ting).HasColumnName("TING");
            Property(m => m.wei).HasColumnName("WEI");
            Property(m => m.image).HasColumnName("IMAGE");
            Property(m => m.measure).HasColumnName("MEASURE");
            Property(m => m.houseid).HasColumnName("HOUSEID");
            Property(m => m.name).HasColumnName("NAME");
        }
    }

}
