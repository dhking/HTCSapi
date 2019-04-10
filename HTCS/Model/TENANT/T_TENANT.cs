using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TENANT
{
    public  class T_Teant:BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }

        public int issign { get; set; }
        public DateTime BatethDay { get; set; }
        public string Phone { get; set; }
        [NotMapped]
        public string NewPhone { get; set; }
        public string QQ { get; set; }
        public string Weinxin { get; set; }
        public string Work { get; set; }
        public string Hobby { get; set; }
        public string Password { get; set; }
        public int DocumentType { get; set; }
        public string Document { get; set; }
        public DateTime CreateTime { get; set; }
        public string Pt_UserName { get; set; }
        public string Pt_PassWord { get; set; }
        public string Zidcard { get; set; }
        public string Fidcard { get; set; }

        public int mobject { get; set; }
    }
    public class T_OwerTeant : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
      
        public string Phone { get; set; }
        public string Password { get; set; }
        public int DocumentType { get; set; }
        public string Document { get; set; }
        public DateTime CreateTime { get; set; }
 
    }
}
