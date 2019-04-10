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
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    public class IndependentHouseController : DataCenterController
    {
        IndentHouseService service = new IndentHouseService();
        LogService log = new LogService();
        //新增独栋房源
        [JurisdictionAuthorize(name = new string[] { "dhouseadd" })]
        
        [Route("api/IndependHouse/add")]
        public SysResult add(HouseModel model)
        {
            model.Status = 1;
            string jsonData = JsonConvert.SerializeObject(model);

            log.LogError("新增独栋参数" + jsonData);
            SysResult result = new SysResult(0, "保存成功");
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
                    return result;
                }
                if (model.floor ==null)
                {
                    result.Code = 1;
                    result.Message = "楼层不能为空";
                    return result;
                }
                model.RecrntType = 3;
                model.CompanyId = user.CompanyId;
                result = service.saveHouse(model, user.Id);
            }
            catch (Exception ex)
            {
                result.Code = 2;
                result.Message = ex.ToString();
            }
            return result;
        }
        //分页查询
        [Route("api/IndependHouse/Queryhouselist")]
        public SysResult<List<WrapIndentHouse>> Queryhouselist(HouseModel model)
        {
            SysResult<List<WrapIndentHouse>> sysresult = new SysResult<List<WrapIndentHouse>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
          
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Queryhouse(model);
            return sysresult;
        }
        //PC端根据公寓查询楼层和楼层房间
        
        [JurisdictionAuthorize(name =new string[] { "d-house" })]
        [Route("api/IndependHouse/PCQueryhouselist")]
        public SysResult<Wrapdudong> PCQueryhouselist(HouseModel model)
        {
            SysResult<Wrapdudong> sysresult = new SysResult<Wrapdudong>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.QueryPChouse(model,this.OrderablePagination);
            return sysresult;
        }
        //楼层筛选条件
        [Route("api/IndependHouse/shaixuan")]
        public SysResult<List<WrapIndentHouse>> shaixuan()
        {
            SysResult<List<WrapIndentHouse>> sysresult = new SysResult<List<WrapIndentHouse>>();
            sysresult = service.Queryshaixuan();
            return sysresult;
        }
        //楼层下面的单间列表
        [Route("api/IndependHouse/singleroomlist")]
        public SysResult<wrapHousePendent> singleroomlist(HousePendent model)
        {
            SysResult<wrapHousePendent> sysresult = new SysResult<wrapHousePendent>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Querysinglehouse(model, this.OrderablePagination);
            return sysresult;
        }
        //查询公寓详情
        [Route("api/IndependHouse/Queryhousebyid")]
        public SysResult<HouseModel> Queryhousebyid(HouseModel model)
        {
            SysResult<HouseModel> sysresult = new SysResult<HouseModel>();
            sysresult = service.Queryhousebyid(model);
            return sysresult;
        }
        //编辑公区
        [Route("api/IndependHouse/Update")]
        public SysResult Update(HouseModel model)
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
            sysresult = service.Update(model);
            return sysresult;
        }
        //添加//编辑楼层
        [Route("api/IndependHouse/savefloor")]
        public SysResult addfloor(T_Floor model)
        {
           return  service.addfloor(model);
        }
        //楼层
        [Route("api/IndependHouse/floorlist")]
        public SysResult<List<T_Floor>> floorlist(HousePendent model)
        {
            return service.Queryfloorlist(model);
        }
        //快捷添加房间
        [Route("api/IndependHouse/addhouse")]
        public SysResult<HousePendent> addindenthouse(HousePendent model)
        {
            model.Status = 1;
            return service.savehouse(model);
        }
        //删除楼层
        [Route("api/IndependHouse/delelefloor")]

        public SysResult delelefloor(T_Floor model)
        {
            return service.deleteData(model);
        }
        //删除公寓
        [Route("api/IndependHouse/delele")]
        public SysResult deleleindenthouse(HouseModel model)
        {
            return service.deletepart(model);
        }
        public bool Valite(out string errmsg, HouseModel model)
        {
            errmsg = "";
            bool result = true;
            if (model.Adress ==null)
            {
                result = false;
                errmsg += "地址不能为空";
                return result;
            }
            if (model.AreamName == "")
            {
                result = false;
                errmsg += "市不能为空";
                return result;
            }
            if (model.CityName == "")
            {
                result = false;
                errmsg += "市不能为空";
                return result;
            }
            return result;
        }
    }
}