using DAL.Common;
using DBHelp;
using Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Common
{
    public class SysTableRepository 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="cmd"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string spName, OracleCommand cmd, string Db = "PMSDB", params OracleParameter[] commandParameters)
        {
            //create & open a OracleConnection, and dispose of it after we are done.
            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings[Db].ConnectionString))
            {
                cn.Open();
                //OracleCommand cmd=new OracleCommand();
                //call the overload that takes a connection in place of the connection string
                return SqlHelper.ExecuteDataset(cn, CommandType.StoredProcedure, spName, ref cmd, commandParameters);
            }
        }





        public int ExecuteScalar(string sql, params OracleParameter[] commandParameters)
        {
            return Int32.Parse(SqlHelper.ExecuteScalar("PMSDb", CommandType.Text, sql, commandParameters).ToString());
        }

        public DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset(connectionString, commandType, commandText, commandParameters);
        }


        public DataSet ExecuteDataset2(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset2(connectionString, commandType, commandText, commandParameters);
        }

        public DataSet ExecuteDataset(OracleTransaction trans, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset(trans, commandType, commandText, commandParameters);
        }
        /**/
        /// <summary>
        /// 执行一个指定连接串上的一个OracleCommand（不返回记录集），使用指定的参数集 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a OracleConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {

            //create & open a OracleConnection, and dispose of it after we are done.
            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            {
                cn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = commandText;
                cmd.CommandTimeout = 1000;
                int result = SqlHelper.ExecuteNonQuery(cn, cmd, commandType, commandText, null, commandParameters);

                return result;
            }
        }

        public int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, int timeoutSecond, params OracleParameter[] commandParameters)
        {

            //create & open a OracleConnection, and dispose of it after we are done.
            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            {
                cn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = commandText;
                cmd.CommandTimeout = timeoutSecond;
                int result = SqlHelper.ExecuteNonQuery(cn, cmd, commandType, commandText, null, commandParameters);

                return result;
            }
        }



        public int ExecuteNonQuery(OracleConnection connection, OracleCommand cmd, CommandType commandType, string commandText, OracleTransaction trans = null, params OracleParameter[] commandParameters)
        {
            return SqlHelper.ExecuteNonQuery(connection, cmd, commandType, commandText, trans, commandParameters);
        }

        public OracleDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            return SqlHelper.ExecuteReader(connectionString, CommandType.Text, commandText, commandParameters);
        }

        public object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            return SqlHelper.ExecuteScalar(connectionString, commandType, commandText, commandParameters);
        }


        /// <summary>
        /// 执行一个指定连接串上的一个OracleCommand（不返回记录集），使用指定的参数集 ,返回对象Id
        /// 主要针对插入这种情况,返回Id
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="id"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, out string id, params OracleParameter[] commandParameters)
        {
            id = "-1";
            //create & open a OracleConnection, and dispose of it after we are done.
            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            {
                cn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = commandText;

                int result = SqlHelper.ExecuteNonQuery(cn, cmd, commandType, commandText, null, commandParameters);

                return result;
            }


        }


       

        /// <summary>
        /// 将DataSet 转换为实体类.
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns>实体类SysTableModel</returns>
      
        /// <summary>
        /// 获取所有数据到DataSet
        /// </summary>
        /// <returns>数据集</returns>
        public DataSet GetAllToDataSet()
        {
            string sql = "SELECT * FROM T_SysTable ";
            DataSet ds = SqlHelper.ExecuteDataset("PMSDB", CommandType.Text, sql, null);
            return ds;
        }

        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="Id">业务主键</param>
        /// <returns>是否存在记录</returns>
        public bool IsExist(int Id)
        {
            string sql = "SELECT COUNT(*) FROM T_SysTable WHERE ID=:Id";
            //参数
            OracleParameter paramId = new OracleParameter(":Id", OracleDbType.Int32, 10);
            paramId.Value = Id;
            //执行Sql
            int count = ConvertHelper.ObjToInt(SqlHelper.ExecuteScalar("PMSDB", CommandType.Text, sql, new OracleParameter[] { paramId }));
            if (count > 0)
                return true;
            else
                return false;
        }




        /// <summary>
        /// 通过Roleid更新角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="list"></param>
        /// <returns></returns>


        /// <summary>
        /// 根据角色和子系统编号获取权限
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="subSystemId"></param>
        /// <returns></returns>
        public DataTable GetPermissionByRoleandSubSysId(string RoleId, int subSystemId)
        {
            OracleParameter paraRoleId = new OracleParameter("RoleId", RoleId);
            OracleParameter paraSubSystemId = new OracleParameter("v_SubSystemId", subSystemId);
            OracleParameter paraCursor = new OracleParameter("o_cur", OracleDbType.RefCursor);
            paraCursor.Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset("PMSDB", CommandType.StoredProcedure, "sp_Sys_GetAuthByRoleSysId", new OracleParameter[] { paraRoleId, paraSubSystemId, paraCursor });

            if (ConvertHelper.HasMoreRow(ds))
                return ds.Tables[0];
            else
                return null;
        }
        public DataSet GetGridRows(string dbName, string view, string where, string sortname, string sortorder, int? pagenumber, int? pagesize, out int rowcount)
        {

            bool pagable = pagenumber.HasValue && pagesize.HasValue;
            if (pagable && pagesize == -1)
                pagable = false;
            if (string.IsNullOrEmpty(sortname))
                sortname = "ID";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "DESC";
            bool sortable = !string.IsNullOrEmpty(sortname) && !string.IsNullOrEmpty(sortorder);

            rowcount = -1;
            int errorCode = -1;

            string fields = " * ";

            OracleCommand cmd = new OracleCommand();

            OracleParameter paramFileds = new OracleParameter("Fields", OracleDbType.Varchar2);
            paramFileds.Direction = ParameterDirection.Input;
            paramFileds.Value = fields;
            cmd.Parameters.Add(paramFileds);

            OracleParameter paramPageTableName = new OracleParameter("PageTableName", OracleDbType.Varchar2);
            paramPageTableName.Direction = ParameterDirection.Input;
            paramPageTableName.Value = view;
            cmd.Parameters.Add(paramPageTableName);

            OracleParameter paramWhereCriterias = new OracleParameter("WhereCriterias", OracleDbType.Varchar2);
            paramWhereCriterias.Direction = ParameterDirection.Input;
            paramWhereCriterias.Value = where;
            cmd.Parameters.Add(paramWhereCriterias);

            OracleParameter paramOrderFields = new OracleParameter("OrderFields", OracleDbType.Varchar2);
            paramOrderFields.Direction = ParameterDirection.Input;
            paramOrderFields.Value = sortname + " " + sortorder;
            cmd.Parameters.Add(paramOrderFields);

            OracleParameter paramPageSize = new OracleParameter("PageSize", OracleDbType.Int32);
            paramPageSize.Direction = ParameterDirection.Input;
            paramPageSize.Value = pagesize;
            cmd.Parameters.Add(paramPageSize);

            OracleParameter paramCurrentPage = new OracleParameter("CurrentPage", OracleDbType.Int32);
            paramCurrentPage.Direction = ParameterDirection.Input;
            paramCurrentPage.Value = pagenumber;
            cmd.Parameters.Add(paramCurrentPage);



            OracleParameter paramRowCount = new OracleParameter("Totalrecords", OracleDbType.Int32);
            paramRowCount.Direction = ParameterDirection.Output;
            // paramRowCount.Value = rowcount;
            cmd.Parameters.Add(paramRowCount);

            OracleParameter paramErrorCode = new OracleParameter("ErrorCode", OracleDbType.Int32);
            paramErrorCode.Direction = ParameterDirection.Output;
            // paramErrorCode.Value = errorCode;
            cmd.Parameters.Add(paramErrorCode);

            OracleParameter paraCursor = new OracleParameter("o_cur", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add(paraCursor);

            DataSet ds = ExecuteDataSet("GeneralPaginationProc", cmd, dbName, null);
            errorCode = paramErrorCode.Value.ToInt();
            rowcount = paramRowCount.Value.ToInt();

            return ds;

        }

      
        /// <summary>
        /// 获取当个对象
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="tableName">表名</param>
        /// <param name="id">主键</param>
        /// <param name="field">字段</param>
        /// <returns>DataTable</returns>
        public DataTable ObjectGet(string db, string tableName, string id, string field = "*", string filter = "")
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE ID=:ID {2}", field, tableName, filter);
            OracleParameter OracleParameter = new OracleParameter(":ID", OracleDbType.Int32);
            OracleParameter.Value = id;
            DataSet ds = ExecuteDataset(db, CommandType.Text, sql, new OracleParameter[] { OracleParameter });
            if (ConvertHelper.HasMoreRow(ds))
                return ds.Tables[0];
            else
                return null;
        }


        //TODO:缓存
        /// <summary>
        /// 获取所有表数据,
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tableName"></param>
        /// <param name="filter"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public DataTable ObjectQuery(string db, string tableName, string filter, string field = "*")
        {

            if (string.IsNullOrEmpty(filter))
                filter = "1=1";

            if (field.IndexOf("*") >= 0 && tableName.IndexOf(",") < 0)
            {
                field = field.Replace("*", "t.*");
                tableName = tableName + " t";
            }

            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}", field, tableName, filter);
            DataSet ds = ExecuteDataset(db, CommandType.Text, sql, null);
            if (ConvertHelper.HasMoreRow(ds))
                return ds.Tables[0];
            else
                return null;
        }


        public DataTable ObjectQuerydetails(string db, string maintableName, string chaildrenname, string FKname, string filter, int startnum, int endnum, string field = "*")
        {

            if (string.IsNullOrEmpty(filter))
                filter = "1=1";

            if (field.IndexOf("*") >= 0 && maintableName.IndexOf(",") < 0)
            {
                field = field.Replace("*", "t.*");
                maintableName = maintableName + " t";
            }
            string sql = string.Format("with v_details as (select id from (select rownum as rn,a.* from (SELECT {0} FROM {1} WHERE {2} order by id desc)a where rownum<={4}) where rn>={3})", field, maintableName, filter, startnum, endnum);
            sql = sql + string.Format(@"SELECT {0} FROM {1} where exists(select null from v_details where v_details.id={2})", chaildrenname + ".*", chaildrenname, FKname);
            new LogService().logInfo(sql);
            DataSet ds = ExecuteDataset(db, CommandType.Text, sql, null);
            if (ConvertHelper.HasMoreRow(ds))
                return ds.Tables[0];
            else
                return null;
        }


        /// <summary>
        /// 获取所有表数据(分页导出),
        /// 
        public DataTable ObjectQuery(string db, string tableName, string filter, int startnum, int endnum, string field = "*")
        {
            if (string.IsNullOrEmpty(filter))
                filter = "1=1";

            if (field.IndexOf("*") >= 0 && tableName.IndexOf(",") < 0)
            {
                field = field.Replace("*", "t.*");
                tableName = tableName + " t";
            }
            string sql = string.Format("select * from (select rownum as rn,a.* from (SELECT {0} FROM {1} WHERE {2} order by id desc)a where rownum<={4}) where rn>={3}", field, tableName, filter, startnum, endnum);
            new LogService().logInfo(sql);
            DataSet ds = ExecuteDataset(db, CommandType.Text, sql, null);
            if (ConvertHelper.HasMoreRow(ds))
                return ds.Tables[0];
            else
                return ds.Tables[0];
        }

        /// <summary>
        /// 删除单个短信
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="tableName">表名</param>
        /// <param name="id"></param>
        /// <returns>返回条数</returns>
        public int ObjectDelete(string db, string tableName, string id)
        {
            string sql = string.Format("DELETE FROM {0} WHERE ID=:ID {1}", tableName, "");
            OracleParameter OracleParameter = new OracleParameter(":ID", OracleDbType.Int32);
            OracleParameter.Value = id;
            int result = ExecuteNonQuery(db, CommandType.Text, sql, new OracleParameter[] { OracleParameter });
            return result;

        }

        public DataTable GetGridRows(string dbName, string view, string where, string sortname, string sortorder, int? pagenumber, int? pagesize, OracleParameter[] sqlPara, out int rowcount, bool IsPaging = true)
        {

            if (IsPaging)//如果可以分页
            {
                bool pagable = pagenumber.HasValue && pagesize.HasValue;
                if (pagable && pagesize == -1)
                    pagable = false;
                if (string.IsNullOrEmpty(sortname))
                    sortname = "ID";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = "DESC";
                bool sortable = !string.IsNullOrEmpty(sortname) && !string.IsNullOrEmpty(sortorder);

                rowcount = -1;
                int errorCode = -1;

                OracleCommand cmd = new OracleCommand();
                OracleParameter paramPageTableName = new OracleParameter("PageTableName", OracleDbType.Varchar2);
                paramPageTableName.Direction = ParameterDirection.Input;
                paramPageTableName.Value = view;
                cmd.Parameters.Add(paramPageTableName);

                OracleParameter paramOrderFields = new OracleParameter("rderFields", OracleDbType.Varchar2, 255);
                paramOrderFields.Direction = ParameterDirection.Input;
                paramOrderFields.Value = sortname + " " + sortorder;
                cmd.Parameters.Add(paramOrderFields);

                OracleParameter paramPageSize = new OracleParameter("PageSize", OracleDbType.Int32);
                paramPageSize.Direction = ParameterDirection.Input;
                paramPageSize.Value = pagesize;
                cmd.Parameters.Add(paramPageSize);

                OracleParameter paramCurrentPage = new OracleParameter("CurrentPage", OracleDbType.Int32);
                paramCurrentPage.Direction = ParameterDirection.Input;
                paramCurrentPage.Value = pagenumber;
                cmd.Parameters.Add(paramCurrentPage);

                OracleParameter paramWhereCriterias = new OracleParameter("WhereCriterias", OracleDbType.Varchar2, 2000);
                paramWhereCriterias.Direction = ParameterDirection.Input;
                paramWhereCriterias.Value = where;
                cmd.Parameters.Add(paramWhereCriterias);

                OracleParameter paramRowCount = new OracleParameter("RowCount", OracleDbType.Int32);
                paramRowCount.Direction = ParameterDirection.Output;
                paramRowCount.Value = rowcount;
                cmd.Parameters.Add(paramRowCount);

                OracleParameter paramErrorCode = new OracleParameter("ErrorCode", OracleDbType.Int32);
                paramErrorCode.Direction = ParameterDirection.Output;
                paramErrorCode.Value = errorCode;
                cmd.Parameters.Add(paramErrorCode);

                OracleParameter paraCursor = new OracleParameter("o_cur", OracleDbType.RefCursor);
                paraCursor.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paraCursor);

                DataTable dt = null;

                DataSet ds = SqlHelper.ExecuteDataset("PMSDB", CommandType.StoredProcedure, "GeneralPaginationProc", null);
                if (ConvertHelper.HasMoreRow(ds))
                {
                    dt = ds.Tables[0];
                }
                errorCode = paramErrorCode.Value.ToInt();
                rowcount = paramRowCount.Value.ToInt();
                return dt;
            }
            else
            {
                string temp = string.IsNullOrEmpty(where) ? " 1=1" : where;
                string basesql = "SELECT * FROM {0} WHERE " + temp;


                if (sqlPara != null)
                {
                    foreach (OracleParameter para in sqlPara)
                    {
                        basesql += " AND " + para.ParameterName.Replace("", "").Replace("_datespanend", "").Replace("_datespanstart", "") + "=" + para.ParameterName;
                    }
                }
                string sql = string.Format(basesql, view);

                DataSet ds = ExecuteDataset(dbName, CommandType.Text, sql, sqlPara);
                rowcount = 0;
                if (ConvertHelper.HasMoreRow(ds))
                {
                    return ds.Tables[0];
                }
                else
                    return null;



            }
        }
        
        public DataTable GetAllMenuAndButtonByRoleId(int roleId)
        {
            OracleCommand cmd = new OracleCommand();

            OracleParameter para = new OracleParameter("v_roleid", OracleDbType.Int32);
            OracleParameter paraCursor = new OracleParameter("o_cur", OracleDbType.RefCursor);
            paraCursor.Direction = ParameterDirection.Output;

            para.Value = roleId;
            DataSet ds = ExecuteDataSet("sp_Sys_GetAllMenuandButton", cmd, "PMSDB", new OracleParameter[] { para, paraCursor });
            if (!ConvertHelper.HasMoreRow(ds))
                return null;
            else
                return ds.Tables[0];

        }
        /// <summary>
        /// 获取自动完成数据
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public DataTable GetAutoCompleteResult(string dbName, string tableName, string columnName, string columnValue, string filter = "")
        {
            string sql = string.Format("SELECT * FROM {0} WHERE {2} {1} LIKE :p0 and rownum<=30 order by {1}", tableName, columnName, filter);
            OracleParameter parameter = new OracleParameter("p0", string.Format("{0}%", columnValue));

            DataSet ds = ExecuteDataset(dbName, CommandType.Text, sql, new OracleParameter[] { parameter });
            if (ConvertHelper.HasMoreRow(ds))
                return ds.Tables[0];
            else
                return null;
        }
        /// <summary>
        /// 获取导出数据方法 此方法会获取当前表的在Wing2.0关联表的数据
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="tableName"></param>
        /// <param name="spName"></param>
        /// <param name="filterRule"></param>
        /// <param name="currentLoginName"></param>
        /// <param name="adminAccount"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataSet DataGridExport2(string dbName, string tableName, string commandText)
        {

            OracleCommand cmd = new OracleCommand();
            OracleParameter mainTableParam = new OracleParameter("MainTableName", OracleDbType.NVarchar2);
            OracleParameter paraCursor1 = new OracleParameter("o_cur", OracleDbType.RefCursor);
            paraCursor1.Direction = ParameterDirection.Output;
            mainTableParam.Value = tableName;

            DataSet ds = ExecuteDataSet("Sp_Sys_ExportTableData", cmd, dbName, new OracleParameter[] { mainTableParam, paraCursor1 });
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dttables = ds.Tables[0];
                //多线程获取数据
                int threadCount = 100;
                Dictionary<int, int> startrownumDic = new Dictionary<int, int>();
                Dictionary<int, int> endrownumDic = new Dictionary<int, int>();
                int j = 10000;
                for (int i = 0; i < threadCount; i++)
                {
                    endrownumDic[i] = j * (i + 1);
                    startrownumDic[i] = j * i + 1;
                }
                foreach (DataRow dr in dttables.Rows)
                {
                    DataTable dt = new DataTable();
                    string fkColumn = dr["FkColumn"].ToStr();
                    string mainTable = dr["MainTable"].ToStr();
                    string chileTable = dr["ChildTable"].ToStr();

                    var source = new CancellationTokenSource();
                    if (mainTable == chileTable)
                    {
                        new LogService().logInfo("获取主表数据开始:" + mainTable + DateTime.Now.ToString());

                        //System.Threading.Tasks.Task<DataTable> parentTask = new System.Threading.Tasks.Task<DataTable>(() =>
                        //{
                        //DataTable dtables = new DataTable();
                        foreach (var k in startrownumDic.Keys)
                        {
                            var data = ObjectQuery(dbName, mainTable, commandText, startrownumDic[k], endrownumDic[k]);
                            if (data != null && data.Rows.Count > 0)
                            {
                                // new System.Threading.Tasks.Task(n => dtables.Merge(data), startrownumDic[k],
                                //TaskCreationOptions.AttachedToParent).Start();
                                lock (dt)
                                {
                                    dt.Merge(data);
                                }
                                if (data.Rows.Count < j)
                                {
                                    break;
                                }
                                new LogService().logInfo("订单数量:" + data.Rows.Count);
                            }
                            else
                            {
                                new LogService().logInfo("跳出主表查询");
                                new LogService().logInfo("主表dtables订单数量:" + dt.Rows.Count);
                                break;
                            }

                        }

                        //    return dtables;
                        //});
                        //parentTask.Start();
                        //parentTask.Wait();
                        new LogService().logInfo("跳出主表开始合并数据");
                        //dt = parentTask.Result;

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            dt.Columns.Remove("rn");
                            new LogService().logInfo("主表订单数量总:" + dt.Rows.Count);
                        }
                        new LogService().logInfo("获取主表数据结束:" + mainTable + DateTime.Now.ToString());

                    }
                    else
                    {
                        new LogService().logInfo("获取子表数据开始:" + chileTable + DateTime.Now.ToString());

                        //System.Threading.Tasks.Task<DataTable> childrentTask = new System.Threading.Tasks.Task<DataTable>(() =>
                        //{
                        //    DataTable dtables = new DataTable();
                        foreach (var k in startrownumDic.Keys)
                        {
                            var data = ObjectQuerydetails(dbName, mainTable, chileTable, chileTable + "." + fkColumn, commandText, startrownumDic[k], endrownumDic[k]);
                            if (data != null && data.Rows.Count > 0)
                            {
                                //new System.Threading.Tasks.Task(n => dtables.Merge(data), startrownumDic[k],
                                //  TaskCreationOptions.AttachedToParent).Start();
                                lock (dt)
                                {
                                    dt.Merge(data);
                                }
                                //dt.Merge(data);
                                if (data.Rows.Count < j)
                                {
                                    break;
                                }
                                new LogService().logInfo("订单商品明细数量:" + data.Rows.Count);
                            }
                            else
                            {
                                new LogService().logInfo("跳出子表查询");
                                new LogService().logInfo("子表dtables订单数量:" + dt.Rows.Count);
                                break;
                            }

                        }
                        //    return dtables;
                        //});
                        //childrentTask.Start();
                        //childrentTask.Wait();
                        new LogService().logInfo("跳出子表表开始合并数据");
                        //dt = childrentTask.Result;
                        new LogService().logInfo("子表数量总数:" + dt.Rows.Count);
                        //合并所有table
                        new LogService().logInfo("获取子表数据结束:" + chileTable + DateTime.Now.ToString());
                    }
                    DataTable dt2 = dt.Copy();
                    dt2.TableName = chileTable;
                    ds.Tables.Add(dt2);
                    GC.Collect();
                    dt.Clear();
                }
                return ds;
            }
            else
            {
                return null;
            }

        }

   
        /// <summary>
        /// 获取系统表按钮
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public DataSet GetSysButtonById(int tableId)
        {
            System.Data.DataSet ds = new DataSet();

            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["PMSDB"].ConnectionString))
            {
                cn.Open();
                string selectQuery = "select a.*,b.TableName from T_SysButton a,T_SysTable b where a.SysTableId=b.id and b.id={0} order by b.id,a.OrderBy";
                selectQuery = string.Format(selectQuery, tableId.ToStr());
                OracleCommand cmd = new OracleCommand(selectQuery, cn);
                OracleDataAdapter ad = new OracleDataAdapter(cmd);
                ad.Fill(ds, "button");

            }


            return ds;
        }
        /// <summary>
        /// T4模板使用
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="buttonOrder"></param>
        /// <returns></returns>
        public DataSet GetSysTableConfig(int tableId, int buttonOrder)
        {
            System.Data.DataSet ds = new DataSet();

            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["PMSDB"].ConnectionString))
            {

                conn.Open();

                string selectQuery = @"select 
       Id           AS     ""Id""
      ,ShowName		AS     ""ShowName""
      ,OrderBy		   AS  ""OrderBy""
      ,TableName	   AS  ""TableName""
      ,RealTableName   AS  ""RealTableName""
      ,ObjUIConfig	   AS  ""ObjUIConfig""
      ,IsBig		   AS  ""IsBig""
      ,DataOptions	   AS  ""DataOptions""
      ,Comments		   AS  ""Comments""
      ,DbName		   AS  ""DbName""
      ,IsActive		   AS  ""IsActive""
      ,OwnerId		   AS  ""OwnerId""
      ,ModifierId	   AS  ""ModifierId""
      ,CreationDate	   AS  ""CreationDate""
      ,ModifiedDate	   AS  ""ModifiedDate""
      ,SpNameForQuery  AS  ""SpNameForQuery""
      ,DK			   AS  ""DK""
      ,DetailViewURL   AS  ""DetailViewURL""
      ,ColumnOrderBy   AS  ""ColumnOrderBy""
      ,IsMultiple	   AS  ""IsMultiple""
      ,AK			   AS  ""AK""
      ,IsPaging		   AS  ""IsPaging""
      ,PageSize		   AS  ""PageSize""
      ,IsShare		   AS  ""IsShare""
      ,DataPermission  AS  ""DataPermission""
      ,CompanyId	   AS  ""CompanyId""
      ,SearchOrderBy   AS  ""SearchOrderBy""
      from T_SysTable T WHERE T.id='" + tableId + "'";
                OracleCommand command = new OracleCommand(selectQuery, conn);
                OracleDataAdapter ad = new OracleDataAdapter(command);
                ad.Fill(ds, "table");

                selectQuery = string.Format(@"SELECT Id              AS  ""Id""
      ,ShowName		   AS  ""ShowName""
      ,OrderBy		   AS  ""OrderBy""
      ,Mask			   AS  ""Mask""
      ,ColumnName	   AS  ""ColumnName""
      ,ColumnType	   AS  ""ColumnType""
      ,SysTableId	   AS  ""SysTableId""
      ,IsUnique		   AS  ""IsUnique""
      ,IsNullable	   AS  ""IsNullable""
      ,IsUpperCase	   AS  ""IsUpperCase""
      ,IsIndexed	   AS  ""IsIndexed""
      ,ObtainType	   AS  ""ObtainType""
      ,DefaultValue	   AS  ""DefaultValue""
      ,DataOptions	   AS  ""DataOptions""
      ,DisplayType	   AS  ""DisplayType""
      ,DisplayRows	   AS  ""DisplayRows""
      ,DisplayColumns  AS  ""DisplayColumns""
      ,Comments		   AS  ""Comments""
      ,IsShowInQuery   AS  ""IsShowInQuery""
      ,QueryOrderBy	   AS  ""QueryOrderBy""
      ,IsNewLine	   AS  ""IsNewLine""
      ,GroupName	   AS  ""GroupName""
      ,IsVirtual	   AS  ""IsVirtual""
      ,ColumnLength	   AS  ""ColumnLength""
      ,Precision	   AS  ""Precision""
      ,width		   AS  ""width""
      ,FKTableId	   AS  ""FKTableId""
      ,Formatter	   AS  ""Formatter""
      ,IsNeedFormatter AS  ""IsNeedFormatter""
      ,Tip			   AS  ""Tip""
      ,CompanyId	   AS  ""CompanyId""
      ,SearchOrderBy   AS  ""SearchOrderBy""
       FROM T_SysColumn T WHERE T.SysTableId='{0}' order by orderby", tableId);
                command = new OracleCommand(selectQuery, conn);
                ad = new OracleDataAdapter(command);
                ad.Fill(ds, "column");

                //关联表
                selectQuery = string.Format(@"SELECT 
                b.Id                as ""Id""
      ,b.OrderBy		   as ""OrderBy""
      ,b.RefDesc		   as ""RefDesc""
      ,b.RefColumnId	   as ""RefColumnId""
      ,b.AssocType	   as ""AssocType""
      ,b.SysTableId	   as ""SysTableId""
      ,b.ChildTableId	   as ""ChildTableId""
      ,b.ChildTableName  as ""ChildTableName""
      ,b.Mask			   as ""Mask""
      ,b.InnerEdit	   as ""InnerEdit""
      ,b.CompanyId	   as ""CompanyId"" ，replace(b.ChildTableName,'T_','') as ""propertyName"",n.ColumnName as ""ColumnName"" FROM T_SysRefTable b left join T_SysColumn n on b.RefColumnId = n.Id  WHERE b.SysTableId='{0}'", tableId);
                command = new OracleCommand(selectQuery, conn);
                ad = new OracleDataAdapter(command);
                ad.Fill(ds, "refTable");

                //按钮
                selectQuery = string.Format(@"select  Id   as ""Id""
      ,BtnName				   as ""BtnName""
      ,BtnClass				   as ""BtnClass""
      ,BtnIcon				   as ""BtnIcon""
      ,BtnScript			   as ""BtnScript""
      ,IsActive				   as ""IsActive""
      ,OrderBy				   as ""OrderBy""
      ,ButtonUrl			   as ""ButtonUrl""
      ,IsSystem				   as ""IsSystem""
      ,SpName				   as ""SpName""
      ,Location				   as ""Location""
      ,SysTableId			   as ""SysTableId""
      ,BtnNo				   as ""BtnNo""
      ,IsShowSave			   as ""IsShowSave""
      ,EditType				   as ""EditType""
      ,DisplayCondition		   as ""DisplayCondition""
      ,SubSystem			   as ""SubSystem""
      ,IsCanDoubleClick		   as ""IsCanDoubleClick""
      ,IsCanBatch			   as ""IsCanBatch""
      ,RefreshType			   as ""RefreshType""
      ,IsShowInGroup		   as ""IsShowInGroup""
      ,IsConfirm			   as ""IsConfirm""
      ,Tip					   as ""Tip""
      ,IsSelfManagementTrans   as ""IsSelfManagementTrans""
      ,IsAutoGenPage		   as ""IsAutoGenPage""
      ,RefButtons			   as ""RefButtons""
      ,RedirectButton		   as ""RedirectButton""
      ,CompanyId			   as ""CompanyId""
                from (select t.*,rownum as rowi from T_SysButton t where t.SysTableId={0} order by t.id) g where rowi={1}", tableId, buttonOrder);
                command = new OracleCommand(selectQuery, conn);
                ad = new OracleDataAdapter(command);
                ad.Fill(ds, "button");

                foreach (DataRow dtr in ds.Tables["refTable"].Rows)
                {
                    //查询出关联表的头表
                    selectQuery = string.Format(@"SELECT 
                    Id           AS     ""Id""
      ,ShowName		AS     ""ShowName""
      ,OrderBy		   AS  ""OrderBy""
      ,TableName	   AS  ""TableName""
      ,RealTableName   AS  ""RealTableName""
      ,ObjUIConfig	   AS  ""ObjUIConfig""
      ,IsBig		   AS  ""IsBig""
      ,DataOptions	   AS  ""DataOptions""
      ,Comments		   AS  ""Comments""
      ,DbName		   AS  ""DbName""
      ,IsActive		   AS  ""IsActive""
      ,OwnerId		   AS  ""OwnerId""
      ,ModifierId	   AS  ""ModifierId""
      ,CreationDate	   AS  ""CreationDate""
      ,ModifiedDate	   AS  ""ModifiedDate""
      ,SpNameForQuery  AS  ""SpNameForQuery""
      ,DK			   AS  ""DK""
      ,DetailViewURL   AS  ""DetailViewURL""
      ,ColumnOrderBy   AS  ""ColumnOrderBy""
      ,IsMultiple	   AS  ""IsMultiple""
      ,AK			   AS  ""AK""
      ,IsPaging		   AS  ""IsPaging""
      ,PageSize		   AS  ""PageSize""
      ,IsShare		   AS  ""IsShare""
      ,DataPermission  AS  ""DataPermission""
      ,CompanyId	   AS  ""CompanyId""
      ,SearchOrderBy   AS  ""SearchOrderBy"" FROM T_SysTable T WHERE T.Id='{0}'", dtr["childtableid"].ToString());
                    command = new OracleCommand(selectQuery, conn);
                    ad = new OracleDataAdapter(command);
                    ad.Fill(ds, dtr["childtablename"].ToString() + "table");

                    //查询出关联表的字段
                    selectQuery = string.Format(@"SELECT 
                    Id              AS  ""Id""
      ,ShowName		   AS  ""ShowName""
      ,OrderBy		   AS  ""OrderBy""
      ,Mask			   AS  ""Mask""
      ,ColumnName	   AS  ""ColumnName""
      ,ColumnType	   AS  ""ColumnType""
      ,SysTableId	   AS  ""SysTableId""
      ,IsUnique		   AS  ""IsUnique""
      ,IsNullable	   AS  ""IsNullable""
      ,IsUpperCase	   AS  ""IsUpperCase""
      ,IsIndexed	   AS  ""IsIndexed""
      ,ObtainType	   AS  ""ObtainType""
      ,DefaultValue	   AS  ""DefaultValue""
      ,DataOptions	   AS  ""DataOptions""
      ,DisplayType	   AS  ""DisplayType""
      ,DisplayRows	   AS  ""DisplayRows""
      ,DisplayColumns  AS  ""DisplayColumns""
      ,Comments		   AS  ""Comments""
      ,IsShowInQuery   AS  ""IsShowInQuery""
      ,QueryOrderBy	   AS  ""QueryOrderBy""
      ,IsNewLine	   AS  ""IsNewLine""
      ,GroupName	   AS  ""GroupName""
      ,IsVirtual	   AS  ""IsVirtual""
      ,ColumnLength	   AS  ""ColumnLength""
      ,Precision	   AS  ""Precision""
      ,width		   AS  ""width""
      ,FKTableId	   AS  ""FKTableId""
      ,Formatter	   AS  ""Formatter""
      ,IsNeedFormatter AS  ""IsNeedFormatter""
      ,Tip			   AS  ""Tip""
      ,CompanyId	   AS  ""CompanyId""
      ,SearchOrderBy   AS  ""SearchOrderBy""
                    FROM T_SysColumn T WHERE T.SysTableId='{0}' order by orderby", dtr["childtableid"].ToString());
                    command = new OracleCommand(selectQuery, conn);
                    ad = new OracleDataAdapter(command);
                    ad.Fill(ds, dtr["childtablename"].ToString() + "column");
                }
                if (ds.Tables["refTable"].Rows.Count > 0)
                {
                    foreach (DataRow refrow in ds.Tables["refTable"].Rows)
                    {
                        selectQuery = string.Format(@"select
                          Id           AS     ""Id""
      ,ShowName		AS     ""ShowName""
      ,OrderBy		   AS  ""OrderBy""
      ,TableName	   AS  ""TableName""
      ,RealTableName   AS  ""RealTableName""
      ,ObjUIConfig	   AS  ""ObjUIConfig""
      ,IsBig		   AS  ""IsBig""
      ,DataOptions	   AS  ""DataOptions""
      ,Comments		   AS  ""Comments""
      ,DbName		   AS  ""DbName""
      ,IsActive		   AS  ""IsActive""
      ,OwnerId		   AS  ""OwnerId""
      ,ModifierId	   AS  ""ModifierId""
      ,CreationDate	   AS  ""CreationDate""
      ,ModifiedDate	   AS  ""ModifiedDate""
      ,SpNameForQuery  AS  ""SpNameForQuery""
      ,DK			   AS  ""DK""
      ,DetailViewURL   AS  ""DetailViewURL""
      ,ColumnOrderBy   AS  ""ColumnOrderBy""
      ,IsMultiple	   AS  ""IsMultiple""
      ,AK			   AS  ""AK""
      ,IsPaging		   AS  ""IsPaging""
      ,PageSize		   AS  ""PageSize""
      ,IsShare		   AS  ""IsShare""
      ,DataPermission  AS  ""DataPermission""
      ,CompanyId	   AS  ""CompanyId""
      ,SearchOrderBy   AS  ""SearchOrderBy""
                        from T_SysTable T WHERE T.ID='{0}' order by orderby", refrow["ChildTableId"].ToString());
                        command = new OracleCommand(selectQuery, conn);
                        ad = new OracleDataAdapter(command);
                        ad.Fill(ds, refrow["ChildTableName"].ToString());

                        selectQuery = string.Format(@"SELECT 
                         Id              AS  ""Id""
      ,ShowName		   AS  ""ShowName""
      ,OrderBy		   AS  ""OrderBy""
      ,Mask			   AS  ""Mask""
      ,ColumnName	   AS  ""ColumnName""
      ,ColumnType	   AS  ""ColumnType""
      ,SysTableId	   AS  ""SysTableId""
      ,IsUnique		   AS  ""IsUnique""
      ,IsNullable	   AS  ""IsNullable""
      ,IsUpperCase	   AS  ""IsUpperCase""
      ,IsIndexed	   AS  ""IsIndexed""
      ,ObtainType	   AS  ""ObtainType""
      ,DefaultValue	   AS  ""DefaultValue""
      ,DataOptions	   AS  ""DataOptions""
      ,DisplayType	   AS  ""DisplayType""
      ,DisplayRows	   AS  ""DisplayRows""
      ,DisplayColumns  AS  ""DisplayColumns""
      ,Comments		   AS  ""Comments""
      ,IsShowInQuery   AS  ""IsShowInQuery""
      ,QueryOrderBy	   AS  ""QueryOrderBy""
      ,IsNewLine	   AS  ""IsNewLine""
      ,GroupName	   AS  ""GroupName""
      ,IsVirtual	   AS  ""IsVirtual""
      ,ColumnLength	   AS  ""ColumnLength""
      ,Precision	   AS  ""Precision""
      ,width		   AS  ""width""
      ,FKTableId	   AS  ""FKTableId""
      ,Formatter	   AS  ""Formatter""
      ,IsNeedFormatter AS  ""IsNeedFormatter""
      ,Tip			   AS  ""Tip""
      ,CompanyId	   AS  ""CompanyId""
      ,SearchOrderBy   AS  ""SearchOrderBy""
                        FROM T_SysColumn T WHERE T.SysTableId='{0}' order by orderby", refrow["ChildTableId"].ToString());
                        command = new OracleCommand(selectQuery, conn);
                        ad = new OracleDataAdapter(command);
                        ad.Fill(ds, refrow["ChildTableName"].ToString() + "ColumnList");
                    }
                }
            }

            return ds;

        }
        /// <summary>
        /// 根据指定的菜单获取当前菜单下所有的系统表
        /// </summary>
        /// <param name="menuId">系统菜单id</param>
        /// <returns></returns>
        public DataTable GetSysTablesBySysMenuId(int menuId)
        {
            string sql = "select nvl(m.ParentMenuId,-1) as ParentId,m.Id as MenuId,m.MenuName,m.SubSystemId,t.Id as Id,t.ShowName,t.TableName,t.CreationDate,t.Comments from T_SysTable t left join T_SysMenu m on(m.SysTableId = t.Id and m.ParentMenuId <>0 and m.ParentMenuId is not null) where m.Id=" + menuId;
            return ExecuteDataset("PMSDB", CommandType.Text, sql, null).Tables[0];
        }


        /// <summary>
        /// 根据角色编号获取子系统权限
        /// </summary>
        /// <param name="RoleId">角色编号</param>
        /// <returns></returns>
        public DataTable GetSubSystemPermissionByRoleId(string RoleId, bool isAdmin)
        {
            OracleParameter[] parameters = null;

            string sql = "";
            if (!isAdmin)
            {
                sql = "SELECT DISTINCT b.id as \"id\",b.SystemName as \"SystemName\",b.OrderBy as \"OrderBy\",b.Link as \"Link\",b.SystemIcon as \"SystemIcon\",b.Remark as \"Remark\"  FROM T_SysRoleMenu a  left join T_SysMenu c  on a.MenuId=c.Id  left join T_SubSystem b on c.SubSystemId=b.Id where  1=1  ";
                string[] roleArray = RoleId.Split(',');
                sql += " AND  b.IsActive=1 AND b.IsShow=1 AND b.ID IS NOT NULL ";
                sql += "AND (";

                parameters = new OracleParameter[roleArray.Length];
                for (int i = 0, k = roleArray.Length; i < k; i++)
                {
                    parameters[i] = new OracleParameter(string.Format("RoleId{0}", i), roleArray[i]);
                    sql += string.Format(" RoleId=:RoleId{0} OR", i);
                }

                sql = sql.Trim().TrimEnd('R').TrimEnd('O');
                sql += " ) ";
                sql += " ORDER BY \"OrderBy\"";
            }
            else
            {
                sql = "SELECT DISTINCT b.id as \"id\",b.SystemName as \"SystemName\",b.OrderBy as \"OrderBy\",b.Link as \"Link\",b.SystemIcon as \"SystemIcon\",b.Remark as \"Remark\"  FROM T_SubSystem b   where b.IsActive=1 AND b.IsShow=1 ORDER BY \"OrderBy\"";
            }

            DataTable dt = ExecuteDataset("PMSDB", CommandType.Text, sql, parameters).Tables[0];
            return dt;

        }
        /// <summary>
        /// 获取存储过程的输出游标
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public DataTable getProceCursorArguments(string procName, string packageName = "1")
        {

            string sql = @"select t.argument_name
                  from all_arguments t 
                 where t.object_name =:proName  and t.data_type='REF CURSOR' and t.in_out='OUT' and ( t.package_name=:packageName or :packageName='1') order by  position";
            OracleParameter paramProName = new OracleParameter(":proName", OracleDbType.Varchar2);
            paramProName.Value = procName.ToUpper();

            OracleParameter paramPackageName = new OracleParameter(":packageName", OracleDbType.Varchar2);
            paramPackageName.Value = packageName.ToUpper();

            DataSet ds = ExecuteDataset("PMSDB", CommandType.Text, sql, new OracleParameter[] { paramProName, paramPackageName });
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
       
    
        public DataSet ExecuteDatasetForSp(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            throw new NotImplementedException();
        }
    }
}
