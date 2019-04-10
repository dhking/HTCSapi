using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public  class T_ZafeiListMapping: BaseEntityTypeMap<T_ZafeiList>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_OTHERFEELIST");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.Code).HasColumnName("CODE");
            Property(m => m.IsYajin).HasColumnName("ISYAJIN");
            Property(m => m.TuiType).HasColumnName("TUITYPE");
            Property(m => m.IsActive).HasColumnName("ISACTIVE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
