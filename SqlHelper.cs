using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Hotel.Common;

namespace Hotel.Common
{
    /// <summary>
    ///    数据库帮助类
    /// </summary>
    public class SqlHelper 
    {
        private static string connection;
        /// 执行数据库的增改操作
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">语句类型,SQLTEXT或存储过程</param>
        /// <param name="ps">参数数组</param>
        /// <returns>业务操作结果</returns>
        public static int ExecuteNonQuery(string sql, CommandType type, params SqlParameter[] ps)
        {
            connection = AppSetConfig.GetConnection();
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(sql, con))
                {
                    com.CommandType = type;
                    com.Parameters.AddRange(ps);

                    return com.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 获取数据表Table
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">语句类型,SQLTEXT或存储过程</param>
        /// <param name="ps">参数数组</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string sql, CommandType type, params SqlParameter[] ps)
        {
            connection = AppSetConfig.GetConnection();
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
                {
                    DataTable dt = new DataTable();
                    da.SelectCommand.CommandType = type;
                    da.SelectCommand.Parameters.AddRange(ps);
                    da.Fill(dt);
                    return dt;
                }
            }
        }
        /// <summary>
        /// 仅支持全字段写入且实体名称必须和表名一致
        /// </summary>
        /// <typeparam name="T">实体模型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">语句类型</param>
        /// <param name="Model">实体类型</param>
        /// <returns></returns>
        public static int Insert<T>(T Model) where T : class, new()
        {
            connection = AppSetConfig.GetConnection();
            ///获取实体的属性
            var list = Model.GetType().GetProperties();
            //获取实体名
            var tableName = Model.GetType().Name;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO "+tableName+"");
            string tableparms = string.Empty;
            foreach (var item in list)
            {
                tableparms += ""+item.Name+",";
            }
            tableparms = tableparms.Substring(0, tableparms.Length - 1);
            sb.Append("("+tableparms+")");
            string tableValue = string.Empty;
            foreach (var item in list)
            {
                tableValue += "'"+item.GetValue(Model)+"',";
            }
            tableValue = tableValue.Substring(0, tableValue.Length - 1);
            sb.Append("VALUES("+tableValue+")");
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(sb.ToString(), con))
                {
                    return com.ExecuteNonQuery();
                }
            }
        }
    }
}