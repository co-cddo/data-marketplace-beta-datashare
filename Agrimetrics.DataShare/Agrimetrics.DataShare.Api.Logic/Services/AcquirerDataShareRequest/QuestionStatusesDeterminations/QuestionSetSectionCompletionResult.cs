using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

internal class DataShareRequestQuestionSetSectionCompletionDeterminationResult : IDataShareRequestQuestionSetSectionCompletionDeterminationResult
{
    public required IEnumerable<QuestionSummaryModelData> QuestionsRequiringAResponse { get; init; }
}