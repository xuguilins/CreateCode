using CreateCode.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CreateCode.Services
{
    public static class CodeExtendsition
    {
       public static DbBaseType GetBaseType(string name)
        {
            //获取枚举
            var type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => typeof(DbBaseType) == x);
           if (name!=null)
            {
                var value = DbBaseType.SqlServer;
                if (name != null)
                   value= (DbBaseType)Enum.Parse(typeof(DbBaseType),name);
                return value;
            }
            return DbBaseType.SqlServer;
        }

        public static string GetStrValue(this string str,string keyword)
        {
            str = Regex.Replace(str, @"\s", "");
            //验证字符串最后以;结尾
            if (!str.EndsWith(";"))
                str += ";";
            var conStr = str.ToUpper().Trim();
            //获取特定字符的位置
            int startIndex = conStr.IndexOf(keyword);
            int allcount = conStr.Length;
            //获取剩余的长度
            var count = allcount - startIndex;
            //剩余字符串
            string otherStr = conStr.Substring(startIndex, count);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < otherStr.Length; i++)
            {
                var item = otherStr[i];
                if (item == ';')
                    break;
                sb.Append(item);
            }
            //分隔获取名称
            var splitItem = sb.ToString().Split('=')[1];
            return splitItem;
        }
    }
}
