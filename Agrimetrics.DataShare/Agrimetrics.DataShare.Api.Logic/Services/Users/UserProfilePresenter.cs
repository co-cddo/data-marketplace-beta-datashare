using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model.External;
using Agrimetrics.DataShare.Api.Logic.Services.Users.UserIdPresentation;
using Flurl.Http;

namespace Agrimetrics.DataShare.Api.Logic.Services.Users;

internal class UserProfilePresenter(
    IUserIdPresenter userIdPresenter,
    IUsersServiceConfigurationPresenter usersServiceConfigurationPresenter) : IUserProfilePresenter
{
    async Task<IUserIdSet> IUserProfilePresenter.GetInitiatingUserIdSetAsync()
    {
        var userProfile = await GetUserProfileOfInitiatingUserAsync();

        return BuildUserIdSetFromUserProfile(userProfile);
    }

    async Task<IUserDetails> IUserProfilePresenter.GetInitiatingUserDetailsAsync()
    {
        var userProfile = await GetUserProfileOfInitiatingUserAsync();

        return BuildUserDetailsFromProfile(userProfile);
    }

    async Task<IUserDetails> IUserProfilePresenter.GetUserDetailsByUserIdAsync(int userId)
    {
        var userProfile = await GetUserProfileByIdAsync(userId);

        return BuildUserDetailsFromProfile(userProfile);
    }

    async Task<IEnumerable<IUserDetails>> IUserProfilePresenter.GetUserDetailsByUserIdsAsync(IEnumerable<int> userIds)
    {
        var userProfiles = await GetUserProfilesByIdsAsync(userIds);

        return userProfiles.Select(BuildUserDetailsFromProfile);
    }

    async Task<IUserDetails?> IUserProfilePresenter.GetUserDetailsByUserEmailAddressAsync(string userEmailAddress)
    {
        try
        {
            var userProfile = await GetUserProfileByEmailAddressAsync(userEmailAddress);

            return BuildUserDetailsFromProfile(userProfile);
        }
        catch
        {
            return null;
        }
    }

    async Task<IOrganisationInformation> IUserProfilePresenter.GetInitiatingUserOrganisationInformationAsync()
    {
        var userProfile = await GetUserProfileOfInitiatingUserAsync();

        return BuildOrganisationInformationFromUserProfile(userProfile);
    }

    async Task<IOrganisationInformation> IUserProfilePresenter.GetOrganisationInformationByUserIdAsync(int userId)
    {
        var userProfile = await GetUserProfileByIdAsync(userId);

        return BuildOrganisationInformationFromUserProfile(userProfile);
    }

    async Task<IOrganisationInformation> IUserProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(int organisationId)
    {
        var userOrganisation = await GetUserOrganisationByOrganisationIdAsync(organisationId);

        return BuildOrganisationInformationFromUserOrganisation(userOrganisation);
    }

    private static IUserDetails BuildUserDetailsFromProfile(
        UserProfile userProfile)
    {
        return new UserDetails
        {
            UserIdSet = BuildUserIdSetFromUserProfile(userProfile),
            UserContactDetails = BuildUserContactDetailsFromUserProfile(userProfile)
        };
    }

    private static IUserIdSet BuildUserIdSetFromUserProfile(
        UserProfile userProfile)
    {
        return new UserIdSet
        {
            UserId = userProfile.User.UserId,
            DomainId = userProfile.Domain.DomainId,
            OrganisationId = userProfile.Organisation.OrganisationId,
            EmailNotification = userProfile.EmailNotification,
        };
    }

    private static IUserContactDetails BuildUserContactDetailsFromUserProfile(
        UserProfile initiatingUserProfile)
    {
        return new UserContactDetails
        {
            EmailAddress = initiatingUserProfile.User.UserEmail,
            UserName = initiatingUserProfile.User.UserName,
            EmailNotification = initiatingUserProfile.EmailNotification
        };
    }

    private static IOrganisationInformation BuildOrganisationInformationFromUserProfile(
        UserProfile userProfile)
    {
        return new OrganisationInformation
        {
            OrganisationId = userProfile.Organisation.OrganisationId,
            OrganisationName = userProfile.Organisation.OrganisationName,
            Domains =
            [
                new DomainInformation
                {
                    DomainId = userProfile.Domain.DomainId,
                    DomainName = userProfile.Domain.DomainName,
                    DataShareRequestMailboxAddress = userProfile.Domain.DataShareRequestMailboxAddress
                }
            ]
        };
    }

    private static IOrganisationInformation BuildOrganisationInformationFromUserOrganisation(
        UserOrganisation userOrganisation)
    {
        return new OrganisationInformation
        {
            OrganisationId = userOrganisation.OrganisationId,
            OrganisationName = userOrganisation.OrganisationName,
            Domains = userOrganisation.Domains.Select(BuildDomainInformationFromUserDomain)
        };

        IDomainInformation BuildDomainInformationFromUserDomain(UserDomain userDomain)
        {
            return new DomainInformation
            {
                DomainId = userDomain.DomainId,
                DomainName = userDomain.DomainName,
                DataShareRequestMailboxAddress = userDomain.DataShareRequestMailboxAddress
            };
        }
    }

    private async Task<UserProfile> GetUserProfileOfInitiatingUserAsync()
    {
        try
        {
            var initiatingUserIdToken = userIdPresenter.GetInitiatingUserIdToken();

            var getUserInfoByTokenEndPoint = usersServiceConfigurationPresenter.GetUserInfoByTokenEndPoint();

            return await getUserInfoByTokenEndPoint
                .WithOAuthBearerToken(initiatingUserIdToken)
                .PostJsonAsync(null)
                .ReceiveJson<UserProfile>();
        }
        catch (FlurlHttpException ex)
        {
            throw new ExternalServiceAccessException($"GetUserProfileOfInitiatingUserAsync: Failed to fetch user profile from users service: {ex.StatusCode}");
        }
    }

    private async Task<UserProfile> GetUserProfileByIdAsync(int userId)
    {
        try
        {
            var initiatingUserIdToken = userIdPresenter.GetInitiatingUserIdToken();

            var getUserInfoByUserIdEndPoint = usersServiceConfigurationPresenter.GetUserInfoByUserIdEndPoint();

            return await getUserInfoByUserIdEndPoint
                .WithOAuthBearerToken(initiatingUserIdToken)
                .SetQueryParam("userid", userId)
                .GetJsonAsync<UserProfile>();
        }
        catch (FlurlHttpException ex)
        {
            throw new ExternalServiceAccessException($"GetUserProfileByIdAsync: Failed to fetch user profile from users service: {ex.StatusCode}");
        }
    }

    private async Task<IEnumerable<UserProfile>> GetUserProfilesByIdsAsync(IEnumerable<int> userIds)
    {
        try
        {
            var initiatingUserIdToken = userIdPresenter.GetInitiatingUserIdToken();

            var getUserInfosByUserIdsEndPoint = usersServiceConfigurationPresenter.GetUserInfosByUserIdsEndPoint();

            return await getUserInfosByUserIdsEndPoint
                .WithOAuthBearerToken(initiatingUserIdToken)
                .SetQueryParam("userIds", userIds)
                .GetJsonAsync<List<UserProfile>>();
        }
        catch (FlurlHttpException ex)
        {
            throw new ExternalServiceAccessException($"GetUserProfilesByIdsAsync: Failed to fetch user profiles from users service: {ex.StatusCode}");
        }
    }

    private async Task<UserProfile> GetUserProfileByEmailAddressAsync(string userEmailAddress)
    {
        try
        {
            var initiatingUserIdToken = userIdPresenter.GetInitiatingUserIdToken();

            var getUserInfoByUserEmailAddressEndPoint = usersServiceConfigurationPresenter.GetUserInfoByUserEmailEndAddressPoint();

            return await getUserInfoByUserEmailAddressEndPoint
                .WithOAuthBearerToken(initiatingUserIdToken)
                .SetQueryParam("email", userEmailAddress)
                .GetJsonAsync<UserProfile>();
        }
        catch (FlurlHttpException ex)
        {
            throw new ExternalServiceAccessException($"GetUserProfileByEmailAddressAsync: Failed to fetch user profile from users service: {ex.StatusCode}");
        }
    }

    private async Task<UserOrganisation> GetUserOrganisationByOrganisationIdAsync(int organisationId)
    {
        try
        {
            var initiatingUserIdToken = userIdPresenter.GetInitiatingUserIdToken();

            var getUserOrganisationByOrganisationIdEndPoint = usersServiceConfigurationPresenter.GetUserOrganisationByOrganisationIdEndPoint();

            return await getUserOrganisationByOrganisationIdEndPoint
                .WithOAuthBearerToken(initiatingUserIdToken)
                .AppendPathSegment(organisationId)
                .GetJsonAsync<UserOrganisation>();
        }
        catch (FlurlHttpException ex)
        {
            throw new ExternalServiceAccessException($"GetUserOrganisationByOrganisationId: Failed to get user organisation from users service: {ex.StatusCode}");
        }
    }
}