using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class TuikuanMapping : BaseEntityTypeMap<TuizuZhu>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_TUIZUZHU");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Amount).HasColumnName("AMOUNT");
            Property(m => m.AmountName).HasColumnName("AMOUNTNAME");
            Property(m => m.IsKongzhi).HasColumnName("ISKONGZHI");
            Property(m => m.TuifangTime).HasColumnName("TUIFANGTIME");
            Property(m => m.fukuanTime).HasColumnName("FUKUANTIME");
            Property(m => m.PayType).HasColumnName("PAYTYPE");
            Property(m => m.BanLi).HasColumnName("BANLI");
            Property(m => m.Bank).HasColumnName("BANK");
            Property(m => m.Pingzheng).HasColumnName("PINGZHENG");
            Property(m => m.ContractId).HasColumnName("CONTRACTID");
            Property(m => m.FukuanType).HasColumnName("FUKUANDYPE");
            Property(m => m.TkType).HasColumnName("TKTYPE");
            Property(m => m.Account).HasColumnName("ACCOUNT");
            Property(m => m.result).HasColumnName("RESULT");
            Property(m => m.Status).HasColumnName("STATUS");
            Property(m => m.BankName).HasColumnName("BANKNAME");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.time).HasColumnName("TIME");
            Property(m => m.subbranch).HasColumnName("SUBBRANCH");
            

        }
    }
}
