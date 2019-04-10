using Model;
using Model.Map;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class MapDAL
    {
        public SysResult<List<tip>> Query(QueryMap param)
        {
            SysResult<List<tip>> result = new SysResult<List<tip>>();
            MapAdress map = new MapAdress();
            //string url = "http://restapi.amap.com/v3/assistant/inputtips?keywords=" + param.xiaoqu + " &type=120302&city=" + param.city + "&citylimit=true&output=json&key=dc7168dfcba0683198d14c057eae4c50&extensions=all";
            string url = "http://restapi.amap.com/v3/place/text?keywords=" + param.xiaoqu + " &types=120000&city=" + param.city + "&citylimit=true&output=json&key=dc7168dfcba0683198d14c057eae4c50&extensions=all&offset=25&page=1";
            var Requst = (HttpWebRequest)WebRequest.Create(url);
            var Response = (HttpWebResponse)Requst.GetResponse();
            var responseString = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            map = JsonConvert.DeserializeObject<MapAdress>(responseString);
            if (map.status != 1)
            {
                result.Code = map.status;
                result.Message = map.info;
            }
            else
            {
                if (map.pois != null)
                {
                    result.numberData = map.pois;
                    result.numberCount = map.pois.Count();
                }
            }
            return result;
        }
        public SysResult<List<pois>> Querymap(QueryMap param)
        {
            SysResult<List<pois>> result = new SysResult<List<pois>>();
            MapAdress1 map = new MapAdress1();
            string url = "http://restapi.amap.com/v3/place/text?keywords=" + param.xiaoqu + " &type=120302&city=" + param.city + "&citylimit=true&output=json&key=dc7168dfcba0683198d14c057eae4c50";
            var Requst = (HttpWebRequest)WebRequest.Create(url);
            var Response = (HttpWebResponse)Requst.GetResponse();
            var responseString = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            map = JsonConvert.DeserializeObject<MapAdress1>(responseString);
            if (map.status != 1)
            {
                result.Code = map.status;
                result.Message = map.info;
            }
            else
            {
                result.numberData = map.pois;

            }
            return result;
        }
        public QueryArea Querymap1(QueryArea param)
        {
            QueryArea result = new QueryArea();
            string url = "https://restapi.amap.com/v3/config/district?key=" + "dc7168dfcba0683198d14c057eae4c50" + "&subdistrict=3&page=1&offset=20&keywords=" + param.name;
            var Requst = (HttpWebRequest)WebRequest.Create(url);
            var Response = (HttpWebResponse)Requst.GetResponse();
            var responseString = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            result = JsonConvert.DeserializeObject<QueryArea>(responseString);

            return result;
        }

        public districts Querymap2(QueryArea param)
        {
            districts result = new districts();
            string url = "https://restapi.amap.com/v3/config/district?key=" + "dc7168dfcba0683198d14c057eae4c50" + "&subdistrict=3&page=1&offset=20&keywords=" + param.name;
            var Requst = (HttpWebRequest)WebRequest.Create(url);
            var Response = (HttpWebResponse)Requst.GetResponse();
            var responseString = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            result = JsonConvert.DeserializeObject<districts>(responseString);

            return result;
        }
    }
}
