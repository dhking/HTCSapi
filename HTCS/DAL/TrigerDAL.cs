using DAL.Common;
using Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public   class SysAutoTaskTriggerRepository : ISysAutoTaskTriggerRepository, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 通过id获取单个对象
        /// </summary>
        /// <param name="Id">业务主键</param>
        /// <returns>实体类SysAutoTaskTriggerModel</returns>
        public SysAutoTaskTriggerModel GetById(int Id, string dbType = "oracle", bool isLock = false)
        {
            string sql = "SELECT * FROM T_SysAutoTaskTrigger ";

            sql += "WHERE ID=:Id";

            if (dbType == "oracle")
            {
                OracleParameter paramId = new OracleParameter("Id", OracleDbType.Int32, 10);
                paramId.Value = Id;
                DataSet ds = SqlHelper.ExecuteDataset("EntityDB", CommandType.Text, sql, new OracleParameter[] { paramId });
                return ConvertDsToEntity(ds);
            }
            else
            {
                return null;
            }
        }

        public int Insert(SysAutoTaskTriggerModel entity, OracleConnection cn = null, OracleTransaction trans = null)
        {
            throw new NotImplementedException();
        }

        public int Update(SysAutoTaskTriggerModel entity, OracleConnection cn = null, OracleTransaction trans = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将DataSet 转换为实体类.
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns>实体类SysAutoTaskTriggerModel</returns>
        private SysAutoTaskTriggerModel ConvertDsToEntity(DataSet ds)
        {
            if (!ConvertHelper.HasMoreRow(ds))
                return null;

            SysAutoTaskTriggerModel sysautotasktriggermodel = new SysAutoTaskTriggerModel();
            map(sysautotasktriggermodel, ds.Tables[0].Rows[0]);
            return sysautotasktriggermodel;
        }
        private void map(SysAutoTaskTriggerModel entity, DataRow tempdr, bool isFull = true)
        {
            entity.Id = ConvertHelper.ObjToInt(tempdr["Id"]);
            entity.ShowName = ConvertHelper.ObjToStr(tempdr["ShowName"]);
            entity.CronExpression = ConvertHelper.ObjToStr(tempdr["CronExpression"]);
            entity.IsActive = ConvertHelper.ObjToBool(tempdr["IsActive"]);
            entity.OwnerId = ConvertHelper.ObjToStr(tempdr["OwnerId"]);
            entity.ModifierId = ConvertHelper.ObjToStr(tempdr["ModifierId"]);
            entity.CreationDate = ConvertHelper.ObjToDateNull(tempdr["CreationDate"]);
            entity.ModifiedDate = ConvertHelper.ObjToDateNull(tempdr["ModifiedDate"]);
            
        }
    }
}
