using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;

namespace WcfService.Logic
{
    public class DbGatewayADO : IDisposable
    {
        private readonly SqlConnection _connection;

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

        public DbGatewayADO()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public void AddToQueue(string remark)
        {
            SqlCommand command = new SqlCommand("dbo.Test_AddToQueue", _connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@remark",
                Value = remark
            };
            command.Parameters.Add(param);

            command.ExecuteNonQuery();
        }
    }
}