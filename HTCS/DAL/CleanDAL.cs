using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Model;
using Model.House;
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
    public  class CleanDAL : RcsBaseDao
    {
       //查询
        public List<Wrapclean> Querylist(Wrapclean model,OrderablePagination orderablePagination)
        {

            var data = from m in Bbclean
                       join n in BbUser on m.executor equals n.Id into temp
                       from t in temp.DefaultIfEmpty()
                       join x in t_v_HouseQuery on m.houseid equals x.Id into temp1
                       from t1 in temp1
                       select new Wrapclean() {Id=m.Id, houseid=m.houseid,
                           status =m.status, ugent=m.ugent, apply =m.apply,
                           appointment=m.appointment,
                           applyperson=m.applyperson,
                           phone=m.phone,
                           project=m.project,
                           remark=m.remark,
                           executor=m.executor,
                           executorstr = t == null ?"待认领" : t.RealName,
                           companyid = t1 == null?0:t1.CompanyId,
                           RecentType = t1 == null ? 0 : t1.RecentType,
                           house =t1.Name,
                           cityname=t1.CityName,
                           cellname=t1.CellName,
                           expectedtime = m.expectedtime,
                           areaname= t1.AreaName
                           
                       };
            Expression<Func<Wrapclean, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.companyid != 0)
            {
                where = where.And(m => m.companyid == model.companyid);
            }
            if (model.ugent != 0)
            {
                where = where.And(m => m.ugent == model.ugent);
            }
            if (model.status != 0)
            {
               
                if (model.status ==5)
                {
                    int[] arr = new int[] { 1, 2, 3 };
                    where = where.And(m => arr.Contains(m.status));
                }
                else
                {
                    where = where.And(m => m.status == model.status);
                }
            }
            
            if (model.content != null)
            {
                where = where.And(m => m.applyperson.Contains(model.content) || m.phone.Contains(model.content) || m.house.Contains(model.content) || m.executorstr.Contains(model.content));
            }
            if (model.cityname != null)
            {
                where = where.And(m => m.cityname == model.cityname);
            }
            if (model.arrCellNames != null)
            {
                where = where.And(m => model.arrCellNames.Contains(m.cellname));
            }
            if (model.areaname != null)
            {
                where = where.And(m => m.areaname == model.areaname);
            }
            data = data.Where(where);
            IOrderByExpression<Wrapclean> order = new OrderByExpression<Wrapclean, int>(p => p.ugent, false);
            IOrderByExpression<Wrapclean> order1 = new OrderByExpression<Wrapclean, long>(p => p.Id, true);
            List<Wrapclean> list = QueryableForList(data, orderablePagination, order, order1);
            return list;
        }
        //查询详情
        public Wrapclean Queryxq(clean model)
        {
            Wrapclean remodel = new Wrapclean();
            var data = from m in Bbclean
                       join n in BbUser on m.executor equals n.Id into temp
                       from t in temp.DefaultIfEmpty()
                       join x in t_v_HouseQuery on m.houseid equals x.Id into temp1
                       from t1 in temp1
                       select new Wrapclean()
                       {
                           Id = m.Id,
                           houseid = m.houseid,
                           status = m.status,
                           ugent = m.ugent,
                           apply = m.apply,
                           appointment = m.appointment,
                           applyperson = m.applyperson,
                           phone = m.phone,
                           project = m.project,
                           remark = m.remark,
                           executor = m.executor,
                           companyid = m.companyid,
                           house = t1.Name,
                           cityname = t1.CityName,
                           cellname = t1.CellName,
                           expectedtime=m.expectedtime,
                           executorstr=t.RealName==null?t.Mobile:t.RealName
                       };
            data = data.Where(p => p.Id == model.Id);
            remodel = data.FirstOrDefault();
            return remodel;
        }
        public List<wrapcleanRZ> QuerylistRz(cleanRZ model)
        {
            var data = from m in BbcleanRZ
                       join n in BbUser on m.opera equals n.Id into temp
                       from t in temp.DefaultIfEmpty()
                       where m.cleanid == model.cleanid
                       select new wrapcleanRZ() { Id = m.Id, createtime=m.createtime, cleanid=m.cleanid,
                           content=m.content,
                           operastr= t.RealName==null?t.Mobile:t.RealName
                       };
            data = data.OrderByDescending(p => p.createtime);
            return data.ToList();
        }
        public int  Save(clean model)
        {
            int result = 0;
            if (model.Id == 0)
            {
                model.status = 1;
                model.apply = DateTime.Now;
                model.Id = GetNextValNum("GET_WSEQUENCES('T_CLEANING')");
                result= AddModel(model);
            }
            else
            {
                result= ModifiedModel1<clean>(model,false,new string[] { "expectedtime", "status", "ugent", "executor" });
            }
            return result;
        }
        public int action(clean model,string[] field)
        {

            return ModifiedModel1<clean>(model, false, field); ;
        }
        public int Saverz(cleanRZ model)
        {
            if (model.Id == 0)
            {
                model.createtime = DateTime.Now;
                model.Id = GetNextValNum("GET_WSEQUENCES('T_CLEANINGRZ')");
                AddModel(model);
            }
            else
            {
                ModifiedModel<cleanRZ>(model, false);
            }
            return this.SaveChanges();
        }
        public int Delete(clean model)
        {
            return DeleteModel(model); ;
        }
        public DbSet<clean> Bbclean { get; set; }
        public DbSet<cleanRZ> BbcleanRZ { get; set; }
        public DbSet<T_SysUser> BbUser { get; set; }
        public DbSet<HouseQuery> t_v_HouseQuery { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new T_SysUserMapping());
            modelBuilder.Configurations.Add(new cleanRZMapping());
            modelBuilder.Configurations.Add(new cleanMapping());
            modelBuilder.Configurations.Add(new HouseQueryMapping());
        }
    }
}
