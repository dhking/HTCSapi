using Model.Base;
using Model.House;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class Repaire : BasicModel
    {
        public long Id { get; set; }
        public long  HouseId { get; set; }
        [NotMapped]
        public long storeid { get; set; }
        public long CompanyId { get; set; }
        public DateTime AppiontTime { get; set; }
        public string Adress { get; set; }
        public DateTime CreateTime { get; set; }
        public string JournaList { get; set; }
        public string Phone { get; set; }

        public string Remark { get; set; }

        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string House { get; set; }
        [NotMapped]
        public string Content { get; set; }
        [NotMapped]
        public DateTime BeginTime { get; set; }
        [NotMapped]
        public DateTime EndTime { get; set; }
        [NotMapped]
        public int Status { get; set; }
        [NotMapped]
        public string projectname { get; set; }
        [NotMapped]
        public long UserId { get; set; }
        [NotMapped]
        public List<string> project { get; set; }
        [NotMapped]
        public List<RepairList> list { get; set; }
        [NotMapped]
        public List<RepairList> deletebilllist { get; set; }

        [NotMapped]

        public int group { get; set; }

    }
    public class WrapShaixuan
    {
        public List<WrapCell> cell { get; set; }

        public List<T_Basics> subject { get; set; }
    }
    public class WrapRepaire : BasicModel
    {
      
        public long storeid { get; set; }
        public long CompanyId { get; set; }
        public long Id { get; set; }
        public long HouseId { get; set; }
        public long ContractId { get; set; }

        public DateTime AppiontTime { get; set; }
        public string Adress { get; set; }
        public string Image { get; set; }
        public string Revsion { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string JournaList { get; set; }

        public string Phone { get; set; }

        public string Province { get; set; }

        public string City { get; set; }
        public string Area { get; set; }
        public string House { get; set; }

        public string Content { get; set; }
        [NotMapped]
        public string Ipimgadess { get; set; }
        [NotMapped]
        public List<string> project { get; set; }
        [NotMapped]
        public List<WrapRepairList> list { get; set; }
    }
    public class paratou
    {
        
      
       
       
       
       
        
        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string House { get; set; }
    }
    public class RepairList : BasicModel
    {
        [NotMapped]
        public DateTime CreateTime { get; set; }
        [NotMapped]
        public DateTime AppiontTime { get; set; }
        [NotMapped]
        public string  House { get; set; }
        [NotMapped]
        public string Adress { get; set; }
        [NotMapped]
        public string JournaList { get; set; }
        [NotMapped]
        public string Phone { get; set; }
        public long CompanyId { get; set; }

        public int Urgent { get; set; }
        public string Remark { get; set; }
        public long Id { get; set; }
        public long RepairId { get; set; }
        public string Project { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Imageweixiu { get; set; }
        public long? UserId { get; set; }
        public int Status { get; set; }
    }
    public class WrapRepairList : BasicModel
    {
        public long Id { get; set; }
        public string Remark { get; set; }
        public long RepairId { get; set; }

        public string Project { get; set; }

        public string Content { get; set; }

        public long? UserId { get; set; }
        public string Image { get; set; }

        public string Imageweixiu { get; set; }

        public int Urgent { get; set; }

        public int Status { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public string UserPhone { get; set; }
    }
}
