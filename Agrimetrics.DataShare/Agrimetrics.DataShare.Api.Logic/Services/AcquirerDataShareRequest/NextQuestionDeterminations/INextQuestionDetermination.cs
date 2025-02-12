using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.NextQuestionDeterminations
{
    public interface INextQuestionDetermination
    {
        Guid? DetermineNextQuestion(Guid currentQuestionId,
            IEnumerable<IDataShareRequestQuestionSetQuestionStatusDataModel> questionStatuses);
    }
}
