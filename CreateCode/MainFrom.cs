
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
using CreateCode.Services;

namespace CreateCode
{
    public partial class MainFrom : Form
    {
        public BaseService dbService;
        private Point mPoint = new Point();
        public MainFrom()
        {
            InitializeComponent();
        }

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
            var connetion = LoginFrom.ConnectionStr;
            var type = LoginFrom.DbInstaceType;
            var dbInstace = DbFactory.CreateInstance(type);
            dbInstace.ConnectionDb(connetion);
            dbService = dbInstace;
            var list= dbInstace.InitDbEntity();
            if (list.Any())
            {
                var parentName = list.FirstOrDefault().ParentName;
                treeView1.Nodes.Add(parentName,parentName);
                var parentNode = treeView1.Nodes[parentName];
                //添加子项
                parentNode.Nodes.AddRange(list.Select(m => new TreeNode { Text = m.TableName, Name = m.TableName}).ToArray());
            }
            
        }

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
            this.textBox1.Text = e.Node.Text;
            this.textBox1.ReadOnly = true;
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
            string type = this.radioButton1.Checked ? "1" : "2";
            this.richTextBox1.Clear();
            string tablename = this.textBox1.Text;
            var list = dbService.InitTableEntity(tablename);
            if (list.Any())
            {
               this.richTextBox1.Text = FormTextBuilder.Builder(list, tablename, type);
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
    }
}