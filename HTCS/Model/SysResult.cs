using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class LayResult<T>
    {
        public int code { get; set; }
        public string msg { get; set; }
        public T data { get; set; }
    }

    public  class SysResult<T>
    {
        public long numberCount { get; set; }

        public T numberData { get; set; }

        public int Code { get; set; }

        public string Message { get; set; }

        public string other { get; set; }
        public string other1 { get; set; }

        public SysResult()
        {
           
        }
        public SysResult(int code,string message)
        {
            this.Code = code;
            this.Message = message;
        }
     

    }
    public class weilfanhui
    {

    }
    public class SysResult
    {
        public int Code { get; set; }
        public weilfanhui numberData { get; set; }
        public string Message { get; set; }
        public SysResult()
        {
            numberData = new Model.weilfanhui();
        }

        public SysResult(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
        public SysResult FailResult(string message)
        {
            return new SysResult(1, message);
        }

        public SysResult SuccessResult(string message)
        {
            return new SysResult(0, message);
        }
        /// <summary>
        /// 系统异常Exception
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <param name="ce"></param>
        /// <returns></returns>
        public SysResult ExceptionResult(string message, Exception ce)
        {
            return new SysResult(-1, message+ ce.ToString());

        }

        public static implicit operator HttpContent(SysResult v)
        {
            throw new NotImplementedException();
        }
    }

}
