using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTaskService.JobImpl
{
    [DisallowConcurrentExecution]
    public   class TestJob : BaseJobImpl
    {
        
        public override void ExecuteJob(IJobExecutionContext context)
        {
            try
            {
                logger.Info("测试自动任务");
            }
            catch (Exception ex)
            {
                
                base.execStatus = 2;
                base.execMessage = "上传Emax订单 异常" + ex.Message;
            }
        }
    }
}
