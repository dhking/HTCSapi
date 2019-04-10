using Model;
using Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class FinanceMapping : BaseEntityTypeMap<FinanceModel>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_FINANCE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.HouseId).HasColumnName("HOUSEID");
            Property(m => m.Trader).HasColumnName("TRADER");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.CostName).HasColumnName("COSTNAME");
            Property(m => m.Amount).HasColumnName("AMOUNT");
            Property(m => m.PayType).HasColumnName("PAYTYPE");
            Property(m => m.TradingDate).HasColumnName("TRADINGTIME");
            Property(m => m.Remark).HasColumnName("REMARK");
            Property(m => m.TrandId).HasColumnName("TRANTID");
            Property(m => m.PayMentNumber).HasColumnName("PAYMENTNUMBER");
            Property(m => m.Transaoctor).HasColumnName("TRANSACTOR");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");



        }
    }
}
