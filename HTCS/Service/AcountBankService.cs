using DAL;
using Model;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class AcountBankService
    {
        AccountNameDAL dal = new AccountNameDAL();
        public SysResult<List<accountbank>> Accountlist(accountbank model)
        {
            SysResult<List<accountbank>> result = new SysResult<List<accountbank>>();
            List<accountbank> list= dal.Accountlist(model);
            result.numberData = list;
            return result;
        }
        public SysResult Accountsave(accountbank model)
        {
            SysUserService susservice = new SysUserService();
            SysResult result = new SysResult();
            SysUserService service = new SysUserService();
            if (service.Viltyzm(model.yzm, model.Mobile, 6))
            {
                dal.saveaccount(model);
            }
            else
            {
                result.Code = 1;
                result.Message = "验证码错误";
            }
            return result;
        }
        public SysResult deleteaccount(accountbank model)
        {
            SysUserService susservice = new SysUserService();
            SysResult result = new SysResult();
            dal.DeleteHouse(model);
            result.Message = "解绑成功";
            return result;
        }
    }
}
