using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;

namespace Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;

public class DataShareRequestSubmissionResult
{
    public Guid DataShareRequestId { get; set; }

    public string DataShareRequestRequestId { get; set; } = string.Empty;

    public NotificationSuccess NotificationSuccess { get; set; }
}