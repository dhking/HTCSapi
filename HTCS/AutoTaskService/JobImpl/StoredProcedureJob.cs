using DAL.Common;
using Model;
using Oracle.ManagedDataAccess.Client;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTaskService.JobImpl
{
    [DisallowConcurrentExecution]
    public class StoredProcedureJob : BaseJobImpl
    {

        public override void ExecuteJob(Quartz.IJobExecutionContext context)
        {
            SysResult result = new SysResult();

            //if (context.JobDetail.JobDataMap["SpName"].ToStr().IndexOf("_Convert") < 0)
            if (jobPara2 == "Y")
            {
                using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings[jobPara1].ConnectionString))
                {

                    cn.Open();
                    OracleTransaction trans = cn.BeginTransaction();//初始化事物
                    try
                    {


                        OracleParameter restParameter = new OracleParameter("RestParameter", "");
                        OracleParameter OperatorParameter = new OracleParameter("Operator", "AutoTaskService");
                        OracleParameter CodeParameter = new OracleParameter("Code", OracleDbType.Int32);
                        CodeParameter.Direction = ParameterDirection.Output;
                        OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.Varchar2, 2000);
                        MsgParameter.Direction = ParameterDirection.Output;
                        OracleCommand cmd = new OracleCommand();

                        cmd.CommandTimeout = 1000 * 60 * 20;
                        //s.ExecuteNonQuery(st.DbName, CommandType.StoredProcedure, buttonModel.SpName, newOracleParameter[] { restParameter, OperatorParameter, CodeParameter, MsgParameter });
                        new SysTableRepository().ExecuteNonQuery(cn, cmd, CommandType.StoredProcedure, context.JobDetail.JobDataMap["SpName"].ToStr(), trans, new OracleParameter[] { restParameter, OperatorParameter, CodeParameter, MsgParameter });
                        result.Code = CodeParameter.Value.ToInt();
                        result.Message = MsgParameter.Value.ToStr();
                        if (result.Code == 0)
                        {
                            base.execStatus = 1;
                            base.execMessage = "执行成功." + result.Message;
                            trans.Commit();
                            cn.Close();
                        }
                        else
                        {
                            base.execStatus = 2;
                            base.execMessage = "执行失败!" + result.Message;
                            trans.Rollback();
                            cn.Close();
                        }


                    }
                    catch (Exception ce)
                    {

                        base.execStatus = 2;
                        base.execMessage = "执行任务时出现异常!" + ce.Message;
                        base.logException(ce, "执行任务时出现异常!任务编号:" + base.taskId.ToStr());
                        trans.Rollback();
                        cn.Close();
                    }

                }
            }
            else
            {

                OracleParameter restParameter = new OracleParameter("RestParameter", "");
                OracleParameter OperatorParameter = new OracleParameter("Operator", "AutoTaskService");
                OracleParameter CodeParameter = new OracleParameter("Code", OracleDbType.Int32);
                CodeParameter.Direction = ParameterDirection.Output;
                OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.Varchar2, 2000);
                MsgParameter.Direction = ParameterDirection.Output;
                new SysTableRepository().ExecuteNonQuery(jobPara1, CommandType.StoredProcedure, context.JobDetail.JobDataMap["SpName"].ToStr(), 20 * 60, new OracleParameter[] { restParameter, OperatorParameter, CodeParameter, MsgParameter });
                
                result.Code = CodeParameter.Value.ToInt();
                result.Message = MsgParameter.Value.ToStr();

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
        }

    }
}
