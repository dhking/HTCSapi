using DAL;
using DBHelp;

using Model;
using Model.House;
using Model.Menu;
using Model.User;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
namespace Service
{

    public  class HelpService
    {
        public string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        public T_SysUser getnewuer(T_SysUser user)
        {
            T_SysUser newuser = new T_SysUser();
            
            newuser.areas = getcityorarea(user.area);
            newuser.citys = getcityorarea(user.city);
            newuser.Id = user.Id;
            newuser.CompanyId = user.CompanyId;
            return newuser;
        }
        public long[] getstore(string str)
        {
            long[] arrs = new long[] { };
            string[] strarrs = new string[] { };
            if (str != null)
            {
                strarrs = str.Split(",");
                if (strarrs.Length > 0)
                {
                    arrs = Array.ConvertAll<string, long>(strarrs, long.Parse);
                }
            }
            return arrs;
        }
        public string[] getcityorarea(string str)
        {
            long[] arrs = new long[] { };
            string[] strarrs = new string[] { };
            if (str != null)
            {
                strarrs = str.Split(",");
            }
            return strarrs;
        }
        /// <summary>
        /// 通过token获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public  T_SysUser GetCurrentUser(String token)
        {
            RedisHtcs redis = new RedisHtcs();
            T_SysUser userDto = redis.GetModel<T_SysUser>(token);
            return userDto;
        }
        protected ElecUser GetelecUser(String token)
        {
            RedisHtcs redis = new RedisHtcs();
            ElecUser userDto = redis.GetModel<ElecUser>(token);
            return userDto;
        }
        public bool checkPression(T_SysUser user, string[] buttonname)
        {
            bool result = false;
            int count = 0;
            if (user.listpression != null)
            {
                count = user.listpression.Where(p => buttonname.Contains(p.Code)&& p.appcheck == 1).Count();
                //count = user.listpression.Where(p => buttonname.Contains(p.Code)).Count();
                if (count > 0)
                {
                    result = true;
                }
            }
            return result;
        }
       
      
     
    }
}
