using API.CommonControllers;
using Model;
using Model.Menu;
using Model.User;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{
    public class MenuController : DataCenterController
    {
        MenuService service = new MenuService();
        [JurisdictionAuthorize(name = new string[] { "menu" })]
        [Route("api/Menu/Querymenu")]
        public Model.SysResult<List<T_Menu>> Querymenu()
        {
           return  service.Querymenu();
        }
        [Route("api/Menu/Querylist")]
        [HttpGet]
        public Model.SysResult<List<T_Menu>> Querylist([FromUri]T_SysUser user)
        {
            T_SysUser sysuser = GetCurrentUser(GetSysToken());
            SysResult<List<T_Menu>> reslut = new SysResult<List<T_Menu>>();
            if (sysuser == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            return service.Querylist(sysuser);
        }
        [Route("api/Menu/hQuerylist")]
        [HttpPost]
        public Model.SysResult<List<T_Menu>> hQuerylist(T_SysUser user)
        {
            return service.Querylist1(user, 2);
        }
        [Route("api/Menu/QueryMenulist")]
        public SysResult<List<T_Menu>> Queryhouselist(T_Menu model)
        {
            SysResult<List<T_Menu>> sysresult = new SysResult<List<T_Menu>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Querymenufy();
            return sysresult;

        }
        [Route("api/Menu/SaveMenu")]
        [JurisdictionAuthorize(name = new string[] { "menu-add-btn", "menu-edit-btn" })]
        
        public SysResult SaveMenu(T_Menu model)
        {
            return service.Savemenu(model);
        }
        [Route("api/Menu/delete")]
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
        [Route("api/Menu/Queryid")]
        public SysResult<T_Menu> Queryid(T_Menu model)
        {
            return service.Queryid(model);
        }
    }
}