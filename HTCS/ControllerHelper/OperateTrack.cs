using DBHelp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ControllerHelper
{
    /// <summary>
    /// 基类访问过滤器标记
    /// </summary>
    public class BaseActionFilter : ActionFilterAttribute
    {
        private static string simulator = ConfigurationHelper.GetValueByKey("simulator");
        private static string compressPC = ConfigurationHelper.GetValueByKey("compressPC");
        private static string compressHandHold = ConfigurationHelper.GetValueByKey("compressHandHold");
        private static string encryptPC = ConfigurationHelper.GetValueByKey("encryptPC");
        private static string encryptHandHold = ConfigurationHelper.GetValueByKey("encryptHandHold");
        private const string MOBILE_TYPE = "nokia|sony|ericsson|mot|samsung|sgh|lg|sie|philips|panasonic|alcatel|lenovo|cldc|midp|wap|iphone|mobile";
        private const string ENCRYPTCONTENT = "encryptContent";
        private bool _isMobile = false;
        
    

       

    

    
      

      

        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <param name="obj">被转换对象</param>
        /// <returns>转换后byte数组</returns>
        private byte[] Object2Bytes(object obj)
        {
            byte[] buff;
            if (obj is byte[])
            {
                buff = obj as byte[];
            }
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    IFormatter iFormatter = new BinaryFormatter();
                    iFormatter.Serialize(ms, obj);
                    buff = ms.GetBuffer();
                }
            }
            return buff;
        }
    }
    /// <summary>
    /// Restful返回数据结构
    /// </summary>
    internal class ResultCollections<T> where T : new()
    {
        /// <summary>
        /// 返回结果，0：失败；1:成功；2：异常
        /// </summary>
        [JsonProperty("result")]
        public int Result { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonProperty("errorMessage")]
        public String ErrorMessage { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// 翻页数据
        /// </summary>
        [JsonProperty("orderablePagination")]
        public OrderablePagination OrderablePagination { get; set; }

        /// <summary>
        /// 服务器返回时间
        /// </summary>
        [JsonProperty("timeStamp")]
        public DateTime TimeStamp { get; set; }

    }

    /// <summary>
    /// 翻页功能使用
    /// </summary>
    public class OrderablePagination
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int EndIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LastStartIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int NextIndex { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PreviousIndex { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int Sorters { get; set; }
        /// <summary>
        /// 开始页
        /// </summary>
        public int StartIndex { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalCount { get; set; }
    }
}
