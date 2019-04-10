using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class HouseTrant : BasicModel
    {
        public long Id { get; set; }

        public string HouseName { get; set; }

        public string TrantName { get; set; }
        public long CompanyId { get; set; }
        public string Phone { get; set; }
    }
}
