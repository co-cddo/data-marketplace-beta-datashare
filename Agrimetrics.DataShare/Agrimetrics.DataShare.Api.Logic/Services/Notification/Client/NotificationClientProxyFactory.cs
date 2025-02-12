using Notify.Client;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Client;

internal class NotificationClientProxyFactory : INotificationClientProxyFactory
{
    INotificationClientProxy INotificationClientProxyFactory.Create(string govNotifyApiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(govNotifyApiKey);

        var notificationClient = new NotificationClient(govNotifyApiKey);

        return new NotificationClientProxy(notificationClient);
    }
}