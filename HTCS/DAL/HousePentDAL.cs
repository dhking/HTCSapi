using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Model;
using Model.House;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public   class HousePentDAL: RcsBaseDao
    {

        public SysResult<IList<WrapIndentHouse>> Querylist(HouseModel model, OrderablePagination orderablePagination)
        {
            SysResult<IList<WrapIndentHouse>> sysresult = new SysResult<IList<WrapIndentHouse>>();
            var data = from a in BbHouse join x in Floor on a.Id equals x.ParentId into temp from  t in temp.DefaultIfEmpty()  select new WrapIndentHouse()
            {
                Name=a.CellName,listfloor= temp.ToList()
            };
            Expression<Func<WrapIndentHouse, bool>> where = m => 1 == 1;
           
            data = data.Where(where);
            IOrderByExpression<WrapIndentHouse> order = new OrderByExpression<WrapIndentHouse, DateTime>(p => p.CreateTime, model.GroupBy);
            sysresult.numberData = QueryableForList(data, orderablePagination, order);
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
        public List<HousePendent> Querybyparentid(long id )
        {
            var data = from m in BbHousepent where m.ParentRoomid==id select m;
          
            return data.ToList();
        }

        public List<HousePendent> Querybyparentids(List<long> ids,int status)
        {
            var data = from m in BbHousepent where ids.Contains(m.ParentRoomid) select m;
            Expression<Func<HousePendent, bool>> where = m => 1 == 1;
            if (status != 0)
            {
                data = data.Where(p => p.Status == status);
            }

            return data.Where(where).ToList();
        }
        public long SaveorUpdateHouse(HousePendent model,string[] param)
        {
            if (model.ID == 0)
            {
                model.Status = 1;
                model.ID = GetNextValNum("GET_WSEQUENCES('T_HOURESOURCES_PENDENT')");
                AddModel(model);
            }
            else
            {
                
                ModifiedModel1(model, false,param);
            }
            return model.ID;
        }
        public HousePendent Querybyid(HousePendent model)
        {
            var data = from m in BbHousepent  select m;
            Expression<Func<HousePendent, bool>> where = m => 1 == 1;
            if (model.ParentRoomid != 0)
            {
                where = where.And(p => p.ParentRoomid == p.ParentRoomid);
            }
            if (model.ID != 0)
            {
                where = where.And(p => p.ID ==model.ID);
            }
            if (model.Electricid != null)
            {
                where = where.And(p => p.Electricid == model.Electricid);
            }
           
            data = data.Where(where);
            return data.FirstOrDefault();
        }

        public Stock StockQuery(int housetype)
        {
            DateTime datetime10 = DateTime.Now.AddDays(-10);
            DateTime datetime20 = DateTime.Now.AddDays(-20);
            DateTime datetime30 = DateTime.Now.AddDays(-30);
            Stock stoc = new Stock();
            var data = from m in BbHouse where m.RecrntType== housetype select  m;
           
            stoc.ALL = (from m in BbHousepent join n in data on m.ParentRoomid equals n.Id   select m).Count();
            stoc.Rent = (from m in BbHousepent join n in data on m.ParentRoomid equals n.Id
                        where m.Status==2 select m).Count();
            stoc.Configuration = (from m in BbHousepent
                                  join n in data on m.ParentRoomid equals n.Id
                                  where m.Status == 3
                                  select m).Count();
            stoc.Vacancy10 = (from m in BbHousepent
                              join n in data on m.ParentRoomid equals n.Id
                              where m.RecentTime < DateTime.Now && m.RecentTime > datetime10 && m.Status ==1 select m).Count();
            stoc.Vacancy20 = (from m in BbHousepent
                              join n in data on m.ParentRoomid equals n.Id
                              where m.RecentTime < datetime10 && m.RecentTime > datetime20 && m.Status == 1 select m).Count();
            stoc.Vacancy30 = (from m in BbHousepent
                              join n in data on m.ParentRoomid equals n.Id
                              where m.RecentTime < datetime20 && m.RecentTime > datetime30 && m.Status == 1 select m).Count();
            stoc.Vacancyover30 = (from m in BbHousepent
                                  join n in data on m.ParentRoomid equals n.Id
                                  where m.RecentTime < datetime30 && m.Status == 0 select m).Count();
            stoc.Vacancy = stoc.ALL - stoc.Rent - stoc.Configuration;
            double a = (double)stoc.Vacancy / stoc.ALL;
            stoc.RentPert = a.ToString("0.0%");
            return stoc;
        }
        public int SavePent(HousePendent model)
        {
            model.ID = GetNextValNum("GET_WSEQUENCES('t_houresources_pendent')");
            return AddModel(model);
        }
        public int autoSavePent(long parentid, int shinumber,long CompanyId)
        {
            int result = 0;
            for(int i = 0; i < shinumber; i++)
            {
                HousePendent model = new HousePendent();
                var housename = getzimu(i);
                model.Name = housename;
                model.RecentTime = DateTime.Now;
                model.CompanyId=
                model.Status = 1;
                model.sign = 4;
                model.ParentRoomid = parentid;
                model.ID =GetNextValNum("GET_WSEQUENCES('t_houresources_pendent')");
                result +=AddModel<HousePendent>(model);
            }
            this.SaveChanges();
            return result;
        }
        //查询是否楼层名称重复
        public int Maxfloor(T_Floor model)
        {
            var mo = from m in Floor where m.ParentId ==model.ParentId &&m.Floor== model.Floor select m;
            return mo.Count();

        }
        //查询楼层数量
        public int queryfloorcount(long parentid)
        {
            var mo = from m in Floor where m.ParentId == parentid select m;
            return mo.Count();

        }
        //查询公寓房间数量
        public int queryhousecount(long parentid)
        {
            var mo = from m in BbHousepent where m.ParentRoomid == parentid  select m;
            return mo.Count();

        }
        //查询楼层和房间数量
        public List<Floorco> queryfloor(HousePendent model)
        {
            var data = from m in BbHousepent where m.ParentRoomid == model.ParentRoomid group m by m.FloorId into g select new Floorco() { Floorcount=g.Count(),FloorId=g.Key };

            return data.ToList();
        }
        //查询楼层列表
        public List<T_Floor> queryfloorlist(HousePendent model)
        {
            var data = from m in Floor  select m;
            Expression<Func<T_Floor, bool>> where = m => 1 == 1;
            if (model.ParentRoomid != 0)
            {
                where = where.And(m => m.ParentId == model.ParentRoomid);
            }
            data = data.Where(where);
            return data.ToList();
        }
        //保存楼层
        public int autoFloor(long parentid, List<T_Floor> floors,params  string [] arr)
        {
            int result = 0;
            foreach (var mo in floors)
            {
                if (mo.Id == 0)
                {
                 
                    mo.ParentId = parentid;
                    mo.Id = GetNextValNum("GET_WSEQUENCES('T_FLOOR')");
                    result += AddModel<T_Floor>(mo);
                }
                else
                {
                    result += ModifiedModel<T_Floor>(mo,false, arr);
                }
               
            }
            return result;
        }
        //查询楼层和所属房间
        public List<HousePendent> Querylistpc( List<long> ids)
        {
            List<HousePendent> list = new List<HousePendent>();
            var data = from a in BbHousepent where ids.Contains(a.FloorId)
                       select a;
            list = data.ToList();
            return list;
        }
        //按照楼层和房间保存
        public int autoIndentSavePent(long parentid, List<T_Floor> floors,int HouseCount,long CompanyId)
        {
            int result = 0;
            foreach(var mo in floors)
            {
                for (int i = 0; i < HouseCount; i++)
                {
                    
                    HousePendent model = new HousePendent();
                    model.CompanyId = CompanyId;
                    var housename = ObjectExtension.ToHouseName(i, mo.Floor);
                    model.FloorId= mo.Id;
                    model.Name = housename;
                    model.RecentTime = DateTime.Now;
                    model.ParentRoomid = parentid;
                    model.Status = 1;
                    model.sign =4;
                    model.ID = GetNextValNum("GET_WSEQUENCES('t_houresources_pendent')");
                    result += AddModel<HousePendent>(model);
                }
            }
            return result;
        }
        //查询分页数据
        public List<HousePendent> Query(HousePendent model, OrderablePagination orderablePagination)
        {
            var data = from mo in BbHousepent select mo;  
            Expression<Func<HousePendent, bool>> where = m => 1 == 1;
            where = where.And(p => p.ParentRoomid == model.ParentRoomid);
            //楼层筛选条件
            where = where.And(p => p.FloorId == model.FloorId);
            data = data.Where(where);
            IOrderByExpression<HousePendent> order = new OrderByExpression<HousePendent, long>(p => p.ID, false);

            return QueryableForList(data, orderablePagination, order);
        }
        //不分页
        public List<HousePendent> Query1(HouseModel model)
        {
            var data = from mo in BbHousepent select mo;
            Expression<Func<HousePendent, bool>> where = m => 1 == 1;
            where = where.And(p => p.ParentRoomid == model.Id);
          
            data = data.Where(where);
          

            return data.ToList();
        }
        //查询统计数据

        public DataSet floorstatic(long restparameter, long floor)
        {
            OracleCommand cmd = new OracleCommand();
            OracleParameter pagesize = new OracleParameter("restparameter", OracleDbType.Varchar2);
            pagesize.Direction = ParameterDirection.Input;
            pagesize.Value = restparameter;
            cmd.Parameters.Add(pagesize);
            OracleParameter pageindex = new OracleParameter("floor", OracleDbType.Varchar2);
            pageindex.Direction = ParameterDirection.Input;
            pageindex.Value = floor;
            cmd.Parameters.Add(pageindex);
            OracleParameter paramCode = new OracleParameter("Code", OracleDbType.Int16);
            paramCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramCode);
            OracleParameter paramMsg = new OracleParameter("Msg", OracleDbType.Varchar2, 2000);
            paramMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramMsg);
            OracleParameter paraCursor1 = new OracleParameter("o_cur", OracleDbType.RefCursor);
            paraCursor1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor1);
            return SqlHelper.ExecuteDataSet("sp_gethousestatic", cmd, "EntityDB", null);
        }
        public int DeleteHouse(HousePendent model)
        {
            int count = (from m in BbHouse where m.Id==model.ID select m).Count();
            if (count == 0)
            {
                return 1;
            }
            else
            {
                return DeleteModel(model);
            }
        }
       
        public HousePendent addPent(long parentid)
        {
          
            HousePendent model = new HousePendent();
            int count = (from m in BbHousepent where m.ParentRoomid == parentid select m).Count();
            var housename = getzimu(count);
            model.Name = housename;
            model.ParentRoomid = parentid;
            model.ID = GetNextValNum("GET_WSEQUENCES('t_houresources_pendent')");
            model.Status = 1;
            if (AddModel<HousePendent>(model) > 0)
            {
                return model;
            }
            else
            {
                return null;
            }
        }
        public int saveeditPent(HousePendent model)
        {
            if (model.ID == 0)
            {
                model.Status = 1;
                model.ID = GetNextValNum("GET_WSEQUENCES('t_houresources_pendent')");
               return AddModel<HousePendent>(model);
            }
            else
            {
                return ModifiedModel<HousePendent>(model,false,new string[] { "ParentRoomid", "FloorId", "Status","RecentTime", "Status", "CompanyId" });
            }
        }

      
        public string getzimu(int index)
        {
            string[] str = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            if (index <= 24)
            {
                return str[index];
            }
            else
            {
                if (index < 10)
                {
                    return "00" + index.ToString();
                }
                if (index < 100)
                {
                    return "0" + index.ToString();
                }
                else
                {
                    return index.ToString();
                }
            }
        }
        //查询数量
        public int Querycount(HousePendent model)
        {
            var data = from m in BbHousepent select m;
            if (model.ParentRoomid != 0)
            {
                data = data.Where(p => p.ParentRoomid == model.ParentRoomid);
            }
            return data.Count();
        }
        public List<HousePendent> Housedepend(HousePendent model, OrderablePagination orderablePagination)
        {
            var data = from mo in BbHousepent
                       select mo;
            Expression<Func<HousePendent, bool>> where = m => 1 == 1;
            if (model.ParentRoomid != 0)
            {
                where = where.And(p => p.ParentRoomid == model.ParentRoomid);
            }
            if (model.isbind == 1)
            {
                where = where.And(p => string.IsNullOrEmpty(p.Electricid));
            }
            if (model.isbind == 2)
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Electricid));
            }
            data = data.Where(where);
            IOrderByExpression<HousePendent> order = new OrderByExpression<HousePendent, long>(p => p.ID, false);
            return QueryableForList(data, orderablePagination, order);
        }
        //楼层分页
        public List<T_Floor> follorlist(HouseModel model, dtmode dtmo, OrderablePagination orderablePagination,out long housecount)
        {
            var data = from mo in Floor  
                       select mo;
            var data1 = from mo in BbHousepent select mo;
            Expression<Func<T_Floor, bool>> where = m => 1 == 1;
            Expression<Func<HousePendent, bool>> where1 = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(p => p.ParentId == model.Id);
                where1 = where1.And(p => p.ParentRoomid == model.Id);
            }
            if (model.Status != 0 )
            {
                where1 = where1.And(m => m.Status == model.Status);
            }
            if (dtmo.dt != DateTime.MinValue)
            {
                where1 = where1.And(m => m.RecentTime >= dtmo.dt);
                where1 = where1.And(m => m.RecentTime <= dtmo.dt2);
            }
            if (model.sign != 0)
            {
                where1 = where1.And(m => m.sign == model.sign);
            }
            if (model.Huxing != null)
            {
                where1 = where1.And(m => m.Huxing == model.Huxing);
            }
            data = data.Where(where);
            data1 = data1.Where(where1);
            data = data.Where(p => data1.Select(m => m.FloorId).Contains(p.Id));
            housecount = data1.Count();
            IOrderByExpression<T_Floor> order = new OrderByExpression<T_Floor, long>(p => p.Id, false);
            return QueryableForList(data, orderablePagination, order);
        }
        
        public DbSet<HouseModel> BbHouse { get; set; }
        public DbSet<HousePendent> BbHousepent { get; set; }
        public DbSet<T_Floor> Floor { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new HouseMapping());
            modelBuilder.Configurations.Add(new HousedePentMaping());
            modelBuilder.Configurations.Add(new FloorMaping());
        }
    }

}
