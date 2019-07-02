using Model;
using Model.Base;
using Model.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class MessageQueueMapping : BaseEntityTypeMap<T_MessageQueue>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_MESSAGEQUEUE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.phone).HasColumnName("PHONE");
            Property(m => m.message).HasColumnName("MESSAGE");
            Property(m => m.createtime).HasColumnName("CREATETIME");
            Property(m => m.times).HasColumnName("TIMES");
            Property(m => m.status).HasColumnName("STATUS");
            Property(m => m.begin).HasColumnName("BEGIN");
            Property(m => m.end).HasColumnName("END");
            Property(m => m.type).HasColumnName("TYPE");
            Property(m => m.contractid).HasColumnName("CONTRACTID");
            Property(m => m.companyid).HasColumnName("COMPANYID");
            Property(m => m.name).HasColumnName("NAME");
        }
    }
   
}
