using ControllerHelper;
using DAL.Common;
using DBHelp;
using Mapping.cs;
using Mapping.cs.Contrct;
using Model;
using Model.Bill;
using Model.Contrct;
using Model.House;
using Model.TENANT;
using Model.User;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public class OwerContractDAL: RcsBaseDao
    {
        public long SaveContrct(T_OwernContrct model)
        {

            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_CONTRACT')");
                model.CreateTime = DateTime.Now;
                AddModel<T_OwernContrct>(model);

            }
            else
            {
                ModifiedModel<T_OwernContrct>(model, false, new string[] { "HouseId" });
            }
            return model.Id;
        }
        //查询租客信息
        public T_Teant queryteant(T_Teant model)
        {
            var mo = from m in Teant where m.Phone == model.Phone select m;
            Expression<Func<T_Teant, bool>> where = m => 1 == 1;
            if (model.Phone != null)
            {
                where = where.And(m => m.Phone == model.Phone);
            }
            mo = mo.Where(where);
            return mo.FirstOrDefault();
        }
        //更新租客信息
        public long updatetrant(T_Teant model)
        {
            if (model.Id == 0)
            {
                model.Id = GetNextValNum("GET_WSEQUENCES('T_OWERTEANT')");
                AddModel<T_Teant>(model);

            }
            else
            {
                ModifiedModel<T_Teant>(model, false,new string[] { "mobject"});
            }
            return model.Id;
        }
        public WrapOwernContract QueryId(WrapOwernContract model)
        {
            ZafeiDAL zafei = new DAL.ZafeiDAL();
            //整租查询
            var data = from m in Contract
                       join c in t_v_HouseQuery on m.HouseId equals c.Id
                       into temp1
                       from x in temp1.DefaultIfEmpty()
                       select new WrapOwernContract()
                       {
                           Id = m.Id,
                           BeginTime = m.BeginTime,
                           EndTime = m.EndTime,
                           Deposit=m.Deposit,
                           Pinlv = m.PinLv,
                           Recent = m.Recent,
                           DayRecnet = m.DayRecnet,
                           Remark = m.Remark,
                           Enclosure = m.Enclosure,
                           CreatePerson = m.CreatePerson,
                           Status = m.Status,
                           HouseName = x.Name,
                           BeforeDay = m.BeforeDay,
                           Recivetype = m.Recivetype,
                           RecivedType = m.RecivedType,
                           RecivedAmount = m.RecivedAmount,
                           RecivedAccount = m.RecivedAccount,
                           TeantId = m.TeantId,
                           HouseId = m.HouseId,
                           payee = m.payee,
                           accounts = m.accounts,
                           bank = m.bank,          
    };
            Expression<Func<WrapOwernContract, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.HouseId != 0)
            {
                where = where.And(m => m.HouseId == model.HouseId);
            }
            data = data.Where(where);
            WrapOwernContract contract = data.FirstOrDefault();
            if (contract != null)
            {
                contract.Teant = (from m in Teant where m.Id == contract.TeantId select m).FirstOrDefault();
                List<T_Otherfee> fee = new List<T_Otherfee>();
                fee = QueryFee(contract.Id);
                List<T_Otherfee> otherfee = fee.Where(p => p.IsYajin == 0).ToList();
                List<T_Otherfee> jajin = fee.Where(p => p.IsYajin == 1).DistinctBy(p => p.Name).ToList();
                contract.Otherfee = otherfee;
                contract.Yajin = jajin;
            }
            return contract;
        }
        public List<T_Otherfee> QueryFee(long ContractId)
        {
            var data = from m in Otherfee where m.ContractId == ContractId && m.Object==1 select m;
            return data.ToList();
        }
        public List<WrapOwernContract> Query(WrapOwernContract model, OrderablePagination orderablePagination,T_SysUser user)
        {
            //整租查询
            var data = (from m in Contract
                        join n in Teant on m.TeantId equals n.Id
                        into temp
                        from t in temp.DefaultIfEmpty()
                        join c in t_v_HouseQuery on m.HouseId equals c.Id
                        into temp1
                        from x in temp1.DefaultIfEmpty()

                        select new WrapOwernContract()
                        {
                            Id = m.Id,
                            Name = t.Name == null ? "" : t.Name,
                            BeginTime = m.BeginTime,
                            EndTime = m.EndTime,
                            Status = m.Status == null ? 0 : m.Status,
                            Deposit = m.Deposit,
                            Recent = m.Recent,
                            Pinlv = m.PinLv,
                            Phone = t == null ? "" : t.Phone,
                            CellName = x == null ? "" : x.CellName,
                            HouseName = x == null ? "" : x.Name,
                            City = x == null ? 0 : x.City,
                            Area = x == null ? 0 : x.Area,
                            LockId = x == null ? "" : x.LocalId,
                            HouseType = x == null ? 0 : x.RecentType,
                            HouseId = x == null ? 0 : x.Id,
                            storeid = x == null ? 0 : x.storeid,
                            CompanyId = x == null ? 0 : x.CompanyId,
                            CreatePerson = m.CreatePerson,
                            CreateTime = m.CreateTime
                        });
            Expression<Func<WrapOwernContract, bool>> where = m => 1 == 1;
            if (model.HouseId != 0)
            {
                where = where.And(m => m.HouseId == model.HouseId);
            }
            if (model.storeid != 0)
            {
                where = where.And(m =>user.storeids.Contains(m.storeid));
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (model.HouseType != 0)
            {
                where = where.And(m => m.HouseType == model.HouseType);
            }
            if (model.Name !=null)
            {
                where = where.And(m => m.Name.Contains(model.Name));
            }
            if (model.Status != 0)
            {
                if (model.Status == 1 || model.Status == 7)
                {
                    where = where.And(m => m.Status == model.Status);
                }
                if (model.Status == 4)
                {
                    where = where.And(m => m.Status == 2);
                    where = where.And(m => m.BeginTime > DateTime.Now);
                }
                if (model.Status == 5)
                {
                    where = where.And(m => m.Status == 2);
                    where = where.And(m => m.BeginTime <= DateTime.Now);
                    where = where.And(m => m.EndTime >= DateTime.Now);
                }
                if (model.Status == 6)
                {
                    where = where.And(m => m.Status == 2);
                    where = where.And(m => m.EndTime < DateTime.Now);
                }
                if (model.Status == 7)
                {
                    where = where.And(m => m.Status == 3);
                }
            }
            if (model.City != 0)
            {
                where = where.And(m => m.City == model.City);
            }
            if (model.Area != 0)
            {
                where = where.And(m => m.Area == model.Area);
            }
            if (model.CellName != null)
            {
                where = where.And(m => m.CellName == model.CellName);
            }
            if (model.Content != null)
            {
                where = where.And(m => m.Phone.Contains(model.Content) || m.Name.Contains(model.Content) || m.HouseName.Contains(model.Content) || m.CreatePerson.Contains(model.Content));
            }
            if (model.BeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.BeginTime >= model.BeginTime && m.BeginTime <= model.EndTime);
            }
            if (model.tBeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.EndTime >= model.tBeginTime && m.EndTime <= model.tEndTime);
            }
            if (model.Phone != null)
            {
                where = where.And(m => m.Phone == model.Phone);
            }
            if (model.EndTime != DateTime.MinValue)
            {
                where = where.And(m => m.EndTime >= model.EndTime);
            }
            if (model.Content != null)
            {
                if (model.Type == 1)
                {
                    where = where.And(m => m.Name == model.Content);
                }
            }
            data = data.Where(where);
            IOrderByExpression<WrapOwernContract> order1 = new OrderByExpression<WrapOwernContract, long>(p => p.Id, true);
            List<WrapOwernContract> list = QueryableForList(data, orderablePagination, order1);
            return list;
        }

        public List<WrapOwernContract> excelQuery(WrapOwernContract model, T_SysUser user)
        {
            //整租查询
            var data = (from m in Contract
                        join n in Teant on m.TeantId equals n.Id
                        into temp
                        from t in temp.DefaultIfEmpty()
                        join c in t_v_HouseQuery on m.HouseId equals c.Id
                        into temp1
                        from x in temp1.DefaultIfEmpty()

                        select new WrapOwernContract()
                        {
                            Id = m.Id,
                            Name = t.Name == null ? "" : t.Name,
                            BeginTime = m.BeginTime,
                            EndTime = m.EndTime,
                            Status = m.Status == null ? 0 : m.Status,
                            Deposit = m.Deposit,
                            Recent = m.Recent,
                            Pinlv = m.PinLv,
                            Phone = t == null ? "" : t.Phone,
                            CellName = x == null ? "" : x.CellName,
                            HouseName = x == null ? "" : x.Name,
                            City = x == null ? 0 : x.City,
                            Area = x == null ? 0 : x.Area,
                            LockId = x == null ? "" : x.LocalId,
                            HouseType = x == null ? 0 : x.RecentType,
                            HouseId = x == null ? 0 : x.Id,
                            storeid = x == null ? 0 : x.storeid,
                            CompanyId = x == null ? 0 : x.CompanyId,
                            CreatePerson = m.CreatePerson,
                            CreateTime = m.CreateTime
                        });
            Expression<Func<WrapOwernContract, bool>> where = m => 1 == 1;
            if (model.HouseId != 0)
            {
                where = where.And(m => m.HouseId == model.HouseId);
            }
            if (model.storeid != 0)
            {
                where = where.And(m => user.storeids.Contains(m.storeid));
            }
            if (model.CompanyId != 0)
            {
                where = where.And(m => m.CompanyId == model.CompanyId);
            }
            if (model.HouseType != 0)
            {
                where = where.And(m => m.HouseType == model.HouseType);
            }
            if (model.Name != null)
            {
                where = where.And(m => m.Name.Contains(model.Name));
            }
            if (model.Status != 0)
            {
                if (model.Status == 1 || model.Status == 7)
                {
                    where = where.And(m => m.Status == model.Status);
                }
                if (model.Status == 4)
                {
                    where = where.And(m => m.Status == 2);
                    where = where.And(m => m.BeginTime > DateTime.Now);
                }
                if (model.Status == 5)
                {
                    where = where.And(m => m.Status == 2);
                    where = where.And(m => m.BeginTime <= DateTime.Now);
                    where = where.And(m => m.EndTime >= DateTime.Now);
                }
                if (model.Status == 6)
                {
                    where = where.And(m => m.Status == 2);
                    where = where.And(m => m.EndTime < DateTime.Now);
                }
                if (model.Status == 7)
                {
                    where = where.And(m => m.Status == 3);
                }
            }
            if (model.City != 0)
            {
                where = where.And(m => m.City == model.City);
            }
            if (model.Area != 0)
            {
                where = where.And(m => m.Area == model.Area);
            }
            if (model.CellName != null)
            {
                where = where.And(m => m.CellName == model.CellName);
            }
            if (model.Content != null)
            {
                where = where.And(m => m.Phone.Contains(model.Content) || m.Name.Contains(model.Content) || m.HouseName.Contains(model.Content) || m.CreatePerson.Contains(model.Content));
            }
            if (model.BeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.BeginTime >= model.BeginTime && m.BeginTime <= model.EndTime);
            }
            if (model.tBeginTime != DateTime.MinValue)
            {
                where = where.And(m => m.EndTime >= model.tBeginTime && m.EndTime <= model.tEndTime);
            }
            if (model.Phone != null)
            {
                where = where.And(m => m.Phone == model.Phone);
            }
            if (model.EndTime != DateTime.MinValue)
            {
                where = where.And(m => m.EndTime >= model.EndTime);
            }
            if (model.Content != null)
            {
                if (model.Type == 1)
                {
                    where = where.And(m => m.Name == model.Content);
                }
            }
            data = data.Where(where);
          
            List<WrapOwernContract> list = data.ToList();
            return list;
        }
        public int Querycount1(T_OwernContrct model)
        {
            ZafeiDAL zafei = new DAL.ZafeiDAL();
            //整租查询
            var data = from m in Contract select m;
            Expression<Func<T_OwernContrct, bool>> where = m => 1 == 1;
            if (model.Id != 0)
            {
                where = where.And(m => m.Id == model.Id);
            }
            if (model.HouseId != 0)
            {
                where = where.And(m => m.HouseId == model.HouseId);
            }
            if (model.Status != 0)
            {
                where = where.And(m => m.Status == model.Status);
            }
            data = data.Where(where);
            return data.Count();
        }
        public long SaveTrent(T_Teant model)
        {

            model.Id = GetNextValNum("GET_WSEQUENCES('T_TENANT')");
            if (AddModel(model) > 0)
            {
                return model.Id;
            }
            else
            {
                return 0;
            }

        }
        public int SaveOtherFee(List<T_Otherfee> model, long ContractId)
        {
            if (model.Count() == 0)
            {
                return 1;
            }
            foreach (var mo in model)
            {
                mo.Object = 1;
                mo.ContractId = ContractId;
                mo.Id = GetNextValNum("GET_WSEQUENCES('T_OTHERFEE')");
                PlAddModel<T_Otherfee>(mo);
            }
            return this.SaveChanges();
        }
        public bool CmdBill(long ContractId, string opera, out string errmsg)
        {
            errmsg = "";
            var cmd = this.Database.Connection.CreateCommand();
            OracleParameter paramSkuids = new OracleParameter("restparameter", OracleDbType.Varchar2);
            paramSkuids.Direction = ParameterDirection.Input;
            paramSkuids.Value = ContractId;
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
            cmd.CommandText = "sp_createowerbill";
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
        public bool deleteotherfee(long ContractId, out string errmsg)
        {
            errmsg = "";
            var cmd = this.Database.Connection.CreateCommand();
            OracleParameter paramSkuids = new OracleParameter("restparameter", OracleDbType.Varchar2);
            paramSkuids.Direction = ParameterDirection.Input;
            paramSkuids.Value = ContractId;
            cmd.Parameters.Add(paramSkuids);
            OracleParameter paramobj = new OracleParameter("obj", OracleDbType.Varchar2);
            paramobj.Direction = ParameterDirection.Input;
            paramobj.Value = 1;
            cmd.Parameters.Add(paramobj);
            OracleParameter code = new OracleParameter("code", OracleDbType.Int16);
            code.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(code);
            OracleParameter msg = new OracleParameter("msg", OracleDbType.Varchar2, 500);
            msg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(msg);
            cmd.CommandText = "sp_deletefee";
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
        public DbSet<T_OwernContrct> Contract { get; set; }
        public DbSet<T_Teant> Teant { get; set; }
        public DbSet<HouseModel> House { get; set; }
        public DbSet<HouseQuery> t_v_HouseQuery { get; set; }
        public DbSet<T_Otherfee> Otherfee { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OwerContractMapping());
            modelBuilder.Configurations.Add(new OtherFeeMapping());
            modelBuilder.Configurations.Add(new TenantMapping());
            modelBuilder.Configurations.Add(new HouseMapping());
            modelBuilder.Configurations.Add(new HouseQueryMapping());

        }
    }
}
