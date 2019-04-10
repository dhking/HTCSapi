using ControllerHelper;
using DAL;
using Model;
using Model.Contrct;
using Model.House;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class ElectricService
    {
        ElectricDAL dal = new ElectricDAL();
        public Elec1<ElecUser> Login(ElecUser user, T_SysUser sysuser)
        {
            Elec1<ElecUser> result = new Elec1<ElecUser>();
            //查询子账号
            ElecUser elecuser = dal.Query(new ElecUser() { CompanyId = sysuser.CompanyId ,paratype=1});
            if (elecuser != null)
            {
                Elec1<ElecUser> reuser = dal.Login(elecuser);
                result = reuser;
            }
            else
            {
                result.Code = 1002;
                result.Message = "请先登录";
            }
            return result;
        }
        //解除绑定房间
        public SysResult notBing(DeviceData model)
        {
            SysResult result = new SysResult();
            HouseDAL dal = new HouseDAL();
            HousePentDAL pdal = new HousePentDAL();
            kjxDAL kjx = new kjxDAL();
            List<HouseQuery> listmodel = dal.Query4(new HouseModel() { Electricid = model.devid });
            model.devid = null;
            model.Uuid = null;
            foreach(var mo in listmodel)
            {
                if (mo.RecentType == 1)
                {
                    dal.SaveorUpdateHouse1(new HouseModel() { Id = model.HouseId, Electricid = model.devid, uuid = model.Uuid }, new string[] { "Electricid", "uuid" });
                }
                else
                {
                    pdal.SaveorUpdateHouse(new HousePendent() { ID = mo.Id, Electricid = model.devid, uuid = model.Uuid }, new string[] { "Electricid", "uuid" });
                }
            }
            result.Code = 0;
            result.Message = "解除绑定成功";
            return result;
        }
        //绑定房间
        public SysResult<Wraplocklist> Bing(DeviceData model)
        {
            SysResult<Wraplocklist> result = new SysResult<Wraplocklist>();
            kjxDAL kjx = new kjxDAL();
            HouseDAL hdal = new HouseDAL();
            if (kjx.QueryCount1(model.devid) > 0)
            {
                result.Code = 1;
                result.Message = "本电表已经被绑定，请先解绑";
                return result;
            }
            HouseLockQuery query = hdal.Queryhouse2(model.HouseId);
            if (model.ElecType==0)
            {
                HouseDAL dal = new HouseDAL();
                dal.SaveorUpdateHouse1(new HouseModel() { Id = model.HouseId, Electricid = model.devid,uuid=model.Uuid }, new string[] { "Electricid", "uuid" });
            }
            if ((query.RecentType == 3 && model.ElecType == 0)|| model.ElecType ==1)
            {
                HousePentDAL dal = new HousePentDAL();
                dal.SaveorUpdateHouse(new HousePendent() { ID = model.HouseId, Electricid = model.devid, uuid = model.Uuid }, new string[] { "Electricid", "uuid" });
            }
            Wraplocklist remodel = new Wraplocklist();
            remodel.HouseId = model.HouseId;
            remodel.HouseType = model.HouseType;
            result.Code = 0;
            result.Message = "绑定成功";
            result.numberData = remodel;
            return result;
        }
        public Elec1<ElecUser> checkuser(ElecUser user, T_SysUser sysuser)
        {
            Elec1<ElecUser> reuser = new Elec1<ElecUser>();
            if (user == null)
            {
                reuser = Login(user, sysuser);
                return reuser;
            }
            double hourse = (DateTime.Now - user.now).TotalHours;
            reuser.Data = new ElecUser();
            reuser.Data.Uuid = user.Uuid;
            reuser.Data.Expand =user.Expand;
            reuser = Login(user, sysuser);
            //if (hourse < 0.9)
            //{
            //    reuser = dal.Reflashtoken(user);
            //}
            //else
            //{
            //    reuser = Login(user, sysuser);
            //}
            return reuser;
        }
        //充值
        public SysResult threshold(other model, ElecUser user, T_SysUser sysuser)
        {
            SysResult result = new SysResult();
            Elec1<ElecUser> eleresult = checkuser(user,sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            model.Uuid = eleresult.Data.Uuid;
            model.Expand = eleresult.Expand;
            Elec<DeviceData> elecre =dal.accountthreshold(model);
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            return result;
        }
        //入住
        public SysResult ammeterstayroom(ruzhu model, ElecUser user, T_SysUser sysuser)
        {
            SysResult result = new SysResult();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            model.Uuid = eleresult.Data.Uuid;
            model.Expand = eleresult.Expand;
            Elec<DeviceData> elecre = dal.ammeterstayroom(model);
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            return result;
        }
        //退房
        public SysResult ammeterrecederoom(ruzhu model, ElecUser user, T_SysUser sysuser)
        {
            SysResult result = new SysResult();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            model.Uuid = eleresult.Data.Uuid;
            model.Expand = eleresult.Expand;
            Elec<DeviceData> elecre = dal.ammeterrecederoom(model);
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            return result;
        }
        //租客端充值
        public SysResult zkthreshold(T_Contrct model)
        {
            SysResult result = new SysResult();
            //根据房间编号获取电表
            HouseDAL housedal = new HouseDAL();
            HouseQuery query = housedal.Queryhouse1(model.HouseId);
            other para = new other();
            para.devid = query.uuid;
            para.value = model.value;
            return threshold(para, new ElecUser(), new T_SysUser() { CompanyId= query.CompanyId });
        }
        //获取账号下电表
        public SysResult<List<DeviceData>> ammeterpage(other model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<List<DeviceData>> result = new SysResult<List<DeviceData>>();
            Elec1<ElecUser> eleresult = checkuser(user,sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            model.Uuid = eleresult.Data.Uuid;
            model.Expand = eleresult.Data.Expand;
            Elec<List<DeviceData>> elecre = dal.ammeterpage(model);
            if (elecre.Data != null && elecre.Data.Count > 0)
            {
                List<string> str = elecre.Data.Select(p => p.Pcode).ToList();
                //查询集中器的主房间号
                List<HouseLockQuery> houmodel = dal.lockquery(str);
                //循环
                foreach (var mo in elecre.Data)
                {
                    foreach (var ho in houmodel)
                    {
                        mo.Title = ho.Room;
                        if (mo.Pcode == ho.ElecId)
                        {
                            mo.HouseName =ho.Room +  "("+ho.RoomName+")";
                            mo.Status = ho.Status;
                            continue;
                        }
                    }
                    if (mo.HouseName == null)
                    {
                        mo.HouseName = "未绑定房间";
                    }
                }
            }
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            result.numberData = elecre.Data;
            return result;
        }
       
        //获取房源设备
        public SysResult<List<DeviceData>> housedevice1(other model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<List<DeviceData>> result = new SysResult<List<DeviceData>>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            model.Uuid = eleresult.Data.Uuid;
            model.Expand = eleresult.Data.Expand;
            Elec<List<DeviceData>> elecre = dal.housedevice1(model);
            if (elecre.Data != null && elecre.Data.Count > 0)
            {
                List<string> str = elecre.Data.Select(p => p.Pcode).ToList();
                //查询集中器的主房间号
                List <HouseLockQuery> houmodel = dal.lockquery(str);
                //循环
                foreach (var mo in elecre.Data)
                {
                    foreach (var ho in houmodel)
                    {
                        mo.Title = ho.Room;
                        if (mo.Pcode == ho.ElecId)
                        {
                            mo.HouseName = ho.RoomName;
                            mo.Status = ho.Status;
                            continue;
                        }
                    }
                    if (mo.HouseName == null)
                    {
                        mo.HouseName = "未绑定房间";
                    }   
                }
            }
            result.Message = elecre.Message;
            result.numberData = elecre.Data;
            return result;
        }
        //添加设备
        public SysResult<List<DeviceData>> housedevice(other model, ElecUser user,T_SysUser sysuser)
        {
            SysResult<List<DeviceData>> result = new SysResult<List<DeviceData>>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            model.Uuid = eleresult.Data.Uuid;
            model.Expand = eleresult.Data.Expand;
            model.username = eleresult.Data.username;
            Elec elecre = dal.housedevice(model);
          
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            return result;
        }
        //创建房源
        public SysResult<List<DeviceData>> createhouse(other model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<List<DeviceData>> result = new SysResult<List<DeviceData>>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            model.userid = eleresult.Data.username;
            model.Uuid = eleresult.Data.Uuid;
            model.Expand = eleresult.Data.Expand;
            Elec<elechouse> elechouse = dal.house(new elechouse() { districtcode = "1001010001", title = model.title, level = 2 }, user);
            result.Code = elechouse.Code;
            result.Message = elechouse.Message;
            return result;
        }
        //添加智能设备
        public SysResult<List<DeviceData>> terminalbind(other model, ElecUser user, T_SysUser sysuser)
        {
            
            SysResult<List<DeviceData>> result = new SysResult<List<DeviceData>>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            model.userid = eleresult.Data.username;
            model.Uuid = eleresult.Data.Uuid;
            model.Expand = eleresult.Data.Expand;
            Elec<DeviceData> elecre = dal.binddevice(model);
           
            if (elecre.Code == 0 && model.houseid != null)
            {
                //绑定本地房源
                ProceDAL proce = new ProceDAL();
                proce.CmdProce(new Pure() { Id = model.houseid.ToStr(), Other = model.cid, Other1 = elecre.Expand.Uuid, Spname = "sp_bindelec1" });
                //创建小区
                Elec<eleccell> eleccell= dal.cellname(new eleccell() { code = model.AreaCode, title = model.CellName, content = model.CellName, }, user);
                if (eleccell.Code == 0)
                {
                    //创建房源
                    Elec<elechouse> elechouse=dal.house(new elechouse() { districtcode = eleccell.Data.code, title = model.title, level = 2 }, user);
                    if (eleccell.Code == 0)
                    {
                        //绑定房源
                        dal.housedevice(new other() { devid = elecre.Expand.Uuid, houseid = elechouse.Data.Uuid,Uuid= model.Uuid,Expand=model.Expand });
                        //切换成预付费
                        dal.ammeterpaymode(new DeviceData() { devid = elecre.Expand.Uuid }, user);
                    }
                }
            }
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            return result;
        }
        //绑定节点
        public SysResult<List<DeviceData>> nodebind(other model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<List<DeviceData>> result = new SysResult<List<DeviceData>>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            model.userid = eleresult.Data.username;
            model.Uuid = eleresult.Data.Uuid;
            model.Expand = eleresult.Data.Expand;
            Elec<DeviceData> elecre = dal.nodebind(model);
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            if (elecre.Code == 0 && model.houseid != null)
            {
                //切换成预付费
                dal.ammeterpaymode(new DeviceData() { devid = elecre.Expand.Uuid }, user);
                //绑定本地房源
                ProceDAL proce = new ProceDAL();
                proce.CmdProce(new Pure() { Id = model.houseid.ToStr(), Other = model.nid, Other1 = elecre.Expand.Uuid, Spname = "sp_bindelec" });
            }
            return result;
        }
        //电表退房清单
        public SysResult<DeviceData> recederoom(other model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<DeviceData> result = new SysResult<DeviceData>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            model.userid = eleresult.Data.username;
            model.Uuid = eleresult.Data.Uuid;
            model.Expand = eleresult.Data.Expand;
            Elec<DeviceData> elecre = dal.recederoom(model);
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            result.numberData = elecre.Data;
            return result;
        }
        //刷新电表
        public SysResult<List<DeviceData>> deviceammeter(DeviceData model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<List<DeviceData>> result = new SysResult<List<DeviceData>>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
            Elec<List<DeviceData>> elecre = dal.deviceammeter(model, user);
            if (elecre.Code!= 0)
            {
                result.Code = elecre.Code;
                result.Message = elecre.Message;
                return result;
            }
            //查询集中器的主房间号
            DeviceData data = elecre.Data.FirstOrDefault();
            if (data != null)
            {
                //查询绑定房间
                if (data.External != null)
                {
                    if (data.External.apportion != null)
                    {
                        foreach (var mo in data.External.apportion)
                        {
                            HouseLockQuery lockquery = dal.lockquery2(mo.devid);
                            if (lockquery != null)
                            {
                                mo.HouseName = lockquery.RoomName;
                                mo.devid = lockquery.ElecId;
                            }
                        }
                    }
                }
                HouseLockQuery houmodel = dal.lockquery1(data.Pcode);
                if (houmodel != null)
                {
                    data.HouseName = houmodel.RoomName;
                    data.HouseId = houmodel.Id;
                    data.HouseType = houmodel.RecentType;
                    data.iscuizu = houmodel.iscuizu;
                }
                elecre.Data[0] = data;
            }
          
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            result.numberData = elecre.Data;
            return result;
        }
        //获取单价和当前度数
        public SysResult<zkelec> zkdeviceammeter(T_Contrct model)
        {
            SysResult<zkelec> result = new SysResult<zkelec>();
            zkelec data = new zkelec();
            //根据房间编号获取电表
            HouseDAL housedal = new HouseDAL();
            HouseQuery query = housedal.Queryhouse1(model.HouseId);
            if (query == null)
            {
                result.Code = 2;
                result.Message = "房间不存在请联系房管员";
                return result;
            }
            DeviceData para = new DeviceData();
            para.Uuid = query.electricid;
            if (para.Uuid == null)
            {
                result.Code = 1;
                result.Message = "当前房间未绑定智能电表不能充值";
                return result;
            }
            List<DeviceData>remo = deviceammeter(para, new ElecUser(), new T_SysUser() { CompanyId = query.CompanyId }).numberData;
            if (remo != null)
            {
                data.Price = remo.FirstOrDefault().Price;
                data.surplus = remo.FirstOrDefault().Expand.surplus;
            }
            result.numberData = data;
            return result;
        }
        //获取房源树
        public SysResult<List<fenzu>> houserefresh(fenzu model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<List<fenzu>> result = new SysResult<List<fenzu>>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
            Elec<List<fenzu>> elecre = dal.houserefresh(model, user);
            foreach(var mo in elecre.Data)
            {
                if (mo.Citys != null)
                {
                    if (mo.Citys.Count > 0)
                    {
                        mo.Citys = mo.Citys.Where(p => p.Quantity != 0).ToList();
                    }
                }
            }
            elecre.Data = elecre.Data.Where(p => p.Quantity != 0).ToList();
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            result.numberData = elecre.Data;
            return result;
        }
        //判断是子表还是主表
        public SysResult<int> devicechecktype(DeviceData model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<int> result = new SysResult<int>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
            Elec<int> elecre = dal.devicechecktype(model, user);
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            result.numberData = elecre.Data;
            return result;
        }
        //电表
        public SysResult<List<DeviceData>> housesearch(DeviceData model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<List<DeviceData>> result = new SysResult<List<DeviceData>>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
            Elec2<List<DeviceData>> elecre = dal.housesearch(model, user);
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            result.numberData = elecre.Expand;
            return result;
        }
        //选择省市区
        public SysResult<List<DeviceData>> housecity(localcity model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<List<DeviceData>> result = new SysResult<List<DeviceData>>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
            Elec2<List<DeviceData>> elecre = dal.housecity(model, user);
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            result.numberData = elecre.Expand;
            return result;
        }
        //通电或者断电
        public SysResult deviceswitchon(DeviceData model, ElecUser user, T_SysUser sysuser,bool cmd)
        {
            SysResult result = new SysResult();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
            Elec elecre = new Elec();
            if (cmd == true)
            {
               elecre=dal.deviceswitchon(model, user);
            }
            else
            {
               elecre=dal.switchoff(model, user);
            }
            result.Code = elecre.Code;
            result.Message = elecre.Message;
        
            return result;
        }
        //修改电价
        public SysResult ammeterprice(DeviceData model, ElecUser user, T_SysUser sysuser, bool cmd)
        {
            SysResult result = new SysResult();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
            Elec elecre = new Elec();
            elecre=dal.ammeterprice(model, user);
            result.Code = elecre.Code;
            result.Message = elecre.Message;

            return result;
        }
        //设置催租

        public SysResult deviceiscuizu(DeviceData model)
        {
            SysResult result = new SysResult();
            ProceDAL proce = new ProceDAL();
            proce.CmdProce2(new Pure() { Spname = "sp_iscuizu", Other = model.devid, Other1 = model.iscuizu.ToStr() });
            result.Code =0;
            result.Message = "操作成功";
            return result;
        }
        //设置电表类型
        public SysResult ammeterchangetype(DeviceData model, ElecUser user, T_SysUser sysuser)
        {
            SysResult result = new SysResult();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
            //选择个人房间
            if (model.Value==0)
            {
                //绑定房源
                dal.housedevice(new other() { devid = model.Uuid, houseid = model.HouseId.ToStr(), Uuid = model.Uuid});
                //切换预付费
                //dal.ammeterpaymode(new DeviceData() { devid = model.Uuid }, user);
                ProceDAL proce = new ProceDAL();
                //绑定本地房源
                proce.CmdProce(new Pure() { Id = model.HouseId.ToStr(), Other = model.Cid,
                                              Other1 = model.Uuid, Spname = "sp_bindelec1" });
            
            }
            if (model.fentan == null|| model.fentan.Count>0)
            {
                result = result.FailResult("请添加分摊电表");
            }
            //计算权重
            decimal amount = model.fentan.Sum(p => p.Percent);
            int count = model.fentan.Count;
            int index = 0;
            decimal percentall = 0;
            foreach(var mo in model.fentan)
            {
                index++;
                if (index < count)
                {
                    mo.Percent = Math.Round(mo.Percent/amount, 2);
                    percentall += mo.Percent;
                    continue;
                }
                if (index == count)
                {
                    mo.Percent = 1 - percentall;
                }
            }
            foreach (var mo in model.fentan)
            {
                mo.Percent = mo.Percent*100;
            }
            Elec elecre = new Elec();
            elecre = dal.ammeterchangetype(model, user);
            result.Code = elecre.Code;
            result.Message = elecre.Message;

            return result;
        }
        //解绑集中器
        public SysResult terminalunbound(DeviceData model, ElecUser user, T_SysUser sysuser)
        {
            SysResult result = new SysResult();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
            Elec elecre = new Elec();
            Elec elec = dal.terminallock(model, user);
            elecre = dal.terminalunbound(model, user);
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            return result;
        }
        //解绑节点
        public SysResult nodeunbound(DeviceData model, ElecUser user, T_SysUser sysuser)
        {
            SysResult result = new SysResult();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
            Elec elecre = new Elec();
          
            elecre = dal.nodeunbound(model, user);
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            return result;
        }
        //通过设备id获取设备的一段时间的用电统计
        public SysResult<List<ElecStatic>> reportaddupdevice(ElecStatic model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<List<ElecStatic>> result = new SysResult<List<ElecStatic>>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
            Elec<List<ElecStatic>> elecre = new Elec<List<ElecStatic>>();
            elecre = dal.reportaddupdevice(model, user);
            
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            return result;
        }
        //租客端获取用电记录
        public SysResult<List<ElecStatic>> zkaddupmonth(ElecStatic model)
        {
            HouseDAL housedal = new HouseDAL();
            SysResult<List<ElecStatic>> result = new SysResult<List<ElecStatic>>();
            HouseQuery query = housedal.Queryhouse1(model.HouseId);
            if (query == null)
            {
                result.Code = 2;
                result.Message = "房间不存在请联系房管员";
                return result;
            }
            if (query.uuid == null)
            {
                result.Code = 2;
                result.Message = "当前房间未绑定智能电表";
                return result;
            }
            ElecStatic para = new ElecStatic();
            para.devid = query.uuid;
            para.Month = model.Month;
            para.bdate = para.Month.AddDays(1 - para.Month.Day).Date;
            para.edate = para.Month.AddDays(1 - para.Month.Day).Date.AddMonths(1).AddSeconds(-1);
            return addupmonth(para, new ElecUser(), new T_SysUser() { CompanyId = query.CompanyId });
        }
        //用电量小计，主要用于统计每月的累计用电
        public SysResult<List<ElecStatic>> addupmonth(ElecStatic model, ElecUser user, T_SysUser sysuser)
        {
            SysResult<List<ElecStatic>> result = new SysResult<List<ElecStatic>>();
            Elec1<ElecUser> eleresult = checkuser(user, sysuser);
            if (eleresult.Code != 0)
            {
                result.Code = 1002;
                result.Message = "请重新登陆";
                return result;
            }
            user.Uuid = eleresult.Data.Uuid;
            user.Expand = eleresult.Data.Expand;
           
            Elec<List<ElecStatic>> elecre = new Elec<List<ElecStatic>>();
            elecre = dal.addupmonth(model, user);
            result.Code = elecre.Code;
            result.Message = elecre.Message;
            result.numberData = elecre.Data;
            return result;
        }
    }
}
