using Agrimetrics.DataShare.Api.Dto.Models.Reporting;
using Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;

namespace Agrimetrics.DataShare.Api.Logic.Services.Reporting;

public interface IReportingService
{
    Task<IServiceOperationDataResult<IQueryDataShareRequestCountsResult>> QueryDataShareRequestCountsAsync(
        IEnumerable<IDataShareRequestCountQuery> dataShareRequestCountQueries);
}