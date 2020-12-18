using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Model
{
    public class ReturnResult
    {
        public ReturnResult(string message,bool success,object data = null)
        {
            Message = message;
            Data = data;
            Success = success;
        }
        public string Message { get; set; }
        public object Data { get; set; }
        public bool Success { get; set; }
    }
}
