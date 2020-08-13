using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Services
{
    public abstract class BaseService
    {
        public virtual DbType Type { get; set; }

        public string BegineCreate()
        {
            return null;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        public abstract string CreateModel();

        /// <summary>
        /// 创建数据层
        /// </summary>
        /// <returns></returns>
        public abstract string CreateDbService();

        /// <summary>
        /// 创建业务层
        /// </summary>
        /// <returns></returns>
        public abstract string CreateService();

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <returns></returns>
        public abstract DbType GetDbType();
    }
}