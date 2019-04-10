using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{
    public class ConvertHelper
    {
        private int rep = 0;
        public static string  pinlv(int pinlv)
        {
            string str = "";
            switch (pinlv)
            {
                case 0:
                    str = "随租金支付";
                    break;
                case 1:
                    str = "一月一付";
                    break;
                case 2:
                    str = "二月一付";
                    break;
                case 3:
                    str = "三月一付";
                    break;
                case 4:
                    str = "四月一付";
                    break;
                case 5:
                    str = "五月一付";
                    break;
                case 6:
                    str = "半年一付";
                    break;
                case 12:
                    str = "一年一付";
                    break;
                case 24:
                    str = "二年一付";
                    break;
                case 99:
                    str = "一次付清";
                    break;
                default:
                    str = "Unknown";
                    break;
            }
            return str;
        }
        public static string getsecond()
        {
            DateTime dt_1970 = new DateTime(1970, 1, 1);
            TimeSpan span = DateTime.Now - dt_1970;
            span -= TimeSpan.FromHours(8);
            long lo = (long)span.TotalMilliseconds;
            return lo.ToString();
        }

        public static long getsecond2(DateTime dt)
        {
            DateTime dt_1970 = new DateTime(1970, 1, 1);
            TimeSpan span = dt - dt_1970;
            span -= TimeSpan.FromHours(8);
            long lo = (long)span.TotalMilliseconds;
            return lo;
        }


        public static string getsecond1()
        {
            DateTime dt_1970 = new DateTime(1970, 1, 1);
            TimeSpan span = DateTime.Now - dt_1970;
            span -= TimeSpan.FromHours(8);
            long lo = (long)span.TotalSeconds;
            return lo.ToString();
        }
        public static string GenerateCheckCodeNum(int codeCount)
        {
           return  Guid.NewGuid().ToString();
        }
        public static  DateTime DateByInt(long date)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = date * 10000;
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public static DateTime Transfer(long date)
        {
            return DateTime.Now.AddSeconds(date);
        }
        public static TimeSpan DateDiff(DateTime DateTime1, DateTime DateTime2)
        {

            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                return ts1.Subtract(ts2).Duration();

            }
            catch
            {
                return TimeSpan.MaxValue;
            }

        }

        public static Hashtable ConvertDataRowToHashTable(DataRow row, DataColumnCollection columns, Hashtable ht = null)
        {
            if (ht == null)
                ht = new Hashtable();
            if (row == null || row[0] == null)
                return ht;
            else
            {
                foreach (DataColumn column in columns)
                {
                    ht.Add(column.ColumnName.ToUpper(), row[column.ColumnName]);
                }

            }
            return ht;
        }

        // Methods
        public static bool ObjToBool(object obj)
        {
            bool flag;
            if (obj == null)
            {
                return false;
            }
            if (obj.Equals(DBNull.Value))
            {
                return false;
            }
            string str = obj.ToString();
            if (str == "1" || str == "Y")
                return true;
            else if (str == "0" || str == "N")
                return false;
            else
            {
                return (bool.TryParse(obj.ToString(), out flag) && flag);
            }

        }

        public static DateTime? ObjToDateNull(object obj)
        {
            if (obj == null || obj.ToStr() == "0001/1/1 0:00:00")
            {
                return Convert.ToDateTime(DateTime.MinValue);
            }
            try
            {
                return new DateTime?(Convert.ToDateTime(obj));
            }
            catch
            {
                return null;
            }
        }

        public static DateTime ObjToDateTime(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return Convert.ToDateTime(DateTime.MinValue);
            }
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                throw;
            }
        }
        public static SqlDateTime ObjToSqlDateTime(object obj)
        {
            if (obj == null || obj.Equals(DBNull.Value))
            {
                return SqlDateTime.MinValue;
            }
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                throw;
            }
        }


        public static SqlDateTime? ObjToSqlDateNull(object obj)
        {
            if (obj == null)
            {
                return SqlDateTime.MinValue;
            }
            try
            {
                return new DateTime?(Convert.ToDateTime(obj));
            }
            catch
            {
                return SqlDateTime.MinValue;
            }
        }



        public static decimal ObjToDecimal(object obj)
        {
            if (obj == null)
            {
                return 0M;
            }
            if (obj.Equals(DBNull.Value))
            {
                return 0M;
            }
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return 0M;
            }
        }

        public static decimal? ObjToDecimalNull(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            if (obj.Equals(DBNull.Value))
            {
                return null;
            }
            return new decimal?(ObjToDecimal(obj));
        }

        public static double ObjToDouble(object obj)
        {
            if (obj == null)
            {
                return 0.0;
            }
            if (obj.Equals(DBNull.Value))
            {
                return 0.0;
            }
            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return 0.0;
            }
        }

        public static double? ObjToDoubleNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj.Equals(DBNull.Value))
            {
                return null;
            }
            return new double?(ObjToDouble(obj));
        }

        public static int ObjToInt(object obj)
        {
            if (obj != null)
            {
                int num;
                if (obj.Equals(DBNull.Value))
                {
                    return 0;
                }
                if (int.TryParse(obj.ToString(), out num))
                {
                    return num;
                }
            }
            return 0;
        }

        public static Int64 ObjToInt64(object obj)
        {
            if (obj != null)
            {
                Int64 num;
                if (obj.Equals(DBNull.Value))
                {
                    return 0;
                }
                if (Int64.TryParse(obj.ToString(), out num))
                {
                    return num;
                }
            }
            return 0;
        }

        public static int? ObjToIntNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj.Equals(DBNull.Value))
            {
                return null;
            }
            return new int?(ObjToInt(obj));
        }

        public static bool HasMoreRow(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool HasMoreRow(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// MD5(32位加密)
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>MD5加密后的字符串</returns>
        public static string GetMd5HashStr(string str)
        {
            string pwd = string.Empty;

            //实例化一个md5对像
            MD5 md5 = MD5.Create();

            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }

            return pwd;
        }
        //32 位小写加密
        public static string GetMd5HashStr1(string str)
        {
            string pwd = string.Empty;

            //实例化一个md5对像
            MD5 md5 = MD5.Create();

            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString();
            }

            return pwd;
        }
        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }
            return byte2String;
        }
        public static string ObjToStr(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            if (obj.Equals(DBNull.Value))
            {
                return "";
            }
            return Convert.ToString(obj);
        }

        public static byte ObjToByte(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            if (obj.Equals(DBNull.Value))
            {
                return 0;
            }
            return Convert.ToByte(obj);
        }

        public static string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.Default.GetBytes(strText));
            return System.Text.Encoding.Default.GetString(result);
        }
        public static string Getsuiji()
        {
            string str = "";
            for (int i = 0; i < 1000; i++)
            {
                str += new Random(Guid.NewGuid().GetHashCode()).Next(0, 100);
            }
            return str;
        }
        public static string MD5Encrypt(string strText, Encoding encoding)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(encoding.GetBytes(strText));
            return System.Text.Encoding.Default.GetString(result);
        }
    }
}
