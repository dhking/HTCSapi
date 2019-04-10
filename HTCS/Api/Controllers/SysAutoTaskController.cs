using API.CommonControllers;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    public class SysAutoTaskController : DataCenterController
    {
        AutoTaskService service = new AutoTaskService();
        //分页查询任务程序配置
        [Route("api/SysAutoTask/Querylist")]
        public SysResult<List<SysAutoTaskModel>> Querylist(SysAutoTaskModel model)
        {
            SysResult<List<SysAutoTaskModel>> sysresult = new SysResult<List<SysAutoTaskModel>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Query(model, this.OrderablePagination);
            return sysresult;
        }
         //查询计划任务管理
        [Route("api/autotaskservice/Querylist")]
        public SysResult<List<SysAutoTaskServiceModel>> Querylist1(SysAutoTaskServiceModel model)
        {
            SysResult<List<SysAutoTaskServiceModel>> sysresult = new SysResult<List<SysAutoTaskServiceModel>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Query1(model, this.OrderablePagination);
            return sysresult;
        }
        //查询触发器列表
        [Route("api/autotasktriger/Querylist")]
        public SysResult<List<SysAutoTaskTriggerModel>> Querylist2(SysAutoTaskTriggerModel model)
        {
            SysResult<List<SysAutoTaskTriggerModel>> sysresult = new SysResult<List<SysAutoTaskTriggerModel>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Query2(model, this.OrderablePagination);
            return sysresult;
        }
        //查询任务执行历史
        [Route("api/autotaskhistory/Querylist")]
        public SysResult<List<SysAutoTaskHistoryModel>> Querylist3(SysAutoTaskHistoryModel model)
        {
            SysResult<List<SysAutoTaskHistoryModel>> sysresult = new SysResult<List<SysAutoTaskHistoryModel>>();
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Query3(model, this.OrderablePagination);
            return sysresult;
        }
        //查询计划任务所有的
        
        [Route("api/autotaskservice/SysSchedules")]
        public SysResult<object> SysSchedules(Server model)
        {
            return service.GetShoulder(model);
        }
        //更改行事件
        [Route("api/autotaskservice/ChangeScheduler")]
        public SysResult ChangeScheduler(Server model)
        {
            return service.ChangeScheduler(model);
        }
    }
}