using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

namespace Agrimetrics.DataShare.Api.Logic.Services.Users
{
    public interface IUserProfilePresenter
    {
        Task<IUserIdSet> GetInitiatingUserIdSetAsync();

        Task<IUserDetails> GetInitiatingUserDetailsAsync();

        Task<IUserDetails> GetUserDetailsByUserIdAsync(int userId);

        Task<IEnumerable<IUserDetails>> GetUserDetailsByUserIdsAsync(IEnumerable<int> userIds);

        Task<IUserDetails?> GetUserDetailsByUserEmailAddressAsync(string userEmailAddress);

        Task<IOrganisationInformation> GetInitiatingUserOrganisationInformationAsync();

        Task<IOrganisationInformation> GetOrganisationInformationByUserIdAsync(int userId);

        Task<IOrganisationInformation> GetOrganisationDetailsByOrganisationIdAsync(int organisationId);
    }
}
