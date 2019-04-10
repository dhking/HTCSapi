using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Indexnew()
        {
            return View();
        }
        public JsonResult uploadimage(string orderid)
        {
            sysresult result = new Controllers.sysresult();
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
                        result.code = 0;
                        result.Msg = "上传成功";
                        result.Name = newname;
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {

                msg = ex.Message;

                result.code = 1;
                result.Msg = msg;
               
            }
            return Json(result);
        }
        public string getNewname(string name)
        {
            int startindex = name.IndexOf("."[0]);
            int length = name.Length-startindex;
            string extion = name.Substring(startindex, length);
            DateTime dt = DateTime.Now;
            return dt.ToString("yyyyMMddhhmmss") + System.Guid.NewGuid().ToString()+extion;
        }
    }
    public class sysresult
    {
        public int code { get; set; }
        public string Msg { get; set; }

        public string Name { get; set; }
    }
}