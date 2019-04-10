using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ControllerHelper
{
    /// <summary>
    /// 控制器抽象基类
    /// </summary>
    public abstract class RcsController : ApiController
    {
        private OrderablePagination _orderablePagination = new OrderablePagination() { PageSize = 20 };
        private string _errorMessage = "";
        private bool _isSuccess = true;

        /// <summary>
        /// 
        /// </summary>
        public OrderablePagination OrderablePagination
        {
            get
            {
                return _orderablePagination;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return _isSuccess;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public RcsController()
        {
            //var fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            //foreach (var item in fields)
            //{
            //    AutowiredAttribute at = Attribute.GetCustomAttribute(item, typeof(AutowiredAttribute)) as AutowiredAttribute;
            //    if (at != null)
            //    {
            //        Assembly asm = null;
            //        string typeName = "";
            //        if (string.IsNullOrWhiteSpace(at.FullPathName))
            //        {
            //            asm = item.FieldType.Assembly;
            //        }
            //        else
            //        {
            //            asm = Assembly.Load(at.FullPathName);
            //        }

            //        if (string.IsNullOrWhiteSpace(at.Namespace))
            //        {
            //            typeName = item.FieldType.Namespace;
            //        }
            //        else
            //        {
            //            typeName = at.Namespace;
            //        }

            //        if (string.IsNullOrWhiteSpace(at.Name))
            //        {
            //            typeName += "." + item.FieldType.Name.Substring(1);
            //        }
            //        else
            //        {
            //            typeName += "." + at.Name;
            //        }

            //        var serviceImpl = asm.CreateInstance(typeName);

            //        if (serviceImpl == null)
            //        {
            //            throw new ArgumentNullException("serviceImpl");
            //        }

            //        item.SetValue(this, serviceImpl);
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        //protected override void Initialize(HttpControllerContext controllerContext)
        //{
        //    base.Initialize(controllerContext);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        //{
        //    return base.ExecuteAsync(controllerContext, cancellationToken);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        protected void SetOrderablePagination(OrderablePagination pagination)
        {
            _orderablePagination = pagination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="start"></param>
        protected void InitPage(int limit, int start)
        {
            
            if (_orderablePagination != null)
            {
                _orderablePagination.PageSize = limit;
                _orderablePagination.StartIndex = start - limit;
                _orderablePagination.TotalCount = 0;
            }
        }

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T ReturnFail<T>()
        {
            return this.ReturnFail<T>("访问服务器失败");
        }

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T ReturnFail<T>(string errMsg)
        {
            _errorMessage = errMsg;

            _isSuccess = false;
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errMsg"></param>
        public void Fail(string errMsg)
        {
            _isSuccess = false;
            _errorMessage = errMsg;
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="path">文件存放相对路径</param>
        /// <returns>返回上传的文件名已经服务器存放的文件名（包括相对地址）</returns>
        //protected async Task<Dictionary<string, string>> FileUpload(string path = "")
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    string root = HttpContext.Current.Server.MapPath("~/App_Data/" + path + "/");//指定要将文件存入的服务器物理位置
        //    if (!Directory.Exists(root))
        //    {
        //        try
        //        {
        //            Directory.CreateDirectory(root);
        //        }
        //        catch
        //        {

        //        }
        //    }
        //    var provider = new MultipartFormDataStreamProvider(root);
        //    try
        //    {
        //        // Read the form data.
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        // This illustrates how to get the file names.
        //        foreach (MultipartFileData file in provider.FileData)
        //        {
        //            //接收文件
        //            //Trace.WriteLine(file.Headers.ContentDisposition.FileName);//获取上传文件实际的文件名
        //            //Trace.WriteLine("Server file path: " + file.LocalFileName);//获取上传文件在服务上默认的文件名
        //            dic.Add(file.Headers.ContentDisposition.FileName, Path.GetFileName(file.LocalFileName));

        //        }
        //        string str = "";
        //        foreach (var key in provider.FormData.AllKeys)
        //        {
        //            if (key == "FileName")
        //            {
        //                str = provider.FormData[key];
        //            }
        //            //接收FormData
        //            dic.Add(key, provider.FormData[key]);
        //        }
        //        ////这样做直接就将文件存到了指定目录下，暂时不知道如何实现只接收文件数据流但并不保存至服务器的目录下
        //        //foreach (var key in provider.FormData.AllKeys)
        //        //{
        //        //    //接收FormData
        //        //    dic.Add(key, provider.FormData[key]);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return dic;
        //}


        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="path">文件存放相对路径</param>
        /// <returns></returns>
        //protected async Task<HttpResponseMessage> FileDown(string path = "")
        //{
        //    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    string root =Current.Server.MapPath("~/App_Data/" + path + "/");//指定要将文件存入的服务器物理位置

        //    var provider = new MultipartFormDataStreamProvider(root);
        //    try
        //    {
        //        // Read the form data.
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        //// This illustrates how to get the file names.
        //        //foreach (MultipartFileData file in provider.FileData)
        //        //{
        //        //    //接收文件
        //        //    //Trace.WriteLine(file.Headers.ContentDisposition.FileName);//获取上传文件实际的文件名
        //        //    //Trace.WriteLine("Server file path: " + file.LocalFileName);//获取上传文件在服务上默认的文件名
        //        //    dic.Add(file.Headers.ContentDisposition.FileName, path + Path.GetFileName(file.LocalFileName));
        //        //}

        //        //这样做直接就将文件存到了指定目录下，暂时不知道如何实现只接收文件数据流但并不保存至服务器的目录下
        //        string fileName = "";
        //        foreach (var key in provider.FormData.AllKeys)
        //        {
        //            if (key == "FileName")
        //            {
        //                fileName = provider.FormData[key];
        //            }
        //            //接收FormData
        //            dic.Add(key, provider.FormData[key]);
        //        }

        //        MemoryStream ms = new MemoryStream(File.ReadAllBytes(root + fileName));
        //        result.Content = new StreamContent(ms);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }

    /// <summary>
    /// 普通控制器基类
    /// </summary>
    //[TokenProjector]
    public class BaseController : RcsController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        //protected override void Initialize(HttpControllerContext controllerContext)
        //{
        //    base.Initialize(controllerContext);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        //{
        //    return base.ExecuteAsync(controllerContext, cancellationToken);
        //}
    }

    /// <summary>
    /// 登录控制器基类
    /// </summary>
    public class LoginBaseController : RcsController
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            return base.ExecuteAsync(controllerContext, cancellationToken);
        }
    }
}
