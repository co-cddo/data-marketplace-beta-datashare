using System.Data;

namespace Agrimetrics.DataShare.Api.Db.DbAccess;

public interface IDatabaseChannel : IDisposable, IAsyncDisposable
{
    IDbConnection Connection { get; }
    IDbTransaction? Transaction { get; }

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}