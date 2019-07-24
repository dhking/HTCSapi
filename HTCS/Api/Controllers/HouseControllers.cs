using Api.CommonControllers;
using API.CommonControllers;
using DBHelp;
using Model;
using Model.House;
using Model.User;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace API
{
    //合租房源
    public class HouseController : DataCenterController
    {
        LogService log = new LogService();
        HoseService service = new HoseService();
        //分页查询
        [JurisdictionAuthorize(name = new string[] { "h-house" })]
        [Route("api/House/Queryhouselist")]
        public SysResult<IList<WrapHouseModel>> Queryhouselist(HouseModel model)
        {
            SysResult<IList<WrapHouseModel>> sysresult = new SysResult<IList<WrapHouseModel>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            long[] userids = getuserids(user.departs,user.Id);
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Queryhouse(model, this.OrderablePagination,user, userids);
            return sysresult;
        }
        //房源搜索提示
        [Route("api/House/Querybyname")]
        public SysResult<List<HouseTip>> Querybuxiaoqu(ParaTip model)
        {
            SysResult<List<HouseTip>> sysresult = new SysResult<List<HouseTip>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            long[] userids = getuserids(user.departs, user.Id);
            model.CompanyId = user.CompanyId;
            return service.Querytip(model,this.OrderablePagination, userids,user);
        }
        //搜索房源
        [Route("api/House/Query")]
        public SysResult<List<houresources>> HouseQuery(houresources model)
        {
            SysResult<List<houresources>> sysresult = new SysResult<List<houresources>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            return service.HouseQuery(model, this.OrderablePagination);
        }
        //整租房源列表
        [JurisdictionAuthorize(name = new string[] { "z-house" })]
        [Route("api/zHouse/Query")]
        public SysResult<List<WrapHouseModel1>> zHouseQuery(HouseModel model)
        {
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            SysResult<List<WrapHouseModel1>> sysresult = new SysResult<List<WrapHouseModel1>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            string[] citys = getcity(user);
            return service.zzHouseQuery(model, this.OrderablePagination, citys, user);
        }
        //新分组整租房源列表
        [JurisdictionAuthorize(name = new string[] { "z-house" })]
        [Route("api/newzHouse/Query")]
        public SysResult<List<WrapCellName>> newzHouseQuery(HouseModel model)
        {
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            SysResult<List<WrapCellName>> sysresult = new SysResult<List<WrapCellName>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            string[] citys = getcity(user);
            return service.zzHouseQuerygroupby(model, this.OrderablePagination, citys, user);
        }
        [Route("api/Housedepend/Query")]
        public SysResult<List<HousePendent>> Housedepend(HousePendent model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
           
            log.LogError("搜索参数" + jsonData);
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            return service.Housedepend(model, this.OrderablePagination);
        }
        //房源搜索提示升级版
        [Route("api/House/Queryallbyname")]
        public SysResult<List<HouseTip>> Queryallbyname(ParaTip model)
        {
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            return service.Querytip1(model, this.OrderablePagination);
        }
        
        //智能门锁搜索提示
        [Route("api/House/Querylockbyname")]
        public SysResult<List<HouseTip>> Querylockbyname(ParaTip model)
        {
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            return service.Querytip2(model, this.OrderablePagination);
        }

        //添加合租房源
        [JurisdictionAuthorize(name = new string[] { "hhouseadd", "hhouseedit" })]
        [Route("api/House/SaveHouse")]
        public SysResult<List<HousePendent>> SaveHouse(HouseModel model)
        {
            SysResult<List<HousePendent>> result = new SysResult<List<HousePendent>>(0, "保存成功");
           
            try
            {
                string jsonData = JsonConvert.SerializeObject(model);
                LogService log = new LogService();
                log.logInfo("添加整租参数" + jsonData);
                string errmsg = "";
                T_SysUser user = GetCurrentUser(GetSysToken());
                if (user == null)
                {
                    result.Code = 1002;
                    result.Message = "请先登录";
                    return result;
                }
                if (!Valite(out errmsg, model))
                {
                    result.Code = 1;
                    result.Message = "验证失败" + errmsg;
                    return result;
                }
                if (model.ShiNumber <= 0)
                {
                    result.Code = 1;
                    result.Message = "室不能为空";
                    return result;
                }
                
                model.RecrntType = 2;
                model.CompanyId = user.CompanyId;
                model.CreatePerson = user.RealName;
               
                result = service.saveHouse(model, user.Id,user);
                
            }
            catch (Exception ex)
            {
                result.Code = 2;
                result.Message = ex.ToString();
            }
            log.logInfo("我走了" + DateTime.Now);
            return result;
        }
        [JurisdictionAuthorize(name = new string[] { "zhouseadd", "zhouseedit" })]
        
        [Route("api/House/SaveHouse2")]
        public SysResult<List<HousePendent>> SaveHouse2(HouseModel model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("添加整租参数" + jsonData);
            model.Status = 1;
            SysResult<List<HousePendent>> result = new SysResult<List<HousePendent>>(0, "保存成功");
            try
            {
                string errmsg = "";
                T_SysUser user = GetCurrentUser(GetSysToken());
                if (user == null)
                {
                    result.Code = 1002;
                    result.Message = "请先登录";
                    return result;
                }
                if (!Valite(out errmsg, model))
                {
                    result.Code = 1;
                    result.Message = "验证失败" + errmsg;

                }
                if (model.ShiNumber <= 0)
                {
                    result.Code = 1;
                    result.Message = "室不能为空";
                }
                model.RecrntType = 1;
                model.CompanyId = user.CompanyId;
                
                result = service.saveHouse(model,user.Id, user);

            }
            catch (Exception ex)
            {
                result.Code = 3;
                result.Message = ex.ToString();
            }
            log.logInfo("我走了" + DateTime.Now);
            return result;
        }
        //查询公开区域详情
        [Route("api/House/publicHousexq")]
        public SysResult<HouseModel> publicHousexq(HouseModel model)
        {
            return service.queryhouse(model);
        }
        //添加房间
        [JurisdictionAuthorize(name =new string[] { "hhouseedit" })]
        [Route("api/House/adddepentHouse")]
        public SysResult<HousePendent> adddepentHouse(HouseModel model)
        {
            model.Status = 1;
            return service.addHouse(model);
        }
        //编辑独立合租房源
        //[AuthFilterAttribute]
        [JurisdictionAuthorize(name = new string[] { "dhouseedit" })]
        [Route("api/House/editsavepentHouse")]
        public SysResult editsavepentHouse(HousePendent model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("编辑单间信息" + jsonData);
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            model.CompanyId = user.CompanyId;
            return service.saveeditHouse(model, user.Id);
        }
        //删除房间
        [JurisdictionAuthorize(name = new string[] { "hhousedelete"})]
        [Route("api/House/deletedepentHouse")]
        [HttpPost]
        public SysResult deletedepentHouse(HouseModel model)
        {
           
            return service.deleteData(model);
        }
        //整租删除房间
        [JurisdictionAuthorize(name = new string[] {  "zhousedelete" })]
        [Route("api/zHouse/deletedepentHouse")]
        [HttpPost]
        public SysResult zdeletedepentHouse(HouseModel model)
        {

            return service.deleteData(model);
        }
        //删除整套房间
        [Route("api/House/deleteHouse")]
        [HttpPost]
        public SysResult deleteHouse(HouseModel model)
        {
            return service.sp_deleteallhouse(model);
        }
        //查询单间详情
        [Route("api/House/Querydepent")]
        [HttpPost]
        public SysResult<HousePendent> Querydepent(HousePendent model)
        {
            return service.Querydepent(model);
        }
        public bool Valite(out string errmsg,HouseModel model)
        {
            errmsg = "";
            bool result = true;
            if (model.ShiNumber== 0)
            {
                result = false;
                errmsg += "室不能为空";
                return result;
            }
            if (model.TingNumber == 0)
            {
                result = false;
                errmsg += "厅不能为空";
                return result;
            }
            if (model.WeiNumber == 0)
            {
                result = false;
                errmsg += "卫不能为空";
                return result;
            }
            if(model.CellName==null|| model.CellName == "")
            {
                result = false;
                errmsg += "小区名称不能为空";
                return result;
            }
            
            if (model.CityName ==null)
            {
                result = false;
                errmsg += "市不能为空";
                return result;
            }
            return result;
        }

        //根据合同编号筛选名下锁列表
        [Route("api/House/Querylocal")]
        [HttpPost]
        public SysResult<List<HouseLockQuery>> Querylocal(HouseModel  model)
        {
            return service.Querylockbyhouse(model);
        }
        //查询房管员名下房源
        [Route("api/House/userQueryfgy")]
        [HttpPost]
        public SysResult<List<HouseQueryfgy>> Queryfgy(HouseModel model)
        {
            return service.queryfgy(model);
        }
        //查询所有房源
        [Route("api/House/Queryhouse")]
        public SysResult<List<distributionHouseQuery>> Queryhouse(HouseModel model)
        {
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            SysResult<List<distributionHouseQuery>> sysresult = new SysResult<List<distributionHouseQuery>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            return service.Queryhouse(model, this.OrderablePagination);
        }
        //按照小区和楼查询房源
        [Route("api/House/Queryhousecount")]
        public SysResult<List<WrapHouse>> Queryhousecount(HouseModel model)
        {
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            SysResult<List<WrapHouse>> sysresult = new SysResult<List<WrapHouse>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            return service.Queryhousecount(model, this.OrderablePagination);
        }
        //查询所有部门房源
        [Route("api/House/Queryhousedepart")]
        public SysResult<List<distributionHouseQuery>> Queryhousedepart(HouseModel model)
        {
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            SysResult<List<distributionHouseQuery>> sysresult = new SysResult<List<distributionHouseQuery>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            return service.Queryhousedepart(model, this.OrderablePagination);
        }
        //查询选中的房源
        [Route("api/House/checkQueryhousedepart")]
        public SysResult<List<long>> checkQueryhousedepart(HouseModel model)
        {
            SysResult<List<long>> sysresult = new SysResult<List<long>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            return service.Queryhousedepartcheck(model);
        }
        //查询股东已分配的房源
        [Route("api/House/checkQueryhousedepart1")]
        public SysResult<List<string>> checkQueryhousedepart1(T_SysUser model)
        {
            SysResult<List<string>> sysresult = new SysResult<List<string>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            return service.Queryhousedepartcheck1(model);
        }
    }
}
