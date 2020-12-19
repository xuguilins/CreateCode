
using CreateCode.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace CreateCode.Services
{
    public class MySqlDbContext : BaseService
    {
        public override string DataBaseName { get ; set; }
        public override string PassWord { get; set; }
        public override string UserId { get; set; }
        private MySqlConnection _MySqlConnection;
        public override ReturnResult ConnectionDb(string connectionString)
        {
            try
            {
                DataBaseName = connectionString.GetStrValue("DATABASE");
                PassWord = connectionString.GetStrValue("PASSWORD");
                UserId = connectionString.GetStrValue("USER");
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                _MySqlConnection = connection;
                return new ReturnResult("数据库连接成功", true);
            }
            catch (Exception ex)
            {

                return new ReturnResult("数据库连接失败", false, ex.Message);
            }
             


        }

        public override List<MenuEntity> InitDbEntity()
        {
            List<MenuEntity> list = new List<MenuEntity>();
           string sql = string.Format("select TABLE_NAME AS TABLENAME,TABLE_SCHEMA AS DbName from information_schema.tables where table_schema = '{0}' and table_type = 'BASE TABLE'; ", DataBaseName);
            var dt = GetDataTable(sql);
            if (dt!=null && dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    MenuEntity info = new MenuEntity();
                    if (row["TABLENAME"] != DBNull.Value)
                        info.TableName = row["TABLENAME"].ToString();
                    if (row["DbName"] != DBNull.Value)
                        info.ParentName = row["DbName"].ToString();
                    list.Add(info);
                }

            }
            return list;

        }

        public override List<TableEntity> InitTableEntity(string tableName)
        {
            List<TableEntity> list = new List<TableEntity>();
            var sql = string.Format("select COLUMN_NAME,DATA_TYPE,TABLE_SCHEMA,TABLE_NAME from information_schema.COLUMNS where table_name = '{0}'; ", tableName);
            var dt = GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    TableEntity info = new TableEntity();
                    if (row["COLUMN_NAME"] != DBNull.Value)
                        info.column_name = row["COLUMN_NAME"].ToString();
                    if (row["DATA_TYPE"] != DBNull.Value)
                        info.colum_type = row["DATA_TYPE"].ToString();
                    info.DbBaseType = DbBaseType.MySql;
                    if (row["TABLE_NAME"] != DBNull.Value)
                        info.table_name = row["TABLE_NAME"].ToString();
                    list.Add(info);
                }

            }
            return list;
        }
        private DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
           using(MySqlCommand com = new MySqlCommand(sql,_MySqlConnection))
            {
                using(MySqlDataAdapter da = new MySqlDataAdapter(com))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }
        public override DbConnection DbConnection { get; set; }
    }
}
