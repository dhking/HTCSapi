using DAL;
using Model;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Api.CommonControllers
{
    public class baseController : Controller
    {
        
            public string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

            public T_SysUser getnewuer(T_SysUser user)
            {
                T_SysUser newuser = new T_SysUser();
              
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
            protected T_SysUser GetCurrentUser(String token)
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
           
         
          
        
    }
}