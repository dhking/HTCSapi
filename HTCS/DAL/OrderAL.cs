using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model;
using Mapping.cs;
using System.Linq.Expressions;
using DBHelp;
using ControllerHelper;
using Model.TENANT;
using Model.House;

namespace DAL
{
    public class OrderAL : RcsBaseDao
    {
        //订单列表
        public List<wrapOrder> Querylist(wrapOrder model, OrderablePagination orderablePagination)
        {
            var data = (from m in BbOrder
             join n in Teant on m.UserId equals n.Id
             into temp
             from t in temp.DefaultIfEmpty()
             join c in t_v_HouseQuery on m.HouseId equals c.Id
             into temp1
             from x in temp1.DefaultIfEmpty()
             select new wrapOrder()
             {
                 Id=m.Id,
                 House=x.Name,
                 zkName=t.Name,
                 Pone=t.Phone,
                 Type=m.Type,
                 zftype=m.zftype,
                 CreateTime = m.CreateTime,
                 CompanyId = m.CompanyId,
                 Amount=m.Amount,
                 Status=m.Status,
                 ispt=m.ispt
             });
            Expression<Func<wrapOrder, bool>> where = m => 1 == 1;
            if (model.Content != null)
            {
                where = where.And(m => m.Pone.Contains(model.Content) || m.zkName.Contains(model.Content) || m.House.Contains(model.Content) );
            }
            if (model.Type != 0)
            {
                where = where.And(m => m.Type==m.Type);
            }
            if (model.Status != 0)
            {
                where = where.And(m => m.Status == m.Status);
            }
            if (model.zftype != null)
            {
                where = where.And(m => m.zftype == m.zftype);
            }
            if (model.BeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.CreateTime >= model.BeginTime && m.CreateTime <= model.EndTime);
            }
            if (model.minamount != 0)
            {
                where = where.And(m => m.Amount >= model.minamount && m.Amount <= model.maxamount);
            }
            data = data.Where(where);
            IOrderByExpression<wrapOrder> order1 = new OrderByExpression<wrapOrder, long>(p => p.Id, true);
            List<wrapOrder> list = QueryableForList(data, orderablePagination, order1);
            return list;
        }
        public DbSet<T_Teant> Teant { get; set; }
        public DbSet<HouseQuery> t_v_HouseQuery { get; set; }
        public DbSet<Order> BbOrder { get; set; }
        public DbSet<T_OrderList> BbFinance { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrderMapping());
            modelBuilder.Configurations.Add(new OrderListMapping());
            modelBuilder.Configurations.Add(new TenantMapping());
            modelBuilder.Configurations.Add(new HouseQueryMapping());
        }
    }
}
