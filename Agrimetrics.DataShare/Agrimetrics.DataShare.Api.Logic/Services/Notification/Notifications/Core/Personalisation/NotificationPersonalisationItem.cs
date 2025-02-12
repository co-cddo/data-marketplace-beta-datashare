namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core.Personalisation;

internal class NotificationPersonalisationItem : INotificationPersonalisationItem
{
    public required string FieldName { get; init; }

    public required dynamic Value { get; init; }
}