using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Agrimetrics.DataShare.Api.HealthChecks
{
    public class CustomSqlHealthCheck(
        ICustomSqlHealthCheckSqlCommandRunner customSqlHealthCheckSqlCommandRunner,
        string connectionString) : IHealthCheck
    {
        private readonly ICustomSqlHealthCheckSqlCommandRunner _customSqlHealthCheckSqlCommandRunner = customSqlHealthCheckSqlCommandRunner ?? throw new ArgumentNullException(nameof(customSqlHealthCheckSqlCommandRunner));
        private readonly string _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _customSqlHealthCheckSqlCommandRunner.RunCommandAsync(
                    _connectionString,
                    "SELECT 1", // Just perform any old SQL to force the connection attempt
                    cancellationToken);

                var description = "SQL Database is up and running.";
                return HealthCheckResult.Healthy(description);
            }
            catch (Exception ex)
            {
                var description = "SQL Database is not responding.";
                return HealthCheckResult.Unhealthy(description, ex);
            }
        }
    }
}