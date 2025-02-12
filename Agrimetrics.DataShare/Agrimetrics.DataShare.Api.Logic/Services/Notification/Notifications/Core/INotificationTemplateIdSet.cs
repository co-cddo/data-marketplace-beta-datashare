namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;

public interface INotificationTemplateIdSet
{
    Guid SupplierNewDataShareRequestReceivedId { get; }

    Guid SupplierDataShareRequestCancelledId { get; }

    Guid AcquirerDataShareRequestAcceptedId { get; }

    Guid AcquirerDataShareRequestRejectedId { get; }

    Guid AcquirerDataShareRequestReturnedWithCommentsId { get; }
}