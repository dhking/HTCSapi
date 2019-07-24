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
    public class FinanceDAL : RcsBaseDao
    {
        public List<WrapFinanceModel> Querylist(FinanceModel model, OrderablePagination orderablePagination, long[] userids, T_SysUser user)
        {
            var data = from o in BbFinance join x in t_v_HouseQuery on o.HouseId equals  x.Id
                       into temp from t in temp.DefaultIfEmpty()
           select new WrapFinanceModel() { Id=o.Id,     
           HouseId = o.HouseId ,
           Trader= o.Trader ,
           Type= o.Type ,
           CostName= o.CostName,
           Amount= o.Amount ,
           PayType= o.PayType ,
           TradingDate= o.TradingDate,
           Remark=o.Remark,
           TrandId=o.TrandId,
           HouseName= t.Name,
           PayMentNumber=o.PayMentNumber,
           Transaoctor=o.Transaoctor,
           CompanyId=o.CompanyId,
           HouseType = t==null?0: t.RecentType,
           CityName=t.CityName,
           AreaName=t.AreaName,
           CellName=t.CellName,
           HouseKeeper=t.HouseKeeper,
           storeid=t.storeid
           };
            Expression<Func<WrapFinanceModel, bool>> where = m => 1 == 1;
            data = data.Where(where);
            IOrderByExpression<WrapFinanceModel> order1 = new OrderByExpression<WrapFinanceModel, long>(p => p.Id, true);
            //部门信息筛选
            if (user != null)
            {
                if (user.departs != null && user.roles != null)
                {
                    List<long> depentids = user.departs.Select(p => p.Id).ToList();
                    if (user.roles.range == 2)
                    {
                        where = where.And(m => depentids.Contains(m.storeid));
                        if (userids != null && userids.Length > 0)
                        {
                            where = where.Or(m => userids.Contains(m.HouseKeeper));
                        }
                    }
                    if (user.roles.range == 3)
                    {
                        where = where.And(m => m.HouseKeeper == user.roles.userid);
                    }
                }
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (model.HouseType != 0)
            {
                where = where.And(m => m.HouseType == model.HouseType);
            }
            if (model.Type != 0)
            {
                where = where.And(m => m.Type == model.Type);
            }
           
            if (model.HouseType != 0)
            {
                where = where.And(m => m.HouseType == model.HouseType);
            }
            if (!string.IsNullOrEmpty(model.PayType))
            {
                where = where.And(m => m.PayType == model.PayType);
            }
            if (!string.IsNullOrEmpty(model.Content))
            {
                where = where.And(m => m.HouseName.Contains(model.Content) || m.Trader.Contains(model.Content));
            }
            if (!string.IsNullOrEmpty(model.CostName))
            {
                where = where.And(m => m.CostName == model.CostName);
            }
            if (!string.IsNullOrEmpty(model.CityName))
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            if (!string.IsNullOrEmpty(model.AreaName))
            {
                where = where.And(m => m.AreaName == model.AreaName);
            }
            if (model.arrCellNames!=null)
            {
                where = where.And(m => model.arrCellNames.Contains(m.CellName));
            }
            if (model.BeginTime!=DateTime.MinValue)
            {
                where = where.And(m => m.TradingDate>= model.BeginTime&& m.TradingDate <= model.EndTime);
            }
            data = data.Where(where);
            List<WrapFinanceModel> list = QueryableForList(data, orderablePagination, order1);
            return list;
        }
       
        public WrapFinanceModel Queryxq(FinanceModel model)
        {
            var data = from o in BbFinance
                       join x in t_v_HouseQuery on o.HouseId equals x.Id
   into temp
                       from t in temp.DefaultIfEmpty()
                       where o.Id==model.Id
                       select new WrapFinanceModel()
                       {
                           Id = o.Id,
                           HouseId = o.HouseId,
                           Trader = o.Trader,
                           Type = o.Type,
                           CostName = o.CostName,
                           Amount = o.Amount,
                           PayType = o.PayType,
                           TradingDate = o.TradingDate,
                           Remark = o.Remark,
                           TrandId = o.TrandId,
                           HouseName = t.Name
                       };
            return data.FirstOrDefault();
        }
        public int save(FinanceModel bill)
        {
            if (bill.Id == 0)
            {
                bill.Id = GetNextValNum("GET_WSEQUENCES('T_FINANCE')");
                PlAddModel<FinanceModel>(bill);
            }
            else
            {
                PLModifiedModel<FinanceModel>(bill, false);
            }
            return this.SaveChanges();
        }
        public DbSet<HouseQuery> t_v_HouseQuery { get; set; }
        public DbSet<FinanceModel> BbFinance { get; set; }
        
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FinanceMapping());
            modelBuilder.Configurations.Add(new HouseQueryMapping());
           
        }
    }
}
