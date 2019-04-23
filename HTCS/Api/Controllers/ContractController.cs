using API.CommonControllers;
using DAL.Common;
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
    public class ContractController :DataCenterController
    {
        ContrctService service = new ContrctService();
        //测试生成word
        [Route("api/Contract/Create")]
        [HttpPost]
        public SysResult Create(T_Contrct model)
        {
            SysResult sysresult = new SysResult();
            CreateWord dal =  new CreateWord();
            //dal.SaveAsWord(@"D:\Contract\contract_615.docx");
            dal.SaveAsWord(@"<h2 style='text-align:left;'>你好</ h2 >我是企业", @"D:\Contract\1.doc");
            return sysresult;
        }
        [Route("api/Contract/SaveData")]
        [JurisdictionAuthorize(name = new string[] { "zcontract-add-btn", "menu-edit-btn" })]
        public SysResult SaveHouse(T_Contrct model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("添加合同输入参数" + jsonData);
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult result = new SysResult();
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            ElecUser elecuser = GetelecUser("elec" + user.CompanyId);
            model.CreatePerson = user.RealName==null ? user.Mobile : user.RealName;
            model.userid = user.Id;
            model.CompanyId = user.CompanyId;
            T_SysUser newuser = getnewuer(user);
            return service.SaveContract(model, elecuser, newuser);
        }
       
        [Route("api/Contract/GetEnum")]
        [HttpPost]
        public SysResult<WrapEnum> GetEnum()
        {
            SysResult<WrapEnum> result = new SysResult<WrapEnum>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
            
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            return service.Query(user.CompanyId);
        }
        //合同列表查询
        [JurisdictionAuthorize(name = new string[] { "z-contract" })]
        [Route("api/Contract/Query")]
        [HttpPost]
        public SysResult<List<WrapContract>> Query(WrapContract model)
        {
            SysResult<List<WrapContract>> sysresult = new SysResult<List<WrapContract>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            T_SysUser newuser = getnewuer(user);
            model.arrCellNames = getcityorarea(model.CellNames);
            sysresult = service.Querymenufy(model,this.OrderablePagination,newuser);
            return sysresult;
        }
        //待退租
        [Route("api/Contract/tuizuQuery")]
        [HttpPost]
        public SysResult<List<WrapContract>> tuizuQuery(WrapContract model)
        {
            SysResult<List<WrapContract>> sysresult = new SysResult<List<WrapContract>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Querytuizu(model, this.OrderablePagination);
            return sysresult;
        }
        //带抄表查询
        [JurisdictionAuthorize(name = new string[] { "chaobiao" })]
        [Route("api/chaobiao/Query")]
        [HttpPost]
        public SysResult<List<chaobiao>> chaobiaoQuery(chaobiao model)
        {
            SysResult<List<chaobiao>> sysresult = new SysResult<List<chaobiao>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
        
            sysresult = service.chaobiaoquery(model, this.OrderablePagination);
            return sysresult;
        }
        //抄表详情查询
        [Route("api/chaobiao/Queryxq")]
        [HttpPost]
        public SysResult<chaobiao> chaobiaoQueryxq(chaobiao model)
        {
            SysResult<chaobiao> sysresult = new SysResult<chaobiao>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.chaobiaoqueryxq(model);
            return sysresult;
        }
        //添加抄表账单
        [Route("api/chaobiao/addbill")]
        [HttpPost]
        public SysResult chaobiaoaddbill(chaobiao model)
        {
            SysResult sysresult = new SysResult();
            sysresult = service.addbill(model);
            return sysresult;
        }
        //删除合同
        [Route("api/Contract/delete")]
        [HttpPost]
        public SysResult delete(T_Contrct model)
        {
            SysResult sysresult = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.userid = user.Id;
            sysresult = service.DeleteContract(model);
            return sysresult;
        }
        [Route("api/Contract/qianyue")]
        [HttpPost]
        public SysResult<WrapContract> qianyue(T_Contrct model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("签约入参" + jsonData);
            SysResult<WrapContract> sysresult = new SysResult<WrapContract>();
            sysresult = service.qianyue(model);
            return sysresult;
        }

        [Route("api/Contract/QuerybyId")]
        [HttpPost]
        public SysResult<WrapContract> QuerybyId(WrapContract model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("合同详情输入参数" + jsonData);
            SysResult<WrapContract> sysresult = new SysResult<WrapContract>();
            sysresult = service.QueryById(model);
            return sysresult;
        }
        //查询退租项
        [Route("api/Contract/Querytuizu")]
        [HttpPost]
        public SysResult<List<Inittuizu>> Querytuizu(tuzuReq model)
        {
            LogService log = new LogService();
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("查询退租项参数" + jsonData);
            return service.Querytuikuan(model);
        }
        //确认退租
        [Route("api/Contract/tuizu")]
        [HttpPost]
        public SysResult tuizu(TuizuZhu model)
        {
            LogService log = new LogService();
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("确认退租参数" + jsonData);
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult result = new SysResult();
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            ElecUser elecuser = GetelecUser("elec" + user.CompanyId);
            return service.tuikuan(model, elecuser, user);
        }
        //退租租客端
        [Route("api/Contract/zktuizu")]
        [HttpPost]
        public SysResult zktuizu(TuizuZhu model)
        {
            LogService log = new LogService();
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("租客端确认退租参数" + jsonData);

            return service.zktuikuan(model);
        }

        //查询退租详情
        [Route("api/tuizu/Query")]
        [HttpPost]
        public SysResult<TuizuZhu> tuizuQuery(TuizuZhu model)
        {
            return service.tuizuQuery(model);
        }
        //查询是否同意退租
        [Route("api/Contract/istuizu")]
        [HttpPost]
        public SysResult istuizu(TuizuZhu model)
        {
            return service.istuikuan(model);
        }
        //续租
        [Route("api/Contract/xuzu")]
        [HttpPost]
        public SysResult xuzu(T_Contrct model)
        {
            LogService log = new LogService();
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("续租参数" + jsonData);
            return service.xuzu(model);
        }
        //根据房源查询在租合同
        [Route("api/Contract/houseQuerybyId")]
        [HttpPost]
        public SysResult<WrapContract> houseQuerybyId(WrapContract model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            model.arrStatus =new int[] { 1,2, 4, 5, 6,9 };
            SysResult<WrapContract> sysresult = new SysResult<WrapContract>();
            sysresult = service.QueryById(model);
            return sysresult;
        }
        //根据房源查询历史合同
        [Route("api/Contract/houseQuery")]
        [HttpPost]
        public SysResult<List<WrapContract>> houseQuery(WrapContract model)
        {
            SysResult<List<WrapContract>> sysresult = new SysResult<List<WrapContract>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            model.arrStatus = new int[] { 7 };
            sysresult = service.Querymenufy(model,this.OrderablePagination,null);
            return sysresult;
        }
        //修改手机号
      
        [Route("api/Teant/UpdatePhone")]
        [HttpPost]
        public SysResult UpdatePhone(T_Teant model)
        {
            SysResult sysresult = new SysResult();
            sysresult = service.UpdatePhone(model);
            return sysresult;
        }
        //根据手机号查询名下的合同
        [Route("api/Contract/querybyuser")]
        [HttpPost]
        public SysResult<List<WrapContract>> querybyuser(WrapContract model)
        {
            if (model.Status != 7)
            {
                model.arrStatus = new int[] { 1,2, 4, 5, 6,9};
            }
            else
            {
                model.arrStatus = new int[] { 7 };
            }
            return service.nopagequery(model);
        }
        //根据手机号查询认证信息
        [Route("api/Teant/Query")]
        [HttpPost]
        public SysResult<T_Teant> TeantQuery(T_Teant model)
        {
            return service.queryteant(model);
        }
        
    }
}