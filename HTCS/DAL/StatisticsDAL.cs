using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model.User;
using Mapping.cs;
using Model;
using System.Linq.Expressions;
using ControllerHelper;
using DBHelp;
using Model.Menu;
using Mapping.cs.Menu;
using Model.Bill;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DAL
{
    public class StatisticsDAL : RcsBaseDao
    {

        //运营统计数据
        public DataSet StatisticsQuery()
        {

            OracleCommand cmd = new OracleCommand();

            OracleParameter paramwmscode = new OracleParameter("Id", OracleDbType.Varchar2);
            paramwmscode.Direction = ParameterDirection.Input;
            paramwmscode.Value = 1;
            cmd.Parameters.Add(paramwmscode);
            OracleParameter paramCode = new OracleParameter("Code", OracleDbType.Int16);
            paramCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramCode);

            OracleParameter paramMsg = new OracleParameter("Msg", OracleDbType.Varchar2, 2000);
            paramMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramMsg);
            OracleParameter paraCursor1 = new OracleParameter("o_cur1", OracleDbType.RefCursor);
            paraCursor1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor1);
            OracleParameter paraCursor2 = new OracleParameter("o_cur2", OracleDbType.RefCursor);
            paraCursor2.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor2);
            return SqlHelper.ExecuteDataSet("sp_statistics", cmd, "EntityDB", null);
        }
        //财务统计数据
        public DataSet caiwuStatisticsQuery()
        {

            OracleCommand cmd = new OracleCommand();

            OracleParameter paramwmscode = new OracleParameter("Id", OracleDbType.Varchar2);
            paramwmscode.Direction = ParameterDirection.Input;
            paramwmscode.Value = 1;
            cmd.Parameters.Add(paramwmscode);
            OracleParameter paramCode = new OracleParameter("Code", OracleDbType.Int16);
            paramCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramCode);

            OracleParameter paramMsg = new OracleParameter("Msg", OracleDbType.Varchar2, 2000);
            paramMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramMsg);
            OracleParameter paraCursor1 = new OracleParameter("o_cur1", OracleDbType.RefCursor);
            paraCursor1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor1);
            OracleParameter paraCursor2 = new OracleParameter("o_cur2", OracleDbType.RefCursor);
            paraCursor2.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor2);
            return SqlHelper.ExecuteDataSet("sp_caiwustatistics", cmd, "EntityDB", null);
        }
        //首页统计数据
        public DataSet indexStatisticsQuery()
        {
            OracleCommand cmd = new OracleCommand();
            OracleParameter paramwmscode = new OracleParameter("Id", OracleDbType.Varchar2);
            paramwmscode.Direction = ParameterDirection.Input;
            paramwmscode.Value = 1;
            cmd.Parameters.Add(paramwmscode);
            OracleParameter paramCode = new OracleParameter("Code", OracleDbType.Int16);
            paramCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramCode);

            OracleParameter paramMsg = new OracleParameter("Msg", OracleDbType.Varchar2, 2000);
            paramMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramMsg);
            OracleParameter paraCursor1 = new OracleParameter("o_cur1", OracleDbType.RefCursor);
            paraCursor1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor1);
          
            return SqlHelper.ExecuteDataSet("sp_indexstatistics", cmd, "EntityDB", null);
        }
        //pc端运营数据

        public DataSet StatisticsPCQuery()
        {

            OracleCommand cmd = new OracleCommand();
            OracleParameter paramwmscode = new OracleParameter("Id", OracleDbType.Varchar2);
            paramwmscode.Direction = ParameterDirection.Input;
            paramwmscode.Value = 1;
            cmd.Parameters.Add(paramwmscode);
            OracleParameter paramCode = new OracleParameter("Code", OracleDbType.Int16);
            paramCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramCode);

            OracleParameter paramMsg = new OracleParameter("Msg", OracleDbType.Varchar2, 2000);
            paramMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramMsg);
            OracleParameter paraCursor1 = new OracleParameter("statichouse", OracleDbType.RefCursor);
            paraCursor1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor1);
            OracleParameter paraCursor2 = new OracleParameter("o_cur2", OracleDbType.RefCursor);
            paraCursor2.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor2);
            OracleParameter paraCursor3 = new OracleParameter("o_cur3", OracleDbType.RefCursor);
            paraCursor3.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor3);
            OracleParameter paraCursor4 = new OracleParameter("o_cur4", OracleDbType.RefCursor);
            paraCursor4.Direction = ParameterDirection.Output;
           
            cmd.Parameters.Add(paraCursor4);
            return SqlHelper.ExecuteDataSet("sp_pcstatistics", cmd, "EntityDB", null);
        }

        //pc端运营数据
        public DataSet StatisticsPCQuery1(DateTime month,long companyid)
        {

            OracleCommand cmd = new OracleCommand();

            OracleParameter paramwmscode = new OracleParameter("rmonth", OracleDbType.Date);
            paramwmscode.Direction = ParameterDirection.Input;
            paramwmscode.Value = month;
            cmd.Parameters.Add(paramwmscode);
            OracleParameter pararcompany = new OracleParameter("rcompany", OracleDbType.Int64);
            pararcompany.Direction = ParameterDirection.Input;
            pararcompany.Value = companyid;
            cmd.Parameters.Add(pararcompany);
            OracleParameter paramCode = new OracleParameter("Code", OracleDbType.Int16);
            paramCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramCode);

            OracleParameter paramMsg = new OracleParameter("Msg", OracleDbType.Varchar2, 2000);
            paramMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramMsg);
            OracleParameter paraCursor1 = new OracleParameter("o_cur1", OracleDbType.RefCursor);
            paraCursor1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor1);
         
          

           
            return SqlHelper.ExecuteDataSet("sp_pcstatistics1", cmd, "EntityDB", null);
        }
        //消息数量
        public DataSet Statisticsmessage(long  userid)
        {

            OracleCommand cmd = new OracleCommand();
            OracleParameter paramwmscode = new OracleParameter("id", OracleDbType.Int64);
            paramwmscode.Direction = ParameterDirection.Input;
            paramwmscode.Value = userid;
            cmd.Parameters.Add(paramwmscode);
            OracleParameter paramCode = new OracleParameter("Code", OracleDbType.Int16);
            paramCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramCode);

            OracleParameter paramMsg = new OracleParameter("Msg", OracleDbType.Varchar2, 2000);
            paramMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paramMsg);
            OracleParameter paraCursor1 = new OracleParameter("o_cur1", OracleDbType.RefCursor);
            paraCursor1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(paraCursor1);
            return SqlHelper.ExecuteDataSet("sp_statisticsmessage", cmd, "EntityDB", null);
        }

        public int Savememo(T_memo model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_MEMO')");
                return AddModel(model);
            }
            else
            {
                return ModifiedModel(model, false);
            }

        }
        public List<T_memo> Query(T_memo model, OrderablePagination orderablePagination, out int count)
        {
            count = 0;
            var data = from a in Bbmeno select a;
            Expression<Func<T_memo, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            data = data.Where(where);
            data.Count();
            IOrderByExpression<T_memo> order = new OrderByExpression<T_memo, long>(p => p.Id, true);
            return this.QueryableForList<T_memo>(data, orderablePagination, order);
        }
        public List<t_appstatistics> queryappstatistics(t_appstatistics model)
        {
            var mo = from m in appstatistics select m;
            Expression<Func<t_appstatistics, bool>> where = m => 1 == 1;
            if (model.Value != 0)
            {
                where = where.And(m => m.Value == model.Value);
                where = where.And(m => m.Key == "Month");
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            mo = mo.Where(where);
            return mo.ToList();
        }
        public DbSet<T_memo> Bbmeno { get; set; }
        public DbSet<t_appstatistics> appstatistics { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TmemoMapping());
            modelBuilder.Configurations.Add(new appstatisticsMapping());
        }
    }
}
