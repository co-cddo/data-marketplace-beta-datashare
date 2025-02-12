using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class ReturnedSubmissionInformationModelData
{
    public Guid ReturnedSubmission_DataShareRequestId { get; set; }

    public string ReturnedSubmission_DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatusType ReturnedSubmission_RequestStatus { get; set; }

    public int ReturnedSubmission_AcquirerOrganisationId { get; set; }

    public string ReturnedSubmission_AcquirerOrganisationName { get; set; } = string.Empty;

    public string ReturnedSubmission_EsdaName { get; set; } = string.Empty;

    public DateTime ReturnedSubmission_SubmittedOn { get; set; }

    public DateTime ReturnedSubmission_ReturnedOn { get; set; }

    public DateTime? ReturnedSubmission_WhenNeededBy { get; set; }

    public string ReturnedSubmission_SupplierNotes { get; set; } = string.Empty;

    public string ReturnedSubmission_FeedbackProvided { get; set; } = string.Empty;
}