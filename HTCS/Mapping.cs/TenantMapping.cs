using Model;
using Model.TENANT;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class TenantMapping : BaseEntityTypeMap<T_Teant>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_TENANT");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.issign).HasColumnName("ISSIGN");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Sex).HasColumnName("SEX");
            Property(m => m.BatethDay).HasColumnName("BIRTHDAY");
            Property(m => m.Phone).HasColumnName("PHONE");
            Property(m => m.QQ).HasColumnName("QQ");
            Property(m => m.Weinxin).HasColumnName("WEIXIN");
            Property(m => m.Work).HasColumnName("WORK");
            Property(m => m.Hobby).HasColumnName("HOBBY");
            Property(m => m.Password).HasColumnName("PASSWORD");
            Property(m => m.DocumentType).HasColumnName("DOCUMENTTYPE");
            Property(m => m.Document).HasColumnName("DOCUMENT");
            Property(m => m.Document).HasColumnName("DOCUMENT");
            Property(m => m.CreateTime).HasColumnName("CREATETIME");
            Property(m => m.Pt_UserName).HasColumnName("PT_USERNAME");
            Property(m => m.Pt_PassWord).HasColumnName("PT_PASSWORD");

            Property(m => m.Zidcard).HasColumnName("ZIDCARD");
            Property(m => m.Fidcard).HasColumnName("FIDCARD");
            Property(m => m.mobject).HasColumnName("OBJECT");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }
   
    public class OwerTenantMapping : BaseEntityTypeMap<T_OwerTeant>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_OWERNTENANT");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Phone).HasColumnName("PHONE");
            Property(m => m.Password).HasColumnName("PASSWORD");
            Property(m => m.DocumentType).HasColumnName("DOCUMENTTYPE");
            Property(m => m.Document).HasColumnName("DOCUMENT");
            Property(m => m.Document).HasColumnName("DOCUMENT");
            Property(m => m.CreateTime).HasColumnName("CREATETIME");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }
    public class TmemoMapping : BaseEntityTypeMap<T_memo>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_MEMO");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Asress).HasColumnName("ADRESS");
            Property(m => m.Content).HasColumnName("CONTENT");
            Property(m => m.Pdate).HasColumnName("PDATE");
            Property(m => m.UserId).HasColumnName("USERID");
            Property(m => m.Status).HasColumnName("STATUS");

            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }
}
