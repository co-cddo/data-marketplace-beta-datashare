using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;

namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class DataShareRequestReturnResult
{
    public Guid DataShareRequestId { get; set; }

    public ReturnedDecisionSummary ReturnedDecisionSummary { get; set; }

    public NotificationSuccess NotificationSuccess { get; set; }
}