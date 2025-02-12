using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model.External;
using Agrimetrics.DataShare.Api.Logic.Services.Users;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestNotificationRecipientDeterminations;

internal class DataShareRequestNotificationRecipientDetermination(
    IEsdaInformationPresenter esdaInformationPresenter,
    IUserProfilePresenter userProfilePresenter,
    INotificationsConfigurationPresenter notificationsConfigurationPresenter) : IDataShareRequestNotificationRecipientDetermination
{
    async Task<IDataShareRequestNotificationRecipient> IDataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(
        DataShareRequestNotificationInformationModelData dataShareRequestNotificationInformation)
    {
        var userOrganisationInformation = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
            dataShareRequestNotificationInformation.SupplierOrganisationId);

        var esdaDetails = await esdaInformationPresenter.GetEsdaDetailsByIdAsync(dataShareRequestNotificationInformation.EsdaId);

        return esdaDetails.DataShareRequestNotificationRecipientType switch
        {
            DataShareRequestNotificationRecipientType.DomainDsrNotificationAddress => BuildDetailsForDomainRecipient(
                userOrganisationInformation, dataShareRequestNotificationInformation),

            DataShareRequestNotificationRecipientType.EsdaContactPointEmailAddress => BuildDetailsForEsdaContactPointRecipient(
                userOrganisationInformation, esdaDetails),

            DataShareRequestNotificationRecipientType.EsdaCustomDsrNotificationAddress => BuildDetailsForEsdaCustomRecipient(
                userOrganisationInformation, esdaDetails),

            _ => BuildDetailsForCddoAdministrator()
        };
    }

    private IDataShareRequestNotificationRecipient BuildDetailsForDomainRecipient(
        IOrganisationInformation userOrganisationInformation,
        DataShareRequestNotificationInformationModelData dataShareRequestNotificationInformation)
    {
        var userDomainInformation = userOrganisationInformation.Domains.FirstOrDefault(domain => domain.DomainId == dataShareRequestNotificationInformation.SupplierDomainId)
                                    ?? throw new InvalidOperationException(
                                        $"Unable to determine Data Share Request Notification Recipient for ESDA with mismatching supplier domain.  '{dataShareRequestNotificationInformation.SupplierDomainId}'");

        if (string.IsNullOrWhiteSpace(userDomainInformation.DataShareRequestMailboxAddress))
        {
            return BuildDetailsForCddoAdministrator();
        }

        var recipientName = PrettifyRecipientName(userOrganisationInformation.OrganisationName);

        return DoBuildDataShareRequestNotificationRecipient(
                userDomainInformation.DataShareRequestMailboxAddress,
                recipientName);
    }

    private static IDataShareRequestNotificationRecipient BuildDetailsForEsdaContactPointRecipient(
        IOrganisationInformation userOrganisationInformation,
        IEsdaDetails esdaDetails)
    {
        var emailAddress = esdaDetails.ContactPointEmailAddress;
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            throw new InvalidOperationException(
                $"Unable to determine Data Share Request Notification Recipient for ESDA configured with empty Contact Point address.  '{esdaDetails.Id}'");
        }

        var recipientName = !string.IsNullOrWhiteSpace(esdaDetails.ContactPointName)
                            ? esdaDetails.ContactPointName
                            : PrettifyRecipientName(userOrganisationInformation.OrganisationName);

        return DoBuildDataShareRequestNotificationRecipient(
            emailAddress, recipientName);
    }

    private static IDataShareRequestNotificationRecipient BuildDetailsForEsdaCustomRecipient(
        IOrganisationInformation userOrganisationInformation,
        IEsdaDetails esdaDetails)
    {
        var emailAddress = esdaDetails.CustomDsrNotificationAddress;
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            throw new InvalidOperationException(
                $"Unable to determine Data Share Request Notification Recipient for ESDA configured with empty Custom Recipient.  '{esdaDetails.Id}'");
        }

        var recipientName = PrettifyRecipientName(userOrganisationInformation.OrganisationName);

        return DoBuildDataShareRequestNotificationRecipient(
            emailAddress, recipientName);
    }

    private IDataShareRequestNotificationRecipient BuildDetailsForCddoAdministrator()
    {
        var emailAddress = notificationsConfigurationPresenter.GetDataShareRequestNotificationCddoAdminEmailAddress();
        var recipientName = notificationsConfigurationPresenter.GetDataShareRequestNotificationCddoAdminName();

        return DoBuildDataShareRequestNotificationRecipient(
            emailAddress, recipientName);
    }


    private static IDataShareRequestNotificationRecipient DoBuildDataShareRequestNotificationRecipient(
        string emailAddress,
        string recipientName)
    {
        return new DataShareRequestNotificationRecipient
        {
            EmailAddress = emailAddress,
            RecipientName = recipientName
        };
    }

    private static string PrettifyRecipientName(
        string? recipientName)
    {
        if (string.IsNullOrWhiteSpace(recipientName)) return string.Empty;

        var inputTokens = recipientName.Split('-', StringSplitOptions.RemoveEmptyEntries);

        var prettyTokens = inputTokens.Select(token =>
        {
            var firstCharInUpperCase = char.ToUpper(token.First());

            var restOfToken = token.Length > 1 ? token.Substring(1) : string.Empty;

            return $"{firstCharInUpperCase}{restOfToken}";
        });

        return string.Join(" ", prettyTokens);
    }
}