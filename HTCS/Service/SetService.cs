using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class SetService
    {
        SetDAL dal = new SetDAL();
        public SysResult<indepent> Query(indepent model)
        {
            SysResult<indepent> result = new SysResult<indepent>();
            indepent pent = dal.Query(model);
            if(string.IsNullOrEmpty(model.name))
            {
                result.Code = 1;
                result.Message = "请填写公司名称";
                return result;
            }
            if (pent == null)
            {
                result.Code = 1;
                result.Message = "公司不存在";
                return result;
            }
            result.numberData = pent;
            return result;
        }
    }
}
