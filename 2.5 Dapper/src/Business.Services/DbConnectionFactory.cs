using Business.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private DbConnectionOptions _DbConnectionOptions = null;

        public DbConnectionFactory(IOptions<DbConnectionOptions> options)
        {
            this._DbConnectionOptions = options.Value;
        }

        /// <summary>
        /// 获取一个Connection实例
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            IDbConnection dbConnection = null;
            switch (_DbConnectionOptions.ConnectionType)
            {
                case ConnectionTypeEnum.SqlServer:
                    dbConnection = new SqlConnection(_DbConnectionOptions.ConnectionString);
                    break;
                default:
                    throw new Exception("No SqlConnection....");
            }
            if (dbConnection != null)
            {
                dbConnection.Open();
            }
            return dbConnection;
        }
    }
}
