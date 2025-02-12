using Agrimetrics.DataShare.Api.Core.Configuration;
using Agrimetrics.DataShare.Api.Core.Configuration.Model;

namespace Agrimetrics.DataShare.Api.Logic.Services.Esdas.Configuration;

internal class DataAssetInformationServiceConfigurationPresenter(
    IServiceConfigurationPresenter serviceConfigurationPresenter) : IDataAssetInformationServiceConfigurationPresenter
{
    private const string externalServiceApisSectionName = "ExternalServices";
    private const string utilityApiConfigSectionName = "UtilityApi";
    private const string utilityApiAddressValueKey = "api_address";

    string IDataAssetInformationServiceConfigurationPresenter.GetDataAssetByIdEndPoint()
        => DoGetDataAssetByIdEndPoint();
    
    string IDataAssetInformationServiceConfigurationPresenter.GetEsdaOwnershipDetailsEndPoint()
        => DoGetEsdaOwnershipDetailsEndPoint();

    public IEnumerable<SettingValue> GetAllSettings()
    {
        return new List<SettingValue>
        {
            DoGetSettingValue("Get Data Asset By Id EndPoint", DoGetDataAssetByIdEndPoint),
            DoGetSettingValue("Get Esda Ownership Details Endpoint", DoGetEsdaOwnershipDetailsEndPoint)
        };

        static SettingValue DoGetSettingValue(string description, Func<string> getSettingValueFunc)
        {
            return new SettingValue
            {
                Description = description,
                Value = GetSettingValue()
            };

            string GetSettingValue()
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
    }

    private string DoGetDataAssetByIdEndPoint()
    {
        var utilityApiAddress = GetUtilityApiAddress();

        return $"{utilityApiAddress}/DataAsset/get-cddo-data-asset";
    }

    private string DoGetEsdaOwnershipDetailsEndPoint()
    {
        var utilityApiAddress = GetUtilityApiAddress();

        return $"{utilityApiAddress}/DataAsset/get-esda-ownership-details";
    }

    private string GetUtilityApiAddress() =>
        serviceConfigurationPresenter.GetValueInMultiLevelSection(
            [externalServiceApisSectionName, utilityApiConfigSectionName],
            utilityApiAddressValueKey);
}