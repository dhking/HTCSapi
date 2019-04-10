using API.CommonControllers;
using ControllerHelper;
using DAL.Common;
using Microsoft.Owin;
using Model;
using Model.Base;
using Model.Bill;
using Model.Menu;
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
    public class AccountBankController : DataCenterController
    {
        AcountBankService service = new AcountBankService();
         //收款账户
        [Route("api/Account/list")]
        public SysResult<List<accountbank>> Accountlist(accountbank model)
        {
            SysResult<List<accountbank>> result = new SysResult<List<accountbank>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                result.Code = 1002;
                result.Message = "请登录先";
                return result;
            }
            model.CompanyId = user.CompanyId;
            return service.Accountlist(model);
        }
        //保存账户
        [Route("api/AccountBank/save")]
        public SysResult Accountsave(accountbank model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                result.Code = 1002;
                result.Message = "请登录先";
                return result;
            }
            model.CompanyId = user.CompanyId;
            return service.Accountsave(model);
        }
        //删除账号
        [Route("api/AccountBank/delete")]
        public SysResult Accountdelete(accountbank model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                result.Code = 1002;
                result.Message = "请登录先";
                return result;
            }
            model.CompanyId = user.CompanyId;
            return service.deleteaccount(model);
        }
    }
}