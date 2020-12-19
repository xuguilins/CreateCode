using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess;
using Oracle;
using Oracle.ManagedDataAccess.Client;
using CreateCode.Model;
using System.Data;
using System.Data.Common;

namespace CreateCode.Services
{
    public class OracleDbContext : BaseService
    {
 
        public override string DataBaseName { get; set; }
        public override string PassWord { get; set; }
        public override string UserId { get ;  set; }
        private OracleConnection _Connection;
        public override ReturnResult ConnectionDb(string connectionString)
        {
            try
            {
                DataBaseName = connectionString.GetStrValue("USER");
                PassWord = connectionString.GetStrValue("PASSWORD");
                UserId = DataBaseName;
                OracleConnection connection = new OracleConnection(connectionString);
                connection.Open();
                _Connection = connection;
                DbConnection = connection;
                return new ReturnResult("数据库链接成功", true);
            }
            catch (Exception ex)
            {
                return new ReturnResult("数据库链接失败", false, ex.Message);
            }   
        }
        public override List<MenuEntity> InitDbEntity()
        {
            List<MenuEntity> list = new List<MenuEntity>();
            var sql = string.Format("select distinct a.OWNER, a.TABLE_NAME from sys.all_col_comments a,sys.dba_objects b where a.table_name=b.object_name   and b.object_type='TABLE' and a.OWNER='{0}' order by a.OWNER, a.TABLE_NAME", DataBaseName);
            var dt = GetDataTable(sql);
            if (dt!=null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    MenuEntity info = new MenuEntity();
                    if (row["OWNER"] != DBNull.Value)
                        info.ParentName = row["OWNER"].ToString();
                    if (row["TABLE_NAME"] != DBNull.Value)
                        info.TableName = row["TABLE_NAME"].ToString();
                    list.Add(info);
                }
            }
            return list;
        }

        

        private DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            using (OracleCommand cmd = new OracleCommand(sql, _Connection))
            { 
                using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                {
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public override List<TableEntity> InitTableEntity(string tableName)
        {
            var sql = string.Format(" select COLUMN_NAME,DATA_TYPE,DATA_LENGTH from   dba_tab_columns where  table_name =upper('{0}') order by COLUMN_NAME", tableName);
            var dt = GetDataTable(sql);
            List<TableEntity> list = new List<TableEntity>();
            if (dt!=null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    TableEntity info = new TableEntity();
                    if (row["COLUMN_NAME"] != DBNull.Value)
                        info.column_name = row["COLUMN_NAME"].ToString() ;
                    info.table_name = tableName;
                    if (row["DATA_TYPE"] != DBNull.Value)
                        info.colum_type = row["DATA_TYPE"].ToString();
                    if (row["DATA_LENGTH"] != DBNull.Value)
                        info.column_length = row["DATA_LENGTH"].ToString();
                    info.DbBaseType = DbBaseType.Oracle;
                    list.Add(info);
                }
            }
            return list;
        }

        public override DbConnection DbConnection { get; set; }
        public override DbBaseType DbBaseType => DbBaseType.Oracle;
    }
}
