using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest;

public interface IAcquirerDataShareRequestService
{
    Task<IServiceOperationDataResult<QuestionSetOutline>> GetEsdaQuestionSetOutlineRequestAsync(
        int supplierDomainId,
        int supplierOrganisationId,
        Guid esdaId);

    Task<IServiceOperationDataResult<Guid>> StartDataShareRequestAsync(
        Guid esdaId);

    Task<IServiceOperationDataResult<DataShareRequestSummarySet>> GetDataShareRequestSummariesAsync(
        int? acquirerUserId,
        int? acquirerDomainId,
        int? acquirerOrganisationId,
        int? supplierDomainId,
        int? supplierOrganisationId,
        Guid? esdaId,
        IEnumerable<DataShareRequestStatus>? dataShareRequestStatuses);

    Task<IServiceOperationDataResult<DataShareRequestAdminSummarySet>> GetDataShareRequestAdminSummariesAsync(
        int? acquirerOrganisationId,
        int? supplierOrganisationId,
        IEnumerable<DataShareRequestStatus>? dataShareRequestStatuses);

    Task<IServiceOperationDataResult<DataShareRequestSummarySet>> GetAcquirerDataShareRequestSummariesAsync(
        int? supplierDomainId,
        int? supplierOrganisationId,
        Guid? esdaId,
        IEnumerable<DataShareRequestStatus>? dataShareRequestStatuses);

    Task<IServiceOperationDataResult<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>> GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
        Guid esdaId);

    Task<IServiceOperationDataResult<DataShareRequestQuestionsSummary>> GetDataShareRequestQuestionsSummaryAsync(
        Guid dataShareRequestId);

    Task<IServiceOperationDataResult<DataShareRequestQuestion>> GetDataShareRequestQuestionInformationAsync(
        Guid dataShareRequestId,
        Guid questionId);

    Task<IServiceOperationDataResult<SetDataShareRequestQuestionAnswerResult>> SetDataShareRequestQuestionAnswerAsync(
        DataShareRequestQuestionAnswer dataShareRequestQuestionAnswer);

    Task<IServiceOperationDataResult<DataShareRequestAnswersSummary>> GetDataShareRequestAnswersSummaryAsync(
        Guid dataShareRequestId);

    Task<IServiceOperationDataResult<DataShareRequestSubmissionResult>> SubmitDataShareRequestAsync(
        Guid dataShareRequestId);

    Task<IServiceOperationDataResult<DataShareRequestCancellationResult>> CancelDataShareRequestAsync(
        Guid dataShareRequestId,
        string reasonsForCancellation);

    Task<IServiceOperationDataResult<DataShareRequestDeletionResult>> DeleteDataShareRequestAsync(
        Guid dataShareRequestId);
}