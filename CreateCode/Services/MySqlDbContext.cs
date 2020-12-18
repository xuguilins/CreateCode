
using CreateCode.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Services
{
    public class MySqlDbContext : BaseService
    {
        public override ReturnResult ConnectionDb(string connectionString)
        {
            throw new NotImplementedException();
        }

        public override List<MenuEntity> InitDbEntity()
        {
            throw new NotImplementedException();
        }

        public override List<TableEntity> InitTableEntity(string tableName)
        {
            throw new NotImplementedException();
        }

        public override DbConnection DbConnection { get; set; }
    }
}
