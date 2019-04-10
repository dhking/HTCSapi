using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// 相关加密工具
    /// </summary>
    public class EncryptUtil
    {
        /// <summary>
        /// 加密指定字符串为md5
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Md5(string source)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "MD5").ToLower().Substring(8, 16);
        }

        /// <summary>
        /// 加密指定二进制数据为md5
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Md5(byte[] bytes)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(bytes);
            string ret = "";
            for (int i = 0; i < hash.Length; i++)
            {
                ret += hash[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }
    }
}
