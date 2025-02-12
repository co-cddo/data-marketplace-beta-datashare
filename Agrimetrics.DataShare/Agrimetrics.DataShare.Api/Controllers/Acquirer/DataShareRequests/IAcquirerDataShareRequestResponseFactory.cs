using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Dto.Requests.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Responses.Acquirer.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Controllers.Acquirer.DataShareRequests;

public interface IAcquirerDataShareRequestResponseFactory
{
    GetEsdaQuestionSetOutlineResponse CreateGetEsdaQuestionSetOutlineResponse(
        GetEsdaQuestionSetOutlineRequest getEsdaQuestionSetOutlineRequest,
        QuestionSetOutline questionSetOutline);

    StartDataShareRequestResponse CreateStartDataShareRequestResponse(
        StartDataShareRequestRequest startDataShareRequestRequest,
        Guid dataShareRequestId);

    GetDataShareRequestSummariesResponse CreateGetDataShareRequestSummariesResponse(
        DataShareRequestSummarySet dataShareRequestSummaries);

    GetDataShareRequestAdminSummariesResponse CreateGetDataShareRequestAdminSummariesResponse(
        DataShareRequestAdminSummarySet dataShareRequestAdminSummaries);

    GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse CreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse(
        GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest,
        DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet dataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet);

    GetDataShareRequestQuestionsSummaryResponse CreateGetDataShareRequestQuestionsSummaryResponse(
        GetDataShareRequestQuestionsSummaryRequest getDataShareRequestQuestionsSummaryRequest,
        DataShareRequestQuestionsSummary dataShareRequestQuestionsSummary);

    GetDataShareRequestQuestionInformationResponse CreateGetDataShareRequestQuestionInformationResponse(
        GetDataShareRequestQuestionInformationRequest getDataShareRequestQuestionInformationRequest,
        DataShareRequestQuestion dataShareRequestQuestion);

    SetDataShareRequestQuestionAnswerResponse CreateSetDataShareRequestQuestionAnswerResponse(
        SetDataShareRequestQuestionAnswerRequest setDataShareRequestQuestionAnswerRequest,
        SetDataShareRequestQuestionAnswerResult setDataShareRequestQuestionAnswerResult);

    GetDataShareRequestAnswersSummaryResponse CreateGetDataShareRequestAnswersSummaryResponse(
        GetDataShareRequestAnswersSummaryRequest getDataShareRequestAnswersSummaryRequest,
        DataShareRequestAnswersSummary dataShareRequestAnswersSummary);

    SubmitDataShareRequestResponse CreateSubmitDataShareRequestResponse(
        SubmitDataShareRequestRequest submitDataShareRequestRequest,
        DataShareRequestSubmissionResult dataShareRequestSubmissionResult);

    CancelDataShareRequestResponse CreateCancelDataShareRequestResponse(
        CancelDataShareRequestRequest cancelDataShareRequestRequest,
        DataShareRequestCancellationResult dataShareRequestCancellationResult);

    DeleteDataShareRequestResponse CreateDeleteDataShareRequestResponse(
        DeleteDataShareRequestRequest deleteDataShareRequestRequest,
        DataShareRequestDeletionResult dataShareRequestDeletionResult);
}