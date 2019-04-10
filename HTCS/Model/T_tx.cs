using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class T_tx:BasicModel
    {
        public long Id { get; set; }
        public decimal amount { get; set; }
        public long CompanyId { get; set; }
        public int Type { get; set; }

        public string Account { get; set; }

        public string RealName { get; set; }


        public DateTime createtime { get; set; }

        public string createperson { get; set; }

        public long userid { get; set; }

        public string liushui { get; set; }

        public int status { get; set; }
        [NotMapped]
        public DateTime month { get; set; }
    }
}
