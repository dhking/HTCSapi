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

    public class templateMaping : BaseEntityTypeMap<T_template>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_TEMPLATE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.content).HasColumnName("CONTENT");
            Property(m => m.title).HasColumnName("TITLE");
            Property(m => m.isdefault).HasColumnName("ISDEFAULT");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
            Property(m => m.ispublic).HasColumnName("ISPUBLIC");
            Property(m => m.htmlcontent).HasColumnName("HTMLCONTENT");
            
        }
    }
    public class BaseDataMaping : BaseEntityTypeMap<T_Basics>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_BASICS");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Code).HasColumnName("CODE");
            Property(m => m.ParaType).HasColumnName("PARATYPE");
            Property(m => m.IsActive).HasColumnName("ISACTIVE");
            //Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class bankcardMaping : BaseEntityTypeMap<T_bankcard>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_BANKCARD");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }

    public class accountMaping : BaseEntityTypeMap<T_account>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_ACCOUNT");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.name).HasColumnName("NAME");
            Property(m => m.onlinesign).HasColumnName("ONLINESIGN");
            Property(m => m.phone).HasColumnName("PHONE");
           
            Property(m => m.orderamount).HasColumnName("ORDERAMOUNT");
            Property(m => m.password).HasColumnName("PASSWORD");
            Property(m => m.OnlinePay).HasColumnName("ONLINEPAY");
            Property(m => m.IdenTity).HasColumnName("IDENTITY");
            Property(m => m.Zfrz).HasColumnName("ZFRZ");
            Property(m => m.url).HasColumnName("URL");
            Property(m => m.Amount).HasColumnName("AMOUNT");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

            Property(m => m.rentmessage).HasColumnName("RENTMESSAGE");

            Property(m => m.name).HasColumnName("NAME");
            Property(m => m.certificate).HasColumnName("CERTIFICATE");
            Property(m => m.address).HasColumnName("ADDRESS");
            Property(m => m.contact).HasColumnName("CONTACT");
            Property(m => m.isreg).HasColumnName("ISREG");
            Property(m => m.contractnumber).HasColumnName("CONTRACTNUMBER");
            Property(m => m.smsnumber).HasColumnName("SMSNUMBER");
            Property(m => m.brand).HasColumnName("BRAND");

            Property(m => m.charge).HasColumnName("CHARGE");
        }
    }
    public class tuoguanMaping : BaseEntityTypeMap<t_tuoguan>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_TUOGUAN");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Phone).HasColumnName("PHONE");
            Property(m => m.City).HasColumnName("CITY");
            Property(m => m.Adress).HasColumnName("ADRESS");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class T_basticTypeMaping : BaseEntityTypeMap<T_basicsType>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_BASICSTYPE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Code).HasColumnName("CODE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }
    public class T_v_basticTypeMaping : BaseEntityTypeMap<T_V_basicc>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_V_BASICC");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.IsActive).HasColumnName("ISACTIVE");
            Property(m => m.ParaType).HasColumnName("PARATYPE");
            Property(m => m.typecode).HasColumnName("TYPECODE");
            Property(m => m.typename).HasColumnName("TYPENAME");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");

        }
    }
    public class T_TeseMaping : BaseEntityTypeMap<T_Tese>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_TESE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.HouseId).HasColumnName("HOUSEID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class T_peibeiMaping : BaseEntityTypeMap<peipei>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_PEBEI");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.HouseId).HasColumnName("HOUSEID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
