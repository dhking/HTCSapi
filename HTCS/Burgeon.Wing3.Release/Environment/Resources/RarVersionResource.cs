using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Environment.Resources
{
    /// <summary>
    /// 压缩包 版本文件承载类
    /// </summary>
    public class RarVersionResource : VersionResource
    {
        private string rarFileOrDIR;

        public RarVersionResource(string fileOrDIR)
        {
            rarFileOrDIR = fileOrDIR;
        }
    }
}
