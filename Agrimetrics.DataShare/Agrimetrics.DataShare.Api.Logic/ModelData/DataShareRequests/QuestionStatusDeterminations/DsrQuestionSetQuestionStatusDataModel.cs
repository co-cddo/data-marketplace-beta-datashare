using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;

internal class DataShareRequestQuestionSetQuestionStatusDataModel : IDataShareRequestQuestionSetQuestionStatusDataModel
{
    public required Guid QuestionId { get; init; }

    public required int SectionNumber { get; init; }

    public required int QuestionOrderWithinSection { get; init; }

    public required QuestionStatusType QuestionStatus { get; init; }
}