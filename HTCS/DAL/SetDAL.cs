using DAL.Common;
using DBHelp;
using Mapping.cs;
using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public   class SetDAL : RcsBaseDao
    {

        public indepent Query(indepent model)
        {
            var mo = from m in Bbindepent
                     select m;
            Expression<Func<indepent, bool>> where = m => 1 == 1;
            if (!string.IsNullOrEmpty(model.name))
            {
                where = where.And(m => m.name == model.name);
            }
            mo = mo.Where(where);
            return mo.FirstOrDefault();
        }
        public DbSet<indepent> Bbindepent { get; set; }
      
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new indepentMapping());
        }
    }
}
