using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;

internal class DataShareRequestQuestionStatusDataModel : IDataShareRequestQuestionStatusDataModel
{
    public required Guid QuestionId { get; init; }

    public required QuestionStatusType QuestionStatus { get; init; }
}