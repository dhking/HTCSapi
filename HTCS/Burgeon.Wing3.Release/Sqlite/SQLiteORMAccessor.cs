using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.SQLite
{
    /// <summary>
    /// 封装通用的操作SQLite数据库对应表的的基础模型
    /// </summary>
    public class SQLiteORMAccessor
    {
        private static string connection = "";

        static SQLiteORMAccessor()
        {
            string path = Utils.IOUtil.CombineDIR(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "version.db");
            connection = string.Format("Data Source={0};Pooling=true;FailIfMissing=false", path);
            SqliteORM.DbConnection.Initialise(connection);
        }

        /// <summary>
        /// 将指定实体类实例保存到SQLite数据库
        /// </summary>
        /// <returns></returns>
        public static long CreateOrUpdate<T>(T instance)
        {
            if (instance != null)
            {
                using (SqliteORM.TableAdapter<T> adapter = SqliteORM.TableAdapter<T>.Open())
                {
                    return adapter.CreateUpdate(instance);
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 将指定实体类实例从SQLite数据库中删除
        /// </summary>
        /// <returns></returns>
        public static bool Delete<T>(T instance)
        {
            if (instance != null)
            {
                using (SqliteORM.TableAdapter<T> adapter = SqliteORM.TableAdapter<T>.Open())
                {
                    adapter.Delete(instance);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DeleteAll<T>()
        {
            using (SqliteORM.TableAdapter<T> adapter = SqliteORM.TableAdapter<T>.Open())
            {
                adapter.DeleteAll();
            }
        }

        public static void Delete<T>(SqliteORM.Where s)
        {
            using (SqliteORM.TableAdapter<T> adapter = SqliteORM.TableAdapter<T>.Open())
            {
                adapter.Delete(s);
            }
        }

        /// <summary>
        /// 查询指定表数据
        /// </summary>
        /// <returns></returns>
        public static SqliteORM.Query<T> Select<T>()
        {
            using (SqliteORM.TableAdapter<T> adapter = SqliteORM.TableAdapter<T>.Open())
            {
                return adapter.Select();
            }
        }

        public static void RunSQL(string sql)
        {
            using (System.Data.SQLite.SQLiteConnection cn = new System.Data.SQLite.SQLiteConnection(connection))
            {
                cn.Open();
                System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(sql, cn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
