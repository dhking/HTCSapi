using API.CommonControllers;
using DBHelp;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{
    
    public class uploadController : DataCenterController
    {

        LogService lo = new LogService();
        //多个图片上传
        [Route("api/upload/image")]
        public async Task<SysResult<string>> image()
        {
            SysResult<string> result = new SysResult<string>();
            try
            {
                lo.logInfo("开始上传图片");
                string image;
                // Check whether the POST operation is MultiPart?  
                if (!Request.Content.IsMimeMultipartContent())
                {
                    lo.logInfo("没有发现文件");
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                string s = ConfigurationManager.AppSettings["path"]; 
                string fileSaveLocation =s;
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
               
                await Request.Content.ReadAsMultipartAsync(provider);
                image = provider.filename1;
                FileName name = new FileName();
                name.filename = image;
                lo.logInfo("开始返回文件名");
                result.Code = 0;
                result.Message = "上传成功";
                result.numberData = image;
                //return Request.CreateResponse(HttpStatusCode.OK, image);
            }
            catch (System.Exception e)
            {
                lo.logInfo(e.ToString());
                result.Code = 1;
                result.Message = "上传异常";
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            return result;
        }
        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            public string filename1 { get; set; }
            public CustomMultipartFormDataStreamProvider(string path) : base(path)
            {

            }

            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                LogService lo = new LogService();
                string newFileName = string.Empty;
                newFileName = Path.GetExtension(headers.ContentDisposition.FileName.Replace("\"", string.Empty));//获取后缀名  
                newFileName = uploadhelp.buildFileName(newFileName);
                filename1 += newFileName + ";";
                lo.LogError(filename1);
                return newFileName;
            }
        }
        public static class uploadhelp
        {
            public static string url = "http://139.224.196.42:81/";
            public static string substring(string a)
            {
                string[] str = a.Split('\\'); //根据特定符号截取为字符串数组；  
                string temp = str[str.Length - 1];
                temp = url + temp;
                return temp;
            }
            public static string buildFileName(string extion)
            {
                //new一个时间对象date  
                DateTime dt = DateTime.Now;
                return dt.ToString("yyyyMMddhhmmss") + System.Guid.NewGuid().ToString() + extion;
            }
        }
    }

    public class reimage
    {
        public string type { get; set; }
        public string filename { get; set; }
    }
}