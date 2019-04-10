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
    public class SysParaRepository : ISysParaRepository, IDisposable
    {

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public int Insert(SysParaModel entity, OracleConnection cn = null, OracleTransaction trans = null)
        {

            string sql = @"
                         begin
                         select get_wsequences('T_SysPara') into :newId from dual;
                         INSERT INTO T_SysPara (
                         ID,
                         SYSPARATYPEID,
                         PARANAME,
                         PARADESC,
                         PARAVALUE,
                         ISACTIVE,
                         OWNERID,
                         MODIFIERID,
                         CREATIONDATE,
                         MODIFIEDDATE,
                         ORDERBY,
                         REMARK,
                         DISPLAYTYPE,
                         DATAOPTION,
                         DEFAULTVALUE
                      )VALUES(
                         :newId,
                         :SYSPARATYPEID,
                         :PARANAME,
                         :PARADESC,
                         :PARAVALUE,
                         :ISACTIVE,
                         :OWNERID,
                         :MODIFIERID,
                         :CREATIONDATE,
                         :MODIFIEDDATE,
                         :ORDERBY,
                         :REMARK,
                         :DISPLAYTYPE,
                         :DATAOPTION,
                         :DEFAULTVALUE);
                         end;";
            OracleCommand cmd = new OracleCommand(sql);
            OracleParameter paramId = new OracleParameter(":newId", OracleDbType.Int32, 10);
            paramId.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramId);


            OracleParameter paramSysParaTypeId = new OracleParameter(":SysParaTypeId", OracleDbType.Int32);
            paramSysParaTypeId.Value = entity.SysParaTypeId;
            cmd.Parameters.Add(paramSysParaTypeId);


            OracleParameter paramParaName = new OracleParameter(":ParaName", OracleDbType.NVarchar2, 80);
            paramParaName.Value = entity.ParaName;
            cmd.Parameters.Add(paramParaName);


            OracleParameter paramParaDesc = new OracleParameter(":ParaDesc", OracleDbType.NVarchar2, 200);
            paramParaDesc.Value = entity.ParaDesc;
            cmd.Parameters.Add(paramParaDesc);


            OracleParameter paramParaValue = new OracleParameter(":ParaValue", OracleDbType.NVarchar2, 200);
            paramParaValue.Value = entity.ParaValue;
            cmd.Parameters.Add(paramParaValue);


            OracleParameter paramIsActive = new OracleParameter(":IsActive", OracleDbType.Int16);
            paramIsActive.Value = entity.IsActive;
            cmd.Parameters.Add(paramIsActive);


            OracleParameter paramOwnerId = new OracleParameter(":OwnerId", OracleDbType.NVarchar2, 30);
            paramOwnerId.Value = entity.OwnerId;
            cmd.Parameters.Add(paramOwnerId);


            OracleParameter paramModifierId = new OracleParameter(":ModifierId", OracleDbType.NVarchar2, 30);
            paramModifierId.Value = entity.ModifierId;
            cmd.Parameters.Add(paramModifierId);


            OracleParameter paramCreationDate = new OracleParameter(":CreationDate", OracleDbType.Date);
            paramCreationDate.Value = entity.CreationDate;
            cmd.Parameters.Add(paramCreationDate);


            OracleParameter paramModifiedDate = new OracleParameter(":ModifiedDate", OracleDbType.Date);
            paramModifiedDate.Value = entity.ModifiedDate;
            cmd.Parameters.Add(paramModifiedDate);


            OracleParameter paramOrderBy = new OracleParameter(":OrderBy", OracleDbType.Int32);
            paramOrderBy.Value = entity.OrderBy;
            cmd.Parameters.Add(paramOrderBy);


            OracleParameter paramRemark = new OracleParameter(":Remark", OracleDbType.NVarchar2, 255);
            paramRemark.Value = entity.Remark;
            cmd.Parameters.Add(paramRemark);


            OracleParameter paramDisplayType = new OracleParameter(":DisplayType", OracleDbType.Int32);
            paramDisplayType.Value = entity.DisplayType;
            cmd.Parameters.Add(paramDisplayType);


            OracleParameter paramDataOption = new OracleParameter(":DataOption", OracleDbType.NVarchar2, 255);
            paramDataOption.Value = entity.DataOption;
            cmd.Parameters.Add(paramDataOption);


            OracleParameter paramDefaultValue = new OracleParameter(":DefaultValue", OracleDbType.NVarchar2, 255);
            paramDefaultValue.Value = entity.DefaultValue;
            cmd.Parameters.Add(paramDefaultValue);

            foreach (OracleParameter item in cmd.Parameters)
            {
                if (item.Value == null)
                {
                    if (item.OracleDbType == OracleDbType.NVarchar2)
                        item.Value = DBNull.Value;
                    else if (item.OracleDbType == OracleDbType.Int16)
                        item.Value = 0;
                    else if (item.OracleDbType == OracleDbType.Date)
                        item.Value = DateTime.MinValue;
                    else
                        item.Value = 0;
                }
            }
            if (cn != null && trans != null)
            {

                SqlHelper.ExecuteNonQuery(cn, cmd, CommandType.Text, sql, trans, null);
            }
            else
            {
                SqlHelper.ExecuteNonQuery("PMSDB", cmd);
            }

            return Int32.Parse(paramId.Value.ToString());
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public int Update(SysParaModel entity, OracleConnection cn = null, OracleTransaction trans = null)
        {

            string sql = @"UPDATE T_SysPara SET
				SYSPARATYPEID=:SYSPARATYPEID,
				PARANAME=:PARANAME,
				PARADESC=:PARADESC,
				PARAVALUE=:PARAVALUE,
				ISACTIVE=:ISACTIVE,
				OWNERID=:OWNERID,
				MODIFIERID=:MODIFIERID,
				CREATIONDATE=:CREATIONDATE,
				MODIFIEDDATE=:MODIFIEDDATE,
				ORDERBY=:ORDERBY,
				REMARK=:REMARK,
				DISPLAYTYPE=:DISPLAYTYPE,
				DATAOPTION=:DATAOPTION,
				DEFAULTVALUE=:DEFAULTVALUE
				WHERE ID=:ID";
            OracleCommand cmd = new OracleCommand(sql);

            OracleParameter paramId = new OracleParameter(":Id", OracleDbType.Int32);
            paramId.Value = entity.Id;
            cmd.Parameters.Add(paramId);


            OracleParameter paramSysParaTypeId = new OracleParameter(":SysParaTypeId", OracleDbType.Int32);
            paramSysParaTypeId.Value = entity.SysParaTypeId;
            cmd.Parameters.Add(paramSysParaTypeId);


            OracleParameter paramParaName = new OracleParameter(":ParaName", OracleDbType.NVarchar2, 80);
            paramParaName.Value = entity.ParaName;
            cmd.Parameters.Add(paramParaName);


            OracleParameter paramParaDesc = new OracleParameter(":ParaDesc", OracleDbType.NVarchar2, 200);
            paramParaDesc.Value = entity.ParaDesc;
            cmd.Parameters.Add(paramParaDesc);


            OracleParameter paramParaValue = new OracleParameter(":ParaValue", OracleDbType.NVarchar2, 200);
            paramParaValue.Value = entity.ParaValue;
            cmd.Parameters.Add(paramParaValue);


            OracleParameter paramIsActive = new OracleParameter(":IsActive", OracleDbType.Int16);
            paramIsActive.Value = entity.IsActive;
            cmd.Parameters.Add(paramIsActive);


            OracleParameter paramOwnerId = new OracleParameter(":OwnerId", OracleDbType.NVarchar2, 30);
            paramOwnerId.Value = entity.OwnerId;
            cmd.Parameters.Add(paramOwnerId);


            OracleParameter paramModifierId = new OracleParameter(":ModifierId", OracleDbType.NVarchar2, 30);
            paramModifierId.Value = entity.ModifierId;
            cmd.Parameters.Add(paramModifierId);


            OracleParameter paramCreationDate = new OracleParameter(":CreationDate", OracleDbType.Date);
            paramCreationDate.Value = entity.CreationDate;
            cmd.Parameters.Add(paramCreationDate);


            OracleParameter paramModifiedDate = new OracleParameter(":ModifiedDate", OracleDbType.Date);
            paramModifiedDate.Value = entity.ModifiedDate;
            cmd.Parameters.Add(paramModifiedDate);


            OracleParameter paramOrderBy = new OracleParameter(":OrderBy", OracleDbType.Int32);
            paramOrderBy.Value = entity.OrderBy;
            cmd.Parameters.Add(paramOrderBy);


            OracleParameter paramRemark = new OracleParameter(":Remark", OracleDbType.NVarchar2, 255);
            paramRemark.Value = entity.Remark;
            cmd.Parameters.Add(paramRemark);


            OracleParameter paramDisplayType = new OracleParameter(":DisplayType", OracleDbType.Int32);
            paramDisplayType.Value = entity.DisplayType;
            cmd.Parameters.Add(paramDisplayType);


            OracleParameter paramDataOption = new OracleParameter(":DataOption", OracleDbType.NVarchar2, 255);
            paramDataOption.Value = entity.DataOption;
            cmd.Parameters.Add(paramDataOption);


            OracleParameter paramDefaultValue = new OracleParameter(":DefaultValue", OracleDbType.NVarchar2, 255);
            paramDefaultValue.Value = entity.DefaultValue;
            cmd.Parameters.Add(paramDefaultValue);


            foreach (OracleParameter item in cmd.Parameters)
            {
                if (item.Value == null)
                {
                    if (item.OracleDbType == OracleDbType.NVarchar2)
                        item.Value = DBNull.Value;
                    else if (item.OracleDbType == OracleDbType.Int16)
                        item.Value = 0;
                    else if (item.OracleDbType == OracleDbType.Date)
                        item.Value = DateTime.MinValue;
                    else
                        item.Value = 0;
                }
            }


            if (cn != null && trans != null)
            {

                return SqlHelper.ExecuteNonQuery(cn, cmd, CommandType.Text, sql, trans, null);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery("PMSDB", cmd);
            }

        }

        public void Dispose()
        {
        }

        private void map(SysParaModel entity, DataRow tempdr, bool isFull = true)
        {
            entity.Id = ConvertHelper.ObjToInt(tempdr["Id"]);
            entity.SysParaTypeId = ConvertHelper.ObjToInt(tempdr["SysParaTypeId"]);
            entity.ParaName = ConvertHelper.ObjToStr(tempdr["ParaName"]);
            entity.ParaDesc = ConvertHelper.ObjToStr(tempdr["ParaDesc"]);
            entity.ParaValue = ConvertHelper.ObjToStr(tempdr["ParaValue"]);
            entity.IsActive = ConvertHelper.ObjToBool(tempdr["IsActive"]);
            entity.OwnerId = ConvertHelper.ObjToStr(tempdr["OwnerId"]);
            entity.ModifierId = ConvertHelper.ObjToStr(tempdr["ModifierId"]);
            entity.CreationDate = ConvertHelper.ObjToDateTime(tempdr["CreationDate"]);
            entity.ModifiedDate = ConvertHelper.ObjToDateTime(tempdr["ModifiedDate"]);
            entity.OrderBy = ConvertHelper.ObjToInt(tempdr["OrderBy"]);
            entity.Remark = ConvertHelper.ObjToStr(tempdr["Remark"]);
            entity.DisplayType = ConvertHelper.ObjToInt(tempdr["DisplayType"]);
            entity.DataOption = ConvertHelper.ObjToStr(tempdr["DataOption"]);
            entity.DefaultValue = ConvertHelper.ObjToStr(tempdr["DefaultValue"]);
        }

        /// <summary>
        /// 执行Sql 返回对象.
        /// </summary>
        /// <param name="sql">DataSet</param>
        /// <returns>实体类SysParaModel</returns>
        private SysParaModel LoadBySql(string sql)
        {
            DataSet ds = SqlHelper.ExecuteDataset("PMSDB", CommandType.Text, sql);
            return ConvertDsToEntity(ds);
        }

        /// <summary>
        /// 将DataSet 转换为实体类.
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns>实体类SysParaModel</returns>
        private SysParaModel ConvertDsToEntity(DataSet ds)
        {
            if (!ConvertHelper.HasMoreRow(ds))
                return null;

            SysParaModel sysparamodel = new SysParaModel();
            map(sysparamodel, ds.Tables[0].Rows[0]);
            return sysparamodel;
        }

        /// <summary>
        /// 通过公司编号和参数值来确认参数值
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string GetParaValueByParamNameandCompanyId(string paramName, int companyId)
        {
            string sql = "SELECT Fn_PMS_GetParaValue(:paramName,:CompanyId) from dual";
            OracleParameter paramparamName = new OracleParameter(":paramName", OracleDbType.NVarchar2);
            paramparamName.Value = paramName;

            OracleParameter paramcompanyId = new OracleParameter(":CompanyId", OracleDbType.Int32);
            paramcompanyId.Value = companyId;
            object o = SqlHelper.ExecuteScalar("OMSDB", CommandType.Text, sql, new OracleParameter[] { paramparamName, paramcompanyId });
            return o.ToStr().Trim();
        }
        /// <summary>
        /// 通过id获取单个对象
        /// </summary>
        /// <param name="Id">业务主键</param>
        /// <returns>实体类SysParaModel</returns>
        public SysParaModel GetById(int Id)
        {
            string sql = "SELECT * FROM T_SysPara WHERE ID=:Id";
            OracleParameter paramId = new OracleParameter(":Id", OracleDbType.Int32, 10);
            paramId.Value = Id;
            DataSet ds = SqlHelper.ExecuteDataset("PMSDB", CommandType.Text, sql, new OracleParameter[] { paramId });
            return ConvertDsToEntity(ds);
        }
        /// <summary>
        /// 获取所有数据到DataSet
        /// </summary>
        /// <returns>数据集</returns>
        public DataSet GetAllToDataSet(string dbType = "oracle")
        {
            string sql = "SELECT * FROM T_SysPara  ";

            DataSet ds = null;
            if (dbType == "oracle")
            {
                ds = SqlHelper.ExecuteDataset("EntityDB", CommandType.Text, sql, null);
            }
            else
            {
                
            }

            return ds;
        }

        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="Id">业务主键</param>
        /// <returns>是否存在记录</returns>
        public bool IsExist(int Id)
        {
            string sql = "SELECT COUNT(*) FROM T_SysPara  WHERE ID=:Id";
            //参数
            OracleParameter paramId = new OracleParameter(":Id", OracleDbType.Int32, 10);
            paramId.Value = Id;
            //执行Sql
            int count = ConvertHelper.ObjToInt(SqlHelper.ExecuteScalar("EntityDB", CommandType.Text, sql, new OracleParameter[] { paramId }));
            if (count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取所有数据到List<SysParaModel>
        /// </summary>
        /// <returns>List<SysParaModel></returns>
        public IList<SysParaModel> GetAllToList(string dbType = "oracle")
        {
            DataSet ds = GetAllToDataSet(dbType);
            if (!ConvertHelper.HasMoreRow(ds))
                return null;
            IList<SysParaModel> list = new List<SysParaModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                SysParaModel entity = new SysParaModel();
                map(entity, row, true);
                list.Add(entity);
            }
            return list;

        }



        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="Id">业务主键</param>
        /// <returns>数据集</returns>
        public DataSet GetByPropertyToDataSet(string propName, OracleDbType dbType, int length, object propValue, string likeToken, string field = "*", string op = "=", bool isLock = false)
        {
            string leftToken = "";
            string rightToken = "";
            if (op.ToLower() == "like")
            {
                if (string.IsNullOrEmpty(likeToken))
                    throw new ArgumentException("参数likeToken不能为空或参数错误!", "likeToken");
                if (likeToken.IndexOf("L") >= 0)
                    leftToken = "'%'+";
                if (likeToken.IndexOf("R") >= 0)
                    rightToken = "+'%'";
            }

            string sql = String.Format("SELECT {0} FROM T_SysPara {4} WHERE {1}{2}{5}{3}{6}", field, propName, op, ":" + propName, isLock == true ? "" : "  ", leftToken, rightToken);
            OracleParameter param = new OracleParameter("" + propName, dbType, length);
            param.Value = propValue;
            DataSet ds = SqlHelper.ExecuteDataset("PMSDB", CommandType.Text, sql, new OracleParameter[] { param });
            return ds;
        }


    }
}
