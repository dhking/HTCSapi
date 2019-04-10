using Api.CommonControllers;
using API.CommonControllers;
using DBHelp;
using Model;
using Model.Base;
using Model.House;
using Model.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{
    public class ThirdController : DataCenterController
    {
        ThirdService service = new ThirdService();
        //身份证识别
        [HttpPost]
        [Route("api/baidu/IDCard")]
        public SysResult<string> IDCard(Idcard model)
        {
            SysResult<string> result = new SysResult<string>();
            string image = "";
            string fimage = "";
            image = model.image;
            fimage = model.fimage;
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("身份认证" + jsonData);
            string path = "D://Image/";
            model.image = ConvertHelper.ImgToBase64String(path+ model.image);
            model.fimage = ConvertHelper.ImgToBase64String(path + model.fimage);
            result = service.baiduid(model, image, fimage);
            return result;
        }
        //企业认证
        [HttpPost]
        [Route("api/Enterprise/certification")]
        public SysResult certification(T_CertIfication model)
        {
            return service.certification(model);
        }
        //支付认证
        [Route("api/pay/Authentication")]
        public SysResult Authentication(T_account model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                return result=result.FailResult("用户不存在");
            }
            result = service.Authentication(model,user.CompanyId,user.Id);
            return result;
        }
        //保存设置
        [Route("api/account/save")]
        public SysResult accountsave(T_account model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                return result = result.FailResult("用户不存在");
            }
            result = service.accountsave(model, user.CompanyId, user.Id);
            return result;
        }
        //功能开关
        [Route("api/account/function")]
        public SysResult accountfunction(T_account model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                return result = result.FailResult("用户不存在");
            }
            result = service.function(model, user.CompanyId, user.Id);
            return result;
        }
        [Route("api/pay/setpassword")]
        public SysResult setpassword(T_account model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                return result = result.FailResult("用户不存在");
            }
            result = service.setpassword(model, user.CompanyId);
            return result;
        }
        //查询公司信息
        [HttpPost]
        [Route("api/Enterprise/querycompany")]
        public SysResult<T_account> querycompany()
        {
            SysResult<T_account> result = new SysResult<T_account>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                result.Code = 1;
                result.Message = "用户不存在";
                return result;
            }
            result = service.querycompany( user.CompanyId);
            return result;
        }
        //查询认证信息
        [Route("api/sysuser/querycert")]
        public SysResult<Wrapcert> querycert(T_CertIfication model)
        {
            return service.querycert(model);
        }
        //根据合同编号查URL
        [Route("api/contract/queryurl")]
        public SysResult<T_account> queryurl(T_account model)
        {
            return service.queryurl(model);
        }
        //获取授权访问令牌 支付宝授权
        [Route("api/zfb/oauthtoken")]
        public SysResult<string> oauthtoken(zfbzz model)
        {
            SysResult<string> result = new SysResult<string>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            result = service.authtoken();
            return result;
        }
        //微信授权
        [Route("api/weixin/wxnoauthtoken")]
        public SysResult<string> wxnoauthtoken(zfbzz model)
        {
            SysResult<string> result = new SysResult<string>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            result = service.authtoken();
            return result;
        }

    }
}