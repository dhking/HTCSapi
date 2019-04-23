using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
using Model.User;
using ControllerHelper;
using Model.Menu;
using Model.Bill;
using Model.Base;
using Model.House;
using DAL.Common;

namespace Service
{
    public  class SysUserService
    {
        UserDAL1 dal = new UserDAL1();
        //开通在线缴费
        public SysResult Openpay(T_SysUser model,int kaiorguan)
        {
            SysResult result = new SysResult();
            BaseDataDALL bdal = new BaseDataDALL();
            T_account account = bdal.queryaccount(model.CompanyId);
            if (Viltyzm(model.yzm, account.phone,5))
            {
                account.OnlinePay = kaiorguan;
                if (bdal.saveaccount(account) > 0)
                {
                    return result = result.SuccessResult("开通在线支付成功");
                }
                else
                {
                    return result = result.SuccessResult("开通失败，请联系客服人员为您处理");
                }
            }
            result=result.FailResult("验证码错误");
            return result;
        }
        public string GetKey(int Type)
        {
            if (Type == 2)
            {
                return "yzm_";
            }
            if (Type ==1)
            {
                return "zyzm_";
            }
            if (Type == 3)
            {
                return "yzm_";
            }
            if (Type == 4)
            {
                return "cuizuyzm_";
            }
            if (Type == 5)
            {
                return "kaitongyzm_";
            }
            if (Type == 6)
            {
                return "zhanghao_";
            }
            if (Type == 8)
            {
                return "zfmm_";
            }
            return "";
        }
        //登陆
        public SysResult<T_SysUser> Login(T_SysUser model)
        {
            SysResult<T_SysUser> result = new SysResult<T_SysUser>();
            if ((model == null || model.Name == null)&&model.Mobile==null)
            {
                result.Code = 1;
                result.Message = "请填写手机号或用户名";
                return result;
            }
            if (model == null || model.Password == null)
            {
                result.Code = 1;
                result.Message = "请填写密码";
                return result;
            }
            T_SysUser user = new T_SysUser();
            user = dal.Login(model);
            if (user!=null)
            {
                RoleDAL roledal = new RoleDAL();
                List<T_SysUserRole> listrole = dal.listrole(user.Id);
                List<long> roleids = listrole.Select(p => p.SysRoleId).ToList();
                if (roleids.Count > 0)
                {
                    user.roles = roledal.queryrole(roleids);
                    user.listpression = Querybasepressionbuuser(roleids).numberData;
                }
                JWTHelp jwt = new JWTHelp();
                T_SysUser reuser = new T_SysUser();
                reuser = user;
                //string access_token = jwt.getToken(user.Name, user.Password);
                string access_token = ConvertHelper.GetMd5HashStr(reuser.Id.ToStr()); ;
                RedisHtcs rds = new RedisHtcs();
                string key = "sysuser_key" + access_token;
                reuser.Id = user.Id;
                reuser.access_token = key;
                rds.SetModel<T_SysUser>(key, reuser);
             
                result.Code = 0;
                result.Message = "登陆成功";
                result.numberData = reuser;
            }
            else
            {
                result.Code = 1;
                result.Message = "用户名或者密码错误";
            }
            return result;
        }
        public SysResult<T_SysUser> QueryUser(T_SysUser model)
        {
            BaseDataDALL bdal = new BaseDataDALL();
            SysResult<T_SysUser> result = new SysResult<T_SysUser>();
            T_SysUser user = dal.QueryUerbyid(model);
            user.listrole1 = dal.Querynopageuserrole(new T_SysUserRole() { SysUserId = user.Id });
          
            result.numberData = user;
            return result;
        }
        public SysResult<T_SysUser> Queryfgy(HouseModel model)
        {
            SysResult<T_SysUser> result = new SysResult<T_SysUser>();
            T_SysUser user = new T_SysUser();
            //查询房管员编号
            HouseDAL housedal = new HouseDAL();
            HouseModel housemode = housedal.Queryhouse(model);
            //查询系统用户
            if(housemode!=null&& housemode.HouseKeeper!=0)
            {
                user = dal.QueryUerbyid(new T_SysUser() { Id = housemode.HouseKeeper });
                result.numberData = user;
            }
            else
            {
                result.Code = 1;
                result.Message = "没有房管员";
            }
            return result;
        }
        public SysResult Logout(string access_token)
        {
            SysResult result = new SysResult();
            RedisHtcs redis = new RedisHtcs();
            if (redis.Delete(access_token) == true)
            {
                result = result.SuccessResult("退出成功");
            }
            else
            {
                result = result.SuccessResult("退出失败");
            }
            return result;
        }
        //修改个人资料
        public SysResult updatedata(T_SysUser user)
        {
            SysResult result = new SysResult();
            T_SysUser queryuser = dal.QueryUerbyid(user);
            if (user.nickname != null && user.nickname != "")
            {
                queryuser.nickname = user.nickname;
            }
            if (user.userimg != null && user.userimg != "")
            {
                queryuser.userimg = user.userimg;
            }
            if (user.city != null && user.city != "")
            {
                queryuser.city = user.city;
            }
            if (user.province != null && user.province != "")
            {
                queryuser.province = user.province;
            }
            if (dal.updateuser(queryuser, new string[] { }) > 0)
            {
                result = result.SuccessResult("操作成功");
            }
            else
            {
                result = result.FailResult("操作失败");
            }
            return result;
        }
        public string getguid()
        {
            Random rad = new Random();//实例化随机数产生器rad；
            int value = rad.Next(1000, 10000);
            return value.ToString();
        }
        //发送催租短信
        public SysResult cuizu(T_WrapBill model)
        {
            SysResult result = new SysResult();
            yzRequest req = new yzRequest();
            req = GetRequest(4, model.Phone);
            string message = "";
            SendMessageDAL dal = new SendMessageDAL();
            if (dal.SendMessage(req, out message))
            {
                result = result.SuccessResult("发送成功");
            }
            else
            {
                result = result.FailResult("发送失败" + message);
            }
            return result;
        }
        public SysResult pcuizu(List<T_WrapBill> model)
        {
            SysResult result = new SysResult();
            model=model.DistinctBy(p => p.Phone).ToList();
            foreach(var  mo in model)
            {
                yzRequest req = new yzRequest();
                req = GetRequest(4, mo.Phone);
                string message = "";
                SendMessageDAL dal = new SendMessageDAL();
                if (dal.SendMessage(req, out message))
                {
                    result = result.SuccessResult("发送成功");
                }
                else
                {
                    result = result.FailResult("发送失败" + message);
                }
            }
            return result;
        }
        public SysResult Sendyzm(yzRequest req)
        {
            SysResult result = new Model.SysResult();

            SendMessageDAL yzdal = new SendMessageDAL();
            string errmsg = "";
            if (req == null || req.Phone == "")
            {
                return result = result.FailResult("手机号不能为空");
            }
            string yzm = getguid();
            req = GetRequest(req.Type, req.Phone);
            bool re = yzdal.SendMessage(req, out errmsg);
            if (re == false)
            {
                return result = result.FailResult("发送失败" + errmsg);
            }
            else
            {
                RedisHtcs redis = new RedisHtcs();
                string key = GetKey(req.Type) + req.Phone;
                if (req.Type == 5|| req.Type == 6)
                {
                    bool reredis = redis.Set(key, yzm,300);
                }
                else
                {
                    bool reredis = redis.Set(key, yzm);
                }
                
                result = result.SuccessResult("发送成功");
            }
            return result;
        }
        public yzRequest GetRequest(int Type, string Phone)
        {
            yzRequest req = new yzRequest();
            req.Phone = Phone;
            req.yzm = getguid();
            req.Type = Type;
            if (Type == 1)
            {
                req.Temp = "{\"number\":\"" + req.yzm + "\"}";
                req.TemplateCode = "SMS_150174612";
            }
            if (Type == 2)
            {
                req.Temp = "{\"number\":\"" + req.yzm + "\"}";
                req.TemplateCode = "SMS_150184695";
            }
            if (Type == 3)
            {
                req.Temp = "{\"number\":\"" + req.yzm + "\"}";
                req.TemplateCode = "SMS_139240863";
            }
            if (Type == 4)
            {
                req.Temp = "{\"number\":\"" + req.yzm + "\"}";
                req.TemplateCode = "SMS_150742900";
            }
            if (Type == 5)
            {
                req.Temp = "{\"number\":\"" + req.yzm + "\"}";
                req.TemplateCode = "SMS_150742913";
            }
            if (Type == 6)
            {
                req.Temp = "{\"number\":\"" + req.yzm + "\"}";
                req.TemplateCode = "SMS_162736189";
            }
            //绑定合同
            if (Type == 7)
            {
                req.TemplateCode = "SMS_161593766";
            }
            //支付密码
            if (Type == 8)
            {
                req.Temp = "{\"number\":\"" + req.yzm + "\"}";
                req.TemplateCode = "SMS_162730956";
            }
            
            return req;
        }
        //一一公寓
        //public yzRequest GetRequest(int Type, string Phone)
        //{
        //    yzRequest req = new yzRequest();
        //    req.Phone = Phone;
        //    req.yzm = getguid();
        //    req.Type = Type;
        //    if (Type == 1)
        //    {
        //        req.Temp = "{\"number\":\"" + req.yzm + "\"}";
        //        req.TemplateCode = "SMS_163438170";
        //    }
        //    if (Type == 2)
        //    {
        //        req.Temp = "{\"number\":\"" + req.yzm + "\"}";
        //        req.TemplateCode = "SMS_163432944";
        //    }
        //    if (Type == 3)
        //    {
        //        req.Temp = "{\"number\":\"" + req.yzm + "\"}";
        //        req.TemplateCode = "SMS_163438171";
        //    }
        //    if (Type == 4)
        //    {
        //        req.Temp = "{\"number\":\"" + req.yzm + "\"}";
        //        req.TemplateCode = "SMS_163438168";
        //    }
        //    if (Type == 5)
        //    {
        //        req.Temp = "{\"number\":\"" + req.yzm + "\"}";
        //        req.TemplateCode = "SMS_163432943";
        //    }
        //    if (Type == 6)
        //    {
        //        req.Temp = "{\"number\":\"" + req.yzm + "\"}";
        //        req.TemplateCode = "SMS_163438166";
        //    }
        //    //绑定合同
        //    if (Type == 7)
        //    {
        //        req.TemplateCode = "SMS_163432942";
        //    }
        //    //支付密码
        //    if (Type == 8)
        //    {
        //        req.Temp = "{\"number\":\"" + req.yzm + "\"}";
        //        req.TemplateCode = "SMS_163438167";
        //    }

        //    return req;
        //}
        public SysResult withdrawal(T_Record model)
        {
            SysResult result = new SysResult();
            
            result.Message = "申请成功";
            //收款账户
            AccountNameDAL basedal = new AccountNameDAL();
            accountbank account= basedal.queryAccount(new accountbank() { Id=model.accountbankid});
            //查询账户
            BaseDataDALL accountbasedal = new BaseDataDALL();
            T_account taccount = accountbasedal.queryaccount(model.CompanyId);
            if (account != null)
            {
                if (model.type == 1)
                {
                    if (account.accountname == null || account.account == null)
                    {
                        result.Code = 1;
                        result.Message = "支付宝账户或者账户名称不为空";
                        return result;
                    }
                }
                if (model.type == 2)
                {
                    if (account.bank == null || account.accountname == null|| account.account == null|| account.zhbank == null)
                    {
                        result.Code = 1;
                        result.Message = "银行卡信息未完善";
                        return result;
                    }
                }
                if (taccount.password==null)
                {
                    result.Code = 1;
                    result.Message = "您还没有设置支付密码";
                    return result;
                }
                if (taccount.Amount<model.amount)
                {
                    result.Code = 1;
                    result.Message = "余额不足，余额为"+ taccount.Amount+"元";
                    return result;
                }
                if (taccount.password == model.password)
                {
                    if(model.type == 1)
                    {
                        model.name = account.accountname;
                        model.account = account.account;
                    }
                   
                    if (model.type == 2)
                    {
                        model.bank = account.bank;
                        model.name = account.accountname;
                        model.account = account.account;
                    }
                    model.createtime = DateTime.Now;
                    if (dal.Saverecord(model) > 0)
                    {
                        taccount.Amount = taccount.Amount - model.amount;
                        accountbasedal.saveaccount1(taccount);
                    }
                }
                else
                {
                    result.Code = 1;
                    result.Message = "支付密码错误";
                }
            }
            else
            {
                result.Code = 1;
                result.Message = "收款账户不存在";
            }
            
            return result;
        }

        public SysResult<T_SysUser> register(T_SysUser model)
        {
            SysResult<T_SysUser> result = new Model.SysResult<T_SysUser>();
            if (Viltyzm(model.yzm, model.Mobile,2))
            {
                model.range = 5;
                UserDAL1 userdal = new DAL.UserDAL1();
                if (model == null || model.Mobile == null)
                {
                    result.Code = 1;
                    result.Message = "请填写手机号";
                    return result;
                }
                if (model == null || model.Password == null)
                {
                    result.Code = 1;
                    result.Message = "请填写密码";
                    return result;
                }
                if (model == null || model.yzm == null)
                {
                    result.Code = 1;
                    result.Message = "请填写验证码";
                    return result;
                }
                if (userdal.Querybyphone(model.Mobile) > 0)
                {
                    result.Code = 1;
                    result.Message = "手机号已注册";
                    return result;

                }
                else
                {
                    model.Name = model.Mobile;
                    List<T_SysUserRole> listrole = new List<T_SysUserRole>();
                    T_SysUserRole role = new T_SysUserRole();
                    role.SysRoleId = 45;
                    listrole.Add(role);
                    model.listrole = listrole;
                    
                    //注册公司
                    BaseDataDALL cdal = new BaseDataDALL();
                    long companyid=cdal.saveaccount(new T_account() {smsnumber=100,contractnumber=100 });
                    model.CompanyId = companyid;
                    
                    long id = userdal.adduser(model);
                    if (id > 0)
                    {
                        //注册科技侠
                        KjxService kjx = new KjxService();
                        SysResult<T_kjx> remodel = kjx.register(new T_kjx()       { username = model.Mobile, password = model.Password });
                        if (remodel.Code == 0 && remodel.numberData != null)
                        {
                            ElectricDAL elecdal = new ElectricDAL();
                            elecdal.addelec(new ElecUser()
                            {
                                username = remodel.numberData.username,
                                pass = model.Password,
                                paratype = 2,
                                CompanyId = companyid
                            });
                        }
                        JWTHelp jwt = new JWTHelp();
                        T_SysUser re = new T_SysUser();
                        re = model;
                        re.Id = id;
                        re.roles = "老板";
                        RedisHtcs rds = new RedisHtcs();
                        string key = "sysuser_key" + jwt.getToken(re.Mobile, re.Password);
                        re.token = key;
                        rds.SetModel<T_SysUser>(key, re);
                        re.Password = null;
                        result.numberData = re;
                        result.Code = 0;
                        result.Message = "注册成功";
                        return result;
                    }
                    else
                    {
                        result.Code = 1;
                        result.Message = "注册失败";
                        return result;
                    }

                }
            }
            else
            {
                result.Code = 1;
                result.Message = "验证码错误";
                return result;

            }
        }
        public SysResult<T_SysUser> Retrieve(T_SysUser model)
        {
            SysResult<T_SysUser> result = new Model.SysResult<T_SysUser>();
           
            if (Viltyzm(model.yzm, model.Mobile,1))
            {
                UserDAL1 userdal = new DAL.UserDAL1();
                if (model == null || model.Mobile == null)
                {
                    result.Code = 1;
                    result.Message = "请填写手机号";
                    return result;
                }
                if (model == null || model.Password == null)
                {
                    result.Code = 1;
                    result.Message = "请填写密码";
                    return result;
                }
                if (model == null || model.yzm == null)
                {
                    result.Code = 1;
                    result.Message = "请填写验证码";
                    return result;
                }
                T_SysUser user = dal.QueryUerbyid(model);
                if (user != null)
                {
                    user.Password = model.Password;
                    long id = userdal.ModifyPassword(user);
                    if (id > 0)
                    {
                        result.Code = 0;
                        result.Message = "修改成功";
                        return result;
                    }
                    else
                    {
                        result.Code = 1;
                        result.Message = "修改失败";
                        return result;
                    }
                }
                else
                {
                    result.Code = 1;
                    result.Message = "用户不存在";
                    return result;
                }
             
            }
            else
            {
                result.Code = 1;
                result.Message = "验证码错误";
                return result;

            }
        }
        public bool Viltyzm(string yzm, string phone, int type)
        {
            if (yzm == null)
            {
                return false;
            }
            string rdskey = GetKey(type);
            RedisHtcs redis = new RedisHtcs();
            string key = rdskey + phone;
            string realyzm = redis.Get(key);
            if (realyzm != yzm)
            {
                return false;
            }
            else
            {
                //redis.Delete(key);
                return true;
            }
        }
        public Dictionary<string, string> getstr(string stores)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string area = null;
            string city = null;
            FormatterDAL fordal = new FormatterDAL();
            long[] arrs = new long[] { };
            string[] strarrs = new string[] { };
            if (stores != null)
            {
                strarrs = stores.Split(",");
                if (strarrs.Length > 0)
                {
                    arrs = Array.ConvertAll<string, long>(strarrs, long.Parse);
                }
            }
            List<T_CellName> cellnames = fordal.getparent(arrs);
            if (cellnames != null)
            {
                List<T_CellName> areaname= fordal.getparent(cellnames.Select(p=>p.parentid).ToArray()).DistinctBy(p => p.Name).ToList();
                area = string.Join(",", areaname.Select(p => p.Name).ToList());
                if (areaname != null)
                {
                    List<T_CellName> cityname = fordal.getparent(areaname.Select(p => p.parentid).ToArray()).DistinctBy(p => p.Name).ToList(); ;
                    city = string.Join(",", cityname.Select(p => p.Name).ToList());
                }
            }
            dic.Add("city", city);
            dic.Add("area", area);
            return dic;

        }
        public SysResult addUser(T_SysUser model)
        {
            SysResult result = new SysResult();
            T_SysUser user = new T_SysUser();
            Dictionary<string, string> dic = getstr(model.storeid);
            model.area = dic["area"];
            model.city = dic["city"];
            if (dal.adduser(model) > 0)
            {
                if (model.registrationId !=null)
                {
                    PushService pushservice = new PushService();
                    pushservice.Zhuce(new ParamPhsh() { Mobile = model.Name, deviceid = model.registrationId });
                }
                if (model.Id != 0)
                {
                    T_SysUser query = dal.QueryUerbyid(new T_SysUser() { Id = model.Id });
                    List<T_SysUserRole> listrole = dal.listrole(user.Id);
                    List<long> roleids = listrole.Select(p => p.SysRoleId).ToList();
                    query.listpression = Querybasepressionbuuser(roleids).numberData;
                    JWTHelp jwt = new JWTHelp();
                    string access_token =ConvertHelper.GetMd5HashStr(model.Id.ToStr());
                    RedisHtcs rds = new RedisHtcs();
                    string key = "sysuser_key" + access_token;
                    rds.SetModel<T_SysUser>(key, query);
                }
                result = result.SuccessResult("保存成功");
            }
            else
            {
                result = result.SuccessResult("保存失败");
            }
            return result;
        }
        public SysResult<List<T_SysUser>> Querybase(T_SysUser model, OrderablePagination orderablePagination)
        {
            SysResult<List<T_SysUser>> sysresult = new SysResult<List<T_SysUser>>();
            List<T_SysUser> data = new List<T_SysUser>();
            data = dal.Querylist(model, orderablePagination);
            sysresult.numberData = data;
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
        public SysResult<List<T_SysRole>> Querybaserole(T_SysRole model, OrderablePagination orderablePagination)
        {
            SysResult<List<T_SysRole>> sysresult = new SysResult<List<T_SysRole>>();
            List<T_SysRole> data = new List<T_SysRole>();
            data = dal.Querylistrole(model, orderablePagination);
            sysresult.numberData = data;
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
        public SysResult<List<T_Button>> Querybasebutton(T_Button model, OrderablePagination orderablePagination)
        {
            SysResult<List<T_Button>> sysresult = new SysResult<List<T_Button>>();
            List<T_Button> data = new List<T_Button>();
            
            data = dal.Querylistbutton(model, orderablePagination);
            sysresult.numberData = data;
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
        public SysResult<List<T_SysUserRole>> Querybaseuserrole(T_SysUserRole model, OrderablePagination orderablePagination)
        {
            SysResult<List<T_SysUserRole>> sysresult = new SysResult<List<T_SysUserRole>>();
            List<T_SysUserRole> data = new List<T_SysUserRole>();
           
            data = dal.Querylistuserrole(model, orderablePagination);
            sysresult.numberData = data;
            sysresult.numberCount = orderablePagination.TotalCount;
            
            return sysresult;
        }
        public SysResult<List<WrapSysUserRole>> Querynopageuserrole(T_SysUserRole model)
        {
            SysResult<List<WrapSysUserRole>> sysresult = new SysResult<List<WrapSysUserRole>>();
            List<WrapSysUserRole> data = new List<WrapSysUserRole>();
            data = dal.Querynopageuserrole(model);
            sysresult.numberData = data;
            return sysresult;
        }
        public SysResult<List<T_SysRole>> Querylistrolenopage(T_SysRole model)
        {
            SysResult<List<T_SysRole>> result = new SysResult<List<T_SysRole>>();
            List<T_SysRole> data = new List<T_SysRole>();
            int count = 0;
            data = dal.Querylistrolenopage(model, out count);
            result.numberData = data;
            result.numberCount = count;
            return result;
        }
        public SysResult<List<T_Shop>> Querylistshopnopage(T_Shop model)
        {
            SysResult<List<T_Shop>> result = new SysResult<List<T_Shop>>();
            List<T_Shop> data = new List<T_Shop>();
            int count = 0;
            data = dal.Querylistshopnopage(model, out count);
            result.numberData = data;
            result.numberCount = count;
            return result;
        }
        public SysResult Edit(PlAction<T_SysUserRole, T_SysUser> model)
        {
            SysResult result = new SysResult();

            using (var tran = dal.Database.BeginTransaction())
            {

                long parentid = dal.mainadd(model.main);

                foreach (var m in model.dataadd)
                {
                    m.SysUserId = model.main.Id;
                }
                foreach (var m in model.update)
                {
                    m.SysUserId = model.main.Id;
                }
                try
                {
                    dal.wrap(model);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result = result.FailResult(ex.ToString());
                }
                if (parentid > 0)
                {
                    tran.Commit();
                    result = result.SuccessResult("操作成功");
                }
                else
                {
                    tran.Rollback();
                    result = result.FailResult("操作失败");
                }
            }
            return result;
        }
        //保存或者编辑
        public SysResult btnEdit(T_Button model)
        {
            SysResult result = new SysResult();
            long parentid = dal.buttonadd(model);
            if (parentid > 0)
            {
                result = result.SuccessResult("操作成功");
            }
            else
            {

                result = result.FailResult("操作失败");
            }
            return result;
        }
        //删除按钮操作
        public SysResult delete(long ids)
        {
            SysResult result = new SysResult();
            long parentid = dal.delete(ids);
            if (parentid > 0)
            {
                result = result.SuccessResult("删除成功");
            }
            else
            {

                result = result.FailResult("删除失败");
            }
            return result;
        }

        public SysResult<List<Pression>> Querybasepression(long RoleId,long companyid)
        {
            MenuDAL menudal = new MenuDAL();
            SysResult<List<Pression>> sysresult = new SysResult<List<Pression>>();
            int[] arrsign = new int[] { 1,2};
            if (companyid == 1)
            {
                arrsign = new int[] { 1, 2, 3 };
            }
            sysresult.numberData = menudal.Querylistpression(RoleId, arrsign);
            return sysresult;
        }
        //根据用户信息查询权限
        public SysResult<List<Pression>> Querybasepressionbuuser(List<long> role)
        {
            MenuDAL menudal = new MenuDAL();
            SysResult<List<Pression>> sysresult = new SysResult<List<Pression>>();
            
            sysresult.numberData = menudal.Querylistpressionbyuser(role);
            return sysresult;
        }
        public SysResult Savepression(WrapPression Pression)
        {
            SysResult result = new SysResult();
            List<Pression> updateaddPression = Pression.savedata.Where(p =>
            p.checkedd == true).ToList();
            List<T_SysRoleMenu> model= updateaddPression.Select(p=>new T_SysRoleMenu() {MenuId=p.MenuId
                ,ButtonId=p.ButtonId,RoleId=p.RoleId
            }).Distinct().ToList();
            List<Pression> deletePression = Pression.changedata.Where(p =>
            p.checkedOld == true && p.checkedd == false).Distinct().ToList();
            List<T_SysRoleMenu> deletemodel = deletePression.Select(p => new T_SysRoleMenu()
            {
                MenuId = p.MenuId,
                ButtonId = p.ButtonId,
                RoleId = p.RoleId
            }).ToList();
            bool update = dal.Pladdrolemenu(model, Pression.RoleId);
            bool delete = dal.deleterolemenu(deletemodel, Pression.RoleId);
            if (update)
            {
                result = result.SuccessResult("成功");
            }
            else
            {
                result = result.SuccessResult("失败");
            }
            return result;
        }
    }
}
