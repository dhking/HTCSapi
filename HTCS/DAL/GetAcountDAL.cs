using DAL.Common;
using Mapping.cs;
using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class GetAcountDAL : RcsBaseDao
    {
        public T_PayMentAcount GetAcount(int type)
        {
            var mo = from m in PayMent where m.Type == type select m;
             
            return mo.FirstOrDefault();
        }
        public DbSet<T_PayMentAcount> PayMent { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new zfbMapping());
            
        }
    }
}
