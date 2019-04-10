using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Burgeon.Wing3.Release.Http
{
    /// <summary>
    /// 基于HttpWebRequest封装的Http操作类
    /// </summary>
    public class VersionRequest
    {
        /// <summary>
        /// 请求 参数管理容器
        /// </summary>
        private RequestPostContainer container;

        /// <summary>
        /// 获取当前请求地址参数
        /// </summary>
        public string URL { get; private set; }

        /// <summary>
        /// 获取当前Http请求类型
        /// </summary>
        public RequestType Method { get; private set; }

        /// <summary>
        /// 创建一个请求对象
        /// </summary>
        /// <param name="uRL">请求地址</param>
        public VersionRequest(string uRL, RequestType method)
        {
            if (uRL == null)
            {
                throw new ArgumentNullException("uRL");
            }
            if (!uRL.StartsWith("http:"))
            {
                uRL = "http://" + uRL;
            }
            this.URL = uRL;
            this.Method = method;
            this.container = new RequestPostContainer();
        }

        /// <summary>
        /// 开始请求 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public VersionResponse Request(IDictionary<string, string> data = null)
        {
            VersionResponse response = null;

            try
            {
                data = RequestPostContainer.CombineParameters(this.container.GetMethodParameters, data);
                HttpWebRequest request = this.InitRequest(data);
                if (request == null)
                {
                    response = new VersionResponse(HttpStatusCode.NonAuthoritativeInformation, string.Format("VersionRequest 不支持{0}谓词进行Http访问", Enum.GetName(typeof(RequestType), this.Method)));
                }
                else
                {
                    //装载请求数据
                    this.ReadyRequestData(request, data);
                    HttpWebResponse rsp = request.GetResponse() as HttpWebResponse;
                    using (Stream stream = rsp.GetResponseStream())
                    {
                        response = new VersionResponse(HttpStatusCode.OK, rsp.ReadAsString());
                    }
                }
            }
            catch (Exception ex)
            {
                response = new VersionResponse(HttpStatusCode.InternalServerError, "地址:" + URL + " " + ex.ToString());
            }

            return response;
        }

        /// <summary>
        /// 装载请求数据
        /// </summary>
        private void ReadyRequestData(HttpWebRequest request, IDictionary<string, string> data)
        {
            this.container.Write(request, data);
        }

        /// <summary>
        /// 初始化请求 请求派发
        /// </summary>
        /// <returns></returns>
        private HttpWebRequest InitRequest(IDictionary<string, string> data)
        {
            //请求派发
            switch (this.Method)
            {
                case RequestType.GET:
                    return this.InitGetRequest(data);
                case RequestType.POST:
                    return this.InitPostRequest();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 初始化GET请求
        /// </summary>
        /// <returns></returns>
        private HttpWebRequest InitGetRequest(IDictionary<string, string> data)
        {
            string uRL = Utils.UrlParameterUtil.CombineURLParams(URL, Utils.UrlParameterUtil.FormatParameters(data));
            HttpWebRequest request = HttpWebRequest.Create(uRL) as HttpWebRequest;
            request.Method = "GET";
            request.Accept = "application/json";
            return request;
        }

        /// <summary>
        /// 初始化POST请求
        /// </summary>
        /// <returns></returns>
        private HttpWebRequest InitPostRequest()
        {
            HttpWebRequest request = HttpWebRequest.Create(this.URL) as HttpWebRequest;
            request.Accept = "application/json";
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            return request;
        }

        /// <summary>
        /// 设置Get Url请求数据
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="isOverride">如果已存在同名参数 默认为false 当为true时 覆盖原有参数值 为false 时 则原有参数与现有值按照','号隔开 例如: name=jack 到 name=jack,lucy</param>
        public void SetGetParam(string name, string value, bool isOverride = false)
        {
            this.container.SetGetParam(name, value, isOverride);
        }

        /// <summary>
        /// 当Request请求的数据类型设置为multipart/form-data时
        /// 设置一个键值对参数的方法
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="boundary">边界值</param>
        public void SetPostParam(string name, string value, bool isOverride = false)
        {
            this.container.SetPostParam(name, value, isOverride);
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
            this.container.SetPostFileParam(name, fileName);
        }

        /// <summary>
        /// 批量设置文件发送
        /// </summary>
        /// <param name="files"></param>
        public void SetPostFileParams(Dictionary<string, string> files)
        {
            this.SetPostFileParams(files);
        }
    }
}