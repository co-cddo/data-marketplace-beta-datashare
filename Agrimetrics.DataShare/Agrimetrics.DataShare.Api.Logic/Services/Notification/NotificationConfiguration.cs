using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification;

class NotificationConfiguration(
    INotificationTemplateIdSet notificationTemplateIdSet,
    INotificationsConfigurationPresenter notificationsConfigurationPresenter) : INotificationConfiguration
{
    public string GovNotifyApiKey => notificationsConfigurationPresenter.GetGovNotifyApiKey();

    public INotificationTemplateIdSet TemplateIdSet => notificationTemplateIdSet;
}