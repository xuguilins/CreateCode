using CreateCode.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateCode
{
    public partial class TableForm : Form
    {
        /// <summary>
        /// 登录类型
        /// </summary>
        private string type;

        /// <summary>
        /// 服务器名称
        /// </summary>
        private string databasename;

        /// <summary>
        /// 登录信息
        /// </summary>
        private LoginEntity _logentity;

        /// <summary>
        /// 连接器
        /// </summary>
        private SqlConnection con = null;

        /// <summary>
        /// 连接字符串
        /// </summary>
        private string ConnectionInfotion = "";

        public TableForm()
        {
            InitializeComponent();
        }

        public TableForm(LoginEntity loginEntity, string _basename, string _type)
        {
            this._logentity = loginEntity;
            this.databasename = _basename;
            this.type = _type;
            InitializeComponent();
        }

        private Point mPoint = new Point();

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
        /// 窗体加载时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableForm_Load(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            timer1.Interval = 20;

            #region [初始化信息]

            SqlConnection con = null;
            if (type.Contains("Wind"))
            {
                con = new SqlConnection();
                con.ConnectionString = "Data Source=.;Initial Catalog=master;Integrated Security=True";
                con.Open();
            }
            else
            {
                string calog = databasename;
                //获取登录名
                string username = this._logentity.UserName;
                //获取登录密码
                string pwd = this._logentity.UserPwd;
                con = new SqlConnection();
                con.ConnectionString = string.Format("server=" + databasename + ";database={0};uid=" + username + ";pwd=" + pwd + ";Enlist=true", "master");
                con.Open();
            }
            ConnectionInfotion = con.ConnectionString;
            DataTable dt = new DataTable();
            string sql = "SELECT NAME FROM MASTER.DBO.SYSDATABASES ORDER BY NAME";
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                string name = string.Empty;
                foreach (DataRow row in dt.Rows)
                {
                    name = row["NAME"].ToString();
                    comboBox1.Items.Add(name);
                }
            }

            #endregion [初始化信息]
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int maxWidth = this.Width;
            var X = label2.Location.X;
            var Y = label2.Location.Y;
            if (X == -(0 + label2.Width))
            {
                X = maxWidth + 10;
            }
            X--;
            label2.Location = new Point(X, Y);
        }

        /// <summary>
        /// 检查数据表是否存在
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("请选择数据库");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("请输入数据表名");
                return;
            }
            string tablename = textBox2.Text.Trim();
            string dataname = comboBox1.Text.Trim();
            string sql = string.Format("select table_name from information_schema.tables where TABLE_NAME='{0}' GROUP BY table_name", tablename);
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConnectionInfotion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
                {
                    da.Fill(dt);
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                MessageBox.Show($"数据表【{tablename}】已经存在,无法创建重复表");
                return;
            }
            else
            {
                MessageBox.Show("数据表检查成功");
                if (type.Contains("Wind"))
                {
                    ConnectionInfotion = "Data Source=.;Initial Catalog=" + comboBox1.Text + ";Integrated Security=True";
                }
                else
                {
                    string calog = databasename;
                    //获取登录名
                    string username = this._logentity.UserName;
                    //获取登录密码
                    string pwd = this._logentity.UserPwd;
                    ConnectionInfotion = string.Format("server=" + databasename + ";database={0};uid=" + username + ";pwd=" + pwd + ";Enlist=true", comboBox1.Text);
                }
                button1.Enabled = false;
                textBox2.Enabled = false;
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                List<CreateTableEntity> list = new List<CreateTableEntity>();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        CreateTableEntity info = new CreateTableEntity();
                        info.Name = row.Cells[0].Value == null ? "" : row.Cells[0].Value.ToString();
                        info.Type = row.Cells[1].Value == null ? "" : row.Cells[1].Value.ToString();
                        info.ISNULL = row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString();
                        info.ISKEY = row.Cells[3].Value == null ? "" : row.Cells[3].Value.ToString();
                        info.ISINDENTITY = row.Cells[4].Value == null ? "" : row.Cells[4].Value.ToString();
                        info.STARTNUMBER = Convert.ToInt32(row.Cells[5].Value == null ? "1" : row.Cells[5].Value.ToString());
                        list.Add(info);
                    }
                }
                string BaseName = comboBox1.Text.Trim();
                string TableName = textBox2.Text.Trim();
                string createstr = string.Empty;
                StringBuilder sb = new StringBuilder();
                sb.Append($"CREATE TABLE {TableName}");
                sb.Append("(");
                foreach (CreateTableEntity item in list)
                {
                    createstr += $"{ item.Name}{ FormatterValue(item.Type)}{ IsNullValue(item.ISNULL)}{ IsPrimayKeyValue(item.ISKEY)}{ IsIdenttiyValue(item.ISINDENTITY, item.STARTNUMBER, item.Type)},";
                }
                createstr = createstr.Substring(0, createstr.Length - 1);
                sb.Append(createstr);
                sb.Append(")");
                using (SqlConnection con = new SqlConnection(ConnectionInfotion))
                {
                    con.Open();
                    using (SqlCommand com = new SqlCommand(sb.ToString(), con))
                    {
                        com.ExecuteNonQuery();
                        MessageBox.Show($"数据表【{TableName}】创建成功");
                        button1.Enabled = true;
                        textBox2.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"创建数据表异常，{ex.Message}");
            }
        
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
          
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 格式化数据类型
        /// </summary>
        /// <returns></returns>
        private string FormatterValue(string type)
        {
            string result = string.Empty;
            switch (type)
            {
                case "字符串":
                    result = " nvarchar(100) ";
                    break;
                case "整数":
                    result = " int ";
                    break;
                case "金额":
                    result = " decimal(18,2) ";
                    break;
                case "时间":
                    result = " datetime ";
                    break;
                case "GUID":
                    result = " guid ";
                    break;
                case "大文本":
                    result = " text ";
                    break;
                case "小数":
                    result = " float ";
                    break;
            }
            return result;
        }
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string IsNullValue(string type)
        {
            return type == "是" ? " null " : " not null ";
        }
        /// <summary>
        /// 是否为主键
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string IsPrimayKeyValue(string type)
        {
            return type=="是"?" primary key ":"";
        }
        /// <summary>
        /// 是否为自增
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string IsIdenttiyValue(string type,int value,string coltype)
        {
            if (coltype == "整数")
            {
                return type == "是" ? "  identity(" + value + ",1)" : " identity(1,1) ";
            }
            else
            {
                return null;
            }         
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
        }
    }
}