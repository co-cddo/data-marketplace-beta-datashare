using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;

namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class CompletedSubmissionInformation
{
    public Guid DataShareRequestId { get; set; }

    public string DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatus RequestStatus { get; set; }

    public SubmissionDecision Decision { get; set; }

    public string AcquirerUserEmail { get; set; } = string.Empty;

    public string AcquirerOrganisationName { get; set; } = string.Empty;

    public string EsdaName { get; set; } = string.Empty;

    public DateTime SubmittedOn { get; set; }

    public DateTime CompletedOn { get; set; }

    public DateTime? WhenNeededBy { get; set; }

    public string SupplierNotes { get; set; } = string.Empty;

    public string FeedbackProvided { get; set; } = string.Empty;
}