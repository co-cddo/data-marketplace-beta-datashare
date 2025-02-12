using Agrimetrics.DataShare.Api.Core.Configuration.Model;

namespace Agrimetrics.DataShare.Api.Db.Configuration;

public interface IDatabaseConnectionsConfigurationPresenter
{
    string GetSqlConnectionString();

    IEnumerable<SettingValue> GetAllSettingValues();
}