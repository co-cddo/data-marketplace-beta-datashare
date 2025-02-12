namespace Agrimetrics.DataShare.Api.Logic.Services.KeyQuestionPartAnswers
{
    public interface IKeyQuestionPartAnswerProviderService
    {
        Task<DateTime?> GetDateRequiredQuestionPartAnswerAsync(Guid dataShareRequestId);

        Task<string?> GetProjectAimsQuestionPartAnswerAsync(Guid dataShareRequestId);

        Task<IEnumerable<string>> GetDataTypesQuestionPartAnswerAsync(Guid dataShareRequestId);
    }
}
