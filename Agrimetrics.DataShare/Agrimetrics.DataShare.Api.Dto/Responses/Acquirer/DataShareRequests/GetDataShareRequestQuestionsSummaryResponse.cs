using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;

namespace Agrimetrics.DataShare.Api.Dto.Responses.Acquirer.DataShareRequests;

public class GetDataShareRequestQuestionsSummaryResponse
{
    public required Guid DataShareRequestId { get; init; }

    public required string DataShareRequestRequestId { get; init; }

    public required string EsdaName { get; init; }

    public required QuestionSetSummary QuestionSetSummary { get; init; }
}