using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class ApiDAL
    {
        public SysAutoTaskTriggerModel getAutoTaskTriggerById(int id, out string msg)
        {
            msg = "";
            return null;
        }
        public int updateAutoTaskRunInfo(SysAutoTaskModel entity, out string msg)
        {
            msg = "";
            return 0;
        }
        public IList<SysAutoTaskModel> getSysAutoTaskList(out string msg)
        {
            msg = "";
            return null;
        }
    }
}
