using DAL.Common;
using DBHelp;
using Mapping.cs;
using Mapping.cs.Contrct;
using Model;
using Model.Contrct;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class TuizuDAL : RcsBaseDao
    {
        public bool Cmdxuzu(long tuizuid, string opera, string spname, out string errmsg)
        {
            errmsg = "";
            var cmd = this.Database.Connection.CreateCommand();
            OracleParameter paramSkuids = new OracleParameter("restparameter", OracleDbType.Varchar2);
            paramSkuids.Direction = ParameterDirection.Input;
            paramSkuids.Value = tuizuid;
            cmd.Parameters.Add(paramSkuids);
            OracleParameter paramShopId = new OracleParameter("operator", OracleDbType.Varchar2);
            paramShopId.Direction = ParameterDirection.Input;
            paramShopId.Value = opera;
            cmd.Parameters.Add(paramShopId);
            OracleParameter code = new OracleParameter("code", OracleDbType.Int16);
            code.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(code);
            OracleParameter msg = new OracleParameter("msg", OracleDbType.Varchar2, 500);
            msg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(msg);
            cmd.CommandText = spname;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
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
        public List<Inittuizu> Queryinit()
        {
            var mo = from m in Inittuizu  select  m;
            return mo.ToList();
        }
        public void  Savetuizu(List<Tuizu> listmodel,long zhuid)
        {
            foreach(var mo in listmodel)
            {
                mo.zhuId = zhuid;
                mo.Id=GetNextValNum("GET_WSEQUENCES('T_TUIZU')");
                AddModel<Tuizu>(mo);
            }
        }
        public long updateContrct(T_Contrct model)
        {
            if (ModifiedModel<T_Contrct>(model, false, new string[] { "TeantId", "HouseId", "HouseType" }) > 0)
            {
                return model.Id;
            }
            else
            {
                return 0;
            }
        }
        public long updateOwernContrct(T_OwernContrct model)
        {
            if (ModifiedModel<T_OwernContrct>(model, false, new string[] { "TeantId", "HouseId", "HouseType" }) > 0)
            {
                return model.Id;
            }
            else
            {
                return 0;
            }
        }
        public long Savetuizuzhu(TuizuZhu model)
        {
            model.Id = GetNextValNum("GET_WSEQUENCES('T_TUIZUZHU')");
            AddModel<TuizuZhu>(model);
            return model.Id;
        }
        public long deletemdel(TuizuZhu model)
        {
            
            DeleteModel<TuizuZhu>(model);
            return model.Id;
        }
        public bool Cmdtuizu(long tuizuid, string opera,string spname, out string errmsg)
        {
            errmsg = "";
            var cmd = this.Database.Connection.CreateCommand();
            OracleParameter paramSkuids = new OracleParameter("restparameter", OracleDbType.Varchar2);
            paramSkuids.Direction = ParameterDirection.Input;
            paramSkuids.Value = tuizuid;
            cmd.Parameters.Add(paramSkuids);
            OracleParameter paramShopId = new OracleParameter("operator", OracleDbType.Varchar2);
            paramShopId.Direction = ParameterDirection.Input;
            paramShopId.Value = opera;
            cmd.Parameters.Add(paramShopId);
            OracleParameter code = new OracleParameter("code", OracleDbType.Int16);
            code.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(code);
            OracleParameter msg = new OracleParameter("msg", OracleDbType.Varchar2, 500);
            msg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(msg);
            cmd.CommandText = spname;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
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

        
        public TuizuZhu QueryTuizu(TuizuZhu model)
        {
            var mo = from m in BTuizuZhu where m.ContractId == model.ContractId &&m.Type==1 select m;

            return mo.FirstOrDefault();
        }
        public List<Tuizu> Querylist(TuizuZhu model)
        {
            var mo = from m in BTuizu where m.zhuId == model.Id select m;
            Expression<Func<Tuizu, bool>> where = m => 1 == 1;
            //按照地址筛选
            if (model.Id != 0)
            {
                where = where.And(m => m.zhuId == model.Id);
            }
            if (model.ContractId != 0)
            {
                where = where.And(p => (from b in BTuizuZhu where b.ContractId == model.ContractId select b.Id).Contains(p.Id));
            }
            return mo.ToList();
        }
        public DbSet<Inittuizu> Inittuizu { get; set; }
        public DbSet<T_ZafeiList> Zafeilist { get; set; }
        public DbSet<T_Contrct> Contract { get; set; }
        public DbSet<T_OwernContrct> OwernContract { get; set; }
        public DbSet<TuizuZhu> BTuizuZhu { get; set; }
        public DbSet<Tuizu> BTuizu { get; set; }

        //public DbSet<Tuizu> Tuizu { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ContrctMapping());
            modelBuilder.Configurations.Add(new TuzuZhuMapping());
            modelBuilder.Configurations.Add(new OwerContractMapping());
            modelBuilder.Configurations.Add(new TuikuanMapping());
            modelBuilder.Configurations.Add(new InitTuizuMapping());
            modelBuilder.Configurations.Add(new T_ZafeiListMapping());
        }
    }
}
