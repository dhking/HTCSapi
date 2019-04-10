using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model.Base;
using Mapping.cs;
using Model;

namespace DAL
{
    public class PeibeiTeseDal : RcsBaseDao
    {
      
        public List<T_Tese> QueryTese(long houseid, int type)
        {
            var data = from m in Tese where m.HouseId==houseid && m.Type == type select m;
            return data.ToList();
        }
        public List<peipei> QueryPeibei(long houseid,int type)
        {
            var data = from m in Peibei where m.HouseId == houseid&&m.Type==type select m;
            return data.ToList();
        }
        public DbSet<T_Tese> Tese { get; set; }
        public DbSet<peipei> Peibei { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new T_TeseMaping());
            modelBuilder.Configurations.Add(new T_peibeiMaping());
        }
    }
}
