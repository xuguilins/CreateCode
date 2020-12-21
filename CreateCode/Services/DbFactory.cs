using CreateCode.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CreateCode.Services
{
    public static class DbFactory
    {
      
       public static BaseService CreateInstance(DbBaseType dbType)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(m => typeof(BaseService).IsAssignableFrom(m) && !m.IsAbstract && m.IsClass && !m.IsInterface).ToList();
            BaseService service = null;
            for (int i = 0; i < types.Count; i++)
            {
                var item = types[i];
                var value = item.GetProperty("DbBaseType");
                var instaceValue = Activator.CreateInstance(item);
                var propItem = value.GetValue(instaceValue, null)?.ToString();
                if (dbType.ToString() == propItem)
                {
                    service = instaceValue as BaseService;
                    break;
                }
            }
            return service;
        }
    }
}
