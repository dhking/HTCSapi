using API.CommonControllers;
using DBHelp;

using Model;
using Model.Contrct;
using Model.Map;
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
    public class KjxController : DataCenterController
    {
        KjxService service = new KjxService();
        //锁初始化
        [JurisdictionAuthorize(name = new string[] {  "mensuo" })]
        [Route("api/lock/initialize")]
        public SysResult initialize(paralock param)
        {
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult result= new SysResult();
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            return service.initialize(param, user);
        }
        [Route("api/kjx/login")]
        //获取访问令牌
        public SysResult<T_kjx> login(T_kjx param)
        {

            return service.login(param,"kjx"+param.UserId);
        }
        //注册
        [Route("api/kjx/register")]
        public SysResult<T_kjx> register(T_kjx param)
        {
            
            return service.register(param);
        }
        //重置密码
        [Route("api/user/resetPassword")]
        public SysResult<T_kjx> resetPassword(T_kjx param)
        {
            return service.resetPassword(param);
        }
        [Route("api/kjx/loginout")]
        //科技侠退出
        public SysResult loginout(T_kjx param)
        {

            return service.loginout(param);
        }
        [JurisdictionAuthorize(name = new string[] { "mensuo" })]
        [Route("api/kjx/locallist")]
        //获取名下锁列表
        public SysResult<List<Wraplocklist>> list(local model)
        {
            SysResult<List<Wraplocklist>> result = new SysResult<List<Wraplocklist>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.locklist(model, user);
            return result;
        }
        
        [Route("api/kjx/binding")]
        //绑定房间
        public SysResult<Wraplocklist> binding(paralock model)
        {
            SysResult<Wraplocklist> result = service.Bing(model);
            return result;
        }
        //同步钥匙数据
        [Route("api/kjx/keysyncData")]
        public SysResult<keylock> syncData(syspara model)
        {
            SysResult<keylock> result = new SysResult<keylock>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
               
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.syncData(model,user);
            return result;
        }
        
        //发送电子钥匙【授权租客】
        [Route("api/kjx/authorization")]
        public SysResult authorization(keylist model)
        {
            SysResult result = new SysResult();
            string jsonData = JsonConvert.SerializeObject(model);
            LogService log = new LogService();
            log.logInfo("发送钥匙输入参数" + jsonData);
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.send(model,user);
            return result;
        }
        //授权初始化
        [Route("api/kjx/Initauthorization")]
        public SysResult<shouquan> Initauthorization(HouseModel model)
        {
            SysResult<shouquan> result = new SysResult<shouquan>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {

                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.QueryContract(model);
            return result;
        }
        //解除绑定
        [Route("api/kjx/notbinding")]
        public SysResult notbinding(paralock model)
        {
            SysResult result = service.notBing(model);
            return result;
        }
        //删除授权
        [Route("api/kjx/deletekey")]
        public SysResult deletekey(keylist model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.delete(model,user);
            return result;
        }
        //获取密码
        [HttpPost]
        [Route("api/kjx/getkeyboardPwd")]
        public SysResult<jpkeylist> getkeyboardPwd(parajpkey modlel)
        {
            SysResult<jpkeylist> result = new SysResult<jpkeylist>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {

                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.get(modlel, user);
            return result;
        }
        
        [HttpPost]
        [Route("api/kjx/getKeyboardPwdVersion")]
        public SysResult<paraversion> getKeyboardPwdVersion(jpkeylist model)
        {
            SysResult<paraversion> result = new SysResult<paraversion>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {

                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.getKeyboardPwdVersion(model,user);
            return result;
        }
        //获取锁的普通钥匙列表
        [HttpPost]
        [Route("api/kjx/listKey")]
        public SysResult<localkey> listKey(keylist model)
        {
            SysResult<localkey> result = new SysResult<localkey>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {

                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.Keylist(model, user);
            return result;
        }
        //删除所有普通钥匙
        [HttpPost]
        [Route("api/kjx/deleteAllKey")]
        public SysResult deleteAllKey(keylist model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {

                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.deleteAllKey(model, user);
            return result;
        }
        //删除钥匙
        [HttpPost]
        [Route("api/kjx/deleteKey")]
        public SysResult delete(keylist model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {

                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.delete(model,user);
            return result;
        }
        //删除单个密码
        [HttpPost]
        [Route("api/kjx/deletekeyboard")]
        public SysResult deletekeyboard(jpkeylist model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {

                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.delete(model, user);
            return result;
        }
        [HttpPost]
        [Route("api/kjx/VerificationPssword")]
        public SysResult VerificationPssword(locklist model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {

                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.yzpassword(model, user);
            return result;
        }
        //重置键盘密码
        [HttpPost]
        [Route("api/kjx/resetKeyboardPwd")]
        public SysResult resetKeyboardPwd(locklist model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {

                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.resetKeyboardPwd(model, user);
            return result;
        }
        //获取锁的键盘密码
        [HttpPost]
        [Route("api/kjx/listKeyboardPwd")]
        public SysResult<jplist> listKeyboardPwd(jpkeylist model)
        {
            SysResult<jplist> result = new SysResult<jplist>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {

                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            result = service.listKeyboardPwd(model, user);
            return result;
        }
        //同步租客钥匙数据
        [Route("api/kjx/zkkeysyncData")]
        public SysResult<keylock> zksyncData(syspara model)
        {
            SysResult<keylock> result = new SysResult<keylock>();
           
            result = service.zksyncData(model);
            return result;
        }
        //获取租客端名下锁列表
        [Route("api/kjx/zklocallist")]
        public SysResult<List<Wraplocklist>> zklist(local model)
        {
            SysResult<List<Wraplocklist>> result = new SysResult<List<Wraplocklist>>();
            result = service.zklocklist(model);
            return result;
        }
        //获取锁的键盘密码
        [HttpPost]
        [Route("api/kjx/zkgetkeyboardPwd")]
        public SysResult<jpkeylist> zkgetkeyboardPwd(parajpkey model)
        {
            SysResult<jpkeylist> result = new SysResult<jpkeylist>();
            
            result = service.zkgetkeyboardPwd(model);
            return result;
        }

        
        
    }
}