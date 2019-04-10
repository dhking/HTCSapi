using ControllerHelper;
using DAL;
using Model;
using Model.Bill;
using Model.Contrct;
using Model.TENANT;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public   class OwerContractService
    {
        OwerContractDAL dal = new OwerContractDAL();
        public SysResult SaveContract(T_OwernContrct model,long userid)
        {
            SysResult result = new SysResult();
            int type = 0;
            if (model.Id != 0)
            {
                type = 1;
            }
            //检查房间是否绑定智能电表

            using (var context = new OwerContractDAL())
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
                                teant.mobject = 1;
                                teant.Document = model.Teant.Document;
                                teant.Name = model.Teant.Name;
                                teant.Phone = model.Teant.Phone;
                                teant.DocumentType = model.Teant.DocumentType;
                                teant.Zidcard = model.Teant.Zidcard;
                                teant.Phone = model.Teant.Phone;
                                teant.DocumentType = model.Teant.DocumentType;
                                tract = context.updatetrant(teant);
                            }
                            else
                            {
                                model.Teant.mobject = 1;
                                tract = context.SaveTrent(model.Teant);
                            }
                            model.TeantId = tract;
                        }
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
                        bool re = context.CmdBill(ContractId, "system", out errmsg);
                        //添加日志
                        RzService rzservice = new Service.RzService();
                        rzservice.ocontractaddrz(model.HouseId, userid, model.CompanyId, type);
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
        public SysResult<List<WrapOwernContract>> Querymenufy(WrapOwernContract model, OrderablePagination orderablePagination,T_SysUser user)
        {
            SysResult<List<WrapOwernContract>> sysresult = new SysResult<List<WrapOwernContract>>();
            List<WrapOwernContract> list = dal.Query(model, orderablePagination,user);
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
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }

        public SysResult<List<WrapOwernContract>> excelQuerymenufy(WrapOwernContract model,  T_SysUser user)
        {
            SysResult<List<WrapOwernContract>> sysresult = new SysResult<List<WrapOwernContract>>();
            List<WrapOwernContract> list = dal.excelQuery(model, user);
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
      
            return sysresult;
        }
        public SysResult<WrapOwernContract> QueryById(WrapOwernContract model)
        {
            SysResult<WrapOwernContract> sysresult = new SysResult<WrapOwernContract>();
            WrapOwernContract list = dal.QueryId(model);
            if (list.Status == 2)
            {
                if (list.BeginTime > DateTime.Now)
                {
                    list.Status = 4;
                }
                if (list.BeginTime <= DateTime.Now && list.EndTime >= DateTime.Now)
                {
                    list.Status = 5;
                }
                if (list.EndTime < DateTime.Now)
                {
                    list.Status = 6;
                }
            }
            if (list.Status == 3)
            {
                list.Status = 7;
            }
            sysresult.numberData = list;
            return sysresult;
        }
        public SysResult<List<Inittuizu>> Querytuikuan(tuzuReq model)
        {
            SysResult<List<Inittuizu>> result = new SysResult<List<Inittuizu>>();
            if (model.tuizutime == DateTime.MinValue)
            {
                model.tuizutime = DateTime.Now.AddDays(1);
            }
            TuizuDAL dal = new TuizuDAL();
            BillDAL bill = new BillDAL();
            //获取自定义的退款项如违约金项目
            List<Inittuizu> list = new List<Inittuizu>();
            //list = dal.Queryinit();
            //foreach(var t in list)
            //{
            //    //比如是违约的才会添加违约金
            //    string[] arr =t.TkType.Split(new char[';']);
            //    if (!arr.Contains(model.tktype.ToStr()))
            //    {
            //        list.Remove(t);
            //    }
            //}
            //获取所有的账单数据
            List<T_WrapBill> listbill = bill.tuizu_shou(model.contractid,1,1);
            bool isover = false;
            //遍历未支付账单
            foreach (var m in listbill.Where(p => p.PayStatus == 0).OrderBy(p => p.EndTime))
            {
                foreach (var n in m.list)
                {
                    //如果退租时间在本周期内则只收周期开始时间到退租时间  过去到现在  第一个是最晚的账单
                    if (m.EndTime > model.tuizutime && model.tuizutime > m.BeginTime)
                    {
                        //生成待收款项
                        Inittuizu tuizu = new Inittuizu();
                        tuizu.Type = 1;
                        tuizu.Name = n.BillType;
                        tuizu.BeginTime = m.BeginTime;
                        int day = 0;
                        day = (model.tuizutime - m.BeginTime).Days;
                        tuizu.Amount = decimal.Round(day * (m.Amount / (m.EndTime - m.BeginTime).Days), 2);
                        tuizu.EndTime = model.tuizutime;
                        tuizu.Explain = "周期总价" + m.Amount + "*" + day + "/" + (m.EndTime - m.BeginTime).Days;
                        if (tuizu.Amount != 0)
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
                        tuizu.Explain = "退租结算项";
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
            foreach (var m in listbill.Where(p => p.PayStatus == 1).OrderByDescending(p => p.EndTime))
            {
                foreach (var n in m.list)
                {
                    if (m.BeginTime < model.tuizutime && model.tuizutime <= m.EndTime)
                    {
                        Inittuizu tuizu = new Inittuizu();
                        int days = (m.EndTime - DateTime.Now).Days;
                        tuizu.Amount = days * (n.Amount / (m.EndTime - m.BeginTime).Days);
                        tuizu.Type = 0;
                        tuizu.Name = n.BillType;
                        tuizu.BeginTime = m.BeginTime;
                        tuizu.EndTime = model.tuizutime;
                        tuizu.Explain = "退已支付账单";
                        isover = true;
                        list.Add(tuizu);
                    }
                    if (m.BeginTime >= model.tuizutime && model.tuizutime <= m.EndTime)
                    {
                        Inittuizu tuizu = new Inittuizu();
                        tuizu.Amount = n.Amount;
                        tuizu.Type = 0;
                        tuizu.Name = n.BillType;
                        tuizu.BeginTime = m.BeginTime;
                        tuizu.EndTime = m.EndTime;
                        tuizu.Explain = "退已支付账单";
                        list.Add(tuizu);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            result.numberCount = list.Count();
            result.numberData = list;
            return result;
        }
        //删除合同
        public SysResult DeleteContract(T_OwernContrct model)
        {
            SysResult result = new SysResult();
            ContrctDAL context = new ContrctDAL();
            try
            {
                string errmsg = "";
                bool re = context.CmdDelete(model.Id, "system", "sp_deleteowercontract", out errmsg);
                if (re == true)
                {
                    result = result.SuccessResult("删除成功");
                }
                else
                {
                    result = result.FailResult("删除失败" + errmsg);
                }
                return result;
            }
            catch (Exception ex)
            {
                return result = result.FailResult(ex.ToString());
            }
        }
        //退款
        public SysResult tuikuan(TuizuZhu model)
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
                            zhuid = context.Savetuizuzhu(model);
                        }
                        if (model.list != null)
                        {
                            context.Savetuizu(model.list, zhuid);
                        }
                        bool re = context.Cmdtuizu(zhuid, "system","sp_owerntuizu", out errmsg);
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
            int contract = dal.Querycount1(new T_OwernContrct() { Id = model.ContractId, Status = 7 });
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
        public SysResult xuzu(T_OwernContrct model)
        {
            SysResult result = new SysResult();
            using (var context = new TuizuDAL())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string errmsg = "";
                        long ContractId = context.updateOwernContrct(model);
                        bool re = context.Cmdxuzu(model.Id, "system", "sp_owerxuzu", out errmsg);
                        if (re == true && re == true)
                        {
                            dbContextTransaction.Commit();
                            return result = result.SuccessResult("续租成功");
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            return result = result.FailResult("续租失败" + errmsg);
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

    }
}
