
using CreateCode.Model;
using CreateCode.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateCode
{
    public partial class LoginFrom : Form
    {
        public static DbBaseType DbInstaceType = DbBaseType.SqlServer;
        public static string ConnectionStr = string.Empty;
        public LoginFrom()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 程序运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginFrom_Load(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.label3.Visible = false;
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(m => typeof(BaseService).IsAssignableFrom(m) && !m.IsAbstract && m.IsClass && !m.IsInterface).ToList();
            foreach (Type item in types)
            {
                var value = item.GetProperty("DbBaseType");
                var instaceValue = Activator.CreateInstance(item);
                var propItem = value.GetValue(instaceValue, null);
                this.comboBox2.Items.Add(propItem);
            }


        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
 
            if (string.IsNullOrWhiteSpace(textAddress.Text))
            {
                MessageBox.Show("数据库链接字符串不能为空");
                return;
            }
            if (string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("数据库类型不能为空");
                return;
            }
            this.label3.Visible = true;
            label3.Text = "数据库正在连接中，请稍后...";
            var typeName = this.comboBox2.Text;
            DbInstaceType= CodeExtendsition.GetBaseType(typeName);
            var dbInstance = DbFactory.CreateInstance(DbInstaceType);
            ConnectionStr = textAddress.Text;
            var result =  Task.Factory.StartNew( () => ConnectionDb(dbInstance)); //dbInstance.ConnectionDb(this.textAddress.Text);
            result.Wait();
            var dataRes = result.Result;
            if (dataRes.Success)
            {
                MainFrom me = new MainFrom();
                me.Show();
                this.Hide();
            } else
            {
                this.label3.Text = "数据库连接失败";
                MessageBox.Show(dataRes.Message);
            }
        }
        private  ReturnResult ConnectionDb(BaseService dbInstance)
        {
             var result=  dbInstance.ConnectionDb(this.textAddress.Text);
            return result;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
        
        }
    }
}