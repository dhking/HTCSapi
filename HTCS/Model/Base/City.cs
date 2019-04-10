using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    public  class City: BasicModel
    {
        public long Id { get; set; }
        public string RegionName { get; set; }

        public int RegType { get; set; }
        public int IsRemen { get; set; }
        [NotMapped]
        public string szm { get; set; }

        public long CompanyId { get; set; }
    }
    public class WrapCity
    {
        
        public string Name { get; set; }

        public List<City> city { get; set; }
        public long CompanyId { get; set; }
    }
}
