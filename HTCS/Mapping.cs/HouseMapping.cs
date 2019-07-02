using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class HouseMapping : BaseEntityTypeMap<HouseModel>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_HOURESOURCES");
            Property(m => m.Id).HasColumnName("ID");
            
            Property(m => m.BuildingNumber).HasColumnName("BUILDINGNUMBER");
            Property(m => m.Unit).HasColumnName("UNIT");
            Property(m => m.Province).HasColumnName("PROVINCE");
            Property(m => m.City).HasColumnName("CITY");
            Property(m => m.sign).HasColumnName("SIGN");
            Property(m => m.Adress).HasColumnName("ADRESS");
            Property(m => m.costprice).HasColumnName("COSTPRICE");
            Property(m => m.RecrntType).HasColumnName("RENTTYPE");
            Property(m => m.RoomId).HasColumnName("ROOMID");
            Property(m => m.Orientation).HasColumnName("ORIENTATION");
            Property(m => m.ProvinceName).HasColumnName("PROVINCENAME");
            Property(m => m.CityName).HasColumnName("CITYNAME");
            Property(m => m.AreamName).HasColumnName("AREANAME");
            Property(m => m.Status).HasColumnName("STATUS");
            Property(m => m.CreateTime).HasColumnName("CREATETIME");
            Property(m => m.CreatePerson).HasColumnName("CREATEPERSON");
            Property(m => m.DeleteTime).HasColumnName("DELETETIME");
            Property(m => m.DeletePerson).HasColumnName("DELETEPERSON");

            Property(m => m.CellName).HasColumnName("CELLNAME");
            Property(m => m.ShiNumber).HasColumnName("SHINUMBER");
            Property(m => m.TingNumber).HasColumnName("TINGNUMBER");
            Property(m => m.WeiNumber).HasColumnName("WEINUMBER");
            Property(m => m.NowFloor).HasColumnName("NOWFLOOR");
            Property(m => m.AllFloor).HasColumnName("ALLFLOOR");
            Property(m => m.PublicPeibei).HasColumnName("PUBLICPEIBEI");

            Property(m => m.PublicTeshe).HasColumnName("PUBLICTESHE");
            Property(m => m.PublicImg).HasColumnName("PUBLICIMG");
            Property(m => m.Fangguanyuan).HasColumnName("FANGGUANYUAN");
            Property(m => m.Price).HasColumnName("PRICE");
            Property(m => m.Measure).HasColumnName("MEASURE");
            Property(m => m.Area).HasColumnName("AREA");
            Property(m => m.BusinessArea).HasColumnName("BUSINESSAREA");
            Property(m => m.Renttime).HasColumnName("RENTTIME");
            Property(m => m.IsRm).HasColumnName("ISRM");
            Property(m => m.LocalId).HasColumnName("LOCALID");
            Property(m => m.Remarks).HasColumnName("REMARKS");
            Property(m => m.HouseKeeper).HasColumnName("HOUSEKEEPER");
            Property(m => m.Electricid).HasColumnName("ELECTRICID");

            Property(m => m.uuid).HasColumnName("UUID");
            Property(m => m.iscuizu).HasColumnName("ISCUIZU");

            Property(m => m.rentfree).HasColumnName("RENTFREE");

            Property(m => m.budget).HasColumnName("BUDGET");

            Property(m => m.profit).HasColumnName("PROFIT");

            Property(m => m.term).HasColumnName("TERM");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
            Property(m => m.storeid).HasColumnName("STOREID");

            Property(m => m.isyccontract).HasColumnName("ISYCCONTRACT");

            Property(m => m.latitude).HasColumnName("LATITUDE");

            Property(m => m.longitude).HasColumnName("LONGITUDE");

            //永昌
            //Property(m => m.yeara).HasColumnName("YEARA");

            //Property(m => m.yearb).HasColumnName("YEARB");

            //Property(m => m.yearc).HasColumnName("YEARC");

            //Property(m => m.yeard).HasColumnName("YEARD");

            //Property(m => m.yeare).HasColumnName("YEARE");

        }
    }
   
    public class T_V_HouseMapping : BaseEntityTypeMap<T_V_HouseModel>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_V_HOUSE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.CellName).HasColumnName("CELLNAME");
            Property(m => m.Renttype).HasColumnName("RENTTYPE");
            Property(m => m.RoomId).HasColumnName("ROOMID");
            Property(m => m.BuildingNumber).HasColumnName("BUILDINGNUMBER");
           
        }
    }
}
