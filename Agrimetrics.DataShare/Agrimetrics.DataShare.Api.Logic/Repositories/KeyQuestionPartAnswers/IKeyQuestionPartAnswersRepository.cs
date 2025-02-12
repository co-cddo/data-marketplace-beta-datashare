using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.KeyQuestionPartAnswers
{
    public interface IKeyQuestionPartAnswersRepository
    {
        Task<IEnumerable<string>> GetKeyQuestionPartResponsesAsync(
            Guid dataShareRequestId,
            QuestionPartKeyType questionPartKey);
    }
}
