using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions
{
    public class PendingSubmissionSummaryModelData
    {
        public Guid PendingSubmissionSummary_DataShareRequestId { get; set; }

        public string PendingSubmissionSummary_DataShareRequestRequestId { get; set; } = string.Empty;

        public int PendingSubmissionSummary_AcquirerOrganisationId { get; set; }

        public string PendingSubmissionSummary_AcquirerOrganisationName { get; set; } = string.Empty;

        public string PendingSubmissionSummary_EsdaName { get; set; } = string.Empty;

        public DateTime PendingSubmissionSummary_SubmittedOn { get; set; }

        public DateTime? PendingSubmissionSummary_WhenNeededBy { get; set; }

        public DataShareRequestStatusType PendingSubmissionSummary_RequestStatus { get; set; }
    }
}
