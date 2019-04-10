using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// Http 请求参数 辅助工具
    /// </summary>
    public class UrlParameterUtil
    {
        /// <summary>
        /// 将传入的键值对参数 转换成GET请求参数 键值对字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FormatParameters(IDictionary<string, string> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            int ind = 0;
            string[] kvs = new string[data.Keys.Count];

            foreach (string key in data.Keys)
            {
                kvs[ind++] = (string.Format("{0}={1}", key, data[key]));
            }

            return String.Join("&", kvs);
        }

        /// <summary>
        /// 合并url片段 例如: http://a.com/ 与 /api/send
        /// 合并成:http://a.com/api/send
        /// </summary>
        /// <param name="uRL"></param>
        /// <param name="uRL2"></param>
        /// <returns></returns>
        public static string CombineURL(string uRL, string uRL2)
        {
            if (uRL == null || uRL2 == null)
            {
                return null;
            }
            else
            {
                uRL = uRL.EndsWith("/") ? uRL.TrimEnd('/') : uRL;
                uRL2 = uRL2.StartsWith("/") ? uRL2 : "/" + uRL2;
                return string.Format("{0}{1}", uRL, uRL2);
            }
        }

        /// <summary>
        /// 合并Get URL请求参数
        /// </summary>
        /// <param name="remoteURL"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string CombineURLParams(string remoteURL, string parameters)
        {
            if (remoteURL == null)
            {

            }
            else if (remoteURL.IndexOf("?") > -1)
            {
                return string.Format("{0}&{1}", remoteURL, parameters);
            }
            else
            {
                return string.Format("{0}?{1}", remoteURL, parameters);
            }

            return remoteURL;
        }
    }
}
