using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release
{
    public class AppVersionLibrary
    {
        /// <summary>
        /// 获取已经存在版本
        /// </summary>
        /// <returns></returns>
        public static string[] GetPartVersions()
        {
            string[] versions = System.IO.Directory.GetFiles(Environment.ResourceEnvironment.Environment.TopBuilVerionDIR, "*.rar");
            return versions.Select<string, string>(m => System.IO.Path.GetFileNameWithoutExtension(m)).ToArray();
        }
    }
}
