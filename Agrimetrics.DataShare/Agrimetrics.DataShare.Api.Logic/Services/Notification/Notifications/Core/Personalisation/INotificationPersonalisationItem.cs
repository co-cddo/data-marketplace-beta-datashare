namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core.Personalisation;

public interface INotificationPersonalisationItem
{
    string FieldName { get; }

    dynamic Value { get; }
}