using API.CommonControllers;
using Model;
using Model.User;
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
        [JurisdictionAuthorize(name = new string[] { "zsysuser-zhuanyi-bt" })]
        public SysResult zhuanyifgy(Pure model)
        {
            SysResult sysresult = new SysResult();
            string token = GetSysToken();
            T_SysUser user = GetCurrentUser(token);
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            return service.CmdProce3(model, user, token);
        }
        [Route("api/Procedure/distributiondepart")]
        
        public SysResult distributiondepart(Pure model)
        {
            model.Spname = "sp_distributiondepart";
            SysResult sysresult = new SysResult();
            string token = GetSysToken();
            T_SysUser user = GetCurrentUser(token);
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            return service.CmdProce3(model, user, token);
        }
        [Route("api/Procedure/signbill")]
        [JurisdictionAuthorize(name = new string[] { "signbill" })]
        public SysResult signbill(Pure model)
        {
            return service.CmdProce4(model);
        }
    }
}