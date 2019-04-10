using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp
{
    /// <summary>
    /// 读取配置文件
    /// </summary>
    public class ConfigurationHelper
    {
        /// <summary>
        /// 每页显示记录数Key
        /// </summary>
        const String PAGE_SIZE = "PageSize";

        /// <summary>
        /// 
        /// </summary>
        public const String RESTFUL_URL = "RestfulUrl";

        /// <summary>
        /// 获取Restfulf地址
        /// </summary>
        /// <returns></returns>
        public static String GetRestfulUrl()
        {
            String restfulUrl = "";
            try
            {
                restfulUrl = ConfigurationManager.AppSettings[RESTFUL_URL];
            }
            catch (Exception)
            {
                LogUtil.WriteLog("获取的RestfulUrl的值失败。", UtilLogType.Error);
            }

            return restfulUrl;
        }

        /// <summary>
        /// 获取每页显示记录数
        /// </summary>
        /// <returns></returns>
        public static int GetGridPageSize()
        {
            int pageSize = 20;
            try
            {
                String size = ConfigurationManager.AppSettings[PAGE_SIZE];
                if (int.TryParse(size, out pageSize) == false)
                {
                    pageSize = 20;
                    LogUtil.WriteLog("PageSize转换失败,使用默认值。", UtilLogType.Error);
                }
                if (pageSize <= 0)
                {
                    pageSize = 20;
                    LogUtil.WriteLog("获取的PageSize值是:" + size + ",不是有效值，使用默认值。", UtilLogType.Error);
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("获取PageSize失败,使用默认值。", UtilLogType.Error, ex);
            }

            return pageSize;
        }

        /// <summary>
        /// 通过Key返回对应的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns>获取失败返回空字符串</returns>
        public static String GetValueByKey(String key)
        {
            try
            {
                String keyValue = ConfigurationManager.AppSettings[key];

                return keyValue;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("获取" + key + "值失败,返回空字符串。", UtilLogType.Error, ex);

                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String GetConnectionString(String key)
        {
            try
            {
                ConnectionStringSettings keyValue = ConfigurationManager.ConnectionStrings[key];

                return keyValue.ConnectionString;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("获取" + key + "连接串失败,返回空字符串。", UtilLogType.Error, ex);

                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static object GetSection(string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                return null;
            }
            return ConfigurationManager.GetSection(sectionName);
        }
    }
}
