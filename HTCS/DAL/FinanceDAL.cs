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
        public List<WrapFinanceModel> Querylist(FinanceModel model, OrderablePagination orderablePagination)
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
        CompanyId=o.CompanyId
                       };
            Expression<Func<WrapFinanceModel, bool>> where = m => 1 == 1;
            data = data.Where(where);
            IOrderByExpression<WrapFinanceModel> order1 = new OrderByExpression<WrapFinanceModel, long>(p => p.Id, true);
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
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
