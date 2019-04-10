using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class T_ZafeiList: BasicModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public int IsYajin { get; set; }
        public int TuiType { get; set; }
        public long CompanyId { get; set; }
        [DefaultValue(1)]
        public int IsActive { get; set; }
    }
}
