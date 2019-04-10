using DBHelp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp
{
    public class CouchbaseHelper
    {
        private IDataBucket couchbaseClient;
        private static CouchbaseHelper couchbaseHelper;
        private static CouchbaseHelper memcachedHelper;
        private static readonly object syncRoot = new object();    ////程序运行时创建一个静态的只读对象
        public static CouchbaseHelper CouchbaseInstance
        {
            get
            {
                if (couchbaseHelper == null)
                {
                    lock (syncRoot)
                    {
                        if (couchbaseHelper == null)
                        {
                            couchbaseHelper = new CouchbaseHelper();
                            couchbaseHelper.couchbaseClient = CouchbaseManager.GetCouchbaseClient();
                        }
                    }
                }
                if (couchbaseHelper.couchbaseClient == null)
                {
                    couchbaseHelper.couchbaseClient = CouchbaseManager.GetCouchbaseClient();
                }

                return couchbaseHelper;
            }
        }
        public static CouchbaseHelper MemcachedInstance
        {
            get
            {
                if (memcachedHelper == null)
                {
                    lock (syncRoot)
                    {
                        if (memcachedHelper == null)
                        {
                            memcachedHelper = new CouchbaseHelper();
                            memcachedHelper.couchbaseClient = CouchbaseManager.GetMemcachedClient();
                        }
                    }
                }
                if (memcachedHelper.couchbaseClient == null)
                {
                    memcachedHelper.couchbaseClient = CouchbaseManager.GetCouchbaseClient();
                }

                return memcachedHelper;
            }
        }

        private CouchbaseHelper()
        {

        }

        public Object GetValue(String key)
        {
            return couchbaseClient.GetValue(key);
        }

        public T GetValue<T>(string key)
        {
            return couchbaseClient.GetValue<T>(key);
        }

        public bool AddValue(string key, object value)
        {
            return couchbaseClient.AddValue(key, value);
        }

        public bool SaveValue(string key, object value)
        {
            return couchbaseClient.SaveValue(key, value);
        }

        public bool ReplaceValue(string key, object value)
        {
            return couchbaseClient.ReplaceValue(key, value);
        }

        public bool RemoveValue(string key)
        {
            return couchbaseClient.RemoveValue(key);
        }
    }
}
