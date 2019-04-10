using ControllerHelper;
using DBHelp;
using Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.EntityFramework;
using System;
using System.Activities.Statements;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{
    public static class Class1
    {
        public static IEnumerable<T> DistinctBy2<T, TResult>(this IEnumerable<T> source, Func<T, TResult> where)
        {
            HashSet<TResult> hashSetData = new HashSet<TResult>();
            foreach (T item in source)
            {
                if (hashSetData.Add(where(item)))
                {
                    yield return item;
                }
            }
        }
    }
    /// <summary>
    /// 与数据库连接的上下文抽象基类
    /// </summary>
    public abstract class RcsBaseDao : RCSDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public RcsBaseDao() : base("EntityDB")
        {
           
        }

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetCurrentDateTime()
        {
            DateTime time = DateTime.Now;// getEntityManager().queryForUniqueObjectBySql(Date.class, "select LOCALTIMESTAMP  CRTDATE from dual");

            var r = this.SqlQuery<DateTime>("select LOCALTIMESTAMP  CRTDATE from dual").FirstOrDefault();

            return time;
        }

       

       

        /// <summary>
        /// 获取数据库版本
        /// </summary>
        /// <returns></returns>
        public long GetBasicDataVersion()
        {
            //        return getEntityManager().queryForUniqueObjectBySql(BigDecimal.class,
            //"select  SEQ_BASIC_DATA_VERSION.NEXTVAL from dual").longValue();
            long ver = this.SqlQuery<long>("select  SEQ_BASIC_DATA_VERSION.NEXTVAL from dual").FirstOrDefault();

            return ver;
        }

        /// <summary>
        /// 获取系统版本
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public String QueryVersion(String type)
        {
            String sql = "select version_name from XT_VERSION where version_id = (select max(version_id) from  "
                    + "XT_VERSION where version_type = '" + type + "' )";

            return this.SqlQuery<string>(sql).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                if (this.Database != null
                    && this.Database.Connection != null
                    && !string.IsNullOrWhiteSpace(this.Database.Connection.ConnectionString))
                {
                    string connetionStr = this.Database.Connection.ConnectionString;
                    var list = connetionStr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in list)
                    {
                        if (item.ToUpper().Contains("USER ID"))
                        {
                            var userid = item.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();

                            modelBuilder.HasDefaultSchema(userid);

                            break;
                        }
                    }
                }
            }
            catch
            {

            }

            base.OnModelCreating(modelBuilder);

            this.CreateModelMap(modelBuilder);
            //modelBuilder.HasDefaultSchema("RCSDBO");
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException("sql");
            }

            return this.Database.ExecuteSqlCommand(sql, parameters);
        }

        /// <summary>
        /// 查询SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sql))
                {
                    throw new ArgumentNullException("sql");
                }
                return this.Database.SqlQuery<T>(sql, parameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 查询SQL
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable SqlQuery(Type elementType, string sql, params object[] parameters)
        {

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException("sql");
            }
            return this.Database.SqlQuery(elementType, sql, parameters);

        }

        /// <summary>
        /// 查询SQL,带分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="orderablePagination"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IList<T> QueryForListBySql<T>(string sql, OrderablePagination orderablePagination, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException("sql");
            }

            if (orderablePagination == null)
            {
                throw new ArgumentNullException("orderablePagination");
            }

            StringBuilder sb = new StringBuilder();
            int totalRecord = 0;
            sb.Append(@"select * from ( ");
            sb.Append(" select pagT.*, rownum rn  from (");
            sb.Append(sql);
            sb.Append(" ) pagT ");
            sb.Append(" ) where rn >= ");
            sb.Append(orderablePagination.StartIndex + 1);
            sb.Append(" and rn < ");
            sb.Append(orderablePagination.StartIndex + orderablePagination.PageSize + 1);
            var data = this.Database.SqlQuery<T>(sql, parameters);
            totalRecord = data.Count();

            if (totalRecord == 0)
            {
                orderablePagination.TotalCount = totalRecord;
                orderablePagination.CurrentPage = 0;
                return new List<T>();
            }

            orderablePagination.TotalCount = totalRecord;
            orderablePagination.CurrentPage = orderablePagination.StartIndex / orderablePagination.PageSize + 1;
            orderablePagination.PageCount = int.Parse(Math.Ceiling(Decimal.Parse(totalRecord.ToString()) / Decimal.Parse(orderablePagination.PageSize.ToString())).ToString());

            return this.Database.SqlQuery<T>(sb.ToString(), parameters).ToList<T>();
        }

        /// <summary>
        /// 查询SQL,带分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="where"></param>
        /// <param name="orderablePagination"></param>
        /// <param name="orderByExpressions"></param>
        /// <returns></returns>
        public IList<T> QueryForList<T>(DbSet<T> dbSet, Expression<Func<T, bool>> where, OrderablePagination orderablePagination, params IOrderByExpression<T>[] orderByExpressions) where T : class
        {
            if (dbSet == null)
            {
                throw new ArgumentNullException("dbSet");
            }

            if (orderablePagination == null)
            {
                throw new ArgumentNullException("orderablePagination");
            }

            int totalRecord = 0;


            IQueryable<T> list;
            if (where == null)
            {
                list = dbSet;
            }
            else
            {
                list = dbSet.Where(where);
            }


            if (orderByExpressions != null)
            {
                IOrderedQueryable<T> output = null;

                foreach (var orderByExpression in orderByExpressions)
                {
                    if (output == null)
                    {
                        output = orderByExpression.ApplyOrderBy(list);
                    }
                    else
                    {
                        output = orderByExpression.ApplyThenBy(output);
                    }
                }

                if (output != null)
                {
                    list = output;
                }
            }

            totalRecord = list.Count();

            if (totalRecord == 0)
            {
                orderablePagination.TotalCount = totalRecord;
                orderablePagination.CurrentPage = 0;
                return new List<T>();
            }

            int takeInt = orderablePagination.PageSize;
            if (takeInt > totalRecord)
            {
                takeInt = totalRecord;
            }
            int skipInt = ((orderablePagination.StartIndex / orderablePagination.PageSize)) * orderablePagination.PageSize;

            //list = list.Skip(orderablePagination.StartIndex + 1).Take(orderablePagination.StartIndex + orderablePagination.PageSize + 1);

            list = list.Skip(skipInt).Take(takeInt);

            orderablePagination.TotalCount = totalRecord;
            orderablePagination.CurrentPage = orderablePagination.StartIndex / orderablePagination.PageSize + 1;
            orderablePagination.PageCount = int.Parse(Math.Ceiling(Decimal.Parse(totalRecord.ToString()) / Decimal.Parse(orderablePagination.PageSize.ToString())).ToString());


            return list.ToList();
        }

        /// <summary>
        /// 查询SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="where"></param>
        /// <param name="orderByExpressions"></param>
        /// <returns></returns>
        public IList<T> QueryForList<T>(DbSet<T> dbSet, Expression<Func<T, bool>> where, params IOrderByExpression<T>[] orderByExpressions) where T : class
        {
            if (dbSet == null)
            {
                throw new ArgumentNullException("dbSet");
            }

            IQueryable<T> list;
            if (where == null)
            {
                list = dbSet;
            }
            else
            {
                list = dbSet.Where(where);
            }

            if (orderByExpressions != null)
            {
                IOrderedQueryable<T> output = null;

                foreach (var orderByExpression in orderByExpressions)
                {
                    if (output == null)
                    {
                        output = orderByExpression.ApplyOrderBy(list);
                    }
                    else
                    {
                        output = orderByExpression.ApplyThenBy(output);
                    }
                }

                return output == null ? list.ToList() : output.ToList();
            }

            return list.ToList();
        }
        /// <summary>
        /// 查询SQL不Tolist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="where"></param>
        /// <param name="orderByExpressions"></param>
        /// <returns></returns>
        public IQueryable<T> QueryableNoList<T>(IQueryable<T> list, OrderablePagination orderablePagination, params IOrderByExpression<T>[] orderByExpressions) where T : class
        {
            int totalRecord = 0;
            if (orderByExpressions != null)
            {
                IOrderedQueryable<T> output = null;

                foreach (var orderByExpression in orderByExpressions)
                {
                    if (output == null)
                    {
                        output = orderByExpression.ApplyOrderBy(list);
                    }
                    else
                    {
                        output = orderByExpression.ApplyThenBy(output);
                    }
                }

                if (output != null)
                {
                    list = output;
                }
            }

            totalRecord = list.Count();

            if (totalRecord == 0)
            {
                orderablePagination.TotalCount = totalRecord;
                orderablePagination.CurrentPage = 0;
                return null;
            }

            int takeInt = orderablePagination.PageSize;
            if (takeInt > totalRecord)
            {
                takeInt = totalRecord;
            }
            int skipInt = ((orderablePagination.StartIndex / orderablePagination.PageSize)) * orderablePagination.PageSize;

            //list = list.Skip(orderablePagination.StartIndex + 1).Take(orderablePagination.StartIndex + orderablePagination.PageSize + 1);

            list = list.Skip(skipInt).Take(takeInt);

            orderablePagination.TotalCount = totalRecord;
            orderablePagination.CurrentPage = orderablePagination.StartIndex / orderablePagination.PageSize + 1;
            orderablePagination.PageCount = int.Parse(Math.Ceiling(Decimal.Parse(totalRecord.ToString()) / Decimal.Parse(orderablePagination.PageSize.ToString())).ToString());


            return list;
        }
        public List<T> QueryableForList<T>(IQueryable<T> list, OrderablePagination orderablePagination, params IOrderByExpression<T>[] orderByExpressions) where T : class
        {
            int totalRecord = 0;
            if (orderByExpressions != null)
            {
                IOrderedQueryable<T> output = null;

                foreach (var orderByExpression in orderByExpressions)
                {
                    if (output == null)
                    {
                        output = orderByExpression.ApplyOrderBy(list);
                    }
                    else
                    {
                        output = orderByExpression.ApplyThenBy(output);
                    }
                }

                if (output != null)
                {
                    list = output;
                }
            }

            totalRecord = list.Count();

            if (totalRecord == 0)
            {
                orderablePagination.TotalCount = totalRecord;
                orderablePagination.CurrentPage = 0;
                return new List<T>();
            }

            int takeInt = orderablePagination.PageSize;
            if (takeInt > totalRecord)
            {
                takeInt = totalRecord;
            }
            int skipInt = ((orderablePagination.StartIndex / orderablePagination.PageSize)) * orderablePagination.PageSize;

            //list = list.Skip(orderablePagination.StartIndex + 1).Take(orderablePagination.StartIndex + orderablePagination.PageSize + 1);

            list = list.Skip(skipInt).Take(takeInt);

            orderablePagination.TotalCount = totalRecord;
            orderablePagination.CurrentPage = orderablePagination.StartIndex / orderablePagination.PageSize + 1;
            orderablePagination.PageCount = int.Parse(Math.Ceiling(Decimal.Parse(totalRecord.ToString()) / Decimal.Parse(orderablePagination.PageSize.ToString())).ToString());


            return list.ToList();
        }
        /// <summary>
        /// 根据DataSet和对象类型返回该对象集合
        /// </summary>
        public List<object> GetDtoListByDataSet(DataSet ds,int i, Type t)
        {
            List<object> dtoList = new List<object>();

            if (ds == null || ds.Tables.Count < 0) return null;
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            DataTable dt = ds.Tables[i];

            foreach (DataRow row in dt.Rows)
            {
                object dto;
                dto = Activator.CreateInstance(t.GetType());
                propertypes = dto.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        object value = row[tempName];
                        pro.SetValue(dto, value, null);
                    }
                }
                dtoList.Add(dto);
            }

            return dtoList;
        }

        ///// <summary>
        ///// 更新对象 
        ///// </summary>
        ///// <typeparam name="T">修改对象类型</typeparam>
        ///// <param name="dto">修改对象</param>
        ///// <param name="noSaveFields">不需要更新的字段</param>
        ///// <returns>受影响的行数</returns>
        //public int SaveChange<T>(T dto, params string[] noSaveFields) where T : BasicModel
        //{
        //    var en = this.Entry<T>(dto);
        //    en.State = EntityState.Modified;
        //    if (noSaveFields != null && noSaveFields.Any())
        //    {
        //        for (int i = 0; i < noSaveFields.Count(); i++)
        //        {
        //            en.Property(noSaveFields[i]).IsModified = false;
        //        }
        //    }
        //    return this.SaveChanges();
        //}

        ///// <summary>
        ///// 更新对象 
        ///// </summary>
        ///// <typeparam name="T">修改对象类型</typeparam>
        ///// <param name="dto">修改对象</param>
        ///// <param name="functions">不需要更新的字段</param>
        ///// <returns>受影响的行数</returns>
        //public int SaveChange<T, TProperty>(T dto, params Expression<Func<T, TProperty>>[] functions) where T : class
        //{
        //    var en = this.Entry<T>(dto);
        //    en.State = EntityState.Modified;
        //    if (functions != null)
        //    {
        //        foreach (var fun in functions)
        //        {
        //            en.Property(fun).IsModified = false;
        //        }
        //    }

        //    return this.SaveChanges();
        //}

        ///// <summary>
        ///// 泛型更新方法  
        ///// </summary>
        ///// <typeparam name="T">泛型类型</typeparam>
        ///// <param name="dto">泛型对象</param>
        ///// <param name="fieldNames">需要更新的属性  为null时更新全部</param>
        ///// <returns>受影响的行数</returns>
        ///// <summary>
        ///// 添加
        ///// </summary>
        ///// <typeparam name="T">添加的对象类型</typeparam>
        ///// <param name="dto">添加的对象</param>
        ///// <returns></returns>
        //public int SaveChange<T>(T dto) where T : class
        //{
        //    var en = this.Entry<T>(dto);
        //    en.State = EntityState.Added;
        //    return this.SaveChanges();
        //}

        /// <summary>
        /// 公共修改方法(lm)
        /// </summary>
        /// <typeparam name="T">修改的实体类型</typeparam>
        /// <param name="t">被修改的实体</param>
        /// <param name="fields">需要修改的字段</param>
        /// <returns>受影响的行数</returns>
        //public int UpdateModel<T>(T t, params string[] fields) where T : BasicModel
        //{
        //    int result = 0;

        //    t.LastModifyTime = this.GetCurrentDateTime();
        //    var en = this.Entry<T>(t);

        //    en.State = EntityState.Unchanged;

        //    foreach (var field in fields)
        //    {
        //        en.Property(field).IsModified = true;
        //    }

        //    if (!fields.Contains("LastModifier"))
        //        en.Property("LastModifier").IsModified = true;
        //    if (!fields.Contains("LastModifyTime"))
        //        en.Property("LastModifyTime").IsModified = true;
        //    if (!fields.Contains("Validity"))
        //        en.Property("Validity").IsModified = true;

        //    result = this.SaveChanges();

        //    return result;
        //}

        /// <summary>
        /// 公共修改方法(lm) 
        /// 1、如果没有添加到上下文中  默认全部为true  只会修改IsModified=true的属性
        /// 2、如果已添加到上下文中 默认全部为true 且设置为IsModified=false无效 
        /// 3、如果上下文中存在 并且与新实例化的对象主键相同时 报错（上下文中存在相同的主键）
        /// </summary>
        /// <typeparam name="T">修改的实体类型</typeparam>
        /// <param name="t">被修改的实体</param>
        /// <param name="isExist">上下文中是否存在</param>
        /// <param name="notModiFields">不需要修改的字段</param>
        /// <returns>受影响的行数</returns>
        public int ModifiedModel<T>(T t, bool isExist, params string[] notModiFields) where T : BasicModel
        {
            int result = 0;
            var en = this.Entry<T>(t);
            en.State = EntityState.Modified;
            if (!isExist)
            {
                if (notModiFields != null)
                {
                    foreach (var field in notModiFields)
                    {
                        en.Property(field).IsModified = false;
                    }
                }
                //if (!notModiFields.Contains("Creator"))
                //    en.Property("Creator").IsModified = false;
                //if (!notModiFields.Contains("CreateTime"))
                //    en.Property("CreateTime").IsModified = false;
                //if (!notModiFields.Contains("Validity"))
                //    en.Property("Validity").IsModified = false;
            }
            result = this.SaveChanges();
            return result;
        }
        public int ModifiedModel1<T>(T t, bool isExist, params string[] notModiFields) where T : BasicModel
        {
            int result = 0;
            var en = this.Entry<T>(t);
            en.State = EntityState.Unchanged;
            if (!isExist)
            {
                if (notModiFields != null)
                {
                    foreach (var field in notModiFields)
                    {
                        en.Property(field).IsModified = true;
                    }
                }
                //if (!notModiFields.Contains("Creator"))
                //    en.Property("Creator").IsModified = false;
                //if (!notModiFields.Contains("CreateTime"))
                //    en.Property("CreateTime").IsModified = false;
                //if (!notModiFields.Contains("Validity"))
                //    en.Property("Validity").IsModified = false;
            }
            result = this.SaveChanges();
            return result;
        }
        public void PLModifiedModel<T>(T t, bool isExist, params string[] notModiFields) where T : BasicModel
        {
            
            var en = this.Entry<T>(t);
            en.State = EntityState.Modified;
            if (!isExist)
            {
                foreach (var field in notModiFields)
                {
                    en.Property(field).IsModified = false;
                }

                //if (!notModiFields.Contains("Creator"))
                //    en.Property("Creator").IsModified = false;
                //if (!notModiFields.Contains("CreateTime"))
                //    en.Property("CreateTime").IsModified = false;
                //if (!notModiFields.Contains("Validity"))
                //    en.Property("Validity").IsModified = false;
            }
        }

        ///// <summary>
        ///// 公共删除方法(lm)
        ///// </summary>
        ///// <typeparam name="T">删除的实体类型</typeparam>
        ///// <param name="t">被删除的实体</param>
        ///// <returns>受影响的行数</returns>
        public int DeleteModel<T>(T t) where T : BasicModel
        {
            int result = 0;

            var en = this.Entry<T>(t);
            en.State =EntityState.Deleted;
            //en.Property("LastModifier").IsModified = true;
            //en.Property("LastModifyTime").IsModified = true;
            //en.Property("Validity").IsModified = true;
            result = this.SaveChanges();
            return result;
        }
        public void PLDeleteModel<T>(T t) where T : BasicModel
        {
           

            var en = this.Entry<T>(t);
            en.State = EntityState.Deleted;
            //en.Property("LastModifier").IsModified = true;
            //en.Property("LastModifyTime").IsModified = true;
            //en.Property("Validity").IsModified = true;
            
        }
        // <summary>
        // 公共添加方法(lm)
        //</summary>
        // <typeparam name = "T" > 实体类型 </ typeparam >
        // < param name="t">实体对象</param>
        // <returns>受影响的行数</returns>
        public int AddModel<T>(T t) where T : BasicModel
        {
            try
            {
                int result = 0;
                var en = this.Entry<T>(t);
                en.State = EntityState.Added;
                result=this.SaveChanges();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int JAdd<T>(T t)where T: BasicModel
        {
            try
            {
                int result = 0;
                var en = this.Entry<T>(t);
                en.State = EntityState.Added;
                this.SaveChanges();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int PlAddModel<T>(T t) where T : BasicModel
        {
            try
            {
                int result = 0;
                var en = this.Entry<T>(t);
                en.State = EntityState.Added;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 获取sequence值
        /// </summary>
        /// <param name="sequenceName"></param>
        /// <returns></returns>
        public long GetNextValNum(string sequenceName)
        {
            string sql = String.Format("select {0} from dual", sequenceName);

            var result = this.SqlQuery<long>(sql).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// 获取sequence值
        /// </summary>
        /// <param name="sequenceName"></param>
        /// <returns></returns>
        public string GetNextVal(string sequenceName)
        {
            if (this.Database.Connection.State != ConnectionState.Open)
            {
                this.Database.Connection.Open();
            }
            var cmd = this.Database.Connection.CreateCommand();
            cmd.CommandText = String.Format("select {0}.nextval from dual", sequenceName);
            cmd.CommandType = CommandType.Text;

            var result = cmd.ExecuteScalar().ToString();

            return result;
        }
    }

    
  

   


}
