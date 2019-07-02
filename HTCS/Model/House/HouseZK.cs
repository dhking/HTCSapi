using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.House
{
    public class HouseZK : BasicModel
    {
        public long Id { get; set; }
        public string Fx { get; set; }
        public string Cx { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Adress { get; set; }
        public string city { get; set; }
        public string area { get; set; }
        public string businessarea { get; set; }
        public long cityid { get; set; }
        public long areaid { get; set; }
        public decimal Price { get; set; }
        public int IsRm { get; set; }
        public DateTime PushTime { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
        public string Fukuan { get; set; }
        public string TingWei { get; set; }
        public int Type { get; set; }
        public int Shi { get; set; }
        public decimal Measure { get; set; }
        //精度
        public double LongiTude { get; set; }
        //纬度
        public double LatiTude { get; set; }
        //总层
        public int Floor { get; set; }
        //当前层
        public int FloorIndex { get; set; }
        public string CellName { get; set; }
        public string pts { get; set; }
        public string metro { get; set; }
        public string  Phone { get; set; }
        public DateTime RecentTime { get; set; }
        public long CompanyId { get; set; }
        [NotMapped]
        public decimal MaxPrice { get; set; }
        [NotMapped]
        public decimal MinPrice { get; set; }
        //特色信息
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public List<TeseZK> Ts { get; set; }
        [NotMapped]
        public List<PeibeiZK> Peipei { get; set; }
        [NotMapped]
        public List<Zhoubian> Zhoubian { get; set; }
        [NotMapped]
        public List<JiaoTong> JiaoTong { get; set; }
    }
    public class TeseZK: BasicModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        //所属房源的编号
        public long HouseId { get; set; }
        public long CompanyId { get; set; }
    }
    public class PeibeiZK: BasicModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public string HouseId { get; set; }
    }
    public class Zhoubian : BasicModel
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public long HouseId { get; set; }
        public long CompanyId { get; set; }
    }
    public class JiaoTong : BasicModel
    {
        public long Id { get; set; }
        public string Xian { get; set; }

        public string Zhan { get; set; }
        public long CompanyId { get; set; }
        public long Juli { get; set; }

        public int XianNumber { get; set; }

        public long HouseId { get; set; }
    }
    //预约信息
    public class Appointment : BasicModel
    {
        public DateTime date { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public long HouseId { get; set; }
    }
}
