using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class CompletedSubmissionInformationModelData
{
    public Guid CompletedSubmission_DataShareRequestId { get; set; }

    public string CompletedSubmission_DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatusType CompletedSubmission_DataShareRequestStatus { get; set; }

    public SubmissionDecisionType CompletedSubmission_Decision { get; set; }

    public int CompletedSubmission_AcquirerUserId { get; set; }

    public string CompletedSubmission_AcquirerUserEmailAddress { get; set; } = string.Empty;

    public int CompletedSubmission_AcquirerOrganisationId { get; set; }

    public string CompletedSubmission_AcquirerOrganisationName { get; set; } = string.Empty;

    public string CompletedSubmission_EsdaName { get; set; } = string.Empty;

    public DateTime CompletedSubmission_SubmittedOn { get; set; }

    public DateTime CompletedSubmission_CompletedOn { get; set; }

    public DateTime? CompletedSubmission_WhenNeededBy { get; set; }

    public string CompletedSubmission_SupplierNotes { get; set; }
    
    public string CompletedSubmission_FeedbackProvided { get; set; }
}