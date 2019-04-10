using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public  class SysMessageMapping : BaseEntityTypeMap<T_SysMessage>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_SYSMESSAGE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.content).HasColumnName("CONTENT");
            Property(m => m.createperson).HasColumnName("CREATEPERSON");
            Property(m => m.createtime).HasColumnName("CREATETIME");
            Property(m => m.url).HasColumnName("URL");
            Property(m => m.userid).HasColumnName("USERID");
            Property(m => m.title).HasColumnName("TITLE");
            Property(m => m.type).HasColumnName("TYPE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
