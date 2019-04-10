using Model;
using Model.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class ShopMapping : BaseEntityTypeMap<T_Shop>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_SHOP");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }

    public class appstatisticsMapping : BaseEntityTypeMap<t_appstatistics>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_APPSTATISTICS");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Key).HasColumnName("KEY");
            Property(m => m.Value).HasColumnName("VALUE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
