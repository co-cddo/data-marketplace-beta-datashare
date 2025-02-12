using Agrimetrics.DataShare.Api.Core.Configuration.Model;
using Microsoft.Extensions.Configuration;

namespace Agrimetrics.DataShare.Api.Db.Configuration;

internal class DatabaseConnectionsConfigurationPresenter(
    IConfiguration configuration) : IDatabaseConnectionsConfigurationPresenter
{
    string IDatabaseConnectionsConfigurationPresenter.GetSqlConnectionString() => DoGetSqlConnectionString();

    IEnumerable<SettingValue> IDatabaseConnectionsConfigurationPresenter.GetAllSettingValues()
    {
        return new List<SettingValue>
        {
            new()
            {
                Description = "Database Connection String",
                Value = GetSettingValue(DoGetSqlConnectionString)
            }
        };

        static string GetSettingValue(Func<string> getSettingValueFunc)
        {
            try
            {
                return getSettingValueFunc();
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }
    }

    private string DoGetSqlConnectionString()
    {
        var sqlConnectionString = configuration.GetConnectionString("sql_connection_string");

        ArgumentException.ThrowIfNullOrWhiteSpace(sqlConnectionString);

        return sqlConnectionString;
    }
}