namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core.Personalisation;

internal class NotificationPersonalisation : INotificationPersonalisation
{
    public required IEnumerable<INotificationPersonalisationItem> PersonalisationItems { get; init; }
}