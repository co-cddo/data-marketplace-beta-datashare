using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class SubmissionInformationModelData
{
    public Guid SubmissionInformation_DataShareRequestId { get; set; }

    public string SubmissionInformation_DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatusType SubmissionInformation_RequestStatus { get; set; }

    public string SubmissionInformation_EsdaName { get; set; } = string.Empty;

    public int SubmissionInformation_AcquirerUserId { get; set; }

    public int SubmissionInformation_AcquirerOrganisationId { get; set; }

    public string SubmissionInformation_AcquirerOrganisationName { get; set; } = string.Empty;

    public List<string> SubmissionInformation_DataTypes { get; set; } = [];

    public string SubmissionInformation_ProjectAims { get; set; } = string.Empty;

    public DateTime? SubmissionInformation_WhenNeededBy { get; set; }

    public DateTime SubmissionInformation_SubmittedOn { get; set; }

    public string SubmissionInformation_AcquirerEmailAddress { get; set; } = string.Empty;

    public List<string> SubmissionInformation_AnswerHighlights { get; set; } = [];
}