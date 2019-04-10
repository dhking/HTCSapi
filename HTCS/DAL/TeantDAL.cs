using DAL.Common;
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
    public class TrantDAL : RcsBaseDao
    {
        public T_Teant Query(string phone)
        {
            var mo = from m in Teant where m.Phone == phone select m;
            return mo.FirstOrDefault();
        }
        public DbSet<T_Teant> Teant { get; set; }
      
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TenantMapping());
        }
    }
}
