using API.CommonControllers;
using Model;
using Model.House;
using Model.User;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{
    public class GuestController: DataCenterController
    {
        GuestService service = new GuestService();
        //客源列表分页查询
        [JurisdictionAuthorize(name = new string[] { "guest/" })]
        [Route("api/Guest/Querylist")]
        public SysResult<List<WrapGuest>> Querylist(Guest model)
        {
            SysResult<List<WrapGuest>> sysresult = new SysResult<List<WrapGuest>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            sysresult = service.Query(model, this.OrderablePagination);
            return sysresult;
        }
        //客源详情
        [Route("api/Guest/Queryxq")]
        public SysResult<WrapGuest> Queryxq(Guest model)
        {
            SysResult<WrapGuest> sysresult = new SysResult<WrapGuest>();
           
            sysresult = service.Queryxq(model);
            return sysresult;
        }
        //客源跟踪日志
        [Route("api/Guestrz/rzQuerylist")]
        public SysResult<List<GuestRz>> rzQuerylist(GuestRz model)
        {
            SysResult<List<GuestRz>> sysresult = new SysResult<List<GuestRz>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            sysresult = service.QueryRz(model);
            return sysresult;
        }
        //添加客源跟进日志
        [Route("api/Guestrz/addrz")]
        public SysResult addrz(GuestRz model)
        {
            SysResult sysresult = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CreatePerson = user.RealName;
            model.CompanyId = user.CompanyId;
            return service.addrz(model);
        }
        //添加编辑客源
        [Route("api/Guest/add")]
        public SysResult add(Guest model)
        {
            SysResult sysresult = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CreatePerson = user.RealName;
            model.CompanyId = user.CompanyId;
            return service.add(model);
        }
        //官网预约
        [Route("api/Guest/gwadd")]
        public SysResult gwadd(Appointment model)
        {
            return service.gwadd(model);
        }
        //放弃
        [Route("api/Guest/giveup")]
        public SysResult action(Guest model)
        {
          
            return service.giveup(model);
        }

        //分配
        [Route("api/Guest/distribution")]
        public SysResult distribution(Guest model)
        {
            return service.distribution(model);
        }
    }
}