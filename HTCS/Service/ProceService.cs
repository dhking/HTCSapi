using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public   class ProceService
    {
        ProceDAL dal = new ProceDAL();
        public SysResult CmdProce(Pure model)
        {
            return dal.CmdProce(model);
        }
        public SysResult CmdProce1(Pure model)
        {
            return dal.Cmdproce8(model);
        }
        public SysResult CmdProce2(Pure model)
        {
            return dal.Cmdproce9(model);
        }
        public SysResult CmdProce3(Pure model)
        {
            
            return dal.Cmdproce10(model);
        }
        public SysResult CmdProce4(Pure model)
        {
            model.Spname = "sp_billsign";
            return dal.Cmdproce10(model);
        }
    }
}
