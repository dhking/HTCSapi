using DAL.Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model.User;
using System.Data;
using System.Reflection;

using DBHelp;
using Model.House;
using Mapping.cs;
using System.Linq.Expressions;

namespace DAL
{
    public  class FormatterDAL: RcsBaseDao
    {
        public string Query(Formatter model)
        {
            string sql = "select "+model.Field+" from  "+model.Table +" where ID="+model.Value;
            string list = SqlQuery<string>(sql).FirstOrDefault();
            return list;
        }
        public T Queryxq<T>(Formatter model)
        {
            string sql = "select " + model.Field + " from  " + model.Table + " where ID=" + model.Value;
            T list = SqlQuery<T>(sql).FirstOrDefault();
            return list;
        }
        public List<WrapCell> QueryOne(WrapCell model)
        {
            List<T_CellName> listcity = getcity(model);
            List<WrapCell> delete = new List<WrapCell>();
            List<City> deletearea = new List<City>();
            List<WrapCell> listwrap = (from m in listcity where m.regtype == 1 select new WrapCell() { provinceCode = m.City, provinceName = m.Name, Id = m.Id }).ToList();
            foreach (var mo in listwrap)
            {
                mo.mallCityList = (from j in listcity where j.parentid == mo.Id select new City() { cityCode = j.Area, cityName = j.Name, Id = j.Id }).ToList();
                mo.mallCityList.Insert(0, new Model.House.City() { cityName = "全部", cityCode = 0 });
                if (mo.mallCityList.Count == 0)
                {
                    delete.Add(mo);
                }
                else
                {
                    foreach (var m in mo.mallCityList)
                    {
                        m.mallAreaList = (from j in listcity where j.parentid == m.Id && j.regtype == model.regtype select new Area() { areaCode = j.Id, areaName = j.Name }).ToList();
                        if (m.mallAreaList.Count == 0&& m.cityName!="全部")
                        {
                            deletearea.Add(m);
                        }
                        if(m.cityName!= "全部")
                        {
                            m.mallAreaList.Insert(0, new Model.House.Area() { areaName = "全部", areaCode = 0 });
                        }
                    }
                }

            }
            WrapCell model1 = new WrapCell();
            model1.provinceName = "全部";
            listwrap.Insert(0, model1);
            listwrap = distinctby(listwrap, delete, deletearea);
            return listwrap;
        }
        public List<T_CellName> getcity(WrapCell model)
        {
            var data = from m in Cell select m;
            Expression<Func<T_CellName, bool>> where = m =>1 == 1;
            //查询门店或者小区数据
            where = where.And(p => p.regtype == model.regtype);
            if (model.Type != 0)
            {
                where = where.And(p => p.Type == model.Type);
            }
            else
            {
                int[] arr= new int[] { 1, 2, 3 };
                where = where.And(p => arr.Contains(p.Type));
            }
            if (model.CompanyId != 0)
            {
                where = where.And(p => p.CompanyId == model.CompanyId);
            }
            data = data.Where(where);
            //查询市和区数据
            int[] arrs = new int[] { 1, 2 };
            if (model.Type == 4)
            {
                arrs = new int[] { 1,5 };
            }
            var data1 = from m in Cell where arrs.Contains(m.regtype) select m;
            data = data.Union(data1);
            List<T_CellName> listcell = data.ToList();
            return listcell;
        }
        //获取所有市数据
        public List<T_CellName> getarea(WrapCell model)
        {
            var data = from m in Cell select m;
            Expression<Func<T_CellName, bool>> where = m => m.regtype == 1;
            if (model.Type != 0)
            {
                where = where.And(p => p.Type == model.Type);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(p => p.CompanyId == model.CompanyId);
            }
            data = data.Where(where);
            List<T_CellName> listcell = data.ToList();
            return listcell;
        }
        //去重复数据
        public List<WrapCell> distinctby(List<WrapCell> list, List<WrapCell> delete, List<City> deletearea)
        {
            list = list.Except(delete).ToList();
            List<WrapCell> twodelete = new List<WrapCell>();
            foreach (var mo in list)
            {
                if (mo.provinceName == "全部")
                {
                    continue;
                }
                if (mo.mallCityList== null || mo.mallCityList.Count == 0)
                {
                    twodelete.Add(mo);
                    continue;
                }
                mo.mallCityList = mo.mallCityList.Except(deletearea).ToList();
                if (mo.mallCityList==null|| mo.mallCityList.Count == 0)
                {
                    twodelete.Add(mo);
                }
            }
            //二次去重
            list = list.Except(twodelete).ToList();
            return list;
        }
        public List<WrapCell> QueryOne1(WrapCell model)
        {
            List<T_CellName> listcity = getcity(model);
            List<WrapCell> delete = new List<WrapCell>();
            List<City> deletearea = new List<City>();
            List<WrapCell> listwrap = (from m in listcity where m.regtype==1 select new WrapCell() { provinceCode = m.City, provinceName = m.Name, Id = m.Id }).ToList();
            foreach (var mo in listwrap)
            {
                mo.mallCityList = (from j in listcity where j.parentid == mo.Id select new City() { cityCode = j.Area, cityName = j.Name,Id=j.Id }).ToList();
                if (mo.mallCityList.Count == 0)
                {
                    delete.Add(mo);
                }
                else
                {
                    foreach (var m in mo.mallCityList)
                    {
                        m.mallAreaList = (from j in listcity where j.parentid == m.Id && j.regtype == model.regtype select new Area() { areaCode = j.Id, areaName = j.Name }).ToList();
                        if (m.mallAreaList.Count == 0)
                        {
                            deletearea.Add(m);
                        }
                    }
                }
                
            }
            listwrap = distinctby(listwrap, delete, deletearea);
            return listwrap;
        }

        public List<WrapCell> QueryOne3(WrapCell model)
        {
            List<T_CellName> listcity = getcity(model);
            List<WrapCell> delete = new List<WrapCell>();
            List<City> deletearea = new List<City>();
            List<WrapCell> listwrap = (from m in listcity where m.regtype == 1 select new WrapCell() { provinceCode = m.City, provinceName = m.Name, Id = m.Id }).ToList();
            foreach (var mo in listwrap)
            {
                mo.mallCityList = (from j in listcity where j.parentid == mo.Id&&j.regtype==5 select new City() { cityCode = j.Area, cityName = j.Name, Id = j.Id }).ToList();
                if (mo.mallCityList.Count == 0)
                {
                    delete.Add(mo);
                }
                else
                {
                    foreach (var m in mo.mallCityList)
                    {
                        m.mallAreaList = (from j in listcity where j.parentid == m.Id && j.regtype == model.regtype select new Area() { areaCode = j.Id, areaName = j.Name }).ToList();
                        //if (m.mallAreaList.Count == 0)
                        //{
                        //    deletearea.Add(m);
                        //}
                    }
                }

            }
            listwrap = distinctby(listwrap, delete, deletearea);
            return listwrap;
        }
        public List<WrapCity> Querycity()
        {
            List<T_CellName> listcell = (from m in Cell select m).ToList();
            List<WrapCity> listwrap = (from m in listcell select new WrapCity() {cityCode = m.City, cityName = m.CityName }).GroupBy(r => r.cityCode).Select(r => r.First()).ToList();
            return listwrap;
        }
        public List<WrapCity> Querycity1(WrapCell model)
        {
            List<T_CellName> listcell = (from m in Cell where m.regtype==1&&m.CompanyId==model.CompanyId select m).ToList();
            List<WrapCity> listwrap = (from m in listcell select new WrapCity() { cityCode = m.City, cityName = m.CityName }).ToList();
            return listwrap;
        }
        public int QueryCount(T_CellName model)
        {
            var mo = from m in Cell where m.Name==model.Name&&m.AreaName == model.AreaName select m;
            return mo.Count();
        }
        public int Savecell(T_CellName model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_CELLNAME')");
                AddModel(model);
            }
            else
            {

                ModifiedModel<T_CellName>(model, false);
            }
            return this.SaveChanges();
        }
        //获取父级元素
        public List<T_CellName> getparent(long[] arr) 
        {
            var data = from m in Cell select m;
            Expression<Func<T_CellName, bool>> where = m => 1 == 1;
            if (arr.Length > 0)
            {
                where = where.And(p => arr.Contains(p.Id));
            }
            data = data.Where(where);
            return data.ToList();
        }
        public DbSet<T_CellName> Cell { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CellNameMapping());
        }
    }
}
