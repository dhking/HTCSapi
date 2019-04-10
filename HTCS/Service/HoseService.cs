using ControllerHelper;
using DAL;
using DAL.Common;
using DBHelp;
using Model;
using Model.Base;
using Model.House;
using Model.TENANT;
using Model.User;
using Newtonsoft.Json;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class HoseService
    {
        HouseDAL dal = new HouseDAL();
        HousePentDAL pentdal = new HousePentDAL();
        HousePendent dentmodel = new HousePendent();

        public dtmode gettuizutime(long Idletime)
        {
            dtmode dtmo = new dtmode();
            DateTime dt = DateTime.MinValue;
            if (Idletime==1)
            {
                dtmo.dt = DateTime.Now.Date.AddDays(0 - 10);
                dtmo.dt2 = DateTime.Now.Date;
            }
            if (Idletime == 2)
            {
                dtmo.dt = DateTime.Now.Date.AddDays(0 - 20);
                dtmo.dt2 = DateTime.Now.Date.AddDays(0 - 11);
            }
            if (Idletime == 3)
            {
                dtmo.dt = DateTime.Now.Date.AddDays(0 - 30);
                dtmo.dt2 = DateTime.Now.Date.AddDays(0 - 21);
            }
            if (Idletime == 4)
            {
                dtmo.dt = DateTime.MinValue;
                dtmo.dt2 = DateTime.Now.Date.AddDays(0 - 30);
            }
            return dtmo;
        }
        public SysResult<IList<WrapHouseModel>> Queryhouse(HouseModel model, OrderablePagination orderablePagination,T_SysUser user)
        {
            SysResult<IList<WrapHouseModel>> result = new SysResult<IList<WrapHouseModel>>();
            IList<WrapHouseModel> listwrap = new List<WrapHouseModel>();
            IList<HousePendent> listpent = new List<HousePendent>();
            IList<HouseModel> listhouse = new List<HouseModel>();
            long housecount = 0;
            dtmode dtmo = gettuizutime(model.Idletime);
            listhouse = dal.Querylist(model, dtmo, orderablePagination,out housecount);
            listpent = pentdal.Querybyparentids(listhouse.Select(p=>p.Id).ToList(),0);
            if (listhouse.Count > 0)
            {
                foreach (var mo in listhouse)
                {
                    WrapHouseModel wrap = new WrapHouseModel();
                    wrap.house = mo;
                    wrap.house.Idletime = (DateTime.Now-mo.Renttime).Days;
                    wrap.housependent = listpent.Where(p => p.ParentRoomid == mo.Id).ToList();
                    if (wrap.housependent != null)
                    {
                        foreach(var m in wrap.housependent)
                        {
                            m.Idletime = (DateTime.Now - mo.Renttime).Days;
                        }
                    }
                    listwrap.Add(wrap);
                }
            }
            result.numberData = listwrap;
            //间数
            result.other = housecount.ToStr();
            //套数
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        //查询导出合租数据
        public SysResult<List<HousePendent>> excelQueryhouse(HouseModel model, T_SysUser user)
        {
            SysResult<List<HousePendent>> result = new SysResult<List<HousePendent>>();
            List<HousePendent> listpent = new List<HousePendent>();
            IList<HouseModel> listhouse = new List<HouseModel>();
            dtmode dtmo = gettuizutime(model.Idletime);
            listhouse = dal.excelQuerylist(model, dtmo);
            listpent = pentdal.Querybyparentids(listhouse.Select(p => p.Id).ToList(), 0);
            if (listpent.Count > 0)
            {
                foreach (var mo in listpent)
                {
                    HouseModel hmodel = listhouse.Where(p => p.Id == mo.ParentRoomid).FirstOrDefault();
                    if (hmodel != null)
                    {
                        mo.Adress = hmodel.CellName + "-" + hmodel.BuildingNumber + "号" + hmodel.RoomId + "室" + mo.Name;
                        mo.Cellname = hmodel.CellName;
                    }
                }
            }
            result.numberData = listpent;
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
               
                savemodel.CityName = model.CityName;
                savemodel.Type = model.RecrntType;
                savemodel.AreaName = model.AreamName;
                savemodel.Area = model.Area;
                savemodel.City = model.City;
                savemodel.CompanyId = model.CompanyId;
                savemodel.Name = model.CellName;
                savemodel.regtype = 3;
                ProceDAL dal = new ProceDAL();
                SysResult result = dal.CmdProce4(savemodel);
                if (result.Code != 0)
                {
                    log.LogError("新增小区异常" + result.Message + "数据:" + JsonConvert.SerializeObject(savemodel));
                }
            }
            catch(Exception ex)
            {
                log.LogError("新增小区异常" + ex.ToStr()+"数据:"+ JsonConvert.SerializeObject(savemodel));
            }
        }
        public SysResult<List<HouseTip>> Querytip(ParaTip model, OrderablePagination orderablePagination)
        {
            SysResult<List<HouseTip>> result = new SysResult<List<HouseTip>>();
            HouseModel para = new HouseModel();
            List<HouseTip> listtip = new List<HouseTip>();
            para.CellName = model.Name;
            listtip = dal.Query(para, orderablePagination);
            foreach (var mo in listtip)
            {
                mo.Name = mo.Name + "("+ getstatusstr(mo.Status)+")";
            }
            result.numberData = listtip;
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        public SysResult<List<houresources>> HouseQuery(houresources model, OrderablePagination orderablePagination)
        {
            SysResult<List<houresources>> result = new SysResult<List<houresources>>();
            

            result.numberData = dal.Querylist1(model, orderablePagination); 
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        //整租房源
        public SysResult<List<HouseModel>> zzHouseQuery(HouseModel model, OrderablePagination orderablePagination)
        {
            SysResult<List<HouseModel>> result = new SysResult<List<HouseModel>>();
            model.RecrntType = 1;
            dtmode dtmo = gettuizutime(model.Idletime);
            List<HouseModel> hlist = dal.Querylist2(model, dtmo, orderablePagination);
            foreach(var mo in hlist)
            {
                mo.Idletime = (DateTime.Now - mo.Renttime).Days;
            }
            result.numberData = hlist;
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        //导出整租房源数据
        public SysResult<List<HouseModel>> zzHouseQuery1(HouseModel model)
        {
            SysResult<List<HouseModel>> result = new SysResult<List<HouseModel>>();
            model.RecrntType = 1;
            dtmode dtmo = gettuizutime(model.Idletime);
            List<HouseModel> hlist = dal.Querylist3(model, dtmo);
            foreach (var mo in hlist)
            {
                mo.Idletime = (DateTime.Now - mo.Renttime).Days;
            }
            result.numberData = hlist;
            return result;
        }
        public SysResult<List<HousePendent>> Housedepend(HousePendent model, OrderablePagination orderablePagination)
        {
            SysResult<List<HousePendent>> result = new SysResult<List<HousePendent>>();
            //根据电表序号查询房间编号
            if (model.Electricid == null)
            {
                result.Code = 1;
                result.Message = "请选择主表";
                return result;
            }
           
            HouseModel remodel = dal.Queryhouse(new HouseModel() { Electricid=model.Electricid});
            if (remodel == null)
            {
               HousePendent  pent = pentdal.Querybyid(model);
               
               if (pent != null)
               {
                    HouseModel housemode = dal.Queryhouse(new HouseModel() { Id = pent.ParentRoomid });
                    if (housemode.RecrntType == 3)
                    {
                        List<HousePendent> list = new List<HousePendent>();
                        list.Add(pent);
                        result.numberData = list;
                        result.numberCount = 1;
                        return result;
                    }
                    model.ParentRoomid = pent.ParentRoomid;
               }
            }
            else
            {
                model.ParentRoomid = remodel.Id;
            }
            if (model.ParentRoomid == 0)
            {
                result.Code = 1;
                result.Message = "请先绑定房源";
                return result;
            }
            result.numberData = pentdal.Housedepend(model, orderablePagination);
            
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        public string getstatusstr(int status)
        {
            if (status == 1)
            {
                return "未租";
            }
            if (status == 2)
            {
                return "已租";
            }
            if (status == 3)
            {
                return "装修中";
            }
            if (status == 4)
            {
                return "作废";
            }
            return "未知";
        }
        
        //房源提示信息升级版
        public SysResult<List<HouseTip>> Querytip1(ParaTip model, OrderablePagination orderablePagination)
        {
            ContrctDAL contractdal = new ContrctDAL();
            SysResult<List<HouseTip>> result = new SysResult<List<HouseTip>>();
            HouseModel para = new HouseModel();
            List<HouseTip> listtip = new List<HouseTip>();
            para.CellName = model.Name;
            listtip = dal.Query1(para, orderablePagination);
            List<long?> arr = listtip.Select(p => p.TeantId).ToList();
            List<T_Teant> listteant= contractdal.Queryteant(arr);
            foreach (var mo in listtip)
            {
                mo.Name = mo.Name + "(" + getstatusstr(mo.Status) + ")";
                T_Teant temo = listteant.Where(p => p.Id == mo.TeantId).FirstOrDefault();
                if (temo != null)
                {
                    mo.UserName = temo.Name;
                    mo.UserPhone = temo.Phone.ToStr();
                }
            }
            result.numberData = listtip;
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        //查询门锁信息
        public SysResult<List<HouseTip>> Querytip2(ParaTip model, OrderablePagination orderablePagination)
        {
            ContrctDAL contractdal = new ContrctDAL();
            SysResult<List<HouseTip>> result = new SysResult<List<HouseTip>>();
            HouseModel para = new HouseModel();
            List<HouseTip> listtip = new List<HouseTip>();
            para.CellName = model.Name;
            listtip = dal.Query3(para, orderablePagination);
            result.numberData = listtip;
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        public SysResult Volite(HouseModel model)
        {
            SysResult result = new SysResult();
            if (model.CityName == null)
            {
                result.Code = 1;
                result.Message = "请选择城市";
            }
            if (model.AreamName == null)
            {
                result.Code = 1;
                result.Message = "请选择区";
            }
            if (model.CellName == null&&model.RecrntType==3)
            {
                result.Code = 1;
                result.Message = "请填写公寓名称";
            }
            if (model.CellName == null && model.RecrntType != 3)
            {
                result.Code = 1;
                result.Message = "请填写小区名称";
            }
            return result;
        }
        public SysResult<List<HousePendent>> saveHouse(HouseModel model,long userid)
        {
            SysResult<List<HousePendent>> result = new SysResult<List<HousePendent>>();
            SysResult voliteresult = Volite(model);
            long type = model.Id;
            
            if (voliteresult.Code != 0)
            {
                result.Code = 1;
                result.Message = voliteresult.Message;
                return result;
            }
            if (model.Id == 0)
            {
                model.sign = 4;
            }
            long parentid = dal.SaveorUpdateHouse(model);
            if (type == 0)
            {
                int pentresult = pentdal.autoSavePent(parentid, model.ShiNumber, model.CompanyId);
                if (pentresult > 0)
                {
                    List<HousePendent> pent = pentdal.Querybyparentid(parentid);
                    result.numberData = pent;
                    //生成装修单
                    if (model.Status == 3)
                    {
                        RenovationDAL rendal = new RenovationDAL();
                        rendal.Save(new Renovation() { budget = model.budget, term = model.term, createperson = "" });
                    }
                    result.Code = 0;
                    result.Message = "保存成功";
                }
                else
                {

                    result.Code = -1;
                    result.Message = "保存失败";
                }
            }
            //添加操作日志
            RzService reservice = new RzService();
            reservice.addrz(model, type, userid, parentid);
            Thread t2 = new Thread(new ParameterizedThreadStart(AddCellName));
            t2.IsBackground = true;//设置为后台线程
            t2.Start(model);
            return result;

        }
        
        public SysResult<HouseModel> queryhouse(HouseModel model)
        {
            SysResult<HouseModel> result = new SysResult<HouseModel>();
            BaseDataDALL basedal = new BaseDataDALL();
            List<T_Basics> listbase = basedal.Querylist1(null);
            List<peipei> listpeibei = new List<peipei>();

            List<T_Tese> listtese = new List<T_Tese>();
            HouseModel remo = dal.Queryhouse(model);
            string[] arr = new string[] { };
            string[] arr1 = new string[] { };
           
            if (remo.PublicPeibei != null)
            {
                 arr = remo.PublicPeibei.Split(","[0]);
            }
            if (remo.PublicTeshe != null)
            {
                arr1 = remo.PublicTeshe.Split(","[0]);
            }
            foreach (var mo in listbase.Where(p => p.ParaType == "publicpeibei"))
            {
                peipei pe = new peipei();
                pe.Name = mo.Name;
                pe.isCheck = 0;
                if (arr.Contains(mo.Name))
                {
                    pe.isCheck = 1;
                }
                listpeibei.Add(pe);
            }
            foreach (var mo in listbase.Where(p => p.ParaType == "puclictese"))
            {
                T_Tese pe = new T_Tese();
                pe.Name = mo.Name;
                pe.IsCheck = 0;
                if (arr1.Contains(mo.Name))
                {
                    pe.IsCheck = 1;
                }
                listtese.Add(pe);
            }
            remo.listpeibei=listpeibei;
            remo.listtese = listtese;
            //查询房管员姓名和电话
            if (remo.HouseKeeper != 0)
            {
                UserDAL1 udal = new UserDAL1();
                T_SysUser user = udal.QueryUerbyid(new T_SysUser() { Id = remo.HouseKeeper });
                if (user != null)
                {
                    remo.housekeepername = user.RealName;
                    remo.housekeeperphone = user.Mobile;
                }
            }
            //查询门店信息
            if (remo.storeid != 0)
            {
                RoleDAL dal = new RoleDAL();
                T_CellName cell = dal.storeQueryid(new T_CellName() { Id = remo.storeid });
                if (cell != null)
                {
                    remo.store = cell.Name;
                }
            }
            result.numberData = remo;
            return result;
        }
        public string getPeibei(List<peipei> modle)
        {
            if (modle == null)
            {
                return null;
            }
            string checkstr = "";
            foreach(var mo in modle)
            {
                if (mo.isCheck == 1)
                {
                    checkstr += mo.Name + ",";
                }
            }
            checkstr = checkstr.TrimEnd(","[0]);
            return checkstr;
        }
        public SysResult<HousePendent> addHouse(HouseModel model)
        {
            SysResult<HousePendent> result = new SysResult<HousePendent>();
            HousePendent hou = pentdal.addPent(model.Id);
            if (hou != null)
            {
                result.numberData = hou;
                result.Code = 0;
                result.Message = "添加成功";
            }
            else
            {
                result.Code = 1;
                result.Message = "添加失败";
            }
            return result;
        }
        public SysResult saveeditHouse(HousePendent model,long userid)
        {
            SysResult result = new SysResult();
            //查询原房源数据
            HousePendent pent = new HousePendent();
            HousePentDAL housepent = new HousePentDAL();
            pent= housepent.Querybyid(model);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Price", model.Price.ToStr());
            dic.Add("yPrice", pent.Price.ToStr());
            dic.Add("HouseKeeper", model.HouseKeeper.ToStr());
            dic.Add("yHouseKeeper", pent.HouseKeeper.ToStr());
            dic.Add("yAdress", pent.Adress.ToStr());
            dic.Add("Adress", model.Adress.ToStr());
            dal.SaveOthers(model.listpeibei, model.listtese,1);
            if (pentdal.saveeditPent(model)>0)
            {
                RzService reservice = new RzService();
                reservice.addrz(new HouseModel() { CompanyId = model.CompanyId, Price = model.Price }, 2, userid, model.ID);
                result.Code = 0;
                result.Message = "保存成功";
            }
            else
            {
                result.Code = 1;
                result.Message = "保存失败";
            }
            return result;
        }
        //删除房间
        public SysResult deleteData(HousePendent ids)
        {
            SysResult result = new SysResult();
            if (ids.ParentRoomid == 0)
            {
                return result = result.FailResult("请输入房源编号");
            }
            ContrctDAL contract = new ContrctDAL();
            
            //判断在租房间
            int count = contract.Querycount(new Model.Contrct.T_Contrct() {HouseId=ids.ID, Status = 7 });
            if (count > 0)
            {
                return result = result.FailResult("删除失败,当前有在租合同请先退租");
            }
            if (pentdal.DeleteHouse(ids) > 0)
            {
                int othercount = pentdal.Querycount(new HousePendent() { ParentRoomid = ids.ParentRoomid });
                if (othercount == 0)
                {
                    dal.DeleteHouse(new HouseModel() { Id = ids.ParentRoomid });
                }
                return result = result.SuccessResult("删除成功");
            }
            else
            {
                return result = result.FailResult("删除失败");
            }
        }
        //删除整套房间
        public SysResult deleteData(HouseModel ids)
        {

            SysResult result = new SysResult();
            ProceDAL dal = new ProceDAL();
            Pure pure = new Pure();
            pure.Id = ids.Id.ToStr();
            pure.Spname = "sp_deletehouse";
            result=dal.Cmdproce8(pure);
            return result;
        }
        public SysResult sp_deleteallhouse(HouseModel ids)
        {
            SysResult result = new SysResult();
            ProceDAL dal = new ProceDAL();
            Pure pure = new Pure();
            pure.Id = ids.Id.ToStr();
            pure.Spname = "sp_deleteallhouse";
            result = dal.Cmdproce8(pure);
            return result;
        }
        
        //查询房间
        public SysResult<HousePendent> Querydepent(HousePendent ids)
        {
            BaseDataService basedal = new BaseDataService();
            SysResult<HousePendent> resut = new SysResult<HousePendent>();
            HousePendent pent = pentdal.Querybyid(ids);
            HouseModel model = dal.Queryhouse(new HouseModel() { Id= pent.ParentRoomid } );
            List<peipei> listpeibei = new List<peipei>();
            List<T_Tese> listtese = new List<T_Tese>();
            WrapBasic wrap = basedal.Query(new Queryparam() { teseorpeibei = 0, type = 2 }).numberData;
            string[] arr = new string[] { };
            string[] arr1 = new string[] { };
            if (pent == null || model == null)
            {
                resut.Code = 1;
                resut.Message = "未找到房源数据";
                return resut;
            }
            if (model != null)
            {
                pent.Adress = model.Adress;
            }
            if (pent.PrivateTeshe != null)
            {
                arr = pent.PrivateTeshe.Split(","[0]);
            }
            if (pent.PrivatePeibei != null)
            {
                arr1 = pent.PrivatePeibei.Split(","[0]);
            }
            foreach (var mo in wrap.peipei)
            {
                peipei pe = new peipei();
                pe.Name = mo.Name;
                pe.isCheck = 0;
                if (arr1.Contains(mo.Name))
                {
                    pe.isCheck = 1;
                }
                listpeibei.Add(pe);
            }
            foreach (var mo in wrap.tese)
            {
                T_Tese pe = new T_Tese();
                pe.Name = mo.Name;
                pe.IsCheck = 0;
                if (arr.Contains(mo.Name))
                {
                    pe.IsCheck = 1;
                }
                listtese.Add(pe);
            }
            pent.listpeibei = listpeibei;
            pent.listtese = listtese;
            //查询房管员姓名和电话
            if (pent.HouseKeeper != 0)
            {
                UserDAL1 udal = new UserDAL1();
                T_SysUser user = udal.QueryUerbyid(new T_SysUser() { Id = pent.HouseKeeper });
                if (user != null)
                {
                    pent.housekeepername = user.RealName;
                    pent.housekeeperphone = user.Mobile;
                }
            }
            resut.numberData = pent;
            return resut;
        }
        //根据合同查询锁数据
        public SysResult<List<HouseLockQuery>> Querylockbyhouse(HouseModel  model)
        {
            kjxDAL  kjxdal = new kjxDAL();
            SysResult<List<HouseLockQuery>> resut = new SysResult<List<HouseLockQuery>>();
            resut.numberData = kjxdal.Querylockbyhouse(model.Id);
            return resut;
        }
        //查询房管员所在的房源
        public SysResult<List<HouseQueryfgy>> queryfgy(HouseModel model)
        {
           
            SysResult<List<HouseQueryfgy>> resut = new SysResult<List<HouseQueryfgy>>();
            resut.numberData = dal.queryhouse(model);
            return resut;
        }
    }
}
