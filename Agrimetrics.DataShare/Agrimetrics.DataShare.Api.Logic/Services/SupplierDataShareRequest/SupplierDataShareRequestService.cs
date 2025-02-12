using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using Agrimetrics.DataShare.Api.Logic.Repositories.SupplierDataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerHighlights;
using Agrimetrics.DataShare.Api.Logic.Services.AuditLog;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas;
using Agrimetrics.DataShare.Api.Logic.Services.KeyQuestionPartAnswers;
using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.QuestionSetPlaceHolderReplacement;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest.SubmissionContentFileBuilding;
using Agrimetrics.DataShare.Api.Logic.Services.Users;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest;

internal class SupplierDataShareRequestService(
    ILogger<SupplierDataShareRequestService> logger,
    IUserProfilePresenter userProfilePresenter,
    ISupplierDataShareRequestRepository supplierDataShareRequestRepository,
    IKeyQuestionPartAnswerProviderService keyQuestionPartAnswerProviderService,
    IAnswerHighlightsService answerHighlightsProviderService,
    IQuestionSetPlaceholderReplacementService questionSetPlaceholderReplacementService,
    IEsdaInformationPresenter esdaInformationPresenter,
    IAuditLogService auditLogService,
    INotificationService notificationService,
    ISubmissionContentPdfFileBuilder submissionContentPdfFileBuilder,
    ISupplierDataShareRequestModelDataFactory supplierDataShareRequestModelDataFactory,
    IServiceOperationResultFactory serviceOperationResultFactory) : ISupplierDataShareRequestService
{
    async Task<IServiceOperationDataResult<SubmissionSummariesSet>> ISupplierDataShareRequestService.GetSubmissionSummariesAsync()
    {
        try
        {
            var initiatingUserIdSet = await userProfilePresenter.GetInitiatingUserIdSetAsync();

            var pendingSubmissionSummaries = await BuildPendingSubmissionSummariesAsync(initiatingUserIdSet);

            var completedSubmissionSummaries = await BuildCompletedSubmissionSummariesAsync(initiatingUserIdSet);

            var submissionSummarySet = supplierDataShareRequestModelDataFactory.CreateSubmissionSummarySet(
                pendingSubmissionSummaries,
                completedSubmissionSummaries);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(submissionSummarySet);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetSubmissionSummaries");

            var response = serviceOperationResultFactory.CreateFailedDataResult<SubmissionSummariesSet>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task<IEnumerable<PendingSubmissionSummaryModelData>> BuildPendingSubmissionSummariesAsync(
            IUserIdSet initiatingUserIdSet)
        {
            var pendingSubmissionSummaries = (await supplierDataShareRequestRepository.GetPendingSubmissionSummariesAsync(
                initiatingUserIdSet.OrganisationId)).ToList();

            foreach (var pendingSubmissionSummary in pendingSubmissionSummaries)
            {
                await PopulateTimestampInformationAsync(pendingSubmissionSummary);

                await PopulateAcquirerInformationAsync(pendingSubmissionSummary);

                await PopulateKeyQuestionInformationAsync(pendingSubmissionSummary);
            }

            return pendingSubmissionSummaries;

            async Task PopulateTimestampInformationAsync(PendingSubmissionSummaryModelData pendingSubmissionSummary)
            {
                var submittedAuditLogEntry = await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    pendingSubmissionSummary.PendingSubmissionSummary_DataShareRequestId,
                    DataShareRequestStatusType.Submitted);

                pendingSubmissionSummary.PendingSubmissionSummary_SubmittedOn = submittedAuditLogEntry?.AuditLogDataShareRequestStatusChange_ChangedAtUtc
                    ?? throw new InconsistentDataException("Pending submission has no Submitted entry in the audit log");
            }

            async Task PopulateAcquirerInformationAsync(PendingSubmissionSummaryModelData pendingSubmissionSummary)
            {
                var acquirerOrganisation = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                    pendingSubmissionSummary.PendingSubmissionSummary_AcquirerOrganisationId);
                
                pendingSubmissionSummary.PendingSubmissionSummary_AcquirerOrganisationName = acquirerOrganisation.OrganisationName;
            }

            async Task PopulateKeyQuestionInformationAsync(PendingSubmissionSummaryModelData pendingSubmissionSummary)
            {
                pendingSubmissionSummary.PendingSubmissionSummary_WhenNeededBy =
                    await keyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(pendingSubmissionSummary.PendingSubmissionSummary_DataShareRequestId);
            }
        }

        async Task<IEnumerable<CompletedSubmissionSummaryModelData>> BuildCompletedSubmissionSummariesAsync(
            IUserIdSet initiatingUserIdSet)
        {
            var completedSubmissionSummaries = (await supplierDataShareRequestRepository.GetCompletedSubmissionSummariesAsync(
                initiatingUserIdSet.OrganisationId)).ToList();

            foreach (var completedSubmissionSummary in completedSubmissionSummaries)
            {
                await PopulateTimestampInformationAsync(completedSubmissionSummary);

                await PopulateAcquirerInformationAsync(completedSubmissionSummary);
            }

            return completedSubmissionSummaries;

            async Task PopulateTimestampInformationAsync(CompletedSubmissionSummaryModelData completedSubmissionSummary)
            {
                var submittedAuditLogEntry = await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    completedSubmissionSummary.CompletedSubmissionSummary_DataShareRequestId,
                    DataShareRequestStatusType.Submitted);

                completedSubmissionSummary.CompletedSubmissionSummary_SubmittedOn = submittedAuditLogEntry?.AuditLogDataShareRequestStatusChange_ChangedAtUtc
                    ?? throw new InconsistentDataException("Completed submission has no Submitted entry in the audit log");

                var completedAuditLogEntry = await GetCompletedSubmissionTimestampInformationAsync();

                completedSubmissionSummary.CompletedSubmissionSummary_CompletedOn = completedAuditLogEntry?.AuditLogDataShareRequestStatusChange_ChangedAtUtc
                    ?? throw new InconsistentDataException("Completed submission has no completed entry in the audit log");

                async Task<AuditLogDataShareRequestStatusChangeModelData?> GetCompletedSubmissionTimestampInformationAsync()
                {
                    return await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(completedSubmissionSummary.CompletedSubmissionSummary_DataShareRequestId, DataShareRequestStatusType.Accepted)
                           ?? await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(completedSubmissionSummary.CompletedSubmissionSummary_DataShareRequestId, DataShareRequestStatusType.Rejected);
                }
            }

            async Task PopulateAcquirerInformationAsync(CompletedSubmissionSummaryModelData completedSubmissionSummary)
            {
                var acquirerOrganisation = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                    completedSubmissionSummary.CompletedSubmissionSummary_AcquirerOrganisationId);

                completedSubmissionSummary.CompletedSubmissionSummary_AcquirerOrganisationName = acquirerOrganisation.OrganisationName;
            }
        }
    }

    async Task<IServiceOperationDataResult<SubmissionInformation>> ISupplierDataShareRequestService.GetSubmissionInformationAsync(
        Guid dataShareRequestId)
    {
        try
        {
            var submissionInformation = await DoGetSubmissionInformationAsync(dataShareRequestId);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(submissionInformation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetSubmissionToReviewSummary");

            var response = serviceOperationResultFactory.CreateFailedDataResult<SubmissionInformation>(ex.Message);

            return await Task.FromResult(response);
        }
    }

    private async Task<SubmissionInformation> DoGetSubmissionInformationAsync(Guid dataShareRequestId)
    {
        var submissionInformationModelData = await supplierDataShareRequestRepository.GetSubmissionInformationModelDataAsync(
            dataShareRequestId);

        await PopulateAuditInformationAsync();

        await PopulateAcquirerInformationAsync();

        await PopulateKeyQuestionInformationAsync();

        await PopulateAnswerHighlightsAsync();

        return supplierDataShareRequestModelDataFactory.CreateSubmissionInformation(submissionInformationModelData);

        async Task PopulateAuditInformationAsync()
        {
            var submittedAuditLogEntry = await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                dataShareRequestId, DataShareRequestStatusType.Submitted);

            if (submittedAuditLogEntry == null)
                throw new InconsistentDataException("Returned submission has no Submitted entry in the audit log");

            submissionInformationModelData.SubmissionInformation_SubmittedOn = submittedAuditLogEntry.AuditLogDataShareRequestStatusChange_ChangedAtUtc;
        }

        async Task PopulateAcquirerInformationAsync()
        {
            var acquirerUserDetails = await userProfilePresenter.GetUserDetailsByUserIdAsync(
                submissionInformationModelData.SubmissionInformation_AcquirerUserId);
            submissionInformationModelData.SubmissionInformation_AcquirerEmailAddress = acquirerUserDetails.UserContactDetails.EmailAddress;

            var acquirerOrganisation = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                submissionInformationModelData.SubmissionInformation_AcquirerOrganisationId);
            submissionInformationModelData.SubmissionInformation_AcquirerOrganisationName = acquirerOrganisation.OrganisationName;
        }

        async Task PopulateKeyQuestionInformationAsync()
        {
            submissionInformationModelData.SubmissionInformation_ProjectAims =
                await keyQuestionPartAnswerProviderService.GetProjectAimsQuestionPartAnswerAsync(submissionInformationModelData.SubmissionInformation_DataShareRequestId)
                ?? throw new InconsistentDataException("Submission has no completed Project Aims");

            submissionInformationModelData.SubmissionInformation_DataTypes =
                (await keyQuestionPartAnswerProviderService.GetDataTypesQuestionPartAnswerAsync(submissionInformationModelData.SubmissionInformation_DataShareRequestId)).ToList();

            submissionInformationModelData.SubmissionInformation_WhenNeededBy =
                await keyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(submissionInformationModelData.SubmissionInformation_DataShareRequestId);
        }

        async Task PopulateAnswerHighlightsAsync()
        {
            var dataShareRequestAnswerHighlights = await answerHighlightsProviderService.GetDataShareRequestsAnswerHighlightsAsync(
                    submissionInformationModelData.SubmissionInformation_DataShareRequestId);

            submissionInformationModelData.SubmissionInformation_AnswerHighlights = dataShareRequestAnswerHighlights.ToList();
        }
    }

    async Task<IServiceOperationDataResult<SubmissionDetails>> ISupplierDataShareRequestService.GetSubmissionDetailsAsync(
        Guid dataShareRequestId)
    {
        try
        {
            var submissionDetailsModelData = await DoGetSubmissionDetailsModelDataAsync();

            questionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionDetailsModelData(
                submissionDetailsModelData, submissionDetailsModelData.SubmissionDetails_EsdaName);

            await DoPopulateAcquirerInformationAsync(submissionDetailsModelData);

            await DoPopulateSubmissionDetailsReturnCommentsAsync(submissionDetailsModelData);

            var submissionDetails = supplierDataShareRequestModelDataFactory.CreateSubmissionDetails(
                submissionDetailsModelData);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(submissionDetails);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetSubmissionDetails");

            var response = serviceOperationResultFactory.CreateFailedDataResult<SubmissionDetails>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task<SubmissionDetailsModelData> DoGetSubmissionDetailsModelDataAsync()
        {
            try
            {
                return await supplierDataShareRequestRepository.GetSubmissionDetailsModelDataAsync(dataShareRequestId);
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to GetSubmissionDetailsModelData from repository";

                logger.LogError(ex, errorMessage);

                throw new DataShareRequestGeneralException(errorMessage, ex);
            }
        }

        async Task DoPopulateAcquirerInformationAsync(SubmissionDetailsModelData submissionDetailsModelData)
        {
            var acquirerOrganisation = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                submissionDetailsModelData.SubmissionDetails_AcquirerOrganisationId);

            submissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName = acquirerOrganisation.OrganisationName;
        }
    }
    
    async Task<IServiceOperationDataResult<SubmissionContentAsFile>> ISupplierDataShareRequestService.GetSubmissionContentAsFileAsync(
        Guid dataShareRequestId,
        DataShareRequestFileFormat dataShareRequestFileFormat)
    {
        try
        {
            var dataShareRequestStatus = await supplierDataShareRequestRepository.GetDataShareRequestStatusAsync(dataShareRequestId);
            if (dataShareRequestStatus is DataShareRequestStatusType.Draft or DataShareRequestStatusType.None)
            {
                throw new InconsistentDataException("Unable to get submission content for Data Share Request with its current status");
            }

            var submissionInformationModelData = await DoGetSubmissionInformationAsync(dataShareRequestId);

            var submissionDetails = await DoGetSubmissionDetailsAsync(dataShareRequestId);

            var submissionContentAsFile = await BuildSubmissionContentAsFileAsync(
                submissionInformationModelData, submissionDetails);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(submissionContentAsFile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetSubmissionContentAsFile");

            var response = serviceOperationResultFactory.CreateFailedDataResult<SubmissionContentAsFile>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task<SubmissionContentAsFile> BuildSubmissionContentAsFileAsync(
            SubmissionInformation submissionInformation,
            SubmissionDetails submissionDetails)
        {
            return dataShareRequestFileFormat switch
            {
                DataShareRequestFileFormat.Pdf => await BuildSubmissionContentAsPdfAsync(),
                _ => throw new InvalidEnumValueException("Unhandled file format requested")
            };

            async Task<SubmissionContentAsFile> BuildSubmissionContentAsPdfAsync()
            {
                return new SubmissionContentAsFile
                {
                    Content = await submissionContentPdfFileBuilder.BuildAsync(
                        submissionInformation,
                        submissionDetails),
                    ContentType = "application/pdf",
                    FileName = $"DSR-{dataShareRequestId}-{DateTime.Now:yyyyMMdd HHmmss}.pdf"
                };
            }
        }
    }

    private async Task<SubmissionDetails> DoGetSubmissionDetailsAsync(
        Guid dataShareRequestId)
    {
        var submissionDetailsModelData = await DoGetSubmissionDetailsModelDataAsync();

        questionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionDetailsModelData(
            submissionDetailsModelData, submissionDetailsModelData.SubmissionDetails_EsdaName);

        await PopulateAcquirerInformationAsync();

        return supplierDataShareRequestModelDataFactory.CreateSubmissionDetails(submissionDetailsModelData);

        async Task<SubmissionDetailsModelData> DoGetSubmissionDetailsModelDataAsync()
        {
            try
            {
                return await supplierDataShareRequestRepository.GetSubmissionDetailsModelDataAsync(dataShareRequestId);
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to GetSubmissionDetailsModelData from repository";

                logger.LogError(ex, errorMessage);

                throw new DataShareRequestGeneralException(errorMessage, ex);
            }
        }

        async Task PopulateAcquirerInformationAsync()
        {
            var acquirerOrganisation = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                submissionDetailsModelData.SubmissionDetails_AcquirerOrganisationId);

            submissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName = acquirerOrganisation.OrganisationName;
        }
    }

    async Task<IServiceOperationDataResult<SubmissionReviewInformation>> ISupplierDataShareRequestService.StartSubmissionReviewAsync(Guid dataShareRequestId)
    {
        try
        {
            var dataShareRequestStatus = await supplierDataShareRequestRepository.GetDataShareRequestStatusAsync(dataShareRequestId);

            if (dataShareRequestStatus != DataShareRequestStatusType.Cancelled)
            {
                // If it's been cancelled then we could do lots of interesting stuff here, but the simplest is to just not allow this
                // request through, and return the status in the returned information
                var initiatingUserIdSet = await userProfilePresenter.GetInitiatingUserIdSetAsync();

                await DoStartSubmissionReviewAsync(initiatingUserIdSet);
            }

            var submissionReviewInformationModelData = await DoGetSubmissionReviewInformationModelDataAsync(dataShareRequestId);

            var submissionReviewInformation = supplierDataShareRequestModelDataFactory.CreateSubmissionReviewInformation(
                submissionReviewInformationModelData);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(submissionReviewInformation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to StartSubmissionReview");

            var response = serviceOperationResultFactory.CreateFailedDataResult<SubmissionReviewInformation>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task DoStartSubmissionReviewAsync(IUserIdSet supplierUserIdSet)
        {
            try
            {
                await supplierDataShareRequestRepository.StartSubmissionReviewAsync(
                    supplierUserIdSet, dataShareRequestId);
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to StartSubmissionReview";

                logger.LogError(ex, errorMessage);

                throw new DataShareRequestGeneralException(errorMessage, ex);
            }
        }
    }

    async Task<IServiceOperationDataResult<SubmissionReviewInformation>> ISupplierDataShareRequestService.GetSubmissionReviewInformationAsync(
        Guid dataShareRequestId)
    {
        try
        {
            var submissionReviewInformationModelData = await DoGetSubmissionReviewInformationModelDataAsync(dataShareRequestId);

            var submissionReviewInformation = supplierDataShareRequestModelDataFactory.CreateSubmissionReviewInformation(
                submissionReviewInformationModelData);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(submissionReviewInformation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetSubmissionReview");

            var response = serviceOperationResultFactory.CreateFailedDataResult<SubmissionReviewInformation>(ex.Message);

            return await Task.FromResult(response);
        }
    }

    async Task<IServiceOperationDataResult<ReturnedSubmissionInformation>> ISupplierDataShareRequestService.GetReturnedSubmissionInformationAsync(
        Guid dataShareRequestId)
    {
        try
        {
            var returnedSubmissionInformationModelData = await supplierDataShareRequestRepository.GetReturnedSubmissionInformationAsync(
                dataShareRequestId);

            await PopulateAuditInformationAsync(returnedSubmissionInformationModelData);

            await PopulateAcquirerInformationAsync(returnedSubmissionInformationModelData);

            await PopulateKeyQuestionInformationAsync(returnedSubmissionInformationModelData);

            var returnedSubmissionInformation = supplierDataShareRequestModelDataFactory.CreateReturnedSubmissionInformation(
                returnedSubmissionInformationModelData);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(returnedSubmissionInformation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetReturnedSubmissionInformation");

            var response = serviceOperationResultFactory.CreateFailedDataResult<ReturnedSubmissionInformation>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task PopulateAuditInformationAsync(ReturnedSubmissionInformationModelData returnedSubmissionInformationModelData)
        {
            var submittedAuditLogEntry = await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                dataShareRequestId, DataShareRequestStatusType.Submitted);

            if (submittedAuditLogEntry == null)
                throw new InconsistentDataException("Returned submission has no Submitted entry in the audit log");

            returnedSubmissionInformationModelData.ReturnedSubmission_SubmittedOn = submittedAuditLogEntry.AuditLogDataShareRequestStatusChange_ChangedAtUtc;

            var returnedAuditLogEntry = await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                dataShareRequestId, DataShareRequestStatusType.Returned);

            if (returnedAuditLogEntry == null)
                throw new InconsistentDataException("Returned submission has no Returned entry in the audit log");

            returnedSubmissionInformationModelData.ReturnedSubmission_ReturnedOn = returnedAuditLogEntry.AuditLogDataShareRequestStatusChange_ChangedAtUtc;

            returnedSubmissionInformationModelData.ReturnedSubmission_FeedbackProvided = DoBuildSubmissionCommentsFromAuditLogEntry(returnedAuditLogEntry);
        }

        async Task PopulateAcquirerInformationAsync(ReturnedSubmissionInformationModelData returnedSubmissionInformationModelData)
        {
            var acquirerOrganisation = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                returnedSubmissionInformationModelData.ReturnedSubmission_AcquirerOrganisationId);

            returnedSubmissionInformationModelData.ReturnedSubmission_AcquirerOrganisationName = acquirerOrganisation.OrganisationName;
        }

        async Task PopulateKeyQuestionInformationAsync(ReturnedSubmissionInformationModelData returnedSubmissionInformationModelData)
        {
            returnedSubmissionInformationModelData.ReturnedSubmission_WhenNeededBy =
                await keyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(returnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestId);
        }
    }

    async Task<IServiceOperationDataResult<CompletedSubmissionInformation>> ISupplierDataShareRequestService.GetCompletedSubmissionInformationAsync(Guid dataShareRequestId)
    {
        try
        {
            var completedSubmissionInformationModelData = await supplierDataShareRequestRepository.GetCompletedSubmissionInformationAsync(
                dataShareRequestId);

            await PopulateAuditInformationAsync(completedSubmissionInformationModelData);

            await PopulateAcquirerInformationAsync(completedSubmissionInformationModelData);

            await PopulateKeyQuestionInformationAsync(completedSubmissionInformationModelData);

            var completedSubmissionInformation = supplierDataShareRequestModelDataFactory.CreateCompletedSubmissionInformation(
                completedSubmissionInformationModelData);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(completedSubmissionInformation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to GetCompletedSubmissionInformationAsync");

            var response = serviceOperationResultFactory.CreateFailedDataResult<CompletedSubmissionInformation>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task PopulateAuditInformationAsync(CompletedSubmissionInformationModelData completedSubmissionInformationModelData)
        {
            var submittedAuditLogEntry = await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                dataShareRequestId, DataShareRequestStatusType.Submitted);

            if (submittedAuditLogEntry == null)
                throw new InconsistentDataException("Completed submission has no Submitted entry in the audit log");

            completedSubmissionInformationModelData.CompletedSubmission_SubmittedOn = submittedAuditLogEntry.AuditLogDataShareRequestStatusChange_ChangedAtUtc;

            var completedAuditLogEntry = await GetCompletedSubmissionTimestampInformationAsync();

            if (completedAuditLogEntry == null)
                throw new InconsistentDataException("Completed submission has no Completed entry in the audit log");

            completedSubmissionInformationModelData.CompletedSubmission_CompletedOn = completedAuditLogEntry.AuditLogDataShareRequestStatusChange_ChangedAtUtc;
            completedSubmissionInformationModelData.CompletedSubmission_FeedbackProvided = DoBuildSubmissionCommentsFromAuditLogEntry(completedAuditLogEntry);

            async Task<AuditLogDataShareRequestStatusChangeModelData?> GetCompletedSubmissionTimestampInformationAsync()
            {
                return await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(dataShareRequestId, DataShareRequestStatusType.Accepted)
                       ?? await auditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(dataShareRequestId, DataShareRequestStatusType.Rejected);
            }
        }

        async Task PopulateAcquirerInformationAsync(CompletedSubmissionInformationModelData completedSubmissionInformationModelData)
        {
            var acquirerOrganisation = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                completedSubmissionInformationModelData.CompletedSubmission_AcquirerOrganisationId);

            completedSubmissionInformationModelData.CompletedSubmission_AcquirerOrganisationName = acquirerOrganisation.OrganisationName;

            var acquirerUserDetails = await userProfilePresenter.GetUserDetailsByUserIdAsync(
                completedSubmissionInformationModelData.CompletedSubmission_AcquirerUserId);

            completedSubmissionInformationModelData.CompletedSubmission_AcquirerUserEmailAddress = acquirerUserDetails.UserContactDetails.EmailAddress;
        }

        async Task PopulateKeyQuestionInformationAsync(CompletedSubmissionInformationModelData returnedSubmissionInformationModelData)
        {
            returnedSubmissionInformationModelData.CompletedSubmission_WhenNeededBy =
                await keyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(returnedSubmissionInformationModelData.CompletedSubmission_DataShareRequestId);
        }
    }

    async Task<IServiceOperationResult> ISupplierDataShareRequestService.SetSubmissionNotesAsync(
        Guid dataShareRequestId,
        string notes)
    {
        try
        {
            var dataShareRequestStatus = await supplierDataShareRequestRepository.GetDataShareRequestStatusAsync(dataShareRequestId);

            if (dataShareRequestStatus != DataShareRequestStatusType.Cancelled)
            {
                // If it's been cancelled then we could do lots of interesting stuff here, but the simplest is to just not allow this
                // request through, and return the status in the returned information

                await supplierDataShareRequestRepository.SetSubmissionNotesAsync(
                    dataShareRequestId,
                    notes);
            }

            return serviceOperationResultFactory.CreateSuccessfulResult();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to SetSubmissionNotesAsync");

            var response = serviceOperationResultFactory.CreateFailedResult(ex.Message);

            return await Task.FromResult(response);
        }
    }

    async Task<IServiceOperationDataResult<DataShareRequestAcceptanceResult>> ISupplierDataShareRequestService.AcceptSubmissionAsync(
        Guid dataShareRequestId,
        string decisionFeedback)
    {
        try
        {
            var dataShareRequestStatus = await supplierDataShareRequestRepository.GetDataShareRequestStatusAsync(dataShareRequestId);

            bool? notificationsSentSuccess = null;

            if (dataShareRequestStatus != DataShareRequestStatusType.Cancelled)
            {
                // If it's been cancelled then we could do lots of interesting stuff here, but the simplest is to just not allow this
                // request through, and return the status in the returned information

                var initiatingUserDetailsSet = await userProfilePresenter.GetInitiatingUserDetailsAsync();

                await DoAcceptSubmissionAsync(initiatingUserDetailsSet.UserIdSet);

                notificationsSentSuccess = await DoSendNotificationsAsync();
            }

            var acceptedDecisionSummaryModelData = await DoGetAcceptedDecisionSummaryModelDataAsync();

            await PopulateAcquirerInformationAsync(acceptedDecisionSummaryModelData);

            var acceptedDecisionSummary = supplierDataShareRequestModelDataFactory.CreateDataShareRequestAcceptanceResult(
                acceptedDecisionSummaryModelData,
                notificationsSentSuccess);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(acceptedDecisionSummary);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to AcceptSubmissionAsync");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestAcceptanceResult>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task DoAcceptSubmissionAsync(IUserIdSet supplierUserIdSet)
        {
            await supplierDataShareRequestRepository.AcceptSubmissionAsync(supplierUserIdSet, dataShareRequestId, decisionFeedback);
        }

        async Task<AcceptedDecisionSummaryModelData> DoGetAcceptedDecisionSummaryModelDataAsync()
        {
            return await supplierDataShareRequestRepository.GetAcceptedDecisionSummaryAsync(
                    dataShareRequestId);
        }

        async Task PopulateAcquirerInformationAsync(AcceptedDecisionSummaryModelData acceptedDecisionSummaryModelData)
        {
            var acquirerOrganisationDetails = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationId);

            acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationName = acquirerOrganisationDetails.OrganisationName;

            var acquirerUserDetails = await userProfilePresenter.GetUserDetailsByUserIdAsync(
                acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId);

            acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserEmailAddress = acquirerUserDetails.UserContactDetails.EmailAddress;
        }

        async Task<bool> DoSendNotificationsAsync()
        {
            try
            {
                var dataShareRequestNotificationInformation = await supplierDataShareRequestRepository.GetDataShareRequestNotificationInformationAsync(
                    dataShareRequestId);

                var acquirerUserDetails = await userProfilePresenter.GetUserDetailsByUserIdAsync(
                    dataShareRequestNotificationInformation.AcquirerUserId);

                if (acquirerUserDetails.UserContactDetails.EmailNotification)
                {
                    var esdaDetails = await esdaInformationPresenter.GetEsdaDetailsByIdAsync(dataShareRequestNotificationInformation.EsdaId);

                    var result = await notificationService.SendToAcquirerDataShareRequestAcceptedNotificationAsync(
                        acquirerUserDetails.UserContactDetails.EmailAddress,
                        esdaDetails.ContactPointEmailAddress ?? "",
                        esdaDetails.ContactPointName ?? "",
                        acquirerUserDetails.UserContactDetails.UserName,
                        dataShareRequestNotificationInformation.EsdaName,
                        dataShareRequestNotificationInformation.DataShareRequestRequestId);

                    return result.Success;
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send notification of Data Share Request acceptance");

                return false;
            }
        }
    }

    async Task<IServiceOperationDataResult<DataShareRequestRejectionResult>> ISupplierDataShareRequestService.RejectSubmissionAsync(
        Guid dataShareRequestId,
        string decisionFeedback)
    {
        try
        {
            var dataShareRequestStatus = await supplierDataShareRequestRepository.GetDataShareRequestStatusAsync(dataShareRequestId);

            bool? notificationsSentSuccess = null;
            
            if (dataShareRequestStatus != DataShareRequestStatusType.Cancelled)
            {
                // If it's been cancelled then we could do lots of interesting stuff here, but the simplest is to just not allow this
                // request through, and return the status in the returned information

                var initiatingUserDetailsSet = await userProfilePresenter.GetInitiatingUserDetailsAsync();

                await DoRejectSubmissionAsync(initiatingUserDetailsSet.UserIdSet);

                notificationsSentSuccess = await DoSendNotificationsAsync();
            }
            
            var rejectedDecisionSummaryModelData = await DoGetRejectedDecisionSummaryModelDataAsync();

            await PopulateAcquirerInformationAsync(rejectedDecisionSummaryModelData);

            var rejectedDecisionSummary = supplierDataShareRequestModelDataFactory.CreateDataShareRequestRejectionResult(
                rejectedDecisionSummaryModelData,
                notificationsSentSuccess);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(rejectedDecisionSummary);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to RejectSubmissionAsync");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestRejectionResult>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task DoRejectSubmissionAsync(IUserIdSet supplierUserIdSet)
        {
            await supplierDataShareRequestRepository.RejectSubmissionAsync(supplierUserIdSet, dataShareRequestId, decisionFeedback);
        }

        async Task<RejectedDecisionSummaryModelData> DoGetRejectedDecisionSummaryModelDataAsync()
        {
            return await supplierDataShareRequestRepository.GetRejectedDecisionSummaryAsync(
                dataShareRequestId);
        }

        async Task PopulateAcquirerInformationAsync(RejectedDecisionSummaryModelData rejectedDecisionSummaryModelData)
        {
            var acquirerOrganisationDetails = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                rejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationId);

            rejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationName = acquirerOrganisationDetails.OrganisationName;
        }

        async Task<bool> DoSendNotificationsAsync()
        {
            try
            {
                var dataShareRequestNotificationInformation = await supplierDataShareRequestRepository.GetDataShareRequestNotificationInformationAsync(
                    dataShareRequestId);

                var acquirerUserDetails = await userProfilePresenter.GetUserDetailsByUserIdAsync(
                    dataShareRequestNotificationInformation.AcquirerUserId);

                if(acquirerUserDetails.UserContactDetails.EmailNotification)
                {
                    var esdaDetails = await esdaInformationPresenter.GetEsdaDetailsByIdAsync(dataShareRequestNotificationInformation.EsdaId);

                    var result = await notificationService.SendToAcquirerDataShareRequestRejectedNotificationAsync(
                        acquirerUserDetails.UserContactDetails.EmailAddress,
                        esdaDetails.ContactPointEmailAddress ?? "",
                        acquirerUserDetails.UserContactDetails.UserName,
                        dataShareRequestNotificationInformation.EsdaName,
                        dataShareRequestNotificationInformation.DataShareRequestRequestId,
                        decisionFeedback);

                    return result.Success;
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send notification of Data Share Request rejection");

                return false;
            }
        }
    }

    async Task<IServiceOperationDataResult<DataShareRequestReturnResult>> ISupplierDataShareRequestService.ReturnSubmissionAsync(
        Guid dataShareRequestId,
        string commentsToAcquirer)
    {
        try
        {
            var dataShareRequestStatus = await supplierDataShareRequestRepository.GetDataShareRequestStatusAsync(dataShareRequestId);

            bool? notificationsSentSuccess = null;

            if (dataShareRequestStatus != DataShareRequestStatusType.Cancelled)
            {
                // If it's been cancelled then we could do lots of interesting stuff here, but the simplest is to just not allow this
                // request through, and return the status in the returned information

                var initiatingUserDetails = await userProfilePresenter.GetInitiatingUserDetailsAsync();

                await DoReturnSubmissionAsync(initiatingUserDetails.UserIdSet);

                notificationsSentSuccess = await DoSendNotificationsAsync();
            }

            var returnedDecisionSummaryModelData = await DoGetReturnedDecisionSummaryModelDataAsync();

            await PopulateAcquirerInformationAsync(returnedDecisionSummaryModelData);

            var returnedDecisionSummary = supplierDataShareRequestModelDataFactory.CreateDataShareRequestReturnResult(
                returnedDecisionSummaryModelData,
                notificationsSentSuccess);

            return serviceOperationResultFactory.CreateSuccessfulDataResult(returnedDecisionSummary);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to ReturnSubmissionAsync");

            var response = serviceOperationResultFactory.CreateFailedDataResult<DataShareRequestReturnResult>(ex.Message);

            return await Task.FromResult(response);
        }

        async Task DoReturnSubmissionAsync(IUserIdSet supplierUserIdSet)
        {
            await supplierDataShareRequestRepository.ReturnSubmissionAsync(supplierUserIdSet, dataShareRequestId, commentsToAcquirer);
        }

        async Task<ReturnedDecisionSummaryModelData> DoGetReturnedDecisionSummaryModelDataAsync()
        {
            return await supplierDataShareRequestRepository.GetReturnedDecisionSummaryAsync(
                dataShareRequestId);
        }

        async Task PopulateAcquirerInformationAsync(ReturnedDecisionSummaryModelData returnedDecisionSummaryModelData)
        {
            var acquirerOrganisationDetails = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                returnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationId);

            returnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationName = acquirerOrganisationDetails.OrganisationName;
        }

        async Task<bool> DoSendNotificationsAsync()
        {
            try
            {
                var dataShareRequestNotificationInformation = await supplierDataShareRequestRepository.GetDataShareRequestNotificationInformationAsync(
                    dataShareRequestId);

                var acquirerUserDetails = await userProfilePresenter.GetUserDetailsByUserIdAsync(
                    dataShareRequestNotificationInformation.AcquirerUserId);

                if (acquirerUserDetails.UserContactDetails.EmailNotification) 
                {
                    var esdaDetails = await esdaInformationPresenter.GetEsdaDetailsByIdAsync(dataShareRequestNotificationInformation.EsdaId);

                    var result = await notificationService.SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
                        acquirerUserDetails.UserContactDetails.EmailAddress,
                        esdaDetails.ContactPointEmailAddress ?? "",
                        acquirerUserDetails.UserContactDetails.UserName,
                        dataShareRequestNotificationInformation.EsdaName,
                        dataShareRequestNotificationInformation.DataShareRequestRequestId);

                    return result.Success;
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send notification of Data Share Request returned");

                return false;
            }
        }
    }

    private static string DoBuildSubmissionCommentsFromAuditLogEntry(
        AuditLogDataShareRequestStatusChangeModelData auditLogDataShareRequestStatusChangeModelData)
    {
        var auditLogComments = auditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_Comments
            .OrderBy(x => x.AuditLogDataShareRequestStatusChangeComment_CommentOrder)
            .Select(x => x.AuditLogDataShareRequestStatusChangeComment_Comment);

        return string.Join(Environment.NewLine, auditLogComments);
    }

    private async Task<SubmissionReviewInformationModelData> DoGetSubmissionReviewInformationModelDataAsync(
        Guid dataShareRequestId)
    {
        try
        {
            var submissionReviewInformation = await supplierDataShareRequestRepository.GetSubmissionReviewInformationModelDataAsync(dataShareRequestId);

            questionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionReviewInformationModelData(
                submissionReviewInformation,
                submissionReviewInformation.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_EsdaName);

            await DoPopulateAcquirerInformationAsync(submissionReviewInformation);

            await DoPopulateSubmissionDetailsReturnCommentsAsync(submissionReviewInformation.SubmissionReviewInformation_SubmissionDetails);

            return submissionReviewInformation;
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetSubmissionReviewInformationModelData from repository";

            logger.LogError(ex, errorMessage);

            throw new DataShareRequestGeneralException(errorMessage, ex);
        }

        async Task DoPopulateAcquirerInformationAsync(SubmissionReviewInformationModelData submissionReviewInformation)
        {
            var acquirerOrganisation = await userProfilePresenter.GetOrganisationDetailsByOrganisationIdAsync(
                submissionReviewInformation.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_AcquirerOrganisationId);
            
            submissionReviewInformation.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_AcquirerOrganisationName = acquirerOrganisation.OrganisationName;
        }
    }

    private async Task DoPopulateSubmissionDetailsReturnCommentsAsync(SubmissionDetailsModelData submissionDetailsModelData)
    {
        var auditLogsForSubmissionReturns = (await auditLogService.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                submissionDetailsModelData.SubmissionDetails_DataShareRequestId,
                toStatuses: [DataShareRequestStatusType.Returned]))
            .Where(x => x is not null)
            .OrderByDescending(x => x!.AuditLogDataShareRequestStatusChange_ChangedAtUtc);

        submissionDetailsModelData.SubmissionDetails_SubmissionReturnComments = auditLogsForSubmissionReturns
            .Select(auditLogDataShareRequestStatusChangeModelData => new SubmissionReturnCommentsModelData
            {
                ReturnedOnUtc = auditLogDataShareRequestStatusChangeModelData!.AuditLogDataShareRequestStatusChange_ChangedAtUtc,
                Comments = DoBuildSubmissionCommentsFromAuditLogEntry(auditLogDataShareRequestStatusChangeModelData)
            }).ToList();
    }
}