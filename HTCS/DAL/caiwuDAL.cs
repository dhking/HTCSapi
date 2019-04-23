using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Mapping.cs.Contrct;
using Model;
using Model.Bill;
using Model.Contrct;
using Model.House;
using Model.TENANT;
using Model.User;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public   class caiwuDAL : RcsBaseDao
    {
        public List<T_Record> Query(T_Record model, OrderablePagination orderablePagination)
        {
            //整租查询
            var data = from m in dbtx select m;
            Expression<Func<T_Record, bool>> where = m => 1 == 1;
            if (model.BeginTime != DateTime.MinValue&& model.EndTime != DateTime.MinValue)
            {
                where = where.And(p=>p.createtime>= model.BeginTime&& p.createtime <= model.EndTime);
            }
            data = data.Where(where);
            IOrderByExpression<T_Record> order1 = new OrderByExpression<T_Record, long>(p => p.Id, true);
            List<T_Record> list = QueryableForList(data, orderablePagination, order1);
            return list;
        }
        public List<HouseReport> Querybaobiao(HouseReport model, OrderablePagination orderablePagination)
        {
            //整租查询
            var data = from m in baobiao select m;
            Expression<Func<HouseReport, bool>> where = m => 1 == 1;
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            data = data.Where(where);
            IOrderByExpression<HouseReport> order1 = new OrderByExpression<HouseReport, long>(p => p.Id, true);
            List<HouseReport> list = QueryableForList(data, orderablePagination, order1);
            return list;
        }

        public List<WrapHouseReportList> baobiaochildQuery(HouseReportList model, OrderablePagination orderablePagination)
        {
            var data = from m in baobiaolist
                       join n in HouseQuery on
                           m.houseid equals n.Id
                            into temp
                       from t in temp.DefaultIfEmpty()
                       select new WrapHouseReportList()
                       {
                           Id = m.Id,
                           housereportid=m.housereportid,
                           vacant=m.vacant,
                           profit=m.profit,
                           updatetime=m.updatetime,
                           houseid=m.houseid,
                           HouseName=t.Name,
                           CompanyId=t==null?0:t.CompanyId,
                           storeid = t == null ? 0 : t.storeid
                       };
            Expression<Func<WrapHouseReportList, bool>> where = m => 1 == 1;
            data = data.Where(where);
            if (model.housereportid != 0)
            {
                where = where.And(p=>p.housereportid==model.housereportid);
            }
            if (model.storeid != null)
            {
                where = where.And(p => p.storeid == model.storeid);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
           
            data = data.Where(where);
            IOrderByExpression<WrapHouseReportList> order1 = new OrderByExpression<WrapHouseReportList, long>(p => p.Id, true);
            List<WrapHouseReportList> list = QueryableForList(data, orderablePagination, order1);
            return list;
        }
        public DbSet<T_Record> dbtx { get; set; }

        public DbSet<HouseReport> baobiao { get; set; }
        public DbSet<HouseQuery> HouseQuery { get; set; }
        public DbSet<HouseReportList> baobiaolist { get; set; }

        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new T_RecordMapping());
            modelBuilder.Configurations.Add(new HouseReportMapping());
            modelBuilder.Configurations.Add(new HouseReportListMapping());
            modelBuilder.Configurations.Add(new HouseQueryMapping());
        }
    }
}
