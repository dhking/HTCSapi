using ControllerHelper.Attrbute;
using DBHelp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ControllerHelper
{
    /// <summary>
    /// 用户令牌验证
    /// </summary>
    public class TokenProjectorAttribute : ActionFilterAttribute
    {
        private const string UserToken = "token";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // 匿名访问验证
            var anonymousAction = actionContext.ActionDescriptor.GetCustomAttributes<AnonymousAttribute>();
            if (!anonymousAction.Any())
            {
                // 验证token
                var token = TokenVerification(actionContext);
            }

            GetData(actionContext);

            base.OnActionExecuting(actionContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

        /// <summary>
        /// 身份令牌验证
        /// </summary>
        /// <param name="actionContext"></param>
        protected virtual string TokenVerification(HttpActionContext actionContext)
        {
            // 获取token
            if (!actionContext.ActionArguments.ContainsKey(UserToken))
            {
                if (actionContext.ControllerContext != null
                    && actionContext.ControllerContext.RouteData != null
                    && actionContext.ControllerContext.RouteData.Values != null)
                {
                    bool bl = actionContext.ControllerContext.RouteData.Values.ContainsKey(UserToken);
                    if (bl == true)
                    {
                        var tokenVal = actionContext.ControllerContext.RouteData.Values[UserToken];
                        actionContext.ActionArguments.Add(UserToken, tokenVal);
                    }
                }
            }
            var token = GetToken(actionContext.ActionArguments, actionContext.Request.Method);

            ////Test
            //if (!string.IsNullOrWhiteSpace(token))
            //{
            //    return token;
            //}

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new IsExitException("Token无效，请重新登陆");
            }

            if (!Authorize(token))
            {
                throw new IsExitException("Token已失效，请重新登陆!");
            }

            return token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionArguments"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetToken(Dictionary<string, object> actionArguments, HttpMethod type)
        {
            var token = "";

            if (type == HttpMethod.Post)
            {
                foreach (var value in actionArguments.Values)
                {
                    if (value == null)
                    {
                        continue;
                    }
                    token = value.GetType().GetProperty(UserToken) == null
                        ? GetToken(actionArguments, HttpMethod.Get)
                        : value.GetType().GetProperty(UserToken).GetValue(value).ToString();
                }
            }
            else if (type == HttpMethod.Get)
            {
                if (!actionArguments.ContainsKey(UserToken))
                {
                    throw new Exception("未附带token!");
                }

                if (actionArguments[UserToken] != null)
                {
                    token = actionArguments[UserToken].ToString();
                }
                else
                {
                    throw new Exception("token不能为空!");
                }
            }
            else
            {
                throw new Exception("暂未开放其它访问方式!");
            }

            return token;
        }

        protected async void GetData(HttpActionContext actionContext)
        {
            for (int i = 0; i < actionContext.ActionArguments.Count; i++)
            {
                var item = actionContext.ActionArguments.ElementAt(i);

                if (item.Value == null)
                {
                    Stream v = await actionContext.Request.Content.ReadAsStreamAsync();
                    actionContext.ActionArguments[item.Key] = v;
                }
            }
        }

        /// <summary>
        /// Action 访问权限验证
        /// </summary>
        /// <param name="token">身份令牌</param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected virtual void AuthorizeCore(string token, HttpActionContext actionContext)
        {
            // 权限控制Action验证
            var moduleAuthorizationAction = actionContext.ActionDescriptor.GetCustomAttributes<ModuleAuthorizationAttribute>();
            if (moduleAuthorizationAction.Any())
            {
                var userRole = AccountHelper.GetUserType(token);
                if (!moduleAuthorizationAction[0].Authorizations.Contains(userRole.ToString()))
                {
                    throw new Exception("用户非法跨权限访问，token：" + token);
                }
            }
        }


        /// <summary>
        /// 访问权限验证
        /// </summary>
        /// <param name="token">身份令牌</param>
        /// <returns></returns>
        protected virtual bool Authorize(string token)
        {
            var user = CouchbaseHelper.CouchbaseInstance.GetValue(token);
            if (user == null)
            {
                String reason = (String)CouchbaseHelper.CouchbaseInstance
                        .GetValue<string>(CommonEnums.CouchBaseKey.LOGSTATUS.ToString() + token);

                if (reason != null && reason == "")
                {
                    reason = "当前用户已退出";
                }
                else if (reason == null)
                {
                    //res.setStatus(HttpServletResponse.SC_NOT_FOUND);

                    return false;
                }

                //bool flag = true;
                //try
                //{
                //    String[] reasons = reason.Split(':');
                //    reason = reasons[0];
                //    if (reasons[1] == CommonEnums.Scope.HAND_HOLD.ToString())
                //    {// 安卓的不压缩
                //        flag = false;
                //    }
                //}
                //catch (Exception e)
                //{
                //}

                //byte[] result = WriteResponseUtil.returnResult(CommonEnums.ReturnStatus.LOGOUT, reason, flag);

                CouchbaseHelper.CouchbaseInstance.RemoveValue(CommonEnums.CouchBaseKey.LOGSTATUS.ToString() + token);
            }
            return true;
        }
    }
}
