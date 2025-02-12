using Agrimetrics.DataShare.Api.Dto.Models.Reporting;

namespace Agrimetrics.DataShare.Api.Dto.Responses.Reporting;

public class QueryDataShareRequestsCountsResponse
{
    public List<DataShareRequestCount> DataShareRequestCounts { get; set; } = [];
}