using DAL;
using DAL.Common;
using DBHelp;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTaskService.DB
{
    public class AutoTaskServiceDB: IAutoTaskService
    {
        private readonly ISysAutoTaskRepository autoTaskRepo;
        private new readonly ILogService logService;
        private readonly ISysAutoTaskTriggerRepository triggerRepo;
        private readonly ISysAutoTaskHistoryRepository historyRepo;
        public AutoTaskServiceDB(ISysAutoTaskRepository _AutoTaskRepo, ILogService _LogService, ISysAutoTaskTriggerRepository _triggerRepo, ISysAutoTaskHistoryRepository _historyRepo)
        {
            this.autoTaskRepo = _AutoTaskRepo;
            this.logService = _LogService;
            this.triggerRepo = _triggerRepo;
            this.historyRepo = _historyRepo;
        }

        /// <summary>
        /// 获取可用的自动任务
        /// </summary>
        /// <returns></returns>
        public IList<SysAutoTaskModel> GetAvailableSysAutoTask(out string msg, string dbType = "oracle")
        {
            msg = "";
            IList<SysAutoTaskModel> list = new List<SysAutoTaskModel>();
            if (dbType == "oracle")
            {
                list = autoTaskRepo.GetAllToList(dbType);
            }
            else
            {
                ApiDAL apiservice = new ApiDAL();
                list = apiservice.getSysAutoTaskList(out msg);
                if (list == null)
                {
                    return null;
                }
            }
            IList<SysAutoTaskModel> returnList = new List<SysAutoTaskModel>();
            foreach (SysAutoTaskModel model in list)
            {
                //ID为57的任务默认开启(系统维护)
                if (DBHelp.ConvertHelper.ObjToBool(model.IsActive) || model.Id == 57)
                    returnList.Add(model);
            }

            return returnList;

        }

        //获取触发器
        public SysAutoTaskTriggerModel GetAutoTaskTriggerById(out string msg, int id, string dbType = "oracle")
        {
            msg = "";
            SysAutoTaskTriggerModel model = new SysAutoTaskTriggerModel();
            if (dbType == "oracle")
            {
                model = triggerRepo.GetById(id, dbType);
            }
            else
            {
                ApiDAL apiservice = new ApiDAL();
                model = apiservice.getAutoTaskTriggerById(id, out msg);
                if (model == null)
                {
                    return null;
                }
            }
            return model;
        }


        //public SysAutoTaskModel GetAutoTaskById(int id, string dbType = "sqlserver")
        //{
        //    SysAutoTaskModel model = autoTaskRepo.GetById(id,dbType);
        //    return model;
        //}


        /// <summary>
        /// 更新自动任务状态,将任务状态设置为未执行,用于启动自动任务时,废弃
        /// </summary>
        /// <param name="group">空表示所有</param>
        /// <returns></returns>
        //public int UpdateAutoTaskJobStatus(int status,int id,string group)
        //{
        //    return autoTaskRepo.UpdateAutoTaskJobStatus(status,id, group);
        //}

        /// <summary>
        /// 通过Id更新单个任务的运行情况
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateAutoTaskRunInfo(SysAutoTaskModel entity, out string msg, string dbType = "oracle")
        {
            msg = "";
            if (dbType == "oracle")
            {
                return autoTaskRepo.UpdateAutoTaskRunInfo(entity, dbType);
            }
            else
            {
                ApiDAL apiservice = new ApiDAL();
                return apiservice.updateAutoTaskRunInfo(entity, out msg);
            }
        }

        //api上层控制
        /// <summary>
        /// 插入任务执行记录
        /// </summary>
        /// <param name="enity"></param>
        /// <returns></returns>
        public int Insert(int SysAutoTaskId, byte execStatus, string execMessage, int totalSeconds, string jobPara1, string jobPara2, string OwnerId, string InstanceId, string dbType = "oracle")
        {
            SysAutoTaskHistoryModel entity = new SysAutoTaskHistoryModel();
            entity.SysAutoTaskId = SysAutoTaskId;
            entity.ExecStatus = execStatus;
            entity.ExecMessage = execMessage;
            entity.TotalSeconds = totalSeconds;
            entity.JobPara1 = jobPara1;
            entity.JobPara2 = jobPara2;
            entity.OwnerId = OwnerId;
            entity.InstanceId = InstanceId;
            entity.IsActive = true;
            entity.CreationDate = DateTime.Now;
            // logService.logInfo(Newtonsoft.Json.JsonConvert.SerializeObject(entity));
            return historyRepo.Insert(entity, null, null, dbType);

        }

        public void test()
        {
            logService.logInfo("测试成功");
        }
    }
}
