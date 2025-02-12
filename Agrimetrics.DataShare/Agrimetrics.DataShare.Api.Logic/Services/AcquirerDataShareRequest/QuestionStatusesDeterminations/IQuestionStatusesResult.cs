namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

public interface IDataShareRequestQuestionStatusesDeterminationResult
{
    bool QuestionsRemainThatRequireAResponse { get; }

    IEnumerable<IDataShareRequestQuestionStatusDeterminationResult> QuestionStatusDeterminationResults { get; }
}