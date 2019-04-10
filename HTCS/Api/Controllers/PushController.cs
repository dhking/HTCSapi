using Api.CommonControllers;
using API.CommonControllers;
using ControllerHelper;
using DBHelp;
using Microsoft.Owin;
using Model;
using Model.Base;
using Model.Bill;
using Model.User;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
namespace Api.Controllers
{
    public class PushController : DataCenterController
    {
        PushService service = new PushService();
        [Route("api/Push/Push")]
        public SysResult Push(ParamPhsh model)
        {
            return service.Push(model);
        }
        [Route("api/SysMessage/List")]
        public SysResult<List<T_SysMessage>> ListMessage(T_SysMessage model)
        {
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            return service.ListMessage(model, this.OrderablePagination);
        }
    }
}