using Agrimetrics.DataShare.Api.Core.SystemProxies;
using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;
using Microsoft.Extensions.Logging;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs.ParameterModels;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using System.Data;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.SupplierDataShareRequests;

internal class SupplierDataShareRequestRepository(
    ILogger<SupplierDataShareRequestRepository> logger,
    IDatabaseChannelCreation databaseChannelCreation,
    IDatabaseCommandRunner databaseCommandRunner,
    ISupplierDataShareRequestSqlQueries supplierDataShareRequestSqlQueries,
    IAuditLogRepository auditLogRepository,
    IClock clock) : ISupplierDataShareRequestRepository
{
    private readonly List<DataShareRequestStatusType> pendingSubmissionStatuses =
    [
        DataShareRequestStatusType.Submitted,
        DataShareRequestStatusType.InReview,
        DataShareRequestStatusType.Returned
    ];

    private readonly List<DataShareRequestStatusType> completedSubmissionStatuses =
    [
        DataShareRequestStatusType.Accepted,
        DataShareRequestStatusType.Rejected
    ];

    #region GetPendingSubmissionSummariesAsync()
    async Task<IEnumerable<PendingSubmissionSummaryModelData>> ISupplierDataShareRequestRepository.GetPendingSubmissionSummariesAsync(
        int supplierOrganisationId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await databaseCommandRunner.DbQueryAsync<PendingSubmissionSummaryModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                supplierDataShareRequestSqlQueries.GetPendingSubmissionSummaries,
                new
                {
                    PendingSubmissionStatuses = pendingSubmissionStatuses.Select(x => x.ToString()).ToList(),
                    SupplierOrganisationId = supplierOrganisationId
                }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetPendingSubmissionSummaries";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region GetCompletedSubmissionSummariesAsync()
    async Task<IEnumerable<CompletedSubmissionSummaryModelData>> ISupplierDataShareRequestRepository.GetCompletedSubmissionSummariesAsync(
        int supplierOrganisationId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var completedSubmissionSummaryModelDatas = (await databaseCommandRunner.DbQueryAsync<CompletedSubmissionSummaryModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                supplierDataShareRequestSqlQueries.GetCompletedSubmissionSummaries,
                new
                {
                    CompletedSubmissionStatuses = completedSubmissionStatuses.Select(x => x.ToString()).ToList(),
                    SupplierOrganisationId = supplierOrganisationId
                }).ConfigureAwait(false)).ToList();

            foreach (var completedSubmissionSummaryModelData in completedSubmissionSummaryModelDatas)
            {
                completedSubmissionSummaryModelData.CompletedSubmissionSummary_Decision =
                    completedSubmissionSummaryModelData.CompletedSubmissionSummary_Status == DataShareRequestStatusType.Accepted
                        ? SubmissionDecisionType.Accepted
                        : SubmissionDecisionType.Rejected;
            }

            return completedSubmissionSummaryModelDatas;
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetCompletedSubmissionSummaries";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region GetSubmissionInformationModelDataAsync()
    async Task<SubmissionInformationModelData> ISupplierDataShareRequestRepository.GetSubmissionInformationModelDataAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await databaseCommandRunner.DbQuerySingleAsync<SubmissionInformationModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                supplierDataShareRequestSqlQueries.GetSubmissionInformationModelData,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetSubmissionDetailsModelDataAsync";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region GetSubmissionDetailsModelDataAsync()
    async Task<SubmissionDetailsModelData> ISupplierDataShareRequestRepository.GetSubmissionDetailsModelDataAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await DoGetSubmissionDetailModelDatasAsync(databaseChannel.Connection, databaseChannel.Transaction!, dataShareRequestId);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetSubmissionDetailsModelDataAsync";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    private async Task<SubmissionDetailsModelData> DoGetSubmissionDetailModelDatasAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid dataShareRequestId)
    {
        var submissionDetailModelData = await DoGetSubmissionDetailModelDataAsync();

        // Unfortunately Dapper doesn't go to a sufficient number of levels, so we have to split the call at first getting everything up to and including
        // the answer parts, and then a separate call per answer part
        await DoGetSubmissionDetailAnswerPartsModelDataAsync();

        return submissionDetailModelData;

        async Task<SubmissionDetailsModelData> DoGetSubmissionDetailModelDataAsync()
        {
            var submissionDetailModelDatasFlattened = (await databaseCommandRunner.DbQueryAsync<
                SubmissionDetailsModelData,
                SubmissionDetailsSectionModelData,
                SubmissionDetailsMainQuestionModelData,
                SubmissionDetailsAnswerPartModelData,
                SubmissionDetailsBackingQuestionModelData?,
                SubmissionDetailsAnswerPartModelData?,
                SubmissionDetailsModelData>(
                dbConnection,
                dbTransaction,
                supplierDataShareRequestSqlQueries.GetSubmissionDetailsModelDatas,
                (submission,
                    section,
                    mainQuestion, mainQuestionAnswerPart,
                    backingQuestion, backingQuestionAnswerPart) =>
                {
                    if (backingQuestion != null)
                    {
                        if (backingQuestionAnswerPart != null)
                        {
                            backingQuestion.SubmissionDetailsBackingQuestion_AnswerParts.Add(backingQuestionAnswerPart);
                        }
                        mainQuestion.SubmissionDetailsMainQuestion_BackingQuestions.Add(backingQuestion);
                    }
                    
                    mainQuestion.SubmissionDetailsMainQuestion_AnswerParts.Add(mainQuestionAnswerPart);
                    section.SubmissionDetailsSection_Questions.Add(mainQuestion);

                    submission.SubmissionDetails_Sections.Add(section);
                    
                    return submission;
                },
                $"{nameof(SubmissionDetailsSectionModelData.SubmissionDetailsSection_SectionId)}, " +
                $"{nameof(SubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_Id)}, " +
                $"{nameof(SubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_Id)}, " +
                $"{nameof(SubmissionDetailsBackingQuestionModelData.SubmissionDetailsBackingQuestion_Id)}, " +
                $"{nameof(SubmissionDetailsAnswerPartModelData.SubmissionDetailsAnswerPart_Id)}",
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false)).ToList();

            return BuildGroupedData();

            SubmissionDetailsModelData BuildGroupedData()
            {
                var firstEntryInGroup = submissionDetailModelDatasFlattened[0];

                var allSectionsFlattened = submissionDetailModelDatasFlattened.SelectMany(x => x.SubmissionDetails_Sections).ToList();

                var sectionsGroupedBySectionId = allSectionsFlattened.GroupBy(x => x.SubmissionDetailsSection_SectionId);

                var sections = BuildSections(sectionsGroupedBySectionId).ToList();

                firstEntryInGroup.SubmissionDetails_Sections = sections;

                return firstEntryInGroup;
            }
        }

        async Task DoGetSubmissionDetailAnswerPartsModelDataAsync()
        {
            var allAnswerParts = ExtractAllAnswerParts();

            foreach (var answerPart in allAnswerParts)
            {
                await DoPopulateAnswerPart(dbConnection, dbTransaction, answerPart);
            }

            IEnumerable<SubmissionDetailsAnswerPartModelData> ExtractAllAnswerParts()
            {
                var allQuestions = submissionDetailModelData.SubmissionDetails_Sections.SelectMany(x => x.SubmissionDetailsSection_Questions).ToList();

                var allMainQuestionAnswerParts = allQuestions.SelectMany(x =>
                    x.SubmissionDetailsMainQuestion_AnswerParts);

                var allBackingQuestionAnswerParts = allQuestions.SelectMany(x =>
                    x.SubmissionDetailsMainQuestion_BackingQuestions.SelectMany(y => y.SubmissionDetailsBackingQuestion_AnswerParts));

                return allMainQuestionAnswerParts.Concat(allBackingQuestionAnswerParts);
            }
        }
    }

    private static IEnumerable<SubmissionDetailsSectionModelData> BuildSections(IEnumerable<IGrouping<Guid, SubmissionDetailsSectionModelData>> sectionGroups)
    {
        foreach (var sectionGroup in sectionGroups)
        {
            var allMainQuestionsFlattened = sectionGroup.SelectMany(x => x.SubmissionDetailsSection_Questions).ToList();

            var mainQuestionsGroupedById = allMainQuestionsFlattened.GroupBy(x => x.SubmissionDetailsMainQuestion_Id);

            var mainQuestions = BuildMainQuestions(mainQuestionsGroupedById).ToList();

            var firstRecordInGroup = sectionGroup.First();

            firstRecordInGroup.SubmissionDetailsSection_Questions = mainQuestions;

            yield return firstRecordInGroup;
        }
    }

    private static IEnumerable<SubmissionDetailsMainQuestionModelData> BuildMainQuestions(IEnumerable<IGrouping<Guid, SubmissionDetailsMainQuestionModelData>> mainQuestionGroups)
    {
        foreach (var mainQuestionGroup in mainQuestionGroups)
        {
            var allAnswerPartsFlattened = mainQuestionGroup.SelectMany(x => x.SubmissionDetailsMainQuestion_AnswerParts).ToList();

            var answerPartsGroupedById = allAnswerPartsFlattened.GroupBy(x => x.SubmissionDetailsAnswerPart_Id);

            var answerParts = BuildAnswerParts(answerPartsGroupedById).ToList();

            var firstRecordInGroup = mainQuestionGroup.First();

            firstRecordInGroup.SubmissionDetailsMainQuestion_AnswerParts = answerParts;

            yield return firstRecordInGroup;
        }
    }

    private static IEnumerable<SubmissionDetailsAnswerPartModelData> BuildAnswerParts(IEnumerable<IGrouping<Guid, SubmissionDetailsAnswerPartModelData>> answerPartGroups)
    {
        foreach (var answerPartGroup in answerPartGroups)
        {
            var allAnswerPartResponsesFlattened = answerPartGroup.SelectMany(x => x.SubmissionDetailsAnswerPart_Responses).ToList();

            var answerPartResponsesGroupedById = allAnswerPartResponsesFlattened.GroupBy(x => x.SubmissionDetailsAnswerPartResponse_Id);

            var answerPartResponses = BuildAnswerPartResponses(answerPartResponsesGroupedById).ToList();

            var firstRecordInGroup = answerPartGroup.First();

            firstRecordInGroup.SubmissionDetailsAnswerPart_Responses = answerPartResponses;

            yield return firstRecordInGroup;
        }
    }
    
    private async Task DoPopulateAnswerPart(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        SubmissionDetailsAnswerPartModelData answerPartModelData)
    {
        var answerPartResponseModelDatasFlattened = (await databaseCommandRunner.DbQueryAsync<
            SubmissionDetailsAnswerPartResponseModelData?,
            SubmissionDetailsAnswerPartResponseItemModelData?,
            SubmissionDetailsAnswerPartResponseItemFreeFormModelData?,
            SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData?,
            SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData?,
            SubmissionDetailsAnswerPartResponseModelData?>(
                dbConnection,
                dbTransaction,
                supplierDataShareRequestSqlQueries.GetSubmissionDetailsAnswerPartResponseModelDatas,
                (response, responseItem, responseItemFreeForm, responseItemSelectionOption, selectedOption) =>
                {
                    if (response == null || responseItem == null) return response;

                    if (responseItemFreeForm != null)
                    {
                        responseItem.SubmissionDetailsAnswerResponseItem_FreeFormData = responseItemFreeForm;
                    }

                    if (responseItemSelectionOption != null)
                    {
                        if (selectedOption != null)
                        {
                            responseItemSelectionOption
                                .SubmissionDetailsAnswerPartResponseItemSelectionOption_SelectedOptions
                                .Add(selectedOption);
                        }

                        responseItem.SubmissionDetailsAnswerResponseItem_SelectionOptionData =
                            responseItemSelectionOption;
                    }

                    response.SubmissionDetailsAnswerPartResponse_ResponseItems.Add(responseItem);

                    return response;
                },
                $"{nameof(SubmissionDetailsAnswerPartResponseItemModelData.SubmissionDetailsAnswerResponseItem_Id)}, " +
                $"{nameof(SubmissionDetailsAnswerPartResponseItemFreeFormModelData.SubmissionDetailsAnswerPartResponseItemFreeForm_Id)}, " +
                $"{nameof(SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOption_Id)}, " +
                $"{nameof(SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_Id)}",
                new
                {
                    AnswerPartId = answerPartModelData.SubmissionDetailsAnswerPart_Id
                }).ConfigureAwait(false)).ToList();

        answerPartModelData.SubmissionDetailsAnswerPart_Responses = BuildGroupedData().ToList();

        IEnumerable<SubmissionDetailsAnswerPartResponseModelData> BuildGroupedData()
        {
            var nonNullResponses = answerPartResponseModelDatasFlattened
                .OfType<SubmissionDetailsAnswerPartResponseModelData>().ToList();

            var responseItemsGroupedById = nonNullResponses.GroupBy(x => x.SubmissionDetailsAnswerPartResponse_Id).ToList();

            return BuildAnswerPartResponses(responseItemsGroupedById);
        }
    }

    private static IEnumerable<SubmissionDetailsAnswerPartResponseModelData> BuildAnswerPartResponses(IEnumerable<IGrouping<Guid, SubmissionDetailsAnswerPartResponseModelData>> answerPartResponseGroups)
    {
        foreach (var answerPartResponseGroup in answerPartResponseGroups)
        {
            var allAnswerPartResponseItemsFlattened = answerPartResponseGroup.SelectMany(x => x.SubmissionDetailsAnswerPartResponse_ResponseItems).ToList();

            var answerPartResponseItemsGroupedById = allAnswerPartResponseItemsFlattened.GroupBy(x => x.SubmissionDetailsAnswerResponseItem_Id);

            var answerPartResponseItems = BuildAnswerPartResponseItems(answerPartResponseItemsGroupedById).ToList();

            var firstRecordInGroup = answerPartResponseGroup.First();

            firstRecordInGroup.SubmissionDetailsAnswerPartResponse_ResponseItems = answerPartResponseItems;

            yield return firstRecordInGroup;
        }
    }

    private static IEnumerable<SubmissionDetailsAnswerPartResponseItemModelData> BuildAnswerPartResponseItems(IEnumerable<IGrouping<Guid, SubmissionDetailsAnswerPartResponseItemModelData>> answerPartResponseItemGroups)
    {
        foreach (var answerPartResponseItemGroup in answerPartResponseItemGroups)
        {
            var firstRecordInGroup = answerPartResponseItemGroup.First();

            if (firstRecordInGroup.SubmissionDetailsAnswerResponseItem_SelectionOptionData != null)
            {
                var allSelectionOptionDatas = answerPartResponseItemGroup
                    .Select(x => x.SubmissionDetailsAnswerResponseItem_SelectionOptionData)
                    .Where(x => x != null)
                    .Cast<SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData>();

                var allSelectedOptionsInGroup = allSelectionOptionDatas
                    .SelectMany(x => x.SubmissionDetailsAnswerPartResponseItemSelectionOption_SelectedOptions).ToList();

                firstRecordInGroup.SubmissionDetailsAnswerResponseItem_SelectionOptionData.SubmissionDetailsAnswerPartResponseItemSelectionOption_SelectedOptions = allSelectedOptionsInGroup;
            }

            yield return firstRecordInGroup;
        }
    }
    #endregion

    #region GetSubmissionReviewInformationModelDataAsync()
    async Task<SubmissionReviewInformationModelData> ISupplierDataShareRequestRepository.GetSubmissionReviewInformationModelDataAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!;

            var submissionDetailsModelData = await DoGetSubmissionDetailModelDatasAsync(dbConnection, dbTransaction, dataShareRequestId);

            var supplierNotes = await DoGetSupplierNotesAsync(dbConnection, dbTransaction);

            return new SubmissionReviewInformationModelData
            {
                SubmissionReviewInformation_SubmissionDetails = submissionDetailsModelData,
                SubmissionReviewInformation_SupplierNotes = supplierNotes
            };
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetSubmissionDetailsModelDataAsync";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }

        async Task<string> DoGetSupplierNotesAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            return await databaseCommandRunner.DbQuerySingleAsync<string>(
                dbConnection,
                dbTransaction,
                supplierDataShareRequestSqlQueries.GetSubmissionNotes,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false);
        }
    }
    #endregion
    
    #region GetReturnedSubmissionInformationAsync
    async Task<ReturnedSubmissionInformationModelData> ISupplierDataShareRequestRepository.GetReturnedSubmissionInformationAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!;

            var dataShareRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            if (dataShareRequestStatus != DataShareRequestStatusType.Returned)
                throw new InconsistentDataException("Unable to get returned submission information for DataShareRequest that does not have Returned status");

            return await databaseCommandRunner.DbQuerySingleAsync<ReturnedSubmissionInformationModelData>(
                dbConnection,
                dbTransaction,
                supplierDataShareRequestSqlQueries.GetReturnedSubmissionInformation,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetReturnedSubmissionInformation";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region GetCompletedSubmissionInformationAsync()
    async Task<CompletedSubmissionInformationModelData> ISupplierDataShareRequestRepository.GetCompletedSubmissionInformationAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var completedStatuses = new List<DataShareRequestStatusType>
            {
                DataShareRequestStatusType.Accepted, DataShareRequestStatusType.Rejected
            };

            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!; 

            var dataShareRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            if (!completedStatuses.Contains(dataShareRequestStatus))
                throw new InconsistentDataException("Unable to get completed submission information for DataShareRequest that does not have a completed status");

            var completedSubmissionInformationModelData = await databaseCommandRunner.DbQuerySingleAsync<CompletedSubmissionInformationModelData>(
                dbConnection,
                dbTransaction,
                supplierDataShareRequestSqlQueries.GetCompletedSubmissionInformation,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false);

            completedSubmissionInformationModelData.CompletedSubmission_Decision =
                completedSubmissionInformationModelData.CompletedSubmission_DataShareRequestStatus == DataShareRequestStatusType.Accepted
                    ? SubmissionDecisionType.Accepted
                    : SubmissionDecisionType.Rejected;

            return completedSubmissionInformationModelData;
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetCompletedSubmissionInformation";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region SetSubmissionNotesAsync()
    async Task ISupplierDataShareRequestRepository.SetSubmissionNotesAsync(
        Guid dataShareRequestId,
        string notes)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            await databaseCommandRunner.DbExecuteAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                supplierDataShareRequestSqlQueries.SetSubmissionNotes,
                new
                {
                    DataShareRequestId = dataShareRequestId,
                    Notes = notes
                }).ConfigureAwait(false);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to SetSubmissionNotesAsync";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region StartSubmissionReviewAsync()
    async Task ISupplierDataShareRequestRepository.StartSubmissionReviewAsync(
        IUserIdSet supplierUserIdSet,
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!;

            var previousDataShareRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            await databaseCommandRunner.DbExecuteAsync(
                dbConnection,
                dbTransaction,
                supplierDataShareRequestSqlQueries.UpdateDataShareRequestStatus,
                new
                {
                    DataShareRequestId = dataShareRequestId,
                    DataShareRequestStatus = DataShareRequestStatusType.InReview.ToString()
                }).ConfigureAwait(false);

            await DoRecordDataShareRequestStatusChangeInAuditLogAsync(dbConnection, dbTransaction,
                dataShareRequestId, supplierUserIdSet, previousDataShareRequestStatus);

            await databaseChannel.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to StartSubmissionReview";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    private async Task DoRecordDataShareRequestStatusChangeInAuditLogAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid dataShareRequestId,
        IUserIdSet changedByUser,
        DataShareRequestStatusType previousStatus)
    {
        try
        {
            var dataShareRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            var recordDataShareRequestStatusChangeParameters = new RecordDataShareRequestStatusChangeParameters
            {
                DbConnection = dbConnection,
                DbTransaction = dbTransaction,
                DataShareRequestId = dataShareRequestId,
                FromStatus = previousStatus,
                ToStatus = dataShareRequestStatus,
                ChangedByUser = changedByUser,
                ChangedAtLocalTime = clock.LocalNow
            };

            await auditLogRepository.RecordDataShareRequestStatusChangeAsync(recordDataShareRequestStatusChangeParameters);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to record DataShareRequest Status Change in audit log";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
    }
    #endregion

    #region AcceptSubmissionAsync()
    async Task ISupplierDataShareRequestRepository.AcceptSubmissionAsync(
        IUserIdSet supplierUserIdSet,
        Guid dataShareRequestId,
        string decisionFeedback)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!;

            var previousDataShareRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            await DoUpdateDataShareRequestStatus(dbConnection, dbTransaction);

            await DoRecordDataShareRequestStatusChangeWithCommentsInAuditLogAsync(dbConnection, dbTransaction,
                dataShareRequestId, supplierUserIdSet, previousDataShareRequestStatus, [decisionFeedback]);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to AcceptSubmission";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }

        async Task DoUpdateDataShareRequestStatus(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            var existingRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            if (existingRequestStatus != DataShareRequestStatusType.InReview)
                throw new InconsistentDataException("Unable to Accept Submission that is not in review");

            await databaseCommandRunner.DbExecuteAsync(
                dbConnection,
                dbTransaction,
                supplierDataShareRequestSqlQueries.UpdateDataShareRequestStatus,
                new
                {
                    DataShareRequestId = dataShareRequestId,
                    DataShareRequestStatus = DataShareRequestStatusType.Accepted.ToString()
                }).ConfigureAwait(false);
        }
    }
    #endregion

    #region RejectSubmissionAsync()
    async Task ISupplierDataShareRequestRepository.RejectSubmissionAsync(
        IUserIdSet supplierUserIdSet,
        Guid dataShareRequestId,
        string decisionFeedback)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!;

            var previousDataShareRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            await DoUpdateDataShareRequestStatus(dbConnection, dbTransaction);

            await DoRecordDataShareRequestStatusChangeWithCommentsInAuditLogAsync(dbConnection, dbTransaction,
                dataShareRequestId, supplierUserIdSet, previousDataShareRequestStatus, [decisionFeedback]);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to RejectSubmission";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }

        async Task DoUpdateDataShareRequestStatus(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            var existingRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            if (existingRequestStatus != DataShareRequestStatusType.InReview)
                throw new InconsistentDataException("Unable to Reject Submission that is not in review");

            await databaseCommandRunner.DbExecuteAsync(
                dbConnection,
                dbTransaction,
                supplierDataShareRequestSqlQueries.UpdateDataShareRequestStatus,
                new
                {
                    DataShareRequestId = dataShareRequestId,
                    DataShareRequestStatus = DataShareRequestStatusType.Rejected.ToString()
                }).ConfigureAwait(false);
        }
    }
    #endregion

    #region ReturnSubmissionAsync()
    async Task ISupplierDataShareRequestRepository.ReturnSubmissionAsync(
        IUserIdSet supplierUserIdSet,
        Guid dataShareRequestId,
        string commentsToAcquirer)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!;

            var previousDataShareRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            await DoUpdateDataShareRequestStatus(dbConnection, dbTransaction);

            await DoRecordDataShareRequestStatusChangeWithCommentsInAuditLogAsync(dbConnection, dbTransaction,
                dataShareRequestId, supplierUserIdSet, previousDataShareRequestStatus, [commentsToAcquirer]);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to ReturnSubmission";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }

        async Task DoUpdateDataShareRequestStatus(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            var existingRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            if (existingRequestStatus != DataShareRequestStatusType.InReview)
                throw new InconsistentDataException("Unable to Return Submission that is not in review");

            await databaseCommandRunner.DbExecuteAsync(
                dbConnection,
                dbTransaction,
                supplierDataShareRequestSqlQueries.UpdateDataShareRequestStatus,
                new
                {
                    DataShareRequestId = dataShareRequestId,
                    DataShareRequestStatus = DataShareRequestStatusType.Returned.ToString()
                }).ConfigureAwait(false);
        }
    }
    #endregion

    #region GetAcceptedDecisionSummaryAsync()
    async Task<AcceptedDecisionSummaryModelData> ISupplierDataShareRequestRepository.GetAcceptedDecisionSummaryAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await databaseCommandRunner.DbQuerySingleAsync<AcceptedDecisionSummaryModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                supplierDataShareRequestSqlQueries.GetAcceptedDecisionSummary,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetAcceptedDecisionSummary";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region GetRejectedDecisionSummaryAsync()
    async Task<RejectedDecisionSummaryModelData> ISupplierDataShareRequestRepository.GetRejectedDecisionSummaryAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await databaseCommandRunner.DbQuerySingleAsync<RejectedDecisionSummaryModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                supplierDataShareRequestSqlQueries.GetRejectedDecisionSummary,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetRejectedDecisionSummary";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region GetReturnedDecisionSummaryAsync()
    async Task<ReturnedDecisionSummaryModelData> ISupplierDataShareRequestRepository.GetReturnedDecisionSummaryAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await databaseCommandRunner.DbQuerySingleAsync<ReturnedDecisionSummaryModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                supplierDataShareRequestSqlQueries.GetReturnedDecisionSummary,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetReturnedDecisionSummary";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region GetDataShareRequestStatusAsync()
    async Task<DataShareRequestStatusType> ISupplierDataShareRequestRepository.GetDataShareRequestStatusAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await DoGetDataShareRequestStatusAsync(databaseChannel.Connection, databaseChannel.Transaction!, dataShareRequestId);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetDataShareRequestStatus";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region Get Data Share Request Notification Information
    async Task<DataShareRequestNotificationInformationModelData> ISupplierDataShareRequestRepository.GetDataShareRequestNotificationInformationAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await databaseCommandRunner.DbQuerySingleAsync<DataShareRequestNotificationInformationModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                supplierDataShareRequestSqlQueries.GetDataShareRequestNotificationInformation,
                new
                {
                    DataShareRequestId = dataShareRequestId
                });
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync().ConfigureAwait(false);

            const string errorMessage = "Failed to GetDataShareRequestNotificationInformation";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region Helpers
    private async Task<DataShareRequestStatusType> DoGetDataShareRequestStatusAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid dataShareRequestId)
    {
        try
        {
            var requestStatusModelData = await databaseCommandRunner.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                dbConnection,
                dbTransaction,
                supplierDataShareRequestSqlQueries.GetDataShareRequestStatus,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false);

            return requestStatusModelData.DataShareRequestStatus_RequestStatus;
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetDataShareRequestStatus from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
    }
    
    private async Task DoRecordDataShareRequestStatusChangeWithCommentsInAuditLogAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid dataShareRequestId,
        IUserIdSet changedByUser,
        DataShareRequestStatusType previousStatus,
        IEnumerable<string> comments)
    {
        try
        {
            var dataShareRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            var recordDataShareRequestStatusChangeWithCommentsParameters = new RecordDataShareRequestStatusChangeWithCommentsParameters
            {
                DbConnection = dbConnection,
                DbTransaction = dbTransaction,
                DataShareRequestId = dataShareRequestId,
                FromStatus = previousStatus,
                ToStatus = dataShareRequestStatus,
                ChangedByUser = changedByUser,
                ChangedAtLocalTime = clock.LocalNow,
                Comments = comments
            };

            await auditLogRepository.RecordDataShareRequestStatusChangeWithCommentsAsync(recordDataShareRequestStatusChangeWithCommentsParameters);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to record DataShareRequest Status Change in audit log";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
    }
    #endregion
}