using Agrimetrics.DataShare.Api.Dto.Models.Reporting;
using Agrimetrics.DataShare.Api.Dto.Responses.Reporting;

namespace Agrimetrics.DataShare.Api.Controllers.Reporting;

internal class ReportingResponseFactory : IReportingResponseFactory
{
    QueryDataShareRequestsCountsResponse IReportingResponseFactory.CreateQueryDataShareRequestsCountsResponse(
        IEnumerable<IDataShareRequestCount> dataShareRequestCountResults)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestCountResults);

        return new QueryDataShareRequestsCountsResponse
        {
            DataShareRequestCounts = dataShareRequestCountResults.OfType<DataShareRequestCount>().ToList()
        };
    }
}