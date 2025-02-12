using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;


public class SubmissionDetailsModelData
{
    public Guid SubmissionDetails_DataShareRequestId { get; set; }

    public string SubmissionDetails_DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatusType SubmissionDetails_RequestStatus { get; set; }

    public string SubmissionDetails_EsdaName { get; set; } = string.Empty;

    public int SubmissionDetails_AcquirerOrganisationId { get; set; }

    public string SubmissionDetails_AcquirerOrganisationName { get; set; } = string.Empty;

    public List<SubmissionDetailsSectionModelData> SubmissionDetails_Sections { get; set; } = [];

    public List<SubmissionReturnCommentsModelData> SubmissionDetails_SubmissionReturnComments { get; set; } = [];
}