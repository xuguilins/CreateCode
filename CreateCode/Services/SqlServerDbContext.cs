
using CreateCode.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace CreateCode.Services
{
    public class SqlServerDbContext : BaseService
    {
        public override string DataBaseName { get; set; }
        public override string PassWord { get; set; }
        public override string UserId { get; set; }
        private SqlConnection _SqlConnection;
        public override ReturnResult ConnectionDb(string connectionString)
        {
            try
            {
                UserId = connectionString.GetStrValue("USER ID");
                DataBaseName = connectionString.GetStrValue("CATALOG");
                PassWord = connectionString.GetStrValue("PASSWORD");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                _SqlConnection = connection;
                return new ReturnResult("数据库连接成功", true);
            }
            catch (Exception ex)
            {
                return new ReturnResult("数据库连接失败", false,ex.Message);
            }

        }

        public override List<MenuEntity> InitDbEntity()
        {
            List<MenuEntity> list = new List<MenuEntity>();
            string sql = string.Format("select name from {0}..sysobjects where xtype='U'", DataBaseName);
            var table = GetDataTable(sql);
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    MenuEntity info = new MenuEntity();
                    if (row["name"] != DBNull.Value)
                        info.TableName = row["name"].ToString();
                    info.ParentName = DataBaseName;
                    list.Add(info);
                }
            }
            return list;
        }

        public override List<TableEntity> InitTableEntity(string tableName)
        {
            List<TableEntity> list = new List<TableEntity>();
            var sql = string.Format("SELECT A.name AS table_name,  B.name AS column_name, C.value AS column_description, d.DATA_TYPE as colum_type  FROM    sys.tables  A INNER JOIN sys.columns B ON B.object_id = A.object_id LEFT JOIN sys.extended_properties C ON C.major_id = B.object_id AND C.minor_id = B.column_id   join information_schema.columns d   on b.name = d.COLUMN_NAME and a.name = d.TABLE_NAME  WHERE A.name = '{0}'", tableName);
            var table = GetDataTable(sql);
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    TableEntity info = new TableEntity();
                    if (row["table_name"] != DBNull.Value)
                        info.table_name = row["table_name"].ToString();
                    if (row["column_name"] != DBNull.Value)
                        info.column_name = row["column_name"].ToString();
                    if (row["colum_type"] != DBNull.Value)
                        info.colum_type = row["colum_type"].ToString();
                    info.DbBaseType = DbBaseType.SqlServer;
                    list.Add(info);
                }
            }
            return list;

        }

        public override DbConnection DbConnection { get; set; }
        private DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            using(SqlCommand com = new SqlCommand(sql, _SqlConnection))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(com))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}
