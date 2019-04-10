using API.CommonControllers;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{

    public class ProcedureController : DataCenterController
    {
        ProceService service = new ProceService();
        [Route("api/Procedure/CmdProce")]
        public SysResult CmdProce(Pure model)
        {
            return service.CmdProce(model);
        }
        [Route("api/Procedure/shenhe")]
        public SysResult shenhe(Pure model)
        {
            return service.CmdProce2(model);
        }
        [Route("api/Procedure/zhuanyifgy")]
        public SysResult zhuanyifgy(Pure model)
        {
            return service.CmdProce3(model);
        }
        [Route("api/Procedure/signbill")]
        public SysResult signbill(Pure model)
        {
            return service.CmdProce4(model);
        }
    }
}