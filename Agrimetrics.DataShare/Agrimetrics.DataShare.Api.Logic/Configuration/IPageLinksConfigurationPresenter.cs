using Agrimetrics.DataShare.Api.Core.Configuration.Model;

namespace Agrimetrics.DataShare.Api.Logic.Configuration;

public interface IPageLinksConfigurationPresenter
{
    string GetDataMarketPlaceSignInAddress();

    IEnumerable<SettingValue> GetAllSettings();
}