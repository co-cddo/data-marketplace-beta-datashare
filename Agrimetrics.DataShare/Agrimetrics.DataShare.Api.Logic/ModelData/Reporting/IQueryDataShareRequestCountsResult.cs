using Agrimetrics.DataShare.Api.Dto.Models.Reporting;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;

public interface IQueryDataShareRequestCountsResult
{
    IEnumerable<IDataShareRequestCount> DataShareRequestCounts { get; }
}