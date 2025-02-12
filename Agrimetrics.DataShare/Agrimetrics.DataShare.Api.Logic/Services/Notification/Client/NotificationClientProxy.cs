using System.Diagnostics.CodeAnalysis;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core.Personalisation;
using Notify.Client;

namespace Agrimetrics.DataShare.Api.Logic.Services.Notification.Client;

[ExcludeFromCodeCoverage] // Justification - Proxy class to third party entity that cannot be reliably unit tested
internal class NotificationClientProxy(
    NotificationClient notificationClient) : INotificationClientProxy
{
    async Task INotificationClientProxy.SendEmailAsync(IEmailNotification emailNotification)
    {
        ArgumentNullException.ThrowIfNull(emailNotification);

        var personalisationData = FlattenPersonalisationData(emailNotification.Personalisation);

        await notificationClient.SendEmailAsync(
            emailNotification.RecipientEmailAddress,
            emailNotification.TemplateId.ToString(),
            personalisation: personalisationData);
    }

    async Task INotificationClientProxy.SendSmsAsync(ISmsNotification smsNotification)
    {
        ArgumentNullException.ThrowIfNull(smsNotification);

        var personalisationData = FlattenPersonalisationData(smsNotification.Personalisation);

        await notificationClient.SendSmsAsync(
            smsNotification.MobileNumber,
            smsNotification.TemplateId.ToString(),
            personalisation: personalisationData);
    }

    private static Dictionary<string, dynamic>? FlattenPersonalisationData(INotificationPersonalisation? notificationPersonalisation)
    {
        return notificationPersonalisation?.PersonalisationItems
            .ToDictionary(x => x.FieldName, x => x.Value);
    }
}