using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;

public interface IAcquirerDataShareRequestRejectedNotification : IEmailNotification
{
    string AcquirerUserEmailAddress { get; }

    string SupplierOrganisationEmailAddress { get; }

    string AcquirerUserName { get; }

    string EsdaName { get; }

    string DataShareRequestRequestId { get; }

    string ReasonsForRejection { get; }
}