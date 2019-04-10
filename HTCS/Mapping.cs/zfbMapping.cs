using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class zfbMapping : BaseEntityTypeMap<T_PayMentAcount>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_PAYMENTACOUNT");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.app_id).HasColumnName("APP_ID");
            Property(m => m.private_key).HasColumnName("PRIVATE_KEY");
            Property(m => m.public_key).HasColumnName("PUBLIC_KEY");
            Property(m => m.public_key_zf).HasColumnName("PUBLIC_KEY_ZF");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.reflash_token).HasColumnName("REFLASH");
            Property(m => m.redirect_uri).HasColumnName("REDIRECT_URI");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }

    }

    public class accountbankMapping : BaseEntityTypeMap<accountbank>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_ACCOUNTBANK");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.type).HasColumnName("TYPE");
            Property(m => m.accountname).HasColumnName("ACCOUNTNAME");
            Property(m => m.account).HasColumnName("ACCOUNT");
            Property(m => m.bank).HasColumnName("BANK");
            Property(m => m.zhbank).HasColumnName("ZHBANK");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }
}
