using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class AcceptedDecisionSummaryModelData
{
    public Guid AcceptedDecisionSummary_DataShareRequestId { get; set; }

    public string AcceptedDecisionSummary_DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatusType AcceptedDecisionSummary_RequestStatus { get; set; }

    public int AcceptedDecisionSummary_AcquirerUserId { get; set; }

    public string AcceptedDecisionSummary_AcquirerUserEmailAddress { get; set; } = string.Empty;

    public int AcceptedDecisionSummary_AcquirerOrganisationId { get; set; }

    public string AcceptedDecisionSummary_AcquirerOrganisationName { get; set; } = string.Empty;
}