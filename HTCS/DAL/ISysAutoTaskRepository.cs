using Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface ISysAutoTaskRepository
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <returns></returns>
        int Insert(SysAutoTaskModel entity, OracleConnection cn = null, OracleTransaction trans = null);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <returns></returns>
        int Update(SysAutoTaskModel entity, OracleConnection cn = null, OracleTransaction trans = null);


        /// <summary>
        /// 获取所有数据到List
        /// </summary>
        /// <returns></returns>
        IList<SysAutoTaskModel> GetAllToList(string dbType = "sqlserver", bool isLock = false);

        /// <summary>
        /// 通过id获取单个对象
        /// </summary>
        /// <param name="Id">业务主键</param>
        /// <returns>实体类SysAutoTaskModel</returns>
        SysAutoTaskModel GetById(int Id, string dbType = "sqlserver", bool isLock = false);

        /// <summary>
        /// 更新自动任务状态,用于启动自动任务时
        /// </summary>
        /// <param name="status"></param>
        /// <param name="group">空表示所有</param>
        /// <returns></returns>
        int UpdateAutoTaskJobStatus(int status, int id, string group);

        /// <summary>
        /// 通过Id更新单个任务的运行情况
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateAutoTaskRunInfo(SysAutoTaskModel entity, string dbType = "sqlserver");



        /// <summary>
        /// 更新自动任务的参数,用于自动任务
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        int UpdateAutoTaskParam(SysAutoTaskModel entity, string dbType = "sqlserver");

    }
}
