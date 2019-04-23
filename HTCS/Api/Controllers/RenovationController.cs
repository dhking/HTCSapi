
using API.CommonControllers;
using ControllerHelper;
using Microsoft.Owin;
using Model;
using Model.Base;
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
    public class RenovationController : DataCenterController
    {
        RenovationService service = new RenovationService();
        //分页查询
        [JurisdictionAuthorize(name = new string[] { "Renovation" })]
        [Route("api/Renovation/Querylist")]
        public SysResult<List<WrapRenovation>> Querylist(Renovation model)
        {
            SysResult<List<WrapRenovation>> sysresult = new SysResult<List<WrapRenovation>>();
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
        //查询详情
        [Route("api/Renovation/Queryid")]
        public SysResult<WrapRenovation> Queryid(WrapRenovation model)
        {
            SysResult<WrapRenovation> sysresult = new SysResult<WrapRenovation>();
            sysresult = service.Querybyid(model);
            return sysresult;
        }
        //保存
        [Route("api/Renovation/Save")]
        public SysResult Save(Renovation model)
        {
            SysResult sysresult = new SysResult();
            sysresult = service.save(model);
            return sysresult;
        }
        //保存列
        [Route("api/Renovationlist/Save")]
        public SysResult Savelist(TRenovationList model)
        {
            SysResult sysresult = new SysResult();
            sysresult = service.savelist(model);
            return sysresult;
        }
        //查询子表
        //报修项目列表
        [Route("api/Renovationlist/RepairList")]
        public SysResult<List<TRenovationList>> RepairList(TRenovationList model)
        {
            SysResult<List<TRenovationList>> sysresult = new SysResult<List<TRenovationList>>();
            sysresult = service.Queryrepairlist(model);
            return sysresult;
        }
        //查询子表详情
     
        [Route("api/Renovationlist/Repairxq")]
        public SysResult<TRenovationList> Repairxq(WrapRenovation model)
        {
            SysResult<TRenovationList> sysresult = new SysResult<TRenovationList>();
            sysresult = service.Repairxq(model);
            return sysresult;
        }
    }
}