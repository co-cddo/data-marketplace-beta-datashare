using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Logic.Repositories.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Services.QuestionConfiguration;

internal class QuestionConfigurationService(
    ILogger<QuestionConfigurationService> logger,
    IQuestionConfigurationRepository questionConfigurationRepository,
    IQuestionConfigurationModelDataFactory questionConfigurationModelDataFactory,
    IServiceOperationResultFactory serviceOperationResultFactory) : IQuestionConfigurationService
{
    #region Compulsory Questions
    async Task<IServiceOperationDataResult<CompulsoryQuestionSet>> IQuestionConfigurationService.GetCompulsoryQuestionsAsync(
        int requestingUserId)
    {
        try
        {
            var compulsoryQuestionModelDatas = await questionConfigurationRepository.GetCompulsoryQuestionsAsync();

            var compulsoryQuestions = questionConfigurationModelDataFactory.CreateCompulsoryQuestionSet(compulsoryQuestionModelDatas);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(compulsoryQuestions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetCompulsoryQuestions");

            var response = serviceOperationResultFactory.CreateFailedDataResult<CompulsoryQuestionSet>(ex.Message);

            return await Task.FromResult(response);
        }
    }

    async Task<IServiceOperationResult> IQuestionConfigurationService.SetCompulsoryQuestionAsync(
        int requestingUserId,
        Guid questionId)
    {
        try
        {
            await questionConfigurationRepository.SetCompulsoryQuestionAsync(questionId);

            return serviceOperationResultFactory.CreateSuccessfulResult();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to SetCompulsoryQuestion");

            var response = serviceOperationResultFactory.CreateFailedResult(ex.Message);

            return await Task.FromResult(response);
        }
    }

    async Task<IServiceOperationResult> IQuestionConfigurationService.ClearCompulsoryQuestionAsync(
        int requestingUserId,
        Guid questionId)
    {
        try
        {
            await questionConfigurationRepository.ClearCompulsoryQuestionAsync(questionId);

            return serviceOperationResultFactory.CreateSuccessfulResult();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to ClearCompulsoryQuestion");

            var response = serviceOperationResultFactory.CreateFailedResult(ex.Message);

            return await Task.FromResult(response);
        }
    }
    #endregion

    #region Compulsory Supplier-Mandated Questions
    async Task<IServiceOperationDataResult<CompulsorySupplierMandatedQuestionSet>> IQuestionConfigurationService.GetCompulsorySupplierMandatedQuestionsAsync(
        int requestingUserId,
        int supplierOrganisationId)
    {
        try
        {
            var compulsorySupplierMandatedQuestionModelDatas = await questionConfigurationRepository.GetCompulsorySupplierMandatedQuestionsAsync(
                supplierOrganisationId);

            var compulsorySupplierMandatedQuestions = questionConfigurationModelDataFactory.CreateCompulsorySupplierMandatedQuestionSet(
                compulsorySupplierMandatedQuestionModelDatas);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(compulsorySupplierMandatedQuestions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to StartDataShareRequest");

            var response = serviceOperationResultFactory.CreateFailedDataResult<CompulsorySupplierMandatedQuestionSet>(ex.Message);

            return await Task.FromResult(response);
        }
    }

    async Task<IServiceOperationResult> IQuestionConfigurationService.SetCompulsorySupplierMandatedQuestionAsync(
        int requestingUserId,
        int supplierOrganisationId,
        Guid questionId)
    {
        try
        {
            await questionConfigurationRepository.SetCompulsorySupplierMandatedQuestionAsync(supplierOrganisationId, questionId);

            return serviceOperationResultFactory.CreateSuccessfulResult();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to SetCompulsorySupplierMandatedQuestion");

            var response = serviceOperationResultFactory.CreateFailedResult(ex.Message);

            return await Task.FromResult(response);
        }
    }

    async Task<IServiceOperationResult> IQuestionConfigurationService.ClearCompulsorySupplierMandatedQuestionAsync(
        int requestingUserId,
        int supplierOrganisationId,
        Guid questionId)
    {
        try
        {
            await questionConfigurationRepository.ClearCompulsorySupplierMandatedQuestionAsync(supplierOrganisationId, questionId);

            return serviceOperationResultFactory.CreateSuccessfulResult();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to ClearCompulsorySupplierMandatedQuestion");

            var response = serviceOperationResultFactory.CreateFailedResult(ex.Message);

            return await Task.FromResult(response);
        }
    }
    #endregion
}