using Api.CommonControllers;
using API.CommonControllers;
using ControllerHelper;
using DAL;
using DBHelp;
using Microsoft.Owin;
using Model;
using Model.Base;
using Model.Bill;
using Model.House;
using Model.TENANT;
using Model.User;
using Newtonsoft.Json;
using Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    //[AuthFilter]
    public class RepaIreController : DataCenterController
    {
        RepairedService service = new RepairedService();
        LogService log = new LogService();
   
        //分页查询
        [Route("api/Repaire/Querylist")]
       
        public SysResult<List<WrapRepaire>> Querylist(Repaire model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("分页查询" + jsonData);
            SysResult<List<WrapRepaire>> sysresult = new SysResult<List<WrapRepaire>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            //model.storeid = user.storeid;

            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Query(model, this.OrderablePagination);
            return sysresult;
        }
        [Route("api/zkRepaire/Querylist")]
        public SysResult<List<WrapRepaire>> zkRepaire(Repaire model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("分页查询" + jsonData);
            SysResult<List<WrapRepaire>> sysresult = new SysResult<List<WrapRepaire>>();
           
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Query(model, this.OrderablePagination);
            return sysresult;
        }
        //报修详情
        [Route("api/Repaire/Queryxq")]
        public SysResult<WrapRepaire> Queryxq(Guest model)
        {
            SysResult<WrapRepaire> sysresult = new SysResult<WrapRepaire>();

            sysresult = service.Queryxq(model);
            return sysresult;
        }
        //报修项目列表
        [Route("api/Repaire/RepairList")]
        public SysResult<List<RepairList>> RepairList(RepairList model)
        {
            SysResult<List<RepairList>> sysresult = new SysResult<List<RepairList>>();
            sysresult = service.Queryrepairlist(model);
            return sysresult;
        }
        //报修列表展示后台
        [JurisdictionAuthorize(name = new string[] { "repaire/" })]
        [Route("api/Repaire/hRepairList")]
        public SysResult<List<RepairList>> hRepairList(RepairList model)
        {
            
            SysResult<List<RepairList>> sysresult = new SysResult<List<RepairList>>();
            sysresult = service.Queryhrepairlist(model);
            return sysresult;
        }
        //保存报修
        [HttpPost]
        [Route("api/Repaire/save")]
        public SysResult save(Repaire bill)
        {
            string jsonData = JsonConvert.SerializeObject(bill);
            log.logInfo("保存报修" + jsonData);
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult result = new SysResult();
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            bill.CompanyId =user.CompanyId;
            return service.save(bill);
        }
        [HttpPost]
        [Route("api/zkRepaire/save")]
        public SysResult zksave(Repaire bill)
        {
            string jsonData = JsonConvert.SerializeObject(bill);
            log.logInfo("保存报修" + jsonData);
            return service.save(bill);
        }
        //用户报修
        [HttpPost]
        [Route("api/Repaire/usersave")]
        public SysResult usersave(Repaire model)
        {
            ContrctDAL contractdal = new ContrctDAL();
            T_Teant teant= contractdal.queryteant(new T_Teant() { Phone = model.Phone });
            if (teant != null)
            {
                model.JournaList = teant.Name;
            }
            return service.save(model);
        }
        //接单
        [HttpPost]
        [Route("api/Repaire/Receipt")]
        public SysResult Receipt(RepairList bill)
        {
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult result = new SysResult();
                result.Code = 1;
                result.Message = "接单对象不能为空";
                return result;
            }
            bill.UserId = user.Id;
            return service.Receipt(bill);
        }
        //完成
        [HttpPost]
        [Route("api/Repaire/End")]
        public SysResult End(RepairList bill)
        {
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult result = new SysResult();
                result.Code = 2;
                result.Message = "完成人不能为空";
                return result;
            }
            bill.UserId = user.Id;
            return service.end(bill);
        }
        //删除报修
        [HttpPost]
        [Route("api/Repaire/delete")]
        public SysResult delete(Repaire model)
        {
            return service.delete(model);
        }
        //我的报修列表
        [HttpPost]
        [Route("api/Repaire/MyReceipt")]
        public SysResult<List<WrapRepaire>> MyReceipt(Repaire model)
        {
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult<List<WrapRepaire>> result = new SysResult<List<WrapRepaire>>();
                result.Code = 2;
                result.Message = "接单对象不能为空";
                return result;
            }
            model.UserId = user.Id;
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            return service.Query(model, this.OrderablePagination);
            
        }
        [HttpPost]
        [Route("api/Repaire/subject")]
        public SysResult<List<T_Basics>> subject()
        {
            return service.subject();
        }
        [HttpPost]
        [Route("api/Repaire/shaixuan")]
        public SysResult<WrapShaixuan> shaixuan(WrapCell model)
        {
            return service.shaixuan(model);
        }
    }
}