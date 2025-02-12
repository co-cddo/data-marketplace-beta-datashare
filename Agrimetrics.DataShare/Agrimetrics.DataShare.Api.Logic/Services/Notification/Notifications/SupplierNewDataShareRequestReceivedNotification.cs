using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core.Personalisation;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;

internal class SupplierNewDataShareRequestReceivedNotification(
    INotificationConfiguration notificationConfiguration) : ISupplierNewDataShareRequestReceivedNotification
{
    public Guid TemplateId => notificationConfiguration.TemplateIdSet.SupplierNewDataShareRequestReceivedId;

    public INotificationPersonalisation Personalisation => new NotificationPersonalisation
    {
        PersonalisationItems =
        [
            new NotificationPersonalisationItem { FieldName = "acquirer-organisation", Value = AcquirerOrganisationName},
            new NotificationPersonalisationItem { FieldName = "supplier-name", Value = SupplierOrganisationName},
            new NotificationPersonalisationItem { FieldName = "resource-name", Value = EsdaName},
            new NotificationPersonalisationItem { FieldName = "sign-in", Value = $"[sign in]({DataMarketPlaceSignInAddress})"}
        ]
    };

    public string RecipientEmailAddress => SupplierOrganisationEmailAddress;

    public required string SupplierOrganisationEmailAddress { get; init; }

    public required string SupplierOrganisationName { get; init; }

    public required string AcquirerOrganisationName { get; init; }

    public required string EsdaName { get; init; }

    public required string DataMarketPlaceSignInAddress { get; init; }
}