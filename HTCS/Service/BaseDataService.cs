using ControllerHelper;
using DAL;
using Microsoft.International.Converters.PinYinConverter;
using Model;
using Model.Base;
using Model.Bill;
using Model.Contrct;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service
{
    public  class BaseDataService
    {
        BaseDataDALL dal = new BaseDataDALL();
        CityDAL citydal = new CityDAL();
      
        public SysResult<IList<T_V_basicc>> Querybase(T_Basics model, OrderablePagination orderablePagination)
        {
            return dal.Querylist(model, orderablePagination);
        }
        public SysResult<IList<T_bankcard>> bankcardQuery()
        {
            SysResult<IList<T_bankcard>> result = new SysResult<IList<T_bankcard>>();
            result.numberData = dal.bankcardQuery();
            return result;
        }
        public SysResult<List<T_template>> templateQuery(T_template model)
        {
            SysResult<List<T_template>> result = new SysResult<List<T_template>>();
           
            result.numberData = dal.templateQuery(model);
            return result;
        }
        public SysResult<T_template> xqQuery(T_template model)
        {
            SysResult<T_template> result = new SysResult<T_template>();

            result.numberData = dal.xqQuery(model);
            return result;
        }
        public SysResult<T_template> morenQuery(WrapContract model)
        {
            SysResult<T_template> result = new SysResult<T_template>();
            ContrctDAL contdal = new ContrctDAL();
            //合同信息
            WrapContract wrap = contdal.QueryId(model);
            if (wrap == null)
            {
                result.Code = 1;
                result.Message = "合同不存在";
                return result;
            }
            T_template temp = dal.morenQuery(new T_template() { isdefault = 1, CompanyId = model.CompanyId });
            if (temp == null)
            {
                result.Code = 1;
                result.Message = "请至少添加一条合同模板并设置为默认";
                return result;
            }
            temp.onlinesign = wrap.onlinesign;
            temp.content = replace(wrap, temp.htmlcontent);
            result.numberData = temp;
            return result;
        }
        public string replace(WrapContract wrap,string str)
        {
            string oristr = str;
            //查询公司信息
            BaseDataDALL bdal = new BaseDataDALL();
            T_account account = bdal.queryaccount(wrap.CompanyId);
            //查询账单
            BillDAL bildal = new BillDAL();
            T_Bill bill= bildal.queryby(new Model.Bill.T_Bill() { ContractId = wrap.Id, stage = 1 });
            if (account != null)
            {
                string zujinlist = "";
                string otherlist = "";
                string paystr = "";
                string firtpay = "";
                zujinlist = "月租金" + wrap.Recent +"元;"+ wrap.Pinlv + "月1付";
                if (wrap.Otherfee != null)
                {
                    foreach (var mo in wrap.Otherfee)
                    {
                        otherlist += mo.Name+";";
                    }
                }
                if (wrap.Recivetype == 1)
                {
                    paystr = "账单开始前"+wrap.BeforeDay+"天收租";
                }
                if (wrap.Recivetype == 2)
                {
                    paystr = "固定每月" + wrap.BeforeDay + "号收租";
                }
                if (bill != null)
                {
                    firtpay = bill.Amount.ToStr();
                }
                Dictionary<string, string> dicstring = new Dictionary<string, string>();
                dicstring.Add("{甲方品牌}", account.brand);
                dicstring.Add("{甲方名称}", account.name);
                dicstring.Add("{乙方名称}", wrap.Teant.Name);
                dicstring.Add("{租客姓名}", wrap.Teant.Name);
                dicstring.Add("{租客证件号}", wrap.Teant.Document);
                dicstring.Add("{租客电话}", wrap.Teant.Phone);
                dicstring.Add("{城市}", wrap.CityName);
                dicstring.Add("{物理地址}", wrap.HouseName);
                dicstring.Add("{楼层}", wrap.floor.ToStr());
                dicstring.Add("{总层高}", wrap.allfloor.ToStr());
                dicstring.Add("{房间面积}", wrap.mesure.ToStr());
                dicstring.Add("{合同起始日期}", wrap.BeginTime.ToStr());
                dicstring.Add("{合同结束日期}", wrap.EndTime.ToStr());
                dicstring.Add("{合同总周期}", (wrap.EndTime - wrap.BeginTime).Days.ToStr());
                dicstring.Add("{交房日期}", wrap.BeginTime.ToStr());
                dicstring.Add("{押金金额}", wrap.Deposit.ToStr());
                dicstring.Add("{租金清单}", zujinlist);
                dicstring.Add("{其他费用清单}", otherlist);
                dicstring.Add("{支付时间}", paystr);
                dicstring.Add("{首付款金额}", firtpay);
                foreach (var mo in dicstring)
                {
                    oristr = oristr.Replace(mo.Key, mo.Value);
                }
            }
            return oristr;
        }
        public SysResult templateadd(T_template model)
        {
            SysResult result = new SysResult();
            model.NotUpdatefield = new string[] { "title","isdefault", "CompanyId", "ispublic" };
            dal.templateadd(model);
            result.Code =0;
            result.Message ="保存成功";
            return result;
        }
        public SysResult templateadd1(T_template model)
        {
            SysResult result = new SysResult();
            ProceDAL procedal = new ProceDAL();
            procedal.Cmdproce8(new Pure(){ Spname = "sp_changemoren",Id=model.Id.ToStr(), Other = model.isdefault.ToStr()});
            result.Code = 0;
            result.Message = "设置成功";
            return result;
        }
        public SysResult<IList<T_basicsType>> Querybasetype(T_basicsType model, OrderablePagination orderablePagination)
        {
            return dal.Querylisttype(model, orderablePagination);
        }

        public SysResult<IList<T_V_basicc>> Querybasedata(T_V_basicc model, OrderablePagination orderablePagination)
        {
            return dal.Querylistdata(model, orderablePagination);
        }
        public SysResult<List<T_basicsType>> Querybasetype(T_basicsType model)
        {
            SysResult<List<T_basicsType>> result = new SysResult<List<T_basicsType>>();
            result.numberData = dal.Querylisttype(model);
            return result;
        }
        public SysResult<List<WrapCity>> Querycity(City model)
        {
            SysResult <List<WrapCity>> result = new SysResult<List<WrapCity>>();
            result.numberData = citydal.Querycity(model);
            return result;
        }

        public SysResult<List<City>> Querycity1(City model)
        {
            SysResult<List<City>> result = new SysResult<List<City>>();
            result.numberData = citydal.Querycity1(model);
            return result;
        }
        public SysResult<WrapBasic> Query(Queryparam model)
        {
            UserDAL1 userdal = new UserDAL1();
            CityDAL citydal = new CityDAL();
            SysResult<WrapBasic> result = new SysResult<WrapBasic>();
            WrapBasic wrap = new WrapBasic();
            if (model.teseorpeibei == 0)
            {
                model.paratype = "publicpeibei";
                wrap.peipei = dal.Querylist1(model);
                model.paratype = "puclictese";
                wrap.tese = dal.Querylist1(model);
            }
            if (model.teseorpeibei == 1)
            {
                model.paratype = "publicpeibei";
                wrap.peipei = dal.Querylist1(model);
            }
            if (model.teseorpeibei == 2)
            {
                model.paratype = "puclictese";
                wrap.tese = dal.Querylist1(model);
            }
            //wrap.user = userdal.QuerylistUser(new T_SysUser() { });
            //wrap.city = citydal.Querycity(null);
            result.numberData =wrap;
            return result;
        }
        //查询房管员
        public SysResult<List<T_SysUser>> Queryfgy(T_SysUser model)
        {
            UserDAL1 userdal = new UserDAL1();
            CityDAL citydal = new CityDAL();
            SysResult<List<T_SysUser>> result = new SysResult<List<T_SysUser>>();
            List<T_SysUser> listuser = new List<T_SysUser>();
            foreach(var mo in listuser)
            {
                if (mo.RealName == null)
                {
                    mo.RealName = mo.Mobile;
                }
            }
            result.numberData = userdal.QuerylistUser(model);
            return result;
        }
        //查询托管数据
        public SysResult<List<T_Basics>> Querytg(T_Basics model)
        {
            UserDAL1 userdal = new UserDAL1();
            CityDAL citydal = new CityDAL();
            List<T_Basics> list = new List<T_Basics>();
            model.Code = "Managedaddress";
            T_Basics basic1= dal.Querylist2(model);
            list.Add(basic1);
            model.Code = "Trusteeship";
            T_Basics basic2 = dal.Querylist2(model);
            list.Add(basic2);
            SysResult<List<T_Basics>> result = new SysResult<List<T_Basics>>();
            result.numberData = list;
            return result;
        }

        public SysResult Savetuoguan(t_tuoguan model)
        {
            SysResult result = new SysResult();
            if( dal.savebill(model) > 0)
            {
                result=result.SuccessResult("保存成功");
            }
            else
            {
                result = result.FailResult("保存失败");
            }
            return result;
        }
        public SysResult SaveData(T_Basics model)
        {
            SysResult result = new SysResult();
            if (model.Id == 0)
            {
                if (dal.Savedata(model) > 0)
                {
                    return result = result.SuccessResult("保存成功");
                }
                else
                {
                    return result = result.FailResult("保存失败");
                }
            }
            else
            {
                if (dal.Updatedata(model) > 0)
                {
                    return result = result.SuccessResult("更新成功");
                }
                else
                {
                    return result = result.FailResult("更新失败");
                }
            }
            
        }
        public SysResult SaveTypeData(T_basicsType model)
        {
            SysResult result = new SysResult();
            if (model.Id == 0)
            {
                if (dal.SaveTypedata(model) > 0)
                {
                    return result = result.SuccessResult("保存成功");
                }
                else
                {
                    return result = result.FailResult("保存失败");
                }
            }
            else
            {
                if (dal.UpdateTypedata(model) > 0)
                {
                    return result = result.SuccessResult("更新成功");
                }
                else
                {
                    return result = result.FailResult("更新失败");
                }
            }
        }
        public SysResult deleteData(long ids)
        {
          
            SysResult result = new SysResult();
            
            if (dal.deletedata(ids) > 0)
            {
                return result = result.SuccessResult("删除成功");
            }
            else
            {
                return result = result.FailResult("删除失败");
            }
        }
        public SysResult<T_Basics> Queryid(T_Basics model)
        {
            SysResult<T_Basics> result = new SysResult<T_Basics>();
            T_basicsType modeltype=new T_basicsType ();
            T_Basics baseic = dal.QueryId(model);
            baseic.listparatype = dal.Querylisttype(modeltype);
            result.numberData = baseic;
            return result;
        }
        public SysResult<BanBen> Querybanben(BanBen model)
        {
            SysResult<BanBen> result = new SysResult<BanBen>();
            BanbenDAL banbendal = new BanbenDAL();
            result.numberData = banbendal.Query(model);
            return result;
        }
        public SysResult<T_basicsType> Querytypeid(T_basicsType model)
        {
            SysResult<T_basicsType> result = new SysResult<T_basicsType>();

            result.numberData = dal.QueryTypeId(model);
            return result;
        }

        public string getCode(string name)
        {
            
            var chs = name.ToCharArray();
            string pinyins = "";
            var stop = true;
            foreach (var c in chs)
            {
                ChineseChar cc = new ChineseChar(c);
                pinyins +=cc.Pinyins.FirstOrDefault();
            }
           
            while (stop)
            {
                if (dal.QueryCount(pinyins)== 0)
                {
                    stop = false;
                }
                else
                {
                    Random rd = new Random();
                    pinyins = pinyins + rd.Next(1, 10);
                }
            }
            return pinyins;
        }
    }
}
