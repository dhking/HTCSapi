using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// 配置文件读取辅助类
    /// </summary>
    public class ConfigurationUtil
    {
        /// <summary>
        /// 读取AppSettings指定配置文件的指定键值 
        /// </summary>
        /// <param name="name">设置名称</param>
        /// <param name="dv">默认值</param>
        /// <returns></returns>
        public static string GetAppsetting(string name, string dv="")
        {
            string v = ConfigurationManager.AppSettings[name];

            if (string.IsNullOrWhiteSpace(v))
            {
                v = dv;
            }

            return v;
        }
    }
}
