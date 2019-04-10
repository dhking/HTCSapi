using DAL;
using DBHelp;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Api.Controllers
{
    public class ZerpController : Controller
    {
        LogService lo = new LogService();
        public ContentResult notify()
        {
            IDictionary<string, string> parameters = GetRequestPost();
            lo.logInfo("--回调--参数" + PuclicDataHelp.BuildQuery(parameters));

            try
            {
                SysResult result = new SysResult();
                string no = parameters["no"];
                SignService service = new SignService();
                result=service.completionContract(new SignVersion() { no = no });
                if (result.Code == 0)
                {
                    return Content("success");
                }
                else
                {
                    return Content("faile");
                }
            }
            catch (Exception ex)
            {
                lo.LogError("支付宝异步通知异常" + ex.ToString());
                return Content("faile");
            }
        }
        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }
}