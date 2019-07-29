using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public  class indepent : BasicModel
    {
        public long Id { get; set; }
        
        public string name { get; set; }

        public string image { get; set; }

        public string ip { get; set; }

       
    }
    public class houresourcesupper : BasicModel
    {
        public long Id { get; set; }

        public long houseid { get; set; }

        public int gwisupper { get; set; }

        public int housetype { get; set; }
    }
}
