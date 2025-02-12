using Agrimetrics.DataShare.Api.Dto.Models.Reporting;
using Agrimetrics.DataShare.Api.Dto.Responses.Reporting;

namespace Agrimetrics.DataShare.Api.Controllers.Reporting;

public interface IReportingResponseFactory
{
    QueryDataShareRequestsCountsResponse CreateQueryDataShareRequestsCountsResponse(
        IEnumerable<IDataShareRequestCount> dataShareRequestCountResults);
}