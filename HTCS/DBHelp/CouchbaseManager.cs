using DBHelp.ConfigHelp;
using DBHelp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp
{
    public class CouchbaseManager
    {
        const string COUCHBASECLIENT = "couchbaseclient";
        const string MEMCACHEDCLIENT = "memcachedclient";
        private static IDataBucket couchbaseClient;
        private static IDataBucket memcachedClient;
        private static LoadedObjectsSettings loadedObject = LoadedObjectsSettings.GetSection();
        private static readonly object syncRoot = new object();    ////程序运行时创建一个静态的只读对象
        private CouchbaseManager() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IDataBucket GetCouchbaseClient(string name = "")
        {
            if (couchbaseClient == null)
            {
                lock (syncRoot)
                {
                    if (couchbaseClient == null)
                    {
                        ObjectElement element;
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            element = loadedObject.GetObjectElement(COUCHBASECLIENT);
                        }
                        else
                        {
                            element = loadedObject.GetObjectElement(name);
                        }

                        couchbaseClient = CreateDataBucket(element.AssemblyName, element.TypeName);
                    }
                }
            }
            return couchbaseClient;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IDataBucket GetMemcachedClient(string name = "")
        {
            if (memcachedClient == null)
            {
                lock (syncRoot)
                {
                    if (memcachedClient == null)
                    {
                        ObjectElement element;
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            element = loadedObject.GetObjectElement(MEMCACHEDCLIENT);
                        }
                        else
                        {
                            element = loadedObject.GetObjectElement(name);
                        }

                        memcachedClient = CreateDataBucket(element.AssemblyName, element.TypeName);
                    }
                }
            }
            return memcachedClient;
        }

        private static IDataBucket CreateDataBucket(string assemblyName, string typeName)
        {
            return (IDataBucket)Assembly.Load(assemblyName).CreateInstance(typeName);
        }

    }
}
