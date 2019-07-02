using DAL.Common;
using Mapping.cs;
using Model.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CityDAL : RcsBaseDao
    {
        public List<WrapCity> Querycity(City model)
        {
            ZmHelp zm = new Common.ZmHelp();
            List<City> list = new List<City>();
            var data = from m in Bbcity where m.RegType == 3 select m;
            list = data.ToList();
            List<WrapCity> listwrap = new List<WrapCity>();
            
            foreach (var mo in list)
            {
                if (mo.IsRemen == 1)
                {
                    mo.szm = "#";
                    continue;
                }
                var szm = ZmHelp.GetSpellCode(mo.RegionName);
                mo.szm = szm;
            }
          
            foreach (IGrouping<string, City> group in list.GroupBy(c => c.szm))
            {
               
                    WrapCity wrap = new WrapCity();
                    wrap.Name = group.Key;
                    wrap.city = group.ToList();
                    listwrap.Add(wrap);
               
            }
            listwrap= listwrap.OrderBy(c => c.Name).ToList();
            return listwrap;
        }
        public List<City> Querycity1(City model)
        {
            ZmHelp zm = new Common.ZmHelp();
            List<City> list = new List<City>();
            var data = from m in Bbcity where m.RegType == 3 select m;
            list = data.ToList();
            list = list.OrderByDescending(c => c.IsRemen).ToList();
            return list;
        }
        //永昌
        //public List<City> Querycity1(City model)
        //{
        //    ZmHelp zm = new Common.ZmHelp();
        //    List<City> list = new List<City>();
        //    list.Add(new City() { RegionName = "南京市" });
        //    list.Add(new City() { RegionName = "北京市" });
        //    return list;
        //}

        public DbSet<City> Bbcity { get; set; }
    protected override void CreateModelMap(DbModelBuilder modelBuilder)
    {
        modelBuilder.Configurations.Add(new CityMapping());
    }
}
}

