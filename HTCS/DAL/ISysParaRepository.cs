using Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface ISysParaRepository
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <returns></returns>
        int Insert(SysParaModel entity, OracleConnection cn = null, OracleTransaction trans = null);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <returns></returns>
        int Update(SysParaModel entity, OracleConnection cn = null, OracleTransaction trans = null);


        SysParaModel GetById(int Id);

        IList<SysParaModel> GetAllToList(string dbType = "oracle");

        /// <summary>
        /// 通过公司编号和参数值来确认参数值
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        string GetParaValueByParamNameandCompanyId(string paramName, int companyId);
    }
}
