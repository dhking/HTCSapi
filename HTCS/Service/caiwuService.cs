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
        public SysResult<List<HouseReport>> Querybaobiao(HouseReport model,T_SysUser user, OrderablePagination orderablePagination)
        {
            caiwuDAL dal = new caiwuDAL();
            SysResult<List<HouseReport>> sysresult = new SysResult<List<HouseReport>>();
            HouseReport paramodel=getparam(model, user);
            List<HouseReport> list = dal.Querybaobiao(paramodel, orderablePagination,user.type);
            sysresult.numberData = list;
            sysresult.numberCount = orderablePagination.TotalCount;
            return sysresult;
        }
        public HouseReport getparam(HouseReport model,T_SysUser user)
        {
            if (user.type == 1)
            {
                if (user.cellname != null)
                {
                    string[] cellarr = new string[] { };
                    cellarr = user.cellname.Split(",");
                    if (model.cellnames != null)
                    {
                        model.cellnames = model.cellnames.Concat(cellarr).ToArray();
                    }
                    else
                    {
                        model.cellnames = cellarr;
                    }
                }
                else
                {
                    model.cellnames = new string[] { };
                }
                if (user.city != null)
                {
                    string[] cityarr = new string[] { };
                    cityarr = user.city.Split(",");
                    if (model.cellnames != null)
                    {
                        model.citynames = model.cellnames.Concat(cityarr).ToArray();
                    }
                    else
                    {
                        model.citynames = cityarr;
                    }
                }
                else
                {
                    model.citynames = new string[] { };
                }
            }
            return model;
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
