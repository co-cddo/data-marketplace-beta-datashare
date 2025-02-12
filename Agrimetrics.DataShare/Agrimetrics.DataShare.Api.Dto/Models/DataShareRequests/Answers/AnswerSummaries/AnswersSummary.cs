﻿namespace Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries
{
    public class DataShareRequestAnswersSummary
    {
        public Guid DataShareRequestId { get; set; }

        public string RequestId { get; set; } = string.Empty;

        public string EsdaName { get; set; } = string.Empty;

        public DataShareRequestStatus DataShareRequestStatus { get; set; }

        public bool QuestionsRemainThatRequireAResponse { get; set; }

        public List<DataShareRequestAnswersSummarySection> SummarySections { get; set; } = [];

        public string? SubmissionResponseFromSupplier { get; set; }

        public string? CancellationReasonsFromAcquirer { get; set; }
    }
}
