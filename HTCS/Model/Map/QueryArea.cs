using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Map
{
    public  class QueryArea
    {
       public int status { get; set; }

       public string   info { get; set; }

       public string infocode { get; set; }
    
       public int count { get; set; }

       public List<districts> districts { get; set; }
    }
    public class  districts
    {
        public string citycode { get; set; }

        public string adcode { get; set; }

        public string name { get; set; }

        [JsonProperty("districts")]
        public List<districts> districts1 { get; set; }
    }
}
