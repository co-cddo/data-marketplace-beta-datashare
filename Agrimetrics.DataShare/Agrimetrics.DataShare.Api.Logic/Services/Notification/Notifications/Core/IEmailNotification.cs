namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;

public interface IEmailNotification : INotification
{
    string RecipientEmailAddress { get; }
}