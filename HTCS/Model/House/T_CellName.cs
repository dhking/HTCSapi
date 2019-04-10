using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.House
{
    public class T_CellName : BasicModel
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public long City { get; set; }
        public long Area { get; set; }
        public string Adress { get; set; }
        public string CityName { get; set; }
        public string AreaName { get; set; }
        public long CompanyId { get; set; }
        public int regtype { get; set; }
        public long parentid { get; set; }
        public string code { get; set; }
    }
    public class WrapCell
    {
        public long CompanyId { get; set; }
        public long Id { get; set; }
        public int Type { get; set; }
        public int regtype { get; set; }
        public long provinceCode { get; set; }
        public string provinceName { get; set; }
        public List<City>  mallCityList{get;set;}
    }
    public class WrapCity
    {
        public long cityCode { get; set; }
        public string cityName { get; set; }
     
    }
    public class City
    {
        public long Id { get; set; }
        public long cityCode { get; set; }
        public string cityName { get; set; }
        public List<Area> mallAreaList { get; set; }
    }
    public class Area
    {
        public long Id { get; set; }
        public long areaCode { get; set; }
        public string areaName { get; set; }
    }
}
