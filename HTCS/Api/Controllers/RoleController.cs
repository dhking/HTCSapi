using Api.CommonControllers;
using API.CommonControllers;
using ControllerHelper;
using Microsoft.Owin;
using Model;
using Model.Base;
using Model.Bill;
using Model.House;
using Model.User;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    public class RoleController : DataCenterController
    {
        RoleService service = new RoleService();
        [Route("api/RoleButton/Querylist")]
        public SysResult<List<T_Button>> Querylist(T_SysUserRole model)
        {
            return service.Querybutton(model);
        }
        //地址管理
        [Route("api/cellname/Querylist")]
        public SysResult<List<T_CellName>> storeQuerylist(T_CellName model)
        {
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            SysResult<List<T_CellName>> sysresult = new SysResult<List<T_CellName>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            return service.Querystore(model, this.OrderablePagination, new int[] { 4, 5 });
        }
        [Route("api/cellname/Querylist1")]
        public SysResult<List<T_CellName>> storeQuerylist1(T_CellName model)
        {
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            SysResult<List<T_CellName>> sysresult = new SysResult<List<T_CellName>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            return service.Querystore(model, this.OrderablePagination,new int[] { 4,5});
        }
        //查询门店详情
        [Route("api/store/Queryid")]
        public SysResult<T_CellName> Queryid(T_CellName model)
        {
            return service.Queryid(model);
        }
        //保存门店
        [HttpPost]
        [Route("api/store/save")]
        public SysResult save(T_CellName bill)
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
            return service.storesave(bill);
        }
        //保存大区
        [HttpPost]
        [Route("api/area/save")]
        public SysResult savearea(T_CellName bill)
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
            return service.areasave(bill);
        }
        [HttpPost]
        [Route("api/Role/save")]
        public SysResult save(T_SysRole bill)
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
            return service.save(bill);
        }
        [Route("api/rolr/Queryrole")]
        public SysResult<T_SysRole> QueryUser(T_SysRole model)
        {
           
            return service.QueryUser(model);
        }
        //删除
        [Route("api/tongyong/delete")]
        [JurisdictionAuthorize(isty = 1)]
        [HttpPost]
        public SysResult delete(iids ids)
        {
            return service.delete(ids);
        }
    }
}