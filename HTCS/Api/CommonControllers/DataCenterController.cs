using DAL;
using DBHelp;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
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

namespace API.CommonControllers
{
    public class DataCenterController : RcsBaseController
    {
        public string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        
        public T_SysUser getnewuer(T_SysUser user)
        {
            T_SysUser newuser = new T_SysUser();
            newuser.Id = user.Id;
            newuser.range = user.range;
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
                    arrs=Array.ConvertAll<string, long>(strarrs, long.Parse);
                }
            }
            return arrs;
        }

        public string[] getcity(T_SysUser user)
        {
            string[] strarrs = new string[] { };
            if (user.range == 2)
            {
                if (user.departs != null)
                {
                    foreach (var mo in user.departs)
                    {
                        if (mo != null && mo.city != null)
                        {
                            strarrs = strarrs.Concat(mo.city.Split(",")).ToArray();
                        }
                    }
                }
            }
            if (user.city!=null)
            {
                strarrs = user.city.Split(",");
            }
            return strarrs;
        }
        public string[] getcellname(T_SysUser user)
        {
            string[] strarrs = new string[] { };
            if (user.range == 2)
            {
                if (user.departs != null)
                {
                    foreach (var mo in user.departs)
                    {
                        if (mo.cellname != null&&mo != null  )
                        {
                            if (mo.cellname.Split(",") != null)
                            {
                                strarrs = strarrs.Concat(mo.cellname.Split(",")).ToArray();
                            }
                            if (user.cellname != null)
                            {
                                if (user.cellname.Split(",") != null)
                                {
                                    strarrs = strarrs.Concat(user.cellname.Split(",")).ToArray();
                                }
                            }
                        }
                    }
                }
            }
            if (user.cellname != null)
            {
                strarrs = user.cellname.Split(",");
            }
            return strarrs;
        }
        public long[] getuserids(List<t_department> list,long userid)
        {
            long[] arrs = new long[] { userid };
            if (list != null)
            {
                foreach (var mo in list)
                {
                    if (mo != null && mo.userids != null)
                    {
                        arrs = arrs.Concat(Array.ConvertAll<string, long>(mo.userids.Split(","), long.Parse)).ToArray();
                    }
                }
            }
            return arrs;
        }

        public string GetToken()
        {
            string alltoken;
            var content = Request.Properties["MS_HttpContext"] as HttpContextBase;
            var token = content.Request.Form["zkaccess_token"];
            var token1 = content.Request.Headers["zkaccess_token"];
            if (token == null)
            {
                alltoken = token1;
            }
            else
            {
                alltoken = token;
            }
            return alltoken;
        }
        
        public string GetSysToken()
        {
            string alltoken;
            var content = Request.Properties["MS_HttpContext"] as HttpContextBase;
            var token = content.Request.Form["access_token"];
            var token1 = content.Request.Headers["access_token"];
            if (token == null)
            {
                alltoken = token1;
            }
            else
            {
                alltoken = token;
            }
            return alltoken;
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
        //得到部门列表
        public List<long> getdepart(string access_token)
        {
            RedisHtcs redis = new RedisHtcs();
            List<long> depart = redis.GetModel<List<long>>(access_token);
            return depart;
        }
        protected ElecUser GetelecUser(String token)
        {
            RedisHtcs redis = new RedisHtcs();
            ElecUser userDto = redis.GetModel<ElecUser>(token);
            return userDto;
        }
        public bool checkPression(T_SysUser user,string buttonname)
        {
            bool result = false;
            int count = 0;
            if (user.listpression != null)
            {
                count=user.listpression.Where(p => p.Code == buttonname && p.appcheck == 1).Count();
                if (count > 0)
                {
                    result = true;
                }
            }
            return result;
        }
        public T_SysUser getValue(string token)
        {
            
            T_SysUser user = new T_SysUser();
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                var json = decoder.Decode(token, secret, verify: true);
                user = JsonConvert.DeserializeObject<T_SysUser>(json);
            }
            catch (TokenExpiredException)
            {
                
            }
            catch (SignatureVerificationException)
            {
               
            }
            return user;
        }
        public async Task<SysResult<string>> uploadimage()
        {
            SysResult<string> result = new SysResult<string>();
            LogService lo = new LogService();
            try
            {
                lo.logInfo("开始上传图片");
                string image;
                // Check whether the POST operation is MultiPart?  
                if (!Request.Content.IsMimeMultipartContent())
                {
                    lo.logInfo("没有发现文件");
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                string fileSaveLocation = "D:\\Image";
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                await Request.Content.ReadAsMultipartAsync(provider);
                image = provider.filename1;
                FileName name = new FileName();
                name.filename = image;
                lo.logInfo("开始返回文件名");
                result.Code = 0;
                result.Message = "上传成功";
                result.numberData = image;
                //return Request.CreateResponse(HttpStatusCode.OK, image);
            }
            catch (System.Exception e)
            {
                lo.logInfo(e.ToString());
                result.Code = 1;
                result.Message = "上传异常";
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        protected void InitPage(PagingModel paging)
        {
            if (OrderablePagination != null)
            {
                InitPage(paging.PageSize, (paging.PageIndex - 1) * paging.PageSize);
            }
        }

        /// <summary>
        /// 保存数据之前初始化(lm)
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="model">实体对象</param>
        /// <param name="isAdd">是否是添加操作</param>
        //protected void SaveDataInit(string token, BasicModel model, bool isAdd)
        //{
        //    model.LastModifier = this.GetCurrentUser(token).UserCode;

        //    if (isAdd)
        //    {
        //        model.Creator = model.LastModifier;
        //    }
        //}
    }
   
}
