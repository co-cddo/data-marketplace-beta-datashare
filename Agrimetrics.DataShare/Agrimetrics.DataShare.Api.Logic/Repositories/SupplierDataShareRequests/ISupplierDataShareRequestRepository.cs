using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.SupplierDataShareRequests;

public interface ISupplierDataShareRequestRepository
{
    Task<IEnumerable<PendingSubmissionSummaryModelData>> GetPendingSubmissionSummariesAsync(
        int supplierOrganisationId);

    Task<IEnumerable<CompletedSubmissionSummaryModelData>> GetCompletedSubmissionSummariesAsync(
        int supplierOrganisationId);

    Task<SubmissionInformationModelData> GetSubmissionInformationModelDataAsync(
        Guid dataShareRequestId);

    Task<SubmissionDetailsModelData> GetSubmissionDetailsModelDataAsync(
        Guid dataShareRequestId);

    Task<SubmissionReviewInformationModelData> GetSubmissionReviewInformationModelDataAsync(
        Guid dataShareRequestId);

    Task<ReturnedSubmissionInformationModelData> GetReturnedSubmissionInformationAsync(
        Guid dataShareRequestId);

    Task<CompletedSubmissionInformationModelData> GetCompletedSubmissionInformationAsync(
        Guid dataShareRequestId);

    Task SetSubmissionNotesAsync(
        Guid dataShareRequestId,
        string notes);

    Task StartSubmissionReviewAsync(
        IUserIdSet supplierUserIdSet,
        Guid dataShareRequestId);

    Task AcceptSubmissionAsync(
        IUserIdSet supplierUserIdSet,
        Guid dataShareRequestId,
        string decisionFeedback);

    Task RejectSubmissionAsync(
        IUserIdSet supplierUserIdSet,
        Guid dataShareRequestId,
        string decisionFeedback);

    Task ReturnSubmissionAsync(
        IUserIdSet supplierUserIdSet,
        Guid dataShareRequestId,
        string commentsToAcquirer);

    Task<AcceptedDecisionSummaryModelData> GetAcceptedDecisionSummaryAsync(
        Guid dataShareRequestId);

    Task<RejectedDecisionSummaryModelData> GetRejectedDecisionSummaryAsync(
        Guid dataShareRequestId);

    Task<ReturnedDecisionSummaryModelData> GetReturnedDecisionSummaryAsync(
        Guid dataShareRequestId);

    Task<DataShareRequestStatusType> GetDataShareRequestStatusAsync(
        Guid dataShareRequestId);

    Task<DataShareRequestNotificationInformationModelData> GetDataShareRequestNotificationInformationAsync(
        Guid dataShareRequestId);
}