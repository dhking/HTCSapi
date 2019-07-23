using API.CommonControllers;
using Model;
using Model.Menu;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{

    public class SetController : DataCenterController
    {
        SetService service = new SetService();
        [Route("api/Set/Queryip")]
        public SysResult<indepent> Queryip(indepent model)
        {
            return service.Query(model);
        }

    }
}