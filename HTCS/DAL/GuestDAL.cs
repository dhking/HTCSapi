using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Model;
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
    public  class GuestDAL : RcsBaseDao
    {
       //查询
        public List<WrapGuest> Querylist(Guest model,OrderablePagination orderablePagination)
        {
           
            var data = from m in BbGuest join n in BbUser on m.UserId equals n.Id into temp
                       from t in temp.DefaultIfEmpty()
                       select new WrapGuest() {Id=m.Id, Phone=m.Phone,Sex=m.Sex,Source=m.Source, IntoTime=m.IntoTime, Remark=m.Remark, Ugent= m.Ugent, UserName = t.RealName,House=m.House,Name=m.Name,Status=m.Status,Huxing=m.Huxing,Other=m.Other,MaxPrice=m.MaxPrice,MinPrice=m.MinPrice,RectDate=m.RectDate,CreatePerson=m.CreatePerson,CreateTime=m.CreateTime,CompanyId=m.CompanyId };
            Expression<Func<WrapGuest, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.Status != 0)
            {
                where = where.And(m => m.Status == model.Status);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }

            if (model.Content != null)
            {
                where = where.And(m => m.Phone.Contains(model.Content) || m.Name.Contains(model.Content) || m.House.Contains(model.Content)||m.HopeAdress.Contains(model.Content));
            }
            if (model.BeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.CreateTime >= model.BeginTime && m.CreateTime <= model.EndTime);
            }
            if (model.Ugent != 0)
            {
                where = where.And(m => m.Ugent == model.Ugent);
            }
            if (!string.IsNullOrEmpty(model.Phone))
            {
                where = where.And(m => m.Phone == model.Phone);
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                where = where.And(m => m.Name == model.Name);
            }
           
            data = data.Where(where);
            IOrderByExpression<WrapGuest> order = new OrderByExpression<WrapGuest, int>(p => p.Ugent, false);
            IOrderByExpression<WrapGuest> order1 = new OrderByExpression<WrapGuest, long>(p => p.Id, true);
           
            List<WrapGuest> list = QueryableForList(data, orderablePagination, order, order1);
            foreach(var mo in list)
            {
                var data1 = (from m in BbRzGuest  where m.GuestId == mo.Id select m);
                mo.guestrz = data1.OrderByDescending(a => a.Id).FirstOrDefault();
              
            }
            return list;
        }
        //查询详情
        public WrapGuest Queryxq(Guest model)
        {
            WrapGuest remodel = new WrapGuest();
            var data = from m in BbGuest
                       join n in BbUser on m.UserId equals n.Id into temp
                       from t in temp.DefaultIfEmpty()
                       where m.Id==model.Id
                       select new WrapGuest() { Id = m.Id, Phone = m.Phone, Sex = m.Sex, Source = m.Source, IntoTime = m.IntoTime, Remark = m.Remark, Ugent = m.Ugent, UserName = t.Name, House = m.House, Name = m.Name, Status = m.Status, RectDate=m.RectDate,Huxing=m.Huxing,Other=m.Other,HopeAdress=m.HopeAdress,MaxPrice=m.MaxPrice,MinPrice=m.MinPrice,CreatePerson=m.CreatePerson};
            remodel = data.FirstOrDefault();
            remodel.Ipimgadess = "http://106.14.96.37:82/";
            if (remodel != null)
            {
                var data1 = (from m in BbRzGuest where m.GuestId == remodel.Id select m);
                remodel.guestrzlist =data1.OrderByDescending(a => a.Id).ToList();   
            }
            return remodel;
        }
        public List<GuestRz> QuerylistRz(GuestRz model)
        {
            var data = from m in BbRzGuest orderby m.CreateTime descending where m.GuestId==model.GuestId select m;
            return data.ToList();
        }
        public int  Save(Guest model)
        {
            int result = 0;
            if (model.Id == 0)
            {
                model.Status = 1;
                model.CreateTime = DateTime.Now;
                model.Id = GetNextValNum("GET_WSEQUENCES('T_GUEST')");
                result= AddModel(model);
            }
            else
            {
                result= ModifiedModel<Guest>(model,false,new string[] { "CreateTime", "CreatePerson" });
            }
            return result;
        }
        public int action(Guest model,string[] field)
        {

            return ModifiedModel1<Guest>(model, false, field); ;
        }
        public int Saverz(GuestRz model)
        {
         
            if (model.Id == 0)
            {
               
                model.CreateTime = DateTime.Now;
                model.Id = GetNextValNum("GET_WSEQUENCES('T_GUESTRZ')");
                AddModel(model);
            }
            else
            {
                ModifiedModel<GuestRz>(model, false);
            }
            return this.SaveChanges();
        }
        public int Delete(Guest model)
        {
            return DeleteModel(model); ;
        }
        public DbSet<Guest> BbGuest { get; set; }
        public DbSet<GuestRz> BbRzGuest { get; set; }
        public DbSet<T_SysUser> BbUser { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new T_SysUserMapping());
            modelBuilder.Configurations.Add(new GuestMapping());
            modelBuilder.Configurations.Add(new GuestRZMapping());
        }
    }
}
