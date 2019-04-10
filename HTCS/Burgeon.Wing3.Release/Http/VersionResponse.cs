using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

using Burgeon.Wing3.Release.Environment;

namespace Burgeon.Wing3.Release.Http
{
    /// <summary>
    ///VersionRequest 访问返回信息对象
    /// </summary>
    public class VersionResponse : IReadAs
    {
        public VersionResponse(HttpStatusCode code, string content)
        {
            this.Code = code;
            this.Content = content;
        }

        /// <summary>
        /// 当前请求结果编码
        /// </summary>
        public HttpStatusCode Code { get; private set; }

        /// <summary>
        /// 当前返回结果 内容
        /// </summary>
        public string Content { get; private set; }

        public CommandResult CommandResult
        {
            get
            {
                if (this.Code == HttpStatusCode.OK)
                {
                    return this.ReadAs<CommandResult>();
                }
                else if (this.Code == HttpStatusCode.NotFound)
                {
                    return new CommandResult(ResultStatus.Error, "客户端服务是否已经开启");
                }
                else
                {
                    return new CommandResult(ResultStatus.Error, this.Content);
                }
            }
        }

        /// <summary>
        /// 获取指定类型返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReadAs<T>()
        {
            if (string.IsNullOrWhiteSpace(this.Content))
            {
                return default(T);
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(this.Content);
            }
        }
    }
}