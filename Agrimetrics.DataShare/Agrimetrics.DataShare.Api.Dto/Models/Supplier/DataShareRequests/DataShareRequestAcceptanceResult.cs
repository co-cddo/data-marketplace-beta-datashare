using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;

namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class DataShareRequestAcceptanceResult
{
    public Guid DataShareRequestId { get; set; }

    public AcceptedDecisionSummary AcceptedDecisionSummary { get; set; }

    public NotificationSuccess NotificationSuccess { get; set; }
}