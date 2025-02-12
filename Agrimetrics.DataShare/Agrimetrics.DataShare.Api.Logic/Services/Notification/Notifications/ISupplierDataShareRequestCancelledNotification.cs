using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;

public interface ISupplierDataShareRequestCancelledNotification : IEmailNotification
{
    string SupplierOrganisationEmailAddress { get; }

    string SupplierOrganisationName { get; }

    string AcquirerUserName { get; }

    string EsdaName { get; }

    string DataShareRequestRequestId { get; }

    string CancellationReasons { get; }
}