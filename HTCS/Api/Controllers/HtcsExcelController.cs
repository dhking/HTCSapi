using API.CommonControllers;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Model.Contrct;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Configuration;
using Model.User;
using System.Web.Http.Cors;
using Api.CommonControllers;

namespace Api.Controllers
{
    // <summary>
    /// 允许跨域
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            //filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "content-type");
            base.OnActionExecuting(filterContext);
        }
    }
    public class AllowOriginAttribute
    {
        public static void onExcute(ControllerContext context, string[] AllowSites)
        {
            var origin = context.HttpContext.Request.Headers["Origin"];
            context.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "content-type");
            Action action = () =>
            {
                context.HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", origin);

            };
            if (AllowSites != null && AllowSites.Any())
            {
                if (AllowSites.Contains(origin))
                {
                    action();
                }
            }

        }
    }

    public class ActionAllowOriginAttribute : ActionFilterAttribute
    {
        public string[] AllowSites { get; set; }
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            AllowOriginAttribute.onExcute(filterContext, AllowSites);
            base.OnActionExecuting(filterContext);
        }
    }
    public class ControllerAllowOriginAttribute : AuthorizeAttribute
    {
        public string[] AllowSites { get; set; }
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            AllowOriginAttribute.onExcute(filterContext, AllowSites);
        }

    }

    //[ControllerAllowOrigin(AllowSites = new string[] { "http://localhost:89" })]
    [AllowCrossSiteJson]
    public class HtcsExcelController : baseController
    {
        HtcsExcelService sercice = new HtcsExcelService();
        //整租导出
        [JurisdictionAuthorize(name = new string[] { "zexcel" })]
        [Route("api/HtcsExcel/excel")]
        public ActionResult excel(Excel model)
        {
            HouseModel hmodel = JsonConvert.DeserializeObject<HouseModel>(model.search);
            byte[] bytes = sercice.excel(hmodel);
            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            return File(bytes, "application/vnd.ms-excel", strdate + "Excel.xls");
        }
       
        //合租导出
        [JurisdictionAuthorize(name = new string[] { "hexcel" })]
        [Route("api/HtcsExcel/hhouseexcel")]
        public ActionResult hhouseexcel(Excel model)
        {
            HouseModel hmodel = JsonConvert.DeserializeObject<HouseModel>(model.search);
            byte[] bytes = sercice.hhouseexcel(hmodel);
            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            return File(bytes, "application/vnd.ms-excel", strdate + "Excel.xls");
        }
        [JurisdictionAuthorize(name = new string[] { "dexcel" })]
        //独栋导出
        [Route("api/HtcsExcel/hhouseexcel")]
        [JurisdictionAuthorize(name = new string[] { "dexcel" })]
        public ActionResult dhouseexcel(Excel model)
        {
            HouseModel hmodel = JsonConvert.DeserializeObject<HouseModel>(model.search);
            byte[] bytes = sercice.dhouseexcel(hmodel,null,null);
            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            return File(bytes, "application/vnd.ms-excel", strdate + "Excel.xls");
        }
        //检查是否登录
        public ActionResult checklogin(Excel model)
        {
            SysResult sysresult = new SysResult();
            if (model.access_token == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return Json(sysresult, JsonRequestBehavior.AllowGet);
            }
            T_SysUser user = GetCurrentUser(model.access_token);
            if (user == null)
            {

                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return Json(sysresult, JsonRequestBehavior.AllowGet);
            }
            return Json(sysresult, JsonRequestBehavior.AllowGet);
        }
        [Route("api/HtcsExcel/contractexcel")]
        public ActionResult contractexcel(Excel model)
        {
            SysResult sysresult = new SysResult();
            if (model.access_token == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return Json(sysresult, JsonRequestBehavior.AllowGet);
            }
            T_SysUser user = GetCurrentUser(model.access_token);
            if (user == null)
            {

                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return Json(sysresult, JsonRequestBehavior.AllowGet);
            }
            ContrctService cservice = new Service.ContrctService();
            WrapContract hmodel = JsonConvert.DeserializeObject<WrapContract>(model.search);
            byte[] bytes = sercice.contractexcel(hmodel,user);
            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            return File(bytes, "application/vnd.ms-excel", strdate + "Excel.xls");
        }
        //业主合同导出
        [Route("api/HtcsExcel/ycontractexcel")]
        public ActionResult ycontractexcel(Excel model)
        {
            SysResult sysresult = new SysResult();
            if (model.access_token == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return Json(sysresult, JsonRequestBehavior.AllowGet);
            }
            T_SysUser user = GetCurrentUser(model.access_token);
            if (user == null)
            {

                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return Json(sysresult, JsonRequestBehavior.AllowGet);
            }
            WrapOwernContract hmodel = JsonConvert.DeserializeObject<WrapOwernContract>(model.search);
            byte[] bytes = sercice.ycontractexcel(hmodel,user);
            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            return File(bytes, "application/vnd.ms-excel", strdate + "Excel.xls");
        }
        [Route("api/File/Down")]
        public ActionResult DownloadFile(Excel model)
        {
            string path = ConfigurationManager.AppSettings["path"];
            string fileName = model.filename;
            string filePath = path+"/"+ model.filename;
            FileStream stream = new FileStream(filePath, FileMode.Open);
            byte[] imgByte = new byte[stream.Length];
            stream.Read(imgByte, 0, imgByte.Length);
            stream.Close();

            return File(imgByte,"application/octet-stream", fileName);
        }
        //下载word文件
        [Route("api/downcontract/word")]
        public ActionResult downword(Excel model)
        {
            byte[] bytes = sercice.downword(model.Id);
            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            return File(bytes, "application/vnd.ms-excel", strdate + "contract.docx");
        }
    }
}