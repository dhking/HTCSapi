using Model;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    
    public class Scheduler
    {
        public readonly IScheduler Instance;
        public string Address { get; private set; }
        public string JobName { get; set; }
        public string JobGroup { get; set; }
        public int Priority { get; set; }
        public string CronExpression { get; set; }

        private readonly ISchedulerFactory _schedulerFactory;


        public Scheduler(string server, int port, string scheduler)
        {
            try
            {
                Address = string.Format("tcp://{0}:{1}/{2}", server, port, "QuartzScheduler");
                _schedulerFactory = new StdSchedulerFactory(GetProperties(Address, scheduler));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            try
            {
                Instance = _schedulerFactory.GetScheduler(scheduler);
                if (Instance == null)
                {
                    Instance = _schedulerFactory.GetScheduler();
                }
                if (!Instance.IsStarted)
                    Instance.Start();
            }
            catch (Exception cx)
            {
                throw new Exception(cx.Message + Newtonsoft.Json.JsonConvert.SerializeObject(cx) + "异常:获取服务出现异常，请检查:服务是否已经停止");
            }
        }

        private static NameValueCollection GetProperties(string address, string instanceName)
        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = instanceName;
            properties["quartz.scheduler.proxy"] = "true";
            //properties["quartz.threadPool.threadCount"] = "0";
            properties["quartz.scheduler.proxy.address"] = address;
            return properties;
        }

        public IScheduler GetScheduler()
        {
            return Instance;
        }

        public List<GroupStatus> GetGroups()
        {
            var results = new List<GroupStatus>();
            foreach (var gp in Instance.GetJobGroupNames())
            {
                results.Add(new GroupStatus()
                {
                    Group = gp,
                    IsJobGroupPaused = Instance.IsJobGroupPaused(gp),
                    IsTriggerGroupPaused = Instance.IsTriggerGroupPaused(gp)
                });
            }
            return results;
        }

        public JobSchedule GetSchedule()
        {
            var jobKey = new JobKey(JobName, JobGroup);

            var trigger = Instance.GetTriggersOfJob(jobKey).FirstOrDefault();

            var js = new JobSchedule();

            if (trigger != null)
            {
                js.Name = trigger.Key.Name;
                js.Group = trigger.Key.Group;
                js.Description = trigger.Description;
                js.Priority = trigger.Priority;
                js.TriggerType = trigger.GetType().Name;
                js.TriggerState = Instance.GetTriggerState(trigger.Key).ToString();

                DateTimeOffset? startTime = trigger.StartTimeUtc;
                js.StartTime = TimeZone.CurrentTimeZone.ToLocalTime(startTime.Value.DateTime);

                var nextFireTime = trigger.GetNextFireTimeUtc();
                if (nextFireTime.HasValue)
                {
                    js.NextFire = TimeZone.CurrentTimeZone.ToLocalTime(nextFireTime.Value.DateTime);
                }

                var previousFireTime = trigger.GetPreviousFireTimeUtc();
                if (previousFireTime.HasValue)
                {
                    js.LastFire = TimeZone.CurrentTimeZone.ToLocalTime(previousFireTime.Value.DateTime);
                }
            }

            return js;
        }

        public List<JobSchedule> GetSchedules()
        {
            var jcs = new List<JobSchedule>();

            foreach (var group in Instance.GetJobGroupNames())
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = Instance.GetJobKeys(groupMatcher);

                foreach (var jobKey in jobKeys)
                {
                    var triggers = Instance.GetTriggersOfJob(jobKey);

                    foreach (var trigger in triggers)
                    {
                        var js = new JobSchedule();
                        js.Name = jobKey.Name;
                        js.Group = jobKey.Group;
                        js.TriggerType = trigger.GetType().Name;
                        js.TriggerState = Instance.GetTriggerState(trigger.Key).ToString();
                        js.Priority = trigger.Priority;

                        DateTimeOffset? startTime = trigger.StartTimeUtc;
                        js.StartTime = TimeZone.CurrentTimeZone.ToLocalTime(startTime.Value.DateTime);

                        DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
                        if (nextFireTime.HasValue)
                        {
                            js.NextFire = TimeZone.CurrentTimeZone.ToLocalTime(nextFireTime.Value.DateTime);
                        }

                        DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
                        if (previousFireTime.HasValue)
                        {
                            js.LastFire = TimeZone.CurrentTimeZone.ToLocalTime(previousFireTime.Value.DateTime);
                        }

                        jcs.Add(js);


                    }
                }
            }
            return jcs;
        }

        public List<JobSchedule> GetSchedules(string groupName)
        {
            var jcs = new List<JobSchedule>();

            var groupMatcher = GroupMatcher<JobKey>.GroupContains(groupName);
            var jobKeys = Instance.GetJobKeys(groupMatcher);

            foreach (var jobKey in jobKeys)
            {
                var triggers = Instance.GetTriggersOfJob(jobKey);
                foreach (var trigger in triggers)
                {
                    var js = new JobSchedule();
                    js.Name = jobKey.Name;
                    js.Description = trigger.Description;
                    js.Group = jobKey.Group;
                    js.TriggerType = trigger.GetType().Name;
                    js.TriggerState = Instance.GetTriggerState(trigger.Key).ToString();
                    js.Priority = trigger.Priority;

                    DateTimeOffset? startTime = trigger.StartTimeUtc;
                    js.StartTime = TimeZone.CurrentTimeZone.ToLocalTime(startTime.Value.DateTime);

                    DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
                    if (nextFireTime.HasValue)
                    {
                        js.NextFire = TimeZone.CurrentTimeZone.ToLocalTime(nextFireTime.Value.DateTime);
                    }

                    DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
                    if (previousFireTime.HasValue)
                    {
                        js.LastFire = TimeZone.CurrentTimeZone.ToLocalTime(previousFireTime.Value.DateTime);
                    }

                    jcs.Add(js);
                }
            }
            return jcs;
        }

        public IList<JobSchedule> GetSchedules2()
        {
            IList<JobSchedule> jobs = new List<JobSchedule>();
            foreach (string group in Instance.GetJobGroupNames())
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = Instance.GetJobKeys(groupMatcher);

                foreach (var jobKey in Instance.GetJobKeys(groupMatcher))
                {
                    var triggers = Instance.GetTriggersOfJob(jobKey);
                    // var detail = Instance.GetJobDetail(jobKey);
                    var isEnter = false;
                    var job = new JobSchedule();
                    job.Name = jobKey.Name;
                    job.Group = jobKey.Group;
                    job.Status = "Stop";

                    //job.Description = detail.Description;
                    foreach (var trigger in triggers)
                    {
                        var triggerStatus = Instance.GetTriggerState(trigger.Key).ToString();

                        if (triggerStatus != "Paused")
                        {
                            job.Status = "Running";
                        }
                        if (isEnter == false)
                        {
                            isEnter = true;

                            job.TriggerState = triggerStatus;
                            job.TriggerType = trigger.Description;
                            job.Priority = trigger.Priority;
                            DateTimeOffset? startTime = trigger.StartTimeUtc;
                            job.StartTime = TimeZone.CurrentTimeZone.ToLocalTime(startTime.Value.DateTime);

                            DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
                            if (nextFireTime.HasValue)
                            {
                                job.NextFire = TimeZone.CurrentTimeZone.ToLocalTime(nextFireTime.Value.DateTime);
                            }

                            DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
                            if (previousFireTime.HasValue)
                            {
                                job.LastFire = TimeZone.CurrentTimeZone.ToLocalTime(previousFireTime.Value.DateTime);
                            }
                            job.Scheduler = Instance.SchedulerName;

                        }
                    }

                    jobs.Add(job);
                }
            }
            return jobs;
        }

        public string GetMetaData()
        {
            var metaData = Instance.GetMetaData();

            return string.Format(
                "{0}Name: '{1}'{0}Version: '{2}'{0}ThreadPoolSize: '{3}'{0}IsRemote: '{4}'{0}JobStoreName: '{5}'                {0}SupportsPersistance: '{6}'{0}IsClustered: '{7}'",
                Environment.NewLine, metaData.SchedulerName, metaData.Version, metaData.ThreadPoolSize,
                metaData.SchedulerRemote, metaData.JobStoreType.Name, metaData.JobStoreSupportsPersistence,
                metaData.JobStoreClustered);
        }

        public bool UnscheduleJob()
        {
            var jobKey = new JobKey(JobName, JobGroup);

            if (Instance.CheckExists(jobKey))
            {
                return Instance.UnscheduleJob(new TriggerKey(JobName, JobGroup));
            }
            return false;
        }

        public bool UnscheduleAll()
        {
            foreach (var group in Instance.GetTriggerGroupNames())
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = Instance.GetJobKeys(groupMatcher);

                foreach (var triggers in jobKeys.Select(jobKey => Instance.GetTriggersOfJob(jobKey)))
                {
                    return Instance.UnscheduleJobs(triggers.Select(t => t.Key).ToList());
                }
            }
            return false;
        }

        public void DeleteAll()
        {
            Instance.Clear();
        }

        public void StartScheduler(string group, string key)
        {
            JobKey jobKey = new JobKey(key, group);
            var triggers = Instance.GetTriggersOfJob(jobKey);
            Instance.RescheduleJob(new TriggerKey(key, group), triggers[0]);
        }

        public void PauseJob(string jobGroup, string jobKey)
        {
            Instance.PauseJob(new JobKey(jobKey, jobGroup));
        }

        public void RunAway(string group, string key)
        {
            Instance.TriggerJob(new JobKey(key, group));
        }

        public void PauseAllJob()
        {
            Instance.PauseAll();
        }

        public void Close()
        {
            if (Instance.IsStarted)
            {
                Instance.Shutdown(false);
            }
        }

        public void RescheduleJob()
        {
            // Build new trigger
            var trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity(JobName, JobGroup)
                .WithCronSchedule(CronExpression)
                .WithPriority(Priority)
                //.StartAt(StartAt.ToUniversalTime())
                .Build();

            Instance.RescheduleJob(new TriggerKey(JobName, JobGroup), trigger);
        }
    }
}
