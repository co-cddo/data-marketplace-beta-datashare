using System.Net;
using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.Repositories.AcquirerDataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestNotificationRecipientDeterminations;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.NextQuestionDeterminations;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Services.AuditLog;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas;
using Agrimetrics.DataShare.Api.Logic.Services.KeyQuestionPartAnswers;
using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.QuestionSetPlaceHolderReplacement;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Agrimetrics.DataShare.Api.Logic.Services.Users;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest;

internal class AcquirerDataShareRequestService(
    ILogger<AcquirerDataShareRequestService> logger,
    IUserProfilePresenter userProfilePresenter,
    IEsdaInformationPresenter esdaInformationPresenter,
    IAcquirerDataShareRequestRepository acquirerDataShareRequestRepository,
    IAuditLogService auditLogService,
    INotificationService notificationService,
    IAcquirerDataShareRequestModelDataFactory acquirerDataShareRequestModelDataFactory,
    IDataShareRequestQuestionAnswerValidationService dataShareRequestQuestionAnswerValidationService,
    IQuestionSetPlaceholderReplacementService questionSetPlaceholderReplacementService,
    IDataShareRequestQuestionStatusesDetermination dataShareRequestQuestionStatusesDetermination,
    IDataShareRequestQuestionSetCompletenessDetermination dataShareRequestQuestionSetCompletenessDetermination,
    INextQuestionDetermination nextQuestionDetermination,
    IDataShareRequestNotificationRecipientDetermination dataShareRequestNotificationRecipientDetermination,
    IKeyQuestionPartAnswerProviderService keyQuestionPartAnswerProviderService,
    IServiceOperationResultFactory serviceOperationResultFactory) : IAcquirerDataShareRequestService
{
    private readonly List<DataShareRequestStatusType> statusesThatAllowADataShareToBeModified =
        [DataShareRequestStatusType.Draft, DataShareRequestStatusType.Returned];

    #region Get Esda Question Set Outline Request
    async Task<IServiceOperationDataResult<QuestionSetOutline>> IAcquirerDataShareRequestService.GetEsdaQuestionSetOutlineRequestAsync(
        int supplierDomainId,
        int supplierOrganisationId,
        Guid esdaId)
    {
        try
        {
            var esdaDetails = await esdaInformationPresenter.GetEsdaDetailsByIdAsync(esdaId);

            var questionSetId = await acquirerDataShareRequestRepository.FindQuestionSetAsync(
                supplierDomainId,
                supplierOrganisationId,
                esdaId);

            var questionSetOutlineModelData = await acquirerDataShareRequestRepository.GetQuestionSetOutlineRequestAsync(
                questionSetId);

            questionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionSetOutlineModelData(
                questionSetOutlineModelData,
                esdaDetails.Title);

            var questionSetOutline = acquirerDataShareRequestModelDataFactory.CreateQuestionSetOutline(
                questionSetOutlineModelData);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(questionSetOutline);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetEsdaQuestionSetOutlineRequest");

            var response = serviceOperationResultFactory.CreateFailedDataResult<QuestionSetOutline>(ex.Message);

            return await Task.FromResult(response);
        }
    }
    #endregion

    #region Start Data Share Request
    async Task<IServiceOperationDataResult<Guid>> IAcquirerDataShareRequestService.StartDataShareRequestAsync(
        Guid esdaId)
    {
        var initiatingUserIdSet = await userProfilePresenter.GetInitiatingUserIdSetAsync();

        try
        {
            var esdaDetails = await esdaInformationPresenter.GetEsdaDetailsByIdAsync(esdaId);

            var supplierOrganisationId = esdaDetails.SupplierOrganisationId;
            var supplierDomainId = esdaDetails.SupplierDomainId;

            var questionSetId = await acquirerDataShareRequestRepository.FindQuestionSetAsync(
                supplierDomainId,
                supplierOrganisationId,
                esdaId);

            var dataShareRequestId = await acquirerDataShareRequestRepository.StartDataShareRequestAsync(
                initiatingUserIdSet,
                supplierDomainId,
                supplierOrganisationId,
                esdaId,
                esdaDetails.Title,
                questionSetId);

            await UpdateQuestionStatusesAsync(dataShareRequestId);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(dataShareRequestId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to StartDataShareRequest");

            var response = serviceOperationResultFactory.CreateFailedDataResult<Guid>(ex.Message);

            return await Task.FromResult(response);
        }
    }
    #endregion

    #region Get Data Share Request Summaries
    async Task<IServiceOperationDataResult<DataShareRequestSummarySet>> IAcquirerDataShareRequestService.GetDataShareRequestSummariesAsync(
        int? acquirerUserId,
        int? acquirerDomainId,
        int? acquirerOrganisationId,
        int? supplierDomainId,
        int? supplierOrganisationId,
        Guid? esdaId,
        IEnumerable<DataShareRequestStatus>? dataShareRequestStatuses)
    {
        try
        {
            var dataShareRequestModelDatas = await acquirerDataShareRequestRepository.GetDataShareRequestsAsync(
                acquirerUserId,
                acquirerDomainId,
                acquirerOrganisationId,
                supplierDomainId,
                supplierOrganisationId,
                esdaId,
                dataShareRequestStatuses);

            var dataShareRequestSummarySet = acquirerDataShareRequestModelDataFactory.CreateDataShareRequestSummarySet(
                dataShareRequestModelDatas);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(dataShareRequestSummarySet);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetDataShareRequestSummaries");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestSummarySet>(ex.Message);

            return await Task.FromResult(response);
        }
    }
    #endregion

    #region Get Data Share Request Admin Summaries
    async Task<IServiceOperationDataResult<DataShareRequestAdminSummarySet>> IAcquirerDataShareRequestService.GetDataShareRequestAdminSummariesAsync(
        int? acquirerOrganisationId,
        int? supplierOrganisationId,
        IEnumerable<DataShareRequestStatus>? dataShareRequestStatuses)
    {
        try
        {
            var dataShareRequestAdminSummaries = await DoGetDataShareRequestAdminSummariesAsync();
            
            var dataShareRequestAdminSummarySet = acquirerDataShareRequestModelDataFactory.CreateDataShareRequestAdminSummarySet(
                dataShareRequestAdminSummaries);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(dataShareRequestAdminSummarySet);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetDataShareRequestAdminSummaries");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestAdminSummarySet>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task<IEnumerable<DataShareRequestAdminSummary>> DoGetDataShareRequestAdminSummariesAsync()
        {
            var dataShareRequestModelDatas = (await acquirerDataShareRequestRepository.GetDataShareRequestsAsync(
                acquirerUserId: null,
                acquirerDomainId: null,
                acquirerOrganisationId: acquirerOrganisationId,
                supplierDomainId: null,
                supplierOrganisationId: supplierOrganisationId,
                esdaId: null,
                dataShareRequestStatuses: dataShareRequestStatuses)).ToList();

            var creatingUserEmailAddresses = await GetCreatingUserEmailAddressesAsync();

            var dataShareRequestAdminSummaries = new List<DataShareRequestAdminSummary>();

            foreach (var dataShareRequestModelData in dataShareRequestModelDatas)
            {
                var dataShareRequestId = dataShareRequestModelData.DataShareRequest_Id;

                var whenCreated = await DoGetWhenCreatedAsync(dataShareRequestId);

                var whenSubmitted = await DoGetWhenSubmittedAsync(dataShareRequestId);

                var createdByUserEmailAddress = DoGetCreatedByUserEmailAddress(dataShareRequestModelData.DataShareRequest_AcquirerUserId);

                var whenNeededBy = await keyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
                    dataShareRequestId);

                dataShareRequestAdminSummaries.Add(acquirerDataShareRequestModelDataFactory.CreateDataShareRequestAdminSummary(
                    dataShareRequestModelData,
                    whenCreated,
                    whenSubmitted,
                    createdByUserEmailAddress,
                    whenNeededBy));
            }

            return dataShareRequestAdminSummaries;

            async Task<IDictionary<int, string>> GetCreatingUserEmailAddressesAsync()
            {
                var creatingUserIds = dataShareRequestModelDatas.Select(x =>
                    x.DataShareRequest_AcquirerUserId).Distinct().ToList();

                var creatingUserDetails = await userProfilePresenter.GetUserDetailsByUserIdsAsync(creatingUserIds);

                return creatingUserDetails.ToDictionary(
                    x => x.UserIdSet.UserId,
                    x => x.UserContactDetails.EmailAddress);
            }

            async Task<DateTime> DoGetWhenCreatedAsync(Guid dataShareRequestId)
            {
                var auditLogsForChangingToDraftStatus= await auditLogService.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                    dataShareRequestId,
                    [DataShareRequestStatusType.Draft]);

                var auditLogForInitialChangeToDraftStatus = auditLogsForChangingToDraftStatus.FirstOrDefault(x =>
                    x?.AuditLogDataShareRequestStatusChange_FromStatus == DataShareRequestStatusType.None);

                return auditLogForInitialChangeToDraftStatus?.AuditLogDataShareRequestStatusChange_ChangedAtUtc ?? DateTime.MinValue;
            }

            async Task<DateTime?> DoGetWhenSubmittedAsync(Guid dataShareRequestId)
            {
                var auditLogsForChangingToDraftStatus = await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    dataShareRequestId,
                    DataShareRequestStatusType.Submitted);

                return auditLogsForChangingToDraftStatus?.AuditLogDataShareRequestStatusChange_ChangedAtUtc;
            }

            string DoGetCreatedByUserEmailAddress(int acquirerUserId)
            {
                return creatingUserEmailAddresses.TryGetValue(acquirerUserId, out var creatingUserEmailAddress)
                    ? creatingUserEmailAddress
                    : string.Empty;
            }
        }
    }
    #endregion

    #region Get Acquirer Data Share Request Summaries
    async Task<IServiceOperationDataResult<DataShareRequestSummarySet>> IAcquirerDataShareRequestService.GetAcquirerDataShareRequestSummariesAsync(
        int? supplierDomainId,
        int? supplierOrganisationId,
        Guid? esdaId,
        IEnumerable<DataShareRequestStatus>? dataShareRequestStatuses)
    {
        try
        {
            var initiatingUserIdSet = await userProfilePresenter.GetInitiatingUserIdSetAsync();

            var dataShareRequestModelDatas = await acquirerDataShareRequestRepository.GetDataShareRequestsAsync(
                initiatingUserIdSet.UserId,
                initiatingUserIdSet.DomainId,
                initiatingUserIdSet.OrganisationId,
                supplierDomainId,
                supplierOrganisationId,
                esdaId,
                dataShareRequestStatuses);

            var dataShareRequestSummarySet = acquirerDataShareRequestModelDataFactory.CreateDataShareRequestSummarySet(
                dataShareRequestModelDatas);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(dataShareRequestSummarySet);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetAcquirerDataShareRequestSummaries");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestSummarySet>(ex.Message);

            return await Task.FromResult(response);
        }
    }
    #endregion

    #region Get Data Share Request Summaries Raised For Esda By Acquirer Organisation
    async Task<IServiceOperationDataResult<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>> IAcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
        Guid esdaId)
    {
        try
        {
            var initiatingUserIdSet = await userProfilePresenter.GetInitiatingUserIdSetAsync();

            var dataShareRequestModelDatas = (await acquirerDataShareRequestRepository.GetDataShareRequestsAsync(
                acquirerUserId: null,
                acquirerDomainId: null,
                acquirerOrganisationId: initiatingUserIdSet.OrganisationId,
                supplierDomainId: null,
                supplierOrganisationId: null,
                esdaId: esdaId,
                dataShareRequestStatuses: null)).ToList();
            
            var dataShareRequestAcquirerUsersIds = dataShareRequestModelDatas
                .Select(x => x.DataShareRequest_AcquirerUserId).Distinct().ToList();

            var dataShareRequestAcquirerUserDetailsSet = dataShareRequestAcquirerUsersIds
                .Select(async userId => await userProfilePresenter.GetUserDetailsByUserIdAsync(userId))
                .Select(x => x.Result)
                .ToDictionary(x => x.UserIdSet.UserId, x => x);

            var dataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries = dataShareRequestModelDatas
                .Select(async x => await BuildDataShareRequestRaisedForEsdaByAcquirerOrganisationSummaryAsync(x))
                .Select(x => x.Result)
                .ToList();

            var esdaTitle = await GetEsdaTitleAsync();

            var dataShareRequestSummarySet = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet
            {
                EsdaId = esdaId,
                EsdaName = esdaTitle,
                DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries = dataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries
            };

            return serviceOperationResultFactory.CreateSuccessfulDataResult(dataShareRequestSummarySet);

            async Task<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary> BuildDataShareRequestRaisedForEsdaByAcquirerOrganisationSummaryAsync(
                DataShareRequestModelData dataShareRequestModelData)
            {
                var auditLogForCreation = await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    dataShareRequestModelData.DataShareRequest_Id,
                    DataShareRequestStatusType.Draft);

                if (auditLogForCreation == null) throw new InconsistentDataException("DataShareRequest has no audit log for creation");

                var auditLogForMostRecentSubmission = await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    dataShareRequestModelData.DataShareRequest_Id,
                    DataShareRequestStatusType.Submitted);

                var dataShareRequestAcquirerUserDetails = dataShareRequestAcquirerUserDetailsSet[dataShareRequestModelData.DataShareRequest_AcquirerUserId];

                return acquirerDataShareRequestModelDataFactory.CreateDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary(
                    dataShareRequestModelData,
                    auditLogForCreation,
                    auditLogForMostRecentSubmission,
                    dataShareRequestAcquirerUserDetails);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task<string> GetEsdaTitleAsync()
        {
            try
            {
                var esdaDetails = await esdaInformationPresenter.GetEsdaDetailsByIdAsync(esdaId);

                return !string.IsNullOrWhiteSpace(esdaDetails.Title) ? esdaDetails.Title : string.Empty;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get ESDA details in GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation");

                return string.Empty;
            }
        }
    }
    #endregion

    #region Get Data Share Request Questions Summary
    async Task<IServiceOperationDataResult<DataShareRequestQuestionsSummary>> IAcquirerDataShareRequestService.GetDataShareRequestQuestionsSummaryAsync(
        Guid dataShareRequestId)
    {
        try
        {
            var dataShareRequestQuestionsSummaryModelData = await acquirerDataShareRequestRepository.GetDataShareRequestQuestionsSummaryAsync(
                dataShareRequestId);

            DetermineWhetherQuestionsCanBeAnswered(dataShareRequestQuestionsSummaryModelData);

            DetermineWhetherSectionIsComplete(dataShareRequestQuestionsSummaryModelData);

            await PopulateSupplierOrganisationDetailsAsync(dataShareRequestQuestionsSummaryModelData);

            await PopulateSubmissionNotesFromSupplierAsync(dataShareRequestQuestionsSummaryModelData);

            await PopulateCancellationReasonsFromAcquirerAsync(dataShareRequestQuestionsSummaryModelData);

            var dataShareRequestQuestionsSummary = acquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionsSummary(
                dataShareRequestQuestionsSummaryModelData);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(dataShareRequestQuestionsSummary);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetDataShareRequestQuestionsSummary");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestQuestionsSummary>(ex.Message);

            return await Task.FromResult(response);
        }

        static void DetermineWhetherQuestionsCanBeAnswered(
            DataShareRequestQuestionsSummaryModelData dataShareRequestQuestionsSummaryModelData)
        {
            var questionStatusesThatCanBeAnswered = new List<QuestionStatusType>
            {
                QuestionStatusType.NotStarted, QuestionStatusType.Completed
            };

            var questionSummaries = dataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary
                .QuestionSet_SectionSummaries.SelectMany(x => x.QuestionSetSection_QuestionSummaries);

            foreach (var questionSummary in questionSummaries)
            {
                questionSummary.Question_QuestionCanBeAnswered = questionStatusesThatCanBeAnswered.Contains(questionSummary.Question_QuestionStatus);
            }
        }

        void DetermineWhetherSectionIsComplete(
            DataShareRequestQuestionsSummaryModelData dataShareRequestQuestionsSummaryModelData)
        {
            var allSectionSummaries = dataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary.QuestionSet_SectionSummaries;

            foreach (var sectionSummary in allSectionSummaries)
            {
                var questionSetSectionCompleteness = dataShareRequestQuestionSetCompletenessDetermination.DetermineDataShareRequestQuestionSetSectionCompleteness(sectionSummary);
                sectionSummary.QuestionSetSection_IsComplete = !questionSetSectionCompleteness.QuestionsRequiringAResponse.Any();
            }

            dataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary.QuestionSet_AnswersSectionComplete = 
                dataShareRequestQuestionsSummaryModelData.DataShareRequest_DataShareRequestStatus is
                    DataShareRequestStatusType.Submitted or DataShareRequestStatusType.Accepted or DataShareRequestStatusType.Rejected;
        }

        async Task PopulateSupplierOrganisationDetailsAsync(
            DataShareRequestQuestionsSummaryModelData dataShareRequestQuestionsSummaryModelData)
        {
            var supplierOrganisationDetails = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                dataShareRequestQuestionsSummaryModelData.DataShareRequest_SupplierOrganisationId);

            dataShareRequestQuestionsSummaryModelData.DataShareRequest_SupplierOrganisationName = supplierOrganisationDetails.OrganisationName;
        }

        async Task PopulateSubmissionNotesFromSupplierAsync(
            DataShareRequestQuestionsSummaryModelData dataShareRequestQuestionsSummaryModelData)
        {
            dataShareRequestQuestionsSummaryModelData.DataShareRequest_SubmissionResponseFromSupplier = await DoGetSubmissionResponseFromSupplierAsync(
                dataShareRequestQuestionsSummaryModelData.DataShareRequest_Id,
                dataShareRequestQuestionsSummaryModelData.DataShareRequest_DataShareRequestStatus);
        }

        async Task PopulateCancellationReasonsFromAcquirerAsync(
            DataShareRequestQuestionsSummaryModelData dataShareRequestQuestionsSummaryModelData)
        {
            dataShareRequestQuestionsSummaryModelData.DataShareRequest_CancellationReasonsFromAcquirer = await DoGetCancellationReasonsFromAcquirerAsync(
                dataShareRequestQuestionsSummaryModelData.DataShareRequest_Id,
                dataShareRequestQuestionsSummaryModelData.DataShareRequest_DataShareRequestStatus);
        }
    }
    #endregion

    #region Get Data Share Request Question Information
    async Task<IServiceOperationDataResult<DataShareRequestQuestion>> IAcquirerDataShareRequestService.GetDataShareRequestQuestionInformationAsync(
        Guid dataShareRequestId,
        Guid questionId)
    {
        try
        {
            var dataShareRequestQuestion = await DoGetDataShareRequestQuestionInformationAsync(
                dataShareRequestId, questionId);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(dataShareRequestQuestion);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetDataShareRequestQuestionInformation");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestQuestion>(ex.Message);

            return await Task.FromResult(response);
        }
    }

    private async Task<DataShareRequestQuestion> DoGetDataShareRequestQuestionInformationAsync(
        Guid dataShareRequestId,
        Guid questionId)
    {
        var dataShareRequestQuestionModelData = await acquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
            dataShareRequestId,
            questionId);

        var esdaName = await acquirerDataShareRequestRepository.GetDataShareRequestResourceNameAsync(
            dataShareRequestId);

        questionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionModelData(
            dataShareRequestQuestionModelData,
            esdaName);

        return acquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestion(
            dataShareRequestQuestionModelData);
    }
    #endregion

    #region Set Data Share Question Answer
    async Task<IServiceOperationDataResult<SetDataShareRequestQuestionAnswerResult>> IAcquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(
        DataShareRequestQuestionAnswer dataShareRequestQuestionAnswer)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestQuestionAnswer);

        try
        {
            await VerifyDataShareRequestQuestionAnswerCanBeSetAsync(dataShareRequestQuestionAnswer.DataShareRequestId);

            // Note: It is imperative that if an answer has been given then exactly one response within that answer has an order value of 1, as this is relied upon
            // in some SQL queries.  At one point the UI was not always adhering to this requirement, so we check for it here.
            foreach (var answerPart in dataShareRequestQuestionAnswer.AnswerParts)
            {
                var numberOfResponsesWithAnswerOrderOne = answerPart.AnswerPartResponses.Count(x => x.OrderWithinAnswerPart == 1);
                if (numberOfResponsesWithAnswerOrderOne != 1)
                {
                    throw new InconsistentDataException("At least one response in each answer part must have an order value of 1");
                }
            }
            
            var dataShareRequestQuestionAnswerValidationResult = await dataShareRequestQuestionAnswerValidationService.ValidateDataShareRequestQuestionAnswerAsync(
                dataShareRequestQuestionAnswer);
            
            if (!dataShareRequestQuestionAnswerValidationResult.AnswerIsValid)
            {
                var currentDataShareRequestQuestion = await DoGetDataShareRequestQuestionInformationAsync(
                    dataShareRequestQuestionAnswer.DataShareRequestId, dataShareRequestQuestionAnswer.QuestionId);

                var dataShareRequestQuestionWithValidationErrors = acquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionFromAnswer(
                    currentDataShareRequestQuestion,
                    dataShareRequestQuestionAnswer,
                    dataShareRequestQuestionAnswerValidationResult.ValidationErrors);

                var questionsRemainThatRequireAResponse = await acquirerDataShareRequestRepository.GetWhetherQuestionsRemainThatRequireAResponseAsync(
                    dataShareRequestQuestionAnswer.DataShareRequestId);

                var setDataShareRequestQuestionAnswerFailedValidationResult = new SetDataShareRequestQuestionAnswerResult
                {
                    AnswerIsValid = false,
                    QuestionInformation = dataShareRequestQuestionWithValidationErrors,
                    NextQuestionId = null,
                    DataShareRequestQuestionsRemainThatRequireAResponse = questionsRemainThatRequireAResponse
                };

                return serviceOperationResultFactory.CreateSuccessfulDataResult(setDataShareRequestQuestionAnswerFailedValidationResult);
            }

            var dataShareRequestQuestionAnswerWriteData =
                acquirerDataShareRequestModelDataFactory.CreateQuestionAnswerWriteData(dataShareRequestQuestionAnswer);

            await acquirerDataShareRequestRepository.SetDataShareRequestQuestionAnswerAsync(dataShareRequestQuestionAnswerWriteData);

            var dataShareRequestQuestionStatusesDeterminationResult = await UpdateQuestionStatusesAsync(dataShareRequestQuestionAnswer.DataShareRequestId);

            var updatedDataShareRequestQuestion = await DoGetDataShareRequestQuestionInformationAsync(
                dataShareRequestQuestionAnswer.DataShareRequestId, dataShareRequestQuestionAnswer.QuestionId);

            var dataShareRequestQuestionsRemainThatRequireAResponse = await acquirerDataShareRequestRepository.GetWhetherQuestionsRemainThatRequireAResponseAsync(
                    dataShareRequestQuestionAnswer.DataShareRequestId);

            var setDataShareRequestQuestionAnswerResult = new SetDataShareRequestQuestionAnswerResult
            {
                AnswerIsValid = true,
                QuestionInformation = updatedDataShareRequestQuestion,
                NextQuestionId = nextQuestionDetermination.DetermineNextQuestion(
                    dataShareRequestQuestionAnswer.QuestionId,
                    dataShareRequestQuestionStatusesDeterminationResult.QuestionStatusDeterminationResults.Select(x => x.QuestionSetQuestionStatusData)),
                DataShareRequestQuestionsRemainThatRequireAResponse = dataShareRequestQuestionsRemainThatRequireAResponse
            };

            return serviceOperationResultFactory.CreateSuccessfulDataResult(setDataShareRequestQuestionAnswerResult);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to SetDataShareRequestQuestionAnswer");

            var response = serviceOperationResultFactory.CreateFailedDataResult<SetDataShareRequestQuestionAnswerResult>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task VerifyDataShareRequestQuestionAnswerCanBeSetAsync(
            Guid dataShareRequestId)
        {
            var dataShareRequestStatus = await acquirerDataShareRequestRepository.GetDataShareRequestStatusAsync(
                dataShareRequestId);

            if (!statusesThatAllowADataShareToBeModified.Contains(dataShareRequestStatus.DataShareRequestStatus_RequestStatus))
            {
                throw new InvalidOperationException(
                    $"DataShareRequest status does not allow Question Answers to be set: '{dataShareRequestStatus.DataShareRequestStatus_RequestStatus}'");
            }
        }
    }
    #endregion

    #region Get Data Share Request Answers Summary
    async Task<IServiceOperationDataResult<DataShareRequestAnswersSummary>> IAcquirerDataShareRequestService.GetDataShareRequestAnswersSummaryAsync(
        Guid dataShareRequestId)
    {
        try
        {
            var dataShareRequestAnswersSummaryModelData = await acquirerDataShareRequestRepository.GetDataShareRequestAnswersSummaryAsync(
                dataShareRequestId);

            questionSetPlaceholderReplacementService.ReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData(
                dataShareRequestAnswersSummaryModelData,
                dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_EsdaName);

            await PopulateSubmissionResponseFromSupplierAsync(dataShareRequestAnswersSummaryModelData);

            await PopulateCancellationReasonsFromAcquirerAsync(dataShareRequestAnswersSummaryModelData);

            var dataShareRequestAnswersSummary = acquirerDataShareRequestModelDataFactory.CreateDataShareRequestAnswersSummary(
                    dataShareRequestAnswersSummaryModelData);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(dataShareRequestAnswersSummary);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetDataShareRequestAnswersSummary");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestAnswersSummary>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task PopulateSubmissionResponseFromSupplierAsync(
            DataShareRequestAnswersSummaryModelData dataShareRequestAnswersSummaryModelData)
        {
            dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SubmissionResponseFromSupplier = await DoGetSubmissionResponseFromSupplierAsync(
                dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_DataShareRequestId,
                dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_RequestStatus);
        }

        async Task PopulateCancellationReasonsFromAcquirerAsync(
            DataShareRequestAnswersSummaryModelData dataShareRequestAnswersSummaryModelData)
        {
            dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_CancellationReasonsFromAcquirer = await DoGetCancellationReasonsFromAcquirerAsync(
                dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_DataShareRequestId,
                dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_RequestStatus);
        }
    }
    #endregion

    #region Submit Data Share Request
    async Task<IServiceOperationDataResult<DataShareRequestSubmissionResult>> IAcquirerDataShareRequestService.SubmitDataShareRequestAsync(
        Guid dataShareRequestId)
    {
        try
        {
            var initiatingUserDetails = await userProfilePresenter.GetInitiatingUserDetailsAsync();

            await VerifyDataShareRequestAnswerCanBeSubmittedAsync();

            var submissionResultModelData = await acquirerDataShareRequestRepository.SubmitDataShareRequestAsync(
                initiatingUserDetails.UserIdSet, dataShareRequestId);

            var notificationsSuccess = await SendNotificationsAsync();

            var dataShareRequestSubmissionResult = acquirerDataShareRequestModelDataFactory.CreateDataShareRequestSubmissionResult(
                submissionResultModelData,
                notificationsSuccess);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(dataShareRequestSubmissionResult);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to SubmitDataShareRequest");

            var result = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestSubmissionResult>(ex.Message);

            return await Task.FromResult(result);
        }

        async Task VerifyDataShareRequestAnswerCanBeSubmittedAsync()
        {
            var dataShareRequestStatus = await acquirerDataShareRequestRepository.GetDataShareRequestStatusAsync(
                dataShareRequestId);

            if (!statusesThatAllowADataShareToBeModified.Contains(dataShareRequestStatus.DataShareRequestStatus_RequestStatus))
            {
                throw new InvalidOperationException(
                    $"DataShareRequest status does not allow it to be submitted: '{dataShareRequestStatus.DataShareRequestStatus_RequestStatus}'");
            }

            if (dataShareRequestStatus.DataShareRequestStatus_QuestionsRemainThatRequireAResponse)
            {
                throw new InvalidOperationException("DataShareRequest has questions that require a response");
            }
        }

        async Task<bool> SendNotificationsAsync()
        {
            try
            {
                var initiatingUserOrganisationInformation = await userProfilePresenter.GetInitiatingUserOrganisationInformationAsync();

                var dataShareRequestNotificationInformation = await acquirerDataShareRequestRepository.GetDataShareRequestNotificationInformationAsync(
                    dataShareRequestId);

                var dataShareRequestNotificationRecipient = await dataShareRequestNotificationRecipientDetermination.DetermineDataShareRequestNotificationRecipientAsync(
                        dataShareRequestNotificationInformation);

               var userDetails = await userProfilePresenter.GetUserDetailsByUserEmailAddressAsync(dataShareRequestNotificationRecipient.EmailAddress);

                if (userDetails == null || userDetails.UserIdSet.EmailNotification) 
                {
                    var result = await notificationService.SendToSupplierNewDataShareRequestReceivedNotificationAsync(
                    dataShareRequestNotificationRecipient.EmailAddress,
                    dataShareRequestNotificationRecipient.RecipientName,
                    initiatingUserOrganisationInformation.OrganisationName,
                    dataShareRequestNotificationInformation.EsdaName);

                    return result.Success;
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send notification of Data Share Request Submission");

                return false;
            }
        }
    }
    #endregion

    #region Cancel Data Share Request
    async Task<IServiceOperationDataResult<DataShareRequestCancellationResult>> IAcquirerDataShareRequestService.CancelDataShareRequestAsync(
        Guid dataShareRequestId,
        string reasonsForCancellation)
    {
        try
        {
            var initiatingUserDetails = await userProfilePresenter.GetInitiatingUserDetailsAsync();

            var dataShareRequestStatus = await GetDataShareRequestStatusAsync();

            VerifyDataShareRequestAnswerCanBeCancelled(dataShareRequestStatus);

            await acquirerDataShareRequestRepository.CancelDataShareRequestAsync(
                initiatingUserDetails.UserIdSet, dataShareRequestId, reasonsForCancellation);

            var notificationsSuccess = await SendNotificationsAsync(initiatingUserDetails.UserContactDetails);

            var dataShareRequestCancellationResult = acquirerDataShareRequestModelDataFactory.CreateDataShareRequestCancellationResult(
                dataShareRequestId,
                reasonsForCancellation,
                notificationsSuccess);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(dataShareRequestCancellationResult);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to CancelDataShareRequest");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestCancellationResult>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task<DataShareRequestStatusType> GetDataShareRequestStatusAsync()
        {
            var requestStatus =  await acquirerDataShareRequestRepository.GetDataShareRequestStatusAsync(
                dataShareRequestId);

            return requestStatus.DataShareRequestStatus_RequestStatus;
        }

        void VerifyDataShareRequestAnswerCanBeCancelled(DataShareRequestStatusType dataShareRequestStatus)
        {
            var cancellableDataShareRequestStatuses = new List<DataShareRequestStatusType>
            {
                DataShareRequestStatusType.Submitted, DataShareRequestStatusType.InReview
            };

            if (!cancellableDataShareRequestStatuses.Contains(dataShareRequestStatus))
            {
                throw new InvalidOperationException("DataShareRequest status does not allow it to be cancelled");
            }
        }

        async Task<bool> SendNotificationsAsync(
            IUserContactDetails initiatingUserContactDetails)
        {
            try
            {
                var dataShareRequestNotificationInformation = await acquirerDataShareRequestRepository.GetDataShareRequestNotificationInformationAsync(
                    dataShareRequestId);

                return await SendNotificationToSupplierAdminAsync(dataShareRequestNotificationInformation);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send notification of Data Share Request Cancellation");

                return false;
            }

            async Task<bool> SendNotificationToSupplierAdminAsync(
                DataShareRequestNotificationInformationModelData dataShareRequestNotificationInformation)
            {
                try
                {
                    var dataShareRequestNotificationRecipient = await dataShareRequestNotificationRecipientDetermination
                        .DetermineDataShareRequestNotificationRecipientAsync(dataShareRequestNotificationInformation);

                    var userDetails = await userProfilePresenter.GetUserDetailsByUserEmailAddressAsync(dataShareRequestNotificationRecipient.EmailAddress);

                    if (userDetails == null || userDetails.UserIdSet.EmailNotification)
                    {

                        var result = await notificationService.SendToSupplierDataShareRequestCancelledNotificationAsync(
                            dataShareRequestNotificationRecipient.EmailAddress,
                            dataShareRequestNotificationRecipient.RecipientName,
                            initiatingUserContactDetails.UserName,
                            dataShareRequestNotificationInformation.EsdaName,
                            dataShareRequestNotificationInformation.DataShareRequestRequestId,
                            reasonsForCancellation);

                        return result.Success;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to send notification of Data Share Request Cancellation");

                    return false;
                }
            }
        }
    }
    #endregion

    #region Delete Data Share Request
    async Task<IServiceOperationDataResult<DataShareRequestDeletionResult>> IAcquirerDataShareRequestService.DeleteDataShareRequestAsync(Guid dataShareRequestId)
    {
        try
        {
            var initiatingUserDetails = await userProfilePresenter.GetInitiatingUserDetailsAsync();

            var dataShareRequestExists = await acquirerDataShareRequestRepository.CheckIfDataShareRequestExistsAsync(dataShareRequestId);
            if (!dataShareRequestExists)
            {
                return serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestDeletionResult>(
                    "Data share request not found", HttpStatusCode.NotFound);
            }

            var dataShareRequestCanBeDeleted = await DetermineWhetherDataShareRequestCanBeDeletedAsync();
            if (!dataShareRequestCanBeDeleted)
            {
                return serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestDeletionResult>(
                    "DataShareRequest status does not allow it to be deleted", HttpStatusCode.Conflict);
            }

            await acquirerDataShareRequestRepository.DeleteDataShareRequestAsync(
                initiatingUserDetails.UserIdSet, dataShareRequestId);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(new DataShareRequestDeletionResult
            {
                DataShareRequestId = dataShareRequestId
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to CancelDataShareRequest");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestDeletionResult>(ex.Message);

            return response;
        }

        async Task<bool> DetermineWhetherDataShareRequestCanBeDeletedAsync()
        {
            var dataShareRequestStatusModelData = await acquirerDataShareRequestRepository.GetDataShareRequestStatusAsync(
                dataShareRequestId);

            var dataShareRequestStatus = dataShareRequestStatusModelData.DataShareRequestStatus_RequestStatus;

            return dataShareRequestStatus is DataShareRequestStatusType.Draft;
        }
    }
    #endregion

    #region Helpers
    #region Cancellation Reasons From Acquirer
    async Task<string?> DoGetCancellationReasonsFromAcquirerAsync(
        Guid dataShareRequestId,
        DataShareRequestStatusType dataShareRequestStatus)
    {
        if (!(dataShareRequestStatus is DataShareRequestStatusType.Cancelled))
        {
            return null;
        }

        var auditLogEntryForCurrentStatus = await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
            dataShareRequestId,
            dataShareRequestStatus);
        if (auditLogEntryForCurrentStatus == null)
        {
            return "Unable to find cancellation reasons from acquirer";
        }

        var auditLogComments = auditLogEntryForCurrentStatus.AuditLogDataShareRequestStatusChange_Comments
            .OrderBy(x => x.AuditLogDataShareRequestStatusChangeComment_CommentOrder)
            .Select(x => x.AuditLogDataShareRequestStatusChangeComment_Comment);

        return string.Join(Environment.NewLine, auditLogComments);
    }
    #endregion

    #region Submission Notes From Supplier
    async Task<string?> DoGetSubmissionResponseFromSupplierAsync(
        Guid dataShareRequestId,
        DataShareRequestStatusType dataShareRequestStatus)
    {
        if (!(dataShareRequestStatus is DataShareRequestStatusType.Accepted or DataShareRequestStatusType.Rejected or DataShareRequestStatusType.Returned))
        {
            return null;
        }

        var auditLogEntryForCurrentStatus = await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
            dataShareRequestId,
            dataShareRequestStatus);
        if (auditLogEntryForCurrentStatus == null)
        {
            return "Unable to find decision comments from supplier";
        }

        var auditLogComments = auditLogEntryForCurrentStatus.AuditLogDataShareRequestStatusChange_Comments
            .OrderBy(x => x.AuditLogDataShareRequestStatusChangeComment_CommentOrder)
            .Select(x => x.AuditLogDataShareRequestStatusChangeComment_Comment);

        return string.Join(Environment.NewLine, auditLogComments);
    }
    #endregion

    #region Question Status Refresh
    private async Task<IDataShareRequestQuestionStatusesDeterminationResult> UpdateQuestionStatusesAsync(Guid dataShareRequestId)
    {
        var dataShareRequestQuestionStatusInformations = await acquirerDataShareRequestRepository.GetDataShareRequestQuestionStatusInformationsAsync(
            dataShareRequestId);

        var dataShareRequestQuestionStatusesDeterminationResult = dataShareRequestQuestionStatusesDetermination.DetermineQuestionStatuses(
            dataShareRequestQuestionStatusInformations);

        var updatedDataShareRequestQuestionStatuses =
            dataShareRequestQuestionStatusesDeterminationResult.QuestionStatusDeterminationResults
                .Where(x => x.QuestionSetQuestionStatusData.QuestionStatus != x.PreviousQuestionStatus)
                .Select(x => new DataShareRequestQuestionStatusDataModel
                {
                    QuestionId = x.QuestionSetQuestionStatusData.QuestionId,
                    QuestionStatus = x.QuestionSetQuestionStatusData.QuestionStatus
                });

        await acquirerDataShareRequestRepository.UpdateDataShareRequestQuestionStatusesAsync(
            dataShareRequestId,
            dataShareRequestQuestionStatusesDeterminationResult.QuestionsRemainThatRequireAResponse,
            updatedDataShareRequestQuestionStatuses);

        return dataShareRequestQuestionStatusesDeterminationResult;
    }
    #endregion
    #endregion
}