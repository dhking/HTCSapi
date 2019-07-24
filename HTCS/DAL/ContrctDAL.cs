using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Mapping.cs.Contrct;
using Model;
using Model.Bill;
using Model.Contrct;
using Model.House;
using Model.TENANT;
using Model.User;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public   class ContrctDAL : RcsBaseDao
    {
        public List<WrapContract> Query(WrapContract model, OrderablePagination orderablePagination,T_SysUser user,long[] userids)
        {
            //整租查询
            var data = (from m in Contract
                       join n in Teant on m.TeantId equals n.Id
                       into temp from t in temp.DefaultIfEmpty()
                       join c in t_v_HouseQuery on m.HouseId equals c.Id
                       into temp1 from x in temp1.DefaultIfEmpty()
                       join c1 in BbUser on m.CreatePerson equals c1.Id
                       into temp2
                       from x1 in temp2.DefaultIfEmpty()
                       select new WrapContract()
                       {
                           CreateTime=m.CreateTime,
                           CreatePersonstr = x1==null?"":x1.RealName,
                           CreatePerson =m.CreatePerson,
                           Id = m.Id,
                           Name = t.Name==null?"":t.Name,
                           BeginTime = m.BeginTime,
                           EndTime = m.EndTime,
                           Status =m.Status,
                           Deposit = m.Deposit,
                           Recent = m.Recent,
                           Pinlv = m.PinLv,
                           Phone =t==null?"":t.Phone,
                           Document = t == null ? "" : t.Document,
                           CellName = x == null ? "" : x.CellName,
                           HouseName = x == null ? "" : x.Name,
                           City = x == null ? 0 : x.City,
                           Area = x == null ?0:x.Area,
                           CityName = x == null ? "" : x.CityName,
                           AreaName = x == null ? "" : x.AreaName,
                           LockId =x==null?"":x.LocalId ,
                           HouseType= x == null ? 0:x.RecentType,
                           HouseId = x == null ? 0 : x.Id,
                           storeid= x == null ? 0 : x.storeid,
                           CompanyId = x == null ? 0 : x.CompanyId,
                           HouseKeeper = x == null ? 0 : x.HouseKeeper,
                           isxuzu=m.isxuzu
                           
                       });
            Expression<Func<WrapContract, bool>> where = m => 1 == 1;
            //部门信息筛选
            if (user.departs != null && user.roles != null)
            {
                List<long> depentids = user.departs.Select(p => p.Id).ToList();
                if (user.roles.range == 2)
                {
                    where = where.And(m => depentids.Contains(m.storeid));
                    if (userids != null && userids.Length > 0)
                    {
                        where = where.Or(m => userids.Contains(m.HouseKeeper));
                    }
                }
                if (user.roles.range == 3)
                {
                    where = where.And(m => m.HouseKeeper == user.roles.userid);
                }
            }
            if (model.HouseId != 0)
            {
                where = where.And(m => m.HouseId == model.HouseId);
            }
            if (model.HouseType != 0)
            {
                where = where.And(m => m.HouseType == model.HouseType);
            }
            //逾期时间筛选
            if (model.yuqitype != 0)
            {
                DateTime dt = DateTime.Now.Date;
                if (model.yuqitype == 2)
                {
                    where = where.And(m => m.EndTime < dt);
                }
                if (model.yuqitype == 3)
                {
                    where = where.And(m => DbFunctions.TruncateTime(m.EndTime) == dt);
                }
                if (model.yuqitype == 4)
                {
                    DateTime dt1 = dt.AddDays(-1);
                    DateTime dt2 = dt.AddDays(-7);
                    where = where.And(m => m.EndTime >= dt2 && m.EndTime <= dt1);
                }
            }
            
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            
            if (model.Status != 0)
            {
                if (model.Status == 1|| model.Status == 7)
                {
                    where = where.And(m => m.Status == model.Status);
                }
                if (model.Status == 4)
                {
                    where = where.And(m => m.Status ==5 || m.Status == 2);
                    where = where.And(m => m.BeginTime >DateTime.Now);
                }
                if (model.Status == 9)
                {
                    where = where.And(m => m.Status == 9);
                }
                if (model.Status == 5)
                {
                    where = where.And(m => m.Status == 5 || m.Status == 2);
                }
                if (model.Status == 6)
                {
                    DateTime dt = DateTime.Now.AddDays(45).Date;
                    where = where.And(m => m.Status == 5 || m.Status == 2);
                    where = where.And(m => m.EndTime <= dt);
                }
                if (model.Status == 10)
                {
                    DateTime dt = DateTime.Now;
                    where = where.And(m => m.Status == 5 || m.Status == 2);
                    where = where.And(m => m.EndTime < dt);
                }
                if (model.Status == 12)
                {
                    DateTime dt = DateTime.Now;
                    where = where.And(m => m.Status == 5|| m.Status == 2||  m.Status == 4);
                    where = where.And(m => m.isxuzu == 1);
                }
            }
            if (model.CityName != "全部" && model.CityName != null)
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            if (model.AreaName != "全部" && model.AreaName != null)
            {
                where = where.And(m => m.AreaName == model.AreaName);
            }
            if (model.CellName != "全部"&& model.CellName !=null)
            {
                where = where.And(m => m.CellName.Contains(model.CellName));
            }
            if (model.CellName1 != "全部" && model.CellName1 != null)
            {
                where = where.And(m => m.CellName.Contains(model.CellName1));
            }
            if (model.Content != null)
            {
                where = where.And(m => m.Phone.Contains(model.Content) || m.Name.Contains(model.Content) || m.HouseName.Contains(model.Content) || m.CreatePersonstr.Contains(model.Content));
            }
            if (model.BeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.BeginTime>= model.BeginTime&& m.BeginTime <= model.EndTime);
            }
            if (model.tBeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.EndTime >= model.tBeginTime && m.EndTime <= model.tEndTime);
            }
            if (model.Phone != null)
            {
                where = where.And(m => m.Phone.Contains(model.Phone));
            }
            if (model.Name != null)
            {
                where = where.And(m => m.Name.Contains(model.Name));
            }
            if (model.EndTime != DateTime.MinValue)
            {
                where = where.And(m => m.EndTime >= model.EndTime);
            }
            if (model.eleccontract !=0)
            {
                where = where.And(m => m.eleccontract >= model.eleccontract);
            }
            if (model.CreatePerson != 0)
            {
                where = where.And(m => m.CreatePerson >= model.CreatePerson);
            }
            data = data.Where(where);
            IOrderByExpression<WrapContract> order1 = new OrderByExpression<WrapContract, long>(p => p.Id, true);
            List<WrapContract> list = QueryableForList(data, orderablePagination, order1);
            return list;
        }

        public List<WrapContract> excelQuery(WrapContract model, T_SysUser user)
        {
            //整租查询
            var data = (from m in Contract
                        join n in Teant on m.TeantId equals n.Id
                        into temp
                        from t in temp.DefaultIfEmpty()
                        join c in t_v_HouseQuery on m.HouseId equals c.Id
                        into temp1
                        from x in temp1.DefaultIfEmpty()
                        join c1 in BbUser on m.CreatePerson equals c1.Id
                        into temp2
                        from x1 in temp2.DefaultIfEmpty()
                        select new WrapContract()
                        {
                            CreateTime = m.CreateTime,
                            CreatePerson = m.CreatePerson,
                            CreatePersonstr=x1==null?"":x1.RealName,
                            Id = m.Id,
                            Name = t.Name == null ? "" : t.Name,
                            BeginTime = m.BeginTime,
                            EndTime = m.EndTime,
                            Status = m.Status,
                            Deposit = m.Deposit,
                            Recent = m.Recent,
                            Pinlv = m.PinLv,
                            Phone = t == null ? "" : t.Phone,
                            Document = t == null ? "" : t.Document,
                            CellName = x == null ? "" : x.CellName,
                            HouseName = x == null ? "" : x.Name,
                            City = x == null ? 0 : x.City,
                            Area = x == null ? 0 : x.Area,
                            CityName = x == null ? "" : x.CityName,
                            AreaName = x == null ? "" : x.AreaName,
                            LockId = x == null ? "" : x.LocalId,
                            HouseType = x == null ? 0 : x.RecentType,
                            HouseId = x == null ? 0 : x.Id,
                            storeid = x == null ? 0 : x.storeid,
                            CompanyId = x == null ? 0 : x.CompanyId,
                            HouseKeeper = x == null ? 0 : x.HouseKeeper,
                        });
            Expression<Func<WrapContract, bool>> where = m => 1 == 1;
            if (model.HouseId != 0)
            {
                where = where.And(m => m.HouseId == model.HouseId);
            }
            if (model.HouseType != 0)
            {
                where = where.And(m => m.HouseType == model.HouseType);
            }
            if (model.storeid != 0)
            {
                where = where.And(m => m.storeid == model.storeid);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (model.CreatePersonstr != null)
            {
                where = where.And(m => m.CreatePersonstr.Contains(model.CreatePersonstr));
            }
            
            //if (model.eleccontract != 2)
            //{
            //    where = where.And(m => m.eleccontract==model.eleccontract);
            //}
            if (model.arrCellNames != null)
            {
                if (model.arrCellNames.Length > 0)
                {
                    where = where.And(m => model.arrCellNames.Contains(m.CellName));
                }
            }
            if (user != null&& user.storeids!=null)
            {
                if (user.storeids.Length > 0 || user.range != 5)
                {
                    if (user.range == 1)
                    {
                        where = where.And(c => c.HouseKeeper == user.Id);
                    }
                    if (user.range == 2)
                    {
                        where = where.And(c => user.storeids.Contains(c.storeid));
                    }
                    if (user.range == 3)
                    {
                        where = where.And(c => user.areas.Contains(c.AreaName));
                    }
                    if (user.range == 4)
                    {
                        where = where.And(c => user.citys.Contains(c.CityName));
                    }

                }
            }

            if (model.Status != 0)
            {
                if (model.Status == 1 || model.Status == 7)
                {
                    where = where.And(m => m.Status == model.Status);
                }
                if (model.Status == 4)
                {
                    where = where.And(m => m.Status == 5);
                    where = where.And(m => m.BeginTime > DateTime.Now);
                }
                if (model.Status == 9)
                {
                    where = where.And(m => m.Status == 9);

                }
                if (model.Status == 5)
                {
                    where = where.And(m => m.Status == 5);
                    //where = where.And(m => m.BeginTime <= DateTime.Now);
                    //where = where.And(m => m.EndTime >= DateTime.Now);
                }
                if (model.Status == 6)
                {
                    DateTime dt = DateTime.Now.AddDays(-5);
                    where = where.And(m => m.Status == 5);
                    where = where.And(m => m.EndTime <= dt);
                }
                if (model.Status == 10)
                {
                    DateTime dt = DateTime.Now;
                    where = where.And(m => m.Status == 5);
                    where = where.And(m => m.EndTime < dt);
                }
            }
            if (model.CityName != "全部" && model.CityName != null)
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            if (model.AreaName != "全部" && model.AreaName != null)
            {
                where = where.And(m => m.AreaName == model.AreaName);
            }
            if (model.CellName != "全部" && model.CellName != null)
            {
                where = where.And(m => m.CellName.Contains(model.CellName));
            }
            if (model.CellName1 != "全部" && model.CellName1 != null)
            {
                where = where.And(m => m.CellName.Contains(model.CellName1));
            }
            if (model.Content != null)
            {
                where = where.And(m => m.Phone.Contains(model.Content) || m.Name.Contains(model.Content) || m.HouseName.Contains(model.Content) || m.CreatePersonstr.Contains(model.Content));
            }
            if (model.BeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.BeginTime >= model.BeginTime && m.BeginTime <= model.EndTime);
            }

            if (model.tBeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.EndTime >= model.tBeginTime && m.EndTime <= model.tEndTime);
            }

            if (model.qBeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.CreateTime >= model.qBeginTime && m.CreateTime <= model.qEndTime);
            }
            if (model.Phone != null)
            {
                where = where.And(m => m.Phone.Contains(model.Phone));
            }
            if (model.Name != null)
            {
                where = where.And(m => m.Name.Contains(model.Name));
            }
            if (model.EndTime != DateTime.MinValue)
            {
                where = where.And(m => m.EndTime >= model.EndTime);
            }
            if (model.Content != null)
            {
                if (model.Type == 1)
                {
                    where = where.And(m => m.Name == model.Content);
                }
            }
            data = data.Where(where);
            List<WrapContract> list = data.ToList();
            return list;
        }
        public List<WrapContract> Querytuizu(WrapContract model, OrderablePagination orderablePagination)
        {
            //整租查询
            var data = (from m in Contract
                        join n in Teant on m.TeantId equals n.Id
                        into temp
                        from t in temp.DefaultIfEmpty()
                        join c in t_v_HouseQuery on m.HouseId equals c.Id
                        into temp1
                        from x in temp1.DefaultIfEmpty()

                        select new WrapContract()
                        {
                            Id = m.Id,
                            Name = t.Name == null ? "" : t.Name,
                            BeginTime = m.BeginTime,
                            EndTime = m.EndTime,
                            Status = m.Status == null ? 0 : m.Status,
                            Deposit = m.Deposit,
                            Recent = m.Recent,
                            Pinlv = m.PinLv,
                            Phone = t == null ? "" : t.Phone,
                            CellName = x == null ? "" : x.CellName,
                            HouseName = x == null ? "" : x.Name,
                            City = x == null ? 0 : x.City,
                            Area = x == null ? 0 : x.Area,
                            CityName = x == null ? "" : x.CityName,
                            AreaName = x == null ? "" : x.AreaName,
                            LockId = x == null ? "" : x.LocalId,
                            HouseType = x == null ? 0 : x.RecentType,
                            HouseId = x == null ? 0 : x.Id
                        });
            Expression<Func<WrapContract, bool>> where = m => 1 == 1;
            DateTime dt2 = DateTime.Now;
            DateTime dt1= DateTime.Now.AddDays(-7);
            where = where.And(m => m.EndTime >= dt2);
            where = where.And(m => m.EndTime >= dt1);
            where = where.And(m => m.Status ==5);
            if (model.CityName != "全部" && model.CityName != null)
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            if (model.AreaName != "全部" && model.AreaName != null)
            {
                where = where.And(m => m.AreaName == model.AreaName);
            }
            if (model.CellName != "全部" && model.CellName != null)
            {
                where = where.And(m => m.CellName.Contains(model.CellName));
            }
            if (model.CellName1 != "全部" && model.CellName1 != null)
            {
                where = where.And(m => m.CellName.Contains(model.CellName1));
            }
            
            
           
            data = data.Where(where);
            IOrderByExpression<WrapContract> order1 = new OrderByExpression<WrapContract, long>(p => p.Id, true);
            List<WrapContract> list = QueryableForList(data, orderablePagination, order1);
            return list;
        }
        public T_Teant QueryTeant(T_Teant model)
        {
            //整租查询
            var data = from m in Teant select m;
            Expression<Func<T_Teant, bool>> where = m => 1 == 1;
            where = where.And(m => m.Phone == model.Phone);
            data = data.Where(where);
            return data.FirstOrDefault();
        }
        public List<WrapContract> notpageQuery(WrapContract model)
        {
            //整租查询
            var data = (from m in Contract
                        join n in Teant on m.TeantId equals n.Id
                        into temp
                        from t in temp.DefaultIfEmpty()
                        join c in t_v_HouseQuery on m.HouseId equals c.Id
                        into temp1
                        from x in temp1.DefaultIfEmpty()

                        select new WrapContract()
                        {
                            Id = m.Id,
                            Name = t.Name == null ? "" : t.Name,
                            BeginTime = m.BeginTime,
                            EndTime = m.EndTime,
                            Status = m.Status == null ? 0 : m.Status,
                            Deposit = m.Deposit,
                            Recent = m.Recent,
                            Pinlv = m.PinLv,
                            Phone = t == null ? "" : t.Phone,
                            CellName = x == null ? "" : x.CellName,
                            HouseName = x == null ? "" : x.Name,
                            City = x == null ? 0 : x.City,
                            Area = x == null ? 0 : x.Area,
                            LockId = x == null ? "" : x.LocalId,
                            HouseType = x == null ? 0 : x.RecentType,
                            HouseId = x == null ? 0 : x.Id,
                            onlinesign=m.onlinesign,
                            CompanyId=m.CompanyId
                        });
            Expression<Func<WrapContract, bool>> where = m => 1 == 1;
            if (model.HouseId != 0)
            {
                where = where.And(m => m.HouseId == model.HouseId);
            }
            if (model.HouseType != 0)
            {
                where = where.And(m => m.HouseType == model.HouseType);
            }
            if (model.arrStatus != null)
            {
                where = where.And(m => model.arrStatus.Contains(m.Status));
            }
            //if (model.Status != 0)
            //{
            //    if (model.Status == 1)
            //    {
            //        where = where.And(m => m.Status == model.Status);
            //    }
            //    if (model.Status == 4)
            //    {
            //        where = where.And(m => m.Status == 2);
            //        where = where.And(m => m.BeginTime > DateTime.Now);
            //    }
            //    if (model.Status == 5)
            //    {
            //        where = where.And(m => m.Status == 2);
            //        where = where.And(m => m.BeginTime <= DateTime.Now);
            //        where = where.And(m => m.EndTime >= DateTime.Now);
            //    }
            //    if (model.Status == 6)
            //    {
            //        where = where.And(m => m.Status == 2);
            //        where = where.And(m => m.EndTime < DateTime.Now);
            //    }
            //    if (model.Status == 7)
            //    {
            //        where = where.And(m => m.Status == 7);
            //    }
            //}
            if (model.City != 0)
            {
                where = where.And(m => m.City == model.City);
            }
            if (model.Area != 0)
            {
                where = where.And(m => m.Area == model.Area);
            }
            if (model.CellName != null)
            {
                where = where.And(m => m.CellName == model.CellName);
            }
            if (model.Phone != null)
            {
                where = where.And(m => m.Phone == model.Phone);
            }
            if (model.EndTime != DateTime.MinValue)
            {
                where = where.And(m => m.EndTime >= model.EndTime);
            }
            if (model.Content != null)
            {
                if (model.Type == 1)
                {
                    where = where.And(m => m.Name == model.Content);
                }
            }
            data = data.Where(where);
            return data.ToList();
        }
        //抄表查询
        public List<chaobiao> chaobiaoquery(chaobiao model,OrderablePagination orderablePagination)
        {
            var data = (from m in Contract
                        join n in Otherfee on m.Id equals n.ContractId
                        into temp
                        join c in t_v_HouseQuery on m.HouseId equals c.Id
                         into temp1
                        from t in temp
                        from x in temp1
                        select new chaobiao()
                        {
                            Id= t == null ? 0 : t.Id,
                            adress =x.Name,
                            treatname=m.treatname,
                            project=t.Name,
                            lasttime= t == null ? DateTime.MinValue : t.ChaobiaoTime,
                            lastdushu = t == null ? 0 : t.Reading,
                            price = t==null ? 0 : t.Price,
                            type = t == null ? 0 : t.Type,
                            HouseType = x == null ? 0 : x.RecentType,
                            name =t.Name,
                            housename=x.CellName,
                            houseid=m.HouseId,
                            isyajin=t.IsYajin,
                            mobject=t.Object,
                            pinlv=t.Pinlv,
                            storeid = x == null ? 0 : x.storeid,
                            CompanyId= x == null ? 0 : x.CompanyId,
                            CityName = x.CityName,
                            AreaName = x.AreaName
                        });
             Expression<Func<chaobiao, bool>> where = m => 1 == 1;
            //where = where.And(m => m.type == 1);
            where = where.And(m => m.isyajin== 0);
            where = where.And(m => m.mobject == 0);
            if (!string.IsNullOrEmpty(model.housename))
            {
                where = where.And(m => m.housename == model.housename);
            }
            if (!string.IsNullOrEmpty(model.AreaName))
            {
                where = where.And(m => m.AreaName == model.AreaName);
            }
            if (!string.IsNullOrEmpty(model.CityName))
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            if (!string.IsNullOrEmpty(model.project))
            {
                where = where.And(m => m.project == model.project);
            }
            if (model.begintime!=DateTime.MinValue&&model.endtime!= DateTime.MinValue)
            {
                where = where.And(m => m.lasttime >= model.begintime);
                where = where.And(m => m.lasttime <= model.endtime);
            }
            if (model.houseid != 0)
            {
                where = where.And(m => model.houseid == model.houseid);
            }
            if (model.HouseType != 0)
            {
                where = where.And(m => model.HouseType == model.HouseType);
            }
            if (model.type != 0)
            {
                where = where.And(m => m.type == model.type);
            }
            if (model.storeid != 0)
            {
                where = where.And(m => model.storeid == model.storeid);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (model.HouseType != 0)
            {
                where = where.And(m => model.HouseType == model.HouseType);
            }
            data = data.Where(where);
            IOrderByExpression<chaobiao> order1 = new OrderByExpression<chaobiao, long>(p => p.Id, true);
            List<chaobiao> list = QueryableForList(data, orderablePagination, order1);
            return list;
        }
        public chaobiao chaobiaoqueryxq(chaobiao model)
        {
            var data = (from m in Contract
                        join n in Otherfee on m.Id equals n.ContractId
                        into temp
                        join c in t_v_HouseQuery on m.HouseId equals c.Id
                         into temp1
                        from t in temp
                        from x in temp1
                        select new chaobiao()
                        {
                            Id = t == null ? 0 : t.Id,
                            adress = x.Name,
                            teantid = m.TeantId,
                            treatname = m.treatname,
                            project = t.Name,
                            lasttime = t == null ? DateTime.MinValue : t.ChaobiaoTime,
                            lastdushu = t == null ? 0 : t.Reading,
                            price = t == null ? 0 : t.Price,
                            type = t == null ? 0 : t.Type,
                            HouseType = x == null ? 0 : x.RecentType,
                            name = t.Name,
                            housename = x.CellName,
                            houseid = m.HouseId
                        });
            Expression<Func<chaobiao, bool>> where = m => 1 == 1;
            
            if (!string.IsNullOrEmpty(model.housename))
            {
                where = where.And(m => m.housename == model.housename);
            }
            if (!string.IsNullOrEmpty(model.project))
            {
                where = where.And(m => m.project == model.project);
            }
            if (model.begintime != DateTime.MinValue && model.endtime != DateTime.MinValue)
            {
                where = where.And(m => m.lasttime >= model.begintime);
                where = where.And(m => m.lasttime <= model.endtime);
            }
            if (model.houseid != 0)
            {
                where = where.And(m => model.houseid == model.houseid);
            }
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.HouseType != 0)
            {
                where = where.And(m => model.HouseType == model.HouseType);
            }
            data = data.Where(where);
            IOrderByExpression<chaobiao> order1 = new OrderByExpression<chaobiao, long>(p => p.Id, true);
            return data.FirstOrDefault();
        }
        public int Querycount()
        {
            int mo = (from m in Contract where m.Status == 1 select m).Count();
            return mo;
        }

        public int Querycountby(T_Contrct model)
        {
            var  data = (from m in Contract where m.Status == 2 || m.Status == 5 select m);
            Expression<Func<T_Contrct, bool>> where = m => 1 == 1;
            if (model.Id!=0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            data = data.Where(where);
            return data.Count();
        }
        public List<T_Teant> Queryteant(List<long?> arr)
        {
            var mo =from m in Teant where arr.Contains(m.Id) select m;
            return mo.ToList();
        }
        public WrapContract QueryId(WrapContract model)
        {
            ZafeiDAL zafei = new DAL.ZafeiDAL();
            //整租查询
            var data = from m in Contract
                       join c in t_v_HouseQuery on m.HouseId equals c.Id
                       into temp1
                       from x in temp1.DefaultIfEmpty()
                       join c in BbUser on m.CreatePerson equals c.Id
                       into temp2
                       from x1 in temp2.DefaultIfEmpty()
                       select new WrapContract() {
                           Id=m.Id,
                           BeginTime = m.BeginTime,
                           EndTime = m.EndTime,
                           Type = m.Type,
                           Pinlv = m.PinLv,
                           Recent = m.Recent,
                           DayRecnet = m.DayRecnet,
                           Deposit = m.Deposit,
                           DepositType = m.DepositType,
                           Remark = m.Remark,
                           Enclosure = m.Enclosure,
                           CreatePerson = m.CreatePerson,
                           CreatePersonstr = x1 == null ? "" : x1.RealName,
                           CreateTime = m.CreateTime,
                           Status = m.Status,
                           HouseName = x.Name,
                           LockId = x.LocalId,
                           BeforeDay = m.BeforeDay,
                           Recivetype = m.Recivetype,
                           RecivedType = m.RecivedType,
                           RecivedAmount = m.RecivedAmount,
                           RecivedAccount = m.RecivedAccount,
                           TeantId=m.TeantId,
                           HouseId=m.HouseId,
                           
                           //收款人
                           payee = m.payee,
                           accounts =m.accounts,
                           bank = m.bank,
                           onlinesign=m.onlinesign,
                           //房间信息
                           CityName = x.CityName,
                           floor = x == null ? 0 : x.nowfloor,
                           allfloor = x == null ? 0 : x.allfloor,
                           mesure = x == null ? 0 : x.measure,
                           CompanyId= x == null ? 0 : x.CompanyId
                       };
            Expression<Func<WrapContract, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.HouseId != 0)
            {
                where = where.And(m => m.HouseId == model.HouseId);
            }
            if (model.LockId != null)
            {
                where = where.And(m => m.LockId == model.LockId);
            }
            if (model.arrStatus != null)
            {
                where = where.And(m => model.arrStatus.Contains(m.Status));
            }
            data = data.Where(where);
            WrapContract contract = data.FirstOrDefault();
            if (contract != null)
            {
                contract.Teant = (from m in Teant where m.Id == contract.TeantId select m).FirstOrDefault();
                List<T_Otherfee> fee = new List<T_Otherfee>();
                fee = QueryFee(contract.Id);
                List<T_Otherfee> otherfee= fee.Where(p => p.IsYajin == 0).ToList();
                foreach(var m in otherfee)
                {
                    m.strPinlv = DAL.Common.ConvertHelper.pinlv(m.Pinlv);
                }
                List<T_Otherfee> jajin = fee.Where(p => p.IsYajin == 1).DistinctBy(p=>p.Name).ToList();
                contract.Otherfee = otherfee;
                contract.Yajin = jajin;
            }
            return contract;
        }
        //查询租客信息
        public T_Teant queryteant(T_Teant model)
        {
            var mo = from m in Teant    select m;
            Expression<Func<T_Teant, bool>> where = m => 1 == 1;
            if (model.Phone !=null)
            {
                where = where.And(m => m.Phone == model.Phone);
            }
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }

            mo = mo.Where(where);
            return mo.FirstOrDefault();

        }
        //更新租客信息
        public bool Cmdxuzu(long tuizuid, string opera, string spname, out string errmsg)
        {
            errmsg = "";
            var cmd = this.Database.Connection.CreateCommand();
            OracleParameter paramSkuids = new OracleParameter("restparameter", OracleDbType.Varchar2);
            paramSkuids.Direction = ParameterDirection.Input;
            paramSkuids.Value = tuizuid;
            cmd.Parameters.Add(paramSkuids);
            OracleParameter paramShopId = new OracleParameter("operator", OracleDbType.Varchar2);
            paramShopId.Direction = ParameterDirection.Input;
            paramShopId.Value = opera;
            cmd.Parameters.Add(paramShopId);
            OracleParameter code = new OracleParameter("code", OracleDbType.Int16);
            code.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(code);
            OracleParameter msg = new OracleParameter("msg", OracleDbType.Varchar2, 500);
            msg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(msg);
            cmd.CommandText = spname;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            if (code.Value.ToString() == "0")
            {
                return true;
            }
            else
            {
                errmsg = msg.Value.ToString();
                return false;
            }
        }

        public long updatetrant(T_Teant model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_TEANT')");
                 AddModel<T_Teant>(model);

            }
            else
            {
                 ModifiedModel<T_Teant>(model, false,new string[] { "mobject"});
            }
            return model.Id;
        }
        //查询合同数量
        public int Querycount(T_Contrct model)
        {
         
            //整租查询
            var data = from m in Contract select m;
            Expression<Func<T_Contrct, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.HouseId != 0)
            {
                where = where.And(m => m.HouseId == model.HouseId);
            }
            if (model.Status != 0)
            {
                where = where.And(m => m.Status != model.Status);
            }
            if (model.CreateTime !=DateTime.MinValue)
            {
              
                where = where.And(m => DbFunctions.TruncateTime(m.CreateTime) ==model.CreateTime);
            }
            data = data.Where(where);
            return data.Count();
        }
        public int Querycount1(T_Contrct model)
        {
            ZafeiDAL zafei = new DAL.ZafeiDAL();
            //整租查询
            var data = from m in Contract select m;
            Expression<Func<T_Contrct, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.HouseId != 0)
            {
                where = where.And(m => m.HouseId == model.HouseId);
            }
            if (model.Status != 0)
            {
                where = where.And(m => m.Status == model.Status);
            }
            data = data.Where(where);
            return data.Count();
        }
        //根据房间查询合同
        public WrapContract Querybyhouse(HouseModel model)
        {
            var mo = from m in Contract where  m.HouseId == model.Id join n in Teant on m.TeantId equals n.Id     select new WrapContract() {BeginTime=m.BeginTime,EndTime=m.EndTime,Name=n.Phone };
            return mo.FirstOrDefault();
        }

        public T_Contrct querycontract(T_Contrct model)
        {
            var data = from m in Contract select m;
            Expression<Func<T_Contrct, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            data = data.Where(where);
            return data.FirstOrDefault();
        }
        public long SaveContrct(T_Contrct model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_CONTRACT')");
                if (model.CreateTime==DateTime.MinValue){
                    model.CreateTime = DateTime.Now;
                }
                AddModel<T_Contrct>(model);
            }
            else
            {
                 ModifiedModel<T_Contrct>(model, false,new string[] {  "HouseId",  "userid" });
            }
            return model.Id;
        }

        public long SaveContrct1(T_Contrct model)
        {
            if (model.Id == 0)
            {

                model.Id = GetNextValNum("GET_WSEQUENCES('T_CONTRACT')");
                if (model.CreateTime == DateTime.MinValue)
                {
                    model.CreateTime = DateTime.Now;
                }
         
                AddModel<T_Contrct>(model);

            }
            else
            {
                ModifiedModel1<T_Contrct>(model, false, new string[] { "Status" });
            }
            return model.Id;
        }
        public long SaveTrent(T_Teant model)
        {
           
            model.Id = GetNextValNum("GET_WSEQUENCES('T_TENANT')");
            if (AddModel(model) > 0)
            {
                return model.Id;
            }
            else
            {
                return 0;
            }
            
        }
        public List<T_Otherfee> QueryFee(long ContractId)
        {
            var data = from m in Otherfee where m.ContractId == ContractId select m;
            return data.ToList();
        }
        public int SaveOtherFee(List<T_Otherfee> model,long ContractId)
        {
            if(model.Count() == 0)
            {
                return 1;
            }
            foreach (var mo in model)
            {
                
                mo.ContractId = ContractId;
                mo.Id = GetNextValNum("GET_WSEQUENCES('T_OTHERFEE')");
                PlAddModel<T_Otherfee>(mo);
            }
            return this.SaveChanges();
        }
        public int updateOtherFee(T_Otherfee model,params string[] arr)
        {
            return ModifiedModel1<T_Otherfee>(model, false,arr);
        }
        public bool CmdBill(long ContractId,string opera,int type,int issendmessage, out string  errmsg)
        {
            errmsg = "";
            var cmd = this.Database.Connection.CreateCommand();
            OracleParameter paramSkuids = new OracleParameter("restparameter", OracleDbType.Varchar2);
            paramSkuids.Direction = ParameterDirection.Input;
            paramSkuids.Value = ContractId;
            cmd.Parameters.Add(paramSkuids);
            OracleParameter paramShopId = new OracleParameter("operator", OracleDbType.Varchar2);
            paramShopId.Direction = ParameterDirection.Input;
            paramShopId.Value = opera;
            cmd.Parameters.Add(paramShopId);
            //操作类型
            OracleParameter paratype = new OracleParameter("paratype", OracleDbType.Varchar2);
            paratype.Direction = ParameterDirection.Input;
            paratype.Value = type;
            cmd.Parameters.Add(paratype);
            //是否发送短信
            OracleParameter sendmessage = new OracleParameter("sendmessage", OracleDbType.Varchar2);
            sendmessage.Direction = ParameterDirection.Input;
            sendmessage.Value = issendmessage;
            cmd.Parameters.Add(sendmessage);
            OracleParameter code = new OracleParameter("code", OracleDbType.Int16);
            code.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(code);
            OracleParameter msg = new OracleParameter("msg", OracleDbType.Varchar2,500);
            msg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(msg);
            cmd.CommandText = "sp_createbill";
            cmd.CommandType = CommandType.StoredProcedure;
           
            cmd.ExecuteNonQuery();
           
            if (code.Value.ToString()=="0")
            {
                return true;
            }
            else
            {
                errmsg = msg.Value.ToString();
                return false;
            }
        }

        public bool deleteotherfee(long ContractId,  out string errmsg)
        {
            errmsg = "";
            var cmd = this.Database.Connection.CreateCommand();
            OracleParameter paramSkuids = new OracleParameter("restparameter", OracleDbType.Varchar2);
            paramSkuids.Direction = ParameterDirection.Input;
            paramSkuids.Value = ContractId;
            cmd.Parameters.Add(paramSkuids);
            OracleParameter paramobj = new OracleParameter("obj", OracleDbType.Varchar2);
            paramobj.Direction = ParameterDirection.Input;
            paramobj.Value = 0;
            cmd.Parameters.Add(paramobj);
            OracleParameter code = new OracleParameter("code", OracleDbType.Int16);
            code.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(code);
            OracleParameter msg = new OracleParameter("msg", OracleDbType.Varchar2, 500);
            msg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(msg);
            cmd.CommandText = "sp_deletefee";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.ExecuteNonQuery();

            if (code.Value.ToString() == "0")
            {
                return true;
            }
            else
            {
                errmsg = msg.Value.ToString();
                return false;
            }
        }
        public bool CmdotherfeeBill(chaobiao model, string opera, out string errmsg)
        {

            errmsg = "";
            var cmd = this.Database.Connection.CreateCommand();
            OracleParameter paramSkuids = new OracleParameter("restparameter", OracleDbType.Int64);
            paramSkuids.Direction = ParameterDirection.Input;
            paramSkuids.Value = model.Id;
            cmd.Parameters.Add(paramSkuids);
            OracleParameter paramlasttime = new OracleParameter("lasttime", OracleDbType.Date);
            paramlasttime.Direction = ParameterDirection.Input;
            paramlasttime.Value = model.lasttime;
            cmd.Parameters.Add(paramlasttime);
            OracleParameter paramlastdushu = new OracleParameter("lastdushu", OracleDbType.Decimal);
            paramlastdushu.Direction = ParameterDirection.Input;
            paramlastdushu.Value = model.lastdushu;
            cmd.Parameters.Add(paramlastdushu);

            OracleParameter paramshoureceivetime = new OracleParameter("shoureceivetime", OracleDbType.Date);
            paramshoureceivetime.Direction = ParameterDirection.Input;
            paramshoureceivetime.Value = model.receivetime;
            cmd.Parameters.Add(paramshoureceivetime);
            OracleParameter paramremark = new OracleParameter("remark", OracleDbType.Varchar2);
            paramremark.Direction = ParameterDirection.Input;
            paramremark.Value = model.remark;
            cmd.Parameters.Add(paramremark);

           
         

            OracleParameter paramShopId = new OracleParameter("operator", OracleDbType.Varchar2);
            paramShopId.Direction = ParameterDirection.Input;
            paramShopId.Value = opera;
            cmd.Parameters.Add(paramShopId);
            OracleParameter code = new OracleParameter("code", OracleDbType.Int16);
            code.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(code);
            OracleParameter msg = new OracleParameter("msg", OracleDbType.Varchar2, 500);
            msg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(msg);
            cmd.CommandText = "sp_createchaobiaobill";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();

            if (code.Value.ToString() == "0")
            {
                return true;
            }
            else
            {
                errmsg = msg.Value.ToString();
                return false;
            }
        }
        public bool CmdDelete(long ContractId, string opera,string spname, out string errmsg)
        {
            errmsg = "";
            string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
            using (OracleConnection cnn = new OracleConnection(connectionString))
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand(spname, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter restParameter = new OracleParameter("RestParameter", ContractId);
                OracleParameter OperatorParameter = new OracleParameter("Operator", opera);
                OracleParameter CodeParameter = new OracleParameter("Code", OracleDbType.Int32);
                CodeParameter.Direction = ParameterDirection.Output;
                OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.NVarchar2, 2000);
                MsgParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(restParameter);
                cmd.Parameters.Add(OperatorParameter);
                cmd.Parameters.Add(CodeParameter);
                cmd.Parameters.Add(MsgParameter);
                cmd.ExecuteNonQuery();
                if (CodeParameter.Value.ToString() == "0")
                {
                    return true;
                }
                else
                {
                    errmsg = MsgParameter.Value.ToString();
                    return false;
                }
            }
        }
        public WrapContract Querycontract(long id)
        {
            var data = (from m in Contract
                        join n in Teant on m.TeantId equals n.Id
                        into temp
                        from t in temp.DefaultIfEmpty()
                        join c in t_v_HouseQuery on m.HouseId equals c.Id
                        into temp1
                        from x in temp1.DefaultIfEmpty()

                        select new WrapContract()
                        {
                            Id = m.Id,
                            Name = t.Name == null ? "" : t.Name,
                            BeginTime = m.BeginTime,
                            EndTime = m.EndTime,
                            Status = m.Status == null ? 0 : m.Status,
                            Deposit = m.Deposit,
                            Recent = m.Recent,
                            Pinlv = m.PinLv,
                            TeantId =t == null ? 0 : t.Id,
                            Phone = t == null ? "" : t.Phone,
                            CellName = x == null ? "" : x.CellName,
                            HouseName = x == null ? "" : x.Name,
                            City = x == null ? 0 : x.City,
                            Area = x == null ? 0 : x.Area,
                            CityName = x == null ? "" : x.CityName,
                            AreaName = x == null ? "" : x.AreaName,
                            LockId = x == null ? "" : x.LocalId,
                            HouseType = x == null ? 0 : x.RecentType,
                            HouseId = x == null ? 0 : x.Id,
                            onlinesign=m.onlinesign,
                            issign=t.issign,
                            CompanyId= x == null ? 0 : x.CompanyId,
                            Document =t.Document
                        });
            Expression<Func<WrapContract, bool>> where = m => 1 == 1;
            where = where.And(p => p.Id == id);
            data = data.Where(where);
            return data.FirstOrDefault();
        } 
        public DbSet<T_Contrct> Contract { get; set; }
        public DbSet<T_Teant> Teant { get; set; }
        public DbSet<HouseModel> House { get; set; }
        public DbSet<HouseQuery> t_v_HouseQuery { get; set; }
        public DbSet<T_Otherfee> Otherfee { get; set; }
        public DbSet<T_SysUser> BbUser { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ContrctMapping());
            modelBuilder.Configurations.Add(new OtherFeeMapping());
            modelBuilder.Configurations.Add(new TenantMapping());
            modelBuilder.Configurations.Add(new HouseMapping());
            modelBuilder.Configurations.Add(new HouseQueryMapping());
            modelBuilder.Configurations.Add(new T_SysUserMapping());
        }
    }
}
