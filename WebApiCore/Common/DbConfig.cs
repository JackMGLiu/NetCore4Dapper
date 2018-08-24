using System.Data;
using System.Data.SqlClient;

namespace WebApiCore.Common
{
    public class DbConfig
    {
        private static string DefaultSqlConnectionString = @"server=140.143.7.32;database=MicroServiceDb;uid=sa;pwd=sa2012LJ";

        public static IDbConnection GetSqlConnection(string sqlConnectionString = null)
        {
            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                sqlConnectionString = DefaultSqlConnectionString;
            }
            IDbConnection conn = new SqlConnection(sqlConnectionString);
            conn.Open();
            return conn;
        }
    }
}