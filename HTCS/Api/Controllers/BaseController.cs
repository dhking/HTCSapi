using Api.CommonControllers;
using API.CommonControllers;
using ControllerHelper;
using Microsoft.Owin;
using Model;
using Model.Base;
using Model.Contrct;
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
    //[AuthFilter]
    public class BaseDataController : DataCenterController
    {
        BaseDataService service = new BaseDataService();
        [Route("api/BaseData/Querypeibei")]
        public SysResult<IList<T_V_basicc>>  Querypeibei(T_Basics model)
        {
            SysResult<IList<T_V_basicc>> sysresult = new SysResult<IList<T_V_basicc>>();
            try
            {
                InitPage(model.PageSize, (model.PageSize * model.PageIndex));
                sysresult = service.Querybase(model, this.OrderablePagination);
            }
            catch(Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        [Route("api/bankcard/Query")]
        public SysResult<IList<T_bankcard>> bankcardQuery()
        {
            SysResult<IList<T_bankcard>> sysresult = new SysResult<IList<T_bankcard>>();
            sysresult = service.bankcardQuery();
            return sysresult;
        }
        [JurisdictionAuthorize(name = new string[] { "muban" })]
        [Route("api/template/Query")]
        public SysResult<List<T_template>> templateQuery(T_template model)
        {
            SysResult<List<T_template>> sysresult = new SysResult<List<T_template>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            sysresult = service.templateQuery(model);
            return sysresult;
        }
        [Route("api/template/xqQuery")]
        public SysResult<T_template> xqQuery(T_template model)
        {
            SysResult<T_template> sysresult = new SysResult<T_template>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            sysresult = service.xqQuery(model);
            return sysresult;
        }
        [Route("api/template/morenQuery")]
        public SysResult<T_template> morenQuery(WrapContract model)
        {
            SysResult<T_template> sysresult = new SysResult<T_template>();
            sysresult = service.morenQuery(model);
            return sysresult;
        }
        [JurisdictionAuthorize(name = new string[] { "addmuban" , "editmuban" })]
        [Route("api/template/add")]
        public SysResult templateadd(T_template model)
        {
            SysResult sysresult = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
          
            sysresult = service.templateadd(model);
            return sysresult;
        }
        [Route("api/template/changemoren")]
        public SysResult changemoren(T_template model)
        {
            SysResult sysresult = new SysResult();
            sysresult = service.templateadd1(model);
            return sysresult;
        }
        [Route("api/BaseData/Querypeibeinotfy")]
        public SysResult<List<T_basicsType>> Querypeibeinotfy(T_basicsType model)
        {
            SysResult<List<T_basicsType>> sysresult = new SysResult<List<T_basicsType>>();
            try
            {
                sysresult = service.Querybasetype(model);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        //查询字典类型
        [Route("api/BaseData/Querytype")]
        public SysResult<IList<T_basicsType>> Querytype(T_basicsType model)
        {
            SysResult<IList<T_basicsType>> sysresult = new SysResult<IList<T_basicsType>>();
            try
            {
                InitPage(model.PageSize, (model.PageSize * model.PageIndex));
                sysresult = service.Querybasetype(model, this.OrderablePagination);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        //查询字典数据
        [Route("api/BaseData/Querydata")]
        public SysResult<IList<T_V_basicc>> Querydata(T_V_basicc model)
        {
            SysResult<IList<T_V_basicc>> sysresult = new SysResult<IList<T_V_basicc>>();
            try
            {
                InitPage(model.PageSize, (model.PageSize * model.PageIndex));
                sysresult = service.Querybasedata(model, this.OrderablePagination);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        //查询特色配备
        [Route("api/BaseData/Query")]
        public SysResult<WrapBasic> Query(Queryparam model)
        {
            SysResult<WrapBasic> sysresult = new SysResult<WrapBasic>();
            try
            {
                sysresult = service.Query(model);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        //查询房管员数据
        [Route("api/BaseData/Queryfgy")]
        public SysResult<List<T_SysUser>> Queryfgy(T_SysUser model)
        {
            SysResult<List<T_SysUser>> sysresult = new SysResult<List<T_SysUser>>();
            try
            {
                sysresult = service.Queryfgy(model);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }

        //查询托管信息
        [Route("api/BaseData/Querytg")]
        public SysResult<List<T_Basics>> Querytg(T_Basics model)
        {
            SysResult<List<T_Basics>> sysresult = new SysResult<List<T_Basics>>();
            try
            {
                sysresult = service.Querytg(model);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        [Route("api/tuoguan/save")]
        public SysResult Savetuoguan(t_tuoguan model)
        {
            SysResult sysresult = new SysResult();
            try
            {
                sysresult = service.Savetuoguan(model);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        [JurisdictionAuthorize(name = new string[] { "addzidian", "editzidian" })]
        [Route("api/BaseData/SaveData")]
        public SysResult SaveHouse(T_Basics model)
        {

            SysResult result = new SysResult(0, "保存成功");
            if (model.Name == null)
            {
                return result = result.FailResult("名称不能为空");
            }
            result = service.SaveData(model);
            return result;
        }
        [Route("api/BaseData/SaveTypeData")]
        public SysResult SaveTypeData(T_basicsType model)
        {

            SysResult result = new SysResult(0, "保存成功");
            if (model.Name == null)
            {
                return result = result.FailResult("名称不能为空");
            }
            if (model.Code == null)
            {
                return result = result.FailResult("编码不能为空");
            }
            result = service.SaveTypeData(model);
            return result;
        }
        
        [JurisdictionAuthorize(name = new string[] { "zcontract-delete-btn"})]
        [Route("api/BaseData/delete")]
        [HttpPost]
        public SysResult delete(iids ids)
        {
            SysResult result = new SysResult(0, "删除成功");
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string[] model = ids.ids.Split(","[0]);
            if (ids.spname != null && ids.spname != "")
            {
                ProceService proservie = new ProceService();
                return  proservie.CmdProce1(new Pure() { Id = ids.ids, Spname = ids.spname, Other = user.Id.ToStr() });
            }
            foreach (var mo in model)
            {
                result = service.deleteData(long.Parse(mo));
            }
            return result;
        }
        //查询城市
        [Route("api/BaseData/QueryCity")]
        public SysResult<List<WrapCity>> QueryCity(City model)
        {
            SysResult<List<WrapCity>> sysresult = new SysResult<List<WrapCity>>();
            try
            {
                sysresult = service.Querycity(model);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        [Route("api/BaseData/QueryCity1")]
        public SysResult<List<City>> QueryCity1(City model)
        {
            SysResult<List<City>> sysresult = new SysResult<List<City>>();
            try
            {
                sysresult = service.Querycity1(model);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        [Route("api/BaseData/Queryid")]
        public SysResult<T_Basics> Queryid(T_Basics model1)
        {
          
            return service.Queryid(model1);
        }
        [Route("api/BaseData/Querytypeid")]
        public SysResult<T_basicsType> Querytypeid(T_basicsType model)
        {
            return service.Querytypeid(model);
        }
        [Route("api/BaseData/banbenquery")]
        public SysResult<BanBen> banbenquery(BanBen model)
        {
            return service.Querybanben(model);
        }
    }
}