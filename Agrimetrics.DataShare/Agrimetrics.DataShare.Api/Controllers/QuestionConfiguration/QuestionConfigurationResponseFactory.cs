using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Dto.Requests.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Dto.Responses.QuestionConfiguration.CompulsoryQuestions;

namespace Agrimetrics.DataShare.Api.Controllers.QuestionConfiguration;

internal class QuestionConfigurationResponseFactory : IQuestionConfigurationResponseFactory
{
    #region Compulsory Questions
    GetCompulsoryQuestionsResponse IQuestionConfigurationResponseFactory.CreateGetCompulsoryQuestionsResponse(
        GetCompulsoryQuestionsRequest getCompulsoryQuestionsRequest, CompulsoryQuestionSet compulsoryQuestionSet)
    {
        ArgumentNullException.ThrowIfNull(getCompulsoryQuestionsRequest);
        ArgumentNullException.ThrowIfNull(compulsoryQuestionSet);

        return new GetCompulsoryQuestionsResponse
        {
            RequestingUserId = getCompulsoryQuestionsRequest.RequestingUserId,
            CompulsoryQuestionSet = compulsoryQuestionSet
        };
    }

    SetCompulsoryQuestionResponse IQuestionConfigurationResponseFactory.CreateSetCompulsoryQuestionResponse(
        SetCompulsoryQuestionRequest setCompulsoryQuestionRequest)
    {
        ArgumentNullException.ThrowIfNull(setCompulsoryQuestionRequest);

        return new SetCompulsoryQuestionResponse
        {
            RequestingUserId = setCompulsoryQuestionRequest.RequestingUserId,
            QuestionId = setCompulsoryQuestionRequest.QuestionId
        };
    }

    ClearCompulsoryQuestionResponse IQuestionConfigurationResponseFactory.CreateClearCompulsoryQuestionResponse(
        ClearCompulsoryQuestionRequest clearCompulsoryQuestionRequest)
    {
        ArgumentNullException.ThrowIfNull(clearCompulsoryQuestionRequest);

        return new ClearCompulsoryQuestionResponse
        {
            RequestingUserId = clearCompulsoryQuestionRequest.RequestingUserId,
            QuestionId = clearCompulsoryQuestionRequest.QuestionId
        };
    }
    #endregion

    #region Compulsory Supplier-Mandated Questions
    GetCompulsorySupplierMandatedQuestionsResponse IQuestionConfigurationResponseFactory.CreateGetCompulsorySupplierMandatedQuestionsResponse(
        GetCompulsorySupplierMandatedQuestionsRequest getCompulsorySupplierMandatedQuestionsRequest,
        CompulsorySupplierMandatedQuestionSet compulsorySupplierMandatedQuestionSet)
    {
        ArgumentNullException.ThrowIfNull(getCompulsorySupplierMandatedQuestionsRequest);
        ArgumentNullException.ThrowIfNull(compulsorySupplierMandatedQuestionSet);

        return new GetCompulsorySupplierMandatedQuestionsResponse
        {
            RequestingUserId = getCompulsorySupplierMandatedQuestionsRequest.RequestingUserId,
            SupplierOrganisationId = getCompulsorySupplierMandatedQuestionsRequest.SupplierOrganisationId,
            CompulsorySupplierMandatedQuestionSet = compulsorySupplierMandatedQuestionSet
        };
    }

    SetCompulsorySupplierMandatedQuestionResponse IQuestionConfigurationResponseFactory.CreateSetCompulsorySupplierMandatedQuestionResponse(
        SetCompulsorySupplierMandatedQuestionRequest setCompulsorySupplierMandatedQuestionRequest)
    {
        ArgumentNullException.ThrowIfNull(setCompulsorySupplierMandatedQuestionRequest);

        return new SetCompulsorySupplierMandatedQuestionResponse
        {
            RequestingUserId = setCompulsorySupplierMandatedQuestionRequest.RequestingUserId,
            SupplierOrganisationId = setCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId,
            QuestionId = setCompulsorySupplierMandatedQuestionRequest.QuestionId
        };
    }

    ClearCompulsorySupplierMandatedQuestionResponse IQuestionConfigurationResponseFactory.CreateClearCompulsorySupplierMandatedQuestionResponse(
        ClearCompulsorySupplierMandatedQuestionRequest clearCompulsorySupplierMandatedQuestionRequest)
    {
        ArgumentNullException.ThrowIfNull(clearCompulsorySupplierMandatedQuestionRequest);

        return new ClearCompulsorySupplierMandatedQuestionResponse
        {
            RequestingUserId = clearCompulsorySupplierMandatedQuestionRequest.RequestingUserId,
            SupplierOrganisationId = clearCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId,
            QuestionId = clearCompulsorySupplierMandatedQuestionRequest.QuestionId
        };
    }
    #endregion
}