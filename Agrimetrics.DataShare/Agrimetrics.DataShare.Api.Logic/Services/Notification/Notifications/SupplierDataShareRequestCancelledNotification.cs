using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core.Personalisation;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;

internal class SupplierDataShareRequestCancelledNotification(
    INotificationConfiguration notificationConfiguration) : ISupplierDataShareRequestCancelledNotification
{
    public Guid TemplateId => notificationConfiguration.TemplateIdSet.SupplierDataShareRequestCancelledId;

    public INotificationPersonalisation Personalisation => new NotificationPersonalisation
    {
        PersonalisationItems =
        [
            new NotificationPersonalisationItem { FieldName = "acquirer-name", Value = AcquirerUserName},
            new NotificationPersonalisationItem { FieldName = "supplier-name", Value = SupplierOrganisationName},
            new NotificationPersonalisationItem { FieldName = "resource-name", Value = EsdaName},
            new NotificationPersonalisationItem { FieldName = "request-id", Value = DataShareRequestRequestId},
            new NotificationPersonalisationItem { FieldName = "cancellation-reason", Value = CancellationReasons}
        ]
    };

    public string RecipientEmailAddress => SupplierOrganisationEmailAddress;

    public required string SupplierOrganisationEmailAddress { get; init; }

    public required string SupplierOrganisationName { get; init; }

    public required string AcquirerUserName { get; init; }

    public required string EsdaName { get; init; }

    public required string DataShareRequestRequestId { get; init; }

    public required string CancellationReasons { get; init; }
}