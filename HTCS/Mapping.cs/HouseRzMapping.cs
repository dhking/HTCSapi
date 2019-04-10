using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
  
    public class HouseRzMapping:BaseEntityTypeMap<HouseRz>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_HOUSERZ");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.createtime).HasColumnName("CREATETIME");
            Property(m => m.createperson).HasColumnName("CREATEPERSON");
            Property(m => m.houseid).HasColumnName("HOUSEID");
            Property(m => m.type).HasColumnName("TYPE");
            Property(m => m.content).HasColumnName("CONTENT");
            Property(m => m.companyid).HasColumnName("COMPANYID");
        }
    }
}
