using Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface ISysAutoTaskHistoryRepository
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <returns></returns>
        int Insert(SysAutoTaskHistoryModel entity, OracleConnection cn = null, OracleTransaction trans = null, string dbType = "sqlserver");

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <returns></returns>
        int Update(SysAutoTaskHistoryModel entity, OracleConnection cn = null, OracleTransaction trans = null);




    }
}
