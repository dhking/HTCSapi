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
    public class HouseZKMapping : BaseEntityTypeMap<HouseZK>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_HOUSEZK");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Fx).HasColumnName("FX");
            Property(m => m.Cx).HasColumnName("CX");
            Property(m => m.Image).HasColumnName("IMAGE");
            Property(m => m.Adress).HasColumnName("ADRESS");
            Property(m => m.Title).HasColumnName("TITLE");
            Property(m => m.Price).HasColumnName("PRICE");
            Property(m => m.IsRm).HasColumnName("ISRM");
            Property(m => m.PushTime).HasColumnName("PUSHTIME");
            Property(m => m.Status).HasColumnName("STATUS");
            Property(m => m.city).HasColumnName("CITY");
            Property(m => m.area).HasColumnName("AREA");
            Property(m => m.businessarea).HasColumnName("BUSINESSAREA");
            Property(m => m.cityid).HasColumnName("CITYID");
            Property(m => m.areaid).HasColumnName("AREAID");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.Fukuan).HasColumnName("FUKUAN");
            Property(m => m.TingWei).HasColumnName("TINGWEI");
            Property(m => m.Shi).HasColumnName("SHI");
            Property(m => m.RecentTime).HasColumnName("RECNTTIME");
            Property(m => m.Phone).HasColumnName("PHONE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class TeseZKMapping : BaseEntityTypeMap<TeseZK>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_TESEZK");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.HouseId).HasColumnName("HOUSEID");
            Property(m => m.Code).HasColumnName("CODE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class PeibeiZKMapping : BaseEntityTypeMap<PeibeiZK>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_PEIBEIZK");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.HouseId).HasColumnName("HOUSEID");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class JiaoTongMapping : BaseEntityTypeMap<JiaoTong>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_JIAOTONG");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Xian).HasColumnName("XIAN");
            Property(m => m.Zhan).HasColumnName("ZHAN");
            Property(m => m.Juli).HasColumnName("JULI");
            Property(m => m.XianNumber).HasColumnName("XIANNUMBER");
            Property(m => m.HouseId).HasColumnName("HOUSEID");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class ZhoubianMapping : BaseEntityTypeMap<Zhoubian>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_ZHOUBIAN");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Value).HasColumnName("VALUE");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.HouseId).HasColumnName("HOUSEID");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    
}
