using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Building;

internal class NotificationBuilder(
    INotificationConfiguration notificationConfiguration,
    IPageLinksConfigurationPresenter pageLinksConfigurationPresenter) : INotificationBuilder
{
    ISupplierNewDataShareRequestReceivedNotification INotificationBuilder.BuildSupplierNewDataShareRequestReceivedNotification(
        string supplierOrganisationEmailAddress,
        string supplierOrganisationName,
        string acquirerOrganisationName,
        string esdaName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierOrganisationEmailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierOrganisationName);
        ArgumentException.ThrowIfNullOrWhiteSpace(acquirerOrganisationName);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);

        return new SupplierNewDataShareRequestReceivedNotification(notificationConfiguration)
        {
            SupplierOrganisationEmailAddress = supplierOrganisationEmailAddress,
            SupplierOrganisationName = supplierOrganisationName,
            AcquirerOrganisationName = acquirerOrganisationName,
            EsdaName = esdaName,
            DataMarketPlaceSignInAddress = pageLinksConfigurationPresenter.GetDataMarketPlaceSignInAddress()
        };
    }

    ISupplierDataShareRequestCancelledNotification INotificationBuilder.BuildSupplierDataShareRequestCancelledNotification(
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

        return new SupplierDataShareRequestCancelledNotification(notificationConfiguration)
        {
            SupplierOrganisationEmailAddress = supplierOrganisationEmailAddress,
            SupplierOrganisationName = supplierOrganisationName,
            AcquirerUserName = acquirerUserName,
            EsdaName = esdaName,
            DataShareRequestRequestId = dataShareRequestRequestId,
            CancellationReasons = cancellationReasons
        };
    }

    IAcquirerDataShareRequestAcceptedNotification INotificationBuilder.BuildAcquirerDataShareRequestAcceptedNotification(
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

        return new AcquirerDataShareRequestAcceptedNotification(notificationConfiguration)
        {
            AcquirerUserEmailAddress = acquirerUserEmailAddress,
            SupplierOrganisationEmailAddress = supplierOrganisationEmailAddress,
            SupplierOrganisationName = supplierOrganisationName,
            AcquirerUserName = acquirerUserName,
            EsdaName = esdaName,
            DataShareRequestRequestId = dataShareRequestRequestId
        };
    }

    IAcquirerDataShareRequestRejectedNotification INotificationBuilder.BuildAcquirerDataShareRequestRejectedNotification(
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

        return new AcquirerDataShareRequestRejectedNotification(notificationConfiguration)
        {
            AcquirerUserEmailAddress = acquirerUserEmailAddress,
            SupplierOrganisationEmailAddress = supplierOrganisationEmailAddress,
            AcquirerUserName = acquirerUserName,
            EsdaName = esdaName,
            DataShareRequestRequestId = dataShareRequestRequestId,
            ReasonsForRejection = reasonsForRejection
        };
    }

    IAcquirerDataShareRequestReturnedWithCommentsNotification INotificationBuilder.BuildAcquirerDataShareRequestReturnedWithCommentsNotification(
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

        return new AcquirerDataShareRequestReturnedWithCommentsNotification(notificationConfiguration)
        {
            AcquirerUserEmailAddress = acquirerUserEmailAddress,
            SupplierOrganisationEmailAddress = supplierOrganisationEmailAddress,
            AcquirerUserName = acquirerUserName,
            EsdaName = esdaName,
            DataShareRequestRequestId = dataShareRequestRequestId
        };
    }
}
