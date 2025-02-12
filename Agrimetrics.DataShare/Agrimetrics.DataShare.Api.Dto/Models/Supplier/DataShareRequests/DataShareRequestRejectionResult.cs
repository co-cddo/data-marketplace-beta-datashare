using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;

namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class DataShareRequestRejectionResult
{
    public Guid DataShareRequestId { get; set; }

    public RejectedDecisionSummary RejectedDecisionSummary { get; set; }

    public NotificationSuccess NotificationSuccess { get; set; }
}