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
   public   class SysAutoTaskHistoryRepository : ISysAutoTaskHistoryRepository, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public int Insert(SysAutoTaskHistoryModel entity, OracleConnection cn = null, OracleTransaction trans = null, string dbType = "oracle")
        {
            string sql = @"begin
                         select get_wsequences('T_SysAutoTaskHistory') into :newId from dual;
                         INSERT INTO T_SysAutoTaskHistory (
                         ID,
                         SYSAUTOTASKID,
                         EXECSTATUS,
                         EXECMESSAGE,
                         TOTALSECONDS,
                         JOBPARA1,
                         JOBPARA2,
                         ISACTIVE,
                         OWNERID,
                         CREATIONDATE,
                         INSTANCEID
                         
                         )VALUES(
                         :newId,
                         :SYSAUTOTASKID,
                         :EXECSTATUS,
                         :EXECMESSAGE,
                         :TOTALSECONDS,
                         :JOBPARA1,
                         :JOBPARA2,
                         :ISACTIVE,
                         :OWNERID,
                         :CREATIONDATE,
                         :INSTANCEID
                         );
                         end;";

            OracleCommand cmd = new OracleCommand(sql);
            OracleParameter paramId = new OracleParameter(":newId", OracleDbType.Int32);
            paramId.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramId);


            OracleParameter paramSysAutoTaskId = new OracleParameter(":SysAutoTaskId", OracleDbType.Int32);
            paramSysAutoTaskId.Value = entity.SysAutoTaskId;
            cmd.Parameters.Add(paramSysAutoTaskId);


            OracleParameter paramExecStatus = new OracleParameter(":ExecStatus", OracleDbType.Int32);
            paramExecStatus.Value = entity.ExecStatus;
            cmd.Parameters.Add(paramExecStatus);


            OracleParameter paramExecMessage = new OracleParameter(":ExecMessage", OracleDbType.Varchar2, 2000);
            paramExecMessage.Value = entity.ExecMessage;
            cmd.Parameters.Add(paramExecMessage);


            OracleParameter paramTotalSeconds = new OracleParameter(":TotalSeconds", OracleDbType.Int32);
            paramTotalSeconds.Value = entity.TotalSeconds;
            cmd.Parameters.Add(paramTotalSeconds);


            OracleParameter paramJobPara1 = new OracleParameter(":JobPara1", OracleDbType.Varchar2, 80);
            paramJobPara1.Value = entity.JobPara1;
            cmd.Parameters.Add(paramJobPara1);


            OracleParameter paramJobPara2 = new OracleParameter(":JobPara2", OracleDbType.Varchar2, 80);
            paramJobPara2.Value = entity.JobPara2;
            cmd.Parameters.Add(paramJobPara2);


            OracleParameter paramIsActive = new OracleParameter(":IsActive", OracleDbType.Int16);
            paramIsActive.Value = entity.IsActive == true ? 1 : 0;
            cmd.Parameters.Add(paramIsActive);


            OracleParameter paramOwnerId = new OracleParameter(":OwnerId", OracleDbType.NVarchar2);
            paramOwnerId.Value = entity.OwnerId;
            cmd.Parameters.Add(paramOwnerId);


            OracleParameter paramCreationDate = new OracleParameter(":CreationDate", OracleDbType.Date);
            paramCreationDate.Value = entity.CreationDate;
            cmd.Parameters.Add(paramCreationDate);


            OracleParameter paramInstanceId = new OracleParameter(":InstanceId", OracleDbType.NVarchar2, 80);
            paramInstanceId.Value = entity.InstanceId;
            cmd.Parameters.Add(paramInstanceId);


          
          

            foreach (OracleParameter item in cmd.Parameters)
            {
                if (item.Value == null)
                {
                    item.Value = DBNull.Value;
                }
            }
            SqlHelper.ExecuteNonQuery("EntityDB", cmd);

            return Int32.Parse(paramId.Value.ToString());

        }

        public int Update(SysAutoTaskHistoryModel entity, OracleConnection cn = null, OracleTransaction trans = null)
        {
            throw new NotImplementedException();
        }
    }
}
