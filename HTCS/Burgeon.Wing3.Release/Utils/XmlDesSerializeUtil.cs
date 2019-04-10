using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Burgeon.Wing3.Release.Utils
{
    public class XmlDesSerializeUtil
    {
        private const string key = "Burgeon1";
        private const string iv = "Burgeon1";

        public static bool Serialize<T>(T instance, string path)
        {
            try
            {
                IOUtil.AutoCreateDIR(IOUtil.GetNameOfDIR(path));

                using (MemoryStream stream1 = new MemoryStream())
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    serializer.Serialize(stream1, instance);
                    using (FileStream stream2 = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        DESCryptoServiceProvider provider1 = new DESCryptoServiceProvider();
                        provider1.Key = Encoding.ASCII.GetBytes(key);
                        provider1.IV = Encoding.ASCII.GetBytes(iv);
                        CryptoStream stream3 = new CryptoStream(stream2, provider1.CreateEncryptor(), CryptoStreamMode.Write);
                        byte[] buffer1 = stream1.GetBuffer();
                        new StreamWriter(stream3);
                        stream3.Write(buffer1, 0, buffer1.Length);
                        stream3.Close();
                        stream2.Close();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T DeSerialize<T>(string path) where T : class
        {
            try
            {
                if (IOUtil.ExistsFile(path))
                {

                    using (FileStream stream = new FileStream(path, FileMode.Open))
                    {
                        DESCryptoServiceProvider provider1 = new DESCryptoServiceProvider();
                        provider1.Key = Encoding.ASCII.GetBytes(key);
                        provider1.IV = Encoding.ASCII.GetBytes(iv);
                        CryptoStream stream3 = new CryptoStream(stream, provider1.CreateDecryptor(), CryptoStreamMode.Read);
                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                        return serializer.Deserialize(stream3) as T;
                    }
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
