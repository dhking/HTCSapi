using DAL.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class SysUserController : Controller
    {
        // GET: SysUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ButtonAdd()
        {
            return View();
        }
        public ActionResult ButtonEdit()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Permissions()
        {
            return View();
        }
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
    }
}