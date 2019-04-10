using ControllerHelper;
using DBHelp;
using Mapping.cs;
using Model;
using Model.House;
using Model.User;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{
    public class RzDAL : RcsBaseDao
    {
        public bool addrz(long houseid, long userid, long companyid,int RecrntType, out string errmsg)
        {
            errmsg = "";
            this.Database.Connection.Open();
            var cmd = this.Database.Connection.CreateCommand();
            
            OracleParameter paramSkuids = new OracleParameter("restparameter", OracleDbType.Varchar2);
            paramSkuids.Direction = ParameterDirection.Input;
            paramSkuids.Value = houseid;
            cmd.Parameters.Add(paramSkuids);
            OracleParameter paramShopId = new OracleParameter("operator", OracleDbType.Varchar2);
            paramShopId.Direction = ParameterDirection.Input;
            paramShopId.Value = userid;
            cmd.Parameters.Add(paramShopId);

            OracleParameter paratype = new OracleParameter("companyid", OracleDbType.Varchar2);
            paratype.Direction = ParameterDirection.Input;
            paratype.Value = companyid;
            cmd.Parameters.Add(paratype);


            OracleParameter paraRecrntType = new OracleParameter("recrnttype", OracleDbType.Varchar2);
            paraRecrntType.Direction = ParameterDirection.Input;
            paraRecrntType.Value = RecrntType;
            cmd.Parameters.Add(paraRecrntType);


            OracleParameter code = new OracleParameter("code", OracleDbType.Int16);
            code.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(code);
            OracleParameter msg = new OracleParameter("msg", OracleDbType.Varchar2, 500);
            msg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(msg);
            cmd.CommandText = "sp_createrz";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.ExecuteNonQuery();
            this.Database.Connection.Close();
            if (code.Value.ToString() == "0")
            {
                return true;
            }
            else
            {
                errmsg = msg.Value.ToString();
                return false;
            }
        }
        public List<WrapHouseRz> Query(HouseRz model, OrderablePagination orderablePagination)
        {
            //整租查询
            var data = (from a in BHouseRz
                        join n in BUser on a.createperson equals n.Id
                              into temp
                        join c in t_v_HouseQuery on a.houseid equals c.Id
                        into temp1
                        from t in temp.DefaultIfEmpty()
                        from x in temp1.DefaultIfEmpty()
                        select new WrapHouseRz()
                        {
                            Id = a.Id,
                            type=a.type,
                            create=t.RealName,
                            content=a.content,
                            companyid=a.companyid,
                            createtime=a.createtime,
                            houseid=a.houseid,
                            House=x.Name
                        });
            Expression<Func<WrapHouseRz, bool>> where = m => 1 == 1;
            if (model.houseid != 0)
            {
                where = where.And(m => m.houseid == model.houseid);
            }
            if (model.companyid != 0)
            {
                where = where.And(m => m.companyid == model.companyid);
            }
            if (model.type !=null)
            {
                where = where.And(m => m.type == model.type);
            }
            if (model.content != null)
            {
                where = where.And(m => m.content.Contains(model.content));
            }
            if (model.house != null)
            {
                where = where.And(m => m.House.Contains(model.house));
            }
            if (model.BeginTime!= DateTime.MinValue|| model.EndTime != DateTime.MinValue)
            {
                where = where.And(m => m.createtime>= model.BeginTime);
                where = where.And(m => m.createtime<= model.EndTime);
            }

            data = data.Where(where);
            IOrderByExpression<WrapHouseRz> order1 = new OrderByExpression<WrapHouseRz, DateTime>(p => p.createtime, true);
            List<WrapHouseRz> list = QueryableForList(data, orderablePagination, order1);
            return list;
        }

        public long Save(HouseRz model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_HOUSERZ')");
                model.createtime = DateTime.Now;
                AddModel<HouseRz>(model);

            }
            else
            {
                ModifiedModel1<HouseRz>(model, false, new string[] { "" });
            }
            return model.Id;
        }

        public DbSet<HouseRz> BHouseRz { get; set; }
        public DbSet<T_SysUser> BUser { get; set; }
        public DbSet<HouseQuery> t_v_HouseQuery { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new HouseRzMapping());
            modelBuilder.Configurations.Add(new T_SysUserMapping());
            modelBuilder.Configurations.Add(new HouseQueryMapping());
        }
    }
}
