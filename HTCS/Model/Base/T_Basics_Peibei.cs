using Model.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
  
    public class T_template : BasicModel
    {
        public long Id { get; set; }

        public int ispublic { get; set; }

        public string title { get; set; }

        public string content { get; set; }
        [NotMapped]
        public int onlinesign { get; set; }
        public int isdefault { get; set; }
        public long CompanyId { get; set; }
    }
    public class T_bankcard : BasicModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public long CompanyId { get; set; }
    }

    public class T_account : BasicModel
    {
        public long Id { get; set; }

        public string phone { get; set; }

        public string name { get; set; }

        public long CompanyId { get; set; }
        public string account { get; set; }

        public decimal Amount { get; set; }

        public decimal orderamount { get; set; }
        public string bank { get; set; }
    
        public string url { get; set; }

        public string password { get; set; }

        public int OnlinePay { get; set; }

        public int onlinesign { get; set; }

        public int Zfrz { get; set; }

        public int IdenTity { get; set; }

        public string zfb { get; set; }

        public string wx { get; set; }

        public string zfbname { get; set; }

        public string wxname { get; set; }

        public long contractnumber { get; set; }

        public long smsnumber { get; set; }

        [NotMapped]
        public string yzm { get; set; }
    }
    public class Queryparam
    {
        public int type { get; set; }
        public int teseorpeibei { get; set; }
        public string paratype { get; set; }
        public long CompanyId { get; set; }

        public string Code { get; set; }
    }
    public class WrapBasic
    {
        public List<T_Basics> peipei { get; set; }
        public List<T_Basics> tese { get; set; }
        public List<WrapCity> city { get; set; }
        public List<T_SysUser> user { get; set; }
    }
    
    public class T_Basics: BasicModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        //字典类型
        public string ParaType { get; set; }
        public int IsActive { get; set; }
        [NotMapped]
       
        public List<T_basicsType> listparatype { get; set; }
        //public long CompanyId { get; set; }
    }
    public class T_basicsType : BasicModel
    {
        public long Id { get; set; }
        //参数名称
        public string Name { get; set; }
        //参数类型
        public string Code { get; set; }
        public long CompanyId { get; set; }

    }
    public class t_tuoguan : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public string Adress { get; set; }


    }
    public class T_V_basicc : BasicModel
    {
       
        public long Id { get; set; }
        public string Name { get; set; }
        //字典类型
        public string ParaType { get; set; }
        public int Type { get; set; }
        public int IsActive { get; set; }
        public string typecode { get; set; }
        public string typename { get; set; }
        public long CompanyId { get; set; }
    }
    public class T_Tese: BasicModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public int HouseId { get; set; }
        public int Type { get; set; }
        [NotMapped]
        public int IsCheck { get; set; }
    }

    public class T_SysMessage : BasicModel
    {
        public long Id { get; set; }
        public string content { get; set; }
        public DateTime createtime { get; set; }
        public string createperson { get; set; }
        public long userid { get; set; }
        public long CompanyId { get; set; }
        public int type { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }
    public enum type
    {

        hezupublic,
        hezuprivate
      

    }
}
