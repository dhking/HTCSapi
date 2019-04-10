using DAL.Common;
using DBHelp;
using Mapping.cs;
using Mapping.cs.Contrct;
using Model;
using Model.Bill;
using Model.Contrct;
using Model.House;
using Model.TENANT;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace DAL
{
    public class MessageDAL : RcsBaseDao
    {
        public List<T_MessageQueue> Query(T_MessageQueue model)
        {
            var mo = from m in MessageQueue  select m;
            Expression<Func<T_MessageQueue, bool>> where = m => 1 == 1;
            where = where.And(m => m.status == model.status);
            if (model.type != 0)
            {
                where = where.And(m => m.type == model.type);
            }
            if (model.contractid != 0)
            {
                where = where.And(m => m.contractid == model.contractid);
            }
            mo = mo.Where(where);
            return mo.ToList();
        }
        public int save(T_MessageQueue bill)
        {
            if (bill.Id == 0)
            {
                bill.Id = GetNextValNum("GET_WSEQUENCES('T_MESSAGEQUEUE')");
                return AddModel<T_MessageQueue>(bill);

            }
            else
            {
                return ModifiedModel<T_MessageQueue>(bill, false);
            }

        }

        public DbSet<T_MessageQueue> MessageQueue { get; set; }

        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MessageQueueMapping());
        }
    }
  
}
