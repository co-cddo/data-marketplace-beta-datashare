using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification;

public interface INotificationConfiguration
{
    string GovNotifyApiKey { get; }

    INotificationTemplateIdSet TemplateIdSet { get; }
}