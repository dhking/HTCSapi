using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Security;
using System.Net;

using Burgeon.Wing3.Release.Utils;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// 下载辅助工具
    /// </summary>
    public class DownloadUtil
    {
        public static string DownloadVersion(string versionURL, string filePath)
        {
            WebClient client = new WebClient();
            client.DownloadFile(new Uri(versionURL), filePath);
            return filePath;
        }
    }
}
