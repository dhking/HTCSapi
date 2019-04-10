using Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface ISysAutoTaskTriggerRepository
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <returns></returns>
        int Insert(SysAutoTaskTriggerModel entity, OracleConnection cn = null, OracleTransaction trans = null);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <returns></returns>
        int Update(SysAutoTaskTriggerModel entity, OracleConnection cn = null, OracleTransaction trans = null);


        /// <summary>
        /// 通过id获取单个对象
        /// </summary>
        /// <param name="Id">业务主键</param>
        /// <returns>实体类SysAutoTaskTriggerModel</returns>
        SysAutoTaskTriggerModel GetById(int Id, string dbType = "sqlserver", bool isLock = false);

    }
}
