using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Action
{
    public class LogService
    {

        /// <summary>
        /// 登陆提醒
        /// </summary>
        /// <returns></returns>
        public  static List<string> GetMessage()
        {
            List<string> list = new List<string>();
            list.Add("正在进行服务器的连接.....");
            list.Add("正在检查服务器网络环境是否连接.....");
            list.Add("正在连接服务器.....");
            list.Add("服务器连接成功.....");
            list.Add("正在初始化数据库.....");
            list.Add("正在初始化程序界面....");
            list.Add("界面初始化成功...");
            list.Add("正在做最后的配置....");
            list.Add("启动成功,请稍等.....");
            return list;
        }

        /// <summary>
        /// 关键字
        /// </summary>
        /// <returns></returns>
        public static  List<string> PrimaryKeyList()
        {
            List<string> lists = new List<string>()
        { "using", "namespace", "public", "int", "string", "DateTime", "class", "decimal","bool","if","else","return","Convert","SqlParameter","SQLHelper","DBNull","new","DataRow","static","CommandType","readonly","SqlConnection","SqlCommand","DataTable","DataSet","object","Dictionary","SqlBulkCopy","Exception","try","catch","Exception","throw","params","double","float","get","set","value"};
            return lists;
        }
    }

  
}
