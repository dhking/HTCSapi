using Model.Contrct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs.Contrct
{
   public  class OtherFeeMapping : BaseEntityTypeMap<T_Otherfee>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           
            ToTable("T_OTHERFEE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Amount).HasColumnName("AMOUNT");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.Price).HasColumnName("PRICE");
            Property(m => m.Reading).HasColumnName("READING");
            Property(m => m.CateTime).HasColumnName("CREATETIME");
            Property(m => m.CreatePerson).HasColumnName("CREATEPERSON");
            Property(m => m.ChaobiaoTime).HasColumnName("CHAOBIAOTIME");
            Property(m => m.ContractId).HasColumnName("CONTRACTID");
            Property(m => m.IsYajin).HasColumnName("ISYAJIN");
            Property(m => m.Pinlv).HasColumnName("PINLV");
            Property(m => m.Object).HasColumnName("OBJECT");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
