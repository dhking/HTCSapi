using Model.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.User
{
    public class t_store : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string name { get; set; }
    }
    public class WrapT_SysUser : BasicModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        
        public string RealName { get; set; }

       
     
        public string Email { get; set; }
        public string Password { get; set; }
        public string AuthShop { get; set; }
        [NotMapped]
        public string roles { get; set; }
        [NotMapped]
        public List<Pression> listpression { get; set; }
        [NotMapped]
        public List<T_SysUserRole> listrole { get; set; }
        [NotMapped]
        public List<T_SysUserRole> deletebilllist { get; set; }
    }
    //public class yzRequest
    //{
    //    public string Phone { get; set; }
    //    public string Temp { get; set; }
    //}
    public  class Wrapcert
    {
        public T_CertIfication qiye { get; set; }

        public T_CertIfication geren { get; set; }
    }
    public class T_CertIfication : BasicModel
    {
        public long   Id { get; set; }
        public string phone { get; set; }
        public string province { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string legalperson { get; set; }
        public string company { get; set; }
        public string account { get; set; }
        public long CompanyId { get; set; }
        public string imgyyzz { get; set; }
        public string imggz { get; set; }
        public long   UserId { get; set; }
        public string realname { get; set; }
        public string idcard { get; set; }
        public string idzimg { get; set; }
        public string idfimg { get; set; }
        public string validity { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public string result { get; set; }

    }
    public   class T_SysUser: BasicModel
    {
        public string registrationId { get; set; }
       
        public long Id { get; set; }
        public string storeid { get; set; }
      
        public string area { get; set; }
        [NotMapped]
        public long[] storeids { get; set; }
        [NotMapped]
        public string[] citys { get; set; }
        [NotMapped]
        public string[] areas { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ptPassword { get; set; }
        public string AuthShop { get; set; }
        public int Vip { get; set; }

        public int range { get; set; }
        public int isactive { get; set; }

       


        public string Zfbzh { get; set; }

        public string Wxzh { get; set; }

     

       

       

        public string userimg { get; set; }

        public string nickname { get; set; }


        public string pt_username { get; set; }

        public string pinpai { get; set; }

        public string city { get; set; }

        public long CompanyId { get; set; }

        public string province { get; set; }



        //我接单的报修信息
        [NotMapped]
        public int Repaire { get; set; }
        [NotMapped]
        public string yzm { get; set; }
        [NotMapped]
        public string token { get; set; }
        [NotMapped]
        public string roles { get; set; }
        [NotMapped]
        public List<Pression> listpression { get; set; }
        [NotMapped]
        public  List<T_SysUserRole> listrole { get; set; }
        [NotMapped]
        public List<WrapSysUserRole> listrole1 { get; set; }

        [NotMapped]
        public List<T_SysUserRole> deletebilllist { get; set; }
    }
    public class T_Record : BasicModel
    {
        [NotMapped]
        public DateTime BeginTime { get; set; }
        [NotMapped]
        public DateTime EndTime { get; set; }

        public long Id { get; set; }
        [NotMapped]
        public long accountbankid { get; set; }
        public int  Status { get; set; }

        public int type { get; set; }

        public long CompanyId { get; set; }
        public DateTime createtime { get; set; }

        public decimal amount { get; set; }

        public string account { get; set; }

        public string liushui { get; set; }

        public string name { get; set; }

        public string bank { get; set; }
        public string zhbank { get; set; }
        [NotMapped]
        public string password { get; set; }
    }
    public class T_Shop : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
    }
    public class T_SysRole : BasicModel
    {
        public long Id { get; set; }

        public string RoleName { get; set; }

        public string RoleDesc { get; set; }
        public long CompanyId { get; set; }
        public int PasswordExpiration { get; set ; }

        public int IsActive { get; set; }
        [NotMapped]
        public T_Button Btn { get; set; }
    }
    public class T_SysUserRole : BasicModel
    {
        public long Id { get; set; }

        public long SysUserId { get; set; }

        public long SysRoleId { get; set; }
        public long CompanyId { get; set; }
        [NotMapped]
        public int edtype { get; set; }

        [NotMapped]
        public int btnjishu { get; set; }
    }

    public class WrapSysUserRole : BasicModel
    {
        public long Id { get; set; }

        public long SysUserId { get; set; }

        public long SysRoleId { get; set; }
        [NotMapped]
        public string rolename { get; set; }
       
    }
    public class T_Button : BasicModel
    {
        public long Id { get; set; }
        public string BtnName { get; set; } = string.Empty;
        public string BtnNo { get; set; }

        public long MenuId { get; set; }

        public int Multiple { get; set; }


        public int btnjishu { get; set; }

        public string ButtonUrl { get; set; }

        public string BtnIcon { get; set; }


        public long orderby { get; set; }
        
    }
   
}
