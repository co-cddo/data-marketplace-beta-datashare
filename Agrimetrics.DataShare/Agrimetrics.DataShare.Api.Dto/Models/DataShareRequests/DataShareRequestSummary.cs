namespace Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;

public class DataShareRequestSummary
{
    public Guid Id { get; set; }

    public string RequestId { get; set; } = string.Empty;

    public string EsdaName { get; set; } = string.Empty;

    public DataShareRequestStatus DataShareRequestStatus { get; set; }
}