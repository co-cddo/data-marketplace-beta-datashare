using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Client;

public interface INotificationClientProxy
{
    Task SendEmailAsync(IEmailNotification emailNotification);

    Task SendSmsAsync(ISmsNotification smsNotification);
}