using DAL.Common;
using DBHelp;
using Mapping.cs;
using Mapping.cs.Bill;
using Model;
using Model.Bill;
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
    public   class AccountNameDAL : RcsBaseDao
    {
        //账户列表
        public List<accountbank> Accountlist(accountbank model)
        {
            var mo = from m in baccountbank where m.CompanyId == model.CompanyId select m;
            return mo.ToList();
        }
        //查询账户详情
        public accountbank queryAccount(accountbank model)
        {
            var mo = from m in baccountbank  select m;
            Expression<Func<accountbank, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            mo = mo.Where(where);
            return mo.FirstOrDefault();
        }
        //修改账号
        public long saveaccount(accountbank model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_ACCOUNTBANK')");
                AddModel<accountbank>(model);
            }
            else
            {
                ModifiedModel<accountbank>(model, false, new string[] {  });
            }
            return model.Id;
        }
        public int DeleteHouse(accountbank model)
        {
            int count = (from m in baccountbank where m.Id == model.Id select m).Count();
            if (count == 0)
            {
                return 1;
            }
            else
            {
                return DeleteModel(model);
            }
        }
        public DbSet<accountbank> baccountbank { get; set; }

        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new accountbankMapping());
          
        }
    }
}
