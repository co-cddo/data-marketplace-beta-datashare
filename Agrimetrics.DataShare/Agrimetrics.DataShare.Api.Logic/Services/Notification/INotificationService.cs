namespace Agrimetrics.DataShare.Api.Logic.Services.Notification;

public interface INotificationService
{
    Task<INotificationSendResult> SendToSupplierNewDataShareRequestReceivedNotificationAsync(
        string supplierOrganisationEmailAddress,
        string supplierOrganisationName,
        string acquirerOrganisationName,
        string esdaName);

    Task<INotificationSendResult> SendToSupplierDataShareRequestCancelledNotificationAsync(
        string supplierOrganisationEmailAddress,
        string supplierOrganisationName,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId,
        string cancellationReasons);

    Task<INotificationSendResult> SendToAcquirerDataShareRequestAcceptedNotificationAsync(
        string acquirerUserEmailAddress,
        string supplierOrganisationEmailAddress,
        string supplierOrganisationName,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId);

    Task<INotificationSendResult> SendToAcquirerDataShareRequestRejectedNotificationAsync(
        string acquirerUserEmailAddress,
        string supplierOrganisationEmailAddress,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId,
        string reasonsForRejection);

    Task<INotificationSendResult> SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
        string acquirerUserEmailAddress,
        string supplierOrganisationEmailAddress,
        string acquirerUserName,
        string esdaName,
        string dataShareRequestRequestId);
}