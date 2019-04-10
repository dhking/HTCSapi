using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class kjxMapping : BaseEntityTypeMap<T_kjx>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.id);
            Property(m => m.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_KJX");
            Property(m => m.id).HasColumnName("ID");
          
            Property(m => m.refresh_token).HasColumnName("REFRESH_TOKEN");
            Property(m => m.openid).HasColumnName("OPENID");
            Property(m => m.r_expires_in).HasColumnName("R_EXPIRES_IN");
            Property(m => m.scope).HasColumnName("SCOPE");
            Property(m => m.client_id).HasColumnName("CLIENT_ID");
            Property(m => m.client_secret).HasColumnName("CLIENT_SECRET");
            Property(m => m.username).HasColumnName("USERNAME");
            Property(m => m.password).HasColumnName("PASSWORD");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
            Property(m => m.redirect_uri).HasColumnName("REDIRECT_URI");
        }
    }
    public class UserKeyMapping : BaseEntityTypeMap<UserKey>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("TZ_USERKEY");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.UserName).HasColumnName("USERNAME");
            Property(m => m.KeyId).HasColumnName("KEYID");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
