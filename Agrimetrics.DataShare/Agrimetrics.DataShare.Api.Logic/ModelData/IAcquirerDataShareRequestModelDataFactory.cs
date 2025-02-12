using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

namespace Agrimetrics.DataShare.Api.Logic.ModelData;

public interface IAcquirerDataShareRequestModelDataFactory
{
    #region Model Data To Dto Data
    QuestionSetOutline CreateQuestionSetOutline(
        QuestionSetOutlineModelData questionSetOutlineModelData);

    DataShareRequestSummarySet CreateDataShareRequestSummarySet(
        IEnumerable<DataShareRequestModelData> dataShareRequestModelDatas);

    DataShareRequestAdminSummarySet CreateDataShareRequestAdminSummarySet(
        IEnumerable<DataShareRequestAdminSummary> dataShareRequestAdminSummaries);

    DataShareRequestAdminSummary CreateDataShareRequestAdminSummary(
        DataShareRequestModelData dataShareRequestModelData,
        DateTime whenCreated,
        DateTime? whenSubmitted,
        string createdByUserEmailAddresses,
        DateTime? whenNeededBy);

    DataShareRequestQuestionsSummary CreateDataShareRequestQuestionsSummary(
        DataShareRequestQuestionsSummaryModelData dataShareRequestQuestionsSummaryModelData);

    DataShareRequestQuestion CreateDataShareRequestQuestion(
        DataShareRequestQuestionModelData dataShareRequestQuestionModelData);

    DataShareRequestAnswersSummary CreateDataShareRequestAnswersSummary(
        DataShareRequestAnswersSummaryModelData dataShareRequestAnswersSummaryModelData);

    DataShareRequestSubmissionResult CreateDataShareRequestSubmissionResult(
        DataShareRequestSubmissionResultModelData submissionResultModelData,
        bool notificationsSuccess);

    DataShareRequestCancellationResult CreateDataShareRequestCancellationResult(
        Guid dataShareRequestId,
        string reasonsForCancellation,
        bool notificationsSuccess);

    DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary CreateDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary(
        DataShareRequestModelData dataShareRequestModelData,
        AuditLogDataShareRequestStatusChangeModelData auditLogForCreation,
        AuditLogDataShareRequestStatusChangeModelData? auditLogForMostRecentSubmission,
        IUserDetails dataShareRequestAcquirerUserDetails);
    #endregion

    #region Dto Data To Model Data
    DataShareRequestQuestionAnswerWriteModelData CreateQuestionAnswerWriteData(
        DataShareRequestQuestionAnswer questionAnswer);
    #endregion

    #region General
    DataShareRequestQuestion CreateDataShareRequestQuestionFromAnswer(
        DataShareRequestQuestion dataShareRequestQuestion,
        DataShareRequestQuestionAnswer dataShareRequestQuestionAnswer,
        IEnumerable<SetDataShareRequestQuestionAnswerPartResponseValidationError> questionAnswerPartResponseValidationErrors);
    #endregion
}