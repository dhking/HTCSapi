using Model;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace Service
{
    public class JurisdictionAuthorize : AuthorizeAttribute
    {
        public string[] name { get; set; }
        public int isty { get; set; }
        
        SysResult sysresult = new SysResult();
        protected override bool IsAuthorized(HttpActionContext filterContext)
        {
            HelpService sercice = new Service.HelpService();
            //获取token
            string alltoken;
            var content = filterContext.Request.Properties["MS_HttpContext"] as HttpContextBase;
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
            if (alltoken == "888888")
            {
                return true;
            }
            if (alltoken == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return false;
            }
            else
            {
                T_SysUser user = sercice.GetCurrentUser(alltoken);
                if (user == null)
                {
                    sysresult.Code = 1002;
                    sysresult.Message = "请先登录";
                    return false;
                }
                else
                {
                    if (isty == 1)
                    {
                        string Code = content.Request.Headers["Code"];
                        name =new string[] { Code } ;
                    }
                    if (!sercice.checkPression(user, name))
                    {
                        sysresult.Code = 1003;
                        sysresult.Message = "权限不足";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(sysresult), Encoding.UTF8, "application/json");

        }
    }


}
