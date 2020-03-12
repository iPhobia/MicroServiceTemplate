using System;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Data;

namespace WcfService.Logic
{
    public class DbGateway : IDisposable
    {
        private readonly SqlConnection _connection;      

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

        public DbGateway()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public void Dispose()
        {
            try
            {
                _connection.Close();
                _connection.Dispose();
            }
            catch (Exception)
            {
            }
        }

        #region Test

        public void InsertData(string input)
        {
            _connection.Execute("[dbo].[Test_InsertData]", new
            {
                text = input
            },
            commandType: CommandType.StoredProcedure);
        }

        internal void AddActionToQueue(string remark)
        {
            _connection.Execute("[dbo].[Test_AddToQueue]",
                new
                {
                    remark
                },
                commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Loggin request/response to Db

        public void WriteLog(string headers, string body, string operation)
        {
            try
            {
                _connection.Execute("[dbo].[WcfLog_AddRecord]", new
                {
                    headers,
                    body,
                    operation
                },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}