
using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Model;
using Model.House;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class ElectricDAL : RcsBaseDao
    {
        WebUtils Util = new Common.WebUtils();
        string servserurl = "http://smartammeter.zg118.com:8001/";
        public string Getmd5(string timestamp)
        {
            string secret = "2c0353a0e8e84c407fcadf467bae1e51";
            //string random = ConvertHelper.GenerateCheckCodeNum(0);
            string random = "123456";
            string daijiammi = servserurl+"user/login/hujian/qqq?timestamp=" + timestamp + "&random="+ random+ secret;
            string signature = Common.ConvertHelper.GetMd5HashStr(daijiammi);
            return signature;
        }
        public Dictionary<string, string> comdic()
        {
            string timestamp = Common.ConvertHelper.getsecond1();
            string md5 = Getmd5(timestamp);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("timestamp", timestamp);
            dic.Add("signature", md5);
            return dic;
        }
        public Elec1<ElecUser> Login(ElecUser user)
        {
            Elec1<ElecUser> reresult= new Elec1<ElecUser>();
            if (user.username != null && user.pass != null)
            {
                string url = servserurl+"user/login" + "/" + user.username + "/" + user.pass;
                string body = Util.DoGet1(url, comdic(), user);
                reresult = JsonConvert.DeserializeObject<Elec1<ElecUser>>(body);
                
                if (reresult != null&& reresult.Code==0)
                {
                    RedisHtcs rds = new RedisHtcs();
                    ElecUser rdsuser = new ElecUser();
                    rdsuser.Expand = reresult.Expand.ToStr();
                    rdsuser.Uuid = reresult.Data.Uuid;
                    rdsuser.username = user.username;
                    rdsuser.CompanyId = user.CompanyId;
                    string key = "elec" + rdsuser.CompanyId;
                    rdsuser.now = DateTime.Now;
                    rds.SetModel<ElecUser>(key, rdsuser);
                    reresult.Data.username = user.username;
                    reresult.Data.Expand = reresult.Expand;
                }
            }
            
            return reresult;
        }
        public Elec1<ElecUser> Reflashtoken(ElecUser user)
        {
            Elec1<ElecUser> reuser = new Elec1<ElecUser>();
            string url = servserurl+"user/token/refresh";
            string body = Util.DoGet1(url, comdic(), user);
            reuser = JsonConvert.DeserializeObject<Elec1<ElecUser>>(body);
            reuser.Data = user;
            return reuser;
        }

     
        public Elec<DeviceData> accountthreshold(other user)
        {
            Elec<DeviceData> result = new Elec<DeviceData>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "account/recharge/" + user.devid + "/" + user.value;
            string body = Util.Put(url, "", req);
            result = JsonConvert.DeserializeObject<Elec<DeviceData>>(body);
            return result;
        }
        //入住
        public Elec<DeviceData> ammeterstayroom(ruzhu user)
        {
            Elec<DeviceData> result = new Elec<DeviceData>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "device/ammeter/stayroom/"+ user.devid;
            string para = JsonConvert.SerializeObject(user);
            string body = Util.Put(url, para, req);
            result = JsonConvert.DeserializeObject<Elec<DeviceData>>(body);
            return result;
        }
        //退房
        public Elec<DeviceData> ammeterrecederoom(ruzhu user)
        {
            Elec<DeviceData> result = new Elec<DeviceData>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "device/ammeter/confirmrecederoom/" + user.devid;
            string body = Util.Put(url,"", req);
            result = JsonConvert.DeserializeObject<Elec<DeviceData>>(body);
            return result;
        }
        public Elec<List<DeviceData>> ammeterpage(other user)
        {
            Elec<List<DeviceData>> result = new Elec<List<DeviceData>>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "device/ammeter/page/" + user.PageIndex + "/" + user.PageSize;
            string body = Util.DoGet1(url, comdic(), req);
            result = JsonConvert.DeserializeObject<Elec<List<DeviceData>>>(body);
            return result;
        }
        public Elec<List<DeviceData>> housedevice1(other user)
        {
            Elec<List<DeviceData>> result = new Elec<List<DeviceData>>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "house/device/" + user.hid;
            string body = Util.DoGet1(url, comdic(), req);
            result = JsonConvert.DeserializeObject<Elec<List<DeviceData>>>(body);
            return result;
        }
        
        public Elec housedevice(other user)
        {
            Elec result = new Elec();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "house/device/" + user.devid + "/" + user.houseid;
            string body = Util.Put(url, "", req);
            result = JsonConvert.DeserializeObject<Elec>(body);
            return result;
        }
        public Elec<DeviceData> binddevice(other user)
        {
            Elec<DeviceData> result = new Elec<DeviceData>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "terminal/bind/" + user.cid + "/" + user.macid+"/"+ user.userid;
            string body = Util.Put(url, "", req);
            result = JsonConvert.DeserializeObject<Elec<DeviceData>>(body);
            if (result.Code == 1002)
            {
                result.Code = 1003;
            }
            return result;
        }
        //绑定节点
        public Elec<DeviceData> nodebind(other user)
        {
            Elec<DeviceData> result = new Elec<DeviceData>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            //获取节点
            Elec<List<DeviceData>> listresult = deviceammeter(user, new string[] { user.nid, user.cid });
            if (listresult.Code == 0)
            {
               // string nid = listresult.Data.Where(p => p.Pcode == user.nid).FirstOrDefault().Uuid;
                string cid = listresult.Data.Where(p => p.Ccode == user.cid).FirstOrDefault().Cid;
                string url = servserurl + "node/bind/" + user.nid + "/" + cid + "/0/0/0";
                string body = Util.Put(url, "", req);
                result = JsonConvert.DeserializeObject<Elec<DeviceData>>(body);
            }
            else
            {
                result.Code = listresult.Code;
                result.Message = listresult.Message;
            }
            return result;
        }
        //获取退房清单
        public Elec<DeviceData> recederoom(other user)
        {
            Elec<DeviceData> result = new Elec<DeviceData>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            //获取节点
            string url = servserurl + "device/ammeter/recederoom/" + user.devid;
            string body = Util.Put(url, "", req);
            result = JsonConvert.DeserializeObject<Elec<DeviceData>>(body);
            return result;
        }
        //通过设备号批量获取电表
        public Elec<List<DeviceData>> deviceammeter(other user,string[] devid)
        {
            Elec<List<DeviceData>> result = new Elec<List<DeviceData>>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            Elec<object> elec = new Elec<object>();
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string str = "[";
            foreach(var m in devid)
            {
                str += '"'+m+ '"'+",";
            }
            str = str.Substring(0, str.Length - 1);
            str += "]";
            string url = servserurl + "device/ammeter/pcode/" + str;
            string body = Util.DoGet1(url, comdic(), req);
            elec = JsonConvert.DeserializeObject<Elec<object>>(body);
            if (elec.Code == 0)
            {
                string bo = JsonConvert.SerializeObject(elec.Data);
                result.Data = JsonConvert.DeserializeObject<List<DeviceData>>(bo);
            }
            return result;
        }
        public ElecUser Query(ElecUser model)
        {
            var mo = from m in ElecUser select m;
            Expression<Func<ElecUser, bool>> where = m => 1 == 1;
            if (model.paratype != 0)
            {
                where = where.And(m => m.paratype == model.paratype);
            }
            if (model.Type != 0)
            {
                where = where.And(m => m.Type == model.Type);
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            mo = mo.Where(where);
            return mo.FirstOrDefault();
        }
        //刷新电表
        public Elec<List<DeviceData>> deviceammeter(DeviceData model, ElecUser user)
        {
            Elec<List<DeviceData>> result = new Elec<List<DeviceData>>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "device/ammeter/" + model.Uuid;
            string body = Util.DoGet1(url, comdic(), req);
            result = JsonConvert.DeserializeObject<Elec<List<DeviceData>>>(body);
            return result;
        }
       
        //获取房源树
        public Elec<List<fenzu>> houserefresh(fenzu model, ElecUser user)
        {
            Elec<List<fenzu>> result = new Elec<List<fenzu>>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "house";
            if (model.district != null)
            {
                url= servserurl + "house/refresh/"+ model.district;
            }
            string body = Util.DoGet1(url, comdic(), req);
            result = JsonConvert.DeserializeObject<Elec<List<fenzu>>>(body);
            return result;
        }
        //判断子表还是主表
        public Elec<int> devicechecktype(DeviceData model, ElecUser user)
        {
            Elec<int> result = new Elec<int>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "device/checktype/" + model.Pcode;
            string body = Util.DoGet1(url, comdic(), req);
            result = JsonConvert.DeserializeObject<Elec<int>>(body);
            return result;
        }
        //查询电表
        public Elec2<List<DeviceData>> housesearch(DeviceData model, ElecUser user)
        {
            Elec2<List<DeviceData>> result = new Elec2<List<DeviceData>>();
           
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "house/search/" + model.type + "/"+model.search;
            string body = Util.DoGet1(url, comdic(), req);
            result = JsonConvert.DeserializeObject<Elec2<List<DeviceData>>>(body);
            return result;
        }
        //获取省市区
        public Elec2<List<DeviceData>> housecity(localcity model, ElecUser user)
        {
            Elec2<List<DeviceData>> result = new Elec2<List<DeviceData>>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "house/city/" + model.subtype + "/" + model.pcode;
            if(model.subtype == 0)
            {
                url = servserurl + "house/city/" + model.subtype;
            }
            string body = Util.DoGet1(url, comdic(), req);
            result = JsonConvert.DeserializeObject<Elec2<List<DeviceData>>>(body);
            return result;
        }
        //通电
        public Elec deviceswitchon(DeviceData model, ElecUser user)
        {
            Elec result = new Elec();
          
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "device/switchon/" + model.Uuid;
            string body = Util.Put(url, "", req);
            result = JsonConvert.DeserializeObject<Elec>(body);
            return result;
        }
        //修改电价
        public Elec ammeterprice(DeviceData model, ElecUser user)
        {
            Elec result = new Elec();

            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "device/ammeter/price/" + model.devid+"/"+model.Value;
            string body = Util.Put(url, "", req);
            result = JsonConvert.DeserializeObject<Elec>(body);
            return result;
        }
        //断电
        public Elec switchoff(DeviceData model, ElecUser user)
        {
            Elec result = new Elec();

            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "device/switchoff/" + model.Uuid;
            string body = Util.Put(url, "", req);
            LogService log = new LogService();
            string jsonData = JsonConvert.SerializeObject(model);
            log.logInfo("断电结果" + body+ jsonData);
            result = JsonConvert.DeserializeObject<Elec>(body);
            return result;
        }
        //设置电表类型
        public Elec ammeterchangetype(DeviceData model, ElecUser user)
        {
            Elec result = new Elec();

            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string para = JsonConvert.SerializeObject(model.fentan);
            string url = servserurl + "device/ammeter/changetype/" + model.Uuid+"/"+model.Value;
            string body = Util.Put(url, para, req);
            result = JsonConvert.DeserializeObject<Elec>(body);
            return result;
        }
        //查询小区
        public Elec<List<eleccell>> searchcellname(eleccell model, ElecUser user)
        {
            Elec<List<eleccell>> result = new Elec<List<eleccell>>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "house/district/"+ model.title;
            string para = JsonConvert.SerializeObject(model);
            string body = Util.DoGet1(url, comdic(), user);
            result = JsonConvert.DeserializeObject<Elec<List<eleccell>>>(body);
            return result;
        }
        //创建小区
        public Elec<eleccell> cellname(eleccell model, ElecUser user)
        {
            model.code = "070204";
            Elec<eleccell> result = new Elec<eleccell>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            Elec<List<eleccell>> searchresult = searchcellname(model, user);
            if (searchresult.Data == null)
            {
                string url = servserurl + "house/district";
                string para = JsonConvert.SerializeObject(model);
                string body = Util.DoPost1(url, para, user, "");
                result = JsonConvert.DeserializeObject<Elec<eleccell>>(body);
            }
            else
            {
                result.Data = searchresult.Data.FirstOrDefault();
                return result;
            }
            return result;

        }
        //创建房源或者分组
        public Elec<elechouse> house(elechouse model, ElecUser user)
        {
            Elec<elechouse> result = new Elec<elechouse>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "house";
            string para = JsonConvert.SerializeObject(model);
            string body = Util.DoPost1(url,para,user,"");
            result = JsonConvert.DeserializeObject<Elec<elechouse>>(body);
            return result;
        }
        //解绑集中器
        public Elec terminalunbound(DeviceData model, ElecUser user)
        {
            Elec result = new Elec();

            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "terminal/unbound/" + model.Cid;
            string body = Util.Put(url, "", req);
            result = JsonConvert.DeserializeObject<Elec>(body);
            return result;
        }
        //切换付费模式
        public Elec ammeterpaymode(DeviceData model, ElecUser user)
        {
            Elec result = new Elec();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "/device/ammeter/paymode/" + model.devid+"/1";
            string body = Util.Put(url, "", req);
            result = JsonConvert.DeserializeObject<Elec>(body);
            return result;
        }
        public Elec nodeunbound(DeviceData model, ElecUser user)
        {
            Elec result = new Elec();

            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "node/unbound/" + model.Nid;
            string body = Util.Put(url, "", req);
            result = JsonConvert.DeserializeObject<Elec>(body);
            return result;
        }
        //设置未锁定
        public Elec terminallock(DeviceData model, ElecUser user)
        {
            Elec result = new Elec();

            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "terminal/lock/" + model.Cid+"/"+ 0;
            string body = Util.Put(url, "", req);
            result = JsonConvert.DeserializeObject<Elec>(body);
            return result;
        }
        //通过设备id获取设备的一段时间的用电统计
        public Elec<List<ElecStatic>> reportaddupdevice(ElecStatic model, ElecUser user)
        {
            Elec<List<ElecStatic>> result = new Elec<List<ElecStatic>>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "report/addupdevice/" + model.bdate + "/" + model.edate;
            string para = "[" + model.devid + "]";
            string body = Util.DoPost1(url, para, user, "");
            result = JsonConvert.DeserializeObject<Elec<List<ElecStatic>>>(body);
            return result;
        }
        //用电量小计，主要用于统计每月的累计用电
        public Elec<List<ElecStatic>> addupmonth(ElecStatic model, ElecUser user)
        {
            Elec<List<ElecStatic>> result = new Elec<List<ElecStatic>>();
            ElecUser req = new ElecUser();
            req.Uuid = user.Uuid;
            req.Expand = user.Expand;
            if (req == null)
            {
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            string url = servserurl + "device/ammeter/report/" + model.devid + "/" +model.bdate.ToString("yyyy-MM-dd")+"/"+model.edate.ToString("yyyy-MM-dd");
            string body = Util.DoGet1(url, comdic(), user);
            result = JsonConvert.DeserializeObject<Elec<List<ElecStatic>>>(body);
            return result;
        }
        public List<HouseLockQuery> lockquery(List<string> str)
        {
            var mo = from m in HouseQuery where str.Contains(m.ElecId) select m;
            return mo.ToList();
        }
        public HouseLockQuery lockquery1(string str)
        {
            var mo = from m in HouseQuery where m.ElecId==str select m;
            return mo.FirstOrDefault();
        }
        public HouseLockQuery lockquery2(string str)
        {
            var mo = from m in HouseQuery where m.UuId == str select m;
            return mo.FirstOrDefault();
        }
        public List<HouseLockQuery> lockquery3(long  houseid)
        {
            var mo = from m in HouseQuery where m.Id == houseid select m;
            return mo.ToList();
        }
        public List<HouseLockQuery> lockquery4(string  LocalId)
        {
            var mo = from m in HouseQuery where m.LocalId == LocalId select m;
            return mo.ToList();
        }
        //新增电表账号或者科技侠账号
        public long addelec(ElecUser model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_ELECTRIC')");
                AddModel<ElecUser>(model);
            }
            else
            {
                ModifiedModel<ElecUser>(model, false, new string[] {});
            }
            return model.Id;
        }
        public DbSet<ElecUser> ElecUser { get; set; }
        public DbSet<HouseLockQuery> HouseQuery { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ElecUserMapping());
            modelBuilder.Configurations.Add(new HouseQueryLockMapping());
        }

    }
}
