using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Building;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Client;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification;

public class NotificationService(
    ILogger<NotificationService> logger,
    INotificationsConfigurationPresenter notificationsConfigurationPresenter,
    INotificationClientProxyFactory notificationClientProxyFactory,
    INotificationBuilder notificationBuilder) : INotificationService
{
    async Task<INotificationSendResult> INotificationService.SendToSupplierNewDataShareRequestReceivedNotificationAsync(
        string supplierOrganisationEmailAddress,
        string supplierOrganisationName,
        string acquirerOrganisationName,
        string esdaName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierOrganisationEmailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierOrganisationName);
        ArgumentException.ThrowIfNullOrWhiteSpace(acquirerOrganisationName);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);
        
        return await DoSendEmailNotificationAsync("New Data Share Request Received", "Supplier", () =>
            notificationBuilder.BuildSupplierNewDataShareRequestReceivedNotification(
                supplierOrganisationEmailAddress,
                supplierOrganisationName,
                acquirerOrganisationName,
                esdaName));
    }

    async Task<INotificationSendResult> INotificationService.SendToSupplierDataShareRequestCancelledNotificationAsync(
        string supplierOrganisationEmailAddress,
        string supplierOrganisationName,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId,
        string cancellationReasons)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierOrganisationEmailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierOrganisationName);
        ArgumentException.ThrowIfNullOrWhiteSpace(acquirerUserName);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);
        ArgumentException.ThrowIfNullOrWhiteSpace(dataShareRequestRequestId);
        ArgumentNullException.ThrowIfNull(cancellationReasons);

        
        return await DoSendEmailNotificationAsync("Data Share Request Cancelled", "Supplier", () =>
            notificationBuilder.BuildSupplierDataShareRequestCancelledNotification(
                supplierOrganisationEmailAddress,
                supplierOrganisationName,
                acquirerUserName,
                esdaName,
                dataShareRequestRequestId,
                cancellationReasons));
    }

    async Task<INotificationSendResult> INotificationService.SendToAcquirerDataShareRequestAcceptedNotificationAsync(
        string acquirerUserEmailAddress,
        string supplierOrganisationEmailAddress,
        string supplierOrganisationName,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(acquirerUserEmailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierOrganisationEmailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierOrganisationName);
        ArgumentException.ThrowIfNullOrWhiteSpace(acquirerUserName);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);
        ArgumentException.ThrowIfNullOrWhiteSpace(dataShareRequestRequestId);
        
        return await DoSendEmailNotificationAsync("Data Share Request Accepted", "Acquirer", () =>
            notificationBuilder.BuildAcquirerDataShareRequestAcceptedNotification(
                acquirerUserEmailAddress,
                supplierOrganisationEmailAddress,
                supplierOrganisationName,
                acquirerUserName,
                esdaName,
                dataShareRequestRequestId));
    }

    async Task<INotificationSendResult> INotificationService.SendToAcquirerDataShareRequestRejectedNotificationAsync(
        string acquirerUserEmailAddress,
        string supplierOrganisationEmailAddress,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId,
        string reasonsForRejection)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(acquirerUserEmailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierOrganisationEmailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(acquirerUserName);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);
        ArgumentException.ThrowIfNullOrWhiteSpace(dataShareRequestRequestId);
        ArgumentNullException.ThrowIfNull(reasonsForRejection);

        return await DoSendEmailNotificationAsync("Data Share Request Rejected", "Acquirer", () =>
            notificationBuilder.BuildAcquirerDataShareRequestRejectedNotification(
                acquirerUserEmailAddress,
                supplierOrganisationEmailAddress,
                acquirerUserName,
                esdaName,
                dataShareRequestRequestId,
                reasonsForRejection));
    }

    async Task<INotificationSendResult> INotificationService.SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
        string acquirerUserEmailAddress,
        string supplierOrganisationEmailAddress,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(acquirerUserEmailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierOrganisationEmailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(acquirerUserName);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);
        ArgumentException.ThrowIfNullOrWhiteSpace(dataShareRequestRequestId);

        return await DoSendEmailNotificationAsync("Data Share Request Returned With Comments", "Acquirer", () =>
            notificationBuilder.BuildAcquirerDataShareRequestReturnedWithCommentsNotification(
                acquirerUserEmailAddress,
                supplierOrganisationEmailAddress,
                acquirerUserName,
                esdaName,
                dataShareRequestRequestId));
    }

    private async Task<INotificationSendResult> DoSendEmailNotificationAsync(
        string notificationDescription,
        string recipientRoleDescription,
        Func<IEmailNotification> notificationBuildFunc)
    {
        try
        {
            logger.LogTrace("Sending {NotificationDescription} notification", notificationDescription);

            var notification = notificationBuildFunc();

            var notificationClient = DoCreateNotificationClient();

            await notificationClient.SendEmailAsync(notification);

            logger.LogTrace("{NotificationDescription} notification sent successfully to {RecipientRoleDescription} '{RecipientEmailAddress}'",
                notificationDescription, recipientRoleDescription, notification.RecipientEmailAddress);

            return BuildNotificationSendResult(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception thrown sending {NotificationDescription} notification", notificationDescription);

            return BuildNotificationSendResult(false);
        }

        INotificationSendResult BuildNotificationSendResult(bool success)
        {
            return new NotificationSendResult {Success = success};
        }
    }

    private INotificationClientProxy DoCreateNotificationClient()
    {
        var govNotifyApiKey = notificationsConfigurationPresenter.GetGovNotifyApiKey();

        var loggedKey = GetKeyToLog();

        logger.LogDebug("Creating Gov Notify client with key starting with key '{LoggedKey}'", loggedKey);

        return notificationClientProxyFactory.Create(govNotifyApiKey);

        string GetKeyToLog()
        {
            if (string.IsNullOrWhiteSpace(govNotifyApiKey)) return "no key found";

            var loggedKeyLength = Math.Min(govNotifyApiKey.Length, 16);
            return govNotifyApiKey[..loggedKeyLength];
        }
    }
}