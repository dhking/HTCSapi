using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Mapping.cs.Menu;
using Model;
using Model.House;
using Model.Menu;
using Model.User;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class RoleDAL : RcsBaseDao
    {
        public List<T_Button> Query(T_SysUserRole model)
        {
            var mo = from m in BbButton
                     join n in RoleMenu on m.Id equals n.ButtonId
                     join t in BbUserRole on n.RoleId equals t.SysRoleId
                     where t.SysUserId == model.SysUserId
                     select m;
            Expression<Func<T_Button, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.MenuId == model.Id);
            }
            if (model.btnjishu != 999)
            {
                where = where.And(m => m.btnjishu == model.btnjishu);
            }
            mo = mo.Where(where);
            mo = mo.OrderByDescending(p => p.orderby);
            return mo.DistinctBy(p => p.Id).ToList();
        }
        public List<T_CellName> storeQuery(T_CellName model, OrderablePagination orderablePagination, int[] arr)
        {
        
            var data = from m in Bbstore
                     select m;
            Expression<Func<T_CellName, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.regtype != 0)
            {
                where = where.And(m => m.regtype == model.regtype);
            }
            if (arr != null&& arr.Length>0)
            {
                where = where.And(m => arr.Contains(m.regtype));
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            data = data.Where(where);
            IOrderByExpression<T_CellName> order = new OrderByExpression<T_CellName, long>(p => p.Id, true);
            return this.QueryableForList<T_CellName>(data, orderablePagination, order);
        }
        public T_CellName storeQueryid(T_CellName model)
        {
            var data = from m in Bbstore
                       select m;
            Expression<Func<T_CellName, bool>> where = m => 1 == 1;
            if (model.regtype != 0)
            {
                where = where.And(m => m.regtype == model.regtype);
            }
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.Name !=null)
            {
                where = where.And(m => m.Name == model.Name);
            }
            data = data.Where(where);
            IOrderByExpression<T_CellName> order = new OrderByExpression<T_CellName, long>(p => p.Id, true);
            return data.FirstOrDefault();
        }

        public List<T_CellName> storelistquery(List<long> parentids)
        {
            var data = from m in Bbstore where parentids.Contains(m.parentid)
                       select m;
            Expression<Func<T_CellName, bool>> where = m => 1 == 1;
           
            data = data.Where(where);
            IOrderByExpression<T_CellName> order = new OrderByExpression<T_CellName, long>(p => p.Id, true);
            return data.ToList();
        }
        public List<T_CellName> storelistquery1(List<long> parentids)
        {
            var data = from m in Bbstore
                       where parentids.Contains(m.Id)
                       select m;
            Expression<Func<T_CellName, bool>> where = m => 1 == 1;
            data = data.Where(where);
            IOrderByExpression<T_CellName> order = new OrderByExpression<T_CellName, long>(p => p.Id, true);
            return data.ToList();
        }
        public T_SysRole QueryUerbyid(T_SysRole model)
        {
            var data = from n in BbRole select n;
            Expression<Func<T_SysRole, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            data = data.Where(where);
            return data.FirstOrDefault();
        }
        public int save(T_SysRole bill)
        {
            if (bill.Id == 0)
            {
                bill.Id = GetNextValNum("GET_WSEQUENCES('T_SYSROLE')");
                return AddModel<T_SysRole>(bill);
               
            }
            else
            {
                return ModifiedModel<T_SysRole>(bill, false);
            }
            
        }
        public long storesave(T_CellName bill)
        {
            if (bill.Id == 0)
            {
                bill.Id = GetNextValNum("GET_WSEQUENCES('T_CELLNAME')");
                AddModel<T_CellName>(bill);
                return bill.Id;

            }
            else
            {
                return ModifiedModel<T_CellName>(bill, false);
            }

        }
        //根据id查询角色名称
        public T_SysRole queryrole(List<long> rolesid)
        {
            var data = from m in BbRole where rolesid.Contains(m.Id) select m;
            
            return data.FirstOrDefault();
        }
        //删除通用
        public void delete(iids model)
        {
            string sql = "delete  from " + model.Table + " where ID in (" + model.ids + ")";
            int list = ExecuteSqlCommand(sql);

        }
        public DbSet<T_SysRoleMenu> RoleMenu { get; set; }
        public DbSet<T_Button> BbButton { get; set; }
        public DbSet<T_SysUser> BbUser { get; set; }
        public DbSet<T_SysRole> BbRole { get; set; }
        public DbSet<T_SysUserRole> BbUserRole { get; set; }
        public DbSet<T_CellName> Bbstore { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SysUserRoleMapping());
            modelBuilder.Configurations.Add(new RoleMenuMapping());
            modelBuilder.Configurations.Add(new SysRoleMapping());
            modelBuilder.Configurations.Add(new ButtonMapping());
            modelBuilder.Configurations.Add(new T_SysUserMapping());
            modelBuilder.Configurations.Add(new CellNameMapping());
        }
    }
}
