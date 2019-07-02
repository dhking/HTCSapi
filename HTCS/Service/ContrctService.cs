using ControllerHelper;
using DAL;
using DAL.Common;
using Model;
using Model.Base;
using Model.Bill;
using Model.Contrct;
using Model.House;
using Model.TENANT;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public   class ContrctService
    {
        ContrctDAL dal = new ContrctDAL();
        public SysResult SaveContract(T_Contrct model, ElecUser user, T_SysUser sysuser)
        {
            SysResult result = new SysResult();
            //检查房间是否绑定智能电表
            if (model.Otherfee != null)
            {
               if( model.Otherfee.Count != model.Otherfee.Select(c => new { c.Name }).Distinct().Count())
                {
                    result.Code = 1;
                    result.Message = "押金或杂费项不能重复";
                    return result;
                }
            }
            //判断是新增还是修改
            int type = 0;
            model.Status = 5;
            if (model.Id != 0)
            {
                type = 1;
            }
            using (var context = new ContrctDAL())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string errmsg = "";
                        int other = 1;
                        long tract = 1;
                        if (model.Teant != null)
                        {
                            T_Teant teant = context.queryteant(model.Teant);
                            if (teant != null)
                            {
                                teant.Document = model.Teant.Document;
                                teant.DocumentType = model.Teant.DocumentType;
                                teant.Name = model.Teant.Name;
                                teant.Weinxin = model.Teant.Weinxin;
                                teant.QQ = model.Teant.QQ;
                                teant.Work = model.Teant.Work;
                                teant.Hobby = model.Teant.Hobby;
                                teant.Name = model.Teant.Name;
                                teant.Sex = model.Teant.Sex;
                                teant.Zidcard = model.Teant.Zidcard;
                                if (string.IsNullOrEmpty(teant.Pt_PassWord))
                                {
                                    teant.Pt_PassWord = ConvertHelper.getsecond();
                                }
                                tract = context.updatetrant(teant);
                            }
                            else
                            {
                                string password = ConvertHelper.getsecond();
                                model.Teant.Pt_PassWord = password;
                                tract = context.SaveTrent(model.Teant);
                            }
                            model.TeantId = tract;
                        }

                        if (model.Id != 0)
                        {
                           if(context.deleteotherfee(model.Id, out errmsg) == false)
                            {
                                dbContextTransaction.Rollback();
                                return result = result.FailResult("保存失败" + errmsg);
                            }
                        }
                        if (model.DepositType == 99&& model.Yajin!=null)
                        {
                            T_Otherfee fee = model.Yajin.Where(p => p.Name == "常规押金").FirstOrDefault();
                            if (fee != null)
                            {
                                model.Deposit = decimal.Parse(fee.Amount.ToStr());
                            }
                        }
                        long ContractId = context.SaveContrct(model);
                        if (model.Otherfee != null)
                        {
                            other = context.SaveOtherFee(model.Otherfee, ContractId);  
                        }
                        if (model.Yajin != null)
                        {
                            model.Yajin.ForEach(p => p.IsYajin = 1);
                            other = context.SaveOtherFee(model.Yajin, ContractId);
                        }
                        bool re = context.CmdBill(ContractId, "system",type, model.issendmessage, out errmsg);
                        //添加日志
                        RzService rzservice = new Service.RzService();
                        rzservice.contractaddrz(model.HouseId, sysuser.Id, model.CompanyId, type);
                        if (ContractId>0&& other>0&& tract > 0&&re==true)
                        {
                           
                            dbContextTransaction.Commit();
                            Thread t2 = new Thread(new ThreadStart(delegate () { ContractAfter(model, user, sysuser); }));
                            t2.IsBackground = true;
                            t2.Start();
                            return result=result.SuccessResult("保存成功");
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            return result = result.FailResult("保存失败"+errmsg);
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        return result = result.FailResult(ex.ToString());
                    }
                }
            }
          
        }

    
        //退租
        public SysResult ContractAfter1(long  contractid, ElecUser user, T_SysUser sysuser)
        {
            SysResult result = new SysResult();
            string errmsg = "";
            //入住电表
            HouseDAL hdal = new HouseDAL();
            ContrctDAL cdal = new ContrctDAL();
            T_Contrct model = cdal.querycontract(new T_Contrct() { Id = contractid });
            HouseQuery house = hdal.Query2(new HouseModel() { Id = model.HouseId });
            if (house.uuid != null)
            {
                //查询退房清单
                ElectricService elecsercice = new ElectricService();
                other para1 = new other();
                para1.devid = house.uuid;
                SysResult<DeviceData> syslistt= elecsercice.recederoom(para1, user, sysuser);
                DeviceData listd = syslistt.numberData;
                errmsg = syslistt.Message;
                if (syslistt.Code == 0)
                {
                    decimal amount = listd.Money;
                    //计入财务流水
                    FinanceService findserice = new Service.FinanceService();
                    findserice.save(new FinanceModel() { HouseId = model.HouseId, Trader = "电表"+ house.electricid, Type = 1, CostName = "电表退租", Amount = amount,TradingDate=DateTime.Now,Transaoctor= sysuser.RealName,CompanyId= model.CompanyId});
                    //电表退租
                    ruzhu para = new ruzhu();
                    para.devid = house.uuid;
                    result = elecsercice.ammeterrecederoom(para, user, sysuser);
                }
                else
                {
                    result.Code = 1;
                    result.Message = "电表退租失败:" + errmsg + "请手动完成退租或者联系管理员";
                }
            }
            return result;
        }
        //新增合同后进行自动授权和注册账号
        public void ContractAfter(object data, ElecUser user, T_SysUser sysuser)
        {
            KjxService kjx = new KjxService();
            T_Contrct model = (T_Contrct)data;
            //发送绑定短信
            if (model.issendmessage == 1)
            {
                messageService service = new messageService();
                service.sendmessage(2, model.Id);
            }
            T_Teant teant = dal.queryteant(model.Teant);
            string password = "";
            HouseDAL hdal = new HouseDAL();
            HouseQuery house = hdal.Query2(new HouseModel() { Id = model.HouseId });
            if (house.LocalId != null)
            {
                if (teant != null && teant.Pt_UserName == null)
                {
                    if (!string.IsNullOrEmpty(teant.Pt_PassWord))
                    {
                        password = teant.Pt_PassWord;
                    }
                    else
                    {
                        password = ConvertHelper.getsecond();
                    }
                    SysResult<T_kjx> remodel = kjx.register(new T_kjx() { username = teant.Phone, password = password });
                    if (remodel.Code == 0)
                    {
                        teant.Pt_PassWord = password;
                        teant.Pt_UserName = remodel.numberData.username;
                        dal.updatetrant(teant);
                    }
                }
            }
            //入住电表
            if (house.uuid != null)
            {
                ElectricService elecsercice = new ElectricService();
                ruzhu para = new ruzhu();
                para.name = teant.Name;
                para.phone = teant.Phone;
                para.checkingdate = model.BeginTime;
                para.devid = house.uuid;
                elecsercice.ammeterstayroom(para, user, sysuser);
            }
            //授权
            //kjx.send(new keylist() { username = teant.Phone, startDate = model.BeginTime.Millisecond, endDate = model.EndTime.Millisecond }, new Model.User.T_SysUser() { });
        }
        //添加抄表账单
        public SysResult addbill(chaobiao model)
        {
            SysResult result = new SysResult();
            if (model.price==0)
            {
                return result = result.FailResult("单价不能为0");
            }
            if (model.dushu <= model.lastdushu)
            {
                return result = result.FailResult("必须大于上次读数");
            }
            if (model.time <= model.lasttime)
            {
                return result = result.FailResult("必须大于上次读表时间");
            }
            using (var context = new ContrctDAL())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string errmsg = "";
                        T_Otherfee therfee = new Model.Contrct.T_Otherfee();
                        therfee.Id = model.Id;
                        therfee.Reading = model.dushu;
                        therfee.ChaobiaoTime = model.time;
                        therfee.Price = model.price;
                        int update= context.updateOtherFee(therfee, new string[] { "Price", "Reading", "ChaobiaoTime" });
                        bool re = context.CmdotherfeeBill(model, "system", out errmsg);
                        if (update > 0&& re == true)
                        {
                            dbContextTransaction.Commit();
                            return result = result.SuccessResult("保存成功");
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            return result = result.FailResult("保存失败" + errmsg);
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        return result = result.FailResult(ex.ToString());
                    }
                }
            }

        }
        public SysResult DeleteContract(T_Contrct model)
        {
            SysResult result = new SysResult();
            ContrctDAL context = new ContrctDAL();
            try
            {
                string errmsg = "";
                bool re = context.CmdDelete(model.Id, model.userid.ToStr(), "sp_deletecontract", out errmsg);
                if (re == true)
                {
                    result = result.SuccessResult("删除成功");
                }
                else
                {
                    result = result.FailResult("删除失败"+errmsg);
                }
                return result;
            }
            catch (Exception ex)
            {
                return result = result.FailResult(ex.ToString());
            }
        }
        public SysResult checkelec(long contractid,long companyid)
        {
            SysResult result = new SysResult();
            ProceDAL pdal = new ProceDAL();
            result=pdal.Cmdproce10(new Pure() { Spname = "sp_checkiselec", Ids = contractid.ToStr(), Other = companyid.ToStr() });
            return result;
        }
        public SysResult<WrapContract> qianyue(T_Contrct model)
        {
            SysResult<WrapContract> result = new SysResult<WrapContract>();
            ContrctDAL dal = new ContrctDAL();
            WrapContract cont =dal.Querycontract(model.Id);
            result.numberData = cont;
            bool iselec = false;
            if (cont == null)
            {
                result.Message = "合同不存在";
                result.Code = 1;
                return result;
            }
            if (cont.Status != 1&& cont.Status != 5 && cont.Status != 2)
            {
                result.Message = "合同状态错误";
                result.Code = 1;
                return result;
            }
            if (cont.onlinesign == 1 && cont.Document ==null)
            {
                result.Message = "电子签约身份证号不能为空";
                result.Code = 1;
                return result;
            }
            if (string.IsNullOrEmpty(cont.Phone))
            {
                result.Message = "租客手机号不能为空，请联系管理员";
                result.Code = 1;
                return result;
            }
            //查询电子合同数量
            if (cont.onlinesign == 1)
            {
                BaseDataDALL bdal = new DAL.BaseDataDALL();
                T_account account = bdal.queryaccount(cont.CompanyId);
                if (account != null)
                {
                    if (account.contractnumber > 0)
                    {
                        SignService signservice = new SignService();
                        //企业证书创建
                        if (account.name != null && account.certificate != null && account.address != null && account.contact != null)
                        {
                            if (account.isreg == 0)
                            {
                                SysResult regresult = signservice.entpReg(new SignVersion() { name = account.name, certificate = account.certificate, address = account.address, user_code = ConvertHelper.GetMd5HashStr(account.CompanyId.ToStr()), contact = account.contact, mobile = account.phone });
                                if (regresult.Code == 0)
                                {
                                    account.isreg = 1;
                                    bdal.saveaccount1(account);
                                }
                            }     
                        }
                        iselec = true;
                        
                    }
                    //电子签约
                    if (cont.onlinesign == 1 && iselec == true)
                    {
                        return result = signonline(cont, account);
                    }
                }
            }
            dal.SaveContrct1(new T_Contrct() { Id=cont.Id,Status=5});
            //授权电子钥匙
            ElectricDAL hdal = new ElectricDAL();
            KjxService kjxser = new KjxService();
            List<HouseLockQuery> houselock = hdal.lockquery3(cont.HouseId);
            keylist key = new keylist();
            key.username = cont.Phone;
            key.startDate =ConvertHelper.getsecond2(cont.BeginTime);
            key.endDate = ConvertHelper.getsecond2(cont.EndTime);
            foreach (var mo in houselock)
            {
                if (!string.IsNullOrEmpty(mo.LocalId))
                {
                    key.lockId = mo.LocalId;
                    kjxser.sendzk(key, new syspara() { UserName = cont.Phone,CompanyId= mo.CompanyId });
                }
            }
           
            result.Message = "签约成功";
            result.Code = 0;
            return result;
        }
        
        public SysResult<WrapContract> signonline(WrapContract model, T_account account )
        {
            SysResult<WrapContract> result = new SysResult<WrapContract>();
            SignService signservice = new SignService();
            //个人证书创建
            if (model.issign == 0&& model.Document!=null)
            {
                signservice.personReg(new SignVersion() { name = model.Name, id_card_no = model.Document, moblie = model.Phone, user_code =ConvertHelper.GetMd5HashStr( model.TeantId.ToStr())});
            }
            //生成合同world
            string filename = "contract_" + model.Id+".docx";
            CreateWord word = new DAL.Common.CreateWord();
            BaseDataService tempservice = new BaseDataService();
            T_template temp = tempservice.morenQuery(model).numberData;
            temp.content = addusercode(temp.content, ConvertHelper.GetMd5HashStr(model.TeantId.ToStr()), ConvertHelper.GetMd5HashStr(account.CompanyId.ToStr()), account.name, model.Name);
            word.SaveAsWord(temp.content, filename);
            //提交合同文本
            signservice.createForWord(new SignVersion() { no = model.Id.ToStr(), filename = filename });
            //企业自动签署
            result = signservice.signAuto(new SignVersion() { no = model.Id.ToStr(), user_code = ConvertHelper.GetMd5HashStr(account.CompanyId.ToStr()), sign_type = "自动签署" });
            //签署合同点击签署
            result = signservice.signAuto(new SignVersion() { no = model.Id.ToStr(), user_code = ConvertHelper.GetMd5HashStr(model.TeantId.ToStr()),sign_type= "SIGNATURE" });
            return result;
        }
        public string addusercode(string content,string usercode,string encode,string jianame,string yiname)
        {
            string insertuser = "<span style='margin-left:15px;color:snow;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + usercode + " </span>";
            string inserten = "<span style='margin-left:15px;color:snow;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + encode + "</span>";
            int startindex = content.IndexOf(yiname)+ yiname.Length;
            content=content.Insert(startindex, insertuser);
            startindex = content.IndexOf(jianame) + jianame.Length;
            content=content.Insert(startindex, inserten);
            return content;
        }
        //钥匙授权期限
        public SysResult getcontractbylock(string lockid,out long start,out long end)
        {
            SysResult result = new SysResult();
            start = 0;
            end = 0;
            int[] arr = new int[] { 2,5};
            WrapContract contract = dal.QueryId(new WrapContract() { LockId = lockid,arrStatus= arr });
            if (contract == null)
            {
                return result=result.FailResult("没有获取到在租合同");
            }
            if (contract.EndTime < DateTime.Now.AddDays(1))
            {
                return result = result.FailResult("合同已到期无法获取密码");
            }
            if (contract.BeginTime < DateTime.Now)
            {
                contract.BeginTime = DateTime.Now;
            }
            start = ConvertHelper.getsecond2(contract.BeginTime);
            end= ConvertHelper.getsecond2(contract.EndTime);
            return result;
        }
        public SysResult<WrapEnum> Query(long contractid)
        {
            SysResult<WrapEnum> result = new SysResult<WrapEnum>();
            ZafeiDAL dal = new ZafeiDAL();
            WrapEnum warp = new WrapEnum();
            //签约方式和提示
            BaseDataDALL bdal = new BaseDataDALL();
            T_account account= bdal.queryaccount(contractid);
            warp.onlinesign = account.onlinesign;
            if (account.IdenTity == 0)
            {
                warp.onlinesignstr = "未经过平台认证,签约后无法律效应。";
            }
            if (account.contractnumber == 0)
            {
                warp.onlinesignstr += "电子合同不足;";
            }
            if (account.smsnumber == 0)
            {
                warp.onlinesignstr += "短信不足";
            }
            List<CEnum> paytype = new List<CEnum>();
            paytype.Add(new CEnum() { key = 1, Value = "支付宝" });
            paytype.Add(new CEnum() { key = 2, Value = "微信" });
            paytype.Add(new CEnum() { key = 3, Value = "网银" });
            paytype.Add(new CEnum() { key = 4, Value = "现金" });
            warp.paytype = paytype;
            List<CEnum> type = new List<CEnum>();
            type.Add(new CEnum() { key = 1, Value = "日租" });
            type.Add(new CEnum() { key = 2, Value = "月租" });
            warp.type = type;
            //普通频率
            List<CEnum> pinlv = new List<CEnum>();
            pinlv.Add(new CEnum() { key = 1, Value = "一月一付" });
            pinlv.Add(new CEnum() { key = 2, Value = "二月一付" });
            pinlv.Add(new CEnum() { key = 3, Value = "三月一付" });
            pinlv.Add(new CEnum() { key = 6, Value = "六月一付" });
            pinlv.Add(new CEnum() { key = 12, Value = "一年一付" });
            pinlv.Add(new CEnum() { key = 24, Value = "两年一付" });
            warp.pinlv = pinlv;
            //杂费频率
            List<CEnum> zafeipinlv = new List<CEnum>();
            zafeipinlv.Add(new CEnum() { key = 0, Value = "随租金支付" });
            zafeipinlv.Add(new CEnum() { key = 1, Value = "一月一付" });
            zafeipinlv.Add(new CEnum() { key = 2, Value = "二月一付" });
            zafeipinlv.Add(new CEnum() { key = 3, Value = "三月一付" });
            zafeipinlv.Add(new CEnum() { key = 6, Value = "六月一付" });
            zafeipinlv.Add(new CEnum() { key = 12, Value = "一年一付" });
            zafeipinlv.Add(new CEnum() { key = 24, Value = "两年一付" });
            zafeipinlv.Add(new CEnum() { key = 999, Value = "一次付清" });
            warp.zafeipinlv = zafeipinlv;
            //押几
            List<CEnum> yaji = new List<CEnum>();
            yaji.Add(new CEnum() { key = 999, Value = "自定义押金" });
            yaji.Add(new CEnum() { key = 0, Value = "无押金" });
            yaji.Add(new CEnum() { key = 1, Value = "押一" });
            yaji.Add(new CEnum() { key = 2, Value ="押二" });
            yaji.Add(new CEnum() { key = 3, Value ="押三" });
            yaji.Add(new CEnum() { key = 4, Value ="押四" });
            yaji.Add(new CEnum() { key = 5,Value ="押五" });
            yaji.Add(new CEnum() { key = 6,Value ="押六" });
            yaji.Add(new CEnum() { key = 7,Value ="押七" });
            yaji.Add(new CEnum() { key = 8, Value = "押八" });
            yaji.Add(new CEnum() { key = 9, Value = "押九" });
            yaji.Add(new CEnum() { key = 10, Value = "押十" });
            yaji.Add(new CEnum() { key = 11, Value = "押十一" });
            yaji.Add(new CEnum() { key = 12, Value = "押十二" });
            warp.yaji = yaji;
            List<CEnum> work = new List<CEnum>();
            work.Add(new CEnum() { key = 1, Value = "程序员" });
            work.Add(new CEnum() { key = 2, Value = "业务员" });
            work.Add(new CEnum() { key = 3, Value = "金融规划师" });
            warp.work = work;
            List<CEnum> Hobby = new List<CEnum>();
            Hobby.Add(new CEnum() { key = 1, Value = "足球" });
            Hobby.Add(new CEnum() { key = 2, Value = "篮球" });
            Hobby.Add(new CEnum() { key = 3, Value = "羽毛球" });
            warp.Hobby = Hobby;
            List<T_ZafeiList> zafeilist = new List<T_ZafeiList>();
            warp.zafei = dal.Query(new T_ZafeiList() {IsActive=1,TuiType=0 });
            result.numberData = warp; 
            return result;
        }
        public SysResult<List<WrapContract>> Querymenufy(WrapContract model, OrderablePagination orderablePagination,T_SysUser user,long[] userids)
        {
            
            SysResult<List<WrapContract>> sysresult = new SysResult<List<WrapContract>>();
            List<WrapContract> list = dal.Query(model, orderablePagination, user,userids);
            foreach(var  mo in list)
            {
                mo.Status = getstatus(mo.Status, mo.BeginTime, mo.EndTime);
                if (mo.Status ==6)
                {
                    TimeSpan t3 = DateTime.Today - mo.EndTime;
                    mo.Day = t3.Days;
                }
            }
            sysresult.numberData = list;
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
        public int getstatus(int Status,DateTime BeginTime,DateTime EndTime)
        {
            int resultStatus = Status;
            DateTime dt = DateTime.Now.AddDays(45).Date;
            if (Status == 2 || Status == 5)
            {
                if (BeginTime > DateTime.Now.Date)
                {
                    resultStatus = 4;
                }
                if (BeginTime <= DateTime.Now && EndTime >= DateTime.Now)
                {
                    resultStatus = 5;
                }
                if (EndTime <= dt)
                {
                    resultStatus = 6;
                }
            }
            if (Status == 3)
            {
                resultStatus = 7;
            }
            return resultStatus;
        }
        //合同导出
        public SysResult<List<WrapContract>> excelQuerymenufy(WrapContract model, T_SysUser user)
        {
            DateTime dt = DateTime.Now.AddDays(-5);
            SysResult<List<WrapContract>> sysresult = new SysResult<List<WrapContract>>();
            List<WrapContract> list = dal.excelQuery(model, user);
            foreach (var mo in list)
            {
                mo.Status = getstatus(mo.Status, mo.BeginTime, mo.EndTime);
                if (mo.Status == 6)
                {
                    TimeSpan t3 = DateTime.Today - mo.EndTime;
                    mo.Day = t3.Days;
                }
            }
            sysresult.numberData = list;
         
            return sysresult;
        }
        public SysResult<List<WrapContract>> Querytuizu(WrapContract model, OrderablePagination orderablePagination)
        {
            SysResult<List<WrapContract>> sysresult = new SysResult<List<WrapContract>>();
            model.Status = 6;
            List<WrapContract> list = dal.Querytuizu(model, orderablePagination);
           
            foreach (var mo in list)
            {
                if (mo.Status == 2)
                {
                    if (mo.BeginTime > DateTime.Now)
                    {
                        mo.Status = 4;
                    }
                    if (mo.BeginTime <= DateTime.Now && mo.EndTime >= DateTime.Now)
                    {
                        mo.Status = 5;
                    }
                    if (mo.EndTime < DateTime.Now)
                    {
                        mo.Status = 6;
                    }
                }
                if (mo.Status == 3)
                {
                    mo.Status = 7;
                }
            }
            sysresult.numberData = list;
            sysresult.numberCount = list.Count;
            return sysresult;
        }
        public SysResult<List<chaobiao>> chaobiaoquery(chaobiao model, OrderablePagination orderablePagination)
        {
            SysResult<List<chaobiao>> sysresult = new SysResult<List<chaobiao>>();
            model.type = 3;
            model.isfentan = 0;
            List<chaobiao> list = dal.chaobiaoquery(model, orderablePagination);
            sysresult.numberData = list;
            sysresult.numberCount = list.Count;
            return sysresult;
        }

        public SysResult<chaobiao> chaobiaoqueryxq(chaobiao model)
        {
            SysResult<chaobiao> sysresult = new SysResult<chaobiao>();
            chaobiao list = dal.chaobiaoqueryxq(model);
            T_Teant teant = dal.queryteant(new T_Teant() { Id = list.teantid });
            if (teant != null)
            {
                list.treatname = teant.Name;
            }
            sysresult.numberData = list;
            return sysresult;
        }
        public SysResult<WrapContract> QueryById(WrapContract model)
        {
            SysResult<WrapContract> sysresult = new SysResult<WrapContract>();
            WrapContract list = dal.QueryId(model);
            list.Status = getstatus(list.Status, list.BeginTime, list.EndTime);
            if (list.Status == 6)
            {
                TimeSpan t3 = DateTime.Today - list.EndTime;
                list.Day = t3.Days;
            }
            sysresult.numberData = list;
            return sysresult;
        }

        public Inittuizu create(T_BillList billlist, T_WrapBill bill,DateTime tuizutime)
        {
            //生成待收款项
            Inittuizu tuizu = new Inittuizu();
            tuizu.Type = 1;
            tuizu.Name = billlist.BillType;
            tuizu.BeginTime = bill.BeginTime;
            if (billlist.BillType.Contains("押金"))
            {
                tuizu.Amount = billlist.Amount;
            }
            else
            {
                int day = 0;
                day = (tuizutime - bill.BeginTime).Days;
                int allday = (bill.EndTime - bill.BeginTime).Days;
                decimal pingjun = decimal.Round(billlist.Amount / allday, 2);
                tuizu.Amount = decimal.Round(day * pingjun, 2);
                tuizu.Explain = pingjun + "[单价]" + "*" + day + "[天数]";
            }
            tuizu.EndTime = tuizutime;
            return tuizu;

        }
        public Inittuizu create1(T_BillList billlist, T_WrapBill bill, DateTime tuizutime)
        {
            Inittuizu tuizu = new Inittuizu();
            tuizu.Type = 0;
            tuizu.Name = billlist.BillType;
            tuizu.BeginTime = bill.BeginTime;
            tuizu.EndTime = tuizutime;
            if (billlist.BillType.Contains("押金"))
            {
                tuizu.Amount = billlist.Amount;
            }
            else
            {
                int alldays = (bill.EndTime - bill.BeginTime).Days;
                int days = (bill.EndTime- tuizutime).Days;
                decimal pingjun = decimal.Round(billlist.Amount / alldays, 2);
                tuizu.Amount = decimal.Round(days * pingjun, 2);
                tuizu.Explain = pingjun + "[单价]" + "*" + days + "[天数]";
            }
            return tuizu;
        }
        public SysResult<List<Inittuizu>> Querytuikuan(tuzuReq model)
        {
            SysResult<List<Inittuizu>> result = new SysResult<List<Inittuizu>>();
            T_Contrct contract = dal.querycontract(new T_Contrct() {Id= model.contractid });
            if (model.tuizutime ==DateTime.MinValue)
            {
                model.tuizutime = DateTime.Now.AddDays(1);
            }
            //获取自定义的退款项
            List<Inittuizu> list = new List<Inittuizu>();
            if (contract.Status == 7)
            {
                TuizuDAL tuzudal = new TuizuDAL();
                List<Tuizu> tuizulist=  tuzudal.Querylist(new TuizuZhu() {ContractId=model.contractid });
                foreach(var mo in tuizulist)
                {
                    Inittuizu tuizu = new Inittuizu();
                    tuizu.Type = mo.Type;
                    tuizu.Name = mo.Name;
                    tuizu.BeginTime = mo.BeginTime;
                    tuizu.Amount = mo.Amount;
                    tuizu.EndTime = mo.EndTime;
                    list.Add(tuizu);
                }
                result.numberCount = list.Count();
                result.numberData = list;
                return result;
            }
            BillDAL bill = new BillDAL();
            //获取所有的账单数据
            List<T_WrapBill> listbill = bill.tuizu_shou(model.contractid,0,0);
            bool isover = false;
            //遍历未支付账单
            foreach (var m in listbill.Where(p=>p.PayStatus==0).OrderBy(p=>p.EndTime))
            {
                foreach (var n in m.list)
                {
                    //如果退租时间在本周期内则只收周期开始时间到退租时间  过去到现在  第一个是最晚的账单
                    if (m.EndTime > model.tuizutime && model.tuizutime > m.BeginTime)
                    {
                        Inittuizu tuizu = create(n, m, model.tuizutime);
                        if (tuizu.Amount!=0)
                        {
                            list.Add(tuizu);
                        }
                    }
                    //如果退租时间超过本周期则收款整个周期
                    if (m.EndTime <= model.tuizutime)
                    {
                        //生成待收款项
                        Inittuizu tuizu = new Inittuizu();
                        tuizu.Type = 1;
                        tuizu.Name = n.BillType;
                        tuizu.BeginTime = m.BeginTime;
                        tuizu.Amount = m.Amount;
                        tuizu.EndTime = m.EndTime;
                        tuizu.Explain = "完整周期,直接取该账单总金额";
                        list.Add(tuizu);
                    }
                    if (model.tuizutime <= m.BeginTime)
                    {
                        isover = true;
                    }
                }
                if (isover)
                {
                    break;
                }
            }
            //遍历已支付账单获取需要退款的项目  未来=》现在 排序 第一个是最未来的账单
            foreach (var m in listbill.Where(p => p.PayStatus ==1).OrderByDescending(p => p.EndTime)) 
            {
                foreach (var n in m.list)
                {
                    if (m.BeginTime <= model.tuizutime && model.tuizutime <= m.EndTime)
                    {
                        isover = true;
                        Inittuizu tuizu = create1(n, m, model.tuizutime);
                        if (tuizu.Amount != 0)
                        {
                            list.Add(tuizu);
                        }
                    }
                    if(m.BeginTime >= model.tuizutime && model.tuizutime < m.EndTime)
                    {
                        Inittuizu tuizu = new Inittuizu();
                        tuizu.Amount = n.Amount;
                        tuizu.Type = 0;
                        tuizu.Name = n.BillType;
                        tuizu.BeginTime = m.BeginTime;
                        tuizu.EndTime = m.EndTime;
                        tuizu.Explain = "完整周期,直接取该账单总金额";
                        list.Add(tuizu);
                    }
                    if(m.EndTime<= model.tuizutime)
                    {
                        break;
                    } 
                }
            }
            result.numberCount = list.Count();
            result.numberData = list;
            return result;
        }
        public SysResult tuikuan(TuizuZhu model,ElecUser user, T_SysUser sysuser )
        {
            SysResult result = new SysResult();
            using (var context = new TuizuDAL())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string errmsg = "";
                        long zhuid = 1;

                        if (model != null)
                        {
                            //删除之前数据
                            TuizuZhu tuizu=context.QueryTuizu1(model);
                            if (tuizu != null)
                            {
                                context.deletetuizuzhu(tuizu);
                            }
                            zhuid = context.Savetuizuzhu(model);
                        }
                        if (model.list != null)
                        {
                            context.Savetuizu(model.list, zhuid);
                        }
                        bool re = context.Cmdtuizu(zhuid, sysuser.Id.ToStr(), "sp_tuizu", out errmsg);
                        if (zhuid > 0&&re == true)
                        {
                            dbContextTransaction.Commit();
                            sysuser.RealName = model.BanLi;
                            ContractAfter1(model.ContractId, user, sysuser);
                            return result = result.SuccessResult("审核成功");
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            return result = result.FailResult("审核失败" + errmsg);
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        return result = result.FailResult(ex.ToString());
                    }
                }
            }
        }
        public SysResult zktuikuan(TuizuZhu model)
        {
            SysResult result = new SysResult();
            using (var context = new TuizuDAL())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string errmsg = "";
                        long zhuid = 1;
                        model.Type = 1;
                        if (model != null)
                        {
                            TuizuZhu remodel = context.QueryTuizu(model);
                            if (remodel != null)
                            {
                                context.deletemdel(remodel);
                            }
                            zhuid = context.Savetuizuzhu(model);
                        }
                        if (model.list != null)
                        {
                            context.Savetuizu(model.list, zhuid);
                        }
                        bool re = context.Cmdtuizu(zhuid, "system", "sp_zktuizu", out errmsg);
                        if (zhuid > 0 && re == true)
                        {
                            dbContextTransaction.Commit();
                            return result = result.SuccessResult("审核成功");
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            return result = result.FailResult("审核失败" + errmsg);
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        return result = result.FailResult(ex.ToString());
                    }
                }
            }
         
        }
        public SysResult istuikuan(TuizuZhu model)
        {
            SysResult result = new SysResult();
            BillDAL billdal = new BillDAL();
            //查询未付账单数量
            int contract = dal.Querycount1(new T_Contrct() { Id = model.ContractId,Status=7 });
            if (contract > 0)
            {
                result = result.FailResult("已退租不能重复退租");
                return result;
            }
            int count = billdal.weishou(new T_Bill() { ContractId = model.ContractId });
            if (count == 0)
            {
                result = result.SuccessResult("");
            }
            else
            {
                string message = "当前合同有" + count + "未付账单,确认退租吗";
                result = result.FailResult(message);
            }
            return result;
        }
        public SysResult xuzu(T_Contrct model)
        {
            SysResult result = new SysResult();

            //判断是新增还是修改
            int type = 0;
            if (model.Id != 0)
            {
                type = 1;
            }
            else
            {
                model.Status = 5;
            }
            using (var context = new ContrctDAL())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string errmsg = "";
                        int other = 1;
                        long tract = 1;
                        if (model.Teant != null)
                        {
                            T_Teant teant = context.queryteant(model.Teant);
                            if (teant != null)
                            {
                                teant.Document = model.Teant.Document;
                                teant.DocumentType = model.Teant.DocumentType;
                                teant.Name = model.Teant.Name;
                                teant.Weinxin = model.Teant.Weinxin;
                                teant.Work = model.Teant.Work;
                                teant.Hobby = model.Teant.Hobby;
                                teant.Name = model.Teant.Name;
                                teant.Sex = model.Teant.Sex;
                                teant.Zidcard = model.Teant.Zidcard;
                                if (string.IsNullOrEmpty(teant.Pt_PassWord))
                                {
                                    teant.Pt_PassWord = ConvertHelper.getsecond();
                                }
                                tract = context.updatetrant(teant);
                            }
                            else
                            {
                                string password = ConvertHelper.getsecond();
                                model.Teant.Pt_PassWord = password;
                                tract = context.SaveTrent(model.Teant);
                            }
                            model.TeantId = tract;
                        }
                        //
                        if (model.Id != 0)
                        {
                            if (context.deleteotherfee(model.Id, out errmsg) == false)
                            {
                                dbContextTransaction.Rollback();
                                return result = result.FailResult("保存失败" + errmsg);
                            }
                        }
                        long ContractId = context.SaveContrct(model);
                        if (model.Otherfee != null)
                        {
                            other = context.SaveOtherFee(model.Otherfee, ContractId);
                        }
                        if (model.Yajin != null)
                        {
                            model.Yajin.ForEach(p => p.IsYajin = 1);
                            other = context.SaveOtherFee(model.Yajin, ContractId);
                        }
                        bool re = context.Cmdxuzu(model.Id, "system", "sp_xuzu", out errmsg);
                        if (ContractId > 0 && other > 0 && tract > 0 && re == true)
                        {
                            dbContextTransaction.Commit();
                            return result = result.SuccessResult("保存成功");
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            return result = result.FailResult("保存失败" + errmsg);
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        return result = result.FailResult(ex.ToString());
                    }
                }
            }
        }
        //修改手机号
        
        public SysResult UpdatePhone(T_Teant model)
        {
            SysResult result = new SysResult();
            T_Teant teant = dal.queryteant(model);
            if (teant != null)
            {
                teant.Phone = model.NewPhone;
                if (teant != null)
                {
                    dal.updatetrant(teant);
                }
            }
            return result;
        }

        public SysResult<List<WrapContract>> query(WrapContract model, OrderablePagination orderablePagination)
        {
            SysResult<List<WrapContract>> sysresult = new SysResult<List<WrapContract>>();
            //model.EndTime = DateTime.Now.AddDays(30);

            List<WrapContract> list = dal.Query(model, orderablePagination,null,null);
            sysresult.numberData = list;
            sysresult.numberCount = list.Count;
            return sysresult;
        }
        public SysResult<List<WrapContract>> nopagequery(WrapContract model)
        {
            SysResult<List<WrapContract>> sysresult = new SysResult<List<WrapContract>>();
            //model.EndTime = DateTime.Now.AddDays(30);
            List<WrapContract> list = dal.notpageQuery(model);
            if (model.iskaimen == 1)
            {
                HoseService house = new HoseService();
                foreach(var mo in list)
                {
                    if (mo.Status != 7)
                    {
                        mo.HouseLock = house.Querylockbyhouse(new HouseModel() { Id = mo.HouseId }).numberData;
                    } 
                }
            }
            sysresult.numberData = list;
            sysresult.numberCount = list.Count;
            return sysresult;
        }

        public SysResult<T_Teant> queryteant(T_Teant model)
        {
            SysResult<T_Teant> sysresult = new SysResult<T_Teant>();
            T_Teant list = dal.QueryTeant(model);
            sysresult.numberData = list;
            return sysresult;
        }

        
        public SysResult<TuizuZhu> tuizuQuery(TuizuZhu model)
        {
            SysResult<TuizuZhu> sysresult = new SysResult<TuizuZhu>();
            TuizuDAL dal = new DAL.TuizuDAL();
            TuizuZhu data = dal.QueryTuizu(model);
            List<Tuizu> list = new List<Tuizu>();
            if (data != null)
            {
                list = dal.Querylist(data);
                data.list = list;
            }
            sysresult.numberData = data;
            return sysresult;
        }
    }
}
