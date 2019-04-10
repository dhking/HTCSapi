using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model
{
    public  class T_MessageQueue : BasicModel
    {
        public long Id { get; set; }
        public string phone { get; set; }
        public string message { get; set; }
        public DateTime createtime { get; set; }
        public int times { get; set; }
        public int status { get; set; }
        public DateTime begin { get; set; }
        public DateTime end { get; set; }
        public int type { get; set; }
        public long companyid { get; set; }
        public long contractid { get; set; }
    }
}
