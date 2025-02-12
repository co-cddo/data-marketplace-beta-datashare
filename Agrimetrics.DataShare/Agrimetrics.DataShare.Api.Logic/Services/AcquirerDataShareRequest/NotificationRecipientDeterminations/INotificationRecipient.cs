namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestNotificationRecipientDeterminations;

public interface IDataShareRequestNotificationRecipient
{
    string EmailAddress { get; }

    string RecipientName { get; }
}