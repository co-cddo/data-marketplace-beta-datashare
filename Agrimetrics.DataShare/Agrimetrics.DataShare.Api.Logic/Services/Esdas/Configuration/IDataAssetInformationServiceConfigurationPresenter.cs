using Agrimetrics.DataShare.Api.Core.Configuration.Model;

namespace Agrimetrics.DataShare.Api.Logic.Services.Esdas.Configuration
{
    public interface IDataAssetInformationServiceConfigurationPresenter
    {
        string GetDataAssetByIdEndPoint();

        string GetEsdaOwnershipDetailsEndPoint();

        IEnumerable<SettingValue> GetAllSettings();
    }
}
