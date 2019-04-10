using API.CommonControllers;
using Model;
using Model.Map;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{
    public class MapController : DataCenterController
    {
        MapService service = new MapService();
        [Route("api/Map/Querybuxiaoqu")]

        public SysResult<List<tip>> Querybuxiaoqu(QueryMap param)
        {
            return service.Query(param); 
        }
        [Route("api/Map/Queryadress")]
       
        public SysResult<List<pois>> Queryadress(QueryMap param)
        {
            return service.Querymap(param); 
        }
        //查询商圈
        [Route("api/Map/QueryArea")]
        [HttpPost]
        public SysResult<QueryArea> QueryArea(QueryArea model)
        {
            return service.Querymap(model);
        }
        //pc端查询商圈
        [Route("api/Map/PCQueryArea")]
        [HttpPost]
        public SysResult<List<districts>> PCQueryArea(QueryArea model)
        {
            return service.PCQuerymap(model);
        }
    }
}