using ControllerHelper;
using DAL;
using Model;
using Model.House;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class FinanceService
    {
        FinanceDAL dal = new FinanceDAL();
        public SysResult<List<WrapFinanceModel>> Query(FinanceModel model, OrderablePagination orderablePagination, long[] userids, T_SysUser user)
        {
            List<WrapFinanceModel> list = new List<WrapFinanceModel>();
            if (model.CellNames != null)
            {
                model.arrCellNames = model.CellNames.Split(',');
            }
            list = dal.Querylist(model, orderablePagination, userids, user);
            
            SysResult<List<WrapFinanceModel>> result = new SysResult<List<WrapFinanceModel>>();
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }
        //报修详情
        public SysResult<WrapFinanceModel> Queryxq(FinanceModel model)
        {
            WrapFinanceModel list = new WrapFinanceModel();
            SysResult<WrapFinanceModel> result = new SysResult<WrapFinanceModel>();
            list = dal.Queryxq(model);
            result.numberData = list;
            return result;

        }
         //意见反馈
         public SysResult feeback(feedback model)
        {
            SysResult result = new SysResult();
            feebackDAL dal = new feebackDAL();
            if (dal.Save(model) > 0)
            {
                result = result.SuccessResult("提交成功，感谢您的反馈");
            }
            else
            {
                result = result.FailResult("提交失败");
            }
            return result;
        }
        public SysResult save(FinanceModel model)
        {
            SysResult result = new SysResult();
            try
            {
                if (dal.save(model) > 0)
                {
                    result = result.SuccessResult("保存成功");
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("操作异常", ex);
            }
            return result;
        }
    
}
}
