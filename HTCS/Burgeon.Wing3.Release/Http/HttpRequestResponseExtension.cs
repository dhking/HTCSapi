using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Burgeon.Wing3.Release.Http
{
    public static class HttpRequestResponseExtension
    {
        /// <summary>
        /// 将本次请求结果返回以字符串的形式
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string ReadAsString(this HttpWebResponse response)
        {
            string content = "";
            if (response != null)
            {
                Stream stream = response.GetResponseStream();
                if (stream != null)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// 将本次返回的结果返回以指定的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static T ReadAs<T>(this HttpWebResponse response) where T : class
        {
            T r = default(T);
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.ReadAsString());
            }
            catch
            {
                return r;
            }
        }

        /// <summary>
        /// 设置Request请求正文内容
        /// </summary>
        /// <param name="stream">Request的InputStream</param>
        /// <param name="name">请求数据字符串</param>
        public static int SetRequestStream(this Stream stream, string content)
        {
            int length = 0;
            if (stream != null && !string.IsNullOrWhiteSpace(content))
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(content);
                length = bytes.Length;
                stream.Write(bytes, 0, bytes.Length);
            }
            return length;
        }
    }
}
