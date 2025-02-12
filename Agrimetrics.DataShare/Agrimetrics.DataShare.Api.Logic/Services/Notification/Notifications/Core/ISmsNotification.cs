namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;

public interface ISmsNotification : INotification
{
    string MobileNumber { get; }
}