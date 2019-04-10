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
    public  class AutoTaskService
    {
        AutoTaskkDAL dal = new AutoTaskkDAL();
        public SysResult<List<SysAutoTaskModel>> Query(SysAutoTaskModel model, OrderablePagination orderablePagination)
        {
            List<SysAutoTaskModel> list = new List<SysAutoTaskModel>();
            list = dal.Querylist(model, orderablePagination);
            SysResult<List<SysAutoTaskModel>> result = new SysResult<List<SysAutoTaskModel>>();
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }
        public SysResult<List<SysAutoTaskServiceModel>> Query1(SysAutoTaskServiceModel model, OrderablePagination orderablePagination)
        {
            List<SysAutoTaskServiceModel> list = new List<SysAutoTaskServiceModel>();
            list = dal.Querylist1(model, orderablePagination);
            SysResult<List<SysAutoTaskServiceModel>> result = new SysResult<List<SysAutoTaskServiceModel>>();
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }
        public SysResult<List<SysAutoTaskTriggerModel>> Query2(SysAutoTaskTriggerModel model, OrderablePagination orderablePagination)
        {
            List<SysAutoTaskTriggerModel> list = new List<SysAutoTaskTriggerModel>();
            list = dal.Querylist2(model, orderablePagination);
            SysResult<List<SysAutoTaskTriggerModel>> result = new SysResult<List<SysAutoTaskTriggerModel>>();
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }
        public SysResult<List<SysAutoTaskHistoryModel>> Query3(SysAutoTaskHistoryModel model, OrderablePagination orderablePagination)
        {
            List<SysAutoTaskHistoryModel> list = new List<SysAutoTaskHistoryModel>();
            list = dal.Querylist3(model, orderablePagination);
            SysResult<List<SysAutoTaskHistoryModel>> result = new SysResult<List<SysAutoTaskHistoryModel>>();
            result.numberData = list;
            result.numberCount = orderablePagination.TotalCount;
            return result;

        }
        public SysResult<object> GetShoulder(Server model)
        {
            SysResult<object> sysresult = new SysResult<object>();
            try
            {
                Scheduler s = new Scheduler(model.server, model.port, model.scheduler);
                IList<JobSchedule> js = s.GetSchedules2();
                sysresult.numberData = js;
                sysresult.Code = 0;
                return sysresult;
            }
            catch (Exception ex)
            {
                var temp = new { total = 0, rows = new List<JobSchedule>(), msg = ex.Message };
                sysresult.Code = 1;
                sysresult.Message = ex.Message;
                sysresult.numberData = temp;
                return sysresult;
            }

        }
        public SysResult ChangeScheduler(Server model)
        {
            SysResult result = new SysResult();
            result = Validate(model.server, model.port, model.scheduler, model.Name,model.op);
            if (result.Code != 0)
            {
                return result;
            }
            try
            {
                Scheduler s = new Scheduler(model.server, model.port, model.scheduler);
                if (model.op == "stop")
                {
                    s.PauseJob(model.Group, model.Name);
                }
                else if (model.op == "start")
                {
                    s.StartScheduler(model.Group, model.Name);
                }
                else if (model.op == "stopall")
                {
                    s.PauseAllJob();
                }
                else if (model.op == "run")
                {
                    s.RunAway(model.Group, model.Name);
                }
                else if (model.op == "close")
                {
                    s.Close();
                }

                result.Code = 0;
                result.Message = "操作成功";
            }
            catch (Exception ex)
            {
                result.Code = 999;
                result.Message = "操作失败"+ ex.ToString();
         
            }

            return  result;

        }
        private SysResult Validate(string server, int port, string scheduler, string jobName, string op)
        {
            SysResult result = new SysResult();
            result.Code = 0;

            if (string.IsNullOrEmpty(op) || string.IsNullOrWhiteSpace(server))
            {
                result.Code = 99;
                result.Message = "操作失败";
              
                return result;
            }

            if (string.IsNullOrWhiteSpace(jobName) && op != "stopall" && op != "close")
            {
                result.Code = 99;
                result.Message = "操作失败";
                
                return result;
            }

            return result;
        }
    }
}
