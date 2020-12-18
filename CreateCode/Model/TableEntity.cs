using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Model
{
    public class TableEntity
    {
        public string table_name { get; set; }
        public string column_name { get; set; }
        public string column_length { get; set; }
        public string colum_type { get; set; }

        public DbBaseType  DbBaseType { get; set; }
    }
}