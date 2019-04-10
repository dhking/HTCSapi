using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Mapping.cs.Contrct;
using Model;
using Model.Contrct;
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
    public  class RepaireDAL : RcsBaseDao
    {
        public List<WrapRepaire> Querylist(Repaire model, OrderablePagination orderablePagination)
        {
            var data = (from m in BbRepaire  join n in BbRepairelist  on m.Id equals n.RepairId into temp from t in temp
                        select new WrapRepaire(){CreateTime=m.CreateTime,Id=m.Id,AppiontTime=m.AppiontTime,City=m.City,Area=m.Area,House=m.House,JournaList=m.JournaList,Adress=m.Adress,Phone=m.Phone, CompanyId = m.CompanyId }).Distinct();
            //筛选报修项
            if (!string.IsNullOrEmpty(model.projectname))
            {
                var data1 = from j in BbRepairelist
                            where j.Project==model.projectname
                            select j;
                data = (from m in BbRepaire join n in data1 on m.Id equals n.RepairId into temp from t in temp  select new WrapRepaire() { CreateTime = m.CreateTime, AppiontTime = m.AppiontTime, Id = m.Id, City = m.City, Area = m.Area, House = m.House, JournaList = m.JournaList, Adress = m.Adress, Phone = m.Phone, CompanyId = m.CompanyId }).Distinct();
            }
          


            if (model.Status != 3)
            {
                var data1 = from j in BbRepairelist
                            where j.Status == model.Status
                            select j;
                data = (from m in BbRepaire join n in data1 on m.Id equals n.RepairId into temp from t in temp select new WrapRepaire() { CreateTime = m.CreateTime, AppiontTime = m.AppiontTime, Id = m.Id, City = m.City, Area = m.Area, House = m.House, JournaList = m.JournaList, Adress = m.Adress, Phone = m.Phone,CompanyId=m.CompanyId }).Distinct();
            }
            //如果承接人不等于0
            if (model.UserId!=0)
            {
                var data1 = from j in BbRepairelist
                            where j.UserId == model.UserId
                            select j;
                data = (from m in BbRepaire join n in data1 on m.Id equals n.RepairId into temp from t in temp select new WrapRepaire() { CreateTime = m.CreateTime, AppiontTime = m.AppiontTime, Id = m.Id, City = m.City, Area = m.Area, House = m.House, JournaList = m.JournaList, Adress = m.Adress, Phone = m.Phone, CompanyId = m.CompanyId }).Distinct();
            }
            Expression<Func<WrapRepaire, bool>> where = m => 1 == 1;
            if (model.Content != null)
            {
                where = where.And(m => m.Phone.Contains(model.Content) || m.JournaList.Contains(model.Content) || m.House.Contains(model.Content));
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (model.BeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.AppiontTime >= model.BeginTime && m.AppiontTime <= model.EndTime);
            }
            //按照地址筛选
            if (!string.IsNullOrEmpty(model.Area))
            {
                where = where.And(m => m.Area == model.Area);
            }
            if (!string.IsNullOrEmpty(model.Phone))
            {
                where = where.And(m => m.Phone == model.Phone);
            }
            data = data.Where(where);
            bool group = false;
            if (model.group == 1)
            {
                group = true;
            }
            IOrderByExpression<WrapRepaire> order1 = new OrderByExpression<WrapRepaire, DateTime>(p => p.AppiontTime, group);
            List<WrapRepaire> list = QueryableForList(data, orderablePagination, order1);
            
            return list;
        }
        public List<RepairList> Queryrepairelist(RepairList model)
        {
            var data = from m in BbRepairelist where m.RepairId==model.RepairId select m;
            return data.ToList();
        }
        //查询报修列表
        public List<RepairList> Queryrepairelist1(RepairList model)
        {
            var data = from m in BbRepairelist  select m;
            Expression<Func<RepairList, bool>> where = m => 1 == 1;
            //按照地址筛选
            if (model.Status != 999)
            {
                where = where.And(m => m.Status == model.Status);
            }
            if (model.Phone != null)
            {
                where = where.And(m => m.Phone == model.Phone);
            }
            if (model.JournaList != null)
            {
                where = where.And(m => m.JournaList == model.JournaList);
            }
            if (model.Urgent != 0)
            {
                where = where.And(m => m.Urgent == model.Urgent);
            }
            
            data = data.Where(where);
            return data.ToList();
        }
        //根据接单人查询数量
        public int Queryrepairelist(long UserId)
        {
            var data = from m in BbRepairelist where m.UserId == UserId select m;
            return data.Count();
        }
        public List<WrapRepairList> Queryrepairelistin(List<long> arr)
        {
            var data = from m in BbRepairelist where arr.Contains(m.RepairId) select new WrapRepairList() { Id=m.Id,Content=m.Content,RepairId=m.RepairId,Project=m.Project,Status=m.Status,Urgent=m.Urgent};
            return data.ToList();
        }
        //查询头部的信息
        public List<Repaire> Queryrepaire(List<long> arr)
        {
            var data = from m in BbRepaire where arr.Contains(m.Id) select  m;
            return data.ToList();
        }
        public WrapRepaire Queryxq(Guest model)
        {
            WrapRepaire remodel = new WrapRepaire();
            var data = from m in BbRepaire
                      
                       where m.Id == model.Id
                       select new WrapRepaire(){ HouseId=m.HouseId, Id = m.Id, Phone =m.Phone,Adress=m.Adress, City=m.City,Area=m.Area, AppiontTime =m.AppiontTime,House=m.House,JournaList=m.JournaList};
            remodel = data.FirstOrDefault();
            remodel.Ipimgadess = "http://106.14.96.37:82/";
            if (remodel != null)
            {
                var data1 = (from m in BbRepairelist
                             join n in BbUser on m.UserId equals n.Id into temp
                             from t in temp.DefaultIfEmpty()
                             where m.RepairId == remodel.Id select new WrapRepairList() {Id=m.Id,Project=m.Project,Status=m.Status,Urgent=m.Urgent,Image=m.Image,Imageweixiu=m.Imageweixiu,Content=m.Content,UserName=t.RealName,UserPhone=t.Mobile,Remark=m.Remark});
                remodel.list = data1.OrderByDescending(a => a.Id).ToList();
            }
            return remodel;
        }
        public int saverepairlist(RepairList bill)
        {
            if (bill.Id == 0)
            {
                bill.Id= GetNextValNum("GET_WSEQUENCES('T_REPAIRLIST')");
                return AddModel<RepairList>(bill);
            }
            else
            {
                return ModifiedModel1<RepairList>(bill,false,new string[] { "UserId","Status" });
            }  
        }
        public int save(Repaire bill)
        {
            if (bill.Id == 0)
            {
                bill.Id = GetNextValNum("GET_WSEQUENCES('T_REPAIR')");
                if (bill.UserId != 0)
                {
                    bill.Status = 1;
                }
                bill.CreateTime = DateTime.Now;
                PlAddModel<Repaire>(bill);
                if (bill.list != null)
                {
                    foreach (var mo in bill.list)
                    {
                        //if (mo.Image != null && mo.Image != "")
                        //{
                        //    mo.Image = mo.Image.Substring(0, mo.Image.Length - 1);
                        //}
                        mo.RepairId = bill.Id;
                        mo.Id = GetNextValNum("GET_WSEQUENCES('T_REPAIRLIST')");
                        PlAddModel<RepairList>(mo);
                    }
                }
            }
            else
            {
                PLModifiedModel<Repaire>(bill, false, new[] { "CreateTime" });
                if (bill.list != null)
                {
                    foreach (var mo in bill.list)
                    {
                        mo.RepairId = bill.Id;
                        if (mo.Id == 0)
                        {
                            mo.Id = GetNextValNum("GET_WSEQUENCES('T_REPAIRLIST')");
                            PlAddModel<RepairList>(mo);
                        }
                        else
                        {
                            PLModifiedModel<RepairList>(mo, false, new[] { "" });
                        }
                    }
                }
                if (bill.deletebilllist != null)
                {
                    foreach (var mo in bill.deletebilllist)
                    {
                        PLDeleteModel<RepairList>(mo);
                    }
                }
            }
            return this.SaveChanges();
        }
      
        public DbSet<HouseQuery> t_v_HouseQuery { get; set; }
        public DbSet<Repaire> BbRepaire { get; set; }
        public DbSet<RepairList> BbRepairelist { get; set; }
        public DbSet<T_Teant> Teant { get; set; }
        public DbSet<T_SysUser> BbUser { get; set; }
        public DbSet<HouseTrant> HouseTrant { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RepaireListMapping());
            modelBuilder.Configurations.Add(new HouseTeantMapping());
            modelBuilder.Configurations.Add(new RepaireMapping());
            modelBuilder.Configurations.Add(new HouseQueryMapping());
            modelBuilder.Configurations.Add(new TenantMapping());
            modelBuilder.Configurations.Add(new T_SysUserMapping());
        }
    }

    public static class ty
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
