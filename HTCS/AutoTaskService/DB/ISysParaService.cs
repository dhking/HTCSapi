using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTaskService.DB
{
    public interface ISysParaService
    {
        IList<SysParaModel> GetAllToList(out string msg, string dbType = "sqlserver");
    }
}
