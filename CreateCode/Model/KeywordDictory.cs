using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Model
{
    public static class KeywordDictory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetKeyworkds()
        {
            return new List<string>
            {
                "class",
                "new",
                "string",
                "int",
                "DateTime",
                "Guid",
                "byte[]",
                "float",
                "decimal",
                "static",
                "get",
                "set",
                "return",
                "public",
                "using",
                "double",
                "namespace"
            };
        }
    }
}
