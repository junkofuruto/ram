using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteAccessManager
{
    internal static class ConnectionManager
    {
        private static SqlConnection _connection;
        public static SqlConnection SqlConnection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection();
                    _connection.ConnectionString = @"Server=FUMOBOMBER; Database=ramdb; Trusted_Connection=True;";
                    _connection.Open();
                }

                return _connection;
            }
        }
    }
}
