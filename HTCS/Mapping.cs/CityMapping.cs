using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class CityMapping : BaseEntityTypeMap<City>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_OMSREGION");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.RegionName).HasColumnName("REGIONNAME");
            Property(m => m.RegType).HasColumnName("REGTYPE");
            Property(m => m.IsRemen).HasColumnName("ISREMEN");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
