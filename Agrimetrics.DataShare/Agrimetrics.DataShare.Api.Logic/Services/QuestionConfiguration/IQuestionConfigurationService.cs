using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;

namespace Agrimetrics.DataShare.Api.Logic.Services.QuestionConfiguration
{
    public interface IQuestionConfigurationService
    {
        #region Compulsory Questions
        Task<IServiceOperationDataResult<CompulsoryQuestionSet>> GetCompulsoryQuestionsAsync(int requestingUserId);

        Task<IServiceOperationResult> SetCompulsoryQuestionAsync(int requestingUserId, Guid questionId);

        Task<IServiceOperationResult> ClearCompulsoryQuestionAsync(int requestingUserId, Guid questionId);
        #endregion

        #region Compulsory Supplier-Mandated Questions
        Task<IServiceOperationDataResult<CompulsorySupplierMandatedQuestionSet>> GetCompulsorySupplierMandatedQuestionsAsync(int requestingUserId, int supplierOrganisationId);

        Task<IServiceOperationResult> SetCompulsorySupplierMandatedQuestionAsync(int requestingUserId, int supplierOrganisationId, Guid questionId);

        Task<IServiceOperationResult> ClearCompulsorySupplierMandatedQuestionAsync(int requestingUserId, int supplierOrganisationId, Guid questionId);
        #endregion
    }
}
