using ControllerHelper;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class RenovationService
    {
        RenovationDAL dal = new RenovationDAL();
        public SysResult<List<WrapRenovation>> Query(Renovation model, OrderablePagination orderablePagination)
        {
            List<WrapRenovation> list = new List<WrapRenovation>();
            list = dal.Querylist(model, orderablePagination);
            SysResult<List<WrapRenovation>> result = new SysResult<List<WrapRenovation>>();
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }
        //查询详情
        public SysResult<WrapRenovation> Querybyid(WrapRenovation model)
        {
            SysResult<WrapRenovation> result = new SysResult<WrapRenovation>();
            WrapRenovation wrap = new WrapRenovation();
            wrap=dal.Queryid(model);
            if (wrap != null)
            {
                wrap.list = dal.Querybylist(model);
            }
            result.numberData = wrap;
            return result;
        }
        //子表详情
        //查询详情

        public SysResult<TRenovationList> Repairxq(WrapRenovation model)
        {
            SysResult<TRenovationList> result = new SysResult<TRenovationList>();
            TRenovationList wrap = new TRenovationList();
            wrap = dal.Repairxq(model);
            result.numberData = wrap;
            return result;
        }
        //保存或者编辑
        public SysResult save(Renovation model)
        {
            SysResult result = new SysResult();
            if (model.Id == 0)
            {
                dal.Save(model);
            }
            else
            {
                dal.Save(model);
                if (model.list != null)
                {
                    dal.Savelist(model.list);
                }
            }
            return result;
        }
        //保存列表
        public SysResult savelist(TRenovationList model)
        {
            SysResult result = new SysResult();
            dal.Savelist(model);
            return result;
        }
        //报修列表
        public SysResult<List<TRenovationList>> Queryrepairlist(TRenovationList model)
        {
            List<TRenovationList> list = new List<TRenovationList>();
            list = dal.Queryrepairelist(model);
            SysResult<List<TRenovationList>> result = new SysResult<List<TRenovationList>>();
            result.numberData = list;
            result.numberCount = list.Count();
            return result;

        }
    }
}
