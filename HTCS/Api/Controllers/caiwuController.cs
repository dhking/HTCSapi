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
    public class caiwuController : DataCenterController
    {

        //提现列表
        [JurisdictionAuthorize(name = new string[] { "set/base/tixian" })]
        [Route("api/caiwutx/Query")]
        [HttpPost]
        public SysResult<List<T_Record>> Query(T_Record model)
        {
            caiwuService service = new caiwuService();
            SysResult<List<T_Record>> sysresult = new SysResult<List<T_Record>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Querymenufy(model, this.OrderablePagination);
            return sysresult;
        }
        //订单列表
        [JurisdictionAuthorize(name = new string[] { "set/base/money" })]
        [Route("api/order/Query")]
        [HttpPost]
        public SysResult<List<wrapOrder>> orderQuery(wrapOrder model)
        {
            OrderService service = new OrderService();
            SysResult<List<wrapOrder>> sysresult = new SysResult<List<wrapOrder>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Query(model, this.OrderablePagination);
            return sysresult;
        }
        //报表
        [Route("api/baobiao/Query")]
        [HttpPost]
        public SysResult<List<HouseReport>> baobiaoQuery(HouseReport model)
        {
            caiwuService service = new caiwuService();
            SysResult<List<HouseReport>> sysresult = new SysResult<List<HouseReport>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            
            sysresult = service.Querybaobiao(model, this.OrderablePagination);
            return sysresult;
        }
        //报表子表
        [Route("api/baobiaochild/Query")]
        [HttpPost]
        public SysResult<List<WrapHouseReportList>> baobiaochildQuery(HouseReportList model)
        {
            caiwuService service = new caiwuService();
            SysResult<List<WrapHouseReportList>> sysresult = new SysResult<List<WrapHouseReportList>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.baobiaochildQuery(model, this.OrderablePagination);
            return sysresult;
        }
    }
}