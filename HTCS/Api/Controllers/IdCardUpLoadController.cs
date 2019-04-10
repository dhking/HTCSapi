using API.CommonControllers;
using DAL;
using DBHelp;

using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class IdCardUpLoadController :  DataCenterController
    {
        LogService lo = new LogService();
        ThirdDAL dal = new ThirdDAL();
        //多个图片上传
        [Route("api/idcardupload/image")]
        public async Task<SysResult<returnidcard>> image()
        {
            SysResult<returnidcard> result = new SysResult<returnidcard>();
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
                string fileSaveLocation = "D:\\Image";
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);

                await Request.Content.ReadAsMultipartAsync(provider);
                image = provider.filename1;
                FileName name = new FileName();
                name.filename = image;
                string reimage = image;
                lo.logInfo("开始返回文件名");
                result.Code = 0;
                //调用身份证识别正面
                string token = "";
                string path = "D://Image/";
                token = dal.GetbaiduToken();
                image = image.Substring(0, image.Length - 1);
                
                lo.logInfo("文件名称"+ image);
                image = ConvertHelper.ImgToBase64String(path + image);
                string re = dal.idcard(image, token, "front");
                returnidcard idmodel = JsonConvert.DeserializeObject<returnidcard>(re);
                idmodel.imagepath = reimage;
                if (idmodel.image_status != "normal")
                {
                    string statusz = geterr(idmodel.image_status);
                    result.Message = "识别失败"+ statusz;
                    result.Code = 1;
                    return result;
                }
                result.Message = "识别成功";
                result.numberData = idmodel;
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
        public string geterr(string str)
        {
            switch (str)
            {
                case "normal":
                    return "识别正常";
                case "reversed_side":
                    return "身份证正反面颠倒";
                case "non_idcard":
                    return "上传的图片中不包含身份证";
                case "blurred":
                    return "身份证模糊";
                case "other_type_card":
                    return "其他类型证照";
                case "over_exposure":
                    return "图片不清晰,关键字段反光或过曝";
                case "unknown":
                    return "未知状态";
                default:
                    return "未知";
            }

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

   
}
