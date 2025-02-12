namespace Agrimetrics.DataShare.Api.Dto.Models.Reporting;

public interface IDataShareRequestCount
{
    DataShareRequestCountQuery DataShareRequestCountQuery { get; }

    int NumberOfDataShareRequests { get; }
}