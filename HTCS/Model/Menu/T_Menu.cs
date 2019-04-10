using Model.Base;
using Model.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Menu
{
    public  class T_Menu: BasicModel
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public string title { get; set; }
        public int sign { get; set; }
        public string jump{ get; set; }
        public int Jishu { get; set; }

        public int orderby { get; set; }
        public long ParentId { get; set; }
        public long SystemId { get; set; }
        [NotMapped]       
        public List<T_Button> btn { get; set; }
        [NotMapped]
       
        public List<T_Menu> list { get; set; }
      
    }
    public class WrapPression
    {
        public List<Pression> changedata { get; set; }
        public List<Pression> savedata { get; set; }
        public long RoleId { get; set; }
    }
    public class Pression
    {
        public long Id { get; set; }
        public long MenuId { get; set; }
        public long ButtonId { get; set; }
        [DefaultValue(0)]
        public long RoleId { get; set; }
        public string name { get; set; }
        public string Code { get; set; }
        public bool open { get; set; }
        public bool doCheck { get; set; }
        public int Jishu { get; set; }
        public long SystemId { get; set; }
        public long ParentId { get; set; }
        [JsonProperty("checked")]
        public bool checkedd { get; set; }

        public int appcheck { get; set; }
        public bool checkedOld { get; set; }
        public int sign { get; set; }
        public List<Pression> children { get; set; }
    }
    public class T_SysRoleMenu : BasicModel
    {
        public long Id { get; set; }
        public long ButtonId { get; set; }
        [DefaultValue(0)]
        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public long Ptype { get; set; }
       
    }
}
