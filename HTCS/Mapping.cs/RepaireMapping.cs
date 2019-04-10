using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class RepaireMapping : BaseEntityTypeMap<Repaire>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_REPAIR");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.HouseId).HasColumnName("HOUSEID");
            Property(m => m.AppiontTime).HasColumnName("APPOINTMENT");
            Property(m => m.CreateTime).HasColumnName("CREATETIME");
            Property(m => m.Adress).HasColumnName("ADRESS");
            Property(m => m.JournaList).HasColumnName("JOURNALIST");     
            Property(m => m.Phone).HasColumnName("PHONE");
            Property(m => m.Province).HasColumnName("PROVINCE");
            Property(m => m.City).HasColumnName("CITY");
            Property(m => m.Area).HasColumnName("AREA");
            Property(m => m.House).HasColumnName("HOUSE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }
    public class RepaireListMapping : BaseEntityTypeMap<RepairList>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_REPAIRLIST");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Project).HasColumnName("PROJECT");
            Property(m => m.Content).HasColumnName("CONTENT");
            Property(m => m.Status).HasColumnName("STATUS");
            Property(m => m.UserId).HasColumnName("USERID");
            Property(m => m.Urgent).HasColumnName("URGENT");
            Property(m => m.Remark).HasColumnName("REMARK");
            Property(m => m.RepairId).HasColumnName("REPAIRID");
            Property(m => m.Image).HasColumnName("IMAGE");
            Property(m => m.Imageweixiu).HasColumnName("IMAGE_WEIXIU");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }

}
