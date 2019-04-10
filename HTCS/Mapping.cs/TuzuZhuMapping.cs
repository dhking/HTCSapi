using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class TuzuZhuMapping : BaseEntityTypeMap<Tuizu>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_TUIZU");
            Property(m => m.Id).HasColumnName("ID");

            Property(m => m.Amount).HasColumnName("AMOUNT");


            Property(m => m.Price).HasColumnName("PRICE");
            Property(m => m.BeginTime).HasColumnName("BEGINTIME");
            Property(m => m.EndTime).HasColumnName("ENDTIME");
            Property(m => m.Type).HasColumnName("TYPE");

            Property(m => m.Code).HasColumnName("CODE");
            Property(m => m.Name).HasColumnName("NAME");

            Property(m => m.zhuId).HasColumnName("TUIFANGZHUID");
            Property(m => m.Status).HasColumnName("STATUS");


            Property(m => m.Explain).HasColumnName("EXPLAIN");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class InitTuizuMapping : BaseEntityTypeMap<Inittuizu>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_INITTUIZU");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Amount).HasColumnName("AMOUNT");
            Property(m => m.Price).HasColumnName("PRICE");
            Property(m => m.BeginTime).HasColumnName("BEGINTIME");
            Property(m => m.EndTime).HasColumnName("ENDTIME");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.TkType).HasColumnName("TKTYPE");
            Property(m => m.IsPayBill).HasColumnName("ISPAYBILL");
            Property(m => m.Code).HasColumnName("CODE");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Explain).HasColumnName("EXPLAIN");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
