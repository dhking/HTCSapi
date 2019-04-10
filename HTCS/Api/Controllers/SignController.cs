using Api.CommonControllers;
using API.CommonControllers;
using DAL;
using DBHelp;
using Model;
using Model.Base;
using Model.Bill;
using Model.Contrct;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Web.Http;


namespace Api.Controllers
{
    public class SignController : DataCenterController
    {
        SignService service = new SignService();
        

        //创建合同
        [HttpPost]
        [Route("api/Sign/createForWord")]
        public SysResult createForWord(SignVersion model)
        {
            return service.createForWord( model);
        }
        //个人证书
        [HttpPost]
        [Route("api/Sign/personReg")]
        public SysResult personReg(SignVersion model)
        {
            return service.personReg(model);
        }
        //签署合同
        [HttpPost]
        [Route("api/Sign/signAuto")]
        public SysResult<WrapContract> signAuto(SignVersion model)
        {
            return service.signAuto(model);
        }
        //合同操作
        [HttpPost]
        [Route("api/Sign/completionContract")]
        public SysResult completionContract(SignVersion model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("合同操作" + jsonData);
            return service.completionContract(model);
        }
        //修改印章
        [HttpPost]
        [Route("api/Sign/signatureChange")]
        public SysResult signatureChange()
        {
            return service.signatureChange();
        }
    }
}