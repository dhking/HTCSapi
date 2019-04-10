
using Microsoft.Owin;
using Model;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{
    
    public class ValuesController : ApiController
    {
        [Route("api/webupload/uploadimage")]
        public SysResult<string> uploadimage()
        {
            SysResult<string> result = new SysResult<string>();
            string key = HttpContext.Current.Request["key"];
            string value = HttpContext.Current.Request["value"];
            HttpFileCollection files = HttpContext.Current.Request.Files;
            string s = ConfigurationManager.AppSettings["path"];
            string fileSaveLocation = s;
         
            if (!Directory.Exists(fileSaveLocation))
                Directory.CreateDirectory(fileSaveLocation);
            foreach (string f in files.AllKeys)
            {
                HttpPostedFile file = files[f];
                if (string.IsNullOrEmpty(file.FileName) == false) {
                    string newname = getNewname(file.FileName);
                    string filePath = Path.Combine(fileSaveLocation, newname);
                    file.SaveAs(filePath);
                    result.Code = 0;
                    result.Message = "上传成功";
                    result.numberData = newname;
                }      
            }
            return result;
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
