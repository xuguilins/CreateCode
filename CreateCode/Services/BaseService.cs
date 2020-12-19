
using CreateCode.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Services
{
    public abstract class BaseService
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public abstract DbBaseType DbBaseType { get;  }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public virtual string  DataBaseName { get;  set; }
        /// <summary>
        /// 数据库的登录名
        /// </summary>
        public virtual string UserId { get; set; }
        /// <summary>
        /// 数据库的登录密码
        /// </summary>
        public virtual string PassWord { get; set; }
        /// <summary>
        /// 数据库链接对象
        /// </summary>
        public abstract DbConnection DbConnection { get; set; }
        /// <summary>
        /// 链接数据库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public abstract ReturnResult ConnectionDb(string connectionString);
        /// <summary>
        /// 初始化数据表
        /// </summary>
        /// <returns></returns>
        public abstract List<MenuEntity> InitDbEntity();
        /// <summary>
        /// 获取指定数据表的字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract List<TableEntity> InitTableEntity(string tableName);
    }
}