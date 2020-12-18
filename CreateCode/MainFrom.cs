
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.textBox1.Text = e.Node.Text;
            this.textBox1.ReadOnly = true;
        }
        #region[字体查找]

        public void changeStrColorFont(RichTextBox rtBox)
        {
            var keys = KeywordDictory.GetKeyworkds();
           
            for (int i = 0; i < keys.Count; i++)
            {
                int pos = 0;
                string str = keys[i];
                while (true)
                {
                    pos = rtBox.Find(str, pos, RichTextBoxFinds.WholeWord);
                    if (pos == -1)
                        break;
                    rtBox.SelectionStart = pos;
                    rtBox.SelectionLength = str.Length;
                    rtBox.SelectionColor = Color.Blue;
                    pos += 1;
                }
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
                var text = richTextBox1.Text;
                changeStrColorFont(richTextBox1);
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