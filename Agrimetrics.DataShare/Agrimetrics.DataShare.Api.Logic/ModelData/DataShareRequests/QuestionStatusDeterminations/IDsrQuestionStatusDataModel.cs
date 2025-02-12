using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations
{
    public interface IDataShareRequestQuestionStatusDataModel
    {
        Guid QuestionId { get; }

        QuestionStatusType QuestionStatus { get; }
    }
}
