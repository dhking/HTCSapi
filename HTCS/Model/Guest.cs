using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public class WrapGuest
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Sex { get; set; }

        public string Source { get; set; }

        public DateTime IntoTime { get; set; }

        public string HopeAdress { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal MinPrice { get; set; }

        public string Huxing { get; set; }

        public string Other { get; set; }

        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }


     

        public string House { get; set; }

        public int Ugent { get; set; }


        public int Status { get; set; }
        public long RectDate { get; set; }

        public string QQorWeinxin { get; set; }

        public long UserId { get; set; }

        public string CreatePerson { get; set; }
        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public string Ipimgadess { get; set; }



        [NotMapped]
        public GuestRz guestrz { get; set; }
        [NotMapped]
        public List<GuestRz> guestrzlist { get; set; }
    }
    
    public  class Guest:BasicModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public long CompanyId { get; set; }
        public string Phone { get; set; }

        public string Sex { get; set; }

        public string Source { get; set; }

        public DateTime IntoTime { get; set; }

        public string HopeAdress { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal MinPrice { get; set; }

        public string Huxing { get; set; }

        public string Other { get; set; }

        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }


        public string  CreatePerson { get; set; }

        public string House { get; set; }

        public int Ugent { get; set; }

      
        public int Status { get; set; }
        public long RectDate { get; set; }

        public string QQorWeinxin { get; set; }
       
        public long  UserId { get; set; }

        public DateTime AppointDate { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public string Content { get; set; }
        [NotMapped]
        public DateTime BeginTime { get; set; }
        [NotMapped]
        public DateTime EndTime { get; set; }
        [NotMapped]
        public GuestRz guestrz { get; set; }
        [NotMapped]
        public List<GuestRz> guestrzlist { get; set; }
    }
    public class GuestRz : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Cont { get; set; }

        public long GuestId { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreatePerson { get; set; }

        public string  Fujian { get; set; }

        public string Type { get; set; }

        public string Remark { get; set; }


    }

  
}
