using CreateCode.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Services
{
    public static class DbFactory
    {
        /// <summary>
        /// 可以用反射来实现
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
       public static BaseService CreateInstance(DbBaseType dbType)
        {
            BaseService service = null;
            switch (dbType)
            {
                case DbBaseType.SqlServer:
                    service = new SqlServerDbContext();
                    break;
                case DbBaseType.MySql:
                    service = new MySqlDbContext();
                    break;
                case DbBaseType.Oracle:
                    service = new OracleDbContext();
                    break;

            }
            return service;
        }
    }
}
