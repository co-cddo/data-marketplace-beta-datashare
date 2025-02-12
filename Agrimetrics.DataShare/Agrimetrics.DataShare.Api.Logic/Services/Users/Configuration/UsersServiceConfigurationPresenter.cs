using Agrimetrics.DataShare.Api.Core.Configuration;
using Agrimetrics.DataShare.Api.Core.Configuration.Model;

namespace Agrimetrics.DataShare.Api.Logic.Services.Users.Configuration;

internal class UsersServiceConfigurationPresenter(
    IServiceConfigurationPresenter serviceConfigurationPresenter) : IUsersServiceConfigurationPresenter
{
    private const string externalServiceApisSectionName = "ExternalServices";
    private const string usersApiConfigSectionName = "UsersApi";
    private const string usersApiAddressValueKey = "api_address";

    string IUsersServiceConfigurationPresenter.GetUserInfoByTokenEndPoint()
        => DoGetUserInfoByTokenEndPoint();

    string IUsersServiceConfigurationPresenter.GetUserInfoByUserIdEndPoint()
        => DoGetUserInfoByUserIdEndPoint();

    string IUsersServiceConfigurationPresenter.GetUserInfosByUserIdsEndPoint()
        => DoGetUserInfosByUserIdsEndPoint();

    string IUsersServiceConfigurationPresenter.GetUserInfoByUserEmailEndAddressPoint()
        => DoGetUserInfoByUserEmailAddressEndPoint();

    string IUsersServiceConfigurationPresenter.GetUserOrganisationByOrganisationIdEndPoint()
        => DoGetUserOrganisationByOrganisationIdEndPoint();

    public IEnumerable<SettingValue> GetAllSettings()
    {
        return new List<SettingValue>
        {
            DoGetSettingValue("Get User Info By Token EndPoint", DoGetUserInfoByTokenEndPoint),
            DoGetSettingValue("Get User Info By User Id EndPoint", DoGetUserInfoByUserIdEndPoint),
            DoGetSettingValue("Get User Infos By User Ids EndPoint", DoGetUserInfosByUserIdsEndPoint),
            DoGetSettingValue("Get User Info By User Email Address EndPoint", DoGetUserInfoByUserEmailAddressEndPoint),
            DoGetSettingValue("Get UserOrganisation By Organisation Id EndPoint", DoGetUserOrganisationByOrganisationIdEndPoint)
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

    private string DoGetUserInfoByTokenEndPoint()
    {
        var userInfoAddress = GetUserInfoAddress();

        return $"{userInfoAddress}/User/userinfo";
    }

    private string DoGetUserInfoByUserIdEndPoint()
    {
        var userInfoAddress = GetUserInfoAddress();

        return $"{userInfoAddress}/User/UserById";
    }

    private string DoGetUserInfosByUserIdsEndPoint()
    {
        var userInfoAddress = GetUserInfoAddress();

        return $"{userInfoAddress}/User/UsersById";
    }

    private string DoGetUserInfoByUserEmailAddressEndPoint()
    {
        var userInfoAddress = GetUserInfoAddress();

        return $"{userInfoAddress}/User/UserByEmail";
    }

    private string DoGetUserOrganisationByOrganisationIdEndPoint()
    {
        var userInfoAddress = GetUserInfoAddress();

        return $"{userInfoAddress}/Organisations";
    }

    private string GetUserInfoAddress() =>
        serviceConfigurationPresenter.GetValueInMultiLevelSection(
            [externalServiceApisSectionName, usersApiConfigSectionName],
            usersApiAddressValueKey);
}