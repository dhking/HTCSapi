using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Mapping.cs.Bill;
using Model;
using Model.Bill;
using Model.House;
using Model.TENANT;
using Model.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public   class BillDAL : RcsBaseDao
    {

        public List<T_WrapBill> Querylist(T_WrapBill model, OrderablePagination orderablePagination,T_SysUser user)
        {
          
            var data = (from a in Bill
                        join n in Teant on a.TeantId equals n.Id
                              into temp
                        from t in temp.DefaultIfEmpty()
                        join c in t_v_HouseQuery on a.HouseId equals c.Id
                         into temp1
                        from x in temp1.DefaultIfEmpty()
                       
                        select new T_WrapBill() { Id = a.Id,ContractId=a.ContractId,
                            Object=a.Object, PayStatus = a.PayStatus, ShouldReceive = a.ShouldReceive, HouseName =x.Name,
                          TeantName = t.Name,Phone=t.Phone, Amount=a.Amount,BeginTime=a.BeginTime,EndTime=a.EndTime,CellName=x.CellName,Liushui=a.Liushui,PayTime=a.PayTime,
                            HouseType = x == null ? 0 : x.RecentType,HouseId =x== null ? 0:x.Id,
                            BillType=a.BillType,type=a.type,
                            sign = a.sign,stage=a.stage,storeid = x == null ? 0 : x.storeid,
                            CompanyId =x == null ? 0 : x.CompanyId,
                            HouseKeeper = x == null ? 0 : x.HouseKeeper,
                            AreaName = x.AreaName,
                            CityName=x.CityName,
                            Remark=a.Remark
                          
                        });
          
            Expression<Func<T_WrapBill, bool>> where = m => 1 == 1;
           
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.storeid != 0)
            {
                where = where.And(m => m.storeid == model.storeid);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (model.arrCellNames != null)
            {
                if (model.arrCellNames.Length > 0)
                {
                    where = where.And(m => model.arrCellNames.Contains(m.CellName));
                }
            }
           
            if (user != null&& user.storeids!=null)
            {
                if (user.storeids.Length > 0 || user.range != 5)
                {
                    if (user.range == 1)
                    {
                        where = where.And(c => c.HouseKeeper == user.Id);
                    }
                    if (user.range == 2)
                    {
                        where = where.And(c => user.storeids.Contains(c.storeid));
                    }
                    if (user.range == 3)
                    {
                        where = where.And(c => user.areas.Contains(c.AreaName));
                    }
                    if (user.range == 4)
                    {
                        where = where.And(c => user.citys.Contains(c.CityName));
                    }
                    //where = where.Or(c => model.Status != 2);
                }
            }
            
            //if (model.Object != 2)
            //{
            //    where = where.And(m => m.Object == model.Object);
            //}
            if (model.Status != 0)
            {
                if (model.Status == 1)
                {
                    DateTime now = DateTime.Now.Date;
                    where = where.And(m => m.ShouldReceive< now);
                    where = where.And(m => m.PayStatus != 1);
                }
                if (model.Status == 2)
                {
                    DateTime now = DateTime.Now.Date;
                    where = where.And(m => m.ShouldReceive== now);
                }
                if (model.Status == 3)
                {
                    DateTime now = DateTime.Now.Date;
                    where = where.And(m => m.ShouldReceive> now);
                    where = where.And(m => m.PayStatus != 1);
                }
                if (model.Status == 4)
                {
                    where = where.And(m => m.PayStatus ==1);
                }
               
            }
           
            if (model.HouseId != 0)
            {
                where = where.And(m => m.HouseId == model.HouseId);
            }
            //查询未付账单和历史账单
            if (model.PayStatus != 2)
            {
                where = where.And(m => m.PayStatus == model.PayStatus);
            }
           
            if (model.BillType !=2)
            {
                where = where.And(m => m.BillType == model.BillType);
            }
            if (model.CellName!=null)
            {
                where = where.And(m => m.CellName == model.CellName);
            }
            if (model.Phone != null)
            {
                where = where.And(m => m.Phone == model.Phone);
            }
            if (model.ContractId != 0)
            {
                where = where.And(m => m.ContractId == model.ContractId);
            }
            if (model.HouseType != 0)
            {
                where = where.And(m => m.HouseType == model.HouseType);
            }
            if (model.HouseName != null)
            {
                where = where.And(m => model.HouseName.Contains(m.HouseName));
            }
            if (model.Content != null)
            {
                where = where.And(m => m.Phone.Contains(model.Content) || m.TeantName.Contains(model.Content) || m.HouseName.Contains(model.Content));
            }
            if (model.BeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.ShouldReceive >= model.BeginTime && m.ShouldReceive <= model.EndTime);
            }
            if (model.TeantId != 0)
            {
                where = where.And(m => m.TeantId == model.TeantId);
            }
            if (model.Object != 2)
            {
                where = where.And(m => m.Object == model.Object);
            }
           
            if (model.TeantName !=null)
            {
                where = where.And(m =>model.TeantName.Contains(m.TeantName));
            }
            bool should = false;
            if (model.OrderbyTime != 0&& model.OrderbyTime != null)
            {
                should = true;
            }
            data = data.Where(where);
            IOrderByExpression<T_WrapBill> order = new OrderByExpression<T_WrapBill, DateTime?>(p => p.ShouldReceive, should);
            List<T_WrapBill> wrapbill = new List<T_WrapBill>();
            wrapbill= this.QueryableForList<T_WrapBill>(data, orderablePagination, order);
            foreach (var mo in wrapbill)
            {
                //计算逾期时间
                if (mo.ShouldReceive != DateTime.MinValue)
                {
                    TimeSpan t3 = DateTime.Today - (DateTime)mo.ShouldReceive;
                    mo.Day = t3.Days;
                }
                if (mo.PayStatus == 1)
                {
                    mo.Status = 4;
                    continue;
                }
                if (mo.ShouldReceive < DateTime.Now.Date)
                {
                    mo.Status = 1;
                    continue;
                }
                if (mo.ShouldReceive == DateTime.Now.Date)
                {
                    mo.Status = 2;
                    continue;
                }

                if (mo.ShouldReceive > DateTime.Now)
                {
                    mo.Status = 3;
                    continue;
                }

            }


            return wrapbill;
        }
        public decimal Geiweishou(DateTime date)
        {
            decimal result = 0;
            var mo = from n in Bill join m in Billlist on n.Id equals m.BillId where n.EndTime == date && n.PayStatus == 0 select m.Amount;
            if (mo.Count() > 0)
            {
                result = mo.Sum();
            }
            return result;
        }
       
        //获取7天内收款账单数据
        public List<T_Bill> Geiweishou(DateTime datestart,DateTime dateend)
        {
           
            var mo = from n in Bill
                     where n.PayTime >= datestart && n.PayTime <= dateend
                     select n;
            
            return mo.ToList();
        }
        public List<T_WrapBill> tuizu_shou(long contractid,int BillType,int mobject)
        {
            var mo = from n in Bill
                     join m in Billlist on n.Id equals m.BillId into t
                     where 
                   n.ContractId == contractid
                     select new T_WrapBill() {Amount=n.Amount,BeginTime=n.BeginTime,EndTime=n.EndTime, list = t.ToList(),PayStatus=n.PayStatus, Liushui = n.Liushui,BillType=n.BillType,Object=n.Object };
            Expression<Func<T_WrapBill, bool>> where = m => 1 == 1;
            if (BillType != 2)
            {
                where = where.And(m => m.BillType == BillType);
            }
            if (mobject != 2)
            {
                where = where.And(m => m.Object == mobject);
            }
            mo = mo.Where(where);
            return mo.ToList();
        }
        //查询某合同下未收款的项目条目
        public int weishou(T_Bill model)
        {
            var mo = from m in Bill where m.PayStatus == 0&&model.ContractId==model.ContractId select m;
            return mo.Count();
        }
        public decimal Geiyishou(DateTime date)
        {
            decimal result = 0;
            var mo = from n in Bill join m in Billlist on n.Id equals m.BillId where n.EndTime==date && n.PayStatus == 1 select m.Amount;
            if (mo.Count() > 0)
            {
                result = mo.Sum();
            }
            return result;
        }
        public List<T_BillList> billlist(T_BillList model)
        {
            List<T_BillList> sysresult = new List<T_BillList>();
            var data = from a in Billlist select a;
            Expression<Func<T_BillList, bool>> where = m => 1 == 1;
            if (model.BillId != 0)
            {
                where = where.And(m => m.BillId == model.BillId);
            }
            data = data.Where(where);
           
            return data.ToList();
        }
        public T_WrapBill Querybillbyid(T_WrapBill model)
        {
            T_WrapBill wrap = new T_WrapBill();
            var data = (from a in Bill
                        join b in t_v_HouseQuery on a.HouseId equals b.Id

         into temp
                        from t in temp.DefaultIfEmpty()
                        join c in Teant on a.TeantId equals c.Id
                        into temp1
                        from x in temp1.DefaultIfEmpty()
                     
                        select new T_WrapBill() {Phone=x.Phone, Id = a.Id,Amount=a.Amount,TeantName=x.Name, RecentName = x.Name,PayStatus=a.PayStatus,BeginTime=a.BeginTime,EndTime=a.EndTime,ShouldReceive=a.ShouldReceive, HouseName=t.Name,PayTime=a.PayTime, Voucher=a.Voucher,PayType=a.PayType,CompanyId=a.CompanyId,Liushui=a.Liushui,stage=a.stage,ContractId=a.ContractId,HouseId=a.HouseId, TeantId=a.TeantId,Remark=a.Remark });
            Expression<Func<T_WrapBill, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            data = data.Where(where);
            wrap = data.FirstOrDefault();
            if (wrap != null)
            {
                wrap.list = (from a in Billlist where a.BillId == model.Id select a).ToList();
            }
            return wrap;
        }
        public int Delete(T_Bill model)
        {
            return DeleteModel(model); ;
        }
        public int wrap(PlAction<T_BillList, T_BillList> model)
        {
            pladd(model.dataadd);
            pledit(model.update);
            pldelete(model.delete);
            return this.SaveChanges();
        }
        public int save(T_Bill bill)
        {
            if (bill.Id == 0)
            {
                bill.Id= GetNextValNum("GET_WSEQUENCES('T_BILL')");
                PlAddModel<T_Bill>(bill);
                if (bill.list != null)
                {
                    foreach (var mo in bill.list)
                    {
                        mo.BillId = bill.Id;
                        mo.Id = GetNextValNum("GET_WSEQUENCES('T_BILLLIST')");
                        PlAddModel<T_BillList>(mo);
                    }
                }
            }
            else
            {
                PLModifiedModel<T_Bill>(bill,false, new[] { "PayStatus", "HouseId", "HouseType", "PayTime", "PayType", "CreatePerson", "TranSactor", "CreateTime",  "Voucher", "ContractId", "ShouldReceive",  "Explain", "TeantId", "BillType","sign" });
                if (bill.list != null)
                {
                    foreach (var mo in bill.list)
                    {
                        if (mo.Id == 0)
                        {
                            mo.Id = GetNextValNum("GET_WSEQUENCES('T_BILLLIST')");
                            mo.BillId = bill.Id;
                            PlAddModel<T_BillList>(mo);
                        }
                        else
                        {
                            PLModifiedModel<T_BillList>(mo, false);
                        }
                    }
                }
                if (bill.deletebilllist != null)
                {
                    foreach (var mo in bill.deletebilllist)
                    {
                        PLDeleteModel<T_BillList>(mo);
                    }
                }
            }
            return this.SaveChanges();
        }
        public int savebill(T_Bill bill)
        {
          
            if (bill.Id == 0)
            {
                bill.Id = GetNextValNum("GET_WSEQUENCES('T_BILL')");
                return  AddModel<T_Bill>(bill);
               
            }
            else
            {
                return ModifiedModel<T_Bill>(bill, false,bill.NotUpdatefield);
            }
            
        }
        public int updatebill(T_Bill model,params string[] updatearr)
        {
          return   ModifiedModel<T_Bill>(model,false, updatearr);
        }
        public void pladd(List<T_BillList> model)
        {
            if (model == null)
            {
                return;
            }
            foreach (var mo in model)
            {
                mo.Id =GetNextValNum("GET_WSEQUENCES('T_BILLLIST')");
                PlAddModel<T_BillList>(mo);
            }
        }
        public void pledit(List<T_BillList> model)
        {
            if(model==null)
            {
                return;
            }
            foreach (var mo in model)
            {
                PLModifiedModel<T_BillList>(mo,false,new string[] { "BillStage","BillId", "sign" });
            }
        }
        public void pldelete(List<T_BillList> model)
        {
            if (model == null)
            {
                return;
            }
            foreach (var mo in model)
            {
                PLDeleteModel<T_BillList>(mo);
            }
        }
        public T_Bill queryid(T_Bill model)
        {
            var mo = (from m in Bill where m.Id == model.Id select m).AsNoTracking();
            return mo.FirstOrDefault();
        }


        public T_Bill queryby(T_Bill model)
        {
            var mo = from m in Bill  select m;
            Expression<Func<T_Bill, bool>> where = m => 1 == 1;
            if (model.ContractId != 0)
            {
                where = where.And(p => p.ContractId == model.ContractId);
            }
            if (model.stage != 0)
            {
                where = where.And(p => p.stage == model.stage);
            }
            return mo.FirstOrDefault();
        }

        public List<T_Bill> queryidlist(List<long> listid)
        {
            var mo = from m in Bill where listid.Contains(m.Id) select m;
            return mo.ToList();
        }
        public DbSet<T_Teant> Teant { get; set; }
        public DbSet<T_Bill> Bill { get; set; }
        public DbSet<T_BillList> Billlist { get; set; }
        public DbSet<HouseModel> BbHouse { get; set; }
      
        public DbSet<HouseQuery> t_v_HouseQuery { get; set; }
      
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new HouseMapping());
            modelBuilder.Configurations.Add(new T_BilllistMapping());
            modelBuilder.Configurations.Add(new T_BillMapping());
   
            modelBuilder.Configurations.Add(new HouseQueryMapping());
            modelBuilder.Configurations.Add(new TenantMapping());

        }
    }
}

//(from a in Bill
//join n in Teant on a.TeantId equals n.Id
//      into temp
//from t in temp.DefaultIfEmpty()
//                        join c in BbHouse on a.HouseId equals c.Id
//                         into temp1
//                        from x in temp1.DefaultIfEmpty()
//                        where a.HouseType == 1
//                        select new T_WrapBill()
//{
//    Id = a.Id, PayStatus = a.Status, ShouldReceive = a.ShouldReceive, CellName = x.CellName,BuildingNumber = x.BuildingNumber,RoomId = x.RoomId,
//                            HouseName = "",TeantName = t.Name,Amount = a.Amount,BeginTime = a.BeginTime,EndTime = a.EndTime,City = x.City,Area = x.Area
//                            }).Union(from a in Bill join n in Teant on a.TeantId equals n.Id
//                          into temp from t in temp.DefaultIfEmpty()                                                               join c in t_v_House on a.HouseId equals c.Id into temp1
//       from x in temp1.DefaultIfEmpty()where(new int?[] { 2, 3 }).Contains(a.HouseType)select new T_WrapBill() { Id = a.Id, PayStatus = a.Status, ShouldReceive = a.ShouldReceive, CellName = x.CellName, BuildingNumber = x.BuildingNumber, RoomId = x.RoomId,HouseName = x.Name, TeantName = t.Name, Amount = a.Amount,BeginTime = a.BeginTime, EndTime = a.EndTime,City = x.City,Area = x.Area });



    //(from a in Bill join b in  BbHouse on a.HouseId equals b.Id

    //                    into temp from t in temp.DefaultIfEmpty()
    //                    join c in Teant on a.TeantId equals c.Id
    //                    into temp1
    //                    from x in temp1.DefaultIfEmpty()
    //                    where a.HouseType == 1
    //                    select new T_WrapBill() { Id = a.Id,House = t == null ? "" : t.CellName,RecentName = x.Name }).Union(
    //                              from a in Bill join b in t_v_House on a.HouseId equals b.Id
    //                              into temp from t in temp.DefaultIfEmpty()
    //                    join c in Teant on a.TeantId equals c.Id
    //                    into temp1
    //                    from x in temp1.DefaultIfEmpty()
    //                    where(new int?[] { 2, 3 }).Contains(a.HouseType)
    //                    select new T_WrapBill() { Id = a.Id, House = t == null ? "" : t.CellName, RecentName = x.Name });