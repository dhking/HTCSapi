using Api.CommonControllers;
using API.CommonControllers;
using ControllerHelper;
using Microsoft.Owin;
using Model;
using Model.Base;
using Model.User;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
namespace Api.Controllers
{
    public class StatisticsController : DataCenterController
    {
        StatisticsService service = new StatisticsService();
        [Route("api/Statistics/Query")]
        public SysResult<StatisticsModel> Query(dynamic obj)
        {
            DateTime date = Convert.ToDateTime(obj.date);
            int housetype= Convert.ToInt16(obj.housetype);
            return service.querystatic(date,housetype);
        }
        [Route("api/Bw/Query")]
        public SysResult<IList<T_memo>> Query(T_memo model)
        {
            SysResult<IList<T_memo>> sysresult = new SysResult<IList<T_memo>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Querybase(model, this.OrderablePagination);
            return sysresult;
        }
        [Route("api/Bw/add")]
        public SysResult bwadd(T_memo model)
        {
            SysResult result = new SysResult();
            service.Savememo(model);
            return result;
        }
        //app财务统计今天
        [Route("api/appFinance/todayQuery")]
        public SysResult<WrapStatistics> todayQuery()
        {
            return service.todayQuery();
        }
        //app运营分析
        [Route("api/appAnalysis/Query")]
        public SysResult<WrapAnalysis> AnalysisQuery()
        {
            return service.AnalysisQuery();
        }
        //首页数据统计 sp_homestatistics
        [Route("api/appHome/Query")]
        public SysResult<WrapHome> appHome()
        {
            return service.HomeQuery();
        }
        //运营统计
        [JurisdictionAuthorize(name = new string[] { "tongji/" })]
        [Route("api/PCHome/Query")]
        public SysResult<WrappcStatic> PCHome()
        {
            return service.PCHome();
        }
        //月度统计
        [Route("api/PCHome/Query1")]
        public SysResult<MonthPersent1> PCHome1(MonthPersent month)
        {
            SysResult<MonthPersent1> sysresult = new SysResult<MonthPersent1>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            month.CompanyId = user.CompanyId;
            return service.PCHome1(month);
        }
    }
}