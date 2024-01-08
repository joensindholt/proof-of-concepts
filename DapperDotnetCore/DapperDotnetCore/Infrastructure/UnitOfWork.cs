using Npgsql;
using System.Data;

namespace DapperDotnetCore.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public IDbConnection Begin()
        {
            _connection = new NpgsqlConnection("Host=postgres; Port=5432; Database=postgres; Username=dbuser; Password=mysecretpassword;");
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            return _transaction.Connection;
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            _connection.Dispose();
        }
    }
}
