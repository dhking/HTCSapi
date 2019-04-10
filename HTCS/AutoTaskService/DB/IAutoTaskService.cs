using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTaskService.DB
{
    public interface IAutoTaskService
    {
        void test();
        /// <summary>
        /// 获取可用的自动任务
        /// </summary>
        /// <returns></returns>
        IList<SysAutoTaskModel> GetAvailableSysAutoTask(out string msg, string dbType = "oracle");

        //获取触发器对象
        SysAutoTaskTriggerModel GetAutoTaskTriggerById(out string msg, int id, string dbType = "oracle");

        /// <summary>
        /// 通过ID获取任务对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //SysAutoTaskModel GetAutoTaskById(int id, string dbType = "sqlserver");

        /// <summary>
        /// 更新自动任务状态
        /// </summary>
        /// <param name="group">空表示所有</param>
        /// <returns></returns>
        //int UpdateAutoTaskJobStatus(int status,int Id,string group);

        /// <summary>
        /// 通过Id更新单个任务的运行情况
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateAutoTaskRunInfo(SysAutoTaskModel entity, out string msg, string dbType = "oracle");


        /// <summary>
        /// 插入任务执行记录
        /// </summary>
        /// <param name="enity"></param>
        /// <returns></returns>
        int Insert(int SysAutoTaskId, byte execStatus, string execMessage, int totalSeconds, string jobPara1, string jobPara2, string OwnerId, string InstanceId, string dbType = "sqlserver");
    }
}
