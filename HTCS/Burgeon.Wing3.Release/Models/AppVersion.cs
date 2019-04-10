using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Environment;

namespace Burgeon.Wing3.Release.Models
{
    /// <summary>
    /// 一个版本信息
    /// </summary>
    public class AppVersion
    {
        /// <summary>
        /// 获取或者设置使用当前升级的用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 获取或者设置当前版本路径
        /// </summary>
        public string VersionRAR { get; set; }

        /// <summary>
        /// 获取或者设置当前版本下载地址
        /// </summary>
        public string VersionDownloadURL { get; set; }

        /// <summary>
        /// 获取或者设置当前打包版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 获取或者设置当前升级主服务器地址
        /// </summary>
        public string MainHost { get; set; }

        /// <summary>
        /// 获取或者设置当前升级是否停止Web网站
        /// </summary>
        public bool StopWebApp { get; set; }

        /// <summary>
        /// 获取或者设置当前升级是否停止API网站
        /// </summary>
        public bool StopApiApp { get; set; }

        /// <summary>
        /// 获取或者设置当前升级是否停止自动任务
        /// </summary>
        public bool StopAutoTask { get; set; }

        public Client Client { get; set; }
    }
}
