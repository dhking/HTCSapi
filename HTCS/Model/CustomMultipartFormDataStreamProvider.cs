using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public string filename1 { get; set; }
        public CustomMultipartFormDataStreamProvider(string path) : base(path)
        {

        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string newFileName = string.Empty;
            newFileName = Path.GetExtension(headers.ContentDisposition.FileName.Replace("\"", string.Empty));//获取后缀名  
            newFileName = uploadhelp.buildFileName(newFileName);
            filename1 += newFileName + ";";
          
            return newFileName;
        }
    }
    public static class uploadhelp
    {
        public static string url = "http://139.224.196.42:81/";
        public static string substring(string a)
        {
            string[] str = a.Split('\\'); //根据特定符号截取为字符串数组；  
            string temp = str[str.Length - 1];
            temp = url + temp;
            return temp;
        }
        public static string buildFileName(string extion)
        {
            //new一个时间对象date  
            DateTime dt = DateTime.Now;
            return dt.ToString("yyyyMMddhhmmss") + System.Guid.NewGuid().ToString() + extion;
        }
    }
}
