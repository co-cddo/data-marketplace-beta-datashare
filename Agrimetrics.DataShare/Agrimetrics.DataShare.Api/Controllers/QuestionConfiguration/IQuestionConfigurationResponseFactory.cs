using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Dto.Requests.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Dto.Responses.QuestionConfiguration.CompulsoryQuestions;

namespace Agrimetrics.DataShare.Api.Controllers.QuestionConfiguration;

public interface IQuestionConfigurationResponseFactory
{
    #region Compulsory Questions
    GetCompulsoryQuestionsResponse CreateGetCompulsoryQuestionsResponse(
        GetCompulsoryQuestionsRequest getCompulsoryQuestionsRequest, CompulsoryQuestionSet compulsoryQuestionSet);

    SetCompulsoryQuestionResponse CreateSetCompulsoryQuestionResponse(
        SetCompulsoryQuestionRequest setCompulsoryQuestionRequest);

    ClearCompulsoryQuestionResponse CreateClearCompulsoryQuestionResponse(
        ClearCompulsoryQuestionRequest clearCompulsoryQuestionRequest);
    #endregion

    #region Compulsory Supplier-Mandated Questions
    GetCompulsorySupplierMandatedQuestionsResponse CreateGetCompulsorySupplierMandatedQuestionsResponse(
        GetCompulsorySupplierMandatedQuestionsRequest getCompulsorySupplierMandatedQuestionsRequest, CompulsorySupplierMandatedQuestionSet compulsorySupplierMandatedQuestionSet);

    SetCompulsorySupplierMandatedQuestionResponse CreateSetCompulsorySupplierMandatedQuestionResponse(
        SetCompulsorySupplierMandatedQuestionRequest setCompulsorySupplierMandatedQuestionRequest);

    ClearCompulsorySupplierMandatedQuestionResponse CreateClearCompulsorySupplierMandatedQuestionResponse(
        ClearCompulsorySupplierMandatedQuestionRequest clearCompulsorySupplierMandatedQuestionRequest);
    #endregion
}