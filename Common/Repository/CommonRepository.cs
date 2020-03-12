using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Common.Repository
{
    public class CommonRepository : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields
        protected SqlConnection _connection;
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

        private bool _isInLambda = false;
        private bool _isInTransaction = false;
        private SqlTransaction _transaction;

        #endregion

        #region Ctr & Init
        public CommonRepository()
        {

        }

        private void InitConnection()
        {
            Log.Debug("Called");
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public void Dispose()
        {
            Log.Debug("Called");

            try
            {
                Log.Debug("Closing connection");
                _connection.Close();
                _connection.Dispose();
            }
            catch (Exception e)
            {
                Log.Error("Dispose error: ", e);
            }
        }
        #endregion

        public void CloseConnections()
        {
            Log.Debug("Called");
            if (_connection.State == ConnectionState.Open)
            {
                try
                {
                    _connection.Close();
                    _connection.Dispose();

                }
                catch (Exception ex)
                {
                    Log.Error("Error Occured:", ex);
                }
            }
        }

        #region Helpers

        public IEnumerable<T> Query<T>(string sql, dynamic param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            if (!_isInLambda) InitConnection();


            using (this)
            {
                try
                {

                    BeginTransactionSingleOperation();

                    IEnumerable<T> result = _connection.Query<T>(sql,
                        (object)param,
                        transaction: _transaction,
                        commandType: commandType);

                    CommitSignleOperation();

                    return result;
                }
                catch (Exception)
                {
                    RollbackSingleOperation();

                    throw;
                }
            }
        }

        public T QueryFirst<T>(string sql, dynamic param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return ((IEnumerable<T>)Query<T>(sql, param, commandType)).FirstOrDefault();
        }

        public void Execute(string sql, dynamic param)
        {
            InitConnection();

            using (this)
            {
                try
                {
                    BeginTransactionSingleOperation();

                    _connection.Execute(sql,
                       (object)param,
                       transaction: _transaction,
                       commandType: CommandType.StoredProcedure);

                    CommitSignleOperation();
                }
                catch (Exception)
                {
                    RollbackSingleOperation();

                    throw;
                }
            }
        }

        #endregion

        #region Transaction helpers


        public void BeginTransactionSingleOperation()
        {
            Log.Debug("Called");

            if (!_isInLambda)
            {
                Log.Debug("Beginning transaction");
                BeginInnerTransaction();
            }
        }


        public void CommitSignleOperation()
        {
            Log.Debug("Called");
            if (!_isInLambda)
            {
                Log.Debug("Commiting transaction");
                CommitInnerTransaction();
            }

        }


        public void RollbackSingleOperation()
        {
            Log.Debug("Called");
            if (!_isInLambda)
            {
                Log.Debug("Rolling back transaction");
                RollbackInnerTransaction();
            }
        }

        private void CommitInnerTransaction()
        {
            if (_transaction != null && _isInTransaction)
            {
                _transaction.Commit();
            }
        }


        private void RollbackInnerTransaction()
        {
            if (_transaction != null && _isInTransaction)
            {
                _transaction.Rollback();
            }

        }


        private void BeginInnerTransaction()
        {
            if (_isInTransaction)
            {
                _transaction = _connection.BeginTransaction();
            }
        }
        #endregion
    }
}
