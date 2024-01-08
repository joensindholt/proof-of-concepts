using System;
using System.Data;

namespace DapperDotnetCore.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Begin();
        void Commit();
        void Rollback();
    }
}