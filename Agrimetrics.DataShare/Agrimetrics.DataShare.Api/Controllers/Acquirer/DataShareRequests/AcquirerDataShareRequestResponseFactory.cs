using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Dto.Requests.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Responses.Acquirer.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Controllers.Acquirer.DataShareRequests;

internal class AcquirerDataShareRequestResponseFactory : IAcquirerDataShareRequestResponseFactory
{
    GetEsdaQuestionSetOutlineResponse IAcquirerDataShareRequestResponseFactory.CreateGetEsdaQuestionSetOutlineResponse(
        GetEsdaQuestionSetOutlineRequest getEsdaQuestionSetOutlineRequest,
        QuestionSetOutline questionSetOutline)
    {
        ArgumentNullException.ThrowIfNull(getEsdaQuestionSetOutlineRequest);
        ArgumentNullException.ThrowIfNull(questionSetOutline);

        return new GetEsdaQuestionSetOutlineResponse
        {
            EsdaId = getEsdaQuestionSetOutlineRequest.EsdaId,
            QuestionSetOutline = questionSetOutline
        };
    }

    StartDataShareRequestResponse IAcquirerDataShareRequestResponseFactory.CreateStartDataShareRequestResponse(
        StartDataShareRequestRequest startDataShareRequestRequest,
        Guid dataShareRequestId)
    {
        ArgumentNullException.ThrowIfNull(startDataShareRequestRequest);

        return new StartDataShareRequestResponse
        {
            EsdaId = startDataShareRequestRequest.EsdaId,
            DataShareRequestId = dataShareRequestId
        };
    }

    GetDataShareRequestSummariesResponse IAcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestSummariesResponse(
        DataShareRequestSummarySet dataShareRequestSummaries)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestSummaries);

        return new GetDataShareRequestSummariesResponse
        {
            DataShareRequestSummaries = dataShareRequestSummaries
        };
    }

    GetDataShareRequestAdminSummariesResponse IAcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestAdminSummariesResponse(
        DataShareRequestAdminSummarySet dataShareRequestAdminSummaries)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestAdminSummaries);

        return new GetDataShareRequestAdminSummariesResponse
        {
            DataShareRequestAdminSummaries = dataShareRequestAdminSummaries
        };
    }

    GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse IAcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse(
        GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest,
        DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet dataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet)
    {
        ArgumentNullException.ThrowIfNull(getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest);
        ArgumentNullException.ThrowIfNull(dataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet);

        return new GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse
        {
            EsdaId = getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest.EsdaId,
            DataShareRequestSummaries = dataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet
        };
    }

    GetDataShareRequestQuestionsSummaryResponse IAcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestQuestionsSummaryResponse(
            GetDataShareRequestQuestionsSummaryRequest getDataShareRequestQuestionsSummaryRequest,
            DataShareRequestQuestionsSummary dataShareRequestQuestionsSummary)
    {
        ArgumentNullException.ThrowIfNull(getDataShareRequestQuestionsSummaryRequest);
        ArgumentNullException.ThrowIfNull(dataShareRequestQuestionsSummary);

        return new GetDataShareRequestQuestionsSummaryResponse
        {
            DataShareRequestId = getDataShareRequestQuestionsSummaryRequest.DataShareRequestId,
            DataShareRequestRequestId = dataShareRequestQuestionsSummary.DataShareRequestRequestId,
            EsdaName = dataShareRequestQuestionsSummary.EsdaName,
            QuestionSetSummary = dataShareRequestQuestionsSummary.QuestionSetSummary
        };
    }

    GetDataShareRequestQuestionInformationResponse IAcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestQuestionInformationResponse(
        GetDataShareRequestQuestionInformationRequest getDataShareRequestQuestionInformationRequest,
        DataShareRequestQuestion dataShareRequestQuestion)
    {
        ArgumentNullException.ThrowIfNull(getDataShareRequestQuestionInformationRequest);
        ArgumentNullException.ThrowIfNull(dataShareRequestQuestion);

        return new GetDataShareRequestQuestionInformationResponse
        {
            DataShareRequestId = getDataShareRequestQuestionInformationRequest.DataShareRequestId,
            QuestionId = getDataShareRequestQuestionInformationRequest.QuestionId,
            DataShareRequestQuestion = dataShareRequestQuestion
        };
    }

    SetDataShareRequestQuestionAnswerResponse IAcquirerDataShareRequestResponseFactory.CreateSetDataShareRequestQuestionAnswerResponse(
        SetDataShareRequestQuestionAnswerRequest setDataShareRequestQuestionAnswerRequest,
        SetDataShareRequestQuestionAnswerResult setDataShareRequestQuestionAnswerResult)
    {
        ArgumentNullException.ThrowIfNull(setDataShareRequestQuestionAnswerRequest);
        ArgumentNullException.ThrowIfNull(setDataShareRequestQuestionAnswerResult);

        return new SetDataShareRequestQuestionAnswerResponse
        {
            Result = setDataShareRequestQuestionAnswerResult
        };
    }

    GetDataShareRequestAnswersSummaryResponse IAcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestAnswersSummaryResponse(
        GetDataShareRequestAnswersSummaryRequest getDataShareRequestAnswersSummaryRequest,
        DataShareRequestAnswersSummary dataShareRequestAnswersSummary)
    {
        ArgumentNullException.ThrowIfNull(getDataShareRequestAnswersSummaryRequest);
        ArgumentNullException.ThrowIfNull(dataShareRequestAnswersSummary);

        return new GetDataShareRequestAnswersSummaryResponse
        {
            DataShareRequestId = getDataShareRequestAnswersSummaryRequest.DataShareRequestId,
            AnswersSummary = dataShareRequestAnswersSummary
        };
    }

    SubmitDataShareRequestResponse IAcquirerDataShareRequestResponseFactory.CreateSubmitDataShareRequestResponse(
        SubmitDataShareRequestRequest submitDataShareRequestRequest,
        DataShareRequestSubmissionResult dataShareRequestSubmissionResult)
    {
        ArgumentNullException.ThrowIfNull(submitDataShareRequestRequest);
        ArgumentNullException.ThrowIfNull(dataShareRequestSubmissionResult);

        return new SubmitDataShareRequestResponse
        {
            DataShareRequestId = submitDataShareRequestRequest.DataShareRequestId,
            DataShareRequestRequestId = dataShareRequestSubmissionResult.DataShareRequestRequestId,
            NotificationSuccess = dataShareRequestSubmissionResult.NotificationSuccess
        };
    }

    CancelDataShareRequestResponse IAcquirerDataShareRequestResponseFactory.CreateCancelDataShareRequestResponse(
        CancelDataShareRequestRequest cancelDataShareRequestRequest,
        DataShareRequestCancellationResult dataShareRequestCancellationResult)
    {
        ArgumentNullException.ThrowIfNull(cancelDataShareRequestRequest);
        ArgumentNullException.ThrowIfNull(dataShareRequestCancellationResult);

        return new CancelDataShareRequestResponse
        {
            DataShareRequestId = cancelDataShareRequestRequest.DataShareRequestId,
            ReasonsForCancellation = cancelDataShareRequestRequest.ReasonsForCancellation,
            NotificationSuccess = dataShareRequestCancellationResult.NotificationSuccess
        };
    }

    DeleteDataShareRequestResponse IAcquirerDataShareRequestResponseFactory.CreateDeleteDataShareRequestResponse(
        DeleteDataShareRequestRequest deleteDataShareRequestRequest,
        DataShareRequestDeletionResult dataShareRequestDeletionResult)
    {
        ArgumentNullException.ThrowIfNull(deleteDataShareRequestRequest);
        ArgumentNullException.ThrowIfNull(dataShareRequestDeletionResult);

        return new DeleteDataShareRequestResponse
        {
            DataShareRequestId = dataShareRequestDeletionResult.DataShareRequestId
        };
    }
}