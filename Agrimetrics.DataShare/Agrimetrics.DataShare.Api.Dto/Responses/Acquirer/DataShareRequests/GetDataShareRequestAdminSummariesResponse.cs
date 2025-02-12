using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Dto.Responses.Acquirer.DataShareRequests;

public class GetDataShareRequestAdminSummariesResponse
{
    public required DataShareRequestAdminSummarySet DataShareRequestAdminSummaries { get; init; }
}