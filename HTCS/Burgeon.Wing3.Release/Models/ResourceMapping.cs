using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Models
{
    /// <summary>
    /// 资源文件映射具体文件或者文件夹结
    /// </summary>
    public class ResourceMapping
    {
        /// <summary>
        /// 获取或者设置当前资源文件绝对路径
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 获取或者设置当前资源文件的资源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或者设置当前资源文件的大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 获取或者设置当前起源文件的扩展名称
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 获取或者设置当前目录父级目录路径
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// 获取或者设置当前资源文件类型
        /// </summary>
        public ResourceType ResourceType { get; set; }

        public string Date { get; set; }
    }

    public enum ResourceType
    {
        File = 0,
        Directory = 1
    }
}
