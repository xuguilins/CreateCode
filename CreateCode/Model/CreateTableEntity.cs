using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Model
{
    public class CreateTableEntity
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 是否为空
        /// </summary>
        public string ISNULL { get; set; }
        /// <summary>
        /// 是否主键
        /// </summary>
        public string ISKEY { get; set; }
        /// <summary>
        /// 是否自增
        /// </summary>
        public string ISINDENTITY { get; set; }
        /// <summary>
        /// 自增开始是数字
        /// </summary>
        public int STARTNUMBER { get; set; }
    }
}
