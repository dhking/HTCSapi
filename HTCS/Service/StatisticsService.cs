using ControllerHelper;
using DAL;
using DBHelp;
using Model;
using Model.Bill;
using Model.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public   class StatisticsService
    {
        StatisticsDAL dal = new StatisticsDAL();
        public SysResult<IList<T_memo>> Querybase(T_memo model, OrderablePagination orderablePagination)
        {
            SysResult<IList<T_memo>> result = new SysResult<IList<T_memo>>();
            int count = 0;
            result.numberData= dal.Query(model, orderablePagination,out count);
            result.numberCount = count;
            return result;
        }
        public SysResult<StatisticsModel> querystatic(DateTime date,int housetype,long companyid, long[] userids,T_SysUser user)
        {
            SysResult<StatisticsModel> result = new SysResult<StatisticsModel>();
            BillDAL bill = new BillDAL();
            ContrctDAL contract = new ContrctDAL();
            StatisticsModel remodel = new StatisticsModel();
            WrapStatistics wrap = new WrapStatistics();
            List<long> depentids = new List<long>();
            int range = 0;
            long userid = 0;
            if (user.departs != null && user.roles != null)
            {
                depentids= user.departs.Select(p => p.Id).ToList();
                userid=user.Id;
            }
            range = user.range;
            DataSet ds = dal.indexStatisticsQuery(companyid,depentids,userids, userid,range);
            DataTable dt = ds.Tables[0];
            remodel.todreveive = decimal.Parse(dt.Rows[0]["V_TODREVEIVE"].ToStr());
            remodel.todpay = decimal.Parse(dt.Rows[0]["V_TODPAY"].ToStr());
            remodel.overreveive = decimal.Parse(dt.Rows[0]["V_OVERREVEIVE"].ToStr());
            remodel.overduepay = decimal.Parse(dt.Rows[0]["V_OVERDUEPAY"].ToStr());
            remodel.Repair = long.Parse(dt.Rows[0]["V_DAIXIU"].ToStr());
            remodel.Appointment = long.Parse(dt.Rows[0]["V_APPOINTMENT"].ToStr());
            remodel.Book = 0;
            if (housetype == 1)
            {
                HouseDAL dal = new HouseDAL();
                remodel.Stock = new Stock();
                remodel.Stock =dal.Query(housetype,companyid);
            }
            if (housetype == 2)
            {
                HousePentDAL dal = new HousePentDAL();
                remodel.Stock = new Stock();
                remodel.Stock = dal.StockQuery(housetype, companyid);
            }
            if (housetype == 3)
            {
                HousePentDAL dal = new HousePentDAL();
                remodel.Stock = new Stock();
                remodel.Stock = dal.StockQuery(housetype, companyid);
            }
            result.numberData = remodel;
            return result;
        }
        //APP财务分析
        public SysResult<WrapStatistics> todayQuery(long companyid)
        {
            SysResult<WrapStatistics> result = new SysResult<WrapStatistics>();
            WrapStatistics wrap = new WrapStatistics();
            DataSet ds = dal.caiwuStatisticsQuery(companyid);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            List<SevenFinance> listseven = new List<SevenFinance>();
            Hashtable hash = new Hashtable() { };
            hash.Add("RDATE", "Date");
            listseven = EntityHelper.GetEntityListByDT<SevenFinance>(dt, hash);
            wrap.SevenFinance = listseven;
            FinanceStatistics now = new FinanceStatistics();
            now.Date = DateTime.Now;
            now.Receivable= decimal.Parse(dt1.Rows[0]["V_RECEIVABLE"].ToStr());
            now.Payed = decimal.Parse(dt1.Rows[0]["V_PAYED"].ToStr());
            now.Notpay = decimal.Parse(dt1.Rows[0]["V_NOTPAY"].ToStr());
            wrap.NowFinance = now;
            result.numberData = wrap;
            return result;
        }
        //运营数据
        public SysResult<WrapAnalysis> AnalysisQuery()
        {
            SysResult<WrapAnalysis> result = new SysResult<WrapAnalysis>();
            WrapAnalysis wrap = new WrapAnalysis();
            //入住率
            DataSet ds = dal.StatisticsQuery();
            DataTable dt = ds.Tables[0];
           
            List<Vacant> list = new List<Vacant>();
            for(int i = 1; i <= 7; i++)
            {
                Vacant va = new Vacant();
                va.Percent = i;
                va.date = DateTime.Now.AddDays(-i);
                list.Add(va);
            }

            //退租
            VTuizu tuizu = new VTuizu();
            tuizu.GeneraNumber= long.Parse(dt.Rows[0]["V_TKCOUNT"].ToStr()); 
            tuizu.Expire = long.Parse(dt.Rows[0]["V_ZCTKCOUNT"].ToStr()); 
            tuizu.Metaphase = tuizu.GeneraNumber - tuizu.Expire;
            //业务转换率
            Business business = new Business();
            business.GeneraNumber= long.Parse(dt.Rows[0]["V_GUESTCOUNT"].ToStr());
            business.Follow = long.Parse(dt.Rows[0]["V_GJCOUNT"].ToStr());
            business.Contract = long.Parse(dt.Rows[0]["V_QYCOUNT"].ToStr());
            wrap.Vacant = list;
            wrap.VTuizu = tuizu;
            wrap.Business = business;
            result.numberData = wrap;
            return result;
        }
        //首页的数据统计
        public SysResult<WrapHome> HomeQuery(long companydid)
        {
            SysResult<WrapHome> result = new SysResult<WrapHome>();
            HouseDAL hdal = new HouseDAL();
            WrapHome wrap = new WrapHome();
            
            HomeNumber homenumber = new HomeNumber();
           
            DataSet ds = dal.Statisticsmessage(companydid);
            DataTable dt = ds.Tables[0];
            homenumber.daishou = long.Parse(dt.Rows[0]["daishou"].ToStr());
            homenumber.daifu = long.Parse(dt.Rows[0]["daifu"].ToStr());
            homenumber.daichao = long.Parse(dt.Rows[0]["daichao"].ToStr());
            homenumber.daitui = long.Parse(dt.Rows[0]["daitui"].ToStr());
            homenumber.daiweixiu = long.Parse(dt.Rows[0]["daiweixiu"].ToStr());
            homenumber.daishouamount = 10;
            homenumber.daifuamount = 10;
            //查询统计数据
            caiwu caiwu = new caiwu();
            kongzhi kz = new kongzhi();
            house shouse = new house();
            house chouse = new house();
            guest guest = new guest();
            List<t_appstatistics> appstatistics = dal.queryappstatistics(new t_appstatistics() {CompanyId= companydid });
            foreach(var mo in appstatistics)
            {
                if(mo.Key== "realshouru")
                {
                    caiwu.realshouru = mo.Value;
                }
                if (mo.Key == "realzhifu")
                {
                    caiwu.realzhifu = mo.Value;
                }
                if (mo.Key == "viralshou")
                {
                    caiwu.viralshou = mo.Value;
                }
                if (mo.Key == "viralfu")
                {
                    caiwu.viralfu = mo.Value;
                }
               
                if (mo.Key == "Month")
                {
                    caiwu.Month = (int)mo.Value;
                }
                
                //查询空置率
                Stock stock = hdal.querykz(companydid);
                kz.kz =stock.Vacancy;
                kz.all = stock.ALL;
                kz.percent = stock.RentPert;
                //查询今日新签租客合同和新签业主合同
                ContrctDAL cdal = new ContrctDAL();
                chouse.today = cdal.Querycount(new Model.Contrct.T_Contrct() { CreateTime = DateTime.Now.Date });
                OwerContractDAL owerdal = new OwerContractDAL();
                shouse.today = owerdal.Querycount(new Model.Contrct.T_OwernContrct() { CreateTime = DateTime.Now.Date });

                if (mo.Key == "Thisweek")
                {
                    shouse.Thisweek = (int)mo.Value;
                }
                if (mo.Key == "ThisMonth")
                {
                    shouse.ThisMonth = (int)mo.Value;
                }

                
                if (mo.Key == "cThisweek")
                {
                    chouse.Thisweek = (int)mo.Value;
                }
                if (mo.Key == "cThisMonth")
                {
                    chouse.ThisMonth = (int)mo.Value;
                }

                if (mo.Key == "daikan")
                {
                    guest.daikan = (int)mo.Value;
                }
                if (mo.Key == "addkeyuan")
                {
                    guest.addkeyuan = (int)mo.Value;
                }
            }
            wrap.home = homenumber;

            wrap.caiwu = caiwu;

            wrap.kongzhi = kz;

            wrap.shouse = shouse;

            wrap.chouse = chouse;

            wrap.guest = guest;

            result.numberData = wrap;

            return result;
        }
        //pc端数据统计
        public SysResult<WrappcStatic> PCHome(long companyid)
        {
            SysResult<WrappcStatic> result = new SysResult<WrappcStatic>();
            WrappcStatic wrap = new WrappcStatic();
            DataSet ds = dal.StatisticsPCQuery(companyid);
            //拿房删房数
            wrap.Table = EntityHelper.GetEntityListByDT<StaticHouse>(ds.Tables[0],null);
            wrap.Table1 = EntityHelper.GetEntityListByDT<Vacant>(ds.Tables[1], null);
            wrap.Table2 = EntityHelper.GetEntityListByDT<chushou>(ds.Tables[2], null);
            wrap.Table3 = EntityHelper.GetEntityListByDT<Stock>(ds.Tables[3], null);
            result.numberData = wrap;
            return result;
        }
        //pc端数据统计2
        public SysResult<MonthPersent1> PCHome1(MonthPersent model)
        {
            SysResult<MonthPersent1> result = new SysResult<MonthPersent1>();
            MonthPersent1 wrap = new MonthPersent1();
            DataSet ds = dal.StatisticsPCQuery1(model.month,model.CompanyId);
            //空置率和续租率
            List<MonthPersent> Table4 = EntityHelper.GetEntityListByDT<MonthPersent>(ds.Tables[0], null);
            if (Table4 != null)
            {
                foreach (var mo in Table4)
                {
                    mo.percentstr = (mo.percent * 100).ToString();
                    if (mo.type == 2)
                    {
                        wrap.xz = mo;
                    }
                    if (mo.type == 3)
                    {
                        wrap.kz = mo;
                    }
                }
            }
            //查询财务信息
            caiwu caiwu = new caiwu();
            List<t_appstatistics> appstatistics = dal.queryappstatistics(new t_appstatistics() { Value = model.month.Month });
            if (appstatistics != null)
            {
                foreach (var mo in appstatistics)
                {
                    if (mo.Key == "realshouru")
                    {
                        caiwu.realshouru = mo.Value;
                    }
                    if (mo.Key == "realzhifu")
                    {
                        caiwu.realzhifu = mo.Value;
                    }
                    if (mo.Key == "viralshou")
                    {
                        caiwu.viralshou = mo.Value;
                    }
                    if (mo.Key == "viralfu")
                    {
                        caiwu.viralfu = mo.Value;
                    }
                    caiwu.realjieyu = caiwu.realshouru - caiwu.realzhifu;
                    caiwu.viraljieyu = caiwu.viralshou - caiwu.viralfu;
                    if (mo.Key == "Month")
                    {
                        caiwu.Month = (int)mo.Value;
                    }
                }
            }
            wrap.caiwu = caiwu;
            result.numberData = wrap;
            return result;
        }
        public SysResult Savememo(T_memo model)
        {
            SysResult sysresult = new SysResult();
            if (dal.Savememo(model) > 0)
            {
                sysresult = sysresult.SuccessResult("保存成功");
            };
            return sysresult;
        }
    }
}
