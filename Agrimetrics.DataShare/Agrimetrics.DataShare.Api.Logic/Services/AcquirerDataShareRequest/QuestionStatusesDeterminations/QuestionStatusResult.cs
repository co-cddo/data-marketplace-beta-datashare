using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

internal class DataShareRequestQuestionStatusDeterminationResult
    : IDataShareRequestQuestionStatusDeterminationResult
{
    public required IDataShareRequestQuestionSetQuestionStatusDataModel QuestionSetQuestionStatusData { get; init; }

    public required QuestionStatusType PreviousQuestionStatus { get; init; }
}