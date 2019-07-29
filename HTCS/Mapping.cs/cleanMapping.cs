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
    public   class cleanMapping : BaseEntityTypeMap<clean>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_CLEANING");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.houseid).HasColumnName("HOUSEID");
          

            Property(m => m.status).HasColumnName("STATUS");
            Property(m => m.ugent).HasColumnName("UGENT");
            Property(m => m.apply).HasColumnName("APPLY");
            Property(m => m.appointment).HasColumnName("APPOINTMENT");
            Property(m => m.applyperson).HasColumnName("APPLYPERSON");
            Property(m => m.phone).HasColumnName("PHONE");

            Property(m => m.expectedtime).HasColumnName("EXPECTEDTIME");

            Property(m => m.project).HasColumnName("PROJECT");
            Property(m => m.remark).HasColumnName("REMARK");
            Property(m => m.executor).HasColumnName("EXECUTOR");
            Property(m => m.companyid).HasColumnName("COMPANYID");
        }
    }
    public class cleanRZMapping : BaseEntityTypeMap<cleanRZ>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_CLEANINGRZ");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.createtime).HasColumnName("CREATETIME");
            Property(m => m.cleanid).HasColumnName("CLEANID");

            Property(m => m.content).HasColumnName("CONTENT");
            Property(m => m.opera).HasColumnName("OPERA");
            Property(m => m.companyid).HasColumnName("COMPANYID");

          
        }
    }
}
