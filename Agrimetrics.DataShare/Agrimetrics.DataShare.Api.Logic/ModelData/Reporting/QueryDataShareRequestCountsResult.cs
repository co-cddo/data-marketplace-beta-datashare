using Agrimetrics.DataShare.Api.Dto.Models.Reporting;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;

internal class QueryDataShareRequestCountsResult : IQueryDataShareRequestCountsResult
{
    public required IEnumerable<IDataShareRequestCount> DataShareRequestCounts { get; init; }
}