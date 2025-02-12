using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core.Personalisation;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;

public interface INotification
{
    Guid TemplateId { get; }

    INotificationPersonalisation? Personalisation { get; }
}