using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Burgeon.Wing3.Release.Http
{
    /// <summary>
    /// 请求辅助工具
    /// </summary>
    internal class RequestPostContainer
    {
        //get 参数键值列表
        private readonly IDictionary<string, string> urlParameters = null;
        //post file 参数键值列表
        private readonly IDictionary<string, string> fileParameters = null;
        //post 参数键值列表
        private readonly IDictionary<string, string> contentParameters = null;

        public RequestPostContainer()
        {
            urlParameters = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            fileParameters = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            contentParameters = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// 将设置的参数进行写出
        /// </summary>
        public void Write(HttpWebRequest myRequest, IDictionary<string, string> data)
        {
            string method = myRequest.Method.ToUpper();

            switch (method)
            {
                case "GET":
                    this.BuildGetParameter(myRequest);
                    break;
                case "POST":
                    if (this.fileParameters.Count <= 0)
                    {
                        this.BuildPostParameter(myRequest);
                    }
                    else
                    {
                        this.BuildPostParameterWhenHasFiles(myRequest);
                    }
                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// 获取当前容器设置的Get请求 参数键值列表
        /// </summary>
        public IDictionary<string, string> GetMethodParameters
        {
            get
            {
                return urlParameters;
            }
        }

        /// <summary>
        /// 设置Get Url请求数据
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="isOverride">如果已存在同名参数 当为true时 覆盖原有参数值 为false 时 则原有参数与现有值按照','号隔开 例如: name=jack 到 name=jack,lucy</param>
        public void SetGetParam(string name, string value, bool isOverride = false)
        {
            this.AddOrOverride(this.urlParameters, name, value, isOverride);
        }

        /// <summary>
        /// 当Request请求的数据类型设置为multipart/form-data时
        /// 设置一个键值对参数的方法
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        ///<param name="isOverride">如果已存在同名参数 当为true时 覆盖原有参数值 为false 时 则原有参数与现有值按照','号隔开 例如: name=jack 到 name=jack,lucy</param>
        public void SetPostParam(string name, string value, bool isOverride = false)
        {
            this.AddOrOverride(this.contentParameters, name, value, isOverride);
        }

        /// <summary>
        /// 当Request请求的数据类型设置为multipart/form-data时
        /// 设置一个文件参数的方法
        /// </summary>
        /// <param name="name">文件参数值名称</param>
        /// <param name="value">要上传的文件路径</param>
        /// <param name="boundary">边界值</param>
        public void SetPostFileParam(string name, string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                if (!fileParameters.ContainsKey(name))
                {
                    fileParameters.Add(name, fileName);
                }
            }
        }

        /// <summary>
        /// 批量设置文件发送
        /// </summary>
        /// <param name="files"></param>
        public void SetPostFileParams(Dictionary<string, string> files)
        {
            if (files != null && files.Any())
            {
                foreach (string name in files.Keys)
                {
                    this.SetPostFileParam(name, files[name]);
                }
            }
        }

        /// <summary>
        /// 写出设置的Post数据参数
        /// </summary>
        /// <param name="myRequest"></param>
        private void BuildPostParameter(HttpWebRequest myRequest)
        {
            myRequest.Accept = "application/json";
            myRequest.ContentType = "application/x-www-form-urlencoded";

            byte[] content = System.Text.Encoding.UTF8.GetBytes(Utils.UrlParameterUtil.FormatParameters(this.contentParameters));
            using (Stream stream = myRequest.GetRequestStream())
            {
                stream.Write(content, 0, content.Length);
                stream.Flush();
            }
        }

        /// <summary>
        /// 写出设置的Post数据参数
        /// </summary>
        /// <param name="myRequest"></param>
        private void BuildPostParameterWhenHasFiles(HttpWebRequest myRequest)
        {
            string times = DateTime.Now.ToString("yyyyMMddHHmmss");
            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> kv in this.contentParameters)
            {
                sb.Append(this.GetMultipartPostParam(kv.Key, kv.Value, boundary));
            }

            foreach (KeyValuePair<string, string> kv in this.fileParameters)
            {
                sb.Append(this.GetMultipartFileParam(kv.Value, kv.Key, boundary));
            }

            List<byte> bytes = new List<byte>();
            bytes.AddRange(Encoding.UTF8.GetBytes("--" + boundary + "\r\n"));
            bytes.AddRange(Encoding.UTF8.GetBytes(sb.ToString()));
            myRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            myRequest.ContentLength = bytes.Count;
            myRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            myRequest.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            myRequest.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            myRequest.Headers.Add("Cache-Control", "max-age=0");
            using (Stream stream = myRequest.GetRequestStream())
            {
                stream.Write(bytes.ToArray(), 0, bytes.Count);
                stream.Flush();
            }
        }

        /// <summary>
        /// 写出设置的Get数据参数
        /// </summary>
        /// <param name="myRequest"></param>
        private void BuildGetParameter(HttpWebRequest myRequest)
        {

        }

        private string GetMultipartParam(string postParams, string strBoundary)
        {
            return string.Format("--{0}\r\n{1}", strBoundary, postParams);
        }

        private string GetMultipartPostParam(string name, string v, string boundary)
        {
            return this.GetMultipartParam(string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n", name, v), boundary);
        }

        private string GetMultipartFileParam(string filepath, string name, string boundary)
        {
            return this.GetMultipartParam(string.Format("Content-Disposition: form-data;filename=\"{0}\"; name=\"{1}\"\r\nContent-Type: text/xml\r\n\r\n{2}\r\n", (System.IO.Path.GetFileName(filepath)), name, System.IO.File.ReadAllText(filepath, Encoding.UTF8)), boundary);
        }

        private void AddOrOverride(IDictionary<string, string> dict, string name, string value, bool isOverride)
        {
            if (dict.ContainsKey(name))
            {
                if (isOverride)
                {
                    dict[name] = value;
                }
                else
                {
                    dict[name] = string.Format("{0},{1}", dict[name], value);
                }
            }
            else
            {
                dict.Add(name, value);
            }
        }

        public static IDictionary<string, string> CombineParameters(IDictionary<string, string> dict1, IDictionary<string, string> dict2, bool isOverride = false)
        {
            if (dict1 == null)
            {
                return dict2;
            }
            else if (dict2 == null)
            {
                return dict1;
            }
            else
            {
                foreach (string key in dict2.Keys)
                {
                    if (dict1.ContainsKey(key))
                    {
                        if (isOverride)
                        {
                            dict1[key] = dict2[key];
                        }
                        else
                        {
                            dict1[key] = string.Format("{0},{1}", dict1[key], dict2[key]);
                        }
                    }
                    else
                    {
                        dict1.Add(key, dict2[key]);
                    }
                }
            }

            return dict1;
        }
    }
}
