using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

namespace Agrimetrics.DataShare.Api.Logic.ModelData;

public interface ISupplierDataShareRequestModelDataFactory
{
    SubmissionSummariesSet CreateSubmissionSummarySet(
        IEnumerable<PendingSubmissionSummaryModelData> pendingSubmissionSummaryModelDatas,
        IEnumerable<CompletedSubmissionSummaryModelData> completedSubmissionSummaryModelDatas);

    SubmissionInformation CreateSubmissionInformation(
        SubmissionInformationModelData submissionInformationModelData);

    SubmissionDetails CreateSubmissionDetails(
        SubmissionDetailsModelData submissionDetailsModelData);

    SubmissionReviewInformation CreateSubmissionReviewInformation(
        SubmissionReviewInformationModelData submissionReviewInformationModelData);

    ReturnedSubmissionInformation CreateReturnedSubmissionInformation(
        ReturnedSubmissionInformationModelData returnedSubmissionInformationModelData);

    CompletedSubmissionInformation CreateCompletedSubmissionInformation(
        CompletedSubmissionInformationModelData completedSubmissionInformationModelData);

    DataShareRequestAcceptanceResult CreateDataShareRequestAcceptanceResult(
        AcceptedDecisionSummaryModelData acceptedDecisionSummaryModelData,
        bool? notificationsSentSuccess);

    DataShareRequestRejectionResult CreateDataShareRequestRejectionResult(
        RejectedDecisionSummaryModelData rejectedDecisionSummaryModelData,
        bool? notificationsSentSuccess);

    DataShareRequestReturnResult CreateDataShareRequestReturnResult(
        ReturnedDecisionSummaryModelData returnedDecisionSummaryModelData,
        bool? notificationsSentSuccess);
}