using CreateCode.Action;
using CreateCode.Model;
using System;

using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CreateCode
{
    public partial class MainFrom : Form
    {
        public string DataBaseNames { get; set; }
        public string calog { get; set; }
        public string ExceMessage { get; set; }
        private string a;
        private SqlConnection con;
        private List<string> list = new List<string>();
        private List<string> lists = LogService.PrimaryKeyList();
        public string connection { get; set; }
        public string A { get => a; set => a = value; }
        private LoginEntity _logentity;
        private string ServiceType = string.Empty;

        public MainFrom(LoginEntity entity)
        {
            this._logentity = entity;
            InitializeComponent();
        }

        private Point mPoint = new Point();

        private void MainFrom_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void MainFrom_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
                //判断当前窗口状态
                if (this.WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }
            }
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrom_Load(object sender, EventArgs e)
        {
            this.textBox1.Enabled = false;
            this.radioButton2.Select();
            GetMenu(_logentity.Type);
        }

        #region[加载菜单]

        public void GetMenu(string type)
        {
            try
            {
                string username = "";
                string pwd = "";
                string databasename = _logentity.HostAddress;
                DataBaseNames = databasename;
                if (type.Contains("Wind"))
                {
                    con = new SqlConnection();
                    con.ConnectionString = "Data Source=" + databasename + ";Initial Catalog=master;Integrated Security=True";
                    con.Open();
                }
                else
                {
                    calog = databasename;
                    //获取登录名
                    username = this._logentity.UserName;
                    //获取登录密码
                    pwd = this._logentity.UserPwd;
                    con = new SqlConnection();
                    con.ConnectionString = string.Format("server=" + databasename + ";database={0};uid=" + username + ";pwd=" + pwd + ";Enlist=true", "master");
                    con.Open();
                }
                SqlDataAdapter adapter = new SqlDataAdapter("select name from master..sysdatabases", con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                #region[创建树形图]
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.treeView1.Nodes.Add(dt.Rows[i]["name"].ToString());
                    list.Add(dt.Rows[i]["name"].ToString());
                }
                //根据父节点加载该父节下面的子节点
                int j = 0;
                foreach (TreeNode item in treeView1.Nodes)
                {
                    TreeNode tn = treeView1.Nodes[j];
                    if (type.Contains("Wind"))
                    {
                        connection = "Data Source=" + databasename + ";Initial Catalog=" + tn.Text + ";Integrated Security=True";
                    }
                    else
                    {
                        connection = "Data Source=" + databasename + ";Initial Catalog=" + tn.Text + ";User ID=" + username + ";Password=" + pwd + "";
                    }

                    con = new SqlConnection(connection);
                    con.Open();
                    string sql = "select table_name from information_schema.tables GROUP BY table_name";
                    SqlDataAdapter da = new SqlDataAdapter(sql, con);
                    dt = new DataTable();
                    da.Fill(dt);
                    int count = dt.Rows.Count;
                    foreach (DataRow row in dt.Rows)
                    {
                        tn.Nodes.Add(row["table_name"].ToString());
                    }
                    j++;
                }
                #endregion
            }
            catch (Exception ex)
            {
                CodeLibrary.Log.LogHelper.WriteToLog($"连接服务器失败:{ex.Message}");
                MessageBox.Show(ex.ToString());

                con.Close();
            }
        }

        #endregion

        #region[格式化数据类型]

        public string GetType(string type)
        {
            string temp = "";
            switch (type)
            {
                case "nvarchar":
                    temp = "string";
                    break;

                case "varchar":
                    temp = "string";
                    break;

                case "datetime":
                    temp = "DateTime";
                    break;

                case "int":
                    temp = "int";
                    break;

                case "decimal":
                    temp = "decimal";
                    break;

                case "bit":
                    temp = "bool";
                    break;

                case "uniqueidentifier":
                    temp = "Guid";
                    break;

                case "float":
                    temp = "double";
                    break;

                case "bigint":
                    temp = "int";
                    break;

                case "image":
                    temp = "byte[]";
                    break;

                case "money":
                    temp = "decimal";
                    break;

                case "tinyint":
                    temp = "byte";
                    break;

                case "numeric":
                    temp = "decimal";
                    break;

                case "ntext":
                    temp = "string";
                    break;
            }
            return temp;
        }

        #endregion

        #region [数据类型转换]

        public string ConverType(string type, string colume)
        {
            string temp = "";
            switch (type)
            {
                case "nvarchar":
                    temp = ".ToString()";
                    break;

                case "varchar":
                    temp = ".ToString()";
                    break;

                case "datetime":
                    temp = "Convert.ToDateTime()";
                    break;

                case "int":
                    temp = "int";
                    break;

                case "decimal":
                    temp = "decimal";
                    break;

                case "bit":
                    temp = "bool";
                    break;

                case "uniqueidentifier":
                    temp = "Guid";
                    break;

                case "float":
                    temp = "double";
                    break;

                case "bigint":
                    temp = "int";
                    break;

                case "image":
                    temp = "byte[]";
                    break;

                case "money":
                    temp = "decimal";
                    break;

                case "tinyint":
                    temp = "byte";
                    break;

                case "numeric":
                    temp = "decimal";
                    break;

                case "ntext":
                    temp = "string";
                    break;
            }
            return temp;
        }

        #endregion

        #region[选择数据表生成模型]

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!list.Contains(e.Node.Text))
            {
                // this.comboBox3.Text = e.Node.Text;
                this.textBox1.Text = e.Node.Text;
                //获取父节点
                string parent = e.Node.Parent.Text;
                GetMemue(_logentity.UserName, this._logentity.UserPwd, this._logentity.Type, parent);
            }
        }

        #endregion

        #region[登录配置]

        public void GetMemue(string username, string pwd, string logtye, string database)
        {
            if (logtye.Contains("Wind"))
            {
                connection = "Data Source=" + calog + ";Initial Catalog=" + database + ";Integrated Security=True";
            }
            else
            {
                connection = "Data Source=" + calog + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + pwd + "";
            }
        }

        #endregion

        #region[字体查找]

        public void changeStrColorFont(RichTextBox rtBox, string str, Color color)
        {
            int pos = 0;
            while (true)
            {
                pos = rtBox.Find(str, pos, RichTextBoxFinds.WholeWord);
                if (pos == -1)
                    break;
                rtBox.SelectionStart = pos;
                rtBox.SelectionLength = str.Length;
                rtBox.SelectionColor = color;

                pos = pos + 1;
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            ServiceType = "实体";
            string type = "2";
            if (this.radioButton1.Checked)
            {
                type = "1";
            }
            this.richTextBox1.Clear();
            string tablename = this.textBox1.Text;
            List<string> zdlist = new List<string>();
            if (tablename != "")
            {
                con = new SqlConnection(connection);
                con.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("using System;\r\n");
                sb.Append("using System.Collections.Generic;\r\n");
                sb.Append("using System.Linq;\r\n");
                sb.Append("using System.Text;\r\n");
                sb.Append("using System.Threading.Tasks;\r\n");
                sb.Append("namespace \r\n");
                sb.Append("{\r\n");
                sb.Append("   public class " + tablename + "\r\n");
                sb.Append("   {\r\n");
                //加载该表对应的所有字段信息

                //  string sql = sql = "select column_name,data_type from information_schema.columns where table_name = '" + tablename + "' ";
                string sql = string.Format("SELECT A.name AS table_name,  B.name AS column_name, C.value AS column_description, d.DATA_TYPE as colum_type  FROM    sys.tables  A INNER JOIN sys.columns B ON B.object_id = A.object_id LEFT JOIN sys.extended_properties C ON C.major_id = B.object_id AND C.minor_id = B.column_id   join information_schema.columns d   on b.name = d.COLUMN_NAME and a.name = d.TABLE_NAME    WHERE A.name = '{0}' ", tablename);
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);//所有字段信息
                foreach (DataRow row in dt.Rows)
                {
                    string columname = string.Empty;
                    string typename = string.Empty;
                    string AttbuiteName = string.Empty;
                    string descname = string.Empty;
                    if (row["column_name"] != DBNull.Value)
                    {
                        columname = "_" + row["column_name"].ToString();
                    }
                    if (row["colum_type"] != DBNull.Value)
                    {
                        typename = GetType(row["colum_type"].ToString());
                    }
                    if (row["column_name"] != DBNull.Value)
                    {
                        AttbuiteName = row["column_name"].ToString();
                    }
                    ///
                    if (row["column_description"] != DBNull.Value)
                    {
                        descname = row["column_description"].ToString();
                    }
                    sb.Append("          /// <summary>\r\n");
                    sb.Append($"         /// {descname}                     \r\n");
                    sb.Append("         /// </summary>\r\n");
                    if (type == "2")
                    {
                        sb.Append("         public " + typename + " " + AttbuiteName + " { get; set; } \r\n");
                    }
                    else
                    {
                        sb.Append($"          public  {typename}   {columname}\r\n");
                        sb.Append($"          public  {typename}   {AttbuiteName}\r\n");
                        sb.Append("          {\r\n");
                        sb.Append("              get { return  " + columname + " ; } \r\n");
                        sb.Append("              set {  " + columname + " = value; } \r\n");
                        sb.Append("          }\r\n");
                    }
                }

                sb.Append("   }\r\n");
                sb.Append("}\r\n");
                this.richTextBox1.Text = sb.ToString();
                con.Close();
                int j = 0;  //定义变量进行查找
                int i = richTextBox1.Find("using");
                int k = richTextBox1.Text.Length;
                richTextBox1.SelectionStart = i;
                richTextBox1.SelectionLength = list[0].Length;

                richTextBox1.SelectionColor = Color.Blue;
                foreach (var item in lists)
                {
                    changeStrColorFont(richTextBox1, item, Color.Blue);
                }
            }
            else
            {
                MessageBox.Show("请选择一张数据表");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// 生成数据访问层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            ServiceType = "数据层";
            string tablename = this.textBox1.Text;
            con = new SqlConnection(connection);
            con.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;\r\n");
            sb.Append("using System.Collections.Generic;\r\n");
            sb.Append("using System.Linq;\r\n");
            sb.Append("using System.Text;\r\n");
            sb.Append("using System.Threading.Tasks;\r\n");
            sb.Append("namespace \r\n");
            sb.Append("{\r\n");
            string sql = string.Format("SELECT A.name AS table_name,  B.name AS column_name, C.value AS column_description, d.DATA_TYPE as colum_type  FROM    sys.tables  A INNER JOIN sys.columns B ON B.object_id = A.object_id LEFT JOIN sys.extended_properties C ON C.major_id = B.object_id AND C.minor_id = B.column_id   join information_schema.columns d   on b.name = d.COLUMN_NAME and a.name = d.TABLE_NAME    WHERE A.name = '{0}' ", tablename);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            da.Fill(dt);//所有字段信息
            sb.Append("   public class " + tablename + "\r\n");
            sb.Append("   {\r\n");
            sb.Append("      public List<" + tablename + "> GetList()\r\n");
            sb.Append("      {\r\n");
            string sqlwhere = string.Empty;
            string sqlparms = string.Empty;
            string eachwhere = "    ";
            string parmsdata = string.Empty;
            string cxsql = string.Empty;
            string updatesql = string.Empty;
            int i = -1;
            foreach (DataRow row in dt.Rows)
            {
                i++;
                string columname = string.Empty;
                string typename = string.Empty;
                if (row["column_name"] != DBNull.Value)
                {
                    columname = row["column_name"].ToString();
                }
                if (row["colum_type"] != DBNull.Value)
                {
                    typename = row["colum_type"].ToString();
                }
                eachwhere += "       if (row[\"" + columname + "\"]!=DBNull.Value) \r\n";
                switch (typename)
                {
                    case "nvarchar":
                        eachwhere += "     info." + columname + "=row[\"" + columname + "\"].ToString();\r\n";
                        break;

                    case "int":
                        eachwhere += "     info." + columname + "=Convert.ToInt32(row[\"" + columname + "\"]);\r\n";
                        break;

                    case "datetime":
                        eachwhere += "     info." + columname + "=Convert.ToDateTime(row[\"" + columname + "\"]);\r\n";
                        break;

                    case "decimal":
                        eachwhere += "       info." + columname + "=Convert.ToDecimal(row[\"" + columname + "\"]);\r\n";
                        break;
                }
                cxsql += columname + ",";
                #region [插入语句]
                if (columname.Contains("Id"))
                {
                    i = i - 1;
                }
                else
                {
                    sqlwhere += columname + ",";
                    sqlparms += "'{" + i + "}',";
                    parmsdata += "info." + columname + ",";
                    updatesql += "" + columname + "='{" + i + "}',";
                }

                #endregion
            }

            #region [查询]
            sqlwhere = sqlwhere.Substring(0, sqlwhere.Length - 1);
            sqlparms = sqlparms.Substring(0, sqlparms.Length - 1);
            parmsdata = parmsdata.Substring(0, parmsdata.Length - 1);
            sb.Append("             string sql=\"SELECT " + cxsql.Substring(0, cxsql.Length - 1) + " FROM " + tablename + "\";\r\n");
            sb.Append("            DataTable dt=SqlHelper.GetDataTable(sql);\r\n");
            sb.Append("             List<" + tablename + "> list = new List<" + tablename + ">();\r\n");
            sb.Append("             if(dt!=null&&dt.Rows.Count>0)\r\n");
            sb.Append("             {\r\n");
            sb.Append("                  foreach(DataRow row in dt.Rows)\r\n");
            sb.Append("                  {\r\n");
            sb.Append("                        " + tablename + "     info=new  " + tablename + "();\r\n");
            sb.Append("               " + eachwhere + "");
            sb.Append("                        list.Add(info);\r\n");
            sb.Append("                  }\r\n");
            sb.Append("             }\r\n");
            sb.Append("               return list;\r\n");
            sb.Append("         }\r\n");
            #endregion

            #region [新增]
            sb.Append("    public int Insert(" + tablename + " info)\r\n");
            sb.Append("    {\r\n");
            sb.Append("            string sql= string.Format(\"INSERT INTO " + tablename + "(" + sqlwhere + ")" +
                "VALUES(" + sqlparms + ")\"," + parmsdata + ");\r\n");
            sb.Append("           return SqlHelper.ExecNonQuery(sql);\r\n");
            sb.Append("    }\r\n");
            #endregion

            #region [修改]
            updatesql = updatesql.Substring(0, updatesql.Length - 1);
            updatesql += " WHERE Id='{" + (i + 1) + "}'\",";
            string updataparms = parmsdata += ",info.Id";
            sb.Append("    public int Update(" + tablename + " info)\r\n");
            sb.Append("    {\r\n");
            sb.Append("            string sql= string.Format(\"UPDATE " + tablename + " SET " + updatesql + "" + updataparms + ");\r\n");
            sb.Append("           return SqlHelper.ExecNonQuery(sql);\r\n");
            sb.Append("    }\r\n");
            #endregion

            #region [删除]
            sb.Append("    public int Delete(string Id)\r\n");
            sb.Append("    {\r\n");
            sb.Append("            string sql= string.Format(\"DELETE " + tablename + " WHERE Id='{0}'\",Id);\r\n");
            sb.Append("           return SqlHelper.ExecNonQuery(sql);\r\n");
            sb.Append("    }\r\n");
            #endregion

            #region [查询]
            sb.Append("    public " + tablename + " GetModel(DataRow row)\r\n");
            sb.Append("    {\r\n");
            sb.Append("                        " + tablename + " info=new  " + tablename + "();\r\n");
            sb.Append("                          " + eachwhere + "");
            sb.Append("        return info;\r\n");
            sb.Append("    }\r\n");
            #endregion

            sb.Append("         }\r\n");
            sb.Append("}\r\n");
            this.richTextBox1.Text = sb.ToString();
            con.Close();
            int j = 0;  //定义变量进行查找
            int d = richTextBox1.Find("using");
            int k = richTextBox1.Text.Length;
            richTextBox1.SelectionStart = d;
            richTextBox1.SelectionLength = list[0].Length;

            richTextBox1.SelectionColor = Color.Blue;
            foreach (var item in lists)
            {
                changeStrColorFont(richTextBox1, item, Color.Blue);
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 参数化数据访问层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            ServiceType = "参数化";
            string tablename = this.textBox1.Text;
            string sql = string.Format("SELECT A.name AS table_name,  B.name AS column_name, C.value AS column_description, d.DATA_TYPE as colum_type  FROM    sys.tables  A INNER JOIN sys.columns B ON B.object_id = A.object_id LEFT JOIN sys.extended_properties C ON C.major_id = B.object_id AND C.minor_id = B.column_id   join information_schema.columns d   on b.name = d.COLUMN_NAME and a.name = d.TABLE_NAME    WHERE A.name = '{0}' ", tablename);
            SqlConnection cons = new SqlConnection(connection);
            SqlDataAdapter da = new SqlDataAdapter(sql, cons);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<TableEntity> list = new List<TableEntity>();
            foreach (DataRow row in dt.Rows)
            {
                TableEntity info = new TableEntity();
                if (row["table_name"] != DBNull.Value)
                    info.table_name = row["table_name"].ToString();
                if (row["column_name"] != DBNull.Value)
                    info.column_name = row["column_name"].ToString();
                if (row["column_description"] != DBNull.Value)
                    info.column_description = row["column_description"].ToString();
                if (row["colum_type"] != DBNull.Value)
                    info.colum_type = row["colum_type"].ToString();
                list.Add(info);
            }
            this.richTextBox1.Clear();
            StringBuilder sb = new StringBuilder();
            //查询字段
            string searchColumnName = string.Empty;
            //新增字段
            string insertColumnName = string.Empty;
            string insertSelectName = string.Empty;
            //修改字段
            string updateIdName = string.Empty;
            string updateParms = string.Empty;
            foreach (TableEntity item in list)
            {
                searchColumnName += item.column_name + ",";
                if (item.column_name == "Id" || item.column_name == "ID" || item.column_name == "id")
                {
                    updateIdName = $" WHERE {item.column_name}=@{item.column_name}";
                    continue;
                }
                else
                {
                    insertColumnName += $"@{item.column_name},";
                    insertSelectName += item.column_name + ",";
                    updateParms += $" {item.column_name}=@{item.column_name},";
                }
            }
            if (string.IsNullOrWhiteSpace(searchColumnName))
            {
                MessageBox.Show("选择数据表");
                return;
            }
            searchColumnName = searchColumnName.Substring(0, searchColumnName.Length - 1);
            sb.Append("using System;\r\n");
            sb.Append("using System.Collections.Generic;\r\n");
            sb.Append("using System.Linq;\r\n");
            sb.Append("using System.Text;\r\n");
            sb.Append("using System.Threading.Tasks;\r\n");
            sb.Append("using System.Data;\r\n");
            sb.Append("using System.Data.SqlClient;\r\n");
            sb.Append("namespace \r\n");
            sb.Append("{\r\n");
            sb.Append($"    public class {tablename}Dal\r\n");
            sb.Append("     {\r\n");
            #region [查询]
            sb.Append($"         public List<{tablename}> GetList()\r\n");
            sb.Append("          {\r\n");
            sb.Append($"              string sql=\"SELECT {searchColumnName} FROM {tablename}\";\r\n");
            sb.Append("               DataTable dt=SqlHelper.GetDataTable(sql,CommandType.Text);\r\n");
            sb.Append($"              List<{tablename}> list=new List<{tablename}>();\r\n");
            sb.Append("               foreach(DataRow row  in dt.Rows)\r\n");
            sb.Append("               {\r\n");
            sb.Append($"                  {tablename} info =new {tablename}();\r\n");
            foreach (TableEntity item in list)
            {
                sb.Append("              if(row[\"" + item.column_name + "\"]!=DBNull.Value)\r\n");
                sb.Append("              {\r\n");
                switch (item.colum_type)
                {
                    case "nvarchar":
                        sb.Append($"        info.{item.column_name}=row[\"{item.column_name}\"].ToString();\r\n");
                        break;

                    case "int":
                        sb.Append($"        info.{item.column_name}=Convert.ToInt32(row[\"{item.column_name}\"]);\r\n");
                        break;

                    case "image":
                        sb.Append($"        info.{item.column_name}=(byte[])row[\"{item.column_name}\"];\r\n");
                        break;

                    case "datetime2":
                        sb.Append($"        info.{item.column_name}=Convert.ToDateTime(row[\"{item.column_name}\"]);\r\n");
                        break;

                    case "datetime":
                        sb.Append($"        info.{item.column_name}=Convert.ToDateTime(row[\"{item.column_name}\"]);\r\n");
                        break;

                    case "decimal":
                        sb.Append($"        info.{item.column_name}=Convert.ToDecimal(row[\"{item.column_name}\"]);\r\n");
                        break;

                    case "float":
                        sb.Append($"        info.{item.column_name}=Convert.ToSingle(row[\"{item.column_name}\"]);\r\n");
                        break;
                }
                sb.Append("               }\r\n");
            }
            sb.Append("                   list.Add(info);\r\n ");
            sb.Append("               }\r\n");
            sb.Append("               return list;\r\n");
            sb.Append("          }\r\n");
            #endregion

            #region [新增]
            insertColumnName = insertColumnName.Substring(0, insertColumnName.Length - 1);
            insertSelectName = insertSelectName.Substring(0, insertSelectName.Length - 1);
            sb.Append("\r\n");
            sb.Append($"        public int Insert({tablename} info)\r\n");
            sb.Append("         {\r\n");
            sb.Append($"            string sql=\"INSERT INTO {tablename}({insertSelectName})VALUES({insertColumnName})\";");
            sb.Append("             SqlParameter[] ps = new SqlParameter[]\r\n");
            sb.Append("             {\r\n");
            foreach (TableEntity item in list)
            {
                if (item.column_name == "Id" || item.column_name == "ID" || item.column_name == "id")
                {
                    continue;
                }
                else
                {
                    sb.Append($"             new SqlParameter(\"@{item.column_name}\",info.{item.column_name}),\r\n");
                }
            }
            sb.Append("             };\r\n");
            sb.Append("             return SqlHelper.ExecuteNonQuery(sql,CommandType.Text,ps);\r\n");
            sb.Append("         }\r\n");
            #endregion

            #region [修改]
            updateParms = updateParms.Substring(0, updateParms.Length - 1);
            updateParms += updateIdName;
            sb.Append("\r\n");
            sb.Append($"         public int Update({tablename} info)\r\n");
            sb.Append("          {\r\n");
            sb.Append($"            string sql=\"UPDATE {tablename} SET {updateParms}\";\r\n");
            sb.Append("             SqlParameter[] ps = new SqlParameter[]\r\n");
            sb.Append("             {\r\n");
            foreach (TableEntity item in list)
            {
                sb.Append($"             new SqlParameter(\"@{item.column_name}\",info.{item.column_name}),\r\n");
            }
            sb.Append("             };\r\n");
            sb.Append("             return SqlHelper.ExecuteNonQuery(sql,CommandType.Text,ps);\r\n");
            sb.Append("          }\r\n");
            #endregion

            #region [删除]
            sb.Append("        public int Delete(string Id)\r\n");
            sb.Append("        {\r\n");
            foreach (TableEntity item in list)
            {
                if (item.column_name == "Id" || item.column_name == "ID" || item.column_name == "id")
                {
                    sb.Append("           string sql=\"DELETE " + tablename + " WHERE " + item.column_name + "=@" + item.column_name + "\";\r\n");
                    sb.Append("           SqlParameter ps = new SqlParameter(\"@" + item.column_name + "\",Id);\r\n");
                    break;
                }
            }
            sb.Append("             return SqlHelper.ExecuteNonQuery(sql,CommandType.Text,ps);\r\n");
            sb.Append("        }\r\n");
            #endregion
            sb.Append("     }\r\n");
            sb.Append("}");
            this.richTextBox1.Text = sb.ToString();
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            TableForm t = new TableForm(_logentity, DataBaseNames, _logentity.Type);
            t.Show();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            GetMenu(_logentity.Type);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "选择保存路径";
            saveFileDialog1.ShowDialog();
            this.textBox2.Text = saveFileDialog1.FileName;
            //写入文件
            using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
            {
                sw.Write(this.richTextBox1.Text);
            }

            // saveFileDialog1.Filter = "*.cs";
        }
    }
}