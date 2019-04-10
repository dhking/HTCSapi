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
    public  class ZafeiServcie
    {
        ZafeiDAL dal = new ZafeiDAL();
        public SysResult<List<T_ZafeiList>> Querymenufy(T_ZafeiList model, OrderablePagination orderablePagination)
        {
            SysResult<List<T_ZafeiList>> sysresult = new SysResult<List<T_ZafeiList>>();
            List<T_ZafeiList> list = dal.Queryfy(model,orderablePagination);
            sysresult.numberData = list;
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
        public SysResult<List<T_ZafeiList>> Querymenu()
        {
            SysResult<List<T_ZafeiList>> sysresult = new SysResult<List<T_ZafeiList>>();
            List<T_ZafeiList> list = dal.Query(new T_ZafeiList() { IsActive = 1 });
            sysresult.numberData = list;
            sysresult.numberCount = list.Count;
            return sysresult;
        }
        public SysResult Savezafei(T_ZafeiList model)
        {
            SysResult sysresult = new SysResult();
            if (dal.SaveZafei(model) > 0)
            {
                sysresult = sysresult.SuccessResult("保存成功");
            };
            return sysresult;
        }
        public SysResult<T_ZafeiList> Queryid(T_ZafeiList model)
        {
            SysResult<T_ZafeiList> result = new SysResult<T_ZafeiList>();

            result.numberData = dal.QueryId(model);
            return result;
        }
        public SysResult deleteData(long ids)
        {

            SysResult result = new SysResult();
            if (dal.deletedata(ids) > 0)
            {
                return result = result.SuccessResult("删除成功");
            }
            else
            {
                return result = result.FailResult("删除失败");
            }
        }
    }
}
