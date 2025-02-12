using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;

namespace Agrimetrics.DataShare.Api.HealthChecks;

[ExcludeFromCodeCoverage] // This class provides a proxy for running SQL commands using third party elements, so cannot be unit tested
internal class CustomSqlHealthCheckSqlCommandRunner : ICustomSqlHealthCheckSqlCommandRunner
{
    async Task ICustomSqlHealthCheckSqlCommandRunner.RunCommandAsync(
        string connectionString, 
        string command,
        CancellationToken cancellationToken)
    {
        await using var sqlConnection = new SqlConnection(connectionString);
        await sqlConnection.OpenAsync(cancellationToken);

        await using var sqlCommand = new SqlCommand("SELECT 1;", sqlConnection);
        await sqlCommand.ExecuteScalarAsync(cancellationToken);
    }
}