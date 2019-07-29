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
        public SysResult<IList<WrapHouseModel>> Queryhouse(HouseModel model, OrderablePagination orderablePagination,T_SysUser user, long[] userids)
        {
            SysResult<IList<WrapHouseModel>> result = new SysResult<IList<WrapHouseModel>>();
            IList<WrapHouseModel> listwrap = new List<WrapHouseModel>();
            List<WrapHousePendent> listpent = new List<WrapHousePendent>();
            IList<WrapHouseModel1> listhouse = new List<WrapHouseModel1>();
            long housecount = 0;
            //计算空置时间
            dtmode dtmo = gettuizutime(model.Idletime);
            string[] arr = getqueryarr(model.Content);
            listhouse = dal.Querylist(model, dtmo,user.departs, user.roles,userids, orderablePagination, arr, out housecount);
            listpent = pentdal.Querybyparentids(listhouse.Select(p=>p.Id).ToList(), model, dtmo);
            if (listhouse.Count > 0)
            {
                foreach (var mo in listhouse)
                {
                    mo.Title = mo.CellName+"-" +(mo.BuildingNumber==null?"": mo.BuildingNumber+"号") + (mo.Unit == 0 ? "" : mo.Unit + "单元") + "-" + (mo.RoomId == null ? "" : mo.RoomId + "室");
                    WrapHouseModel wrap = new WrapHouseModel();
                    wrap.house = mo;
                    wrap.house.Idletime = (DateTime.Now-mo.Renttime).Days;
                    wrap.housependent = listpent.Where(p => p.ParentRoomid == mo.Id).ToList();
                    if (wrap.housependent != null)
                    {
                        foreach(var m in wrap.housependent)
                        {
                            //排序
                            m.sort = pentdal.getindex(m.Name);
                            m.Idletime = (DateTime.Now - mo.Renttime).Days;
                        }
                        //排序
                        wrap.housependent = wrap.housependent.OrderBy(p => p.sort).ToList();
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
        public SysResult<List<WrapHousePendent>> excelQueryhouse(HouseModel model, T_SysUser user)
        {
            SysResult<List<WrapHousePendent>> result = new SysResult<List<WrapHousePendent>>();
            List<WrapHousePendent> listpent = new List<WrapHousePendent>();
            IList<HouseModel> listhouse = new List<HouseModel>();
            dtmode dtmo = gettuizutime(model.Idletime);
            listhouse = dal.excelQuerylist(model, dtmo);
            listpent = pentdal.Querybyparentids(listhouse.Select(p => p.Id).ToList(), model, dtmo);
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
        public SysResult<List<HouseTip>> Querytip(ParaTip model, OrderablePagination orderablePagination ,long[] userids,T_SysUser user)
        {
            SysResult<List<HouseTip>> result = new SysResult<List<HouseTip>>();
            HouseModel para = new HouseModel();
            List<HouseTip> listtip = new List<HouseTip>();
            para.CellName = model.Name;
            para.CompanyId = model.CompanyId;
            listtip = dal.Query(para, orderablePagination,user.departs,user.roles,userids);
            foreach (var mo in listtip)
            {
                if (model.Type == 2)
                {
                    mo.Name = getname(mo.Name, mo.isyccontract);
                }
                
                else
                {
                    mo.Name = mo.Name + "(" + getstatusstr(mo.Status) + ")";
                }
              
            }
            result.numberData = listtip;
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        public SysResult<List<houresources>> HouseQuery(houresources model, OrderablePagination orderablePagination, long[] userids, T_SysUser user)
        {
            SysResult<List<houresources>> result = new SysResult<List<houresources>>();
            

            result.numberData = dal.Querylist1(model, orderablePagination,user.departs, user.roles, userids); 
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        //整租房源
        public SysResult<List<WrapHouseModel1>> zzHouseQuery(HouseModel model, OrderablePagination orderablePagination,string [] citys,T_SysUser user)
        {
            SysResult<List<WrapHouseModel1>> result = new SysResult<List<WrapHouseModel1>>();
            model.RecrntType = 1;
            string[] arr=getqueryarr(model.Content);
            dtmode dtmo = gettuizutime(model.Idletime);
            List<WrapHouseModel1> hlist = dal.Querylist2(model, dtmo, orderablePagination,citys,user, arr);
            foreach(var mo in hlist)
            {
                mo.Title = mo.CellName + "-"  + (mo.BuildingNumber == null ? "" : mo.BuildingNumber + "号")+(mo.Unit == 0 ? "" : mo.BuildingNumber + "单元")+ "-" + (mo.RoomId == null ? "" : mo.RoomId + "室");
                mo.Idletime = (DateTime.Now - mo.Renttime).Days;
            }
            result.numberData = hlist;
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        //分别获取小区名+楼栋+房间号
        public string[] getqueryarr(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] arr = new string[] { };
                arr = str.Split('-');
                for(int i=0;i< arr.Length; i++)
                {
                    if (i == 1)
                    {
                        arr[i] = arr[i].Replace("号", "");
                        arr[i] = arr[i].Replace("楼", "");
                    }
                    if (i == 2)
                    {
                        arr[i] = arr[i].Replace("室", "");
                    }
                }
                return arr;
            }
            else
            {
                return null;
            }
            
        }
        //整租房源分组
        public SysResult<List<WrapCellName>> zzHouseQuerygroupby(HouseModel model, OrderablePagination orderablePagination, string[] citys, T_SysUser user)
        {
            SysResult<List<WrapCellName>> result = new SysResult<List<WrapCellName>>();
            model.RecrntType = 1;
            dtmode dtmo = gettuizutime(model.Idletime);
            List<WrapCellName> hlist = dal.housegroup(model, dtmo, orderablePagination, citys, user);
            //房间查询
            List<WrapHouseModel1> listpent = dal.Querylist2group(model, dtmo, hlist, citys, user);
            foreach (var mo in hlist)
            {
                if (listpent != null)
                {
                    mo.listhouse = listpent.Where(p => p.CellName == mo.CellName).ToList();
                    foreach (var mo1 in mo.listhouse)
                    {
                        mo1.Title = (mo1.BuildingNumber == null ? "" : mo1.BuildingNumber + "号") + (mo1.RoomId == null ? "" : mo1.RoomId + "室");
                        mo1.Idletime = (DateTime.Now - mo1.Renttime).Days;
                    }
                }
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
           
            HouseModel remodel = dal.Queryhouse(new HouseModel() { Electricid=model.Electricid},null,null);
            if (remodel == null)
            {
               HousePendent  pent = pentdal.Querybyid(model);
               
               if (pent != null)
               {
                    HouseModel housemode = dal.Queryhouse(new HouseModel() { Id = pent.ParentRoomid },null,null);
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

        public string getname(string name,int isycontract)
        {
            if (isycontract == 1)
            {
                return name + "(已录入业主合同)";
            }
            return name;
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
                if (model.Type == 1)
                {
                    mo.Name = getname(mo.Name, mo.isyccontract);
                }
                else
                {
                    mo.Name = mo.Name + "(" + getstatusstr(mo.Status) + ")";
                    T_Teant temo = listteant.Where(p => p.Id == mo.TeantId).FirstOrDefault();
                    if (temo != null)
                    {
                        mo.UserName = temo.Name;
                        mo.UserPhone = temo.Phone.ToStr();
                    }
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
            //if (model.AreamName == null)
            //{
            //    result.Code = 1;
            //    result.Message = "请选择区";
            //}
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
        public long fgystore(long userid)
        {
            if (userid == 0)
            {
                return 0;
            }
            UserDAL1 dal = new UserDAL1();
            T_SysUser user = dal.QueryUerbyid(new T_SysUser() {Id=userid });
            return user.storeid;
        }
        public SysResult<List<HousePendent>> saveHouse(HouseModel model,long userid,T_SysUser user)
        {
            SysResult<List<HousePendent>> result = new SysResult<List<HousePendent>>();
            result.numberData= new List<HousePendent>();
            SysResult voliteresult = Volite(model);
            if (model.storeid == 0)
            {
                model.storeid = fgystore(model.HouseKeeper);
            }
            if(!string.IsNullOrEmpty(model.location))
            {
                string [] arr = model.location.Split(',');
                if (arr != null && arr.Length > 1)
                {
                    model.longitude = double.Parse(arr[0]);
                    model.latitude = double.Parse(arr[1]);
                }
               
            }
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
            List<HousePendent> pent = pentdal.Querybyparentid(parentid);
            result.numberData = pent;
            //用户部门添加管理城市
            if (model.HouseKeeper != 0)
            {
                ProceService proce = new Service.ProceService();
                proce.CmdProce3(new Pure() { Ids = model.HouseKeeper.ToStr(), Spname = "sp_departaddhouse",Other1=model.CityName, Other = model.CellName, roperator = model.AreamName }, null, null);
                if (string.IsNullOrEmpty(user.city))
                {
                    user.city = user.city  + model.CityName;
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
                //rds.Delete(key);
                rds.SetModel<T_SysUser>(key, user);
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
            HouseModel remo = dal.Queryhouse(model,null,null);
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
            if (remo.storeid != 0&& remo.storeid != -1)
            {
                MenuDAL menudal = new MenuDAL();
                t_department depart = menudal.queryid(new t_department() { Id= remo.storeid});
                if (depart != null)
                {
                    remo.store = depart.name;
                }
            }
            if (remo.storeid == -1)
            {
                remo.store = "总部";
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
            dic.Add("costprice", model.costprice.ToStr());
            dic.Add("ycostprice", pent.costprice.ToStr());
            dic.Add("HouseKeeper", model.HouseKeeper.ToStr());
            dic.Add("yHouseKeeper", pent.HouseKeeper.ToStr());
            dic.Add("yAdress", pent.Adress.ToStr());
            dic.Add("Adress", model.Adress.ToStr());
            dal.SaveOthers(model.listpeibei, model.listtese,1);
            if (pentdal.saveeditPent(model)>0)
            {
                RzService reservice = new RzService();
                reservice.addrz(new HouseModel() {RecrntType=1, CompanyId = model.CompanyId, Price = model.Price }, 2, userid, model.ID, dic);
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
            HouseModel model = dal.Queryhouse(new HouseModel() { Id= pent.ParentRoomid },null,null );
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
                pent.PublicImage = model.PublicImg;
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
            //查询门店信息
            if (pent.storeid != 0 && pent.storeid != -1)
            {
                MenuDAL menudal = new MenuDAL();
                t_department depart = menudal.queryid(new t_department() { Id = pent.storeid });
                if (depart != null)
                {
                    pent.store = depart.name;
                }
            }
            if (pent.storeid == -1)
            {
                pent.store = "总部";
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
        //查询所有房源
        public SysResult<List<distributionHouseQuery>> Queryhouse(HouseModel model, OrderablePagination orderablePagination)
        {
            SysResult<List<distributionHouseQuery>> result = new SysResult<List<distributionHouseQuery>>();
            if (model.CellNames != null)
            {
                model.arrCellNames = model.CellNames.Split(',');
            }
            List<distributionHouseQuery> hlist = dal.Queryhousepage(model, orderablePagination);
            result.numberData = hlist;
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        //查询所有部门房源
        public SysResult<List<distributionHouseQuery>> Queryhousedepart(HouseModel model, OrderablePagination orderablePagination)
        {
            SysResult<List<distributionHouseQuery>> result = new SysResult<List<distributionHouseQuery>>();
            if (model.CellNames != null)
            {
                model.arrCellNames = model.CellNames.Split(',');
            }
            List<distributionHouseQuery> hlist = dal.Queryhousedepart(model, orderablePagination);
            result.numberData = hlist;
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        //按照小区和楼查询房源
        public SysResult<List<WrapHouse>> Queryhousecount(HouseModel model, OrderablePagination orderablePagination)
        {
            SysResult<List<WrapHouse>> result = new SysResult<List<WrapHouse>>();
            HousePentDAL pentdal = new HousePentDAL();
            if (model.CellNames != null)
            {
                model.arrCellNames = model.CellNames.Split(',');
            }
            if (model.RecrntType == 3)
            {
                
                List<WrapHouse> list = new List<WrapHouse>();
                //查询符合条件的独栋房源
                List<WrapIndentHouse> house = dal.querydudong(model);
                foreach(var mo in house)
                {
                    WrapHouse wr = new WrapHouse();
                    wr.id = mo.Name;
                    wr.RecentType = 3;
                    wr.CityName = mo.CityName;
                    wr.AreaName = mo.AreaName;
                    wr.CellName = mo.Name;
                    wr.count = pentdal.queryhousecount(mo.Id);
                    list.Add(wr);
                }
                result.numberData = list;
            }
            else
            {
                List<WrapHouse> hlist = dal.Queryhousecount(model, orderablePagination);
                result.numberData = hlist;
            }
            result.numberCount = orderablePagination.TotalCount;
            return result;
        }
        //查询选中数据
        public SysResult<List<long>> Queryhousedepartcheck(HouseModel model)
        {
            SysResult<List<long>> result = new SysResult<List<long>>();
            List<long> hlist = dal.Queryhousedepartcheck(model);
            result.numberData = hlist;
            return result;
        }
        //查询股东被选中的数据
        public SysResult<List<string>> Queryhousedepartcheck1(T_SysUser model)
        {
            SysResult<List<string>> result = new SysResult<List<string>>();
            List<long> arr = new List<long>();
            List<string> strarr = new List<string> { };
            UserDAL1 userdal = new UserDAL1();
            T_SysUser sysuser = new T_SysUser();
            sysuser = userdal.QueryUerbyid(model);
            if (sysuser != null&& sysuser.cellname != null)
            {
                strarr = sysuser.cellname.Split(",").ToList();
            }
            result.numberData = strarr;
            return result;
        }
    }
}
