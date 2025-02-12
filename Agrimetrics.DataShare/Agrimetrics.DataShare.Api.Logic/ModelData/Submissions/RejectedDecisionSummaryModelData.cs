using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class RejectedDecisionSummaryModelData
{
    public Guid RejectedDecisionSummary_DataShareRequestId { get; set; }

    public string RejectedDecisionSummary_DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatusType RejectedDecisionSummary_RequestStatus { get; set; }

    public int RejectedDecisionSummary_AcquirerOrganisationId { get; set; }

    public string RejectedDecisionSummary_AcquirerOrganisationName { get; set; } = string.Empty;
}