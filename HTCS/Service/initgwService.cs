using ControllerHelper;
using DAL;
using DAL.Common;
using Model;
using Model.Base;
using Model.Contrct;
using Model.House;
using Model.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public   class initgwService
    {
        queryupperDAl dal = new queryupperDAl();
        HouseDAL housedal = new HouseDAL();
        HousePentDAL pentdal = new HousePentDAL();
        //查询未上架数据
        public SysResult query(int gwisupper)
        {
            SysResult result = new SysResult();
            try
            {
                List<houresourcesupper> list = dal.Query(new houresourcesupper() { gwisupper = gwisupper });
            }
            catch(Exception ex)
            {
                result = result.FailResult(ex.InnerException.ToStr());
            }
            
            return result;
        }
        
        public SysResult inerthouse(HouseZK zk)
        {
            SysResult result = new SysResult();
            //执行插入操作
            HtcsZKClient htcs = new HtcsZKClient("api/House/Save");
            result= htcs.DoExecute2<HouseZK>(zk);
            return  result;
        }
       
    }
}
