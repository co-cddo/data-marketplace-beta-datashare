using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;

namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class CompletedSubmissionSummary
{
    public Guid DataShareRequestId { get; set; }

    public string DataShareRequestRequestId { get; set; } = string.Empty;

    public string AcquirerOrganisationName { get; set; } = string.Empty;

    public string EsdaName { get; set; } = string.Empty;

    public DateTime SubmittedOn { get; set; }

    public DateTime CompletedOn { get; set; }

    public SubmissionDecision Decision { get; set; }
}