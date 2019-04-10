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
    public   class OrderService
    {
        OrderAL dal = new OrderAL();
        public SysResult<List<wrapOrder>> Query(wrapOrder model, OrderablePagination orderablePagination)
        {
            List<wrapOrder> list = new List<wrapOrder>();
            list = dal.Querylist(model, orderablePagination);
            //foreach(var mo in list)
            //{
            //    mo.ServiceCharge = 0.006;
            //    mo.realamount = mo.Amount * mo.ServiceCharge;
            //}
            SysResult<List<wrapOrder>> result = new SysResult<List<wrapOrder>>();
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }
    }
}
