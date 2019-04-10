using Api.CommonControllers;
using API.CommonControllers;
using DBHelp;
using Model;
using Model.Base;
using Model.House;
using Newtonsoft.Json;
using Service;
using Service.ZK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers.ZK
{
    public class HouseZKController : DataCenterController
    {
        HoseZKService service = new HoseZKService();
        //热门房源
        [Route("api/House/zkhouserm")]
        [HttpPost]
        public SysResult<IList<HouseZK>> zkhouserm(HouseZK model)
        {
            SysResult<IList<HouseZK>> sysresult = new SysResult<IList<HouseZK>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            model.IsRm = 1;
            sysresult = service.Queryrmhouse(model, this.OrderablePagination);
            return sysresult;
        }
        //找房列表
        [Route("api/House/zkhouselist")]
        [HttpPost]
        public SysResult<IList<HouseZK>> zkhouselist(HouseZK model)
        {
            SysResult<IList<HouseZK>> sysresult = new SysResult<IList<HouseZK>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Queryrmhouse(model, this.OrderablePagination);
            return sysresult;
        }
        //特色列表
        [Route("api/BasicZK/tese")]
        [HttpPost]
        public SysResult<List<T_Basics>> tese()
        {
            SysResult<List<T_Basics>> sysresult = new SysResult<List<T_Basics>>();
            sysresult = service.Querytese();
            return sysresult;
        }
        //详情页面
        [Route("api/House/xq")]
        [HttpPost]
        public SysResult<HouseZK> xq(HouseZK model)
        {
            SysResult<HouseZK> sysresult = new SysResult<HouseZK>();
            sysresult = service.xq(model);
            return sysresult;
        }
        //预约信息提交
        [Route("api/Appointment/save")]
        [HttpPost]
        public SysResult save(Appointment model)
        {
            SysResult sysresult = new SysResult();
            sysresult = service.Save(model);
            return sysresult;
        }
    }
}