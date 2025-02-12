using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

public interface IDataShareRequestQuestionSetSectionCompletionDeterminationResult
{
    IEnumerable<QuestionSummaryModelData> QuestionsRequiringAResponse { get; }
}