using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Map
{
    public class QueryMap
    {
        public string xiaoqu { get; set; }
        public string city { get; set; }
    }
    public   class MapAdress
    {
        public int status { get; set; }
        public string info { get; set; }
        public int count { get; set; }
        public List<tip> pois { get; set; }
    }
    public class suggestion
    {
       
    }
    public class tip
    {
        public string id { get; set; }
        public string name { get; set; }
        public int pcode { get; set; }
        public int citycode { get; set; }
        public int adcode { get; set; }
        public string pname { get; set; }
        public string cityname { get; set; }
        public string adname { get; set; }
        public object business_area { get; set; }
        public object address { get; set; }
     
    }
    public class MapAdress1
    {
        public int status { get; set; }
        public string info { get; set; }
        public int count { get; set; }
        public List<pois> pois { get; set; }
    }
    public class pois
    {
        public string address { get; set; }
        public string adname { get; set; }
    }
    public class QueryArea
    {
        public int status { get; set; }

        public string info { get; set; }

        public string infocode { get; set; }

        public int count { get; set; }

        public string name { get; set; }

        public string level { get; set; }
        public List<districts> districts { get; set; }
    }
    public class districts
    {
        public string citycode { get; set; }

        public string adcode { get; set; }
        public int status { get; set; }
        public string info { get; set; }
        public string name { get; set; }
        public string level { get; set; }
        [JsonProperty("districts")]
        public List<districts> districts1 { get; set; }
    }
}
