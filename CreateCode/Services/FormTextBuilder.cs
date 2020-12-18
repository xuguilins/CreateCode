using CreateCode.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Services
{
    public static class FormTextBuilder
    {
    
        public  static string Builder(List<TableEntity> list,string tableName, string type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;\r\n");
            sb.Append("using System.Collections.Generic;\r\n");
            sb.Append("using System.Linq;\r\n");
            sb.Append("using System.Text;\r\n");
            sb.Append("using System.Threading.Tasks;\r\n");
            sb.Append("namespace \r\n");
            sb.Append("{\r\n");
            sb.Append("   public class " + tableName + "\r\n");
            sb.Append("   {\r\n");
            foreach (var item in list)
            {
                string typename = ParseType(item.colum_type, item.DbBaseType);
                sb.Append("          /// <summary>\r\n");
                sb.Append($"         ///                      \r\n");
                sb.Append("         /// </summary>\r\n");
                if (type == "2")
                {
                    sb.Append("         public " + typename + " " + item.column_name + " { get; set; } \r\n");
                }
                else
                {
                    var name = item.column_name.ToLower();
                    sb.Append($"          public  {typename}   _{name}\r\n");
                    sb.Append($"          public  {typename}   {item.column_name}\r\n");
                    sb.Append("          {\r\n");
                    sb.Append("              get { return  _" + name + " ; } \r\n");
                    sb.Append("              set {  _" + name + " = value; } \r\n");
                    sb.Append("          }\r\n");
                }
            }
            sb.Append("   }\r\n");
            sb.Append("}\r\n");
            return sb.ToString();
        }
        private static string ParseType(string typename,DbBaseType type)
        {
            string res = string.Empty;
            switch(type)
            {
                case DbBaseType.Oracle:
                    res = OracleParseType(typename);
                    break;
                case DbBaseType.SqlServer:
                    res = SQLServerParseType(typename);
                    break;
            }
            return res;
        }
        private static string OracleParseType(string typeName)
        {
            string res = string.Empty;
            switch(typeName.ToUpper())
            {
                case "VARCHAR2":
                    res = "string";
                    break;
                case "VARCHAR":
                    res = "string";
                    break;
                case "NUMBER":
                    res = "int";
                    break;
                case "CHAR":
                    res = "string";
                    break;
                case "NCHAR":
                    res = "string";
                    break;
                case "NCLOB":
                    res = "string";
                    break;
                case "LONG":
                    res = "string";    
                    break;
                case "FLOAT":
                    res = "decimal";
                    break;
                case "INTEGER":
                    res = "decimal";
                    break;
                case "TIMESTAMP":
                    res = "DateTime";
                    break;
                case "DATE":
                    res = "DateTime";
                    break;
            }
            return res;

        }
        private static string SQLServerParseType(string typeName)
        {
            string res = string.Empty;
            switch (typeName.ToLower())
            {
                case "varbinary":
                    res = "byte[]";
                    break;
                case "varchar":
                    res = "string";
                    break;
                case "nvarchar":
                    res = "string";
                    break;
                case "uniqueidentifier":
                    res = "Guid";
                    break;
                case "int":
                    res = "int";
                    break;
                case "nchar":
                    res = "string";
                    break;
                case "text":
                    res = "string";
                    break;
                case "ntext":
                    res = "string";
                    break;
                case "float":
                    res = "double";
                    break;
                case "LONG":
                    res = "string";
                    break;
                case "char":
                    res = "string";
                    break;
                case "decimal":
                    res = "decimal";
                    break;
                case "timestamp":
                    res = "byte[]";
                    break;
                case "datetime":
                    res = "DateTime";
                    break;
                case "datetime2":
                    res = "DateTime";
                    break;
            }
            return res;

        }
    }
}
