using Model;
using Quartz;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTaskService.JobImpl
{
    [DisallowConcurrentExecution]
    public class CreateQueueJob : BaseJobImpl
    {
        public override void ExecuteJob(IJobExecutionContext context)
        {
            try
            {
                SysResult result = new SysResult();
                ProceService service = new ProceService();
                service.CmdProce1(new Pure() { Spname= "sp_messagequeue" });
                base.execStatus = 1;
                if (result.Code == 0)
                {
                    base.execStatus = 1;
                    base.execMessage = "执行成功." + result.Message;
                }
                else
                {
                    base.execStatus = 2;
                    base.execMessage = "执行失败!" + result.Message;
                }
            }
            catch (Exception ex)
            {

                base.execStatus = 2;
                base.execMessage = "发送催租短信任务异常" + ex.Message;
            }
        }
    }
}
