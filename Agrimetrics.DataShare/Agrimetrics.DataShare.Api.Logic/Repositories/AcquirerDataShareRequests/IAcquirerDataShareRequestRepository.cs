using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AcquirerDataShareRequests;

public interface IAcquirerDataShareRequestRepository
{
    Task<Guid> FindQuestionSetAsync(
        int? supplierDomainId,
        int supplierOrganisationId,
        Guid esdaId);

    Task<QuestionSetOutlineModelData> GetQuestionSetOutlineRequestAsync(
        Guid questionSetId);

    Task<string> GetDataShareRequestResourceNameAsync(
        Guid dataShareRequestId);

    Task<Guid> StartDataShareRequestAsync(
        IUserIdSet acquirerUserIdSet,
        int supplierDomainId,
        int supplierOrganisationId,
        Guid esdaId,
        string esdaName,
        Guid questionSetId);

    Task<DataShareRequestStatusTypeModelData> GetDataShareRequestStatusAsync(
        Guid dataShareRequestId);

    Task UpdateDataShareRequestQuestionStatusesAsync(
        Guid dataShareRequestId,
        bool questionsRemainThatRequireAResponse,
        IEnumerable<IDataShareRequestQuestionStatusDataModel> dataShareRequestQuestionStatuses);

    Task<IEnumerable<DataShareRequestModelData>> GetDataShareRequestsAsync(
        int? acquirerUserId,
        int? acquirerDomainId,
        int? acquirerOrganisationId,
        int? supplierDomainId,
        int? supplierOrganisationId,
        Guid? esdaId,
        IEnumerable<DataShareRequestStatus>? dataShareRequestStatuses);

    Task<DataShareRequestQuestionsSummaryModelData> GetDataShareRequestQuestionsSummaryAsync(
        Guid dataShareRequestId);

    Task<DataShareRequestQuestionModelData> GetDataShareRequestQuestionAsync(
        Guid dataShareRequestId,
        Guid questionId);

    Task<DataShareRequestQuestionStatusInformationSetModelData> GetDataShareRequestQuestionStatusInformationsAsync(
        Guid dataShareRequestId);

    Task SetDataShareRequestQuestionAnswerAsync(
        DataShareRequestQuestionAnswerWriteModelData questionAnswerWriteData);

    Task<DataShareRequestSubmissionResultModelData> SubmitDataShareRequestAsync(
        IUserIdSet acquirerUserIdSet,
        Guid dataShareRequestId);

    Task<DataShareRequestAnswersSummaryModelData> GetDataShareRequestAnswersSummaryAsync(
        Guid dataShareRequestId);

    Task<DataShareRequestNotificationInformationModelData> GetDataShareRequestNotificationInformationAsync(
        Guid dataShareRequestId);

    Task CancelDataShareRequestAsync(
        IUserIdSet acquirerUserIdSet,
        Guid dataShareRequestId,
        string reasonsForCancellation);

    Task DeleteDataShareRequestAsync(
        IUserIdSet acquirerUserIdSet,
        Guid dataShareRequestId);

    Task<bool> GetWhetherQuestionsRemainThatRequireAResponseAsync(
        Guid dataShareRequestId);

    Task<bool> CheckIfDataShareRequestExistsAsync(
        Guid dataShareRequestId);
}