using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Db.DbAccess;

[ExcludeFromCodeCoverage] // This class provides a proxy to a third party SQL connection, which cannot be reliably unit tested
internal class DatabaseChannel(
    DbConnection connection) : IDatabaseChannel
{
    private DbTransaction? transaction;

    IDbConnection IDatabaseChannel.Connection => connection;

    IDbTransaction? IDatabaseChannel.Transaction => transaction;

    async Task IDatabaseChannel.BeginTransactionAsync()
    {
        if (transaction != null)
            throw new InvalidOperationException("Unable to begin a transaction that has already been begun");

        transaction = await connection.BeginTransactionAsync().ConfigureAwait(false);
    }

    async Task IDatabaseChannel.CommitTransactionAsync()
    {
        if (transaction == null)
            throw new InvalidOperationException("Unable to commit transaction that has not been begun");

        await transaction.CommitAsync().ConfigureAwait(false);
    }

    async Task IDatabaseChannel.RollbackTransactionAsync()
    {
        if (transaction == null)
            throw new InvalidOperationException("Unable to rollback transaction that has not been begun");

        await transaction.RollbackAsync().ConfigureAwait(false);
    }

    void IDisposable.Dispose()
    {
        transaction?.Dispose();

        connection.Dispose();
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (transaction != null)
        {
            await transaction.DisposeAsync();
        }

        await connection.DisposeAsync();
    }
}