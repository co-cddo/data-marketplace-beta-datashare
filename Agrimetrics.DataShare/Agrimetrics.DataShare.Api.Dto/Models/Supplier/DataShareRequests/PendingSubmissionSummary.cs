using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class PendingSubmissionSummary
{
    public Guid DataShareRequestId { get; set; }

    public string DataShareRequestRequestId { get; set; } = string.Empty;

    public string AcquirerOrganisationName { get; set; } = string.Empty;

    public string EsdaName { get; set; } = string.Empty;

    public DateTime SubmittedOn { get; set; }

    public DateTime? WhenNeededBy { get; set; }

    public DataShareRequestStatus RequestStatus { get; set; }
}