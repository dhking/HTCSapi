using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Model;
using Mapping.cs;
using System.Linq.Expressions;
using DBHelp;
using ControllerHelper;
using Model.House;
using Model.Base;
using Mapping.cs.Contrct;
using Model.Contrct;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Data.Entity.SqlServer;
using Model.User;
using CodeFirstStoreFunctions;
using Model.TENANT;

namespace DAL
{
    public class HouseDAL : RcsBaseDao
    {


        public IList<WrapHouseModel1>  Querylist(HouseModel model, dtmode dtmo, List<t_department> list, T_SysRole role,long [] userids, OrderablePagination orderablePagination,string[] arr,out long count)
        {
            IList<WrapHouseModel1> listhouse = new List<WrapHouseModel1>();
           
            var data = from a in BbHouse
                       join c1 in BbUser on a.HouseKeeper equals c1.Id
                       into temp2
                       from x1 in temp2.DefaultIfEmpty()
                       select new WrapHouseModel1() {
                           Id=a.Id,
                           BuildingNumber=a.BuildingNumber,
                           housekeepername=x1==null?"": x1.RealName,
                           RoomId =a.RoomId,
                           storeid=a.storeid,
                           ShiNumber=a.ShiNumber,
                           HouseKeeper=a.HouseKeeper,
                           Orientation = a.Orientation,

                           CompanyId = a.CompanyId,
                           CellName = a.CellName,
                           Adress = a.Adress,

                           CityName = a.CityName,
                           AreamName = a.AreamName,
                           RecrntType = a.RecrntType,
                           PublicImg=a.PublicImg,
                           Measure =a.Measure,
                           Status=a.Status,
                           sign=a.sign,
                           Renttime=a.Renttime,
                           distince= get_distance(model.latitude,model.longitude, a.latitude, a.longitude)
                        
                       };
            var data1 = from b in BbHousepent  select b;
            Expression<Func<WrapHouseModel1, bool>> where = m =>1 == 1;
            Expression<Func<HousePendent, bool>> where1 = m => 1 == 1;
            if (model.City != 0)
            {
                where = where.And(m => m.City == model.City);
            }
            if (model.RoomId != null)
            {
                where = where.And(m => m.RoomId == model.RoomId);
            }
            if (arr != null)
            {
                string Content = arr[0];
                if (arr.Length == 1)
                {
                    where = where.And(m => m.CellName.Contains(Content) || m.Adress.Contains(Content) || m.RoomId.Contains(Content) || m.housekeepername.Contains(Content));
                }
                else
                {
                    where = where.And(m => m.CellName.Contains(Content) || m.Adress.Contains(Content));
                    if (arr.Length == 2)
                    {
                        string CellName = arr[0];
                        string BuildingNumber = arr[1];
                        where = where.And(m => m.CellName.Contains(CellName) || m.Adress.Contains(CellName));
                        where = where.And(m => m.BuildingNumber.Contains(BuildingNumber));
                    }
                    if (arr.Length == 3)
                    {
                        string CellName = arr[0];
                        string BuildingNumber = arr[1];
                        string RoomId = arr[2];
                        where = where.And(m => m.CellName.Contains(CellName) || m.Adress.Contains(CellName));
                        where = where.And(m => m.BuildingNumber.Contains(BuildingNumber));
                        where = where.And(m => m.RoomId.Contains(RoomId));
                    }
                }
            }
            //部门信息筛选
            if (list != null&&role!=null)
            {
                List<long> depentids = list.Select(p => p.Id).ToList();
                
                if (role.ishouse == 0)
                {
                    if (role.range == 2)
                    {
                        where = where.And(m => depentids.Contains(m.storeid));
                        if(userids!=null&& userids.Length > 0)
                        {
                            where = where.Or(m => userids.Contains(m.HouseKeeper));
                        }
                    }
                    if (role.range == 3)
                    {
                        where = where.And(m => m.HouseKeeper == role.userid);
                    }
                }
            }
            if (model.ShiNumber != 0)
            {
                if(model.ShiNumber == 5)
                {
                    where = where.And(m => m.ShiNumber >= model.ShiNumber);
                }
                else
                {
                    where = where.And(m => m.ShiNumber == model.ShiNumber);
                }
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            //if (!string.IsNullOrEmpty(model.Content))
            //{
            //    where = where.And(m => m.CellName.Contains(model.Content)||m.Adress.Contains(model.Content)||m.RoomId.Contains(model.Content) || m.housekeepername.Contains(model.Content));
            //}
            if (!string.IsNullOrEmpty(model.CityName)&&model.CityName!="附近")
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            if (!string.IsNullOrEmpty(model.AreamName) && model.AreamName != "不限" && model.AreamName != "1千米" && model.AreamName != "2千米" && model.AreamName != "3千米")
            {
                where = where.And(m => m.AreamName == model.AreamName);
            }
            if (!string.IsNullOrEmpty(model.CellName))
            {
                where = where.And(m => m.CellName==model.CellName);
            }
            if (model.RecrntType!=0)
            {
                where = where.And(m => m.RecrntType== model.RecrntType);
            }
            if (!string.IsNullOrEmpty(model.Orientation))
            {
                where = where.And(m => m.Orientation == model.Orientation);
            }
            if (model.ishaveimg != 0)
            {
                if (model.ishaveimg == 1)
                {
                    where1 = where1.And(m => m.PrivateImage != null);
                }
                if (model.ishaveimg == 2)
                {
                    where1 = where1.And(m => m.PrivateImage == null);
                }
            }
            //if (dtmo.dt != DateTime.MinValue && model.RecrntType == 1)
            //{
            //    data = from a in BbHouse  where a.Renttime >= dtmo.dt && a.Renttime <= dtmo.dt2&&a.Status==1   select a;
            //}
            if (model.Status != 0)
            {
                where1 = where1.And(m => m.Status == model.Status);
            }
            if (!string.IsNullOrEmpty(model.Huxing))
            {
                where1 = where1.And(m => m.Huxing == model.Huxing);
            }
            if (dtmo.dt != DateTime.MinValue||dtmo.dt2 != DateTime.MinValue)
            {
                where = where.And(m => DbFunctions.TruncateTime(m.Renttime) >= dtmo.dt);
                where = where.And(m => DbFunctions.TruncateTime(m.Renttime) <= dtmo.dt2);
                where1 = where1.And(m => m.Status == 1);
            }

            if (model.sign!=0)
            {
                where1 = where1.And(m => m.sign == model.sign);
            }
            if (model.radius != 0&&model.latitude != 0 && model.longitude != 0)
            {
                where = where.And(m => m.distince <= model.radius);
            }
            data1 = data1.Where(where1);
            where = where.And(m=>(data1.Select(p => p.ParentRoomid)).Contains(m.Id));
            data = data.Where(where);
            if (model.latitude != 0 && model.longitude != 0)
            {
                
                IOrderByExpression<WrapHouseModel1> order = new OrderByExpression<WrapHouseModel1, double>(p => p.distince, false);
                listhouse = this.QueryableForList<WrapHouseModel1>(data, orderablePagination, order);
            }
            else
            {
                IOrderByExpression<WrapHouseModel1> order = new OrderByExpression<WrapHouseModel1, long>(p => p.Id, model.GroupBy);
                listhouse = this.QueryableForList<WrapHouseModel1>(data, orderablePagination, order);
            }
            count = data1.Where(p => data.Select(m => m.Id).Contains(p.ParentRoomid)).Count();
            return listhouse;
        }
        //查询导出数据
        public IList<HouseModel> excelQuerylist(HouseModel model, dtmode dtmo)
        {
            IList<HouseModel> listhouse = new List<HouseModel>();
            var data = from a in BbHouse select a;
            var data1 = from b in BbHousepent select b;
            Expression<Func<HouseModel, bool>> where = m => 1 == 1;
            Expression<Func<HousePendent, bool>> where1 = m => 1 == 1;
            if (model.City != 0)
            {
                where = where.And(m => m.City == model.City);
            }
            if (model.RoomId != null)
            {
                where = where.And(m => m.RoomId == model.RoomId);
            }
            if (model.storeid != 0)
            {
                where = where.And(m => m.storeid == model.storeid);
            }
            if (model.ShiNumber != 0)
            {
                where = where.And(m => m.ShiNumber == model.ShiNumber);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (!string.IsNullOrEmpty(model.Content))
            {
                where = where.And(m => m.CellName.Contains(model.Content) || m.Adress.Contains(model.Content) || m.RoomId.Contains(model.Content));
            }
            if (!string.IsNullOrEmpty(model.CityName))
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            if (!string.IsNullOrEmpty(model.AreamName))
            {
                where = where.And(m => m.AreamName == model.AreamName);
            }
            if (!string.IsNullOrEmpty(model.CellName))
            {
                where = where.And(m => m.CellName == model.CellName);
            }
            if (model.RecrntType != 0)
            {
                where = where.And(m => m.RecrntType == model.RecrntType);
            }
            if (model.Status != 0 && model.RecrntType == 1)
            {
                where = where.And(m => m.Status == model.Status);
            }
           
            if (model.Status != 0 && model.RecrntType != 1)
            {
                where1 = where1.And(m => m.Status == model.Status);
            }
            if (dtmo.dt != DateTime.MinValue||dtmo.dt2!=DateTime.MinValue )
            {
                where1 = where1.And(m => m.RecentTime >= dtmo.dt);
                where1 = where1.And(m => m.RecentTime <= dtmo.dt2);
                where1 = where1.And(m => m.Status == 1);
            }
            if (model.sign != 0)
            {
                where1 = where1.And(m => m.sign == model.sign);
            }
            data = data.Where(where);
            data1 = data1.Where(where1);
            where = where.And(m => (data1.Select(p => p.ParentRoomid)).Contains(m.Id));
            IOrderByExpression<HouseModel> order = new OrderByExpression<HouseModel, long>(p => p.Id, model.GroupBy);
            return data.ToList();
        }
        //group分组
        public List<WrapCellName> housegroup(HouseModel model, dtmode dtmo, OrderablePagination orderablePagination, string[] citys, T_SysUser user)
        {
            List<WrapCellName> sysresult = new List<WrapCellName>();
            var data = from a in BbHouse
                       group a by a.CellName into grouped
                       select new WrapCellName()
                       {
                           CellName=grouped.Key,
                           housecount= grouped.Count()
                       };
            var data1 = from a in BbHouse
                       join c1 in BbUser on a.HouseKeeper equals c1.Id
                       into temp2
                       from x1 in temp2.DefaultIfEmpty()

                       join c1 in (from cont in Contract where (cont.Status == 2 || cont.Status == 5) select cont) on a.Id equals c1.HouseId
                       into temp3
                       from x2 in temp3.DefaultIfEmpty()
                       join n in Teant on x2.TeantId equals n.Id
                       into temp4
                       from x3 in temp4.DefaultIfEmpty()
                       select new WrapHouseModel1()
                       {
                           constatus = x2 == null ? 0 : x2.Status,
                           teantname = x3.Name,
                           recent = x2 == null ? 0 : x2.Recent,
                           endtime = x2 == null ? DateTime.MinValue : DbFunctions.TruncateTime(x2.EndTime),
                           Id = a.Id,
                           BuildingNumber = a.BuildingNumber,
                           housekeepername = x1 == null ? "" : x1.RealName,
                           RoomId = a.RoomId,
                           storeid = a.storeid,
                           ShiNumber = a.ShiNumber,
                           CompanyId = a.CompanyId,
                           CellName = a.CellName,
                           Adress = a.Adress,
                           BusinessArea = a.BusinessArea,
                           CityName = a.CityName,
                           AreamName = a.AreamName,
                           RecrntType = a.RecrntType,
                           Measure = a.Measure,
                           Status = a.Status,
                           sign = a.sign,
                           Renttime = a.Renttime,
                           Orientation = a.Orientation,
                           PublicImg = a.PublicImg,
                           PublicTeshe=a.PublicTeshe,
                           Price=a.Price,
                           distince = get_distance(model.latitude, model.longitude, a.latitude, a.longitude)
                       };
            Expression<Func<WrapHouseModel1, bool>> where = m => 1 == 1;
            Expression<Func<WrapCellName, bool>> where1 = m => 1 == 1;
            if (model.City != 0)
            {
                where = where.And(m => m.City == model.City);
            }
            if (model.RoomId != null)
            {
                where = where.And(m => m.RoomId == model.RoomId);
            }
            if (model.radius != 0 && model.latitude != 0 && model.longitude != 0)
            {
                where = where.And(m => m.distince <= model.radius);
            }
            if (model.storeid != 0)
            {
                where = where.And(m => m.storeid == model.storeid);
            }
            if (model.ShiNumber != 0)
            {
                if (model.ShiNumber == 5)
                {
                    where = where.And(m => m.ShiNumber >= model.ShiNumber);
                }
                else
                {
                    where = where.And(m => m.ShiNumber == model.ShiNumber);
                }
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (model.ishaveimg != 0)
            {
                if (model.ishaveimg == 1)
                {
                    where = where.And(m => m.PublicImg != null);
                }
                if (model.ishaveimg == 2)
                {
                    where = where.And(m => m.PublicImg == null);
                }
            }
            if ((user.range == 2 || user.range == 3) && citys != null)
            {
                where = where.And(p => citys.Contains(p.CityName));
            }
            if (!string.IsNullOrEmpty(model.Content))
            {
                where = where.And(m => m.CellName.Contains(model.Content) || m.Adress.Contains(model.Content) || m.RoomId.Contains(model.Content) || m.housekeepername.Contains(model.Content));
            }
            if (!string.IsNullOrEmpty(model.CityName) && model.CityName != "附近")
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            if (!string.IsNullOrEmpty(model.AreamName) && model.AreamName != "不限" && model.AreamName != "1千米" && model.AreamName != "2千米" && model.AreamName != "3千米")
            {
                where = where.And(m => m.AreamName == model.AreamName);
            }
            if (!string.IsNullOrEmpty(model.CellName))
            {
                where = where.And(m => m.CellName == model.CellName);
            }
            if (model.RecrntType != 0)
            {
                where = where.And(m => m.RecrntType == model.RecrntType);
            }
            if (model.Status != 0)
            {
                where = where.And(m => m.Status == model.Status);
            }
            if (!string.IsNullOrEmpty(model.Orientation))
            {
                where = where.And(m => m.Orientation == model.Orientation);
            }

            if (dtmo.dt != DateTime.MinValue || dtmo.dt2 != DateTime.MinValue)
            {
                where = where.And(m => DbFunctions.TruncateTime(m.Renttime) >= dtmo.dt);
                where = where.And(m => DbFunctions.TruncateTime(m.Renttime) <= dtmo.dt2);
                where = where.And(m => m.Status == 1);
            }
            if (model.sign != 0)
            {
                where = where.And(m => m.sign == model.sign);
            }
            data1 = data1.Where(where);
            data =data.Where(where1.And(m =>(data1.Select(p => p.CellName)).Contains(m.CellName)));
            IOrderByExpression<WrapCellName> order = new OrderByExpression<WrapCellName, string>(p => p.CellName, model.GroupBy);
            sysresult = this.QueryableForList<WrapCellName>(data, orderablePagination, order);
            return sysresult;
        }
        public List<WrapHouseModel1> Querylist2(HouseModel model, dtmode dtmo, OrderablePagination orderablePagination,string[] citys,T_SysUser user,string [] arr)
        {
            List<WrapHouseModel1> sysresult = new List<WrapHouseModel1>();
            var data = from a in BbHouse
                       join c1 in BbUser on a.HouseKeeper equals c1.Id
                       into temp2
                       from x1 in temp2.DefaultIfEmpty()

                       join c1 in (from cont in Contract where (cont.Status == 2 || cont.Status == 5) select cont) on a.Id equals c1.HouseId
                       into temp3
                       from x2 in temp3.DefaultIfEmpty()
                       join n in Teant on x2.TeantId equals n.Id
                       into temp4
                       from x3 in temp4.DefaultIfEmpty()
                       select new WrapHouseModel1()
                       {
                           constatus = x2 == null ? 0 : x2.Status,
                           teantname = x3.Name,
                           recent = x2 == null ? 0 : x2.Recent,
                           endtime = x2 == null ? DateTime.MinValue : DbFunctions.TruncateTime(x2.EndTime),

                           Id = a.Id,
                           BuildingNumber = a.BuildingNumber,
                           housekeepername = x1 == null ? "" : x1.RealName,
                           RoomId = a.RoomId,
                           storeid = a.storeid,
                           ShiNumber = a.ShiNumber,

                           CompanyId = a.CompanyId,
                           CellName = a.CellName,
                           Adress = a.Adress,
                           BusinessArea=a.BusinessArea,
                           CityName = a.CityName,
                           AreamName = a.AreamName,
                           RecrntType = a.RecrntType,
                           Measure = a.Measure,
                           Status = a.Status,
                           sign = a.sign,
                           Renttime = a.Renttime,
                           Orientation=a.Orientation,
                           PublicImg=a.PublicImg,
                           Unit=a.Unit,
                           distince = get_distance(model.latitude, model.longitude, a.latitude, a.longitude),
                         
                       };
            Expression<Func<WrapHouseModel1, bool>> where = m => 1 == 1;
         
            if (model.City != 0)
            {
                where = where.And(m => m.City == model.City);
            }
            if (model.RoomId != null)
            {
                where = where.And(m => m.RoomId == model.RoomId);
            }
            if (model.radius != 0 && model.latitude != 0 && model.longitude != 0)
            {
                where = where.And(m => m.distince <= model.radius);
            }
            if (model.storeid != 0)
            {
                where = where.And(m => m.storeid == model.storeid);
            }
            if (model.ShiNumber != 0)
            {
                if (model.ShiNumber ==5)
                {
                    where = where.And(m => m.ShiNumber >= model.ShiNumber);
                }
                else
                {
                    where = where.And(m => m.ShiNumber == model.ShiNumber);
                }
                
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (model.ishaveimg != 0)
            {
                if (model.ishaveimg == 1)
                {
                    where = where.And(m => m.PublicImg != null);
                }
                if (model.ishaveimg == 2)
                {
                    where = where.And(m => m.PublicImg == null);
                }
            }
            if ((user.range == 2 || user.range == 3) && citys != null)
            {
                where = where.And(p => citys.Contains(p.CityName));
            }
            if (arr != null)
            {
                string Content = arr[0];
                if (arr.Length == 1)
                {
                    where = where.And(m => m.CellName.Contains(Content) || m.Adress.Contains(Content) || m.RoomId.Contains(Content) || m.housekeepername.Contains(Content));
                }
                else
                {
                    where = where.And(m => m.CellName.Contains(Content) || m.Adress.Contains(Content));
                    if (arr.Length == 2)
                    {
                        string CellName = arr[0];
                        string BuildingNumber = arr[1];
                        where = where.And(m => m.CellName.Contains(CellName) || m.Adress.Contains(CellName));
                        where = where.And(m => m.BuildingNumber.Contains(BuildingNumber));
                    }
                    if (arr.Length ==3)
                    {
                        string CellName = arr[0];
                        string BuildingNumber = arr[1];
                        string RoomId = arr[2];
                        where = where.And(m => m.CellName.Contains(CellName) || m.Adress.Contains(CellName));
                        where = where.And(m => m.BuildingNumber.Contains(BuildingNumber));
                        where = where.And(m => m.RoomId.Contains(RoomId));
                    }
                }
            }
            if (!string.IsNullOrEmpty(model.CityName) && model.CityName != "附近")
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            if (!string.IsNullOrEmpty(model.AreamName) && model.AreamName != "不限" && model.AreamName != "1千米" && model.AreamName != "2千米" && model.AreamName != "3千米")
            {
                where = where.And(m => m.AreamName == model.AreamName);
            }
            if (!string.IsNullOrEmpty(model.CellName))
            {
                where = where.And(m => m.CellName == model.CellName);
            }
            if (model.RecrntType != 0)
            {
                where = where.And(m => m.RecrntType == model.RecrntType);
            }
            if (model.Status != 0)
            {
                where = where.And(m => m.Status == model.Status);
            }
            if (!string.IsNullOrEmpty(model.Orientation))
            {
                where = where.And(m => m.Orientation == model.Orientation);
            }

            if (dtmo.dt != DateTime.MinValue||dtmo.dt2!=DateTime.MinValue)
            {
                where = where.And(m => DbFunctions.TruncateTime(m.Renttime)  >= dtmo.dt);
                where = where.And(m => DbFunctions.TruncateTime(m.Renttime) <= dtmo.dt2);
                where = where.And(m => m.Status == 1);
            }
            if (model.sign != 0)
            {
                where = where.And(m => m.sign == model.sign);
            }
            data = data.Where(where);
            model.GroupBy = true;
            IOrderByExpression<WrapHouseModel1> order = new OrderByExpression<WrapHouseModel1, long>(p => p.Id, model.GroupBy);
            if (model.radius != 0 && model.latitude != 0 && model.longitude != 0)
            {
                order = new OrderByExpression<WrapHouseModel1, double>(p => p.distince,false);
            }
            sysresult = this.QueryableForList<WrapHouseModel1>(data, orderablePagination, order);

            return sysresult;
        }
        //整租分组查询
        public List<WrapHouseModel1> Querylist2group(HouseModel model, dtmode dtmo, List<WrapCellName> listcell, string[] citys, T_SysUser user)
        {
            List<WrapHouseModel1> sysresult = new List<WrapHouseModel1>();
            var data = from a in BbHouse
                       join c1 in BbUser on a.HouseKeeper equals c1.Id
                       into temp2
                       from x1 in temp2.DefaultIfEmpty()

                       join c1 in (from cont in Contract where (cont.Status == 2 || cont.Status == 5) select cont) on a.Id equals c1.HouseId
                       into temp3
                       from x2 in temp3.DefaultIfEmpty()
                       join n in Teant on x2.TeantId equals n.Id
                       into temp4
                       from x3 in temp4.DefaultIfEmpty()
                       select new WrapHouseModel1()
                       {
                           constatus = x2 == null ? 0 : x2.Status,
                           teantname = x3.Name,
                           recent = x2 == null ? 0 : x2.Recent,
                           endtime = x2 == null ? DateTime.MinValue : DbFunctions.TruncateTime(x2.EndTime),

                           Id = a.Id,
                           BuildingNumber = a.BuildingNumber,
                           housekeepername = x1 == null ? "" : x1.RealName,
                           RoomId = a.RoomId,
                           storeid = a.storeid,
                           ShiNumber = a.ShiNumber,

                           CompanyId = a.CompanyId,
                           CellName = a.CellName,
                           Adress = a.Adress,
                           BusinessArea = a.BusinessArea,
                           CityName = a.CityName,
                           AreamName = a.AreamName,
                           RecrntType = a.RecrntType,
                           Measure = a.Measure,
                           Status = a.Status,
                           sign = a.sign,
                           Renttime = a.Renttime,
                           Orientation = a.Orientation,
                           PublicImg = a.PublicImg,
                           PublicTeshe=a.PublicTeshe,
                           Price=a.Price,
                           TingNumber = a.TingNumber,
                           WeiNumber = a.WeiNumber,
                           distince = get_distance(model.latitude, model.longitude, a.latitude, a.longitude)
                       };
            Expression<Func<WrapHouseModel1, bool>> where = m => 1 == 1;
           
            if (model.City != 0)
            {
                where = where.And(m => m.City == model.City);
            }
            if (model.RoomId != null)
            {
                where = where.And(m => m.RoomId == model.RoomId);
            }
            if (model.radius != 0 && model.latitude != 0 && model.longitude != 0)
            {
                where = where.And(m => m.distince <= model.radius);
            }
            if (model.storeid != 0)
            {
                where = where.And(m => m.storeid == model.storeid);
            }
            if (model.ShiNumber != 0)
            {
                if (model.ShiNumber == 5)
                {
                    where = where.And(m => m.ShiNumber >= model.ShiNumber);
                }
                else
                {
                    where = where.And(m => m.ShiNumber == model.ShiNumber);
                }
            }
           
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (model.ishaveimg != 0)
            {
                if (model.ishaveimg == 1)
                {
                    where = where.And(m => m.PublicImg != null);
                }
                if (model.ishaveimg == 2)
                {
                    where = where.And(m => m.PublicImg == null);
                }
            }
            if ((user.range == 2 || user.range == 3) && citys != null)
            {
                where = where.And(p => citys.Contains(p.CityName));
            }
            if (!string.IsNullOrEmpty(model.Content))
            {
                where = where.And(m => m.CellName.Contains(model.Content) || m.Adress.Contains(model.Content) || m.RoomId.Contains(model.Content) || m.housekeepername.Contains(model.Content));
            }
            if (!string.IsNullOrEmpty(model.CityName) && model.CityName != "附近")
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            if (!string.IsNullOrEmpty(model.AreamName) && model.AreamName != "不限" && model.AreamName != "1千米" && model.AreamName != "2千米" && model.AreamName != "3千米")
            {
                where = where.And(m => m.AreamName == model.AreamName);
            }
            if (!string.IsNullOrEmpty(model.CellName))
            {
                where = where.And(m => m.CellName == model.CellName);
            }
            if (model.RecrntType != 0)
            {
                where = where.And(m => m.RecrntType == model.RecrntType);
            }
            if (model.Status != 0)
            {
                where = where.And(m => m.Status == model.Status);
            }
            if (!string.IsNullOrEmpty(model.Orientation))
            {
                where = where.And(m => m.Orientation == model.Orientation);
            }
            if (dtmo.dt != DateTime.MinValue || dtmo.dt2 != DateTime.MinValue)
            {
                where = where.And(m => DbFunctions.TruncateTime(m.Renttime) >= dtmo.dt);
                where = where.And(m => DbFunctions.TruncateTime(m.Renttime) <= dtmo.dt2);
                where = where.And(m => m.Status == 1);
            }
            if (model.sign != 0)
            {
                where = where.And(m => m.sign == model.sign);
            }
            if (listcell != null)
            {
                where = where.And(m => m.CellName.Contains(m.CellName));
            }
            data = data.Where(where);
            IOrderByExpression<WrapHouseModel1> order = new OrderByExpression<WrapHouseModel1, long>(p => p.Id, model.GroupBy);
            if (model.radius != 0 && model.latitude != 0 && model.longitude != 0)
            {
                order = new OrderByExpression<WrapHouseModel1, double>(p => p.distince, false);
            }
            sysresult = data.ToList();

            return sysresult;
        }
        public List<houresources> Querylist1(houresources model, OrderablePagination orderablePagination)
        {
            var data = from mo in bhouresources
               select        mo;
            Expression<Func<houresources, bool>> where = m => 1 == 1;
            where = where.And(p => (p.Name.Replace("-", "").Contains(model.Name)));
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            data = data.Where(where);
            IOrderByExpression<houresources> order = new OrderByExpression<houresources, long>(p => p.Id, false);
            return QueryableForList(data, orderablePagination, order);
        }


        public List<HouseModel> Querylist4(List<long> ids)
        {
            var data = from mo in BbHouse where ids.Contains(mo.Id)
                       select mo;
            return data.ToList();
        }

        public List<HouseModel> Querylist3(HouseModel model, dtmode dtmo)
        {
            var data = from mo in BbHouse
                       select mo;
            Expression<Func<HouseModel, bool>> where = m => 1 == 1;
            IOrderByExpression<HouseModel> order = new OrderByExpression<HouseModel, long>(p => p.Id, false);
            if (model.Status != 0)
            {
                where = where.And(p => p.Status == model.Status);
            }
            if (model.RecrntType != 0)
            {
                where = where.And(p => p.RecrntType == model.RecrntType);
            }
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.City != 0)
            {
                where = where.And(m => m.City == model.City);
            }
            if (model.RoomId != null)
            {
                where = where.And(m => m.RoomId == model.RoomId);
            }
            if (dtmo.dt != DateTime.MinValue|| dtmo.dt2 != DateTime.MinValue )
            {
                data = from a in BbHouse where a.Renttime >= dtmo.dt && a.Renttime <= dtmo.dt2 && a.Status == 1 select a;
            }
            if (model.ShiNumber != 0)
            {
                where = where.And(m => m.ShiNumber == model.ShiNumber);
            }
            if (!string.IsNullOrEmpty(model.Content))
            {
                where = where.And(m => m.CellName.Contains(model.Content) || m.Adress.Contains(model.Content) || m.RoomId.Contains(model.Content));

            }
            if (!string.IsNullOrEmpty(model.CityName))
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            data = data.Where(where);
            return data.ToList();
        }
        public Stock Query(int housetype,long companyid)
        {
            DateTime datetime10 = DateTime.Now.AddDays(-10);
            DateTime datetime20 = DateTime.Now.AddDays(-20);
            DateTime datetime30 = DateTime.Now.AddDays(-30);
            Stock stoc = new Stock();
            stoc.ALL = (from m in BbHouse where m.RecrntType== housetype&&m.CompanyId==companyid select m).Count();
            stoc.Rent = (from m in BbHouse where m.Status==2&&m.RecrntType==housetype && m.CompanyId == companyid select m).Count();
            stoc.Book = (from m in BbHouse where m.Status == 3 && m.RecrntType == housetype && m.CompanyId == companyid select m).Count();
            stoc.Vacancy10 = (from m in BbHouse where m.Renttime<DateTime.Now&&m.Renttime> datetime10 && m.Status==0 && m.RecrntType == housetype && m.CompanyId == companyid select m).Count();
            stoc.Vacancy20 = (from m in BbHouse where m.Renttime < datetime10 && m.Renttime > datetime20 && m.Status == 0 && m.RecrntType == housetype && m.CompanyId == companyid select m).Count();
        
            stoc.Vacancy30 = (from m in BbHouse where m.Renttime < datetime20 && m.Renttime > datetime30 && m.Status == 0 && m.RecrntType == housetype && m.CompanyId == companyid select m).Count();
            stoc.Vacancyover30 = (from m in BbHouse where m.Renttime < datetime30 && m.Status == 0 && m.RecrntType == housetype && m.CompanyId == companyid select m).Count();
            stoc.Vacancy = stoc.ALL - stoc.Rent - stoc.Configuration;
            double a = (double)stoc.Vacancy / stoc.ALL;
            stoc.RentPert = a.ToString("0.0%");
            return stoc;
        }

        public HouseModel Queryhouse(HouseModel model,string[] citys, T_SysUser user)
        {
            var data = from m in BbHouse  select m;
            Expression<Func<HouseModel, bool>> where = m => 1 == 1;
            if (model.Electricid != null)
            {
                where = where.And(p => p.Electricid == model.Electricid);
            }
           
            
            if (model.Id != 0)
            {
                where = where.And(p => p.Id == model.Id);
            }
            if (model.CellName != null)
            {
                where = where.And(p => p.CellName == model.CellName);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(p => p.CompanyId == model.CompanyId);
            }
            if (citys != null&&(user.range == 2 || user.range == 3) )
            {
                where = where.And(p => citys.Contains(p.CityName));
            }
            data = data.Where(where);
            model = data.FirstOrDefault();
            return model;
        }
        
        public HouseQuery Queryhouse1(long id)
        {
            HouseQuery model = new HouseQuery();
            var data = from m in HouseQuery where m.Id == id select m;
           
            model = data.FirstOrDefault();
            return model;
        }
        public List<distributionHouseQuery> Queryhousepage(HouseModel model, OrderablePagination orderablePagination)
        {
            var data = from mo in bhouresources
                       join c1 in BbUser on mo.HouseKeeper equals c1.Id
                       into temp2
                       from x1 in temp2.DefaultIfEmpty()
                       select new distributionHouseQuery()
                       {
                           id = mo.Id,
                           HouseKeeper = mo.HouseKeeper,
                           Name = mo.Name,
                           RecentType = mo.HouseType,
                           AreaName = mo.AreaName,
                           CityName = mo.CityName,
                           Adress = mo.Adress,
                           CellName = mo.CellName,
                           CompanyId = mo.CompanyId,
                           HouseKeeperName = x1 == null ? "" : x1.RealName,
                           storeid = mo.storeid
                          
                       };
            Expression<Func<distributionHouseQuery, bool>> where = m => 1 == 1;
            if (model.CompanyId != 0)
            {
                where = where.And(p => p.CompanyId == model.CompanyId);
            }
            if (model.CityName !=null)
            {
                where = where.And(p => p.CityName == model.CityName);
            }
            if (model.AreamName != null)
            {
                where = where.And(p => p.AreaName == model.AreamName);
            }
            if (model.arrCellNames != null)
            {
                where = where.And(m => model.arrCellNames.Contains(m.CellName));
            }
            if (model.CellName != null)
            {
                where = where.And(p => p.CellName == model.CellName);
            }
            if (!string.IsNullOrEmpty(model.storename))
            {
                if (model.storename == "未分配")
                {
                    where = where.And(p => p.HouseKeeperName == null);
                }
                if (model.storename == "已分配")
                {
                    where = where.And(p => p.HouseKeeperName != null );
                }
            }
            if (!string.IsNullOrEmpty(model.Content))
            {
                where = where.And(p => p.CellName.Contains(model.Content) || p.storename.Contains(model.Content));
            }
            data = data.Where(where);
            IOrderByExpression<distributionHouseQuery> order = new OrderByExpression<distributionHouseQuery, long>(p => p.id, false);
            return QueryableForList(data, orderablePagination, order);
        }
        //查询所有部门房源
        public List<distributionHouseQuery> Queryhousedepart(HouseModel model, OrderablePagination orderablePagination)
        {
            var data = from mo in bhouresources
                       join c1 in department on mo.storeid equals c1.Id
                       into temp2
                       from x1 in temp2.DefaultIfEmpty()
                       select new distributionHouseQuery()
                       {
                           id = mo.Id,
                           HouseKeeper = mo.HouseKeeper,
                           Name = mo.Name,
                           RecentType = mo.HouseType,
                           AreaName = mo.AreaName,
                           CityName = mo.CityName,
                           Adress = mo.Adress,
                           CellName = mo.CellName,
                           CompanyId = mo.CompanyId,
                           storename = x1 == null ? "": x1.name,
                           storeid =mo.storeid
                       };
            Expression<Func<distributionHouseQuery, bool>> where = m => 1 == 1;
            if (model.CompanyId != 0)
            {
                where = where.And(p => p.CompanyId == model.CompanyId);
            }
            if (model.CityName != null)
            {
                where = where.And(p => p.CityName == model.CityName);
            }
            if (model.arrCellNames != null)
            {
                where = where.And(m => model.arrCellNames.Contains(m.CellName));
            }
            if (model.AreamName != null)
            {
                where = where.And(p => p.AreaName == model.AreamName);
            }
            if (model.CellName != null)
            {
                where = where.And(p => p.CellName == model.CellName);
            }
            if (!string.IsNullOrEmpty(model.storename))
            {
                if (model.storename == "未分配")
                {
                    where = where.And(p => p.storename==null|| p.storename =="");
                }
                if (model.storename == "已分配")
                {
                    where = where.And(p => p.storename != null);
                }
            }
            if (!string.IsNullOrEmpty(model.Content))
            {
                where = where.And(p => p.CellName.Contains(model.Content) || p.storename.Contains(model.Content));
            }
            data = data.Where(where);
            IOrderByExpression<distributionHouseQuery> order = new OrderByExpression<distributionHouseQuery, long>(p => p.id, false);
            return QueryableForList(data, orderablePagination, order);
        }
        //查询所有部门房源
        public List<WrapHouse> Queryhousecount(HouseModel model, OrderablePagination orderablePagination)
        {
            var data = from mo in bhouresources
                       group mo by new { mo.CellName,mo.CityName,mo.AreaName,mo.CompanyId,mo.HouseType }
                       into g
                       select new WrapHouse()
                       {
                           id = g.Key.CellName,
                           count = g.Count(),
                           CellName = g.Key.CellName,
                           CityName=g.Key.CityName,
                           AreaName=g.Key.AreaName,
                           CompanyId=g.Key.CompanyId,
                           RecentType = g.Key.HouseType
                       };
            Expression<Func<WrapHouse, bool>> where = m => 1 == 1;
            where = where.And(p => p.RecentType == 1|| p.RecentType == 2);
            if (model.CompanyId != 0)
            {
                where = where.And(p => p.CompanyId == model.CompanyId);
            }
            if (model.CityName != null)
            {
                where = where.And(p => p.CityName == model.CityName);
            }
            if (model.arrCellNames != null)
            {
                where = where.And(m => model.arrCellNames.Contains(m.CellName));
            }
            if (model.AreamName != null)
            {
                where = where.And(p => p.AreaName == model.AreamName);
            }
            if (model.CellName != null)
            {
                where = where.And(p => p.CellName == model.CellName);
            }
           
            if (!string.IsNullOrEmpty(model.Content))
            {
                where = where.And(p => p.CellName.Contains(model.Content));
            }
            data = data.Where(where);
            IOrderByExpression<WrapHouse> order = new OrderByExpression<WrapHouse, long>(p => p.count, false);
            return QueryableForList(data, orderablePagination, order);
        }
        //查询所有部门房源
        public List<long> Queryhousedepartcheck(HouseModel model)
        {
            var data = from mo in bhouresources  select mo;
            Expression<Func<houresources, bool>> where = m => 1 == 1;
            if (model.CompanyId != 0)
            {
                where = where.And(p => p.CompanyId == model.CompanyId);
            }
            if (model.HouseKeeper != 0)
            {
                where = where.And(p => p.HouseKeeper == model.HouseKeeper);
            }
            if (model.storeid != 0)
            {
                where = where.And(p => p.storeid == model.storeid);
            }
           
            data = data.Where(where);
            return data.Select(p=>p.Id).ToList();
        }
      
        public HouseLockQuery Queryhouse2(long id)
        {
            HouseLockQuery model = new HouseLockQuery();
            var data = from m in LockQuery where m.Id == id select m;

            model = data.FirstOrDefault();
            return model;
        }
        public List<HouseTip> Query(HouseModel model,OrderablePagination orderablePagination,List<t_department> list,T_SysRole role,long [] userids)
        {
            var data = from mo in HouseQuery

                       select new HouseTip()
                       {
                           Name = mo.Name,
                           HouseId = mo.Id,
                           HouseType = mo.RecentType,
                           Adress=mo.Adress,
                           CityName=mo.CityName,
                           AreaName=mo.AreaName,
                          Status=mo.Status,
                          CompanyId=mo.CompanyId,
                           storeid=mo.storeid,
                           HouseKeeper=mo.HouseKeeper
                       };
            Expression<Func<HouseTip, bool>> where = m => 1 == 1;
            //部门信息筛选
            if (list != null && role != null)
            {
                List<long> depentids = list.Select(p => p.Id).ToList();
                if (role.ishouse == 0)
                {
                    if (role.range == 2)
                    {
                        where = where.And(m => depentids.Contains(m.storeid));
                        if (userids != null && userids.Length > 0)
                        {
                            where = where.Or(m => userids.Contains(m.HouseKeeper));
                        }
                    }
                    if (role.range == 3)
                    {
                        where = where.And(m => m.HouseKeeper == role.userid);
                    }
                }
            }
            where = where.And(p => p.Name.Replace("-", "").Contains(model.CellName));
            if (model.CompanyId != 0)
            {
                where = where.And(p => p.CompanyId == model.CompanyId);
            }
            
            data = data.Where(where);
            IOrderByExpression<HouseTip> order = new OrderByExpression<HouseTip,long>(p => p.HouseId, false);
            return QueryableForList(data, orderablePagination,order);
        }
        public List<HouseTip> Query1(HouseModel model, OrderablePagination orderablePagination)
        {
            var data = from mo in HouseQuery
                       join  n in Contract on mo.Id equals n.HouseId
                       into temp from t in temp.DefaultIfEmpty()
                       select new HouseTip()
                       {
                           Name = mo.Name,
                           HouseId = mo.Id,
                           HouseType = mo.RecentType,
                           Adress = mo.Adress,
                           CityName = mo.CityName,
                           AreaName = mo.AreaName,
                           TeantId=t.Id,
                           Status=mo.Status,
                           isyccontract=mo.isyccontract
                       };
            Expression<Func<HouseTip, bool>> where = m => 1 == 1;
            where = where.And(p => p.Name.Replace("-", "").Contains(model.CellName));
            data = data.Where(where);
            IOrderByExpression<HouseTip> order = new OrderByExpression<HouseTip, long>(p => p.HouseId, false);
            return QueryableForList(data, orderablePagination, order);
        }
        //查询房源
        public List<HouseTip> Query3(HouseModel model, OrderablePagination orderablePagination)
        {
            var data = from mo in LockQuery
                       select new HouseTip()
                       {
                           Name = mo.Name,
                           HouseId = mo.Id,
                           HouseType = mo.RecentType,
                           Adress = mo.Adress,
                           CityName = mo.CityName,
                           AreaName = mo.AreaName,
                       };
            Expression<Func<HouseTip, bool>> where = m => 1 == 1;
            where = where.And(p => p.Name.Replace("-", "").Contains(model.CellName));
            data = data.Where(where);
            IOrderByExpression<HouseTip> order = new OrderByExpression<HouseTip, long>(p => p.HouseId, false);
            return QueryableForList(data, orderablePagination, order);
        }
        public HouseQuery Query2(HouseModel model)
        {
            var data = from mo in HouseQuery  select  mo;        
            Expression<Func<HouseQuery, bool>> where = m => 1 == 1;
            if (model.Electricid != null)
            {
                where = where.And(p => p.electricid == model.Electricid);
            }
            if (model.Id != 0)
            {
                where = where.And(p => p.Id == model.Id);
            }
            data = data.Where(where);
            return data.FirstOrDefault();
        }


        public List<HouseLockQuery> Query4(HouseModel model)
        {
            var data = (from mo in LockQuery select mo).AsNoTracking();
            Expression<Func<HouseLockQuery, bool>> where = m => 1 == 1;
            if (model.Electricid != null)
            {
                where = where.And(p => p.ElecId == model.Electricid);
            }
            if (model.Id != 0)
            {
                where = where.And(p => p.Id == model.Id);
            }
            data = data.Where(where);
            return data.ToList();
        }
        public List<T_V_HouseModel> Queryt_v(HouseModel model)
        {
            var data = from m in t_v_House  select m;
            if (model.RecrntType != 0)
            {
                data = data.Where(p => p.Renttype==model.RecrntType);
            }
            if (model.CellName != null)
            {
               data=data.Where(p => p.Name.Contains(model.CellName));
            }
            return data.ToList();
        }
        public long SaveHouse(HouseModel model)
        {
            model.CreateTime = DateTime.Now;
            model.Id = GetNextValNum("GET_WSEQUENCES('t_houresources')");
            AddModel(model);
            return model.Id;
        }
        public long SaveorUpdateHouse(HouseModel model)
        {
            if (model.Id == 0)
            {
               
                model.Renttime = DateTime.Now;
                model.CreateTime = DateTime.Now;
                model.Id = GetNextValNum("GET_WSEQUENCES('T_HOURESOURCES')");
                AddModel(model);
            }
            else
            {
               
                ModifiedModel(model,false,new string[] { "CreateTime", "LocalId", "Status", "Electricid", "uuid", "RecrntType", "Renttime", "CompanyId" });
            }
            return model.Id;
        }

        public long SaveorUpdateHouse1(HouseModel model,string[] param)
        {
            if (model.Id == 0)
            {
                model.CreateTime = DateTime.Now;
                model.Renttime = DateTime.Now;
                model.Id = GetNextValNum("GET_WSEQUENCES('T_HOURESOURCES')");
                AddModel(model);
            }
            else
            {
                ModifiedModel(model, false, param);
            }
            return model.Id;
        }


        public void SaveFangxing(List<Fxing> model, long houseid)
        {
            if (model != null)
            {
                foreach (var mo in model)
                {
                    mo.houseid = houseid;
                    if (mo.Id != 0 && mo.operation == 1)
                    {
                        PLDeleteModel<Fxing>(mo);
                    }
                    if (mo.Id != 0)
                    {
                        PLModifiedModel<Fxing>(mo, false);
                    }
                    else
                    {
                        mo.Id = GetNextValNum("GET_WSEQUENCES('T_FXING')");
                        PlAddModel<Fxing>(mo);
                    }
                    
                
                    
                }
            }
        }
        public long SaveOthers(List<peipei>  listpeibei, List<T_Tese> tese,int type)
        {
            int result = 0;
            if (listpeibei != null)
            {
                foreach (var mo in listpeibei)
                {
                    var cou = from m in tesedb where m.Name ==mo.Name&&m.Type==type&&mo.HouseId==mo.HouseId select m;
                    if (cou.Count() == 0)
                    {
                        mo.Type = type;
                        mo.Id = GetNextValNum("GET_WSEQUENCES('T_PEBEI')");
                        PlAddModel<peipei>(mo);
                    }
                }
            }
            if (tese != null)
            {
                foreach (var tesemo in tese)
                {
                    var cou = from m in tese where m.Name == tesemo.Name && m.Type == type && tesemo.HouseId == tesemo.HouseId select m;
                    if (cou.Count() == 0)
                    {
                        tesemo.Type = type;
                        tesemo.Id = GetNextValNum("GET_WSEQUENCES('T_TESE')");
                        PlAddModel<T_Tese>(tesemo);
                    }
                }
            }           
            result = this.SaveChanges();
            return result;
        }
        public int DeleteHouse(HouseModel model)
        {
            int count = (from m in BbHouse where m.Id == model.Id select m).Count();
            if (count == 0)
            {
                return 1;
            }
            else
            {
                return DeleteModel(model);
            }
        }
        //存储过程查询独栋房源数据
        public DataSet IndentHouseQuery(int mopagesize,int mopageindex,string cellname,int rgroup,long companyid, out long count)
        {
            OracleCommand cmd = new OracleCommand();
            OracleParameter pagesize = new OracleParameter("v_pagesize", OracleDbType.Int64);
            pagesize.Direction = ParameterDirection.Input;
            pagesize.Value = mopagesize;
            cmd.Parameters.Add(pagesize);
            OracleParameter pageindex = new OracleParameter("v_pagenow", OracleDbType.Int64);
            pageindex.Direction = ParameterDirection.Input;
            pageindex.Value = mopageindex;
            cmd.Parameters.Add(pageindex);


            OracleParameter paracellname = new OracleParameter("rcellname", OracleDbType.Varchar2);
            paracellname.Direction = ParameterDirection.Input;
            paracellname.Value = cellname;
            cmd.Parameters.Add(paracellname);

            OracleParameter parargroup = new OracleParameter("rgroup", OracleDbType.Int16);
            parargroup.Direction = ParameterDirection.Input;
            parargroup.Value = rgroup;
            cmd.Parameters.Add(parargroup);

            OracleParameter paracompanyid = new OracleParameter("rcompanyid", OracleDbType.Varchar2);
            paracompanyid.Direction = ParameterDirection.Input;
            paracompanyid.Value = companyid;
            cmd.Parameters.Add(paracompanyid);


            OracleParameter paramCode = new OracleParameter("Code", OracleDbType.Int16);
            paramCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramCode);
            OracleParameter paramMsg = new OracleParameter("Msg", OracleDbType.Varchar2, 2000);
            paramMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramMsg);
            OracleParameter paramcount = new OracleParameter("vcount", OracleDbType.Int64, 2000);
            paramcount.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramcount);
            OracleParameter paraCursor1 = new OracleParameter("o_cur1", OracleDbType.RefCursor);
            paraCursor1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor1);
            OracleParameter paraCursor2 = new OracleParameter("o_cur2", OracleDbType.RefCursor);
            paraCursor2.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor2);
            DataSet ds = SqlHelper.ExecuteDataSet("sp_getdepenthouse", cmd, "EntityDB", null);
            count= int.Parse(paramcount.Value.ToString());
            return ds;
        }
        //查询独栋房源
        public List<WrapIndentHouse> querydudong(HouseModel model)
        {
            var mo = from m in BbHouse   select new WrapIndentHouse() {Id=m.Id,
                Name =m.CellName,CityName=m.CityName,AreaName=m.AreamName,
                RecrntType=m.RecrntType,CompanyId=m.CompanyId} ;
            Expression<Func<WrapIndentHouse, bool>> where = m => 1 == 1;
            if (model.RecrntType != 0)
            {
                where = where.And(p => p.RecrntType ==3);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(p => p.CompanyId == model.CompanyId);
            }
            if (!string.IsNullOrEmpty(model.CityName))
            {
                where = where.And(p => p.CityName == model.CityName);
            }
            if (!string.IsNullOrEmpty(model.AreamName))
            {
                where = where.And(p => p.AreaName == model.AreamName);
            }
            mo = mo.Where(where);
            return mo.ToList();
        }
        
        public List<HouseQueryfgy> queryhouse(HouseModel model)
        {
            var mo = from m in HouseQuery1
                     select m;
            Expression<Func<HouseQueryfgy, bool>> where = m => 1 == 1;
            if (model.HouseKeeper != 0)
            {
                where = where.And(p => p.HouseKeeper == model.HouseKeeper);
            }
            mo = mo.Where(where);
            return mo.ToList();
        }
        //APP首页空置情况统计
        public Stock querykz(long companyid)
        {
            Stock result = new Stock();
            int[] arr = new int[] { 1, 2, 3 };
            var mo = from m in HouseQuery where arr.Contains(m.Status) && m.CompanyId==companyid
                     select m;
            var mo1 = from m in HouseQuery
                      where m.Status == 1 && m.CompanyId == companyid
                      select m;
            result.ALL = mo.Count();
            result.Vacancy= mo1.Count();
            double a = (double)result.Vacancy / result.ALL;
            result.RentPert = a.ToString("0.0%");
            return result;
        }
        //查询房型
        public List<Fxing> queryfx(long houseid)
        {
            var mo = from m in Fxing where m.houseid == houseid select m;
            return mo.ToList();
        }
        [DbFunctionAttribute("CodeFirstDatabaseSchema", "GETDISTANCE")]
        public static double get_distance(double curLat, double curLng, double srcLat, double srcLng)
        {
            throw new NotSupportedException();
        }
        public DbSet<HouseModel> BbHouse { get; set; }

        public DbSet<peipei> peibei { get; set; }
        public DbSet<T_Tese> tesedb { get; set; }
        public DbSet<T_V_HouseModel> t_v_House { get; set; }

        public DbSet<HouseQuery> HouseQuery { get; set; }
        public DbSet<HouseQueryfgy> HouseQuery1 { get; set; }
        public DbSet<T_Contrct> Contract { get; set; }

        public DbSet<HouseLockQuery> LockQuery { get; set; }

        public DbSet<houresources> bhouresources { get; set; }
        public DbSet<HousePendent> BbHousepent { get; set; }
        public DbSet<T_SysUser> BbUser { get; set; }
        public DbSet<T_Teant> Teant { get; set; }
        public DbSet<t_department> department { get; set; }
        public DbSet<Fxing> Fxing { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new FunctionsConvention("SYSTEM", this.GetType()));
            modelBuilder.Configurations.Add(new ContrctMapping());
            modelBuilder.Configurations.Add(new T_TeseMaping());
            modelBuilder.Configurations.Add(new T_peibeiMaping());
            modelBuilder.Configurations.Add(new HouseMapping());
            modelBuilder.Configurations.Add(new T_V_HouseMapping());
            modelBuilder.Configurations.Add(new HouseQueryMapping());
            modelBuilder.Configurations.Add(new HouseQueryMapping1());
            modelBuilder.Configurations.Add(new houresourcesMapping());
            modelBuilder.Configurations.Add(new HouseQueryLockMapping());
            modelBuilder.Configurations.Add(new HousedePentMaping());
            modelBuilder.Configurations.Add(new T_SysUserMapping());
            modelBuilder.Configurations.Add(new TenantMapping());
            modelBuilder.Configurations.Add(new departmentMapping());
            modelBuilder.Configurations.Add(new FxingMaping());
        }
    }
   
}
