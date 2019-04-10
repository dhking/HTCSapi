using API.CommonControllers;
using Model;
using Model.Contrct;
using Model.House;
using Model.TENANT;
using Model.User;
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
            SysResult<List<WrapCell>> sysresult = new SysResult<List<WrapCell>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            sysresult.numberData = service.Querycell(model);
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
            sysresult.numberData = service.Querycell1(model);
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
            sysresult.numberData = service.Querystore(model);
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