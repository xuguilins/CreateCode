using CreateCode.Action;
using CreateCode.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateCode
{
    public partial class LoginFrom : Form
    {
        public LoginFrom()
        {
            // this.timer1.Enabled = false;
            InitializeComponent();
        }

        /// <summary>
        /// 选择登陆方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = this.comboBox2.Text;
            switch (type)
            {
                case "Windows身份认证":
                    this.textUserName.Enabled = false;
                    this.textPwd.Enabled = false;
                    break;

                case "SQL Server身份认证":
                    this.textUserName.Enabled = true;
                    this.textPwd.Enabled = true;
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 程序运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginFrom_Load(object sender, EventArgs e)
        {
            this.comboBox2.Items.Add("Windows身份认证");
            this.comboBox2.Items.Add("SQL Server身份认证");
            this.timer1.Enabled = false;
            this.label6.Visible = false;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textAddress.Text.Trim()))
            {
                MessageBox.Show("服务器地址不能为空");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.comboBox2.Text.Trim()))
            {
                MessageBox.Show("身份验证方式不能为空！！");
                return;
            }
            string type = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.comboBox2.Text.Trim()))
            {
                type = this.comboBox2.Text;
                if (!type.Contains("Wind"))
                {
                    if (string.IsNullOrWhiteSpace(this.textUserName.Text.Trim()))
                    {
                        MessageBox.Show("用户名不能为空");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(this.textPwd.Text.Trim()))
                    {
                        MessageBox.Show("用户密码不能为空");
                        return;
                    }
                }
            }

            #region [连接状态检查]

            //检查服务器的连接状态
            type = this.comboBox2.Text;
            string connection = string.Empty;
            string hostdata = this.textAddress.Text.Trim();
            try
            {
                if (type.Contains("Windows"))
                {
                    connection = "Data Source=" + hostdata + ";Initial Catalog=master;Integrated Security=True";
                }
                else
                {
                    connection = string.Format("server=" + hostdata + ";database={0};uid=" + this.textUserName.Text.Trim() + ";pwd=" + this.textPwd.Text.Trim() + ";Enlist=true", "master");
                }
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();
                    LoginEntity info = new LoginEntity();
                    info.UserName = this.textUserName.Text.Trim();
                    info.UserPwd = this.textPwd.Text.Trim();
                    info.Type = this.comboBox2.Text;
                    info.HostAddress = this.textAddress.Text;
                    MainFrom form = new MainFrom(info);
                    form.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                CodeLibrary.Log.LogHelper.WriteToLog($"连接服务器失败，失败原因:{ex.Message}");
                this.label6.Text = "服务器连接失败，程序停止...";
                return;
            }

            #endregion [连接状态检查]
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}