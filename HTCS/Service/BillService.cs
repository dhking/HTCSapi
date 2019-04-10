﻿using ControllerHelper;
using DAL;
using Model;
using Model.Bill;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class BillService
    {
        BillDAL dal = new BillDAL();
        public SysResult<List<T_WrapBill>> Querybase(T_WrapBill model, OrderablePagination orderablePagination,T_SysUser user)
        {
            SysResult<List<T_WrapBill>> result = new SysResult<List<T_WrapBill>>();
         
            List<T_WrapBill> list = dal.Querylist(model, orderablePagination, user);
            result.numberCount = orderablePagination.TotalCount;
            result.numberData = list;
            return result;
        }
        public SysResult<List<T_BillList>> billlist(T_BillList model)
        {
            SysResult<List<T_BillList>> result = new SysResult<List<T_BillList>>();
            List<T_BillList>  list=dal.billlist(model);
            result.numberCount = list.Count();
            result.numberData = list;
            return result;
        }
        public SysResult edit(PlAction<T_BillList, T_BillList> model)
        {
            SysResult result = new SysResult();
            try
            {
               
                if (dal.wrap(model)>0)
                {
                   
                    result = result.SuccessResult("操作成功");
                }
                else
                {
                    result = result.FailResult("操作失败");
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("操作异常",ex);
            }
            return result; 
        }
        public SysResult save(T_Bill  bill,long userid)
        {
            SysResult result = new SysResult();
            try
            {
                if (bill.list !=null)
                {
                    bill.Amount = bill.list.Sum(p => p.Amount);
                }
                if (bill.Amount == 0)
                {
                  return   result = result.FailResult("总金额不能为0");
                }
                if (bill.Id != 0)
                {
                    RzService service = new RzService();
                    service.addzdrz(bill.HouseId, userid, bill.CompanyId, 2);
                }
                if (dal.save(bill) >0)
                {
                    result = result.SuccessResult("保存成功");
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("操作异常", ex);
            }
            return result;
        }
        public SysResult savebill(T_Bill bill)
        {
            SysResult result = new SysResult();
            try
            {
                bill.PayStatus = 1;
                if (dal.savebill(bill) > 0)
                {
                    result = result.SuccessResult("保存成功");
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("操作异常", ex);
            }
            return result;
        }
        //删除房间
        public SysResult deleteData(T_Bill ids)
        {

            SysResult result = new SysResult();
            if (dal.Delete(ids) > 0)
            {
                return result = result.SuccessResult("删除成功");
            }
            else
            {
                return result = result.FailResult("删除失败");
            }
        }
        public SysResult receive(T_Bill model)
        {
            SysResult result = new SysResult();
            try
            {
                T_Bill remo = dal.queryid(model);
                if (remo.PayStatus == 1)
                {
                    return result = result.FailResult("账单已处理");
                }
                if (remo.PayStatus == 4)
                {
                   return  result = result.FailResult("收款状态错误，收款失败");
                }
                remo.PayStatus = 1;
                remo.PayTime = model.PayTime;
                remo.PayType = model.PayType;
                remo.Voucher = model.Voucher;
                remo.Liushui = model.Liushui;
                remo.Remark = model.Remark;
                if(dal.updatebill(remo) > 0)
                {
                    result = result.SuccessResult("收款成功");
                }
                else
                {
                    result = result.FailResult("收款失败");
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("操作异常", ex);
            }
            return result;
        }
        //批量收款
        public SysResult plreceive(T_WrapBill model)
        {
            SysResult result = new SysResult();
            try
            {
                List<long> listid = model.list.Select(p => p.Id).ToList();
                List<T_Bill> remolist = dal.queryidlist(listid);
                foreach(var remo in remolist)
                {
                    if (remo.PayStatus == 1)
                    {
                        return result = result.FailResult("账单已处理");
                    }
                    if (remo.PayStatus == 4)
                    {
                        return result = result.FailResult("收款状态错误，收款失败");
                    }
                    remo.PayStatus = 1;
                    remo.PayTime = model.PayTime;
                    remo.PayType = model.PayType;
                    remo.Status = model.Status;
                    remo.Liushui = model.Liushui;
                    remo.Voucher = model.Voucher;
                    remo.Remark = model.Remark;
                    if (dal.updatebill(remo) > 0)
                    {
                        result = result.SuccessResult("收款成功");
                    }
                    else
                    {
                        result = result.FailResult("收款失败");
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("操作异常", ex);
            }
            return result;
        }
        public SysResult<T_WrapBill> Querybase(T_WrapBill model)
        {
            SysResult<T_WrapBill> result = new SysResult<T_WrapBill>();
            T_WrapBill mo= dal.Querybillbyid(model);
            
            if (mo.PayStatus == 1)
            {
                mo.Status = 4;
            }
            if (mo.ShouldReceive < DateTime.Now.Date&& mo.PayStatus ==0)
            {
                mo.Status = 1;

            }
            if (mo.ShouldReceive == DateTime.Now.Date && mo.PayStatus == 0)
            {
                mo.Status = 2;
            }

            if (mo.ShouldReceive > DateTime.Now && mo.PayStatus == 0)
            {
                mo.Status = 3;
            }
            result.numberData =mo;
            return result;
        }
    }
}
