using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Contrct
{
    public  class T_Otherfee: BasicModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public decimal Amount { get; set; }

        public int Type { get; set; }

        public decimal Price { get; set; }

        public decimal Reading { get; set; }

        public int Pinlv { get; set; }
        [NotMapped]
        public string strPinlv { get; set; }
        public DateTime CateTime { get; set; }

        public string  CreatePerson { get; set; }
        public DateTime ChaobiaoTime { get; set; }
       
        public long ContractId { get; set; }
        public int IsYajin { get; set; }



        public int Object { get; set; }
    }
}
