using ControllerHelper;
using DAL;
using DBHelp;
using Model;
using Model.House;
using Model.User;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public   class IndentHouseService
    {
        HouseDAL dal = new HouseDAL();
        HousePentDAL pentdal = new HousePentDAL();
        HousePendent dentmodel = new HousePendent();
        public SysResult saveHouse(HouseModel model,T_SysUser user)
        {
            SysResult result = new SysResult();
            model.PublicPeibei = getPeibei(model.listpeibei);
            using (var tran = dal.Database.BeginTransaction())
            {
                long parentid = dal.SaveHouse(model);
                dal.SaveOthers(model.listpeibei, null, 2);
                pentdal.autoFloor(parentid, model.floor);
                int pentresult = pentdal.autoIndentSavePent(parentid, model.storeid, model.HouseKeeper,model.floor, model.ShiNumber,model.CompanyId);
                //添加日志
                RzService rzservice = new RzService();
                rzservice.addrz(model,parentid, user.Id, model.CompanyId);
                if (pentresult > 0)
                {
                    ProceService proce = new Service.ProceService();
                    proce.CmdProce3(new Pure() { Ids = model.HouseKeeper.ToStr(), Spname = "sp_departaddhouse", Other1 = model.CityName, Other = model.CellName, roperator = model.AreamName }, null, null);
                    //缓存操作
                    eidtredis(user, model);
                    tran.Commit();
                    result.Code = 0;
                    result.Message = "保存成功";
                }
                else
                {
                    tran.Rollback();
                    result.Code = -1;
                    result.Message = "保存失败";
                }
            }
            Thread t2 = new Thread(new ParameterizedThreadStart(AddCellName));
            t2.IsBackground = true;//设置为后台线程
            t2.Start(model);
            return result;
        }
        public SysResult eidtredis(T_SysUser user,HouseModel model)
        {
            SysResult result = new SysResult();
            if (string.IsNullOrEmpty(user.city))
            {
                user.city = user.city + model.CityName;
            }
            else
            {
                user.city = user.city + "," + model.CityName;
            }
            if (string.IsNullOrEmpty(user.area))
            {
                user.area = user.area + model.AreamName;
            }
            else
            {
                user.area = user.area + "," + model.AreamName;
            }
            if (string.IsNullOrEmpty(user.cellname))
            {
                user.cellname = user.cellname + model.CellName;
            }
            else
            {
                user.cellname = user.cellname + "," + model.CellName;
            }
            string access_token = DAL.Common.ConvertHelper.GetMd5HashStr(user.Id.ToStr());
            RedisHtcs rds = new RedisHtcs();
            string key = "sysuser_key" + access_token;
            rds.SetModel<T_SysUser>(key, user);
            return result;
        }
        //添加小区列表
        public static void AddCellName(object data)
        {
            LogService log = new LogService();
            T_CellName savemodel = new T_CellName();
            try
            {
                HouseModel model = (HouseModel)data;
                savemodel.Id = model.Id;
                savemodel.CompanyId = model.CompanyId;
                savemodel.CityName = model.CityName;
                savemodel.Type = model.RecrntType;
                savemodel.AreaName = model.AreamName;
                savemodel.Area = model.Area;
                savemodel.City = model.City;
                savemodel.Name = model.CellName;
                savemodel.Type = 3;
                savemodel.regtype = 3;
                ProceDAL dal = new ProceDAL();
                SysResult result = dal.CmdProce4(savemodel);
                if (result.Code != 0)
                {
                    log.LogError("新增小区异常" + result.Message + "数据:" + JsonConvert.SerializeObject(savemodel));
                }
            }
            catch (Exception ex)
            {
                log.LogError("新增小区异常" + ex.ToStr() + "数据:" + JsonConvert.SerializeObject(savemodel));
            }
        }
        //查询房源
        public SysResult<List<WrapIndentHouse>> Queryhouse(HouseModel model)
        {
            SysResult<List<WrapIndentHouse>> result = new SysResult<List<WrapIndentHouse>>();
            long count = 0;
           DataSet ds=dal.IndentHouseQuery(model.PageSize, model.PageIndex,model.CellName,model.Group,model.CompanyId,out count);
            List<WrapIndentHouse> list= EntityHelper.GetEntityListByDT<WrapIndentHouse>(ds.Tables[0],null);
            List<T_Floor> listfloor= EntityHelper.GetEntityListByDT<T_Floor>(ds.Tables[1], null);
            if (list != null)
            {
                foreach (var mo in list)
                {
                    mo.listfloor = listfloor.Where(p => p.ParentId == mo.Id).ToList();
                }
            }
           
            result.numberData = list;
            result.numberCount = count;
            return result;
        }
        //查询楼层和所属房间
        public SysResult<Wrapdudong> QueryPChouse(HouseModel model, OrderablePagination orderpaging, string[] citys, T_SysUser user,long[] userids)
        {
            Wrapdudong wrap = new Wrapdudong();
            HoseService hservice = new Service.HoseService();
            dtmode dtmo = hservice.gettuizutime(model.Idletime);
            SysResult<Wrapdudong> result = new SysResult<Wrapdudong>();
            long housecount = 0;
            //查询公寓
            HouseModel house = dal.Queryhouse(model, citys, user);
            if (house != null)
            {
                model.Id = house.Id;
                //楼层查询
                List<T_Floor> floor = pentdal.follorlist(model, dtmo, orderpaging, citys, user,userids, out housecount);
                //房间查询
                List<WrapHousePendent> listpent = pentdal.Querylistpc(floor.Select(p=>p.Id).ToList(),model, dtmo, citys,user,userids);
                foreach (var mo in floor)
                {
                    if (listpent != null)
                    {
                        mo.list = listpent.Where(p => p.FloorId == mo.Id).ToList();
                        if (mo.list != null)
                        {
                            mo.housecount = mo.list.Count;
                        }
                        //计算空置时间
                        long res = 0;
                        foreach (var mo1 in mo.list)
                        {
                            mo1.Idletime = (DateTime.Now - mo1.RecentTime).Days;
                            long.TryParse(mo1.Name, out res);
                            mo1.sort = res;
                        }
                        mo.list = mo.list.OrderBy(p => p.sort).ToList();
                    }
                }
                wrap.Name = house.CellName;
                wrap.Id = house.Id;
                wrap.floors = floor;
            }
            else
            {
                result.Code = 1;
                result.Message = "没有查询到数据";
            }
           
            result.numberData = wrap;
            result.other = housecount.ToStr();
            result.numberCount = orderpaging.TotalCount;
            return result;
        }
        /// <summary>
        /// 导出房源数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="orderpaging"></param>
        /// <returns></returns>
        public List<HousePendent> excelQueryPChouse(HouseModel model,string [] citys,T_SysUser users)
        {
            HoseService hservice = new Service.HoseService();
            dtmode dtmo = hservice.gettuizutime(model.Idletime);
            List<HousePendent> result = new List<HousePendent>();
            //查询公寓
            HouseModel house = dal.Queryhouse(model,citys,users);
            model.Id = house.Id;
            if (house != null)
            {
                //房间查询
                result = pentdal.Query1(house);
                //计算空置时间
                foreach (var mo1 in result)
                {
                    mo1.Idletime = (DateTime.Now - mo1.RecentTime).Days;
                    mo1.Cellname = house.CellName;
                    mo1.Adress = house.Adress;
                }
            }
            return result;
        }
        //筛选条件查询
        public SysResult<List<WrapIndentHouse>> Queryshaixuan()
        {
            SysResult<List<WrapIndentHouse>> result = new SysResult<List<WrapIndentHouse>>();
            List<WrapIndentHouse> list = dal.querydudong();
            List<T_Floor> listfloor = pentdal.queryfloorlist(new HousePendent() { });
            foreach (var mo in list)
            {
                mo.listfloor = listfloor.Where(p => p.ParentId == mo.Id).ToList();
            }
            result.numberData = list;
            return result;
        }
        //房型列表查询
        public SysResult<List<Fxing>> HouseFxing(HouseModel model)
        {
            SysResult<List<Fxing>> result = new SysResult<List<Fxing>>();
            HouseModel remo = new HouseModel();
            remo = dal.Queryhouse(model,null,null);
            List<Fxing> list = new List<Fxing>();
            if (remo != null)
            {
               list = pentdal.HouseFxing(model);
            }
            result.numberData = list;
            return result;
        }
        //查询公寓详情
        public SysResult<HouseModel> Queryhousebyid(HouseModel model)
        {
            SysResult<HouseModel> result = new SysResult<HouseModel>();
            HouseModel remo = new HouseModel();
            remo = dal.Queryhouse(model,null,null);
            remo.floorcount = pentdal.queryfloorcount(model.Id);
            remo.housecount = pentdal.queryhousecount(model.Id);
            result.numberData= remo;
            return result;
        }
        //编辑公区
        public SysResult Update(HouseModel model)
        {
            SysResult result = new SysResult();
            ProceDAL procedal = new ProceDAL();
            AddCellName(model);
            dal.SaveorUpdateHouse(model);
            result.Message = "编辑成功";
            return result;
        }
        //添加楼层
        public SysResult addfloor(T_Floor model)
        {
            IndentHouseDAL indentdal = new DAL.IndentHouseDAL();
            SysResult result = new SysResult();
            HousePentDAL pentdal = new HousePentDAL();
            //如果是添加的
            if (model.Id != 0)
            {
                if (pentdal.Maxfloor(model) > 0)
                {
                    return result = result.FailResult("楼层已存在");
                }
            }
            
            if (indentdal.addfloor(model,new string[] { "ParentId" }) > 0)
            {
                if (model.Id != 0)
                {
                    List<T_Floor> list = new List<T_Floor>();
                    list.Add(model);
                    pentdal.autoIndentSavePent(model.ParentId, 0, model.HouseKeeper, list, model.housecount,0);
                    result = result.SuccessResult("楼层添加成功");
                }
                else
                {
                  
                    result = result.SuccessResult("楼层编辑成功");
                }
               
            }
            return result;
        }
        //楼层查询
        public SysResult<List<T_Floor>> Queryfloorlist(HousePendent model)
        {
            SysResult<List<T_Floor>> result = new SysResult<List<T_Floor>>();
            List<Floorco> remo = new List<Floorco>();
            remo = pentdal.queryfloor(model);
            List<T_Floor> floor = new List<T_Floor>();
            floor = pentdal.queryfloorlist(model);
            foreach(var mo in floor)
            {
                Floorco fl = remo.Where(p => p.FloorId == mo.Id).FirstOrDefault();
                if (fl != null)
                {
                    mo.housecount = fl.Floorcount;
                }
            }
            result.numberData = floor;
            return result;
        }
        //根据公寓查询楼层
        public SysResult<List<T_Floor>> floorlist(HouseModel model)
        {
            SysResult<List<T_Floor>> result = new SysResult<List<T_Floor>>();
            HouseModel remo = new HouseModel();
            remo = dal.Queryhouse(model,null,null);
            List<T_Floor> list = new List<T_Floor>();
            if (remo != null)
            {
                list = pentdal.queryfloorlist(new HousePendent() { ParentRoomid = remo.Id });
            }
          
            result.numberData = list;
            return result;
        }
        //添加房间
        public SysResult<HousePendent> savehouse(HousePendent model)
        {
            string errmsg = "";
            string name = "";
            long roomid = 0;
            SysResult<HousePendent> result = new SysResult<HousePendent>();
            IndentHouseDAL dal = new IndentHouseDAL();
            HousePendent re = new HousePendent();
            if (dal.addhouse(model.ParentRoomid, model.FloorId, "",model.HouseKeeper, out errmsg,out roomid, out name))
            {
                re.ID = roomid;
                re.Name = name;
                result.numberData = re;
                result.Message= ("添加房间成功");
            }
            else
            {
                result.Message = "添加失败" + errmsg;
            }
            return result;
        }
        //删除楼层
        public SysResult deleteData(T_Floor ids)
        {
            SysResult result = new SysResult();
            ProceDAL dal = new ProceDAL();
            Pure pure = new Pure();
            pure.Id = ids.Id.ToStr();
            pure.Spname = "sp_deletefloor";
            result=dal.Cmdproce8(pure);
            return result;
        }
        //删除公寓
        public SysResult deletepart(HouseModel  model)
        {
            SysResult result = new SysResult();
            ProceDAL dal = new ProceDAL();
            Pure pure = new Pure();
            pure.Id = model.Id.ToStr();
            pure.Spname = "sp_deletehouseindent";
            dal.CmdProce(pure);
            return result;
        }

        //查询单间列表根据公寓和楼层
        public SysResult<wrapHousePendent> Querysinglehouse(HousePendent model, OrderablePagination orderpaging)
        {
            wrapHousePendent wrap = new wrapHousePendent();
            SysResult<wrapHousePendent> result = new SysResult<wrapHousePendent>();

            List<HousePendent> pent= pentdal.Query(model, orderpaging);
            if (pent != null)
            {
                foreach (var m in pent)
                {
                    m.Idletime = (DateTime.Now - m.RecentTime).Days;
                }
            }
            housesstatic sta = new housesstatic();
            DataSet ds = pentdal.floorstatic(model.ParentRoomid,model.FloorId);
            DataTable dt = ds.Tables[0];
           
            sta.count = long.Parse(dt.Rows[0]["V_COUNT"].ToStr());
            sta.Vacantcount = long.Parse(dt.Rows[0]["V_KZCOUNT"].ToStr());
            sta.rentedcount = long.Parse(dt.Rows[0]["V_ZUCOUNT"].ToStr());
            wrap.hstatic = sta;
            wrap.HousePendent = pent;
            result.numberData = wrap;
            return result;
        }
        public string getPeibei(List<peipei> modle)
        {
            if (modle == null)
            {
                return null;
            }
            string checkstr = "";
            foreach (var mo in modle)
            {
                if (mo.isCheck == 1)
                {
                    checkstr += mo.Name + ",";
                }
            }
            checkstr = checkstr.TrimEnd(","[0]);
            return checkstr;
        }
    }
}
