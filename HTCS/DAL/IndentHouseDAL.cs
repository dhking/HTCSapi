using DAL.Common;
using Mapping.cs;
using Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public   class IndentHouseDAL: RcsBaseDao
    {
        //添加楼层
        public long addfloor(T_Floor model,params string[] arr)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_FLOOR')");
                AddModel(model);
            }
            else
            {

                ModifiedModel(model, false, arr);
            }
            return model.Id;
        }
        //添加房间
        public bool addhouse(long restparameter,long floor, string opera,long househeeper, out string errmsg,out long RoomId, out string name)
        {
            errmsg = "";
            name = "";
            RoomId =0;
            string connectionString = ConfigurationManager.ConnectionStrings["EntityDB"].ConnectionString;
            using (OracleConnection cnn = new OracleConnection(connectionString))
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("sp_addhouseindent", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter paramid = new OracleParameter("restparameter", OracleDbType.Varchar2);
                paramid.Direction = ParameterDirection.Input;
                paramid.Value = restparameter.ToStr();
                cmd.Parameters.Add(paramid);
                OracleParameter paramfloor = new OracleParameter("floor", OracleDbType.Varchar2);
                paramfloor.Direction = ParameterDirection.Input;
                paramfloor.Value = floor.ToStr();
                cmd.Parameters.Add(paramfloor);
                OracleParameter paramopera = new OracleParameter("operator", OracleDbType.Varchar2);
                paramopera.Direction = ParameterDirection.Input;
                paramopera.Value = opera;
                cmd.Parameters.Add(paramopera);
                OracleParameter parahousekeeper = new OracleParameter("vHouseKeeper", OracleDbType.Varchar2);
                parahousekeeper.Direction = ParameterDirection.Input;
                parahousekeeper.Value = househeeper.ToStr();
                cmd.Parameters.Add(parahousekeeper);
                OracleParameter code = new OracleParameter("code", OracleDbType.Int16);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                OracleParameter msg = new OracleParameter("msg", OracleDbType.Varchar2, 500);
                msg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msg);
                OracleParameter pararoomid = new OracleParameter("roomid", OracleDbType.Int64);
                pararoomid.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pararoomid);
                OracleParameter pararoom= new OracleParameter("vname", OracleDbType.Varchar2, 500);
                pararoom.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pararoom);
                cmd.ExecuteNonQuery();
                if (code.Value.ToString() == "0")
                {
                    RoomId = long.Parse(pararoomid.Value.ToStr());
                    name = pararoom.Value.ToStr();
                    return true;
                }
                else
                {
                    
                    errmsg = msg.Value.ToString();
                    return false;
                }
            }
        }
        //删除楼层
        public  int deletefloor(T_Floor model)
        {
            return DeleteModel(model);
        }
        public DbSet<T_Floor> BbFloor { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FloorMaping());  
        }
    }
}
