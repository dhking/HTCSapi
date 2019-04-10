using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
{
    public   class ThirdDAL
    {
        //获取百度token
        public string GetbaiduToken()
        {
            string token = "";
            string key = "baidutoken";
            RedisHtcs redis = new RedisHtcs();
            token = redis.GetModel<string>(key);
            if (token == null)
            {
                string clientId = "0evoe5y72GK9FEuGrREzkmib";
                string clientSecret = "FOxeLZQYcCEAaQAGOr8Ovqeox9jDS1bd";
                string re = getAccessToken(clientId, clientSecret);
                Baidu baidu = JsonConvert.DeserializeObject<Baidu>(re);
                token = baidu.access_token;
                redis.Set1(key, token, baidu.expires_in);
                return token;
            }
            return token;
        }
        
        public  String getAccessToken(string clientId, string clientSecret)
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            return result;
        }
        // 身份证识别
        public  string idcard(string strbaser64,string token,string font)
        {
            string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/idcard?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = true;
            String str = "id_card_side="+ font + "&image=" + HttpUtility.UrlEncode(strbaser64);
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            Console.WriteLine("身份证识别:");
            Console.WriteLine(result);
            return result;
        }
    }
}
