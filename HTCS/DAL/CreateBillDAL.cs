using DAL.Common;
using Model.Bill;
using Model.Contrct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class CreateBillDAL
    {

        //日租
        public List<T_Bill> getrzbill(paraCreate model)
        {
            List<T_Bill> listre = new List<T_Bill>();
            T_Bill bill = new T_Bill();
            bill.BeginTime = model.BeginTime;
            bill.EndTime = model.EndTime;
            bill.ContractId = model.Id;
            bill.ShouldReceive = model.BeginTime;
            bill.stage = 1;
            bill.list = new List<T_BillList>();
            //租金
            T_BillList zujin = new T_BillList();
            zujin.BillType = "租金";
            zujin.Amount = model.Recent;
            zujin.BillStage = 1;
            bill.list.Add(zujin);
            //杂费
            if (model.Otherfee != null)
            {
                foreach (var mo in model.Otherfee.Where(p => p.Pinlv == 0))
                {
                    T_BillList billlist = new T_BillList();
                    billlist.Amount = mo.Amount;
                    billlist.BillType = mo.Name;
                    billlist.BillStage = 1;
                    bill.list.Add(billlist);
                }
            }
            bill.Amount = bill.list.Sum(p => p.Amount);
            listre.Add(bill);
            return listre;
        }
        //获取所有账单列表
        public List<T_Bill> getwrapbill(paraCreate model)
        {
            List<T_Bill> list1 = getbill(model);
            List<T_Bill> list2 = getbill1(model);
            list1 = list1.Concat(list2).ToList();
            return list1;
        }
        
        public List<T_Bill> getbill(paraCreate model)
        {
            List<T_Bill> listbill = new List<T_Bill>();
            DateTime biibegin = DateTime.MinValue;
            DateTime billend = DateTime.MinValue;
            DateTime shouldreceive = DateTime.MinValue;
            //租金月数
            int zmonthcount = dateTimeDiff.toResult(model.BeginTime, model.EndTime.AddDays(1), diffResultFormat.mm)[0];
            double monthcount= zmonthcount+(model.EndTime - model.BeginTime.AddMonths(zmonthcount)).TotalDays / 30;
            int qi = int.Parse(Math.Ceiling(decimal.Parse((monthcount/model.PinLv).ToStr())).ToStr());
            for (int i = 1; i <= qi; i++)
            {
                if (i == 1)
                {
                    biibegin = model.BeginTime;
                    billend = biibegin.AddMonths(model.PinLv).AddDays(-1);
                    shouldreceive = biibegin;
                }
                else
                {
                    biibegin = billend.AddDays(1);
                    billend = biibegin.AddMonths(model.PinLv).AddDays(-1);
                    if (model.Recivetype == 1)
                    {
                        if (model.BeforeDay == 30)
                        {
                            shouldreceive = biibegin.AddMonths(-1);
                        }
                        if (model.BeforeDay != 30)
                        {
                            shouldreceive = biibegin.AddDays(0 - model.BeforeDay);
                        }
                    }
                    if (model.Recivetype == 2)
                    {
                        if (model.BeforeDay == 31)
                        {
                            shouldreceive = biibegin.AddMonths(1).AddDays(-1);
                        }
                        if (biibegin.Month == 2)
                        {
                            if (model.BeforeDay == 31|| model.BeforeDay == 30|| model.BeforeDay == 29|| model.BeforeDay == 28)
                            {
                                shouldreceive = biibegin.AddMonths(1).AddDays(-1);
                            }
                        }
                    }
                }
                T_Bill bill= createbill(model, biibegin, billend, shouldreceive, i);
                listbill.Add(bill);
            }
            return listbill;
        }
        public T_Bill createbill(paraCreate model, DateTime begin,DateTime end,DateTime shouldreceive,int stage)
        {
            //整月
            int zmonthcount = dateTimeDiff.toResult(begin, model.EndTime.AddDays(1), diffResultFormat.mm)[0];
            double monthcount = zmonthcount + (model.EndTime - begin.AddMonths(zmonthcount)).TotalDays / 30;
            end = getend(monthcount, model.PinLv, end, model.EndTime);
            decimal reentamount = getrecentamount(monthcount,model.PinLv,model.Recent,begin,end);
            T_Bill bill = new T_Bill();
            bill.TeantId = model.TeantId;
            bill.BeginTime = begin;
            bill.EndTime = end;
            bill.HouseId = model.HouseId;
            bill.CreatePerson = model.CreatePersonstr;
            bill.CreateTime = DateTime.Now;
            bill.ContractId = model.Id;
            bill.HouseType = model.HouseType;
            bill.ShouldReceive = shouldreceive;
            bill.CompanyId = model.CompanyId;
            bill.stage = stage;
            bill.list = new List<T_BillList>();
            //租金
            T_BillList zujin = new T_BillList();
            zujin.BillType = "租金";
            zujin.Amount = reentamount;
            zujin.BillStage = stage;
            bill.list.Add(zujin);
            //杂费
            if (model.Otherfee != null)
            {
                foreach (var mo in model.Otherfee.Where(p => p.Pinlv == 0 || p.Pinlv != 999))
                {
                    T_BillList billlist = new T_BillList();
                    if (mo.IsYajin == 0 && mo.Pinlv != 999)
                    {
                        billlist.Amount = getrecentamount(monthcount, model.PinLv, model.Recent, begin, end);
                    }
                    if ((mo.IsYajin == 1 || mo.Pinlv == 999) && stage == 1)
                    {
                        billlist.Amount =mo.Amount;
                    }
                    billlist.BillType = mo.Name;
                    billlist.BillStage = stage;
                    bill.list.Add(billlist);
                }
            }
            bill.Amount = bill.list.Sum(p => p.Amount);
            return bill;
        }
        //不随押金支付的杂费
        public List<T_Bill> getbill1(paraCreate model)
        {
            List<T_Bill> listbill = new List<T_Bill>();
            DateTime biibegin = DateTime.MinValue;
            DateTime billend = DateTime.MinValue;
            DateTime shouldreceive = DateTime.MinValue;
            //租金月数
            int zmonthcount = dateTimeDiff.toResult(model.BeginTime, model.EndTime.AddDays(1), diffResultFormat.mm)[0];
            double monthcount = zmonthcount + (model.EndTime - model.BeginTime.AddMonths(zmonthcount)).TotalDays / 30;
            int qi = int.Parse(Math.Ceiling(decimal.Parse((monthcount / model.PinLv).ToStr())).ToStr());
            for (int i = 1; i <= qi; i++)
            {
                if (i == 1)
                {
                    biibegin = model.BeginTime;
                    billend = biibegin.AddMonths(model.PinLv).AddDays(-1);
                    shouldreceive = biibegin;
                }
                else
                {
                    biibegin = billend.AddDays(1);
                    billend = biibegin.AddMonths(model.PinLv).AddDays(-1);
                    if (model.Recivetype == 1)
                    {
                        if (model.BeforeDay == 30)
                        {
                            shouldreceive = biibegin.AddMonths(-1);
                        }
                        if (model.BeforeDay != 30)
                        {
                            shouldreceive = biibegin.AddDays(0 - model.BeforeDay);
                        }
                    }
                    if (model.Recivetype == 2)
                    {
                        if (model.BeforeDay == 31)
                        {
                            shouldreceive = biibegin.AddMonths(1).AddDays(-1);
                        }
                        if (biibegin.Month == 2)
                        {
                            if (model.BeforeDay == 31 || model.BeforeDay == 30 || model.BeforeDay == 29 || model.BeforeDay == 28)
                            {
                                shouldreceive = biibegin.AddMonths(1).AddDays(-1);
                            }
                        }
                    }
                }
                T_Bill bill = createbillnotzujin(model, biibegin, billend, shouldreceive, i);
                listbill.Add(bill);
            }
            return listbill;
        }
        public T_Bill createbillnotzujin(paraCreate model, DateTime begin, DateTime end, DateTime shouldreceive, int stage)
        {
            //整月
            int zmonthcount = dateTimeDiff.toResult(begin, model.EndTime.AddDays(1), diffResultFormat.mm)[0];
            double monthcount = zmonthcount + (model.EndTime - begin.AddMonths(zmonthcount)).TotalDays / 30;
            end = getend(monthcount, model.PinLv, end, model.EndTime);
            decimal reentamount = getrecentamount(monthcount, model.PinLv, model.Recent, begin, end);
            T_Bill bill = new T_Bill();
            bill.TeantId = model.TeantId;
            bill.BeginTime = begin;
            bill.EndTime = end;
            bill.HouseId = model.HouseId;
            bill.CreatePerson = model.CreatePersonstr;
            bill.CreateTime = DateTime.Now;
            bill.ContractId = model.Id;
            bill.HouseType = model.HouseType;
            bill.ShouldReceive = shouldreceive;
            bill.CompanyId = model.CompanyId;
            bill.stage = stage;
            bill.list = new List<T_BillList>();
            //杂费
            if (model.Otherfee != null)
            {
                foreach (var mo in model.Otherfee.Where(p => p.Pinlv != 0 && p.Pinlv != 999&&p.IsYajin==0))
                {
                    T_BillList billlist = new T_BillList();
                    billlist.Amount = getrecentamount(monthcount, model.PinLv, model.Recent, begin, end);
                    billlist.BillType = mo.Name;
                    billlist.BillStage = stage;
                    bill.list.Add(billlist);
                }
            }
            bill.Amount = bill.list.Sum(p => p.Amount);
            return bill;
        }
        //获取结束时间
        public DateTime getend(double monthcount,int pinlv,DateTime end,DateTime contractend)
        {
            if(monthcount< pinlv)
            {
                return contractend;
            }
            else
            {
                return end;
            }
        }
        //获取账单金额
        public decimal getrecentamount(double monthcount,int PinLv, decimal Recent,DateTime begin,DateTime end)
        {
            decimal reentamount = 0;
            //月份大于频率
            if (monthcount>PinLv)
            {
                reentamount = Recent * PinLv;
            }
            else
            {
                reentamount = getamount(monthcount, begin, end, Recent);
            }
            reentamount=decimal.Parse(Math.Round(reentamount, 2).ToStr());
            return reentamount;
        }
        public decimal getamount(double  monthcount,DateTime begin,DateTime end,decimal recent)
        {
            int zy =int.Parse(Math.Floor(monthcount).ToString());
            int syday = (end - begin.AddMonths(zy).AddDays(0 - 1)).Days ;
            decimal amount = 0;
            amount = zy * recent;
            amount+= syday * (recent / 30);
            return amount;
        }
    }
}
