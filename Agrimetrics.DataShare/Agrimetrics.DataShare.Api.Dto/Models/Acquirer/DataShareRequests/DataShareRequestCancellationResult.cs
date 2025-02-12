using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;

namespace Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;

public class DataShareRequestCancellationResult
{
    public Guid DataShareRequestId { get; set; }

    public string ReasonsForCancellation { get; set; }

    public NotificationSuccess NotificationSuccess { get; set; }
}