using DAL.Common;
using Mapping.cs;
using Model.Base;
using Model.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BanbenDAL : RcsBaseDao
    {
        public BanBen Query(BanBen model)
        {
            var mo = from m in dbValue where m.Type==2 select m;
            return mo.FirstOrDefault();
        }
        public DbSet<BanBen> dbValue { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BanbenMapping());
        }
    }
    public class feebackDAL : RcsBaseDao
    {

        //添加
        public int Save(feedback model)
        {
            int result = 0;
            if (model.id == 0)
            {
                model.createtime = DateTime.Now;
                model.id = GetNextValNum("GET_WSEQUENCES('T_FEEDBACK')");
                result = AddModel(model);
            }
            else
            {
                result = ModifiedModel<feedback>(model, false);
            }
            return result;
        }
        public DbSet<feedback> Bbfeedback { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new feedbackMapping());
        }
    }
}

