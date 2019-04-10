using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
   
    public class HouseReportMapping : BaseEntityTypeMap<HouseReport>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_HOUSEREPORT");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.vacant).HasColumnName("VACANT");
            Property(m => m.profit).HasColumnName("PROFIT");
            Property(m => m.updatetime).HasColumnName("UPDATETIME");
            Property(m => m.cellname).HasColumnName("CELLNAME");
            Property(m => m.recenttype).HasColumnName("RECENTTYPE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }

    public class HouseReportListMapping : BaseEntityTypeMap<HouseReportList>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_T_HOUSEREPORTLIST");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.housereportid).HasColumnName("HOUSEREPORTID");
            Property(m => m.vacant).HasColumnName("VACANT");
            Property(m => m.profit).HasColumnName("PROFIT");
            Property(m => m.updatetime).HasColumnName("UPDATETIME");
            Property(m => m.houseid).HasColumnName("HOUSEID");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
