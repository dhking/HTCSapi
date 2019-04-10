using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public  class HouseTeantMapping : BaseEntityTypeMap<HouseTrant>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_V_HOUSETRENT");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.HouseName).HasColumnName("HOUSENAME");
            Property(m => m.TrantName).HasColumnName("TRANTNAME");
            Property(m => m.Phone).HasColumnName("PHONE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
