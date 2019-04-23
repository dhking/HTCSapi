using Api.CommonControllers;
using API.CommonControllers;
using DBHelp;
using Model;
using Model.Base;
using Model.Bill;
using Model.User;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Api.Controllers
{
    //[AuthFilter]
    public class BillController: DataCenterController
    {
        BillService service = new BillService();
        [JurisdictionAuthorize(name = new string[] { "z-bill", "y-bill" })]
        [Route("api/Bill/Querylist")]
       
        public SysResult<List<T_WrapBill>> Querypeibei(T_WrapBill model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("账单列表参数" + jsonData);
            SysResult<List<T_WrapBill>> sysresult = new SysResult<List<T_WrapBill>>();
            try
            {
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
                InitPage(model.PageSize, (model.PageSize * model.PageIndex));
                sysresult = service.Querybase(model, this.OrderablePagination, newuser);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        [Route("api/zkBill/Querylist")]

        public SysResult<List<T_WrapBill>> zkQuerypeibei(T_WrapBill model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("账单列表参数" + jsonData);
            SysResult<List<T_WrapBill>> sysresult = new SysResult<List<T_WrapBill>>();
            try
            {
                InitPage(model.PageSize, (model.PageSize * model.PageIndex));
                sysresult = service.Querybase(model, this.OrderablePagination,null);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        [HttpPost]
        [Route("api/Bill/billlist")]
        public SysResult<List<T_BillList>> billlist(T_BillList model)
        {
            return service.billlist(model);
        }
       
        [HttpPost]
        [JurisdictionAuthorize(name = new string[] { "bill-edit-btn" })]
        [Route("api/Bill/edit")]
        public SysResult edit(PlAction<T_BillList, T_BillList> model)
        {
            return service.edit(model);
        }
        [HttpPost]
        [Route("api/Bill/receive")]
        public SysResult receive(T_Bill bill)
        {
            bill.NotUpdatefield = new string[] { "Object", "TeantId", "BeginTime", "EndTime", "HouseId", "HouseType", "CreatePerson", "ShouldReceive", "ContractId", "Amount", "Explain", "payee", "accounts", "bank", "type","sign", "BillType" };
            return service.savebill(bill);
        }
        [HttpPost]
        [Route("api/Bill/save")]
        
        [JurisdictionAuthorize(name = new string[] { "bill-add-btn"})]
        public SysResult save(T_Bill bill)
        {
            SysResult sysresult = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            bill.CompanyId = user.CompanyId;
            return service.save(bill, user.Id);
        }
        //账单详情
        [HttpPost]
        [Route("api/Bill/Querybillbyid")]
        [JurisdictionAuthorize(name = new string[] { "bill-view-btn", "ybill-view-btn" })]
        
        public SysResult<T_WrapBill> Querybillbyid(T_WrapBill model)
        {
            return service.Querybase(model);
        }
        //账单手动收款
        [HttpPost]
        [Route("api/Bill/receivebill")]
        [JurisdictionAuthorize(name = new string[] { "fukuan" })]
        public SysResult receivebill(T_Bill model)
        {
            return service.receive(model);
        }
        //批量收款
        [HttpPost]
        [Route("api/Bill/plreceivebill")]
        public SysResult plreceivebill(T_WrapBill model)
        {

            return service.plreceive(model);
        }
        //催租短信
        [HttpPost]
        [JurisdictionAuthorize(name = new string[] { "bill-sendmessage-btn" })]
        [Route("api/Bill/cuizu")]
        public SysResult cuizu(T_WrapBill model)
        {
            SysUserService sysservice = new SysUserService();
            return sysservice.cuizu(model);
        }
        //批量催租短信
        [HttpPost]
        [Route("api/Bill/pcuizu")]
        [JurisdictionAuthorize(name = new string[] { "bill-sendmessage-btn" })]
        public SysResult pcuizu(List<T_WrapBill> model)
        {
            SysUserService sysservice = new SysUserService();
            return sysservice.pcuizu(model);
        }
        [HttpPost]
        [Route("api/Bill/delete")]
        public SysResult delete(T_Bill model)
        {
            return service.receive(model);
        }
    }
}