namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core.Personalisation;

public interface INotificationPersonalisation
{
    IEnumerable<INotificationPersonalisationItem> PersonalisationItems { get; }
}