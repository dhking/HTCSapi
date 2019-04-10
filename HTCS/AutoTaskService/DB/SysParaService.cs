using DAL;
using DBHelp;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTaskService.DB
{
    public class SysParaService : ISysParaService
    {
        //
        private readonly ISysParaRepository sysParaRepo;
        private new readonly ILogService logService;

        public SysParaService(ISysParaRepository _sysParaRepo, ILogService _LogService)
        {
            this.sysParaRepo = _sysParaRepo;
            this.logService = _LogService;

        }

        public IList<SysParaModel> GetAllToList(out string msg, string dbType = "oracle")
        {
            msg = "";
            logService.logInfo(dbType);
         
                logService.logInfo("22222");
                return sysParaRepo.GetAllToList(dbType);
        }





    }
}
