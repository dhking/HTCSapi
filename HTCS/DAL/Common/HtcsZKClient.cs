using Model;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DAL.Common
{
    public  class HtcsZKClient
    {
        private string token = "123456789";
        private string serverurl = ConfigurationManager.AppSettings["url"];

        private WebUtils webUtils;
        public HtcsZKClient(string api)
        {
            serverurl = serverurl + api;
            this.webUtils = new WebUtils();
        }
        public void SetTimeout(int timeout)
        {
            webUtils.Timeout = timeout;
        }
        public string getSign()
        {
            return token;
        }
        public SysResult<List<T>> DoExecute<T>(T request, string token = "")
        {
            SysResult<List<T>> sys = new SysResult<List<T>>();
            string param = JsonConvert.SerializeObject(request);
            string body = webUtils.DoPost(this.serverurl, param, token);
            sys = JsonConvert.DeserializeObject<SysResult<List<T>>>(body);
            return sys;
        }
        public SysResult<T> DoExecute1<T>(T request)
        {
            SysResult<T> sys = new SysResult<T>();
            string param = JsonConvert.SerializeObject(request);
            string body = webUtils.DoPost(this.serverurl, param);
            sys = JsonConvert.DeserializeObject<SysResult<T>>(body);
            return sys;
        }
        public SysResult DoExecute2<T>(T request)
        {
            SysResult sys = new SysResult();
            string param = JsonConvert.SerializeObject(request);
            string body = webUtils.DoPost(this.serverurl, param);
            sys = JsonConvert.DeserializeObject<SysResult>(body);
            return sys;
        }
    }
}
