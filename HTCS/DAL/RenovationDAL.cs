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
    public class RenovationDAL : RcsBaseDao
    {

        
        public List<WrapRenovation> Querylist(Renovation model,OrderablePagination orderablePagination)
        {
            var mo = from m in Renovation
                     join n in HouseQuery on m.houseid equals n.Id
                       into temp
                     from t in temp.DefaultIfEmpty()
                     select new WrapRenovation() { Id=m.Id, HouseName = t.Name, budget =m.budget , createtime =m.createtime, createperson=m.createperson, CompanyId=t==null?0:t.CompanyId };
            Expression<Func<WrapRenovation, bool>> where = m => 1 == 1;
            mo = mo.Where(where);
            if (model.CompanyId != model.CompanyId)
            {
                where = where.And(p => p.CompanyId == model.CompanyId);
            }
            IOrderByExpression<WrapRenovation> order = new OrderByExpression<WrapRenovation, DateTime>(p => p.createtime, true);
            return this.QueryableForList<WrapRenovation>(mo, orderablePagination, order);
        }

        public WrapRenovation Queryid(WrapRenovation model)
        {
            var mo = from m in Renovation
                     join n in HouseQuery on m.houseid equals n.Id
                     into temp
                     from t in temp.DefaultIfEmpty()
                     select new WrapRenovation() { Id = m.Id, HouseName = t.Name, budget = m.budget, createtime = m.createtime, createperson = m.createperson };
            Expression<Func<WrapRenovation, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(p => p.Id == model.Id);
            }
            mo = mo.Where(where);
            return mo.FirstOrDefault();
        }
        public List<TRenovationList> Querybylist(WrapRenovation model)
        {
            var mo = from m in TRenovationList
                     select m;
            Expression<Func<TRenovationList, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(p => p.parentid == model.Id);
            }
            mo = mo.Where(where);
            return mo.ToList();
        }
        public TRenovationList Repairxq(WrapRenovation model)
        {
            var mo = from m in TRenovationList
                     select m;
            Expression<Func<TRenovationList, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(p => p.Id == model.Id);
            }
            mo = mo.Where(where);
            return mo.FirstOrDefault();
        }
        //编辑父表
        public int Save(Renovation model)
        {
            int result = 0;
            if (model.Id == 0)
            {
                model.createtime = DateTime.Now;
                model.Id = GetNextValNum("GET_WSEQUENCES('T_RENOVATION')");
                result = AddModel(model);
            }
            else
            {
                result = ModifiedModel<Renovation>(model, false, new string[] { "CreateTime", "CreatePerson" });
            }
            return result;
        }
        //保存子表
        public int Savelist(TRenovationList model)
        {
            int result = 0;
            if (model.Id == 0)
            {
                model.time = DateTime.Now;
                model.Id = GetNextValNum("GET_WSEQUENCES('T_RENOVATIONLIST')");
                result = AddModel(model);
            }
            else
            {
                result = ModifiedModel<TRenovationList>(model, false, new string[] { "time", "createperson", "parentid" });
            }
            return result;
        }
        //编辑子表
        public int Savelist(List<TRenovationList> model)
        {
            foreach (var mo in model)
            {
                if (mo.Id == 0)
                {
                    mo.Id = GetNextValNum("GET_WSEQUENCES('T_BILLLIST')");
                    mo.time = DateTime.Now;
                    PlAddModel<TRenovationList>(mo);
                }
                else
                {
                    PLModifiedModel<TRenovationList>(mo, false, new string[] { "CreateTime", "CreatePerson" });
                }
            }
            return this.SaveChanges();
        }
        public List<TRenovationList> Queryrepairelist(TRenovationList model)
        {
            var data = from m in TRenovationList where m.parentid == model.parentid select m;
            return data.ToList();
        }
        public DbSet<Renovation> Renovation { get; set; }

        public DbSet<TRenovationList> TRenovationList { get; set; }
        public DbSet<HouseQuery> HouseQuery { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new renovationlistMapping());
            modelBuilder.Configurations.Add(new RenovationMapping());
            modelBuilder.Configurations.Add(new HouseQueryMapping());
        }
    }
}
