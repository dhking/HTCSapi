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

namespace DBHelp
{
    public class ConvertHelper
    {
        public static string getsuijishu()
        {
            string re = "";
            DateTime dt = DateTime.Now;
            re=dt.ToString("yyyyMMddhhmmss")+System.Guid.NewGuid().ToString() ;
            return re;
        }
        public static string ImgToBase64String(string Imagefilename)
        {
            try
            {
                Bitmap bmp = new Bitmap(Imagefilename);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DateTime DateByInt(long date)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = date * 10000;
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
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

        public static string MD5Encrypt(string strText, Encoding encoding)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(encoding.GetBytes(strText));
            return System.Text.Encoding.Default.GetString(result);
        }
    }
}
