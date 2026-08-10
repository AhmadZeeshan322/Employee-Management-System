using System;
using Microsoft.Data.SqlClient;
namespace EMS
{
    static class DbConnectionHelper
    {
        // Connection string for SQL Server LocalDB
        private static readonly string connectionString =
    @"Server=ems-sql-server,1433;Database=EMSDatabase;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;";
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}