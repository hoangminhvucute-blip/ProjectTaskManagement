using System.Configuration;
using System.Data.SqlClient;

namespace ProjectTaskManagement.DAL
{
    public class DbConnection
    {
        private static readonly string _connStr =
            ConfigurationManager.ConnectionStrings["ProjectTaskDBConn"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connStr);
        }
    }
}
