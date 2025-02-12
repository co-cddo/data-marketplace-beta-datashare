using Agrimetrics.DataShare.Api.Core.Configuration;
using Agrimetrics.DataShare.Api.Core.Configuration.Model;

namespace Agrimetrics.DataShare.Api.Logic.Configuration;

internal class PageLinksConfigurationPresenter(
    IServiceConfigurationPresenter serviceConfigurationPresenter) : IPageLinksConfigurationPresenter
{
    private const string pageLinksSectionName = "PageLinks";

    string IPageLinksConfigurationPresenter.GetDataMarketPlaceSignInAddress() =>
        DoGetDataMarketPlaceSignInAddress();

    public IEnumerable<SettingValue> GetAllSettings()
    {
        return new List<SettingValue>
        {
            DoGetSettingValue("Get Data Market Place Sign In Address", DoGetDataMarketPlaceSignInAddress)
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

    private string DoGetDataMarketPlaceSignInAddress() =>
        DoGetPageLinkValue("data_marketplace_sign_in_address");

    private string DoGetPageLinkValue(string valueKey) => 
        serviceConfigurationPresenter.GetValueInSection(pageLinksSectionName, valueKey);
}