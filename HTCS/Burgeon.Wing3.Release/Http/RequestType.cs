using System;
using System.Collections.Generic;
using System.Web;

namespace Burgeon.Wing3.Release.Http
{
    public enum RequestType
    {
        /// <summary>
        /// 指示使用Http Get谓词访问
        /// </summary>
        GET = 0,
        /// <summary>
        /// 指示使用Http Post谓词访问
        /// </summary>
        POST = 1,
        /// <summary>
        /// 指示使用Http DELETE谓词访问
        /// </summary>
        DELETE = 2,
        /// <summary>
        /// 指示使用Http PUT谓词访问
        /// </summary>
        PUT = 3
    }
}