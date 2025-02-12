namespace Agrimetrics.DataShare.Api.HealthChecks;

public interface ICustomSqlHealthCheckSqlCommandRunner
{
    Task RunCommandAsync(
        string connectionString,
        string command,
        CancellationToken cancellationToken);
}