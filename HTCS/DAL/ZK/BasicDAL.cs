using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Mapping.cs;
using Model.Base;
using Model;
using ControllerHelper;
using System.Linq.Expressions;
using DBHelp;
using Model.House;

namespace DAL.ZK
{
    public class BaseDataDAL : RcsBaseDao
    {
        public List<T_Basics> Querylist1()
        {
             var   data = from a in BbBasic where (a.ParaType =="publictese"||a.ParaType== "privatetese") && a.IsActive == 1 select a;

            return data.ToList().Select(c=> new T_Basics() { Name = c.Name, Code = c.Code }).ToList();
        }
      
        public DbSet<T_Basics> BbBasic { get; set; }
        public DbSet<T_basicsType> basicsType { get; set; }
        public DbSet<T_V_basicc> t_v_basic { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BaseDataMaping());
            modelBuilder.Configurations.Add(new T_basticTypeMaping());
            modelBuilder.Configurations.Add(new T_v_basticTypeMaping());
        }
    }
}
