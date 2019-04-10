using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            
            return View();
        }
        public ActionResult TypeAdd()
        {

            return View();
        }
        public ActionResult TypeEdit()
        {

            return View();
        }
        public ActionResult TEST()
        {

            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult IndexType()
        {
            return View();
        }
        public string GetapiUrl()
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["apiurl"];
        }
        public string GetimgUrl()
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["imgurl"];
        }
    }
}