using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

public interface IDataShareRequestQuestionStatusDeterminationResult
{
    IDataShareRequestQuestionSetQuestionStatusDataModel QuestionSetQuestionStatusData { get; }

    QuestionStatusType PreviousQuestionStatus { get; }
}