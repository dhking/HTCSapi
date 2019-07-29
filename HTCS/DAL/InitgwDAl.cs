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
    public  class queryupperDAl : RcsBaseDao
    {

        public List<houresourcesupper> Query(houresourcesupper model)
        {
            var mo = from m in Bbhouresourcesupper
                     select m;
            Expression<Func<houresourcesupper, bool>> where = m => 1 == 1;
            if (model.gwisupper==0)
            {
                where = where.And(m => m.gwisupper == model.gwisupper);
            }
            mo = mo.Where(where);
            return mo.ToList();
        }
        public int Save(houresourcesupper model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_HOURESOURCESUPPER')");
                return AddModel(model);
            }
            else
            {
                return ModifiedModel(model, false);
            }

        }
        public DbSet<houresourcesupper> Bbhouresourcesupper { get; set; }

        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new houresourcesupperMapping());
        }
    }
}
