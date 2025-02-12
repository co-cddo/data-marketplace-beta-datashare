using Agrimetrics.DataShare.Api.Core.Configuration.Model;

namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Configuration
{
    public interface IUsersServiceConfigurationPresenter
    {
        string GetUserInfoByTokenEndPoint();

        string GetUserInfoByUserIdEndPoint();

        string GetUserInfosByUserIdsEndPoint();

        string GetUserInfoByUserEmailEndAddressPoint();

        string GetUserOrganisationByOrganisationIdEndPoint();

        IEnumerable<SettingValue> GetAllSettings();
    }
}
