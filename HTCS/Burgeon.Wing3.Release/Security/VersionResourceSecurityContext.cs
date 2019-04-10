using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Security
{
    /// <summary>
    /// 版本文件校验上下文
    /// </summary>
    public class VersionResourceSecurityContext
    {
        public VersionResourceSecurityContext(string file, string key)
        {
            this.Key = key;
            this.FileName = file;
        }

        /// <summary>
        /// 当前版本文件
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 当前文件传输通道使用的key
        /// </summary>
        public string Key { get; private set; }

    }
}
