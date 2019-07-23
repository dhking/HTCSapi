using API.CommonControllers;
using Model;
using Model.Base;
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

    public class CleanController: DataCenterController
    {
        CleanService service = new CleanService();
        //保洁列表分页查询
        //[JurisdictionAuthorize(name = new string[] { "baojie/" })]
        [Route("api/clean/Querylist")]
        public SysResult<List<Wrapclean>> Querylist(Wrapclean model)
        {
            SysResult<List<Wrapclean>> sysresult = new SysResult<List<Wrapclean>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.companyid = user.CompanyId;
            sysresult = service.Query(model, this.OrderablePagination);
            return sysresult;
        }
        //客源详情
        [Route("api/clean/Queryxq")]
        public SysResult<Wrapclean> Queryxq(clean model)
        {
            SysResult<Wrapclean> sysresult = new SysResult<Wrapclean>();
           
            sysresult = service.Queryxq(model);
            return sysresult;
        }
        //保洁跟踪日志
        [Route("api/clean/rzQuerylist")]
        public SysResult<List<wrapcleanRZ>> rzQuerylist(cleanRZ model)
        {
            SysResult<List<wrapcleanRZ>> sysresult = new SysResult<List<wrapcleanRZ>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.companyid = user.CompanyId;
            model.opera = user.Id;
            sysresult = service.QueryRz(model);
            return sysresult;
        }
        //添加客源跟进日志
        [Route("api/clean/addrz")]
        public SysResult addrz(cleanRZ model)
        {
            SysResult sysresult = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.companyid = user.CompanyId;
            model.opera = user.Id;
            return service.addrz(model);
        }
        //添加编辑客源
        [Route("api/clean/add")]
        public SysResult add(clean model)
        {
            SysResult sysresult = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.companyid = user.CompanyId;
            return service.add(model, user.Id);
        }
        [HttpPost]
        [Route("api/clean/subject")]
        public SysResult<List<T_Basics>> subject()
        {
            return service.subject();
        }
    }
}