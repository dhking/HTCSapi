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
    public class ZafeiController : DataCenterController
    {
        ZafeiServcie service = new ZafeiServcie();
        [Route("api/Zafei/QueryZafeilist")]
        public SysResult<List<T_ZafeiList>> Queryhouselist(T_ZafeiList model)
        {
            SysResult<List<T_ZafeiList>> sysresult = new SysResult<List<T_ZafeiList>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Querymenufy(model, this.OrderablePagination);
            return sysresult;
        }
        //新增费用项弹框
        [Route("api/Zafei/Zafeilist")]
        public SysResult<List<T_ZafeiList>> Zafeilist()
        {
            SysResult<List<T_ZafeiList>> sysresult = new SysResult<List<T_ZafeiList>>();
            sysresult = service.Querymenu();
            return sysresult;
        }
        
        [Route("api/Zafei/Savezafei")]
        public SysResult Savezafei(T_ZafeiList model)
        {
            return service.Savezafei(model);
        }
        [Route("api/Zafei/Queryid")]
        public SysResult<T_ZafeiList> Queryid(T_ZafeiList model)
        {
            return service.Queryid(model);
        }
        [Route("api/Zafei/delete")]
        [HttpPost]
        public SysResult delete(iids ids)
        {
            string[] model = ids.ids.Split(","[0]);
            SysResult result = new SysResult(0, "删除成功");
            foreach (var mo in model)
            {
                result = service.deleteData(long.Parse(mo));
            }
            return result;
        }
    }
}