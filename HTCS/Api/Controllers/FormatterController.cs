using API.CommonControllers;
using DBHelp;
using Model;
using Model.Contrct;
using Model.House;
using Model.TENANT;
using Model.User;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
namespace Api.Controllers
{
    public class FormatterController : DataCenterController
    {
        FormatterService service = new FormatterService();
        [Route("api/Formatter/Query")]
        public string Query(Formatter model)
        {
            return service.Query(model);
        }
        [Route("api/Formatter/Queryxq")]
        public object Queryxq(Formatter model)
        {
            string yy = model.Model;
            Assembly mockAssembly = Assembly.Load("Model");
            Type type = mockAssembly.GetType(yy);
            FormatterService worker = new FormatterService();
            Type workerType = typeof(FormatterService);
            MethodInfo doWorkMethod = workerType.GetMethod("Queryxy");
            MethodInfo curMethod = doWorkMethod.MakeGenericMethod(type);
            return  curMethod.Invoke(worker, new Object[] { model });
        }
        [Route("api/Formatter/QueryCell")]
        public SysResult<List<WrapCell>> QueryCellname(WrapCell model)
        {
            LogService log = new LogService();
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("筛选参数" + jsonData);
            SysResult<List<WrapCell>> sysresult = new SysResult<List<WrapCell>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            string[] citys = getcity(user);
            string[] cellname = getcellname(user);
            sysresult.numberData = service.Querycell(model, citys, cellname, user);
            return sysresult;
        }
        //app 独栋筛选条件
        [Route("api/Formatter/building")]
        public SysResult<List<WrapCellBuilding>> building(WrapCell model)
        {
            SysResult<List<WrapCellBuilding>> sysresult = new SysResult<List<WrapCellBuilding>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            string[] citys = getcity(user);
            string[] cellname = getcity(user);
            sysresult.numberData = service.Querycellbuilding(model, citys, cellname, user);
            return sysresult;
        }
        [Route("api/Formatter/QueryPCCell1")]
        public SysResult<List<WrapCell>> QueryCellname1(WrapCell model)
        {
            SysResult<List<WrapCell>> sysresult = new SysResult<List<WrapCell>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            string[] citys = getcity(user);
            string[] cellname = getcellname(user);
            sysresult.numberData = service.Querycell1(model, citys, cellname, user);
            return sysresult;
        }
        [Route("api/Formatter/Querystore")]
        public SysResult<List<WrapCell>> Querystore(WrapCell model)
        {
            SysResult<List<WrapCell>> sysresult = new SysResult<List<WrapCell>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.Type = 4;
            model.CompanyId = user.CompanyId;
            string[] citys = getcity(user);
            string[] cellname = getcellname(user);
            sysresult.numberData = service.Querystore(model, citys, cellname, user);
            return sysresult;
        }
      
        [Route("api/Formatter/QueryCity")]
        public SysResult<List<WrapCity>> QueryCity(WrapCell model)
        {
            SysResult<List<WrapCity>> sysresult = new SysResult<List<WrapCity>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
          
            sysresult.numberData = service.Querycity(model);
            return sysresult;
        }

    }
}