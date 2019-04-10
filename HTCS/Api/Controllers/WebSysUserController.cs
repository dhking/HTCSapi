using DAL.Common;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Api.Controllers
{
    public class WebSysUserController : Controller
    {
        public ActionResult GetValidateCode()
        {
            ValImage vCode = new ValImage();
            string code = vCode.CreateValidateCode(4);
            Session["CheckCode"] = code;
            Response.SetCookie(new HttpCookie("CheckCode", code));
            byte[] bytes = vCode.CreateValidateGraphic(code, 16, 25, 16, 0x78000000);
            return File(bytes, @"image/jpeg");
        }
        public void ValidateCheckCode(string checkCode)
        {
            object checkCodeFromSession = Session["CheckCode"];
            checkCodeFromSession = checkCodeFromSession ?? Request.Cookies["CheckCode"].Value;
            string state = "\"state\":\"{0}\",\"msg\":\"{1}\"";
            string returnStr = string.Empty;

            if (string.IsNullOrWhiteSpace(checkCode))
            {
                returnStr = "{" + string.Format(state, "-100", "验证码错误") + "}";
            }
            else if (checkCodeFromSession == null)
            {
                returnStr = "{" + string.Format(state, "-200", "会话过期，无效验证") + "}";
            }
            else if (checkCodeFromSession.ToString().ToLower().Trim() == checkCode.ToLower().Trim())
            {
                returnStr = "{" + string.Format(state, "0", "有效验证") + "}";
            }
            else
            {
                returnStr = "{" + string.Format(state, "-100", "验证码错误") + "}";
            }

            Response.Write(returnStr);
            Response.End();
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult uploadimage(string orderid)
        {
            SysResult<string> result = new SysResult<string>();
            int count = 0;
            string msg = "";
            string fileName = "";
            string filePath = "";
            try
            {
                string directoryPath = System.Web.Hosting.HostingEnvironment.MapPath("/FILE/");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                foreach (string f in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[f];
                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = file.FileName;
                        string newname = getNewname(file.FileName);
                        filePath = Path.Combine(directoryPath, newname);
                        file.SaveAs(filePath);
                        result.Code = 0;
                        result.Message = "上传成功";
                        result.numberData = newname;
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {

                msg = ex.Message;

                result.Code = 1;
                result.Message = msg;

            }
            return Json(result);
        }
        public string getNewname(string name)
        {
            int startindex = name.IndexOf("."[0]);
            int length = name.Length - startindex;
            string extion = name.Substring(startindex, length);
            DateTime dt = DateTime.Now;
            return dt.ToString("yyyyMMddhhmmss") + System.Guid.NewGuid().ToString() + extion;
        }
    }
}