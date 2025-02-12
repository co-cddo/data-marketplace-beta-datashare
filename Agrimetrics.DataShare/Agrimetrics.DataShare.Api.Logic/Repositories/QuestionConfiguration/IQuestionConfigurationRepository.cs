using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration.CompulsoryQuestions;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.QuestionConfiguration
{
    public interface IQuestionConfigurationRepository
    {
        Task<IEnumerable<CompulsoryQuestionModelData>> GetCompulsoryQuestionsAsync();

        Task SetCompulsoryQuestionAsync(Guid questionId);

        Task ClearCompulsoryQuestionAsync(Guid questionId);

        Task<IEnumerable<CompulsorySupplierMandatedQuestionModelData>> GetCompulsorySupplierMandatedQuestionsAsync(int supplierOrganisationId);

        Task SetCompulsorySupplierMandatedQuestionAsync(int supplierOrganisationId, Guid questionId);

        Task ClearCompulsorySupplierMandatedQuestionAsync(int supplierOrganisationId, Guid questionId);
    }
}
