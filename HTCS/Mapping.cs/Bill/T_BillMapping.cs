using Model.Bill;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs.Bill
{
    public  class T_BillMapping : BaseEntityTypeMap<T_Bill>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_BILL");
            Property(m => m.Id).HasColumnName("ID");
         
            Property(m => m.stage).HasColumnName("STAGE");
            Property(m => m.Object).HasColumnName("OBJECT");
            Property(m => m.islock).HasColumnName("ISLOCK");
            Property(m => m.BeginTime).HasColumnName("BEGINTIME");
            Property(m => m.EndTime).HasColumnName("ENDTIME");
            Property(m => m.TeantId).HasColumnName("TEANTID");
            Property(m => m.HouseId).HasColumnName("HOUSEID");
            Property(m => m.HouseType).HasColumnName("HOUSETYPE");
            Property(m => m.PayTime).HasColumnName("PAYTIME");
            Property(m => m.PayType).HasColumnName("PAYTYPE");
            Property(m => m.CreatePerson).HasColumnName("CREATEPERSON");
            Property(m => m.TranSactor).HasColumnName("TRANSACTOR");
            Property(m => m.CreateTime).HasColumnName("CREATETIME");
            Property(m => m.Remark).HasColumnName("REMARK");
            Property(m => m.Voucher).HasColumnName("VOUCHER");
            Property(m => m.ContractId).HasColumnName("CONTRACTID");
            Property(m => m.PayStatus).HasColumnName("STATUS");
            Property(m => m.ShouldReceive).HasColumnName("SHOURECEIVETIME");
            Property(m => m.Amount).HasColumnName("AMOUNT");
            Property(m => m.Explain).HasColumnName("EXPLAIN");
            Property(m => m.Liushui).HasColumnName("LIUSHUI");
            Property(m => m.sign).HasColumnName("SIGN");
            Property(m => m.BillType).HasColumnName("BILLTYPE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

            Property(m => m.payee).HasColumnName("PAYEE");
            Property(m => m.accounts).HasColumnName("ACCOUNTS");
            Property(m => m.bank).HasColumnName("BANK");
            Property(m => m.subbranch).HasColumnName("SUBBRANCH");
            Property(m => m.type).HasColumnName("TYPE");
        }
    }
}
