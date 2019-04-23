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

namespace DAL
{
    public class HouseDAL : RcsBaseDao
    {

        public IList<HouseModel>  Querylist(HouseModel model, dtmode dtmo, OrderablePagination orderablePagination,out long count)
        {
            IList<HouseModel> listhouse = new List<HouseModel>();
            var data = from a in BbHouse  select a;
            var data1 = from b in BbHousepent  select b;
            Expression<Func<HouseModel, bool>> where = m =>1 == 1;
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
                where = where.And(m => m.CellName.Contains(model.Content)||m.Adress.Contains(model.Content)||m.RoomId.Contains(model.Content));
            }
            if (!string.IsNullOrEmpty( model.CityName))
            {
                where = where.And(m => m.CityName == model.CityName);
            }
            if (!string.IsNullOrEmpty(model.AreamName))
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
            if (model.Status != 0 && model.RecrntType == 1)
            {
                where = where.And(m => m.Status==model.Status);
            }
            if (dtmo.dt != DateTime.MinValue && model.RecrntType == 1)
            {
                data = from a in BbHouse  where a.Renttime >= dtmo.dt && a.Renttime <= dtmo.dt2&&a.Status==1   select a;
            }
            if (model.Status != 0&&model.RecrntType!=1)
            {
                where1 = where1.And(m => m.Status == model.Status);
            }
            if (dtmo.dt != DateTime.MinValue && model.RecrntType !=1)
            {
                where1 = where1.And(m => m.RecentTime >= dtmo.dt);
                where1 = where1.And(m => m.RecentTime <= dtmo.dt2);
                where1 = where1.And(m => m.Status == 1);
            }
            if (model.sign!=0)
            {
                where1 = where1.And(m => m.sign == model.sign);
            }
           
            data1 = data1.Where(where1);
            where = where.And(m=>(data1.Select(p => p.ParentRoomid)).Contains(m.Id));
            data = data.Where(where);
            IOrderByExpression<HouseModel> order = new OrderByExpression<HouseModel, long>(p => p.Id, model.GroupBy);
            listhouse = this.QueryableForList<HouseModel>(data, orderablePagination, order);
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
            if (dtmo.dt != DateTime.MinValue && model.RecrntType == 1)
            {
                data = from a in BbHouse where a.Renttime >= dtmo.dt && a.Renttime <= dtmo.dt2 && a.Status == 1 select a;
            }
            if (model.Status != 0 && model.RecrntType != 1)
            {
                where1 = where1.And(m => m.Status == model.Status);
            }
            if (dtmo.dt != DateTime.MinValue && model.RecrntType != 1)
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
        public List<HouseModel> Querylist2(HouseModel model, dtmode dtmo, OrderablePagination orderablePagination)
        {
            List<HouseModel> sysresult = new List<HouseModel>();
            var data = from a in BbHouse select a;
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
            if (dtmo.dt != DateTime.MinValue && model.RecrntType == 1)
            {
                data = from a in BbHouse where a.Renttime >= dtmo.dt && a.Renttime <= dtmo.dt2 && a.Status == 1 select a;
            }
            if (model.Status != 0 && model.RecrntType != 1)
            {
                where1 = where1.And(m => m.Status == model.Status);
            }
            if (dtmo.dt != DateTime.MinValue && model.RecrntType != 1)
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
            IOrderByExpression<HouseModel> order = new OrderByExpression<HouseModel, long>(p => p.Id, model.GroupBy);
            sysresult = this.QueryableForList<HouseModel>(data, orderablePagination, order);

            return sysresult;
        }
        public List<houresources> Querylist1(houresources model, OrderablePagination orderablePagination)
        {
            var data = from mo in bhouresources
               select        mo;
            Expression<Func<houresources, bool>> where = m => 1 == 1;
            where = where.And(p => p.Name.Replace("-", "").Contains(model.Name));
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
            if (dtmo.dt != DateTime.MinValue)
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
        public Stock Query(int housetype)
        {
            DateTime datetime10 = DateTime.Now.AddDays(-10);
            DateTime datetime20 = DateTime.Now.AddDays(-20);
            DateTime datetime30 = DateTime.Now.AddDays(-30);
            Stock stoc = new Stock();
            stoc.ALL = (from m in BbHouse where m.RecrntType== housetype select m).Count();
            stoc.Rent = (from m in BbHouse where m.Status==2&&m.RecrntType==housetype select m).Count();
            stoc.Book = (from m in BbHouse where m.Status == 3 && m.RecrntType == housetype select m).Count();
            stoc.Vacancy10 = (from m in BbHouse where m.Renttime<DateTime.Now&&m.Renttime> datetime10 && m.Status==0 && m.RecrntType == housetype select m).Count();
            stoc.Vacancy20 = (from m in BbHouse where m.Renttime < datetime10 && m.Renttime > datetime20 && m.Status == 0 && m.RecrntType == housetype select m).Count();
        
            stoc.Vacancy30 = (from m in BbHouse where m.Renttime < datetime20 && m.Renttime > datetime30 && m.Status == 0 && m.RecrntType == housetype select m).Count();
            stoc.Vacancyover30 = (from m in BbHouse where m.Renttime < datetime30 && m.Status == 0 && m.RecrntType == housetype select m).Count();
            stoc.Vacancy = stoc.ALL - stoc.Rent - stoc.Configuration;
            double a = (double)stoc.Vacancy / stoc.ALL;
            stoc.RentPert = a.ToString("0.0%");
            return stoc;
        }

        public HouseModel Queryhouse(HouseModel model)
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
        public HouseLockQuery Queryhouse2(long id)
        {
            HouseLockQuery model = new HouseLockQuery();
            var data = from m in LockQuery where m.Id == id select m;

            model = data.FirstOrDefault();
            return model;
        }
        public List<HouseTip> Query(HouseModel model,OrderablePagination orderablePagination)
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
                          Status=mo.Status
                       };
            Expression<Func<HouseTip, bool>> where = m => 1 == 1;
            where = where.And(p => p.Name.Replace("-", "").Contains(model.CellName));
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
                           Status=mo.Status
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


        public List<HouseQuery> Query4(HouseModel model)
        {
            var data = (from mo in HouseQuery select mo).AsNoTracking();
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
                model.Id = GetNextValNum("GET_WSEQUENCES('T_HOURESOURCES')");
                AddModel(model);
            }
            else
            {
                ModifiedModel1(model, false, param);
            }
            return model.Id;
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
        public DataSet IndentHouseQuery(int mopagesize,int mopageindex,string cellname,int rgroup,out long count)
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
        public List<WrapIndentHouse> querydudong()
        {
            var mo = from m in BbHouse where m.RecrntType==3 select new WrapIndentHouse() {Id=m.Id,Name=m.CellName} ;
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
        public Stock querykz()
        {
            Stock result = new Stock();
            int[] arr = new int[] { 1, 2, 3 };
            var mo = from m in HouseQuery where arr.Contains(m.Status) 
                     select m;
            var mo1 = from m in HouseQuery
                      where m.Status == 1
                     select m;
            result.ALL = mo.Count();
            result.Vacancy= mo1.Count();
            double a = (double)result.Vacancy / result.ALL;
            result.RentPert = a.ToString("0.0%");
            return result;
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
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {

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
        }
    }
}
