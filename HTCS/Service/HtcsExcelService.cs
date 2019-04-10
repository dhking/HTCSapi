using ControllerHelper;
using DAL;
using Model;
using Model.Contrct;
using Model.House;
using Model.User;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class HtcsExcelService
    {
        HtcsExcelDAL dal = new HtcsExcelDAL();
        HoseService hservice = new HoseService();
        //整租导出
        public byte[] excel(HouseModel model)
        {
           List<HouseModel> housemodel = hservice.zzHouseQuery1(model).numberData;

           return  dal.excel(housemodel);
        }
        //合租导出
        public byte[] hhouseexcel(HouseModel model)
        {
            List<HousePendent> housemodel = hservice.excelQueryhouse(model,null).numberData;
            return dal.hexcel(housemodel);
        }
        //独栋导出
        public byte[] dhouseexcel(HouseModel model)
        {
            IndentHouseService dentservice = new Service.IndentHouseService();
            List<HousePendent> housemodel = dentservice.excelQueryPChouse(model);
            return dal.hexcel(housemodel);
        }
        //合同导出 
        public byte[] contractexcel(WrapContract model,T_SysUser user)
        {
            ContrctService cservice = new ContrctService();
            List<WrapContract> housemodel = cservice.excelQuerymenufy(model, user).numberData;
            return dal.contractexcel(housemodel);
        }
        //业主合同导出
        public byte[] ycontractexcel(WrapOwernContract model,T_SysUser user)
        {
            OwerContractService cservice = new Service.OwerContractService();
            List<WrapOwernContract> housemodel = cservice.excelQuerymenufy(model, user).numberData;
            return dal.ycontractexcel(housemodel);
        }
        //下载

        protected OrderablePagination InitPage(int limit, int start)
        {
            OrderablePagination _orderablePagination = new OrderablePagination();
            if (_orderablePagination != null)
            {
                _orderablePagination.PageSize = limit;
                _orderablePagination.StartIndex = start - limit;
                _orderablePagination.TotalCount = 0;
            }
            return _orderablePagination;
        }
    }
}
