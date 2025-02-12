using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using Agrimetrics.DataShare.Api.Dto.Requests.Supplier;
using Agrimetrics.DataShare.Api.Dto.Responses.Supplier;

namespace Agrimetrics.DataShare.Api.Controllers.Supplier.DataShareRequests;

public interface ISupplierDataShareRequestResponseFactory
{
    GetSubmissionSummariesResponse CreateGetSubmissionSummariesResponse(
        SubmissionSummariesSet submissionSummariesSet);

    GetSubmissionInformationResponse CreateGetSubmissionInformationResponse(
        GetSubmissionInformationRequest getSubmissionInformationRequest,
        SubmissionInformation submissionInformation);

    GetSubmissionDetailsResponse CreateGetSubmissionDetailsResponse(
        GetSubmissionDetailsRequest getSubmissionDetailsRequest,
        SubmissionDetails submissionDetails);

    StartSubmissionReviewResponse CreateStartSubmissionReviewResponse(
        StartSubmissionReviewRequest startSubmissionReviewRequest,
        SubmissionReviewInformation submissionReviewInformation);

    GetSubmissionReviewInformationResponse CreateGetSubmissionReviewInformationResponse(
        GetSubmissionReviewInformationRequest getSubmissionReviewInformationRequest,
        SubmissionReviewInformation submissionReviewInformation);

    GetReturnedSubmissionInformationResponse CreateGetReturnedSubmissionInformationResponse(
        GetReturnedSubmissionInformationRequest getReturnedSubmissionInformationRequest,
        ReturnedSubmissionInformation returnedSubmissionInformation);

    GetCompletedSubmissionInformationResponse CreateGetCompletedSubmissionInformationResponse(
        GetCompletedSubmissionInformationRequest getCompletedSubmissionInformationRequest,
        CompletedSubmissionInformation completedSubmissionInformation);

    SetSubmissionNotesResponse CreateSetSubmissionNotesResponse(
        SetSubmissionNotesRequest setSubmissionNotesRequest);

    AcceptSubmissionResponse CreateAcceptSubmissionResponse(
        AcceptSubmissionRequest acceptSubmissionRequest,
        DataShareRequestAcceptanceResult dataShareRequestAcceptanceResult);

    RejectSubmissionResponse CreateRejectSubmissionResponse(
        RejectSubmissionRequest rejectSubmissionRequest,
        DataShareRequestRejectionResult dataShareRequestRejectionResult);

    ReturnSubmissionResponse CreateReturnSubmissionResponse(
        ReturnSubmissionRequest returnSubmissionRequest,
        DataShareRequestReturnResult dataShareRequestReturnResult);

    
}