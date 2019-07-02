using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model.User;
using Mapping.cs;
using Model;
using System.Linq.Expressions;
using ControllerHelper;
using DBHelp;
using Model.Menu;
using Mapping.cs.Menu;
using Model.Bill;

namespace DAL
{
    public class UserDAL1 : RcsBaseDao
    {

        public int Querybyphone(string phone)
        {
            var mo = from m in BbUser where m.Mobile == phone select m;
            return mo.Count();
        }
        public T_SysUser Login(T_SysUser model)
        {
            T_SysUser user = new T_SysUser();
            var data = from n in BbUser where (n.Mobile == model.Mobile && n.Password == model.Password) select n;
            user = data.FirstOrDefault();


            return user;
        }

        public List<T_SysUserRole> listrole(long model)
        {
            var m = from mo in BbUserRole where mo.SysUserId == model select mo;

            return m.ToList();
        }
        public T_SysUser QueryUerbyid(T_SysUser model)
        {
            var data = (from n in BbUser select n).AsNoTracking();
            Expression<Func<T_SysUser, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.Mobile != null && model.Mobile != "")
            {
                where = where.And(m => m.Mobile == model.Mobile);
            }
            data = data.Where(where);
            return data.FirstOrDefault();
        }
        //app修改密码
        public int ModifyPassword(T_SysUser model)
        {
            return ModifiedModel<T_SysUser>(model, false);
        }
        public int adduser(T_SysUser model)
        {
            if (model.Id == 0)
            {
                
                model.Id = GetNextValNum("GET_WSEQUENCES('T_SysUser')");
                PlAddModel<T_SysUser>(model);
                if (model.listrole != null)
                {
                    foreach (var mo in model.listrole)
                    {
                        mo.Id = GetNextValNum("GET_WSEQUENCES('T_SysUserRole')");
                        mo.SysUserId = model.Id;
                        PlAddModel<T_SysUserRole>(mo);
                    }
                }
            }
            else
            {
                PLModifiedModel<T_SysUser>(model, false,new string[] {"CompanyId","area", "city" });
                if (model.listrole != null)
                {
                    foreach (var mo in model.listrole)
                    {
                        if (mo.Id == 0)
                        {
                            mo.Id = GetNextValNum("GET_WSEQUENCES('T_SysUserRole')");
                            mo.SysUserId = model.Id;
                            PlAddModel<T_SysUserRole>(mo);
                        }
                        else
                        {
                            PLModifiedModel<T_SysUserRole>(mo, false,new string[] { "CompanyId" });
                        }
                    }
                }
                //删除其他角色
                var data = from m in BbUserRole where m.SysUserId == model.Id select m;
                if (data.Count() > 0)
                {
                    foreach(var mo in data.ToList())
                    {
                        PLDeleteModel<T_SysUserRole>(mo);
                    }
                }
            }
            return this.SaveChanges();
        }
        public int updateuser(T_SysUser model, params string[] arr)
        {
            return ModifiedModel<T_SysUser>(model, false, arr);
        }
        public T_SysUser QueryUser(string model)
        {
            var result = from m in BbUser where m.Name == model select m;
            return result.FirstOrDefault();

        }
        public List<T_SysUser> QuerylistUser(T_SysUser model)
        {
            var result = from m in BbUser select m;
            Expression<Func<T_SysUser, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.RealName != null)
            {
                where = where.And(m => m.RealName.Contains(model.RealName));
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId== model.CompanyId);
            }
            result = result.Where(where);
            return result.ToList().Select(p => new T_SysUser { Name = p.Name,RealName =p.RealName==null?p.Mobile:p.RealName, Id = p.Id }).ToList();
        }
        public List<T_SysUser> Querylist(T_SysUser model, OrderablePagination orderablePagination)
        {
        
            var data = from a in BbUser select a;
            Expression<Func<T_SysUser, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.departs != null&&model.departs.Count>0)
            {
                List<long> ids = model.departs.Select(p => p.Id).ToList();
                if (!ids.Contains(-1))
                {
                    where = where.And(m => ids.Contains(m.storeid));
                }
            }
            if (model.Mobile != null)
            {
                where = where.And(m => m.Mobile.Contains(model.Mobile));
            }
            if (model.RealName !=null)
            {
                where = where.And(m => m.RealName.Contains(model.RealName));
            }
            if (model.CompanyId != 0&& model.CompanyId != 1)
            {
                where = where.And(m => m.CompanyId==model.CompanyId);
            }
            if (model.isquit!=2)
            {
                where = where.And(m => m.isquit == model.isquit);
            }
            data = data.Where(where);
            IOrderByExpression<T_SysUser> order = new OrderByExpression<T_SysUser, long>(p => p.Id, true);
            return this.QueryableForList<T_SysUser>(data, orderablePagination, order);
        }
        public List<T_SysRole> Querylistrole(T_SysRole model, OrderablePagination orderablePagination)
        {
          
            var data = from a in BbRole select a;
            Expression<Func<T_SysRole, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            data = data.Where(where);
        
            IOrderByExpression<T_SysRole> order = new OrderByExpression<T_SysRole, long>(p => p.Id, true);
            return this.QueryableForList<T_SysRole>(data, orderablePagination, order);
        }
        public List<T_Button> Querylistbutton(T_Button model, OrderablePagination orderablePagination)
        {
          
            var data = from a in BbButton select a;
            Expression<Func<T_Button, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.MenuId != 0)
            {
                where = where.And(m => m.MenuId == model.MenuId);
            }
            if (!string.IsNullOrEmpty( model.BtnName))
            {
                where = where.And(m => m.BtnName == model.BtnName);
            }
            data = data.Where(where);
           
            IOrderByExpression<T_Button> order = new OrderByExpression<T_Button, long>(p => p.Id, true);
            return this.QueryableForList<T_Button>(data, orderablePagination, order).Distinct().ToList();
        }
        public List<T_SysUserRole> Querylistuserrole(T_SysUserRole model, OrderablePagination orderablePagination)
        {
           
            var data = from a in BbUserRole select a;
            Expression<Func<T_SysUserRole, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.SysUserId != 0)
            {
                where = where.And(m => m.SysUserId == model.SysUserId);
            }
            if (model.SysRoleId != 0)
            {
                where = where.And(m => m.SysRoleId == model.SysRoleId);
            }
            data = data.Where(where);
          
            IOrderByExpression<T_SysUserRole> order = new OrderByExpression<T_SysUserRole, long>(p => p.Id, true);
            return this.QueryableForList<T_SysUserRole>(data, orderablePagination, order);
        }
        public List<WrapSysUserRole> Querynopageuserrole(T_SysUserRole model)
        {
            var data = from a in BbUserRole join b in BbRole on a.SysRoleId equals 
                      b.Id select new WrapSysUserRole() { Id = a.Id, SysRoleId = a.SysUserId, SysUserId = a.SysUserId,rolename=b.RoleName };
            Expression<Func<WrapSysUserRole, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.SysUserId != 0)
            {
                where = where.And(m => m.SysUserId == model.SysUserId);
            }
            data = data.Where(where);
            return data.ToList();
        }
        public int wrap(PlAction<T_SysUserRole, T_SysUser> model)
        {
            pladd(model.dataadd);
            pledit(model.update);
            pldelete(model.delete);

            return this.SaveChanges();
        }
        public long mainadd(T_SysUser model)
        {
            if (model == null)
            {
                return 0;
            }

            if (model.Id != 0)
            {
                return ModifiedModel<T_SysUser>(model, false);
            }
            else
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_SYSUSER')");
                if (AddModel<T_SysUser>(model) > 0)
                {
                    return model.Id;
                }
                else
                {
                    return 0;
                }
            }

        }
        public long buttonadd(T_Button model)
        {
            if (model == null)
            {
                return 0;
            }
            if (model.Id != 0)
            {
                return ModifiedModel<T_Button>(model, false);
            }
            else
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_SYSBUTTON')");
                if (AddModel<T_Button>(model) > 0)
                {
                    return model.Id;
                }
                else
                {
                    return 0;
                }
            }
        }
        //删除按钮
        public long delete(long id)
        {
            T_Button model = new T_Button() { Id = id };
            return DeleteModel<T_Button>(model);
        }
        public void pladd(List<T_SysUserRole> model)
        {
            if (model == null)
            {
                return;
            }
            foreach (var mo in model)
            {
                mo.Id = GetNextValNum("GET_WSEQUENCES('T_SYSUSERROLE')");
                PlAddModel<T_SysUserRole>(mo);
            }
        }
        public void pledit(List<T_SysUserRole> model)
        {
            if (model == null)
            {
                return;
            }
            foreach (var mo in model)
            {
                PLModifiedModel<T_SysUserRole>(mo, false);
            }
        }
        public void pldelete(List<T_SysUserRole> model)
        {
            if (model == null)
            {
                return;
            }
            foreach (var mo in model)
            {
                PLDeleteModel<T_SysUserRole>(mo);
            }
        }
        public List<T_SysRole> Querylistrolenopage(T_SysRole model, out int count)
        {
            count = 0;
            var data = from a in BbRole select a;
            Expression<Func<T_SysRole, bool>> where = m => 1 == 1;
            if (model != null && model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.CompanyId!=0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            data = data.Where(where);
            count = data.Count();
            return data.ToList();
        }
        public List<T_Shop> Querylistshopnopage(T_Shop model, out int count)
        {
            count = 0;
            var data = from a in BbShop select a;
            Expression<Func<T_Shop, bool>> where = m => 1 == 1;
            if (model != null && model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            data = data.Where(where);
            count = data.Count();
            return data.ToList();
        }
        public bool Pladdrolemenu(List<T_SysRoleMenu> model, long RoleId)
        {
            foreach (var mo in model)
            {
                var data = from n in RoleMenu where n.ButtonId == mo.ButtonId && n.MenuId == mo.MenuId && n.RoleId == mo.RoleId select n;
                var count = data.Count();
                if (count == 0)
                {
                    mo.Id = GetNextValNum("GET_WSEQUENCES('T_SYSROLEMENU')");
                    mo.RoleId = RoleId;
                    AddModel<T_SysRoleMenu>(mo);
                }
                else
                {
                    mo.Id = data.AsNoTracking().FirstOrDefault().Id;
                    ModifiedModel<T_SysRoleMenu>(mo, false);
                }
            }
            return true;
        }
        public int Saverecord(T_Record model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_RECORD')");
                return AddModel(model);
            }
            else
            {
                return ModifiedModel(model, false, model.NotUpdatefield);
            }

        }
        public bool deleterolemenu(List<T_SysRoleMenu> model, long RoleId)
        {
            foreach (var mo in model)
            {
                var data = from n in RoleMenu where n.ButtonId == mo.ButtonId && n.MenuId == mo.MenuId && n.RoleId == RoleId select n;
                T_SysRoleMenu rolemenu = new T_SysRoleMenu();
                rolemenu = data.FirstOrDefault();
                if (data.Count() > 0)
                {
                    int result = DeleteModel<T_SysRoleMenu>(rolemenu);
                }
            }
            return true;
        }
        //修改个人信息个别字段
        public long saveUser(T_SysUser model, params string[] notModiFields)
        {
            if (model == null)
            {
                return 0;
            }
            if (model.Id != 0)
            {
                return ModifiedModel1<T_SysUser>(model, false, notModiFields);
            }
            else
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_SYSUSER')");
                if (AddModel<T_SysUser>(model) > 0)
                {
                    return model.Id;
                }
                else
                {
                    return 0;
                }
            }
        }
        public long savecertifica(T_CertIfication model, params string[] notModiFields)
        {
            if (model == null)
            {
                return 0;
            }
            if (model.Id != 0)
            {
                return ModifiedModel1<T_CertIfication>(model, false, notModiFields);
            }
            else
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('CERTIFICATION')");
                if (AddModel<T_CertIfication>(model) > 0)
                {
                    return model.Id;
                }
                else
                {
                    return 0;
                }
            }
        }
        public List<T_CertIfication> Querycertification(long userid)
        {
            var data = (from m in certification where m.UserId == userid && m.type == 1 select m).Take(1);
            var data1 = (from m in certification where m.UserId == userid && m.type == 2 select m).Take(1);
            var data2 = data.Union(data1);
            return data2.ToList();
        }
        public DbSet<T_SysUser> BbUser { get; set; }
        public DbSet<T_SysRoleMenu> RoleMenu { get; set; }
        public DbSet<T_SysRole> BbRole { get; set; }
        public DbSet<T_SysUserRole> BbUserRole { get; set; }
        public DbSet<T_Button> BbButton { get; set; }
        public DbSet<T_Shop> BbShop { get; set; }
        public DbSet<T_CertIfication> certification { get; set; }
        public DbSet<T_Record> bbRecord { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new T_SysUserMapping());
            modelBuilder.Configurations.Add(new SysUserRoleMapping());
            modelBuilder.Configurations.Add(new RoleMenuMapping());
            modelBuilder.Configurations.Add(new SysRoleMapping());
            modelBuilder.Configurations.Add(new ButtonMapping());
            modelBuilder.Configurations.Add(new ShopMapping());
            modelBuilder.Configurations.Add(new CertIficationMapping());
            modelBuilder.Configurations.Add(new T_RecordMapping());
        }
    }
}
