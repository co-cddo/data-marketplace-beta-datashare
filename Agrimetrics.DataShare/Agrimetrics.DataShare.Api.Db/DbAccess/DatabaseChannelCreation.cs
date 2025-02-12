using System.Diagnostics.CodeAnalysis;
using Agrimetrics.DataShare.Api.Db.Configuration;
using Microsoft.Data.SqlClient;

namespace Agrimetrics.DataShare.Api.Db.DbAccess;

[ExcludeFromCodeCoverage] // This class provides a proxy to a third party SQL connection, which cannot be reliably unit tested
internal class DatabaseChannelCreation(
    IDatabaseConnectionsConfigurationPresenter databaseConnectionsConfigurationPresenter) : IDatabaseChannelCreation
{
    async Task<IDatabaseChannel> IDatabaseChannelCreation.CreateAsync(
        bool beginTransaction)
    {
        var connectionString = databaseConnectionsConfigurationPresenter.GetSqlConnectionString();

        var connection = new SqlConnection(connectionString);

        await connection.OpenAsync();

        IDatabaseChannel databaseChannel = new DatabaseChannel(connection);

        if (beginTransaction)
        {
            await databaseChannel.BeginTransactionAsync();
        }

        return databaseChannel;
    }
}