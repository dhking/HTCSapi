using Model;
using Model.House;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class txMapping : BaseEntityTypeMap<T_tx>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_TX");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.amount).HasColumnName("AMOUNT");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.Account).HasColumnName("ACCOUNT");
            Property(m => m.RealName).HasColumnName("REALNAME");
            Property(m => m.createtime).HasColumnName("CREATETIME");
            Property(m => m.createperson).HasColumnName("CREATEPERSON");
            Property(m => m.userid).HasColumnName("USERID");
            Property(m => m.liushui).HasColumnName("LIUSHUI");
            Property(m => m.status).HasColumnName("STATUS");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
  
}
