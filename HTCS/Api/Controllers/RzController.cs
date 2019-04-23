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
    public class RzController : DataCenterController
    {
        RzService service = new RzService();
        [Route("api/RzService/Querylist")]
        public SysResult<List<WrapHouseRz>> Querylist(HouseRz model)
        {
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult<List<WrapHouseRz>> result = new SysResult<List<WrapHouseRz>>();
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            model.companyid = user.CompanyId;
            return service.Query(model, this.OrderablePagination);
        }
    
        [Route("api/RzService/add")]
        [JurisdictionAuthorize(name = new string[] { "addrz" })]
        public SysResult add(HouseRz model)
        {
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult result = new SysResult();
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            ElecUser elecuser = GetelecUser("elec" + user.CompanyId);
            model.createperson = user.Id;
            return service.save1(model);
        }
    }
}