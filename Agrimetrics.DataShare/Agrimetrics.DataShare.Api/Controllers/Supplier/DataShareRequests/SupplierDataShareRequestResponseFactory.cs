using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Requests.Supplier;
using Agrimetrics.DataShare.Api.Dto.Responses.Supplier;

namespace Agrimetrics.DataShare.Api.Controllers.Supplier.DataShareRequests;

internal class SupplierDataShareRequestResponseFactory : ISupplierDataShareRequestResponseFactory
{
    GetSubmissionSummariesResponse ISupplierDataShareRequestResponseFactory.CreateGetSubmissionSummariesResponse(
        SubmissionSummariesSet submissionSummariesSet)
    {
        ArgumentNullException.ThrowIfNull(submissionSummariesSet);

        return new GetSubmissionSummariesResponse
        {
            SubmissionSummariesSet = submissionSummariesSet
        };
    }

    GetSubmissionInformationResponse ISupplierDataShareRequestResponseFactory.CreateGetSubmissionInformationResponse(
        GetSubmissionInformationRequest getSubmissionInformationRequest,
        SubmissionInformation submissionInformation)
    {
        ArgumentNullException.ThrowIfNull(getSubmissionInformationRequest);
        ArgumentNullException.ThrowIfNull(submissionInformation);

        return new GetSubmissionInformationResponse
        {
            DataShareRequestId = getSubmissionInformationRequest.DataShareRequestId,
            SubmissionInformation = submissionInformation
        };
    }

    GetSubmissionDetailsResponse ISupplierDataShareRequestResponseFactory.CreateGetSubmissionDetailsResponse(
        GetSubmissionDetailsRequest getSubmissionDetailsRequest,
        SubmissionDetails submissionDetails)
    {
        ArgumentNullException.ThrowIfNull(getSubmissionDetailsRequest);
        ArgumentNullException.ThrowIfNull(submissionDetails);

        return new GetSubmissionDetailsResponse
        {
            DataShareRequestId = getSubmissionDetailsRequest.DataShareRequestId,
            SubmissionDetails = submissionDetails
        };
    }

    StartSubmissionReviewResponse ISupplierDataShareRequestResponseFactory.CreateStartSubmissionReviewResponse(
        StartSubmissionReviewRequest startSubmissionReviewRequest,
        SubmissionReviewInformation submissionReviewInformation)
    {
        ArgumentNullException.ThrowIfNull(startSubmissionReviewRequest);
        ArgumentNullException.ThrowIfNull(submissionReviewInformation);

        return new StartSubmissionReviewResponse
        {
            DataShareRequestId = startSubmissionReviewRequest.DataShareRequestId,
            SubmissionReviewInformation = submissionReviewInformation
        };
    }

    GetSubmissionReviewInformationResponse ISupplierDataShareRequestResponseFactory.CreateGetSubmissionReviewInformationResponse(
        GetSubmissionReviewInformationRequest getSubmissionReviewInformationRequest,
        SubmissionReviewInformation submissionReviewInformation)
    {
        ArgumentNullException.ThrowIfNull(getSubmissionReviewInformationRequest);
        ArgumentNullException.ThrowIfNull(submissionReviewInformation);

        return new GetSubmissionReviewInformationResponse
        {
            DataShareRequestId = getSubmissionReviewInformationRequest.DataShareRequestId,
            SubmissionReviewInformation = submissionReviewInformation
        };
    }

    GetReturnedSubmissionInformationResponse ISupplierDataShareRequestResponseFactory.CreateGetReturnedSubmissionInformationResponse(
        GetReturnedSubmissionInformationRequest getReturnedSubmissionInformationRequest,
        ReturnedSubmissionInformation returnedSubmissionInformation)
    {
        ArgumentNullException.ThrowIfNull(getReturnedSubmissionInformationRequest);
        ArgumentNullException.ThrowIfNull(returnedSubmissionInformation);

        return new GetReturnedSubmissionInformationResponse
        {
            DataShareRequestId = getReturnedSubmissionInformationRequest.DataShareRequestId,
            ReturnedSubmissionInformation = returnedSubmissionInformation
        };
    }

    GetCompletedSubmissionInformationResponse ISupplierDataShareRequestResponseFactory.CreateGetCompletedSubmissionInformationResponse(
        GetCompletedSubmissionInformationRequest getCompletedSubmissionInformationRequest,
        CompletedSubmissionInformation completedSubmissionInformation)
    {
        ArgumentNullException.ThrowIfNull(getCompletedSubmissionInformationRequest);
        ArgumentNullException.ThrowIfNull(completedSubmissionInformation);

        return new GetCompletedSubmissionInformationResponse
        {
            DataShareRequestId = getCompletedSubmissionInformationRequest.DataShareRequestId,
            CompletedSubmissionInformation = completedSubmissionInformation
        };
    }

    SetSubmissionNotesResponse ISupplierDataShareRequestResponseFactory.CreateSetSubmissionNotesResponse(
        SetSubmissionNotesRequest setSubmissionNotesRequest)
    {
        ArgumentNullException.ThrowIfNull(setSubmissionNotesRequest);

        return new SetSubmissionNotesResponse
        {
            DataShareRequestId = setSubmissionNotesRequest.DataShareRequestId
        };
    }

    AcceptSubmissionResponse ISupplierDataShareRequestResponseFactory.CreateAcceptSubmissionResponse(
        AcceptSubmissionRequest acceptSubmissionRequest,
        DataShareRequestAcceptanceResult dataShareRequestAcceptanceResult)
    {
        ArgumentNullException.ThrowIfNull(acceptSubmissionRequest);
        ArgumentNullException.ThrowIfNull(dataShareRequestAcceptanceResult);
        
        return new AcceptSubmissionResponse
        {
            DataShareRequestId = acceptSubmissionRequest.DataShareRequestId,
            AcceptedDecisionSummary = dataShareRequestAcceptanceResult.AcceptedDecisionSummary,
            NotificationSuccess = dataShareRequestAcceptanceResult.NotificationSuccess
        };
    }

    RejectSubmissionResponse ISupplierDataShareRequestResponseFactory.CreateRejectSubmissionResponse(
        RejectSubmissionRequest rejectSubmissionRequest,
        DataShareRequestRejectionResult dataShareRequestRejectionResult)
    {
        ArgumentNullException.ThrowIfNull(rejectSubmissionRequest);
        ArgumentNullException.ThrowIfNull(dataShareRequestRejectionResult);

        return new RejectSubmissionResponse
        {
            DataShareRequestId = rejectSubmissionRequest.DataShareRequestId,
            RejectedDecisionSummary = dataShareRequestRejectionResult.RejectedDecisionSummary,
            NotificationSuccess = dataShareRequestRejectionResult.NotificationSuccess
        };
    }

    ReturnSubmissionResponse ISupplierDataShareRequestResponseFactory.CreateReturnSubmissionResponse(
        ReturnSubmissionRequest returnSubmissionRequest,
        DataShareRequestReturnResult dataShareRequestReturnResult)
    {
        ArgumentNullException.ThrowIfNull(returnSubmissionRequest);
        ArgumentNullException.ThrowIfNull(dataShareRequestReturnResult);

        return new ReturnSubmissionResponse
        {
            DataShareRequestId = returnSubmissionRequest.DataShareRequestId,
            ReturnedDecisionSummary = dataShareRequestReturnResult.ReturnedDecisionSummary,
            NotificationSuccess = dataShareRequestReturnResult.NotificationSuccess
        };
    }
}