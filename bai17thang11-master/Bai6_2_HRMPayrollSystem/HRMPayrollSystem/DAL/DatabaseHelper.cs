using System;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace HRMPayrollSystem.DAL
{
    public static class DatabaseHelper
    {
        private static readonly string DefaultConnString = 
            "Data Source=.\\SQLEXPRESS;Initial Catalog=HRMDB;Integrated Security=True;TrustServerCertificate=True;";

        public static string GetConnectionString()
        {
            var cs = ConfigurationManager.ConnectionStrings["HRMDBConn"];
            if (cs != null && !string.IsNullOrWhiteSpace(cs.ConnectionString))
            {
                return cs.ConnectionString;
            }
            return DefaultConnString;
        }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
