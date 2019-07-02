using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RedisHtcs
    {
        private string ip = "127.0.0.1";
        private int port = 6379;
        RedisClient redisClient;
        public RedisHtcs()
        {
            redisClient = new RedisClient(ip, port);
        }
        public bool Set(string key, string value,int seconds=0)
        {

            bool str = redisClient.Set<string>(key, value);
            if (seconds != 0)
            {
                str = redisClient.Expire(key, 120);
            }
            else
            {
                str = redisClient.Expire(key, 120);
            }
            return str;
        }
        public bool Set1(string key, string value,int times)
        {
            bool str = redisClient.Set<string>(key, value);
            str = redisClient.Expire(key, times);
            return str;
        }
        public string Get(string key)
        {
            RedisClient redisClient = new RedisClient(ip, port);
            string str = redisClient.Get<string>(key);

            return str;
        }
        public bool Delete(string key)
        {
            RedisClient redisClient = new RedisClient(ip, port);
            bool str = redisClient.Remove(key);
            return str;
        }
        public bool SetModel<T>(string key, T model)
        {
            //设置10天过期
            bool str = redisClient.Set<T>(key, model);
            str = redisClient.Expire(key, 10*24*3600);
            return str;
        }

        
        public bool SetModel1<T>(string key, T model)
        {
            bool str = redisClient.Set<T>(key, model);
            return str;
        }
        public T GetModel<T>(string key)
        {
            return redisClient.Get<T>(key);
        }
    }
}
