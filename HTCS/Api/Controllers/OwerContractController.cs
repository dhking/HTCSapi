using API.CommonControllers;
using DBHelp;
using Model;
using Model.Contrct;
using Model.TENANT;
using Model.User;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
namespace Api.Controllers
{
    public class OwerContractController : DataCenterController
    {
        OwerContractService service = new OwerContractService();
        [Route("api/OwerContract/SaveData")]
        public SysResult SaveHouse(T_OwernContrct model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("添加业主合同输入参数" + jsonData);
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult result = new SysResult();
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            ElecUser elecuser = GetelecUser("elec" + user.CompanyId);
            model.CreatePerson = user.RealName == null ? user.Mobile : user.RealName;
            model.CompanyId = user.CompanyId;
            return service.SaveContract(model,user.Id);
        }
        //合同列表查询
        [JurisdictionAuthorize(name = new string[] { "y-contract" })]
        [Route("api/OwerContract/Query")]
        [HttpPost]
        public SysResult<List<WrapOwernContract>> Query(WrapOwernContract model)
        {
            SysResult<List<WrapOwernContract>> sysresult = new SysResult<List<WrapOwernContract>>();
            T_SysUser user = GetCurrentUser(GetSysToken()); 
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            T_SysUser newuser = getnewuer(user);
            sysresult = service.Querymenufy(model, this.OrderablePagination, newuser);
            return sysresult;
        }
        //合同详情
        [Route("api/OwerContract/QuerybyId")]
        [HttpPost]
        public SysResult<WrapOwernContract> QuerybyId(WrapOwernContract model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("合同详情输入参数" + jsonData);
            SysResult<WrapOwernContract> sysresult = new SysResult<WrapOwernContract>();
            sysresult = service.QueryById(model);
            return sysresult;
        }
        //查询退租项
        [Route("api/OwerContract/Querytuizu")]
        [HttpPost]
        public SysResult<List<Inittuizu>> Querytuizu(tuzuReq model)
        {
            LogService log = new LogService();
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("查询退租项参数" + jsonData);
            return service.Querytuikuan(model);
        }
        //确认退租
        [Route("api/OwerContract/tuizu")]
        [HttpPost]
        public SysResult tuizu(TuizuZhu model)
        {
            LogService log = new LogService();
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("确认退租参数" + jsonData);
            return service.tuikuan(model);
        }
        //删除合同
        [Route("api/OwerContract/delete")]
        [HttpPost]
        public SysResult delete(T_OwernContrct model)
        {
            SysResult sysresult = new SysResult();
            sysresult = service.DeleteContract(model);
            return sysresult;
        }
        //续租
        [Route("api/OwerContract/xuzu")]
        [HttpPost]
        public SysResult xuzu(T_OwernContrct model)
        {
            LogService log = new LogService();
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("续租参数" + jsonData);
            return service.xuzu(model);
        }
        //查询是否同意退租
        [Route("api/OwerContract/istuizu")]
        [HttpPost]
        public SysResult istuizu(TuizuZhu model)
        {
            return service.istuikuan(model);
        }
    }
}