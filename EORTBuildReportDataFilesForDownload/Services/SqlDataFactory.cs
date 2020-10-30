using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EORTBuildReportDataFilesForDownload.Services
{
    public class SqlDataFactory
    {
        public static string GetConnectionString()
        {
            return AppSettingServices.GetConnectionString("manualQueryConnectionString");
        }

        public static DbConnection CreateAndOpenConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public static DbCommand CreateCommand(DbConnection connection, string queryText)
        {
            SqlCommand command = new SqlCommand(queryText, (SqlConnection)connection)
            {
                CommandTimeout = 1000
            };

            return command;
        }

        public static DbCommand CreateCommand(DbConnection connection, string queryText, int commandTimeout)
        {
            SqlCommand command = new SqlCommand(queryText, (SqlConnection)connection)
            {
                CommandTimeout = commandTimeout
            };

            return command;
        }

        public static DbCommand CreateCommand(DbConnection connection, string queryText, Dictionary<string, object> parameters)
        {
            SqlCommand command = (SqlCommand)CreateCommand(connection, queryText);

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> param in parameters)
                {
                    command.Parameters.AddWithValue("@" + param.Key, param.Value);
                }
            }

            return command;
        }

        public static DbCommand CreateCommand(DbConnection connection, string queryText, int commandTimeout, Dictionary<string, object> parameters)
        {
            SqlCommand command = (SqlCommand)CreateCommand(connection, queryText, commandTimeout);

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> param in parameters)
                {
                    command.Parameters.AddWithValue("@" + param.Key, param.Value);
                }
            }

            return command;
        }

        public static DbDataReader CreateReader(DbCommand command)
        {
            return command.ExecuteReader();
        }

        public static DbTransaction CreateTransaction(DbConnection connection)
        {
            return connection.BeginTransaction();
        }
    }
}
