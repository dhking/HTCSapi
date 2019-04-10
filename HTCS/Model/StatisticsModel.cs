using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    //财务分析
    public class WrapStatistics
    {
        public FinanceStatistics NowFinance { get; set; }

        public List<SevenFinance> SevenFinance { get; set; }
    }

    public class t_appstatistics:BasicModel
    {

        public long  Id { get; set; }
        public long CompanyId { get; set; }
        public decimal Value { get; set; }
      
        public string  Key { get; set; }

    }
    public class FinanceStatistics
    {

        public DateTime Date { get; set; }
        //应收
        public decimal Receivable { get; set; }
        //已收
        public decimal Payed { get; set; }
        //待收
        public decimal Notpay { get; set; }

    }
    //未来七天数据
    public class SevenFinance
    {
        public decimal Receivable { get; set; }

        public decimal ShouldPay { get; set; }

        public DateTime Date { get; set; }
    }
    //运营数据
    public class WrapAnalysis
    {
        public List<Vacant> Vacant { get; set; }

        public VTuizu VTuizu { get; set; }

        public Business Business { get; set; }
    }
    public class StaticHouse
    {
        public DateTime month { get; set; }
        [JsonProperty("INTOHOUSE")]
        public long intohouse { get; set; }
        [JsonProperty("OUTHOUSE")]

        public long outhouse { get; set; }
    }
    //pc端统计数据
    public class WrappcStatic
    {
        //拿房删房
        public List<StaticHouse> Table { get; set; }

        //入住率
        public List<Vacant> Table1 { get; set; }

        //库存
        public List<chushou> Table2 { get; set; }

        //房源出收

        public List<Stock> Table3 { get; set; }

        //月度统计

        public List<MonthPersent> Table4 { get; set; }
    }
    //月度统计
    public class MonthPersent
    {

        public long dataall { get; set; }

        public long dataself { get; set; }

        public int type { get; set; }
        public decimal percent { get; set; }

        public DateTime month { get; set; }
    }
    public class chushou
    {
        public int mtype { get; set; }

        public long today { get; set; }

        public long week { get; set; }

        public long mmonth { get; set; }
    }
    //入住
    public class Vacant
    {
        public decimal Percent { get; set; }

        public DateTime date { get; set; }


        public DateTime month { get; set; }

    }
    //退租
    public class VTuizu
    {
        //总数
        public long GeneraNumber { get; set; }
        //异常
        public long Metaphase { get; set; }
        //到期退租
        public long Expire { get; set; }

    }
    //业务转化
    public class Business
    {
        //总数
        public long GeneraNumber { get; set; }
        //已跟进
        public long Follow { get; set; }
        //已签约
        public long Contract { get; set; }

    }

    public class StatisticsModel
    {
        public decimal CopeWith { get; set; }

        public decimal Paid { get; set; }

        public decimal Receivable { get; set; }
        public decimal Receivableed { get; set; }
        public int Appointment { get; set; }
        public long Repair { get; set; }
        public int Book { get; set; }
        public Stock Stock { get; set; }

    }
    public class Stock
    {
        public int v_all { get; set; }
        public int ALL { get; set; }
        public int Configuration { get; set; }
        public int Vacancy { get; set; }
        public int Book { get; set; }
        public int Rent { get; set; }
        //空置率
        public string RentPert { get; set; }
        public int VacancyRate { get; set; }
        public int Vacancy10 { get; set; }
        public int Vacancy20 { get; set; }
        public int Vacancy30 { get; set; }
        public int Vacancyover30 { get; set; }
    }
    public class T_memo : BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Content { get; set; }
        public DateTime Pdate { get; set; }
        public long UserId { get; set; }
        public int Status { get; set; }

        public string Asress { get; set; }
    }
    //APP首页统计
    public class WrapHome
    {
        //提示数字
        public HomeNumber home { get; set; }
        //财务统计
        public caiwu caiwu { get; set; }
        //库存
        public kongzhi kongzhi { get; set; }
        //收房
        public house shouse { get; set; }
        //出房
        public house chouse { get; set; }
        //客源信息
        public guest guest { get; set; }
    }

    //数量
    public class HomeNumber
    {
        public long daishou { get; set; }

        public long daifu { get; set; }
        public long dairu { get; set; }
        public long daitui { get; set; }

        public long daichao { get; set; }

        public long daiweixiu { get; set; }


    }

    public class caiwu
    {
        public int Month { get; set; }
        public decimal realshouru { get; set; }

        public decimal realzhifu { get; set; }

        public decimal viralshou { get; set; }

        public decimal viralfu { get; set; }

    }

    public class kongzhi
    {
        public string percent { get; set; }

       // public string percentstr { get; set; }
        public long all { get; set; }

        public long kz { get; set; }

    }

    public class house
    {
        public long today { get; set; }

        public long Thisweek { get; set; }

        public long ThisMonth { get; set; }

    }
    public class guest
    {
        public long daikan { get; set; }

        public long addkeyuan { get; set; }
    }

}
