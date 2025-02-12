using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class CompletedSubmissionSummaryModelData
{
    public Guid CompletedSubmissionSummary_DataShareRequestId { get; set; }

    public string CompletedSubmissionSummary_DataShareRequestRequestId { get; set; } = string.Empty;

    public int CompletedSubmissionSummary_AcquirerOrganisationId { get; set; }

    public string CompletedSubmissionSummary_AcquirerOrganisationName { get; set; } = string.Empty;

    public string CompletedSubmissionSummary_EsdaName { get; set; } = string.Empty;

    public DataShareRequestStatusType CompletedSubmissionSummary_Status { get; set; }

    public SubmissionDecisionType CompletedSubmissionSummary_Decision { get; set; }

    public DateTime CompletedSubmissionSummary_SubmittedOn { get; set; }

    public DateTime CompletedSubmissionSummary_CompletedOn { get; set; }
}