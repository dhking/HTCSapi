using AutoTaskService.DB;
using AutoTaskService.JobImpl;
using DBHelp;
using log4net;
using Model;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace AutoTaskService
{
    public class QuartzServer : ServiceControl, IQuartzServer
    {
        private readonly ILog logger;
        private ISchedulerFactory schedulerFactory;
        private IScheduler scheduler;
        private IAutoTaskService autoTaskService;
        private string jobGroup = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="QuartzServer"/> class.
        /// </summary>
        public QuartzServer()
        {
            logger = LogManager.GetLogger(GetType());
            autoTaskService = IoC.Resolve<IAutoTaskService>();
        }


        /// <summary>
        /// Initializes the instance of the <see cref="QuartzServer"/> class.
        /// </summary>
        public virtual void Initialize()
        {
            try
            {
                logger.Info("开始加在任务成配置......");
                //获取自动任务组
                jobGroup = System.Configuration.ConfigurationManager.AppSettings["JobGroup"].ToStr().Trim();
                logger.Info(Program.DbType);

                //如果任务组包含 WCF服务组,则自动启动服务,服务需要开通556端口

                if (jobGroup.Trim().IndexOf("WCF组") >= 0)
                {
                    logger.Info("正在启动WCF服务..........");

                    WCFHost host = new WCFHost();
                    host.Start();


                    logger.Info("启动WCF服务结束..........");
                }

                if (jobGroup.Trim().IndexOf("库存组") >= 0)
                {
                    ThreadPool.SetMinThreads(500, 500);
                }

                logger.Info("正在启动升级程序启动消息监听服务......");
                Burgeon.Wing3.Release.Sandbox.SandboxRunListener.Instance.Run();
                logger.Info(string.Format("启动升级程序启动消息监听服务 成功,监听路径：{0}", Burgeon.Wing3.Release.Sandbox.SandboxRunListener.Instance.file));

                //else
                //{





                schedulerFactory = CreateSchedulerFactory();
                scheduler = GetScheduler();
                //将特定的任务组的任务状态改成未执行
                //autoTaskService.UpdateAutoTaskJobStatus(1, 0, jobGroup);//TODO:是否有必要做这步

                logger.Info(autoTaskService == null ? "autoTaskService为空" : "autoTaskService不为空");
                string errmsg = "";
                IList<SysAutoTaskModel> list = autoTaskService.GetAvailableSysAutoTask(out errmsg, Program.DbType);
                if (list == null)
                {
                    logger.Info(errmsg+"任务程序配置为空");
                    throw new WingBussinessException(errmsg);//失败
                }

                foreach (SysAutoTaskModel model in list)
                {
                   
                    if (!string.IsNullOrWhiteSpace(model.JobGroup))
                    {

                        //如果当前实体的任务组不等于此服务需要执行的任务组,则我们就不加载在任务队列里面了
                        if (jobGroup.Trim().IndexOf(model.JobGroup.Trim()) < 0)
                        {
                            logger.Info(string.Format("任务{0}的任务组属于{1},不属于自动启动的任务组,所以不启动此任务", model.JobName, model.JobGroup));
                            continue;
                        }
                    }


                    if (Program.NotStartTaskId.IndexOf(model.Id + ",") > 0)
                        continue;



                    logger.Info(string.Format("开始装载任务!任务ID:{1},任务名称:{0},任务组:{2}", model.JobName, model.Id.ToStr(), model.JobGroup));
                    JobDataMap jobDataMap = new JobDataMap();

                    if (!string.IsNullOrEmpty(model.JobPara1))
                        jobDataMap.Put("Para1", model.JobPara1);
                    if (!string.IsNullOrWhiteSpace(model.JobPara2))
                        jobDataMap.Put("Para2", model.JobPara2);

                    jobDataMap.Put("TaskId", model.Id);
                    //jobDataMap.Put("IsCanMultiThread", model.IsCanMultiThread);
                    //jobDataMap.Put("JobStatus", 1);

                    if (string.IsNullOrWhiteSpace(model.JobSpName) && string.IsNullOrWhiteSpace(model.JobClassName))
                        throw new WingBussinessException(string.Format("自动任务{0} 类名和存储过程名不能全为空!", model.JobName));
                    Type t = null;
                    if (!string.IsNullOrWhiteSpace(model.JobClassName))
                    {
                        t = Type.GetType(model.JobClassName);
                    }
                    else
                    {


                        if (Program.DbType == "sqlite")
                            throw new WingBussinessException(string.Format("自动任务{0} 为存储过程自动任务,Sqlite数据库不支持存储过程哦!!", model.JobName));
                        jobDataMap.Put("SpName", model.JobSpName);
                        if (string.IsNullOrEmpty(model.JobPara1))
                        {
                            throw new WingBussinessException(string.Format("自动任务{0} 为存储过程自动任务,参数1不能为空,参数1为数据库连接符的名称!", model.JobName));
                        }
                        t = typeof(StoredProcedureJob);
                    }

                    IJobDetail job = JobBuilder.Create(t)
                        .WithIdentity(model.JobName + model.Id, model.JobGroup)
                        .WithDescription(model.JobDesc)
                        .UsingJobData(jobDataMap)
                        .RequestRecovery()
                        .Build();



                    //获取Trigger
                    ITrigger trigger = GetTrigger(autoTaskService, model);

                    if (!scheduler.CheckExists(job.Key))
                    {
                        scheduler.ScheduleJob(job, trigger);
                    }

                    //开启淘宝消息服务,约定消息服务的编号为36
                    //另外启动凌晨自动同步店铺信息任务到短信平台,此举为了子账号登陆
                    if (model.Id == 36)
                    {
                        //StartIMCService(model);

                    }

                    logger.Info(string.Format("开始装载任务!任务ID:{1},任务名称:{0},任务组:{2}", model.JobName, model.Id.ToStr(), model.JobGroup));


                }
                //}

            }
            catch (Exception e)
            {
                logger.Error("Server initialization failed:" + e.Message, e);
                throw;
            }






        }


        /// <summary>
        /// 初始化触发器信息
        /// </summary>
        /// <param name="autoTaskService"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ITrigger GetTrigger(IAutoTaskService autoTaskService, SysAutoTaskModel model)
        {
            string errmsg2 = "";
            SysAutoTaskTriggerModel triggerModel = autoTaskService.GetAutoTaskTriggerById(out errmsg2, ConvertHelper.ObjToInt(model.SysAutoTaskTriggerId), Program.DbType);
            if (triggerModel == null && !string.IsNullOrEmpty(errmsg2))
            {
                throw new WingBussinessException("获取触发器ID为" + model.SysAutoTaskTriggerId + "的数据失败,失败信息是:" + errmsg2);
            }

            if (triggerModel == null || triggerModel.IsActive == null || triggerModel.IsActive.Value == false)
                throw new WingBussinessException(string.Format("自动任务{0} 不能启动,触发器{1}为空或不可用", model.JobName, model.SysAutoTaskTriggerId.ToString()));

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(model.JobName + model.Id, model.JobGroup)
                .WithDescription(triggerModel.ShowName)
                .WithCronSchedule(triggerModel.CronExpression)
                .Build();
            return trigger;
        }

        /// <summary>
        /// Gets the scheduler with which this server should operate with.
        /// </summary>
        /// <returns></returns>
        protected virtual IScheduler GetScheduler()
        {
            return schedulerFactory.GetScheduler();
        }

        /// <summary>
        /// Returns the current scheduler instance (usually created in <see cref="Initialize" />
        /// using the <see cref="GetScheduler" /> method).
        /// </summary>
        protected virtual IScheduler Scheduler
        {
            get { return scheduler; }
        }

        /// <summary>
        /// Creates the scheduler factory that will be the factory
        /// for all schedulers on this instance.
        /// </summary>
        /// <returns></returns>
        protected virtual ISchedulerFactory CreateSchedulerFactory()
        {
            return new StdSchedulerFactory();
        }

        /// <summary>
        /// Starts this instance, delegates to scheduler.
        /// </summary>
        public virtual void Start()
        {
            scheduler.Start();

            try
            {
                Thread.Sleep(500);
            }
            catch (ThreadInterruptedException)
            {
            }

            logger.Info("Scheduler started successfully");
        }

        /// <summary>
        /// Stops this instance, delegates to scheduler.
        /// </summary>
        public virtual void Stop()
        {
            scheduler.Shutdown(false);
            logger.Info("Scheduler shutdown complete");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            // no-op for now
        }

        /// <summary>
        /// Pauses all activity in scheudler.
        /// </summary>
        public virtual void Pause()
        {
            scheduler.PauseAll();
        }

        /// <summary>
        /// Resumes all acitivity in server.
        /// </summary>
        public void Resume()
        {
            scheduler.ResumeAll();
        }

        /// <summary>
        /// TopShelf's method delegated to <see cref="Start()"/>.
        /// </summary>
        public bool Start(HostControl hostControl)
        {
            Start();
            return true;
        }

        /// <summary>
        /// TopShelf's method delegated to <see cref="Stop()"/>.
        /// </summary>
        public bool Stop(HostControl hostControl)
        {
            Stop();
            return true;
        }

        /// <summary>
        /// TopShelf's method delegated to <see cref="Pause()"/>.
        /// </summary>
        public bool Pause(HostControl hostControl)
        {
            Pause();
            return true;
        }

        /// <summary>
        /// TopShelf's method delegated to <see cref="Resume()"/>.
        /// </summary>
        public bool Continue(HostControl hostControl)
        {
            Resume();
            return true;
        }
    }
}
