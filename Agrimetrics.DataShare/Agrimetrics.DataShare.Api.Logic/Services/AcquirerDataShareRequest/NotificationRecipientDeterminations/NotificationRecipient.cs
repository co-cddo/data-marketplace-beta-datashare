namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestNotificationRecipientDeterminations;

internal class DataShareRequestNotificationRecipient : IDataShareRequestNotificationRecipient
{
    public required string EmailAddress { get; init; }

    public required string RecipientName { get; init; }
}