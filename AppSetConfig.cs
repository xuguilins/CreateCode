using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Common
{
    public class AppSetConfig
    {
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <returns></returns>
        public static string GetConnection()
        {

            return ConfigurationManager.ConnectionStrings["Constr"].ConnectionString; ;
        }
    }
}
