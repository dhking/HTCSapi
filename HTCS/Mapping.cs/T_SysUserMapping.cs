using Model;
using Model.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs
{
    public class BanbenMapping : BaseEntityTypeMap<BanBen>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("TZ_BANBEN");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("NAME");
            Property(m => m.Cont).HasColumnName("CONT");
            Property(m => m.Url).HasColumnName("URL");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.banben).HasColumnName("BANBEN");

        }
    }
    public class T_RecordMapping : BaseEntityTypeMap<T_Record>
    {

        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_RECORD");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.type).HasColumnName("TYPE");
            Property(m => m.createtime).HasColumnName("CREATETIME");
            Property(m => m.amount).HasColumnName("AMOUNT");
            Property(m => m.account).HasColumnName("ACCOUNT");
            Property(m => m.liushui).HasColumnName("LIUSHUI");
            Property(m => m.name).HasColumnName("NAME");
            Property(m => m.bank).HasColumnName("BANK");
            Property(m => m.zhbank).HasColumnName("ZHBANK");
            Property(m => m.Status).HasColumnName("STATUS");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
          
        }
    }
    public class storeMapping : BaseEntityTypeMap<t_store>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_STORE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.name).HasColumnName("NAME");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class T_SysUserMapping : BaseEntityTypeMap<T_SysUser>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_SYSUSER");

            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.Name).HasColumnName("USERNAME");
            Property(m => m.storeid).HasColumnName("STOREID");
            Property(m => m.Password).HasColumnName("USERPASSWORD");
            Property(m => m.Mobile).HasColumnName("MOBILE");
            Property(m => m.Vip).HasColumnName("VIP");
            Property(m => m.Email).HasColumnName("EMAIL");
            Property(m => m.RealName).HasColumnName("REALNAME");
            Property(m => m.AuthShop).HasColumnName("AUTHSHOP");

            Property(m => m.isactive).HasColumnName("ISACTIVE");
            Property(m => m.ptPassword).HasColumnName("PT_PASSWORD");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
            Property(m => m.Zfbzh).HasColumnName("ZFBZH");
            Property(m => m.Wxzh).HasColumnName("WXZH");
            Property(m => m.isquit).HasColumnName("ISQUIT");
            Property(m => m.userimg).HasColumnName("USERIMG");
            Property(m => m.nickname).HasColumnName("NICKNAME");
            Property(m => m.pinpai).HasColumnName("PINPAI");
            Property(m => m.city).HasColumnName("CITY");
            Property(m => m.area).HasColumnName("AREA");
            Property(m => m.province).HasColumnName("PROVINCE");

            Property(m => m.registrationId).HasColumnName("REGISTRATIONID");
            Property(m => m.range).HasColumnName("RANGE");
            Property(m => m.pt_username).HasColumnName("PT_USERNAME");
            Property(m => m.roleid).HasColumnName("ROLEID");
            Property(m => m.cellname).HasColumnName("CELLNAME");
            Property(m => m.type).HasColumnName("TYPE");
        }
    }
    public class departmentMapping : BaseEntityTypeMap<t_department>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_DEPARTMENT");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.name).HasColumnName("NAME");
            Property(m => m.city).HasColumnName("CITY");
            Property(m => m.area).HasColumnName("AREA");
            Property(m => m.parentid).HasColumnName("PARENTID");
            Property(m => m.companyid).HasColumnName("COMPANYID");

            Property(m => m.phone).HasColumnName("PHONE");
            Property(m => m.adress).HasColumnName("ADRESS");
            Property(m => m.stroe).HasColumnName("STROE");
            Property(m => m.userids).HasColumnName("USERIDS");
            Property(m => m.cellname).HasColumnName("CELLNAME");
        }
    }
    //电表账号

    public class ElecUserMapping : BaseEntityTypeMap<ElecUser>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_ELECTRIC");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.username).HasColumnName("USERNAME");
            Property(m => m.pass).HasColumnName("PASSWORD");
            Property(m => m.paratype).HasColumnName("PARATYPE");
         

            Property(m => m.CompanyId).HasColumnName("CONPANYID");
            Property(m => m.Type).HasColumnName("TYPE");
           
    }
    }
    //认证信息
    public class CertIficationMapping : BaseEntityTypeMap<T_CertIfication>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("CERTIFICATION");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.phone).HasColumnName("PHONE");
            Property(m => m.province).HasColumnName("PROVINCE");
            Property(m => m.city).HasColumnName("CITY");
            Property(m => m.legalperson).HasColumnName("LEGALPERSON");
            Property(m => m.company).HasColumnName("COMPANY");
            Property(m => m.account).HasColumnName("ACCOUNT");

            Property(m => m.name).HasColumnName("NAME");
            Property(m => m.imgyyzz).HasColumnName("IMGYYZZ");
            Property(m => m.imggz).HasColumnName("IMGGZ");


            Property(m => m.UserId).HasColumnName("USERID");

            Property(m => m.realname).HasColumnName("REALNAME");
            Property(m => m.idcard).HasColumnName("IDCARD");
            Property(m => m.idzimg).HasColumnName("IDZIMG");
            Property(m => m.idfimg).HasColumnName("IDFIMG");
            Property(m => m.validity).HasColumnName("VALIDITY");
            Property(m => m.type).HasColumnName("TYPE");
            Property(m => m.status).HasColumnName("STATUS");
            
            Property(m => m.result).HasColumnName("RESULT");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class SysUserRoleMapping : BaseEntityTypeMap<T_SysUserRole>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_SYSUSERROLE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.SysUserId).HasColumnName("SYSUSERID");
            Property(m => m.SysRoleId).HasColumnName("SYSROLEID");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
    public class SysRoleMapping : BaseEntityTypeMap<T_SysRole>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_SYSROLE");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.RoleName).HasColumnName("ROLENAME");
            Property(m => m.RoleDesc).HasColumnName("ROLEDESC");
            Property(m => m.PasswordExpiration).HasColumnName("PASSWORDEXPIRATION");
            Property(m => m.IsActive).HasColumnName("ISACTIVE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
            Property(m => m.range).HasColumnName("RANGE");
            Property(m => m.ishouse).HasColumnName("ISHOUSE");
        }
    }

    public class feedbackMapping : BaseEntityTypeMap<feedback>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.id);

            Property(m => m.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("T_FEEDBACK");
            Property(m => m.id).HasColumnName("ID");
            Property(m => m.content).HasColumnName("CONTENT");
            Property(m => m.userid).HasColumnName("USERID");
            Property(m => m.phone).HasColumnName("PHONE");
            Property(m => m.createtime).HasColumnName("CREATETIME");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.CompanyId).HasColumnName("COMPANYID");
        }
    }
}
