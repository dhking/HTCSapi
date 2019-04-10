using Model.Bill;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs.Bill
{
    public  class T_BilllistMapping : BaseEntityTypeMap<T_BillList>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_BILLLIST");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.BillId).HasColumnName("BILLID");
            Property(m => m.BillType).HasColumnName("BILLTYPE");
            Property(m => m.BillStage).HasColumnName("BILLSTAGE");
            Property(m => m.Amount).HasColumnName("AMOUNT");
            Property(m => m.ReciveAmount).HasColumnName("RECEIVEAMOUNT");
            Property(m => m.RecivedAmount).HasColumnName("RECEIVEDAMOUNT");
            Property(m => m.Explain).HasColumnName("EXPLAIN");
            Property(m => m.BillCode).HasColumnName("BILLCODE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
