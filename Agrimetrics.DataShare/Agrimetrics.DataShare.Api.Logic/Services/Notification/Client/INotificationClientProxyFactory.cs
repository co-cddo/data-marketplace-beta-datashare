namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Client;

public interface INotificationClientProxyFactory
{
    INotificationClientProxy Create(string govNotifyApiKey);
}