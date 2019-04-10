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
    public class SysAutoTaskRepository : ISysAutoTaskRepository, IDisposable
    {
        /// <summary>
        /// 获取所有数据到List<SysAutoTaskModel>
        /// </summary>
        /// <returns>List<SysAutoTaskModel></returns>
        public IList<SysAutoTaskModel> GetAllToList(string dbType = "oracle", bool isLock = false)
        {
            DataSet ds = GetAllToDataSet(dbType, isLock);
            if (!ConvertHelper.HasMoreRow(ds))
                return null;
            IList<SysAutoTaskModel> list = new List<SysAutoTaskModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                SysAutoTaskModel entity = new SysAutoTaskModel();
                map(entity, row, true);
                list.Add(entity);
            }
            return list;

        }
        /// <summary>
        /// 获取所有数据到DataSet
        /// </summary>
        /// <returns>数据集</returns>
        public DataSet GetAllToDataSet(string dbType = "oracle", bool isLock = false)
        {
            string sql = "SELECT * FROM T_SysAutoTask  ";
            if (!isLock && dbType == "sqlserver")
                sql += " ";
            DataSet ds = null;
            if (dbType == "oracle")
            {
                ds = SqlHelper.ExecuteDataset("EntityDB", CommandType.Text, sql, null);
            }
            else
            {
                ds = null;
            }

            return ds;
        }
        private void map(SysAutoTaskModel entity, DataRow tempdr, bool isFull = true)
        {
            entity.Id = ConvertHelper.ObjToInt(tempdr["Id"]);
            entity.JobName = ConvertHelper.ObjToStr(tempdr["JobName"]);
            entity.JobGroup = ConvertHelper.ObjToStr(tempdr["JobGroup"]);
            entity.JobDesc = ConvertHelper.ObjToStr(tempdr["JobDesc"]);
            entity.JobSpName = ConvertHelper.ObjToStr(tempdr["JobSpName"]);
            entity.JobClassName = ConvertHelper.ObjToStr(tempdr["JobClassName"]);
            entity.TotalCount = ConvertHelper.ObjToInt(tempdr["TotalCount"]);
            entity.TotalSeconds = ConvertHelper.ObjToInt(tempdr["TotalSeconds"]);
            entity.JobPara1 = ConvertHelper.ObjToStr(tempdr["JobPara1"]);
            entity.JobPara2 = ConvertHelper.ObjToStr(tempdr["JobPara2"]);
            entity.JobStatus = ConvertHelper.ObjToByte(tempdr["JobStatus"]);
            entity.LastExecStatus = ConvertHelper.ObjToByte(tempdr["LastExecStatus"]);
            entity.LastExecMessage = ConvertHelper.ObjToStr(tempdr["LastExecMessage"]);
            entity.LastExecDate = ConvertHelper.ObjToDateNull(tempdr["LastExecDate"]);
            entity.SysAutoTaskTriggerId = ConvertHelper.ObjToInt(tempdr["SysAutoTaskTriggerId"]);
            entity.IsCanMultiThread = ConvertHelper.ObjToBool(tempdr["IsCanMultiThread"]);
            entity.IsActive = ConvertHelper.ObjToBool(tempdr["IsActive"]);
            entity.OwnerId = ConvertHelper.ObjToStr(tempdr["OwnerId"]);
            entity.ModifierId = ConvertHelper.ObjToStr(tempdr["ModifierId"]);
            entity.CreationDate = ConvertHelper.ObjToDateNull(tempdr["CreationDate"]);
            entity.ModifiedDate = ConvertHelper.ObjToDateNull(tempdr["ModifiedDate"]);
            
        }
        /// <summary>
        /// 通过Id更新单个任务的运行情况
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateAutoTaskRunInfo(SysAutoTaskModel entity, string dbType = "oracle")
        {

            string sql = @"UPDATE T_SysAutoTask SET
				TOTALCOUNT=TOTALCOUNT+:TOTALCOUNT,
				TOTALSECONDS=TOTALSECONDS+:TOTALSECONDS,
				JOBSTATUS=:JOBSTATUS,
				LASTEXECSTATUS=:LASTEXECSTATUS,
				LASTEXECMESSAGE=:LASTEXECMESSAGE,
				LASTEXECDATE=sysdate
				WHERE ID=:ID";
            OracleCommand cmd = new OracleCommand(sql);
            OracleParameter paramId = new OracleParameter(":Id", OracleDbType.Int32);
            paramId.Value = entity.Id;
            cmd.Parameters.Add(paramId);

            OracleParameter paramTotalSeconds = new OracleParameter(":TotalSeconds", OracleDbType.Int32);
            paramTotalSeconds.Value = entity.TotalSeconds;
            cmd.Parameters.Add(paramTotalSeconds);

            OracleParameter paramTotalCount = new OracleParameter(":TotalCount", OracleDbType.Int32);
            paramTotalCount.Value = entity.TotalCount;
            cmd.Parameters.Add(paramTotalCount);

            OracleParameter paramJobStatus = new OracleParameter(":JobStatus", OracleDbType.Int16);
            paramJobStatus.Value = entity.JobStatus;
            cmd.Parameters.Add(paramJobStatus);




            OracleParameter paramLastExecStatus = new OracleParameter(":LastExecStatus", OracleDbType.Int16);
            paramLastExecStatus.Value = entity.LastExecStatus;
            cmd.Parameters.Add(paramLastExecStatus);


            OracleParameter paramLastExecMessage = new OracleParameter(":LastExecMessage", OracleDbType.NVarchar2, 255);
            paramLastExecMessage.Value = entity.LastExecMessage;
            cmd.Parameters.Add(paramLastExecMessage);
            foreach (OracleParameter item in cmd.Parameters)
            {
                if (item.Value == null)
                {
                    item.Value = DBNull.Value;
                }
            }
            return SqlHelper.ExecuteNonQuery("EntityDB", cmd);
        }

        public int Insert(SysAutoTaskModel entity, OracleConnection cn = null, OracleTransaction trans = null)
        {
            throw new NotImplementedException();
        }

        public int Update(SysAutoTaskModel entity, OracleConnection cn = null, OracleTransaction trans = null)
        {
            throw new NotImplementedException();
        }

        public SysAutoTaskModel GetById(int Id, string dbType = "sqlserver", bool isLock = false)
        {
            throw new NotImplementedException();
        }

        public int UpdateAutoTaskJobStatus(int status, int id, string group)
        {
            throw new NotImplementedException();
        }

        public int UpdateAutoTaskParam(SysAutoTaskModel entity, string dbType = "sqlserver")
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
