using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;

namespace Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest
{
    public interface ISupplierDataShareRequestService
    {
        Task<IServiceOperationDataResult<SubmissionSummariesSet>> GetSubmissionSummariesAsync();

        Task<IServiceOperationDataResult<SubmissionInformation>> GetSubmissionInformationAsync(
            Guid dataShareRequestId);

        Task<IServiceOperationDataResult<SubmissionDetails>> GetSubmissionDetailsAsync(
            Guid dataShareRequestId);

        Task<IServiceOperationDataResult<SubmissionContentAsFile>> GetSubmissionContentAsFileAsync(
            Guid dataShareRequestId,
            DataShareRequestFileFormat dataShareRequestFileFormat);

        Task<IServiceOperationDataResult<SubmissionReviewInformation>> StartSubmissionReviewAsync(
            Guid dataShareRequestId);

        Task<IServiceOperationDataResult<SubmissionReviewInformation>> GetSubmissionReviewInformationAsync(
            Guid dataShareRequestId);

        Task<IServiceOperationDataResult<ReturnedSubmissionInformation>> GetReturnedSubmissionInformationAsync(
            Guid dataShareRequestId);

        Task<IServiceOperationDataResult<CompletedSubmissionInformation>> GetCompletedSubmissionInformationAsync(
            Guid dataShareRequestId);

        Task<IServiceOperationResult> SetSubmissionNotesAsync(
            Guid dataShareRequestId,
            string notes);

        Task<IServiceOperationDataResult<DataShareRequestAcceptanceResult>> AcceptSubmissionAsync(
            Guid dataShareRequestId,
            string decisionFeedback);

        Task<IServiceOperationDataResult<DataShareRequestRejectionResult>> RejectSubmissionAsync(
            Guid dataShareRequestId,
            string decisionFeedback);

        Task<IServiceOperationDataResult<DataShareRequestReturnResult>> ReturnSubmissionAsync(
            Guid dataShareRequestId,
            string commentsToAcquirer);
    }
}
