using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations
{
    public interface IDataShareRequestQuestionSetQuestionStatusDataModel
    {
        Guid QuestionId { get; }

        int SectionNumber { get; }

        int QuestionOrderWithinSection { get; }

        QuestionStatusType QuestionStatus { get; }
    }
}
