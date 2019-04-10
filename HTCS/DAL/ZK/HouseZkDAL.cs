using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model;
using Mapping.cs;
using System.Linq.Expressions;
using DBHelp;
using ControllerHelper;
using Model.House;
using Model.Base;

namespace DAL.ZK
{
    public class HouseZKDAL : RcsBaseDao
    {
        public SysResult<IList<HouseZK>> Querylistrm(HouseZK model, OrderablePagination orderablePagination)
        {
            SysResult<IList<HouseZK>> sysresult = new SysResult<IList<HouseZK>>();
            var data = from a in BbHousezk where a.Status == 1  select a;
            
            Expression<Func<HouseZK, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.cityid != 0)
            {
                where = where.And(m => m.cityid == model.cityid);
            }
            if (model.IsRm != 0)
            {
                where = where.And(m => m.IsRm == model.IsRm);
            }
            if (model.areaid != 0)
            {
                where = where.And(m => m.areaid == model.areaid);
            }
            if (model.businessarea != null)
            {
                where = where.And(m => m.businessarea == model.businessarea);
            }
            if (model.MaxPrice != 0)
            {
                where = where.And(m => m.Price<= model.MaxPrice);
                where = where.And(m => m.Price >= model.MinPrice);
            }
            if (model.RecentTime != DateTime.MinValue)
            {
                where = where.And(m => m.RecentTime >= m.RecentTime);
            }
            if (model.RecentTime == DateTime.MinValue)
            {
                where = where.And(m => m.RecentTime <=DateTime.Now);
            }
            if (model.Shi != 0)
            {
                if (model.Shi == 4)
                {
                    where = where.And(m => m.Shi >= model.Shi);
                }
                else
                {
                    where = where.And(m => m.Shi == model.Shi);
                } 
            }
            if (model.Ts!=null&& model.Ts.Count>0)
            {
                string first = model.Ts.FirstOrDefault().Code;
                var tswhere = (from m in tesezk where m.Code== first select m);
                int count = 0;
                foreach (var t in model.Ts)
                {
                    if (count> 0)
                    {
                        tswhere=tswhere.Union((from m in tesezk where m.Code == t.Code select m));
                    }
                    count++;
                }
                var datats= from j in tswhere
                         group j by j.HouseId into g where g.Count()>= model.Ts.Count
                            select g;
              
                where = where.And(m => datats.Select(p => p.Key).Contains(m.Id));
            }
            if (model.JiaoTong!=null)
            {
                int first = model.JiaoTong.FirstOrDefault().XianNumber;
                var jtwhere = (from m in JiaoTong where m.XianNumber == first select m);
                int count = 0;
                foreach (var t in model.JiaoTong)
                {
                    if (count > 0)
                    {
                        jtwhere = jtwhere.Union((from m in JiaoTong where m.XianNumber == t.XianNumber select m));
                    }
                    count++;
                }
                var datats = from j in jtwhere
                             group j by j.HouseId into g
                             where g.Count() >= model.JiaoTong.Count
                             select g;

                where = where.And(m => datats.Select(p => p.Key).Contains(m.Id));
            }
            if (model.Type != 0)
            {
                where = where.And(m => m.Type == model.Type);
            }
            data = data.Where(where);
            IOrderByExpression<HouseZK> order = new OrderByExpression<HouseZK, decimal>(p => p.Id);
            sysresult.numberData = this.QueryForList<HouseZK>(this.BbHousezk, where, orderablePagination, order);
            foreach (var mo in sysresult.numberData)
            {
                List<TeseZK> tlist = (from j in tesezk where j.HouseId == mo.Id select j).Take(3).ToList();
                mo.Ts = tlist;
                List<JiaoTong> jlist = (from j in JiaoTong where j.HouseId == mo.Id select j).OrderBy(p => p.Juli).Take(1).ToList();
                mo.JiaoTong = jlist;
            }
            sysresult.numberCount = data.Count();
            return sysresult;
        }
        public HouseZK xq(long Id)
        {
            var data = from m in BbHousezk where m.Id== Id select m;
            HouseZK house = data.FirstOrDefault();
            house.Ts = (from j in tesezk where j.HouseId == house.Id select j).ToList();
            house.JiaoTong= (from j in JiaoTong where j.HouseId == house.Id select j).ToList();
            house.Zhoubian= (from j in Zhoubian where j.HouseId == house.Id select j).ToList();
            return house;
        }
        public int Save()
        {
            return 1;
        }
        public DbSet<HouseZK> BbHousezk { get; set; }
        public DbSet<TeseZK> tesezk { get; set; }
        public DbSet<JiaoTong> JiaoTong { get; set; }
        public DbSet<Zhoubian> Zhoubian { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ZhoubianMapping());
            modelBuilder.Configurations.Add(new TeseZKMapping());
            modelBuilder.Configurations.Add(new HouseZKMapping());
            modelBuilder.Configurations.Add(new JiaoTongMapping());
        }

    }
}
