using AutoTaskService.DB;
using DAL;
using DBHelp;
using log4net;
using Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoTaskService
{
    public class BaseJobImpl : IJob
    {
        public static readonly ILog logger = LogManager.GetLogger(typeof(BaseJobImpl));

        IAutoTaskService autoTaskService;
        CommonDBService comm = new CommonDBService();

        public int taskId = 0;//任务编号
        public bool IsCanMultiThread = false;//是否可以多线程
        public string jobPara1 = "";//参数1
        public string jobPara2 = "";//参数2
        public string instanceId = "";//

        public byte execStatus = 0;//执行状态
        public string execMessage = "";//执行消息
        private DateTime StartTime = DateTime.Now;//开始时间
        private int execSeconds = 0;

        public BaseJobImpl()
        {
            if (autoTaskService == null)
                autoTaskService = IoC.Resolve<IAutoTaskService>();
        }
        public void logException(Exception e, string message = "")
        {

            StringBuilder preamble = new StringBuilder();
            preamble.Append(message);
            preamble.Append("\r\n 异常:");
            preamble.Append(e.Message);

            preamble.Append("\r\n");

            StackTrace st = new StackTrace(true);
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                MethodBase mb = sf.GetMethod();
                if (mb != null && mb.ReflectedType != null)
                {
                    string mname = mb.ReflectedType.FullName;
                    preamble.Append("at method:");
                    preamble.Append(mname);
                    preamble.Append("::");
                    preamble.Append(sf.GetMethod().Name);
                    preamble.Append(" :");
                    preamble.Append(sf.GetFileLineNumber());
                    preamble.Append(" +file:");
                    preamble.Append(sf.GetFileName());
                    preamble.Append("\r\n");
                }
            }

            preamble.Append("\r\n");

            logger.Fatal(preamble, e);
        }

        /// <summary>
        /// Called by the <see cref="IScheduler" /> when a <see cref="ITrigger" />
        /// fires that is associated with the <see cref="IJob" />.
        /// </summary>
        /// <remarks>
        /// The implementation may wish to set a  result object on the 
        /// JobExecutionContext before this method exits.  The result itself
        /// is meaningless to Quartz, but may be informative to 
        /// <see cref="IJobListener" />s or 
        /// <see cref="ITriggerListener" />s that are watching the job's 
        /// execution.
        /// </remarks>
        /// <param name="context">The execution context.</param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                BeforeExecute(context);

                ExecuteJob(context);

                AfterExecute(context);
            }
            catch (Exception ce)
            {
                logException(ce, string.Format("执行任务{0}出现异常", context.JobDetail.Description));
            }
        }

        public virtual void BeforeExecute(IJobExecutionContext context)
        {
            logger.Info(context.JobDetail.Key + "running...");

            //context.JobDetail.JobDataMap["JobStatus"] = 1;
            taskId = context.JobDetail.JobDataMap["TaskId"].ToInt();
            //IsCanMultiThread = context.JobDetail.JobDataMap["IsCanMultiThread"].ToBool();
            jobPara1 = context.JobDetail.JobDataMap["Para1"].ToStr();
            jobPara2 = context.JobDetail.JobDataMap["Para2"].ToStr();
            if (taskId == 36)
            {
                if (!Program.IsIMCStart)
                {
                    //logger.Info("系统启动消息服务--开始");
                    //QuartzServer.StartIMCService(new SysAutoTaskModel { JobName = "淘宝消息服务" });
                    //logger.Info("系统启动消息服务--结束");
                }
            }

            //logger.Warn("JobPara1:" + jobPara1);
            //logger.Warn("JobPara2:" + jobPara2);

            ////如果任务不能多线程执行,则我们需要判断当前任务状态
            //if (!IsCanMultiThread)
            //{ 
            //    autoTaskModel = autoTaskService.GetAutoTaskById(taskId);

            //    if (autoTaskModel.JobStatus ==2)
            //    {
            //        autoTaskService.Insert(taskId, 3, "当前任务正在执行,停止本次执行", 0, jobPara1, jobPara2, "", context.FireInstanceId);
            //        return;
            //    }

            //   //TriggerState state= context.Scheduler.GetTriggerState(context.Trigger.Key);
            //}

            //立即将任务状态设置为执行中
            //autoTaskService.UpdateAutoTaskJobStatus(2, taskId, "");



        }

        public virtual void ExecuteJob(IJobExecutionContext context)
        {


        }

        public virtual void AfterExecute(IJobExecutionContext context)
        {
            //插入任务程序执行历史
            TimeSpan ts = DateTime.Now - StartTime;
            execSeconds = ts.Seconds;
            if (Program.DbType != "oracle")
            {
                string errmsg = "";
                bool suc = comm.InsertAutotaskHistory(taskId, execStatus, execMessage, execSeconds, jobPara1, jobPara2, context.Scheduler.SchedulerName, context.FireInstanceId, out errmsg);
                if (!suc)
                {
                    logger.Error("插入任务执行历史失败," + errmsg);
                }
            }
            else//如果不是oracle(sqlite)则不插入sqllite数据库
            {
                autoTaskService.Insert(taskId, execStatus, execMessage, execSeconds, jobPara1, jobPara2, context.Scheduler.SchedulerName, context.FireInstanceId, Program.DbType);
            }

            //更新任务程序状态
            string errmsg2 = "";
            int rows = autoTaskService.UpdateAutoTaskRunInfo(GenerateAutoTaskModel(), out errmsg2, Program.DbType);
            if (rows <= 0)
            {
                logger.Info(errmsg2);
            }
            //设置状态
            //context.JobDetail.JobDataMap["JobStatus"] = 2;
            logger.Info(context.JobDetail.Key + " run finished.");
        }

        /// <summary>
        /// 构建自动任务对象
        /// </summary>
        /// <returns></returns>
        private SysAutoTaskModel GenerateAutoTaskModel()
        {
            SysAutoTaskModel model = new SysAutoTaskModel();
            model.TotalCount = 1;
            model.TotalSeconds = execSeconds;
            model.JobStatus = 3;
            model.LastExecStatus = execStatus;
            model.LastExecMessage = "";
            model.Id = taskId;
            return model;
        }

      
    }
}
