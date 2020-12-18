using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Model
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DbBaseType
    {
        [Description("SqlServer")]
        SqlServer = 1,
        [Description("Oracle")]
        Oracle = 2,
        [Description("MySql")]
        MySql = 3
    }
}