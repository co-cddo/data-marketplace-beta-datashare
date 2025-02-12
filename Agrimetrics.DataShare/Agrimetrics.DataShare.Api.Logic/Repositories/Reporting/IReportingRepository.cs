using Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.Reporting;

public interface IReportingRepository
{
    Task<IEnumerable<ReportingDataShareRequestInformationModelData>> GetAllReportingDataShareRequestInformationAsync();
}