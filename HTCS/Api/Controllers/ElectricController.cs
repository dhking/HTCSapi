using API.CommonControllers;
using DBHelp;
using Model;
using Model.Contrct;
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
    public class ElectricController : DataCenterController
    {
        ElectricService service = new ElectricService();
        [Route("api/Electric/Login")]
        public SysResult Login(ElecUser model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            service.Login(model, user);
            return result;
        }
       
        //充值
        [Route("api/Electric/threshold")]
        public SysResult accountthreshold(other model)
        {
          
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult reslut = new SysResult();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.threshold(model, user, sysuser);
            return reslut;
        }
        //入住
        [Route("api/Electric/ammeterstayroom")]
        public SysResult ammeterstayroom(ruzhu model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult reslut = new SysResult();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.ammeterstayroom(model, user, sysuser);
            return reslut;
        }
        //退房
        [Route("api/Electric/ammeterrecederoom")]
        public SysResult ammeterrecederoom(ruzhu model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult reslut = new SysResult();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.ammeterrecederoom(model, user, sysuser);
            return reslut;
        }
        //租客端充值
        [Route("api/Electric/zkthreshold")]
        public SysResult zkaccountthreshold(T_Contrct model)
        {
            SysResult reslut = new SysResult();
            
            return service.zkthreshold(model); ;
        }
        //分页获取电表
        [JurisdictionAuthorize(name = new string[] { "dianbiao" })]
        [Route("api/ammeter/page")]
        public SysResult<List<DeviceData>> ammeterpage(other model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            model.PageIndex = model.PageIndex - 1;
            SysResult<List<DeviceData>> reslut = new SysResult<List<DeviceData>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.ammeterpage(model, user, sysuser);
            return reslut;
        }
        //获取房源设备
        [Route("api/house/device")]
        public SysResult<List<DeviceData>> housedevice(other model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<List<DeviceData>> reslut = new SysResult<List<DeviceData>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.housedevice1(model, user, sysuser);
            return reslut;
        }
        //创建房源
        [Route("api/terminal/createhouse")]
        public SysResult<List<DeviceData>> createhouse(other model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("绑定集中器参数" + jsonData);
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<List<DeviceData>> reslut = new SysResult<List<DeviceData>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            log.logInfo("公司编号" + sysuser.CompanyId);
            reslut = service.createhouse(model, user, sysuser);
            return reslut;
        }
        //绑定集中器
        [Route("api/terminal/bind")]
        public SysResult<List<DeviceData>> terminalbind(other model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("绑定集中器参数" + jsonData);
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<List<DeviceData>> reslut = new SysResult<List<DeviceData>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            log.logInfo("公司编号" + sysuser.CompanyId);
            reslut = service.terminalbind(model, user, sysuser);
            return reslut;
        }
        //绑定节点
        [Route("api/node/bind")]
        public SysResult<List<DeviceData>> nodebind(other model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("绑定节点参数" + jsonData);
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<List<DeviceData>> reslut = new SysResult<List<DeviceData>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.nodebind(model, user, sysuser);
            return reslut;
        }
        //电表退房清单
        [Route("api/device/recederoom")]
        public SysResult<DeviceData> recederoom(other model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("电表退房清单" + jsonData);
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<DeviceData> reslut = new SysResult<DeviceData>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.recederoom(model, user, sysuser);
            return reslut;
        }
        //绑定房源
        [Route("api/device/binding")]
        public SysResult<Wraplocklist> binding(DeviceData model)
        {
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("绑定房源" + jsonData);
            SysResult<Wraplocklist> result = service.Bing(model);
            return result;
        }
        //解除绑定房源
        [Route("api/device/notbinding")]
        public SysResult notbinding(DeviceData model)
        {
            SysResult result = service.notBing(model);
            return result;
        }
        //判断子表还是主表
        [Route("api/device/checktype")]
        public SysResult<int> devicechecktype(DeviceData model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<int> reslut = new SysResult<int>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.devicechecktype(model, user, sysuser);
            return reslut;
        }
        //获取房源树
        [Route("api/house/refresh")]
        public SysResult<List<fenzu>> houserefresh(fenzu model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<List<fenzu>> reslut = new SysResult<List<fenzu>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.houserefresh(model, user, sysuser);
            return reslut;
        }
        //获取单个电表
        [Route("api/device/ammeter")]
        public SysResult<List<DeviceData>> deviceammeter(DeviceData model)
        {
            LogService log = new LogService();
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("获取单个电表参数" + jsonData);
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<List<DeviceData>> reslut = new SysResult<List<DeviceData>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.deviceammeter(model, user, sysuser);
            return reslut;
        }
        //设置自动催租
        [Route("api/device/iscuizu")]
        public SysResult deviceiscuizu(DeviceData model)
        {
           return  service.deviceiscuizu(model);
        }
        //租客端获取单价和电表度数
        [Route("api/house/zkammeter")]
        public SysResult<zkelec> zkammeter(T_Contrct model)
        {
            SysResult<zkelec> reslut = new SysResult<zkelec>();
            reslut = service.zkdeviceammeter(model);
            return reslut;
        }
        [Route("api/house/zkaddupmonth")]
        public SysResult<List<ElecStatic>> zkaddupmonth(ElecStatic model)
        {
            SysResult<List<ElecStatic>> reslut = new SysResult<List<ElecStatic>>();
            reslut = service.zkaddupmonth(model);
            return reslut;
        }
        //搜索房源或者电表
        [Route("api/house/search")]
        public SysResult<List<DeviceData>> housesearch(DeviceData model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<List<DeviceData>> reslut = new SysResult<List<DeviceData>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.housesearch(model, user, sysuser);
            return reslut;
        }
        //获取省市区代码
        [Route("api/house/city")]
        public SysResult<List<DeviceData>> housecity(localcity model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<List<DeviceData>> reslut = new SysResult<List<DeviceData>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.housecity(model, user, sysuser);
            return reslut;
        }
        //通电或者断电
        [Route("api/device/switchon")]
        public SysResult deviceswitchon(DeviceData model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult reslut = new SysResult();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.deviceswitchon(model, user, sysuser,true);
            return reslut;
        }
        //修改电价
        [Route("api/ammeter/price")]
        public SysResult ammeterprice(DeviceData model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult reslut = new SysResult();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.ammeterprice(model, user, sysuser, true);
            return reslut;
        }
        //断电
        [Route("api/device/switchoff")]
        public SysResult deviceswitchoff(DeviceData model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult reslut = new SysResult();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.deviceswitchon(model, user, sysuser, false);
            return reslut;
        }
        //公共电表设置分摊
        [Route("api/ammeter/changetype")]
        public SysResult ammeterchangetype(DeviceData model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult reslut = new SysResult();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
           
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.ammeterchangetype(model, user, sysuser);
            return reslut;
        }
        //解绑集中器
        [Route("api/terminal/unbound")]
        public SysResult terminalunbound(DeviceData model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult reslut = new SysResult();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.terminalunbound(model, user, sysuser);
            return reslut;
        }
        //解绑节点
        [Route("api/node/unbound")]
        public SysResult nodeunbound(DeviceData model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult reslut = new SysResult();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.nodeunbound(model, user, sysuser);
            return reslut;
        }
        //通过设备id获取设备的一段时间的用电统计
        [Route("api/report/addupdevice")]
        public SysResult<List<ElecStatic>> reportaddupdevice(ElecStatic model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<List<ElecStatic>> reslut = new SysResult<List<ElecStatic>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.reportaddupdevice(model, user, sysuser);
            return reslut;
        }
        //用电量小计，主要用于统计每月的累计用电
        [Route("api/addup/month")]
        public SysResult<List<ElecStatic>> addupmonth(ElecStatic model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<List<ElecStatic>> reslut = new SysResult<List<ElecStatic>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.addupmonth(model, user, sysuser);
            return reslut;
        }
        //修改付费模式
        [Route("api/device/ammeterpaymode")]
        public SysResult ammeterpaymode(DeviceData model)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult reslut = new SysResult();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            ElecUser user = GetelecUser("elec" + sysuser.CompanyId);
            reslut = service.ammeterpaymode(model, user, sysuser);
            return reslut;
        }
    }
}