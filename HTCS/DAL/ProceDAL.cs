using Model;
using Model.House;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class ProceDAL
    {
        public SysResult CmdProce(Pure modle)
        {
            
            SysResult result = new SysResult();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
                using (OracleConnection cnn = new OracleConnection(connectionString))
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand(modle.Spname,cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    OracleParameter restParameter = new OracleParameter("RestParameter",modle.Id);
                    OracleParameter OperatorParameter = new OracleParameter("electric", modle.Other);

                    OracleParameter UuidParameter = new OracleParameter("uuid", modle.Other1);
                    OracleParameter CodeParameter = new OracleParameter("Code", OracleDbType.Int32);
                    CodeParameter.Direction = ParameterDirection.Output;
                    OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.NVarchar2, 2000);
                    MsgParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(restParameter);
                    cmd.Parameters.Add(OperatorParameter);
                    cmd.Parameters.Add(UuidParameter);
                    cmd.Parameters.Add(CodeParameter);
                    cmd.Parameters.Add(MsgParameter);
                    cmd.ExecuteNonQuery();
                    result.Code = int.Parse(CodeParameter.Value.ToString());
                    result.Message = MsgParameter.Value.ToString();
                }
            }
            catch (Exception ex)
            {
               result= result.ExceptionResult("执行存储过程异常", ex);
            }
          
            return result;
        }

        public SysResult CmdProce2(Pure modle)
        {

            SysResult result = new SysResult();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
                using (OracleConnection cnn = new OracleConnection(connectionString))
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand(modle.Spname, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    OracleParameter OperatorParameter = new OracleParameter("electric", modle.Other);

                    OracleParameter UuidParameter = new OracleParameter("iscuizu", modle.Other1);
                    OracleParameter CodeParameter = new OracleParameter("Code", OracleDbType.Int32);
                    CodeParameter.Direction = ParameterDirection.Output;
                    OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.NVarchar2, 2000);
                    MsgParameter.Direction = ParameterDirection.Output;
                  
                    cmd.Parameters.Add(OperatorParameter);
                    cmd.Parameters.Add(UuidParameter);
                    cmd.Parameters.Add(CodeParameter);
                    cmd.Parameters.Add(MsgParameter);
                    cmd.ExecuteNonQuery();
                    result.Code = int.Parse(CodeParameter.Value.ToString());
                    result.Message = MsgParameter.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("执行存储过程异常", ex);
            }

            return result;
        }
        //新增地址库

        public SysResult CmdProce4(T_CellName  model)
        {

            SysResult result = new SysResult();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
                using (OracleConnection cnn = new OracleConnection(connectionString))
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand("sp_addcellname", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    OracleParameter paraid = new OracleParameter("restparameter", model.Id);
                    OracleParameter paracellname = new OracleParameter("cellname1", model.Name);
                    OracleParameter paraareaname = new OracleParameter("areaname1", model.AreaName);
                    OracleParameter paracityname = new OracleParameter("cityname1", model.CityName);
                    OracleParameter paraptype = new OracleParameter("ptype", model.Type);
                    OracleParameter parapregtype = new OracleParameter("regtype", model.regtype);
                    OracleParameter paraarea = new OracleParameter("area", model.Area);
                    OracleParameter paracity = new OracleParameter("city", model.City);
                    OracleParameter paracompany = new OracleParameter("rcompanyid", model.CompanyId);
                    OracleParameter paraCode = new OracleParameter("Code", OracleDbType.Int32);
                    paraCode.Direction = ParameterDirection.Output;
                    OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.NVarchar2, 2000);
                    MsgParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paraid);
                    cmd.Parameters.Add(paracellname);
                    cmd.Parameters.Add(paraareaname);
                    cmd.Parameters.Add(paracityname);
 
                    cmd.Parameters.Add(paraptype);
                    cmd.Parameters.Add(parapregtype);
                    
                    cmd.Parameters.Add(paraarea);
                    cmd.Parameters.Add(paracity);
                    cmd.Parameters.Add(paracompany);
                    cmd.Parameters.Add(paraCode);
                    cmd.Parameters.Add(MsgParameter);
                    cmd.ExecuteNonQuery();
                    result.Code = int.Parse(paraCode.Value.ToString());
                    result.Message = MsgParameter.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("执行存储过程异常", ex);
            }

            return result;
        }
        //新增大区
        public SysResult CmdProce5(T_CellName model)
        {

            SysResult result = new SysResult();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
                using (OracleConnection cnn = new OracleConnection(connectionString))
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand("sp_addareaname", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                  
                  
                    OracleParameter paraareaname = new OracleParameter("areaname1", model.AreaName);
                    OracleParameter paracityname = new OracleParameter("cityname1", model.CityName);
                  
                    OracleParameter parapregtype = new OracleParameter("regtype", model.regtype);
                    OracleParameter paraptype = new OracleParameter("ptype", model.Type);
                    OracleParameter paracompany = new OracleParameter("rcompanyid", model.CompanyId);
                    OracleParameter paraCode = new OracleParameter("Code", OracleDbType.Int32);
                    paraCode.Direction = ParameterDirection.Output;
                    OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.NVarchar2, 2000);
                    MsgParameter.Direction = ParameterDirection.Output;
                 
                    cmd.Parameters.Add(paraareaname);
                    cmd.Parameters.Add(paracityname);
                    cmd.Parameters.Add(parapregtype);
                    cmd.Parameters.Add(paraptype);
                    
                    cmd.Parameters.Add(paracompany);
                    cmd.Parameters.Add(paraCode);
                    cmd.Parameters.Add(MsgParameter);
                    cmd.ExecuteNonQuery();
                    result.Code = int.Parse(paraCode.Value.ToString());
                    result.Message = MsgParameter.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("执行存储过程异常", ex);
            }

            return result;
        }
        //绑定电表

        public SysResult CmdproceElec(Pure modle)
        {
            SysResult result = new SysResult();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
                using (OracleConnection cnn = new OracleConnection(connectionString))
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand(modle.Spname, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    OracleParameter restParameter = new OracleParameter("RestParameter", modle.Id);
                    OracleParameter OperatorParameter = new OracleParameter("electric", modle.Other);
                    OracleParameter CodeParameter = new OracleParameter("Code", OracleDbType.Int32);
                    CodeParameter.Direction = ParameterDirection.Output;
                    OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.NVarchar2, 2000);
                    MsgParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(restParameter);
                    cmd.Parameters.Add(OperatorParameter);
                    cmd.Parameters.Add(CodeParameter);
                    cmd.Parameters.Add(MsgParameter);
                    cmd.ExecuteNonQuery();
                    result.Code = int.Parse(CodeParameter.Value.ToString());
                    result.Message = MsgParameter.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("执行存储过程异常", ex);
            }

            return result;
        }

        public SysResult Cmdproce10(Pure modle)
        {
            SysResult result = new SysResult();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
                using (OracleConnection cnn = new OracleConnection(connectionString))
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand(modle.Spname, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    OracleParameter restParameter = new OracleParameter("RestParameter", modle.Ids);
                    OracleParameter OperatorParameter = new OracleParameter("operator", modle.roperator);
                    OracleParameter user = new OracleParameter("Other", modle.Other);
                    OracleParameter user1 = new OracleParameter("Other1", modle.Other1);
                    OracleParameter CodeParameter = new OracleParameter("Code", OracleDbType.Int32);
                    CodeParameter.Direction = ParameterDirection.Output;
                    OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.NVarchar2, 2000);
                    MsgParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(restParameter);
                    cmd.Parameters.Add(OperatorParameter);
                    cmd.Parameters.Add(user);
                    cmd.Parameters.Add(user1);
                    cmd.Parameters.Add(CodeParameter);
                    cmd.Parameters.Add(MsgParameter);
                    cmd.ExecuteNonQuery();
                    result.Code = int.Parse(CodeParameter.Value.ToString());
                    result.Message = MsgParameter.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("执行存储过程异常", ex);
            }

            return result;
        }
        public SysResult Cmdproce11(Pure modle,out string cityname,out string areaname,out string cellname)
        {
            SysResult result = new SysResult();
            cityname = "";
            areaname = "";
            cellname = "";
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
                using (OracleConnection cnn = new OracleConnection(connectionString))
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand(modle.Spname, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    OracleParameter restParameter = new OracleParameter("RestParameter", modle.Ids);
                    OracleParameter OperatorParameter = new OracleParameter("operator", modle.roperator);
                    OracleParameter user = new OracleParameter("Other", modle.Other);
                    OracleParameter user1 = new OracleParameter("Other1", modle.Other1);
                    OracleParameter CodeParameter = new OracleParameter("Code", OracleDbType.Int32);
                    CodeParameter.Direction = ParameterDirection.Output;
                    OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.NVarchar2, 2000);
                    MsgParameter.Direction = ParameterDirection.Output;
                    OracleParameter cityParameter = new OracleParameter("city", OracleDbType.NVarchar2, 2000);
                    cityParameter.Direction = ParameterDirection.Output;
                    OracleParameter areaParameter = new OracleParameter("area", OracleDbType.NVarchar2, 2000);
                    areaParameter.Direction = ParameterDirection.Output;
                    OracleParameter cellnameParameter = new OracleParameter("cellname", OracleDbType.NVarchar2, 2000);
                    cellnameParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(restParameter);
                    cmd.Parameters.Add(OperatorParameter);
                    cmd.Parameters.Add(user);
                    cmd.Parameters.Add(user1);
                    cmd.Parameters.Add(cityParameter);
                    cmd.Parameters.Add(areaParameter);
                    cmd.Parameters.Add(cellnameParameter);
                    cmd.Parameters.Add(CodeParameter);
                    cmd.Parameters.Add(MsgParameter);
                    cmd.ExecuteNonQuery();
                    result.Code = int.Parse(CodeParameter.Value.ToString());
                    cityname = cityParameter.Value.ToString();
                    areaname = areaParameter.Value.ToString();
                    cellname = cellnameParameter.Value.ToString();
                    result.Message = MsgParameter.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("执行存储过程异常", ex);
            }

            return result;
        }
        //执行存储过程

        public SysResult Cmdproce8(Pure modle)
        {
            SysResult result = new SysResult();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
                using (OracleConnection cnn = new OracleConnection(connectionString))
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand(modle.Spname, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    OracleParameter restParameter = new OracleParameter("RestParameter", modle.Id);
                    OracleParameter OperatorParameter = new OracleParameter("operator", modle.Other);
                    OracleParameter CodeParameter = new OracleParameter("Code", OracleDbType.Int32);
                    CodeParameter.Direction = ParameterDirection.Output;
                    OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.NVarchar2, 2000);
                    MsgParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(restParameter);
                    cmd.Parameters.Add(OperatorParameter);
                    cmd.Parameters.Add(CodeParameter);
                    cmd.Parameters.Add(MsgParameter);
                    cmd.ExecuteNonQuery();
                    result.Code = int.Parse(CodeParameter.Value.ToString());
                    result.Message = MsgParameter.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("执行存储过程异常", ex);
            }

            return result;
        }

        public SysResult Cmdproce11(Pure modle)
        {
            SysResult result = new SysResult();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
                using (OracleConnection cnn = new OracleConnection(connectionString))
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand(modle.Spname, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    OracleParameter restParameter = new OracleParameter("RestParameter", modle.Id);
                    OracleParameter OperatorParameter = new OracleParameter("operator", modle.Other);
                    OracleParameter CodeParameter = new OracleParameter("Code", OracleDbType.Int32);
                    CodeParameter.Direction = ParameterDirection.Output;
                    OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.NVarchar2, 2000);
                    MsgParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(restParameter);
                    cmd.Parameters.Add(OperatorParameter);
                    cmd.Parameters.Add(CodeParameter);
                    cmd.Parameters.Add(MsgParameter);
                    cmd.ExecuteNonQuery();
                    result.Code = int.Parse(CodeParameter.Value.ToString());
                    result.Message = MsgParameter.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("执行存储过程异常", ex);
            }

            return result;
        }
        public SysResult Cmdproce9(Pure modle)
        {
            SysResult result = new SysResult();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
                using (OracleConnection cnn = new OracleConnection(connectionString))
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand(modle.Spname, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    OracleParameter restParameter = new OracleParameter("RestParameter", modle.Id);
                    OracleParameter OperatorParameter = new OracleParameter("operator", modle.Other);
                    OracleParameter Operatorstatus = new OracleParameter("vstatus", modle.Other2);
                    OracleParameter Operatorreason = new OracleParameter("vreason", modle.Other1);
                    OracleParameter CodeParameter = new OracleParameter("Code", OracleDbType.Int32);
                    CodeParameter.Direction = ParameterDirection.Output;
                    OracleParameter MsgParameter = new OracleParameter("Msg", OracleDbType.NVarchar2, 2000);
                    MsgParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(restParameter);
                    cmd.Parameters.Add(OperatorParameter);
                    cmd.Parameters.Add(Operatorreason);
                    cmd.Parameters.Add(Operatorstatus);
                    cmd.Parameters.Add(CodeParameter);
                    cmd.Parameters.Add(MsgParameter);
                    cmd.ExecuteNonQuery();
                    result.Code = int.Parse(CodeParameter.Value.ToString());
                    result.Message = MsgParameter.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = result.ExceptionResult("执行存储过程异常", ex);
            }

            return result;
        }
    }
}
