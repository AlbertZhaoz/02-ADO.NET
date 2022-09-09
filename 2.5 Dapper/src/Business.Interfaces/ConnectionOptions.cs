using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public class DbConnectionOptions
    {
        public string ConnectionString { get; set; }
        public ConnectionTypeEnum ConnectionType { get; set; }
    }

    public enum ConnectionTypeEnum
    {
        SqlServer = 1
    }
}
