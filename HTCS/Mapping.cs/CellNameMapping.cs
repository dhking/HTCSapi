using Model.House;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class CellNameMapping : BaseEntityTypeMap<T_CellName>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_CELLNAME");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.Adress).HasColumnName("ADRESS");
            Property(m => m.Area).HasColumnName("AREA");
            Property(m => m.City).HasColumnName("CITY");
            Property(m => m.AreaName).HasColumnName("AREANAME");
            Property(m => m.CityName).HasColumnName("CITYNAME");

            Property(m => m.CompanyId).HasColumnName("COMPANYID");
            Property(m => m.regtype).HasColumnName("REGTYPE");
            Property(m => m.parentid).HasColumnName("PARENTID");
            Property(m => m.code).HasColumnName("CODE");
        }
    }
}
