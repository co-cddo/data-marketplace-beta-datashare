using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class ReturnedDecisionSummaryModelData
{
    public Guid ReturnedDecisionSummary_DataShareRequestId { get; set; }

    public string ReturnedDecisionSummary_DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatusType ReturnedDecisionSummary_RequestStatus { get; set; }

    public int ReturnedDecisionSummary_AcquirerOrganisationId { get; set; }

    public string ReturnedDecisionSummary_AcquirerOrganisationName { get; set; } = string.Empty;
}