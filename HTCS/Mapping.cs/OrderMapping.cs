using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class OrderMapping : BaseEntityTypeMap<Order>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_ORDER");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.ispt).HasColumnName("ISPT");
            Property(m => m.Amount).HasColumnName("AMOUNT");

            Property(m => m.UserId).HasColumnName("USERID");
            Property(m => m.Status).HasColumnName("STATUS");

            Property(m => m.House).HasColumnName("HOUSE");
            Property(m => m.HouseId).HasColumnName("HOUSEID");

            Property(m => m.zftype).HasColumnName("ZFTYPE");
            Property(m => m.liushui).HasColumnName("LIUSHUI");
            Property(m => m.message).HasColumnName("MESSAGE");
            Property(m => m.Price).HasColumnName("PRICE");
            Property(m => m.value).HasColumnName("VALUE");

            Property(m => m.CreateTime).HasColumnName("CREATETIME");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
            Property(m => m.isaddacountt).HasColumnName("ISADDACOUNTT");

        }
    }
    public class OrderListMapping : BaseEntityTypeMap<T_OrderList>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_ORDERLIST");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.orderid).HasColumnName("ORDERID");
            Property(m => m.amount).HasColumnName("AMOUNT");
            Property(m => m.billid).HasColumnName("BILLID");
            Property(m => m.name).HasColumnName("NAME");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }
}
