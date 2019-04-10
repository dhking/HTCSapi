using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Mapping.cs.Contrct;
using Model;
using Model.Contrct;
using Model.House;
using Model.TENANT;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public   class AutoTaskkDAL : RcsBaseDao
    {
        public List<SysAutoTaskModel> Querylist(SysAutoTaskModel model, OrderablePagination orderablePagination)
        {
            var data = from m in TaskModel select m;
            Expression<Func<SysAutoTaskModel, bool>> where = m => 1 == 1;
            if (!string.IsNullOrEmpty(model.JobName))
            {
                where = where.And(m => m.JobName == model.JobName);
            }
            data = data.Where(where);
            IOrderByExpression<SysAutoTaskModel> order = new OrderByExpression<SysAutoTaskModel, long>(p => p.Id, false);
            List<SysAutoTaskModel> list = QueryableForList(data, orderablePagination, order);
            return list;
        }
        public List<SysAutoTaskServiceModel> Querylist1(SysAutoTaskServiceModel model, OrderablePagination orderablePagination)
        {
            var data = from m in TaskServiceModel select m;
            Expression<Func<SysAutoTaskServiceModel, bool>> where = m => 1 == 1;
            
            data = data.Where(where);
            IOrderByExpression<SysAutoTaskServiceModel> order = new OrderByExpression<SysAutoTaskServiceModel, long>(p => p.Id, false);
            List<SysAutoTaskServiceModel> list = QueryableForList(data, orderablePagination, order);
            return list;
        }
        public List<SysAutoTaskTriggerModel> Querylist2(SysAutoTaskTriggerModel model, OrderablePagination orderablePagination)
        {
            var data = from m in TaskTrigerModel select m;
            Expression<Func<SysAutoTaskTriggerModel, bool>> where = m => 1 == 1;

            data = data.Where(where);
            IOrderByExpression<SysAutoTaskTriggerModel> order = new OrderByExpression<SysAutoTaskTriggerModel, long>(p => p.Id, false);
            List<SysAutoTaskTriggerModel> list = QueryableForList(data, orderablePagination, order);
            return list;
        }
        public List<SysAutoTaskHistoryModel> Querylist3(SysAutoTaskHistoryModel model, OrderablePagination orderablePagination)
        {
            var data = from m in TaskHistoryModel select m;
            Expression<Func<SysAutoTaskHistoryModel, bool>> where = m => 1 == 1;

            data = data.Where(where);
            IOrderByExpression<SysAutoTaskHistoryModel> order = new OrderByExpression<SysAutoTaskHistoryModel, long>(p => p.Id, false);
            List<SysAutoTaskHistoryModel> list = QueryableForList(data, orderablePagination, order);
            return list;
        }
        public DbSet<SysAutoTaskModel> TaskModel { get; set; }
        public DbSet<SysAutoTaskHistoryModel> TaskHistoryModel { get; set; }
        public DbSet<SysAutoTaskServiceModel> TaskServiceModel { get; set; }
        public DbSet<SysAutoTaskTriggerModel> TaskTrigerModel { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AutoTotaskMapping());
            modelBuilder.Configurations.Add(new AutoTotaskServiceMapping());
            modelBuilder.Configurations.Add(new AutoTotaskTrigerMapping());
            modelBuilder.Configurations.Add(new AutoTotaskHistoryMapping());
        }
        
    }
}
