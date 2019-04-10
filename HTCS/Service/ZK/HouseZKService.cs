using ControllerHelper;
using DAL;
using DAL.ZK;
using Model;
using Model.Base;
using Model.House;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ZK
{
    public class HoseZKService
    {
        HouseZKDAL dal = new HouseZKDAL();
        //查询热门房间
        public SysResult<IList<HouseZK>> Queryrmhouse(HouseZK model, OrderablePagination orderablePagination)
        {
            SysResult<IList<HouseZK>> re = dal.Querylistrm(model, orderablePagination);
            return re;
        }
        //查询特色列表
        public SysResult<List<T_Basics>> Querytese()
        {
            DAL.ZK.BaseDataDAL dal1 = new DAL.ZK.BaseDataDAL();
            SysResult<List<T_Basics>> re =new SysResult<List<T_Basics>> ();
            re.numberData = dal1.Querylist1();
            return re;
        }
        //查询房源详情
        public SysResult<HouseZK> xq(HouseZK model)
        {
            SysResult<HouseZK> result = new SysResult<HouseZK>();
            result.numberData = dal.xq(model.Id);
            return result;
        }
        //保存预约信息
        public SysResult Save(Appointment model)
        {
            SysResult result = new SysResult();
            if (dal.Save() > 0)
            {
                result = result.SuccessResult("预约成功");
            }
            return result;
        }
    }
}
