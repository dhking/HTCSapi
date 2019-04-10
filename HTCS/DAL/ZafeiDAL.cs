using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class ZafeiDAL : RcsBaseDao
    {
        public List<T_ZafeiList> Query(T_ZafeiList model)
        {
            var data = from m in Zafei select m;
            Expression<Func<T_ZafeiList, bool>> where = m => 1 == 1;
            if (model.IsActive != 999)
            {
                where = where.And(m => m.IsActive == model.IsActive);
            }
            if (model.TuiType != 999)
            {
                where = where.And(m => m.TuiType == model.TuiType);
            }
            data = data.Where(where);
            return data.ToList();

        }
        public List<T_ZafeiList> Queryfy(T_ZafeiList model, OrderablePagination orderablePagination)
        {
            var data = from m in Zafei select m;
            IOrderByExpression<T_ZafeiList> order = new OrderByExpression<T_ZafeiList, long>(p => p.Id, false);
            
            Expression<Func<T_ZafeiList, bool>> where = m => 1 == 1;
            if (model.IsActive != 999)
            {
                where = where.And(m => m.IsActive == model.IsActive);
            }
            if (model.TuiType != 999)
            {
                where = where.And(m => m.TuiType == model.TuiType);
            }
            if (model.IsYajin != 999)
            {
                where = where.And(m => m.IsYajin == model.IsYajin);
            }
            List<T_ZafeiList> list = QueryableForList(data, orderablePagination, order);
            return data.ToList();
        }
        public int SaveZafei(T_ZafeiList model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_OTHERFEELIST')");
                return AddModel(model);
            }
            else
            {
                return ModifiedModel(model, false,model.NotUpdatefield);
            }

        }
        public T_ZafeiList QueryId(T_ZafeiList model)
        {
            var data = from a in Zafei where a.Id == model.Id select a;
            return data.FirstOrDefault();
        }
       
        public long deletedata(long model)
        {
            T_ZafeiList basc = new T_ZafeiList();
            basc.Id = model;
            DeleteModel(basc);
            return basc.Id;
        }
        public DbSet<T_ZafeiList> Zafei { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new T_ZafeiListMapping());
        }
    }
}
