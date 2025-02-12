namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

internal class DataShareRequestQuestionStatusesDeterminationResult : IDataShareRequestQuestionStatusesDeterminationResult
{
    public required bool QuestionsRemainThatRequireAResponse { get; init; }

    public required IEnumerable<IDataShareRequestQuestionStatusDeterminationResult> QuestionStatusDeterminationResults { get; init; }
}