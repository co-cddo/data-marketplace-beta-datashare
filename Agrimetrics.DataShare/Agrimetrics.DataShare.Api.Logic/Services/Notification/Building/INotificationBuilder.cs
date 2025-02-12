using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Building;

public interface INotificationBuilder
{
    ISupplierNewDataShareRequestReceivedNotification BuildSupplierNewDataShareRequestReceivedNotification(
        string supplierOrganisationEmailAddress,
        string supplierOrganisationName,
        string acquirerOrganisationName,
        string esdaName);

    ISupplierDataShareRequestCancelledNotification BuildSupplierDataShareRequestCancelledNotification(
        string supplierOrganisationEmailAddress,
        string supplierOrganisationName,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId,
        string cancellationReasons);

    IAcquirerDataShareRequestAcceptedNotification BuildAcquirerDataShareRequestAcceptedNotification(
        string acquirerUserEmailAddress,
        string supplierOrganisationEmailAddress,
        string supplierOrganisationName,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId);

    IAcquirerDataShareRequestRejectedNotification BuildAcquirerDataShareRequestRejectedNotification(
        string acquirerUserEmailAddress,
        string supplierOrganisationEmailAddress,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId,
        string reasonsForRejection);

    IAcquirerDataShareRequestReturnedWithCommentsNotification BuildAcquirerDataShareRequestReturnedWithCommentsNotification(
        string acquirerUserEmailAddress,
        string supplierOrganisationEmailAddress,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId);
}