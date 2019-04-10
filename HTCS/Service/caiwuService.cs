using ControllerHelper;
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
  public   class caiwuService
    {
        public SysResult<List<T_Record>> Querymenufy(T_Record model, OrderablePagination orderablePagination)
        {
            caiwuDAL dal = new caiwuDAL();
            SysResult<List<T_Record>> sysresult = new SysResult<List<T_Record>>();
            List<T_Record> list = dal.Query(model, orderablePagination);
            sysresult.numberData = list;
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
        public SysResult<List<HouseReport>> Querybaobiao(HouseReport model, OrderablePagination orderablePagination)
        {
            caiwuDAL dal = new caiwuDAL();
            SysResult<List<HouseReport>> sysresult = new SysResult<List<HouseReport>>();
            List<HouseReport> list = dal.Querybaobiao(model, orderablePagination);
            sysresult.numberData = list;
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
        public SysResult<List<WrapHouseReportList>> baobiaochildQuery(HouseReportList model, OrderablePagination orderablePagination)
        {
            caiwuDAL dal = new caiwuDAL();
            SysResult<List<WrapHouseReportList>> sysresult = new SysResult<List<WrapHouseReportList>>();
            List<WrapHouseReportList> list = dal.baobiaochildQuery(model, orderablePagination);
            sysresult.numberData = list;
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
    }
}
