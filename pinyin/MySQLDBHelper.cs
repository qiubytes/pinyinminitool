using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class MySQLDBHelper
    {
        public readonly static string MySqlConn = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString.ToString();


        /// <summary>
        /// 执行增、删、改的方法：ExecuteNonQuery,返回true,false
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string sql, MySqlParameter[] pms)
        {
            using (MySqlConnection conn = new MySqlConnection(MySqlConn))
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        if (pms != null && pms.Length > 0)
                        {
                            cmd.Parameters.AddRange(pms);
                        }
                        int rows = cmd.ExecuteNonQuery();
                        transaction.Commit();
                        return rows > 0;
                    }
                }
            }
        }

        /// <summary>
        ///  执行增、删、改的方法：ExecuteNonQuery,返回true,false
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string sql, Dictionary<string, object> pms)
        {
            MySqlParameter[] parameters = null;

            if (pms != null && pms.Count > 0)
            {
                parameters = DictionaryToMySqlParameters(pms).ToArray();
            }

            return ExecuteNonQuery(sql, parameters);
        }

        /// <summary>
        ///  执行增、删、改的方法：ExecuteNonQuery,返回true,false
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, new MySqlParameter[] { });
        }

        /// <summary>
        /// 将查出的数据装到实体里面，返回一个List
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static List<T> ExecuteQuery<T>(string sql, MySqlParameter[] pms) where T : new()
        {
            using (var connection = new MySqlConnection(MySqlConn))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    if (pms != null && pms.Length > 0)
                    {
                        command.Parameters.AddRange(pms);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        List<T> tList = new List<T>();

                        while (reader.Read()) // 遍历结果集中的每一行数据  
                        {
                            var t = ConvertToModel<T>(reader);
                            tList.Add(t);
                        }
                        return tList;
                    }
                }
            }
        }

        /// <summary>
        /// 将查出的数据装到实体里面，返回一个List
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static List<T> ExecuteQuery<T>(string sql, Dictionary<string, object> pms) where T : new()
        {
            MySqlParameter[] parameters = null;

            if (pms != null)
            {
                parameters = DictionaryToMySqlParameters(pms).ToArray();
            }

            return ExecuteQuery<T>(sql, parameters);
        }


        /// <summary>
        /// 将查出的数据装到实体里面，返回一个List
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> ExecuteQuery<T>(string sql) where T : new()
        {
            return ExecuteQuery<T>(sql, new MySqlParameter[] { });
        }

        /// <summary>
        /// 将查出的数据装到实体里面，返回一个实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static T ExecuteQueryOne<T>(string sql, MySqlParameter[] pms) where T : new()
        {
            using (var connection = new MySqlConnection(MySqlConn))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    if (pms != null && pms.Length > 0)
                    {
                        command.Parameters.AddRange(pms);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) // 遍历结果集中的每一行数据  
                        {

                            var t = ConvertToModel<T>(reader);

                            return t;
                        }

                        return default(T);
                    }
                }
            }
        }

        /// <summary>
        /// 将查出的数据装到实体里面，返回一个实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static T ExecuteQueryOne<T>(string sql, Dictionary<string, object> pms) where T : new()
        {
            MySqlParameter[] parameters = null;

            if (pms != null && parameters.Length > 0)
            {
                parameters = DictionaryToMySqlParameters(pms).ToArray();
            }

            return ExecuteQueryOne<T>(sql, parameters);
        }

        /// <summary>
        /// 将查出的数据装到实体里面，返回一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static T ExecuteQueryOne<T>(string sql) where T : new()
        {
            return ExecuteQueryOne<T>(sql, new MySqlParameter[] { });
        }


        /// <summary>
        /// 将查出的数据装到table里，返回一个DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryDataTable(string sql, MySqlParameter[] pms = null)
        {
            DataTable dt = new DataTable();

            using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, MySqlConn))
            {
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 将查出的数据装到table里，返回一个DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryDataTable(string sql, Dictionary<string, object> pms)
        {
            MySqlParameter[] parameters = null;

            if (pms != null)
            {
                parameters = DictionaryToMySqlParameters(pms).ToArray();
            }

            return ExecuteQueryDataTable(sql, parameters);
        }

        /// <summary>
        /// 将查出的数据装到table里，返回一个DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryDataTable(string sql)
        {
            MySqlParameter[] parameters = null;

            return ExecuteQueryDataTable(sql, parameters);
        }

        /// <summary>
        /// 字典转MySqlParameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<MySqlParameter> DictionaryToMySqlParameters(Dictionary<string, object> parameters)
        {
            List<MySqlParameter> MySqlParameters = new List<MySqlParameter>();

            foreach (var kvp in parameters)
            {
                string parameterName = kvp.Key;
                object parameterValue = kvp.Value;

                // 创建 MySqlParameter 对象  
                MySqlParameter MySqlParameter = new MySqlParameter(parameterName, parameterValue);
                MySqlParameters.Add(MySqlParameter);
            }
            return MySqlParameters;
        }

        /// <summary>
        /// 按列名转换(单条使用比较方便)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T ConvertToModel<T>(MySqlDataReader reader) where T : new()
        {
            T t = new T();

            PropertyInfo[] propertys = t.GetType().GetProperties();

            List<string> drColumnNames = new List<string>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                drColumnNames.Add(reader.GetName(i));
            }

            foreach (PropertyInfo pi in propertys)
            {
                if (drColumnNames.Contains(pi.Name))
                {
                    if (!pi.CanWrite)
                    {
                        continue;
                    }
                    var value = reader[pi.Name];
                    if (value != DBNull.Value)
                    {
                        pi.SetValue(t, value, null);
                    }
                }
            }
            return t;
        }
    }
}
