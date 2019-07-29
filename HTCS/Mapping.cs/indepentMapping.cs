using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class indepentMapping : BaseEntityTypeMap<indepent>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_INDEPENDENT");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.name).HasColumnName("NAME");
            Property(m => m.ip).HasColumnName("IP");
            Property(m => m.image).HasColumnName("IMAGE");
        }
    }

    public class houresourcesupperMapping : BaseEntityTypeMap<houresourcesupper>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_HOURESOURCESUPPER");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.houseid).HasColumnName("HOUSEID");
            Property(m => m.gwisupper).HasColumnName("GWISUPPER");
            Property(m => m.housetype).HasColumnName("HOUSETYPE");
        }
    }
}
