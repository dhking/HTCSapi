using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model.Menu;
using Mapping.cs.Menu;
using Model.User;
using Mapping.cs;
using System.Linq.Expressions;
using DBHelp;
using ControllerHelper;

namespace DAL
{
    public class MenuDAL : RcsBaseDao
    {

        public List<T_Menu> Query()
        {
            var data = from m in Menu select m;
           
            return data.ToList();
        }
        public List<T_Menu> Querylist(T_SysUser user,int[] sign)
        {
            var data = from m in Menu join n in BbRoleMenu on m.Id equals n.MenuId join  x in BbUserRole on n.RoleId equals x.SysRoleId where x.SysUserId== user.Id
                       orderby m.@orderby descending 
                       select m;
            List<T_Menu> listmenu = new List<T_Menu>();
            listmenu = data.DistinctBy(p => p.Id).ToList();
            List<T_Menu> menu1 = listmenu.Where(p => p.Jishu == 0 && sign.Contains(p.sign)).ToList();
            foreach (var model in menu1)
            {
                model.list = listmenu.Where(p => p.SystemId == model.Id && p.Jishu == 1).ToList();
                foreach (var mo in model.list)
                {
                    mo.list = listmenu.Where(p => p.ParentId == mo.Id && p.Jishu == 2).ToList();
                }
            }
            menu1 = menu1.Distinct().ToList();
            return menu1;
        }
        //列表
        public List<T_Menu> Querylist1(T_SysUser user, int sign)
        {
            var data = from m in Menu
                       join n in BbRoleMenu on m.Id equals n.MenuId
                       join x in BbUserRole on n.RoleId equals x.SysRoleId
                       where x.SysUserId == user.Id
                       orderby m.@orderby descending
                       select m;
            List<T_Menu> listmenu = new List<T_Menu>();
            listmenu = data.ToList();
            List<T_Menu> menu1 = listmenu.Where(p => p.Jishu == 1 && p.sign == sign).ToList();
            menu1 = menu1.Distinct().ToList();
            return menu1;
        }

        public List<Pression> Querylistpression(long RoleId,int[] arrgisn)
        {
            //查询所有的菜单按钮角色表
            var data = from m in Menu  join x in (from j in  BbRoleMenu where j.RoleId==RoleId && j.ButtonId== 0 select j)
                       on m.Id equals x.MenuId 
                       into temp from t in temp.DefaultIfEmpty()
                       select new Pression()
                       {
                           MenuId=m.Id, name=m.title ,
                Jishu =m.Jishu,SystemId=m.SystemId
                ,ParentId=m.ParentId,RoleId=t.RoleId==null?0:t.RoleId,sign=m.sign};
            List<Pression> listmenu = new List<Pression>();
            Expression<Func<Pression, bool>> where = m => 1 == 1;
            if (arrgisn != null)
            {
                where = where.And(m => arrgisn.Contains(m.sign));
            }
            data = data.Where(where);
            listmenu = data.Distinct().ToList();
            //查询第一级数据
            List<Pression> menu1 = listmenu.Where(p => p.Jishu == 0).OrderBy(p=>p.MenuId).ToList();
            foreach (var model in menu1)
            {
                if (model.RoleId == RoleId)
                {
                    model.checkedd = true;
                }
                //查询第二级数据
              model.children= listmenu.Where(p => p.SystemId == model.MenuId && p.Jishu == 1)
                    .Select(p=>new Pression { MenuId=p.MenuId,checkedd=p.RoleId==RoleId?true:false,name=p.name,RoleId=p.RoleId }).OrderBy(p=>p.MenuId)
                    .ToList();
               
                if (model.children.Count == 0)
                {
                    //如果第二级没有子集，说明是页面则直接查询相关的按钮数据
                    model.children = (from n in Btn join x in (from j in BbRoleMenu where j.RoleId == RoleId select j) on n.Id equals x.ButtonId into temp from t in temp.DefaultIfEmpty() where n.MenuId == model.MenuId select new Pression() {RoleId=t.RoleId==null?0: t.RoleId, ButtonId=n.Id, name = n.BtnName,checkedd=t.RoleId==0||t.RoleId == null? false:true,MenuId=n.MenuId }).OrderBy(p=>p.MenuId).Distinct().ToList();
               
                }
                else
                {
                   
                    foreach (var mo in model.children)
                    {
                        //查询第三级的数据并且查询第三级的按钮数据，如果roleid不等于说明选中了
                        
                        var btndata = from n in Btn join x in (from j in BbRoleMenu where j.RoleId == RoleId select j) on n.Id equals x.ButtonId into temp from t in temp.DefaultIfEmpty() where n.MenuId == mo.MenuId select new Pression() { RoleId = t.RoleId == null ? 0 : t.RoleId, ButtonId = n.Id, name = n.BtnName, checkedd = t.RoleId == 0 || t.RoleId == null ? false : true };
                       mo.children= btndata.OrderBy(p=>p.ButtonId).Distinct().ToList();
                     
                    }
                }
            }
            
            return menu1;
        }
        //查询多个角色的权限信息
        public List<Pression> Querylistpressionbyuser(List<long> RoleId)
        {
            //查询所有的菜单按钮角色表
            var data = from m in Menu
                       join x in (from j in BbRoleMenu where RoleId.Contains(j.RoleId) && j.ButtonId == 0 select j)
       on m.Id equals x.MenuId
       into temp
                       from t in temp.DefaultIfEmpty()
                       select new Pression()
                       {
                           MenuId = m.Id,
                           name = m.title,
                           Jishu = m.Jishu,
                           SystemId = m.SystemId
                           ,ParentId = m.ParentId,
                           RoleId = t.RoleId == null ? 0 : t.RoleId,
                           Code=m.name,
                           appcheck=1,
                          
                       };
            List<Pression> listmenu = new List<Pression>();
            listmenu = data.DistinctBy(p=>p.MenuId).ToList();
            //查询第一级数据
            List<Pression> menu1 = listmenu.Where(p => p.Jishu == 0).OrderBy(p => p.MenuId).ToList();
            List<Pression> listchild = new List<Pression>();
            foreach (var model in menu1)
            {
                if (RoleId.Contains(model.RoleId) )
                {
                    model.appcheck = 1;
                }
                //查询第二级数据
                List<Pression> childpression =listmenu.Where(p => p.SystemId == model.MenuId && p.Jishu == 1)
                    .Select(p => new Pression { MenuId = p.MenuId, appcheck = RoleId.Contains(p.RoleId) ? 1 : 0, name = p.name, RoleId = p.RoleId,Code=p.name }).OrderBy(p => p.MenuId)
                    .ToList();
                if (childpression.Count == 0)
                {
                    //如果第二级没有子集，说明是页面则直接查询相关的按钮数据
                    childpression = (from n in Btn join x in (from j in BbRoleMenu where RoleId.Contains(j.RoleId) select j) on n.Id equals x.ButtonId into temp from t in temp where n.MenuId == model.MenuId select new Pression() { appcheck = t.RoleId == 0 ? 0 : 1, RoleId = t.RoleId == null ? 0 : t.RoleId, ButtonId = n.Id, name = n.BtnName, checkedd = t.RoleId == 0 || t.RoleId == null ? false : true, MenuId = n.MenuId,Code=n.BtnNo }).OrderBy(p => p.MenuId).Distinct().ToList();
                    listchild.AddRange(childpression);
                }
                else
                {
                    foreach (var mo in childpression)
                    {
                        //查询第三级的数据并且查询第三级的按钮数据，如果roleid不等于说明选中了
                        var btndata = from n in Btn join x in (from j in BbRoleMenu where RoleId.Contains(j.RoleId) select j) on n.Id equals x.ButtonId into temp from t in temp where n.MenuId == mo.MenuId select new Pression() { RoleId = t.RoleId == null ? 0 : t.RoleId, ButtonId = n.Id, name = n.BtnName, appcheck = t.RoleId == 0 ? 0 : 1,Code=n.BtnNo };
                        List<Pression> child2pression = btndata.OrderBy(p => p.ButtonId).Distinct().ToList();
                        listchild.AddRange(child2pression);
                    }
                }
            }
            listmenu.AddRange(listchild);
            return listmenu.Distinct().ToList();
        }
        public int SaveMenu(T_Menu model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_MENU')");
                return AddModel(model);
            }
            else
            {
                return ModifiedModel(model, false);
            }
           
        }
        public long deletedata(long model)
        {
            T_Menu basc = new T_Menu();
            basc.Id = model;
            DeleteModel(basc);
            return basc.Id;
        }
        public T_Menu QueryId(T_Menu model)
        {
            var data = from a in Menu where a.Id == model.Id select a;
            return data.FirstOrDefault();
        }
        //查询部门
        public List<wrapdepartment> Querydepartment(wrapdepartment model)
        {
            var data = from m in Bbdepartment
                       select new wrapdepartment()
                       {
                           Id = m.Id,
                           name = m.name,
                           parentid = m.parentid,
                           phone = m.phone,
                           adress = m.adress,
                           stroe = m.stroe,
                           companyid=m.companyid,
                           spread=true,
                           open=true,
                           checkedd=true
                       };
            List<wrapdepartment> listdepartment = new List<wrapdepartment>();
            Expression<Func<wrapdepartment, bool>> where = m => m.companyid == model.companyid;
           
            data = data.Where(where);
            listdepartment = data.ToList();
            List<wrapdepartment> returndepart = new List<wrapdepartment>();
            //一级数据
            List<wrapdepartment> partment1 = listdepartment.Where(p => p.parentid == -1).ToList();
            foreach(var mo in partment1)
            {
                mo.children = department(mo, listdepartment);
            }
            //加上默认总部层级
            wrapdepartment wrappart = new wrapdepartment();
            wrappart.name = "总部";
            wrappart.Id = -1;
            wrappart.spread = true;
            wrappart.open = true;
            wrappart.checkedd = true;
            wrappart.children = partment1;
            returndepart.Add(wrappart);
            return returndepart;
        }

        //查询部门分页
        public List<t_department> Querypage(wrapdepartment model)
        {
            var data = from m in Bbdepartment select m;
            Expression<Func<t_department, bool>> where = m => m.companyid == model.companyid; ;
            data = data.Where(where);
            List<t_department> list = data.ToList();
            List<t_department> returndepart = new List<t_department>() ;
            returndepart.Add(new t_department() { Id = -1, name = "总部" });
            //一级数据
            List<t_department> partment1 =new List<t_department> ();
            partment1 = list.Where(p => p.parentid == model.Id).ToList();
            returndepart = returndepart.Union(list.Where(p => p.parentid == model.Id).ToList()).ToList();
            foreach (var mo in partment1)
            {
                returndepart=department1(mo, list, returndepart);
            }
            return returndepart;
        }
        //递归查询
        public List<wrapdepartment> department(wrapdepartment model, List<wrapdepartment> listdepartment)
        {
            List<wrapdepartment> re = listdepartment.Where(p => p.parentid == model.Id).ToList();
            if (re != null)
            {
                foreach (var mo in re)
                {
                    mo.children = department(mo, listdepartment);
                }
            }
            return re;
        }
        //递归查询1
        public List<t_department> department1(t_department model, List<t_department> listdepartment, List<t_department> returnlist)
        {
            
            List<t_department> re = listdepartment.Where(p => p.parentid == model.Id).ToList();
            if (re != null)
            {
                returnlist.AddRange(re);
                foreach (var mo in re)
                {
                    department1(mo, listdepartment, returnlist);
                }
            }
            return returnlist;
        }
        public int Savedepartment(t_department model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_DEPARTMENT')");
                return AddModel(model);
            }
            else
            {
                return ModifiedModel(model, false);
            }

        }
        //查询部门详情
        public t_department queryid(t_department model)
        {
            var data = from m in Bbdepartment select m;
            Expression<Func<t_department, bool>> where = m =>1== 1;
            if (model.Id != 0)
            {
                where = where.And(p => p.Id == model.Id);
            }
            data = data.Where(where);
            return data.FirstOrDefault();
        }
        public DbSet<T_Menu> Menu { get; set; }
        public DbSet<T_Button> Btn { get; set; }
        public DbSet<T_SysRoleMenu> BbRoleMenu { get; set; }
        public DbSet<T_SysUserRole> BbUserRole { get; set; }

        public DbSet<t_department> Bbdepartment { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MenuMapping());
            modelBuilder.Configurations.Add(new ButtonMapping());
            modelBuilder.Configurations.Add(new RoleMenuMapping());
            modelBuilder.Configurations.Add(new SysUserRoleMapping());
            modelBuilder.Configurations.Add(new departmentMapping());
        }
    }
}
