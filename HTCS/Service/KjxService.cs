using ControllerHelper;
using DAL;
using DAL.Common;

using Model;
using Model.Base;
using Model.Contrct;
using Model.House;
using Model.TENANT;
using Model.User;
using Newtonsoft.Json;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class KjxService
    {
        GetAcountDAL dal = new GetAcountDAL();
        kjxDAL kjxdal = new kjxDAL();
        public SysResult<T_kjx> login(T_kjx model,string tokenname)
        {
            SysResult<T_kjx> result = new SysResult<T_kjx>(0, "");
            try
            {
                if (model.username != null && model.password != null)
                {
                    //手工登陆
                    model.password = ConvertHelper.GetMd5HashStr(model.password);
                    T_PayMentAcount info = dal.GetAcount(3);
                    T_kjx rekjx = new T_kjx();
                    IDictionary<string, string> txtParams = new Dictionary<string, string>();
                    txtParams.Add("grant_type", "password");
                    txtParams.Add("client_id", info.app_id);
                    txtParams.Add("client_secret", info.public_key);
                    txtParams.Add("redirect_uri", info.redirect_uri);
                    txtParams.Add("username", model.username);
                    txtParams.Add("password", model.password);
                    TlKjx utils = new TlKjx();
                    string str = utils.DoPost("https://api.sciener.cn/oauth2/token", txtParams);
                    rekjx = Newtonsoft.Json.JsonConvert.DeserializeObject<T_kjx>(str);
                    if (rekjx.errcode == "0" || rekjx.errcode == null)
                    {
                        rekjx.r_expires_in = ConvertHelper.Transfer(rekjx.expires_in);
                        rekjx.UserId = model.UserId;
                        rekjx.password = model.password;
                        RedisHtcs redis = new RedisHtcs();
                        redis.SetModel<T_kjx>(tokenname, rekjx);
                        result.numberData = rekjx;
                        result.Code = 0;
                        result.Message = "登陆成功";
                    }
                    else
                    {
                        result.Code = 0;
                        result.Message = "登陆失败:" + rekjx.description;
                    }
                    return result;
                }
              
            }
            catch (Exception ex)
            {
                result.Message="登陆异常"+ ex.ToStr();
            }
            return result;
        }
        

        //注册
        public SysResult<T_kjx> register(T_kjx model)
        {
            SysResult<T_kjx> result = new SysResult<T_kjx>(0, "");
            try
            {
                if (model.username != null && model.password != null)
                {
                    //注册
                    T_PayMentAcount info = dal.GetAcount(3);
                    T_kjx rekjx = new T_kjx();
                    IDictionary<string, string> txtParams = new Dictionary<string, string>();

                    txtParams.Add("clientId", info.app_id);
                    txtParams.Add("clientSecret", info.public_key);
                    txtParams.Add("username", model.username);
                    txtParams.Add("password", ConvertHelper.GetMd5HashStr(model.password));
                    txtParams.Add("date", DateTime.Now.Millisecond.ToString());
                    TlKjx utils = new TlKjx();
                    string str = utils.DoPost("https://api.sciener.cn/v3/user/register", txtParams);
                    rekjx = Newtonsoft.Json.JsonConvert.DeserializeObject<T_kjx>(str);
                    if (rekjx.errcode == "0" || rekjx.errcode == null)
                    {
                        rekjx.r_expires_in = ConvertHelper.Transfer(rekjx.expires_in);
                        rekjx.UserId = model.UserId;
                        rekjx.password = model.password;
                        result.numberData = rekjx;
                        result.Code = 0;
                        result.Message = "注册成功";
                    }
                    else
                    {
                        result.Code = 1;
                        result.Message = "注册失败:" + rekjx.description;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = "注册异常" + ex.ToStr();
            }
            return result;
        }
        public SysResult<T_kjx> resetPassword(T_kjx model)
        {
            SysResult<T_kjx> result = new SysResult<T_kjx>(0, "");
            try
            {
                if (model.username != null && model.password != null)
                {
                    //注册
                    T_PayMentAcount info = dal.GetAcount(3);
                    T_kjx rekjx = new T_kjx();
                    IDictionary<string, string> txtParams = new Dictionary<string, string>();

                    txtParams.Add("clientId", info.app_id);
                    txtParams.Add("clientSecret", info.public_key);
                    txtParams.Add("username", model.username);
                    txtParams.Add("password", ConvertHelper.GetMd5HashStr(model.password));
                    txtParams.Add("date", DateTime.Now.Millisecond.ToString());
                    TlKjx utils = new TlKjx();
                    string str = utils.DoPost("https://api.sciener.cn/v3/user/resetPassword", txtParams);
                    rekjx = Newtonsoft.Json.JsonConvert.DeserializeObject<T_kjx>(str);
                    if (rekjx.errcode == "0" || rekjx.errcode == null)
                    {
                        rekjx.r_expires_in = ConvertHelper.DateByInt(rekjx.expires_in);
                        rekjx.UserId = model.UserId;
                        rekjx.password = model.password;
                        result.numberData = rekjx;
                        result.Code = 0;
                        result.Message = "注册成功";
                    }
                    else
                    {
                        result.Code = 0;
                        result.Message = "注册失败:" + rekjx.description;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = "注册异常" + ex.ToStr();
            }
            return result;
        }
        public SysResult<T_kjx> Gettoken(long CompanyId)
        {
            RedisHtcs redis = new RedisHtcs();
            SysResult<T_kjx > result= new SysResult<T_kjx>();
            T_kjx rekjx = redis.GetModel<T_kjx>("kjx" + CompanyId);
            ElectricDAL elecdal = new ElectricDAL();
            ElecUser elecuser = elecdal.Query(new ElecUser() { CompanyId = CompanyId,paratype=2 });
            result = login(new T_kjx() { username = elecuser.username, password = elecuser.pass }, "kjx" + CompanyId);
            rekjx = result.numberData;
            result.numberData = rekjx;
            return result;
            //if (rekjx != null|| rekjx.r_expires_in < DateTime.Now.AddDays(-7))
            //{
                //rekjx = reflashtoken(rekjx);
                //if (rekjx.errcode != "0" && rekjx.errcode != null)
                //{
                //    ElecUser elecuser = elecdal.Query(new ElecUser() { CompanyId = CompanyId });
                //    result = login(new T_kjx() { username = elecuser.pt_username, password = elecuser.pt_password }, "kjx" + CompanyId);
                //    rekjx = result.numberData;
                //}
            //    ElecUser elecuser = elecdal.Query(new ElecUser() { CompanyId = CompanyId });
            //    result = login(new T_kjx() { username = elecuser.pt_username, password = elecuser.pt_password }, "kjx" + CompanyId);
            //    rekjx = result.numberData;
            //    result.numberData = rekjx;
            //}
            //else
            //{
            //    ElecUser elecuser = elecdal.Query(new ElecUser() { CompanyId = CompanyId });
            //    result = login(new T_kjx() { username = elecuser.pt_username, password = elecuser.pt_password }, "kjx" + elecuser.CompanyId);
            //}
            //return result;
        }
        public SysResult loginout(T_kjx model)
        {
            SysResult result = new SysResult();
            RedisHtcs redis = new RedisHtcs();
            if(redis.Delete("kjx" + model.UserId)==true)
            {
                result = result.SuccessResult("退出成功");
            }
            else
            {
                result = result.FailResult("退出失败");
            }
            return result;
        }
        public T_kjx Getzktoken(string username)
        {
            RedisHtcs redis = new RedisHtcs();
            T_kjx rekjx = redis.GetModel<T_kjx>("zkkjx" + username);
            if (rekjx != null)
            {
                if (rekjx.r_expires_in >= DateTime.Now.AddDays(7))
                {
                    rekjx.username = username;
                    rekjx = reflashtoken(rekjx);
                    if (rekjx.errcode != "0" && rekjx.errcode != null)
                    {
                        TrantDAL trantdal = new TrantDAL();
                        T_Teant teant = trantdal.Query(username);
                        if (teant != null)
                        {
                            rekjx = login(new T_kjx() { username = teant.Pt_UserName, password = teant.Pt_PassWord }, "zkkjx" + username).numberData;
                        }
                    }
                }
            }
            else
            {
                //尝试登陆
                TrantDAL trantdal = new TrantDAL();
                T_Teant teant= trantdal.Query(username);
                if (teant != null)
                {
                    rekjx= login(new T_kjx() { username = teant.Pt_UserName, password = teant.Pt_PassWord }, "zkkjx"+ username).numberData;
                }
              
            }
            return rekjx;
        }
        public T_kjx reflashtoken(T_kjx model)
        {
            SysResult result = new SysResult(0, "");
            try
            {
              
                T_PayMentAcount info = dal.GetAcount(3);
               
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("grant_type", "refresh_token");

                txtParams.Add("client_id", info.app_id);

                txtParams.Add("client_secret", info.public_key);
                txtParams.Add("redirect_uri", info.redirect_uri);
                txtParams.Add("refresh_token", model.refresh_token);
                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/oauth2/token", txtParams);
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<T_kjx>(str);
                if (model.errcode == "0" )
                {
                    RedisHtcs redis = new RedisHtcs();
                    redis.Delete("kjx" + model.username);
                    model.r_expires_in = ConvertHelper.Transfer(model.expires_in);
                    string key = "";
                    if (model.username != null)
                    {
                        key = "kjx" + model.username;
                    }
                    else
                    {
                        key = "kjx" + model.UserId;
                    }
                    redis.SetModel<T_kjx>(key, model);
                    result = result.SuccessResult("成功");
                }
                else
                {
                    result = result.FailResult("失败:" + model.errmsg);
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("",ex);
            }
            return model;
        }
        //锁初始化
        public SysResult initialize(paralock model,T_SysUser user)
        {
            SysResult result = new SysResult(0, "");
            try
            {
                T_kjx kjx = new T_kjx();
                SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
                if (syskjx.Code!=0)
                {
                    result.Code = 1;
                    result.Message = "科技侠登陆失败请联系管理员";
                    return result;
                }
                if (syskjx.numberData != null)
                {
                    kjx = syskjx.numberData;
                }
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> tobject = new Dictionary<string, string>();
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("lockData", model.lockData);
                txtParams.Add("lockAlias", model.lockAlias);
                txtParams.Add("date", ConvertHelper.getsecond());
                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/lock/initialize", txtParams);
                kjx = Newtonsoft.Json.JsonConvert.DeserializeObject<T_kjx>(str);
                //绑定本地
                if (kjx.errcode == "0" || kjx.errcode == null)
                {
                    model.lockId = kjx.lockId;
                    result = initBing(model);
                }
                else
                {
                    result.Code = kjx.errcode.ToInt();
                    result.Message = kjx.errmsg;
                }
                
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 2;
                result.Message = ex.ToString();
            }
            return result;
        }
        //锁列表
        public SysResult<List<Wraplocklist>> locklist(local model,T_SysUser user)
        {
            SysResult<List<Wraplocklist>> result = new SysResult<List<Wraplocklist>>(0, "");
            try
            {
                
                T_kjx kjx = new T_kjx();
                SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
               
                if (syskjx.Code != 0)
                {
                    result.Code = 1;
                    result.Message = "科技侠登陆失败请联系管理员";
                    return result;
                }
                if (syskjx.numberData != null)
                {
                    kjx = syskjx.numberData;
                }
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> tobject = new Dictionary<string, string>();
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("pageNo", model.PageIndex.ToString());
                txtParams.Add("pageSize", model.PageSize.ToString());
                txtParams.Add("date", ConvertHelper.getsecond());
                
                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/lock/list", txtParams);
               
                local savemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<local>(str);
                List<Wraplocklist> lolist = kjxdal.query(savemodel.list,model.HouseId,model.SearchType);
                result.numberData = lolist;
                result.numberCount = savemodel.total;
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 2;
                result.Message = ex.ToString();
            }
            return result;
        }
        //租客锁列表
        public SysResult<List<Wraplocklist>> zklocklist(local model)
        {
            SysResult<List<Wraplocklist>> result = new SysResult<List<Wraplocklist>>(0, "");
            try
            {
                T_kjx kjx = Getzktoken(model.UserName);
                if (kjx == null)
                {
                    result.Code = 1;
                    result.Message ="门锁获取失败请联系管理员";
                    return result;
                }
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> tobject = new Dictionary<string, string>();
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("pageNo", model.PageIndex.ToString());
                txtParams.Add("pageSize", model.PageSize.ToString());
                txtParams.Add("date", ConvertHelper.getsecond());

                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/lock/list", txtParams);
                local savemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<local>(str);
                List<Wraplocklist> lolist = kjxdal.zkquery(savemodel.list,model.HouseId);
                result.numberData = lolist;
                result.numberCount = savemodel.total;
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 2;
                result.Message = ex.ToString();
            }
            return result;
        }
        //同步钥匙数据
        public SysResult<keylock> syncData(syspara model, T_SysUser user)
        {
            SysResult<keylock> result = new SysResult<keylock>(0, "");
            T_kjx kjx = new T_kjx();
            SysResult<T_kjx> syskjx = Gettoken(user.CompanyId );
            if (syskjx.Code != 0)
            {
                result.Code = 1;
                result.Message = "科技侠登陆失败请联系管理员";
                return result;
            }
            if (syskjx.numberData != null)
            {
                kjx = syskjx.numberData;
            }
            try
            {
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> tobject = new Dictionary<string, string>();
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                if (model.lastUpdateDate != 0)
                {
                    txtParams.Add("lastUpdateDate", model.lastUpdateDate.ToString());
                }
                txtParams.Add("date", ConvertHelper.getsecond());
                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/key/syncData", txtParams);
                keylock savemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<keylock>(str);
                savemodel.keyList = savemodel.keyList.Where(p => p.userType == "110301").ToList();
                result.numberData= savemodel;
            }
            catch (Exception ex)
            {
                
            }
            return result;
        }
        //同步租客端钥匙数据
        public SysResult<keylock> zksyncData(syspara model)
        {
            SysResult<keylock> result = new SysResult<keylock>(0, "");
            try
            {
                T_kjx kjx = Getzktoken(model.UserName);
                if (kjx == null)
                {
                    result.Code = 1;
                    result.Message = "没有获取到账户数据请联系管理员";
                    return result;
                }
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> tobject = new Dictionary<string, string>();
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                if (model.lastUpdateDate != 0)
                {
                    txtParams.Add("lastUpdateDate", model.lastUpdateDate.ToString());
                }
                txtParams.Add("date", ConvertHelper.getsecond());
                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/key/syncData", txtParams);
                keylock savemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<keylock>(str);
                savemodel.openid = kjx.openid;
                result.numberData = savemodel;
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        //发送钥匙到租客端
        public SysResult sendzk(keylist key, syspara model)
        {
            SysResult result = new SysResult(0, "");
            try
            {
                SysResult<T_kjx> kjx = Gettoken(model.CompanyId);
                if (kjx.Code != 0)
                {
                    result.Code = 1;
                    result.Message = "没有获取到账户数据请联系管理员";
                    return result;
                }
                cmdsend(key, kjx.numberData);
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("", ex);
            }
            return result;
        }
        //绑定房间
        public SysResult<Wraplocklist> Bing(paralock model)
        {
            SysResult<Wraplocklist> result = new SysResult<Wraplocklist>();
            kjxDAL kjx = new kjxDAL();
            if (kjx.QueryCount(model.lockId) > 0)
            {
                result.Code = 1;
                result.Message = "本锁已经被绑定";
                return result;
            }
            if (model.HouseType == 1)
            {
                HouseDAL dal = new HouseDAL();
                dal.SaveorUpdateHouse1(new HouseModel() { Id = model.HouseId, LocalId = model.lockId }, new string[] { "LocalId" });
            }
            else
            {
                HousePentDAL dal = new HousePentDAL();
                dal.SaveorUpdateHouse(new HousePendent() { ID = model.HouseId, LoclId = model.lockId },new string[] { "LoclId" });
            }
            Wraplocklist remodel = new Wraplocklist();
            remodel.HouseId = model.HouseId;
            remodel.HouseType= model.HouseType;
            result.Code = 0;
            result.Message = "绑定成功";
            result.numberData = remodel;
            return result;
        }
       
        //初始化绑定房间
        public SysResult initBing(paralock model)
        {
            SysResult result = new SysResult();
            kjxDAL kjx = new kjxDAL();
            HouseDAL dal = new HouseDAL();
            //查询是整租还是合租
            HouseQuery modellock = dal.Queryhouse1(model.HouseId);
            if (modellock.RecentType == 1)
            {
                dal.SaveorUpdateHouse1(new HouseModel() { Id = model.HouseId, LocalId = model.lockId }, new string[] { "LocalId"});
            }
            else
            {
                HousePentDAL indentdal = new HousePentDAL();
                indentdal.SaveorUpdateHouse(new HousePendent() { ID = model.HouseId, LoclId = model.lockId }, new string[] { "LoclId" });
            }
            Wraplocklist remodel = new Wraplocklist();
            remodel.HouseId = model.HouseId;
            remodel.HouseType = model.HouseType;
            result.Code = 0;
            result.Message = "绑定成功";
            return result;
        }
        //解除绑定房间
        public SysResult notBing(paralock model)
        {
            SysResult result = new SysResult();
            
            kjxDAL kjx = new kjxDAL();
            if (kjx.QueryCount(model.lockId) == 0)
            {
               return result = result.FailResult("已解绑可重新绑定房间");
            }
            model.lockId = null;
            if (model.HouseType == 1)
            {
                HouseDAL dal = new HouseDAL();
                dal.SaveorUpdateHouse1(new HouseModel() { Id = model.HouseId, LocalId = model.lockId }, new string[] { "LocalId" });
            }
            else
            {
                HousePentDAL dal = new HousePentDAL();
                dal.SaveorUpdateHouse(new HousePendent() { ID = model.HouseId, LoclId = model.lockId },new string[] { "LoclId" });
            }
            result.Code =0;
            result.Message = "解除绑定成功";
            return result;
        }
        public SysResult cmdsend(keylist model, T_kjx kjx)
        {
            SysResult result = new SysResult(0, "");
            //MyRemark remark = new MyRemark();
            //remark.username = model.username;
            //remark.FN_lockname = query.Name;
            //string remarkstr = JsonConvert.SerializeObject(remark);
            T_PayMentAcount info = dal.GetAcount(3);
            IDictionary<string, string> txtParams = new Dictionary<string, string>();
            txtParams.Add("clientId", info.app_id);
            txtParams.Add("accessToken", kjx.access_token);
            txtParams.Add("lockId", model.lockId.ToString());
            txtParams.Add("receiverUsername", "feiniao_" + model.username);
            txtParams.Add("startDate", model.startDate.ToString());
            txtParams.Add("endDate", model.endDate.ToString());
            //txtParams.Add("remarks", remarkstr);
            txtParams.Add("date", ConvertHelper.getsecond());
            TlKjx utils = new TlKjx();
            string str = utils.DoPost("https://api.sciener.cn/v3/key/send", txtParams);
            reclass remodel = Newtonsoft.Json.JsonConvert.DeserializeObject<reclass>(str);
            if (remodel.errcode == "0" || remodel.errcode == null)
            {
                result = shouquan(model.lockId.ToString(), remodel.keyId, kjx.access_token, info);
            }
            else
            {
                result = result.FailResult("发送钥匙失败:" + remodel.description);
            }
            return result;
        }
        //发送钥匙
        public SysResult send(keylist model, T_SysUser user)
        {
            SysResult result = new SysResult(0, "");
            try
            {
                T_kjx kjx = new T_kjx();
                SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
                if (syskjx.Code != 0)
                {
                    result.Code = 1;
                    result.Message = "科技侠登陆失败请联系管理员";
                    return result;
                }
                if (syskjx.numberData != null)
                {
                    kjx = syskjx.numberData;
                }
                HouseLockQuery query=kjxdal.Querybylocal(model.lockId);
                if (query == null)
                {
                    result.Code = 1;
                    result.Message = "请先绑定房间";
                    return result;
                }
                result=cmdsend(model, kjx);
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("",ex);
            }
            return result;
        }
        //授权钥匙
        public SysResult shouquan(string lockid,string keyid,string access_token, T_PayMentAcount info)
        {
            SysResult result = new SysResult(0, "");
            try
            {
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken",access_token);
                txtParams.Add("lockId", lockid);
                txtParams.Add("keyId", keyid);
                txtParams.Add("date", ConvertHelper.getsecond());
                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/key/authorize", txtParams);
                reclass remodel = Newtonsoft.Json.JsonConvert.DeserializeObject<reclass>(str);
                if (remodel.errcode == "0" || remodel.errcode == null)
                {
                    result = result.SuccessResult("授权成功");
                }
                else
                {
                    result = result.FailResult("授权失败:" + remodel.description);
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("", ex);
            }
            return result;
        }
        //初始化授权
        public SysResult<shouquan> QueryContract(HouseModel model)
        {
            ContrctDAL dal = new ContrctDAL();
            SysResult<shouquan> sysresult = new SysResult<shouquan>();
            WrapContract contract= dal.Querybyhouse(model);
            shouquan sq = new Model.shouquan();
            if (contract != null)
            {
                sq.UserName = contract.Name;
                sq.startDate = DateTime.Now;
                sq.endDate = contract.EndTime.AddDays(1);
            }
            sysresult.numberData = sq;
            return sysresult;
        }
        //删除钥匙
        public SysResult delete(keylist model,T_SysUser user)
        {

            SysResult result = new SysResult(0, "");
            try
            {
                T_kjx kjx = new T_kjx();
                SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
                if (syskjx.Code != 0)
                {
                    result.Code = 1;
                    result.Message = "科技侠登陆失败请联系管理员";
                    return result;
                }
                if (syskjx.numberData != null)
                {
                    kjx = syskjx.numberData;
                }
               
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("keyId", model.keyId.ToString());
                txtParams.Add("date", ConvertHelper.getsecond());
                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/key/delete", txtParams);
                reclass remodel = Newtonsoft.Json.JsonConvert.DeserializeObject<reclass>(str);
                if (remodel.errcode == "0" || remodel.errcode == null)
                {
                    
                    result = result.SuccessResult("删除钥匙成功");
                }
                else
                {
                    result = result.FailResult("删除钥匙失败" + remodel.errmsg);
                }

            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("删除异常",ex);
            }
            return result;
        }

        //获取密码
        public SysResult<jpkeylist> get(parajpkey model, T_SysUser user)
        {
            SysResult<jpkeylist> result = new SysResult<jpkeylist>(0, "");
            
            try
            {
                T_kjx kjx = new T_kjx();
                SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
                if (syskjx.Code != 0)
                {
                    result.Code = 1;
                    result.Message = "科技侠登陆失败请联系管理员";
                    return result;
                }
                if (syskjx.numberData != null)
                {
                    kjx = syskjx.numberData;
                }
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> tobject = new Dictionary<string, string>();
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("lockId", model.lockId.ToString());
                txtParams.Add("keyboardPwdVersion","4");
                txtParams.Add("keyboardPwdType", model.keyboardPwdType.ToStr());
                if (model.startDate != 0)
                {
                    txtParams.Add("startDate", model.startDate.ToString());
                }
                if (model.endDate != 0)
                {
                    txtParams.Add("endDate", model.endDate.ToString());
                }
                if (model.isExclusive != 0)
                {
                    txtParams.Add("isExclusive", model.isExclusive.ToString());
                }
                txtParams.Add("date", ConvertHelper.getsecond());

                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/keyboardPwd/get", txtParams);
                jpkeylist savemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<jpkeylist>(str);
                if (savemodel.errcode == "0" || savemodel.errcode == null)
                {
                    savemodel.ShareContent = "您好，您的密码是："+ savemodel.keyboardPwd+ "生效时间："+model.startDate+"类型："+zh(model.keyboardPwdType)+ " 房间：郑AAA 开锁请按 # 密码 # 修改密码请按 *10# 原密码 # 4-8位新密码 # 重复新密码 # 注：密码须在2018-08-02 21:00前使用一次以激活，否则将自动失效。原密码有使用过后才能修改。#号键在键盘右下角，有可能是其它图标。请勿将密码告诉给其他人。";
                    result.Code = 0;
                    result.numberData = savemodel;
                }
                else
                {
                    result.Code = 1;
                    result.Message = "获取密码失败" + savemodel.errmsg + savemodel.description;
                }
            }
            catch (Exception ex)
            {
                result.Code = 2;
                result.Message = "获取密码异常" + ex.ToStr();
            }
            return result;
        }
        public SysResult authorize(string access_token,string lockId,string keyId)
        {
            SysResult result = new SysResult();
            try
            {
               
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken",access_token);
                txtParams.Add("lockId",lockId.ToString());
                txtParams.Add("keyId",keyId.ToString());
                txtParams.Add("date", ConvertHelper.getsecond());
                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/key/authorize", txtParams);
                reclass remodel = Newtonsoft.Json.JsonConvert.DeserializeObject<reclass>(str);
                if (remodel.errcode == "0" || remodel.errcode == null)
                {

                    result = result.SuccessResult("授权成功");
                }
                else
                {
                    result = result.FailResult("授权失败" + remodel.errmsg);
                }

            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("授权异常", ex);
            }
            return result;
        }
        public string zh(int? type)
        {
            if (type == 1)
            {
                return "单次";
            }
            if (type == 2)
            {
                return "永久";
            }
            if (type == 3)
            {
                return "限期";
            }
            if (type == 4)
            {
                return "删除";
            }
            return "";
        }

        //获取锁的键盘密码版本
        public SysResult<paraversion> getKeyboardPwdVersion(jpkeylist model, T_SysUser user)
        {
            SysResult<paraversion> result = new SysResult<paraversion>(0, "");
            try
            {
                T_kjx kjx = new T_kjx();
                SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
                if (syskjx.Code != 0)
                {
                    result.Code = 1;
                    result.Message = "科技侠登陆失败请联系管理员";
                    return result;
                }
                if (syskjx.numberData != null)
                {
                    kjx = syskjx.numberData;
                }
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> tobject = new Dictionary<string, string>();

                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("lockId", model.lockId.ToString());

                txtParams.Add("date", ConvertHelper.getsecond());

                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.ttlock.com.cn/v3/lock/getKeyboardPwdVersion", txtParams);
                paraversion savemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<paraversion>(str);
                result.numberData = savemodel;
                return result;
              
            }
            catch (Exception ex)
            {
                result.Code = 0;
                result.Message = ex.ToStr();
            }
            return result;
        }
        //锁的钥匙列表
        public SysResult<localkey> Keylist(keylist model, T_SysUser user)
        {
            SysResult<localkey> result = new SysResult<localkey>(0, "");

            try
            {
                T_kjx kjx = new T_kjx();
                SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
                if (syskjx.Code != 0)
                {
                    result.Code = 1;
                    result.Message = "科技侠登陆失败请联系管理员";
                    return result;
                }
                if (syskjx.numberData != null)
                {
                    kjx = syskjx.numberData;
                }
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> tobject = new Dictionary<string, string>();

                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("lockId", model.lockId.ToString());
                txtParams.Add("pageNo", model.PageIndex.ToString());
                txtParams.Add("pageSize", model.PageSize.ToString());
                txtParams.Add("date", ConvertHelper.getsecond());

                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/lock/listKey", txtParams);
                localkey savemodel =Newtonsoft.Json.JsonConvert.DeserializeObject<localkey>(str);
                if (savemodel.errcode == "0" || savemodel.errcode == null)
                {
                    result.Code = 0;
                    result.numberData = savemodel;

                }
                else
                {
                    result.Code = 1;
                    result.Message = "获取普通钥匙列表失败" + savemodel.errmsg + savemodel.description;
                }
                result.Code = 0;
                result.Message = "获取数据成功";
                result.numberData = savemodel;
                
            }
            catch (Exception ex)
            {
                result.Code = 1;
                result.Message = ex.ToStr();
            }
            return result;
        }
        //删除所有的普通钥匙(删除)
        public SysResult deleteAllKey(keylist model, T_SysUser user)
        {
            SysResult result = new SysResult(0, "");

            try
            {
                T_kjx kjx = new T_kjx();
                SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
                if (syskjx.Code != 0)
                {
                    result.Code = 1;
                    result.Message = "科技侠登陆失败请联系管理员";
                    return result;
                }
                if (syskjx.numberData != null)
                {
                    kjx = syskjx.numberData;
                }
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> tobject = new Dictionary<string, string>();

                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("lockId", model.lockId.ToString());

                txtParams.Add("date", ConvertHelper.getsecond());

                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/lock/deleteAllKey", txtParams);
                reclass remodel = Newtonsoft.Json.JsonConvert.DeserializeObject<reclass>(str);
                if (remodel.errcode == "0" || remodel.errcode == null)
                {
                   
                    result = result.SuccessResult("删除成功");
                }
                else
                {
                    result = result.FailResult("删除失败" + remodel.errmsg);
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("删除异常",ex);
            }
            return result;
        }
        //删除单个密码
        public SysResult delete(jpkeylist model,T_SysUser user)
        {
            SysResult result = new SysResult(0, "");

            try
            {
                SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
                T_kjx kjx = new T_kjx();
                if (syskjx.Code != 0)
                {
                    result.Code = 1;
                    result.Message = "科技侠登陆失败请联系管理员";
                    return result;
                }
                if (syskjx.numberData != null)
                {
                    kjx = syskjx.numberData;
                }
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("lockId", model.lockId.ToString());
                txtParams.Add("keyboardPwdId", model.keyboardPwdId.ToString());
                txtParams.Add("deleteType", model.deleteType.ToString());
                txtParams.Add("date", ConvertHelper.getsecond());

                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/keyboardPwd/delete", txtParams);
                reclass savemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<reclass>(str);
                //修改键盘密码
                if (savemodel.errcode == "0" || savemodel.errcode == null)
                {
                    //var mo = from m in db.locklist where m.lockId == model.lockId select m;
                    //model = mo.FirstOrDefault();
                    //model.deletePwd =model.deletePwd;
                    //ldal.savelocallist(model);
                    result = result.SuccessResult("删除密码成功");
                }
                else
                {
                    result = result.FailResult("删除密码失败" + savemodel.errmsg);
                }

            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("删除异常",ex);
            }
            return result;
        }
        //重置键盘密码
        public SysResult resetKeyboardPwd(locklist model, T_SysUser user)
        {
            SysResult result = new SysResult(0, "");

            try
            {
                T_kjx kjx = new T_kjx();
                SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
                if (syskjx.Code != 0)
                {
                    result.Code = 1;
                    result.Message = "科技侠登陆失败请联系管理员";
                    return result;
                }
                if (syskjx.numberData != null)
                {
                    kjx = syskjx.numberData;
                }
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("lockId", model.lockId.ToString());
                txtParams.Add("pwdInfo", model.pwdInfo.ToString());
                txtParams.Add("timestamp", model.timestamp.ToString());

                txtParams.Add("date", ConvertHelper.getsecond());
                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/lock/resetKeyboardPwd", txtParams);
                reclass remodel = Newtonsoft.Json.JsonConvert.DeserializeObject<reclass>(str);
                if (remodel.errcode == "0" || remodel.errcode == null)
                {
                    //var mo = from m in db.keylist where m.lockId == model.lockId select m;
                    //model = mo.FirstOrDefault();
                    //ldal.deleteptkey(model);
                    result = result.SuccessResult("重置键盘密码成功");
                }
                else
                {
                    result = result.FailResult("重置键盘密码失败" + remodel.errmsg);
                }

            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("重置键盘密码", ex);
            }
            return result;
        }
        //验证密码
        public SysResult yzpassword(locklist model, T_SysUser user)
        {
            SysResult result = new SysResult(0, "");
            model.password = ConvertHelper.GetMd5HashStr(model.password);
            T_kjx kjx = new T_kjx();
            SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
            if (syskjx.Code != 0)
            {
                result.Code = 1;
                result.Message = "科技侠登陆失败请联系管理员";
                return result;
            }
            if (syskjx.numberData != null)
            {
                kjx = syskjx.numberData;
            }
            if (model.password != kjx.password)
            {
                result.Code = 1;
                result=result.FailResult("密码错误");
            }
            return result;
        }
        //获取键盘锁密码列表
        public SysResult<jplist> listKeyboardPwd(jpkeylist model, T_SysUser user)
        {
            SysResult<jplist> result = new SysResult<jplist>(0, "");
            try
            {
                T_kjx kjx = new T_kjx();
                SysResult<T_kjx> syskjx = Gettoken(user.CompanyId);
                if (syskjx.Code != 0)
                {
                    result.Code = 1;
                    result.Message = "科技侠登陆失败请联系管理员";
                    return result;
                }
                if (syskjx.numberData != null)
                {
                    kjx = syskjx.numberData;
                }
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> tobject = new Dictionary<string, string>();
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("lockId", model.lockId.ToString());
                txtParams.Add("pageNo", model.PageIndex.ToString());
                txtParams.Add("pageSize", model.PageSize.ToString());
                txtParams.Add("date", ConvertHelper.getsecond());
                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/lock/listKeyboardPwd", txtParams);
                jplist savemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<jplist>(str);
                result.numberData = savemodel;
                result.Code = 0;
                result.Message = "获取数据成功";
            }
            catch (Exception ex)
            {
                result.Code = 2;
                result.Message = ex.ToStr();
            }
            return result;
        }
        //获取租客键盘锁密码
        public SysResult<jpkeylist> zkgetkeyboardPwd(parajpkey model)
        {
            SysResult<jpkeylist> result = new SysResult<jpkeylist>(0, "");
            ContrctService contservice = new ContrctService();
            try
            {
                T_kjx kjx = Getzktoken(model.UserName);
            
                if (kjx == null)
                {
                    result.Code = 1;
                    result.Message = "门锁获取失败请联系管理员";
                    return result;
                }
                //查询租期
                long startDate = 0;
                long endDate = 0;
                SysResult sresult = contservice.getcontractbylock(model.lockId.ToStr(),out startDate, out endDate);
                result.Code = sresult.Code;
                result.Message = sresult.Message;
                if (result.Code != 0)
                {
                    return result;
                }
                model.startDate = startDate;
                model.endDate = endDate;
                T_PayMentAcount info = dal.GetAcount(3);
                IDictionary<string, string> tobject = new Dictionary<string, string>();
                IDictionary<string, string> txtParams = new Dictionary<string, string>();
                txtParams.Add("clientId", info.app_id);
                txtParams.Add("accessToken", kjx.access_token);
                txtParams.Add("lockId", model.lockId.ToString());
                txtParams.Add("keyboardPwdVersion", "4");
                txtParams.Add("keyboardPwdType", model.keyboardPwdType.ToStr());
                if (model.startDate != 0)
                {
                    txtParams.Add("startDate", model.startDate.ToString());
                }
                if (model.endDate != 0)
                {
                    txtParams.Add("endDate", model.endDate.ToString());
                }
                if (model.isExclusive != 0)
                {
                    txtParams.Add("isExclusive", model.isExclusive.ToString());
                }
                txtParams.Add("date", ConvertHelper.getsecond());

                TlKjx utils = new TlKjx();
                string str = utils.DoPost("https://api.sciener.cn/v3/keyboardPwd/get", txtParams);
                jpkeylist savemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<jpkeylist>(str);
                if (savemodel.errcode == "0" || savemodel.errcode == null)
                {
                    savemodel.ShareContent = "您好，您的开门密码是：" + savemodel.keyboardPwd + "生效时间：" + ConvertHelper.DateByInt(model.startDate) + "结束时间：" + ConvertHelper.DateByInt(model.endDate)+ "类型：" + zh(model.keyboardPwdType);
                    result.Code = 0;
                    result.numberData = savemodel;
                }
                else
                {
                    result.Code = 1;
                    result.Message = "获取密码失败" + savemodel.errmsg + savemodel.description;
                }
            }
            catch (Exception ex)
            {
                result.Code = 2;
                result.Message = "获取密码异常" + ex.ToStr();
            }
            return result;
        }
    }
}
