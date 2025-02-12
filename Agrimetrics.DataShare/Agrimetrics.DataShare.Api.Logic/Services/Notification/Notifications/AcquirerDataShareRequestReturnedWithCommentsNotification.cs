using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core.Personalisation;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;

internal class AcquirerDataShareRequestReturnedWithCommentsNotification(
    INotificationConfiguration notificationConfiguration) : IAcquirerDataShareRequestReturnedWithCommentsNotification
{
    public Guid TemplateId => notificationConfiguration.TemplateIdSet.AcquirerDataShareRequestReturnedWithCommentsId;

    public INotificationPersonalisation Personalisation => new NotificationPersonalisation
    {
        PersonalisationItems =
        [
            new NotificationPersonalisationItem { FieldName = "acquirer-name", Value = AcquirerUserName},
            new NotificationPersonalisationItem { FieldName = "dataset-title", Value = EsdaName},
            new NotificationPersonalisationItem { FieldName = "request-ID", Value = DataShareRequestRequestId},
            new NotificationPersonalisationItem { FieldName = "supplier-email", Value = SupplierOrganisationEmailAddress}
        ]
    };

    public string RecipientEmailAddress => AcquirerUserEmailAddress;

    public required string AcquirerUserEmailAddress { get; init; }

    public required string SupplierOrganisationEmailAddress { get; init; }

    public required string AcquirerUserName { get; init; }

    public required string EsdaName { get; init; }

    public required string DataShareRequestRequestId { get; init; }
}