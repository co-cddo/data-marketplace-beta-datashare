namespace Agrimetrics.DataShare.Api.Dto.Models.Reporting;

public class DataShareRequestCount : IDataShareRequestCount
{
    public required DataShareRequestCountQuery DataShareRequestCountQuery { get; set; }

    public required int NumberOfDataShareRequests { get; set; }
}