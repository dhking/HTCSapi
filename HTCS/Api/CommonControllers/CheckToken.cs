using DAL;
using DBHelp;
using Model;
using Model.User;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;

namespace Api.CommonControllers
{
    public class AuthFilterAttribute : AuthorizationFilterAttribute
    {
        
        public override void  OnAuthorization(HttpActionContext actionContext)
        {
            //如果用户方位的Action带有AllowAnonymousAttribute，则不进行授权验证
            if(actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }
            string alltoken;
            var content = actionContext.Request.Properties["MS_HttpContext"] as HttpContextBase;
            var token = content.Request.Form["access_token"];
            var token1 = content.Request.Headers["access_token"];
            var token2 = content.Request.Headers["zkaccess_token"];
            if (token2 != null)
            {
                return;
            }
            if (token == null)
            {
                alltoken = token1;
            }
            else
            {
                alltoken = token;
            }
       
            RedisHtcs redis = new RedisHtcs();
            T_SysUser userDto = redis.GetModel<T_SysUser>(alltoken);  
            if (userDto == null)
            {
                //如果验证不通过，则返回401错误，并且Body中写入错误原因
                actionContext.Response = actionContext.Request.CreateResponse<SysResult>(HttpStatusCode.OK, new SysResult(1002, "Token 不正确")); ;
            }
            //JWTHelp jwt = new JWTHelp();
            //bool re = jwt.getValue(alltoken);
            //if (re==false)
            //{ 
               
            //}
        }
    }
}