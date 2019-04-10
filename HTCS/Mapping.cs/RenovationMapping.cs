using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class RenovationMapping : BaseEntityTypeMap<Renovation>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_RENOVATION");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.houseid).HasColumnName("HOUSEID");
            Property(m => m.budget).HasColumnName("BUDGET");
            Property(m => m.term).HasColumnName("TERM");
            Property(m => m.createtime).HasColumnName("CREATETIME");
            Property(m => m.createperson).HasColumnName("CREATEPERSON");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");


        }
    }


    public class renovationlistMapping : BaseEntityTypeMap<TRenovationList>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_RENOVATIONLIST");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.process).HasColumnName("PROCESS");
            Property(m => m.amount).HasColumnName("AMOUNT");
            Property(m => m.time).HasColumnName("TIME");
            Property(m => m.status).HasColumnName("STATUS");
            Property(m => m.reson).HasColumnName("RESON");
            Property(m => m.scene).HasColumnName("SCENE");
            Property(m => m.parentid).HasColumnName("PARENTID");
            Property(m => m.createperson).HasColumnName("CREATEPERSON");
            Property(m => m.bank).HasColumnName("BANK");
            Property(m => m.detailedlist).HasColumnName("DETAILEDLIST");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }

       
    }
}
