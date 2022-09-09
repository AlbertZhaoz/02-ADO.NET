using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CustomSqlCache<T> where T : class
    {
        private static string QuerySql = string.Empty;
         

        static CustomSqlCache()
        {
            Type type = typeof(T); 
            QuerySql = $"SELECT {string.Join(",", type.GetProperties().Select(c=>$"[{c.Name}]"))} FROM {$"[{type.Name}]"}";
           
        }

        public static string GetQuerySql() => QuerySql;
        public static string GetFindByIdSql(int Id) => $"{QuerySql} WHERE ID={Id}";



    }
}
