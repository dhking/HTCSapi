using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class GuestMapping : BaseEntityTypeMap<Guest>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_GUEST");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
         
            Property(m => m.Phone).HasColumnName("PHONE");
            Property(m => m.Sex).HasColumnName("SEX");
            Property(m => m.Source).HasColumnName("SOURCE");

            Property(m => m.IntoTime).HasColumnName("INTOTIME");
            Property(m => m.HopeAdress).HasColumnName("HOPEADRESS");
            Property(m => m.MaxPrice).HasColumnName("MAXRECENT");

            Property(m => m.MinPrice).HasColumnName("MINRECENT");
            Property(m => m.Huxing).HasColumnName("HUXING");

            Property(m => m.Other).HasColumnName("OTHER");
            Property(m => m.Remark).HasColumnName("REMARK");

            Property(m => m.CreateTime).HasColumnName("CREATETIME");
            Property(m => m.CreatePerson).HasColumnName("CREATEPERSON");
            Property(m => m.House).HasColumnName("HOUSE"); 
            Property(m => m.Ugent).HasColumnName("URGENT");
            Property(m => m.RectDate).HasColumnName("RECTDATE");
            Property(m => m.Status).HasColumnName("STATUS");
            Property(m => m.QQorWeinxin).HasColumnName("QQORWEINXIN");
            Property(m => m.UserId).HasColumnName("CHARGE");
            Property(m => m.AppointDate).HasColumnName("APPOINTDATE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class GuestRZMapping : BaseEntityTypeMap<GuestRz>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_GUESTRZ");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Cont).HasColumnName("CONT");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.GuestId).HasColumnName("GUESTID");
            Property(m => m.CreateTime).HasColumnName("CREATETIME");
            Property(m => m.CreatePerson).HasColumnName("CREATEPERSON");
            Property(m => m.Remark).HasColumnName("REMARK");
            Property(m => m.Fujian).HasColumnName("FUJIAN");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }
}
