using System.Data;
using Agrimetrics.DataShare.Api.Core.SystemProxies;
using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.FreeFormItems;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs.ParameterModels;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AcquirerDataShareRequests;

internal class AcquirerDataShareRequestRepository(
    ILogger<AcquirerDataShareRequestRepository> logger,
    IDatabaseChannelCreation databaseChannelCreation,
    IDatabaseCommandRunner databaseCommandRunner,
    IAcquirerDataShareRequestSqlQueries acquirerDataShareRequestSqlQueries,
    IClock clock,
    IAuditLogRepository auditLogRepository,
    IInputConstraintConfigurationPresenter inputConstraintConfigurationPresenter) : IAcquirerDataShareRequestRepository
{
    #region Find Question Set
    async Task<Guid> IAcquirerDataShareRequestRepository.FindQuestionSetAsync(
        int? supplierDomainId,
        int supplierOrganisationId,
        Guid esdaId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await GetQuestionSetForEsdaAndSupplierDomainAsync(databaseChannel.Connection, databaseChannel.Transaction!)
                 ?? await GetQuestionSetForEsdaAndSupplierOrganisationAsync(databaseChannel.Connection, databaseChannel.Transaction!)
                 ?? await GetDefaultQuestionSetForSupplierDomainAsync(databaseChannel.Connection, databaseChannel.Transaction!)
                 ?? await GetDefaultQuestionSetForSupplierOrganisationAsync(databaseChannel.Connection, databaseChannel.Transaction!)
                 ?? await GetMasterQuestionSetAsync(databaseChannel.Connection, databaseChannel.Transaction!);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to FindQuestionSet in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }

        async Task<Guid?> GetQuestionSetForEsdaAndSupplierDomainAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            return await databaseCommandRunner.DbQuerySingleOrDefaultAsync<Guid?>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetQuestionSetIdForEsdaAndSupplierDomain,
                new
                {
                    SupplierDomainId = supplierDomainId,
                    SupplierOrganisationId = supplierOrganisationId,
                    EsdaId = esdaId
                }
            );
        }

        async Task<Guid?> GetQuestionSetForEsdaAndSupplierOrganisationAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            return await databaseCommandRunner.DbQuerySingleOrDefaultAsync<Guid?>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetQuestionSetIdForEsdaAndSupplierOrganisation,
                new
                {
                    SupplierOrganisationId = supplierOrganisationId,
                    EsdaId = esdaId
                });
        }

        async Task<Guid?> GetDefaultQuestionSetForSupplierDomainAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            return await databaseCommandRunner.DbQuerySingleOrDefaultAsync<Guid?>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetDefaultQuestionSetIdForSupplierDomain,
                new
                {
                    SupplierDomainId = supplierDomainId,
                    SupplierOrganisationId = supplierOrganisationId
                });
        }

        async Task<Guid?> GetDefaultQuestionSetForSupplierOrganisationAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            return await databaseCommandRunner.DbQuerySingleOrDefaultAsync<Guid?>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetDefaultQuestionSetIdForSupplierOrganisation,
                new
                {
                    SupplierOrganisationId = supplierOrganisationId
                });
        }

        async Task<Guid> GetMasterQuestionSetAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            return await databaseCommandRunner.DbQuerySingleAsync<Guid>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetMasterQuestionSetId,
                parameters: null);
        }
    }
    #endregion

    #region Get Question Set Outline
    async Task<QuestionSetOutlineModelData> IAcquirerDataShareRequestRepository.GetQuestionSetOutlineRequestAsync(
        Guid questionSetId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var questionSetOutlineModelDatasFlattened = (await databaseCommandRunner.DbQueryAsync<
                QuestionSetOutlineModelData,
                QuestionSetSectionOutlineModelData,
                QuestionSetQuestionOutlineModelData,
                QuestionSetOutlineModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                acquirerDataShareRequestSqlQueries.GetQuestionSetOutlineRequest,
                (set, section, question) =>
                {
                    section.Questions.Add(question);
                    set.Sections.Add(section);
                    return set;
                },
                nameof(QuestionSetSectionOutlineModelData.QuestionSetSectionOutline_Id) + nameof(QuestionSetQuestionOutlineModelData.QuestionSetQuestionOutline_Id),
                new
                {
                    QuestionSetId = questionSetId
                }).ConfigureAwait(false)).ToList();

            return BuildGroupedData();

            QuestionSetOutlineModelData BuildGroupedData()
            {
                var firstRecordInGroup = questionSetOutlineModelDatasFlattened[0];

                firstRecordInGroup.Sections = BuildSections(questionSetOutlineModelDatasFlattened).ToList();

                return firstRecordInGroup;
            }

            IEnumerable<QuestionSetSectionOutlineModelData> BuildSections(IEnumerable<QuestionSetOutlineModelData> questionSetOutlineModelDataRecords)
            {
                var allSections = questionSetOutlineModelDataRecords.SelectMany(x => x.Sections);

                var sectionsGroupedById = allSections.GroupBy(x => x.QuestionSetSectionOutline_Id);

                foreach (var sectionGroup in sectionsGroupedById)
                {
                    var sectionModelDatasFlattened = sectionGroup.ToList();

                    var firstRecordInGroup = sectionModelDatasFlattened[0];

                    firstRecordInGroup.Questions = sectionModelDatasFlattened.SelectMany(x => x.Questions).ToList();

                    yield return firstRecordInGroup;
                }
            }
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetQuestionSetOutlineRequest";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region Get Data Share Request Resource Name
    async Task<string> IAcquirerDataShareRequestRepository.GetDataShareRequestResourceNameAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await databaseCommandRunner.DbQuerySingleAsync<string>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                acquirerDataShareRequestSqlQueries.GetEsdaNameForDataShareRequest,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetDataShareRequestResourceName from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region Start Data Share Request
    async Task<Guid> IAcquirerDataShareRequestRepository.StartDataShareRequestAsync(
        IUserIdSet acquirerUserIdSet,
        int supplierDomainId,
        int supplierOrganisationId,
        Guid esdaId,
        string esdaName,
        Guid questionSetId)
    {
        ArgumentNullException.ThrowIfNull(acquirerUserIdSet);
        ArgumentException.ThrowIfNullOrWhiteSpace(esdaName);

        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var questionSetQuestionParts = await GetQuestionSetQuestionPartsAsync(databaseChannel.Connection, databaseChannel.Transaction!);

            var dataShareRequestId = await CreateDataShareRequestAsync(databaseChannel.Connection, databaseChannel.Transaction!, questionSetQuestionParts);

            await RecordCreationInAuditLogAsync(databaseChannel.Connection, databaseChannel.Transaction!, dataShareRequestId);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);

            return dataShareRequestId;
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to StartDataShareRequest";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        { 
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }

        async Task<List<QuestionSetQuestionModelData>> GetQuestionSetQuestionPartsAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            try
            {
                var questionSetQuestionModelDataFlattened = await databaseCommandRunner.DbQueryAsync
                    <QuestionSetQuestionModelData, QuestionSetQuestionPartModelData, QuestionSetQuestionModelData>(
                        dbConnection,
                        dbTransaction,
                        acquirerDataShareRequestSqlQueries.GetQuestionSetQuestionParts,
                        (question, questionPart) =>
                        {
                            question.QuestionSet_QuestionParts.Add(questionPart);
                            return question;
                        },
                        nameof(QuestionSetQuestionPartModelData.QuestionSet_QuestionPartId),
                        new
                        {
                            QuestionSetId = questionSetId
                        });

                return BuildGroupedData();

                List<QuestionSetQuestionModelData> BuildGroupedData()
                {
                    var questionSetQuestionModelDataGrouped =
                        questionSetQuestionModelDataFlattened.GroupBy(x => x.QuestionSet_QuestionId).ToList();

                    return questionSetQuestionModelDataGrouped.Select(questionSetQuestionModelGroup =>
                    {
                        var firstRecordInGroup = questionSetQuestionModelGroup.First();

                        firstRecordInGroup.QuestionSet_QuestionParts = questionSetQuestionModelGroup.SelectMany(x => x.QuestionSet_QuestionParts).ToList();

                        return firstRecordInGroup;
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to StartDataShareRequest as failed to read QuestionSet from database";

                logger.LogError(ex, errorMessage);

                throw new DatabaseAccessGeneralException(errorMessage, ex);
            }
        }

        async Task<Guid> CreateDataShareRequestAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction,
            IEnumerable<QuestionSetQuestionModelData> questionSetQuestionParts)
        {
            try
            {
                var totalNumberOfDataShareRequests = await databaseCommandRunner.DbQuerySingleAsync<int>(
                    dbConnection,
                    dbTransaction,
                    acquirerDataShareRequestSqlQueries.GetTotalNumberOfDataShareRequests,
                    parameters: null);

                var requestId = $"DSR{(totalNumberOfDataShareRequests+1):D8}";

                var dataShareRequestId = await databaseCommandRunner.DbExecuteScalarAsync<Guid>(
                    dbConnection,
                    dbTransaction,
                    acquirerDataShareRequestSqlQueries.CreateDataShareRequest,
                    new
                    {
                        AcquirerUserId = acquirerUserIdSet.UserId,
                        AcquirerDomainId = acquirerUserIdSet.DomainId,
                        AcquirerOrganisationId = acquirerUserIdSet.OrganisationId,
                        EsdaId = esdaId,
                        EsdaName = esdaName,
                        SupplierDomainId = supplierDomainId,
                        SupplierOrganisationId = supplierOrganisationId,
                        QuestionSetId = questionSetId,
                        RequestId = requestId
                    });

                var answerSetId = await databaseCommandRunner.DbExecuteScalarAsync<Guid>(
                    dbConnection,
                    dbTransaction,
                    acquirerDataShareRequestSqlQueries.CreateAnswerSet,
                    new
                    {
                        DataShareRequestId = dataShareRequestId
                    });

                foreach (var question in questionSetQuestionParts)
                {
                    var answerId = await databaseCommandRunner.DbExecuteScalarAsync<Guid>(
                        dbConnection,
                        dbTransaction,
                        acquirerDataShareRequestSqlQueries.CreateAnswer,
                        new
                        {
                            AnswerSetId = answerSetId,
                            QuestionId = question.QuestionSet_QuestionId
                        });

                    foreach (var questionPart in question.QuestionSet_QuestionParts)
                    {
                        await databaseCommandRunner.DbExecuteScalarAsync(
                            dbConnection,
                            dbTransaction,
                            acquirerDataShareRequestSqlQueries.CreateAnswerPart,
                            new
                            {
                                AnswerId = answerId,
                                QuestionPartId = questionPart.QuestionSet_QuestionPartId
                            });
                    }
                }

                return dataShareRequestId;
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to StartDataShareRequest as failed to create DataShareRequest in database";

                logger.LogError(ex, errorMessage);

                throw new DatabaseAccessGeneralException(errorMessage, ex);
            }
        }

        async Task RecordCreationInAuditLogAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction,
            Guid dataShareRequestId)
        {
            try
            {
                var dataShareRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

                var recordDataShareRequestStatusChangeParameters = new RecordDataShareRequestStatusChangeParameters
                {
                    DbConnection = dbConnection,
                    DbTransaction = dbTransaction,
                    DataShareRequestId = dataShareRequestId,
                    FromStatus = DataShareRequestStatusType.None,
                    ToStatus = dataShareRequestStatus.DataShareRequestStatus_RequestStatus,
                    ChangedByUser = acquirerUserIdSet,
                    ChangedAtLocalTime = clock.LocalNow
                };
                await auditLogRepository.RecordDataShareRequestStatusChangeAsync(recordDataShareRequestStatusChangeParameters);
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to StartDataShareRequest as failed to record creation in audit log";

                logger.LogError(ex, errorMessage);

                throw new DatabaseAccessGeneralException(errorMessage, ex);
            }
        }
    }
    #endregion

    #region Get Data Share Request Status
    async Task<DataShareRequestStatusTypeModelData> IAcquirerDataShareRequestRepository.GetDataShareRequestStatusAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();
        
        try
        {
            return await DoGetDataShareRequestStatusAsync(databaseChannel.Connection, databaseChannel.Transaction!, dataShareRequestId);
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetDataShareRequestStatus from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region Update Question Statuses
    async Task IAcquirerDataShareRequestRepository.UpdateDataShareRequestQuestionStatusesAsync(
        Guid dataShareRequestId,
        bool questionsRemainThatRequireAResponse,
        IEnumerable<IDataShareRequestQuestionStatusDataModel> dataShareRequestQuestionStatuses)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestQuestionStatuses);

        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var answerSetId = await databaseCommandRunner.DbQuerySingleAsync<Guid>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                acquirerDataShareRequestSqlQueries.GetDataShareRequestAnswerSetId,
                new
                {
                    DataShareRequestId = dataShareRequestId
                });

            foreach (var dataShareRequestQuestionStatus in dataShareRequestQuestionStatuses)
            {
                await databaseCommandRunner.DbExecuteScalarAsync(
                    databaseChannel.Connection,
                    databaseChannel.Transaction!,
                    acquirerDataShareRequestSqlQueries.UpdateDataShareRequestQuestionStatus,
                    new
                    {
                        AnswerSetId = answerSetId,
                        QuestionId = dataShareRequestQuestionStatus.QuestionId,
                        QuestionStatus = dataShareRequestQuestionStatus.QuestionStatus.ToString()
                    });
            }

            await databaseCommandRunner.DbExecuteScalarAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                acquirerDataShareRequestSqlQueries.UpdateDataShareRequestCompleteness,
                new
                {
                    DataShareRequestId = dataShareRequestId,
                    QuestionsRemainThatRequireAResponse = questionsRemainThatRequireAResponse
                });

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to UpdateDataShareRequestQuestionStatuses in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region Get Data Share Requests
    async Task<IEnumerable<DataShareRequestModelData>> IAcquirerDataShareRequestRepository.GetDataShareRequestsAsync(
        int? acquirerUserId,
        int? acquirerDomainId,
        int? acquirerOrganisationId,
        int? supplierDomainId,
        int? supplierOrganisationId,
        Guid? esdaId,
        IEnumerable<DataShareRequestStatus>? dataShareRequestStatuses)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dataShareRequestStatusesList = dataShareRequestStatuses?.ToList() ?? [];

            var dataShareRequestModelDatas = await databaseCommandRunner.DbQueryAsync<DataShareRequestModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                acquirerDataShareRequestSqlQueries.GetDataShareRequestModelData,
                new
                {
                    AcquirerUserId = acquirerUserId,
                    AcquirerDomainId = acquirerDomainId,
                    AcquirerOrganisationId = acquirerOrganisationId,
                    SupplierDomainId = supplierDomainId,
                    SupplierOrganisationId = supplierOrganisationId,
                    EsdaId = esdaId,
                    HasDataShareRequestStatuses = dataShareRequestStatusesList.Any(),
                    DataShareRequestStatuses = dataShareRequestStatusesList.Select(x => x.ToString()).ToList()
                }).ConfigureAwait(false);

            return dataShareRequestModelDatas;
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetDataShareRequests from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region Get Questions Summary
    async Task<DataShareRequestQuestionsSummaryModelData> IAcquirerDataShareRequestRepository.GetDataShareRequestQuestionsSummaryAsync(
        Guid dataShareRequestId)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestId);

        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dataShareRequestQuestionsSummaryModelDataFlattened = (await databaseCommandRunner.DbQueryAsync<
                DataShareRequestQuestionsSummaryModelData,
                QuestionSetSummaryModelData,
                QuestionSetSectionSummaryModelData,
                QuestionSummaryModelData,
                DataShareRequestQuestionsSummaryModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                acquirerDataShareRequestSqlQueries.GetDataShareRequestQuestionsSummaryModelData,
                (questionsSummary, questionSetSummary, questionSetSectionSummary, questionSummary) =>
                {
                    questionSetSectionSummary.QuestionSetSection_QuestionSummaries.Add(questionSummary);

                    questionSetSummary.QuestionSet_SectionSummaries.Add(questionSetSectionSummary);

                    questionsSummary.DataShareRequest_QuestionSetSummary = questionSetSummary;
                    return questionsSummary;
                },
                $"{nameof(QuestionSetSummaryModelData.QuestionSet_Id)}, {nameof(QuestionSetSectionSummaryModelData.QuestionSetSection_Id)}, {nameof(QuestionSummaryModelData.Question_Id)}",
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false)).ToList();

            return BuildGroupedModelData();

            DataShareRequestQuestionsSummaryModelData BuildGroupedModelData()
            {
                var firstRecordInGroup = dataShareRequestQuestionsSummaryModelDataFlattened[0];

                firstRecordInGroup.DataShareRequest_QuestionSetSummary.QuestionSet_SectionSummaries = [.. BuildGroupedQuestionSetSections(dataShareRequestQuestionsSummaryModelDataFlattened)];

                return firstRecordInGroup;
            }

            IEnumerable<QuestionSetSectionSummaryModelData> BuildGroupedQuestionSetSections(IEnumerable<DataShareRequestQuestionsSummaryModelData> questionsSummaryModelDatas)
            {
                var allSectionSummaries = questionsSummaryModelDatas.SelectMany(x => x.DataShareRequest_QuestionSetSummary.QuestionSet_SectionSummaries);

                var groupedSectionSummaries = allSectionSummaries.GroupBy(x => x.QuestionSetSection_Id);

                foreach (var sectionSummaryGroup in groupedSectionSummaries)
                {
                    var firstRecordInGroup = sectionSummaryGroup.First();

                    var sectionSummaries = sectionSummaryGroup.ToList();

                    firstRecordInGroup.QuestionSetSection_QuestionSummaries = [.. sectionSummaries.SelectMany(x => x.QuestionSetSection_QuestionSummaries)];

                    yield return firstRecordInGroup;
                }
            }
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetDataShareRequestQuestionsSummary from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region Get Data Share Request Question
    async Task<DataShareRequestQuestionModelData> IAcquirerDataShareRequestRepository.GetDataShareRequestQuestionAsync(
        Guid dataShareRequestId,
        Guid questionId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var questionModelData = await DoBuildQuestionModelDataAsync(databaseChannel.Connection, databaseChannel.Transaction!, dataShareRequestId, questionId);

            questionModelData.DataShareRequestQuestion_QuestionFooter = await DoBuildQuestionFooterDataAsync(databaseChannel.Connection, databaseChannel.Transaction!, questionId);

            return questionModelData;
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetDataShareRequestQuestion from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    private async Task<DataShareRequestQuestionModelData> DoBuildQuestionModelDataAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid dataShareRequestId,
        Guid questionId)
    {
        var dataShareRequestQuestionModelDataFlattened = (await databaseCommandRunner.DbQueryAsync<
                DataShareRequestQuestionModelData,
                QuestionPartModelData,
                QuestionPartPromptsModelData,
                QuestionPartMultipleAnswerItemControlModelData,
                QuestionPartResponseTypeInformationModelData,
                QuestionPartAnswerModelData?,
                QuestionPartAnswerResponseInformationModelData?,
                DataShareRequestQuestionModelData>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetDataShareRequestQuestionModelData,
            (question,
                questionPart, prompts, multipleAnswerItemControl, responseTypeInformation,
                questionAnswerPart, questionAnswerPartInformation) =>
            {
                questionPart.QuestionPart_Prompts = prompts;
                questionPart.QuestionPart_MultipleAnswerItemControl = multipleAnswerItemControl;
                questionPart.QuestionPart_ResponseTypeInformation = responseTypeInformation;

                if (questionAnswerPart != null && questionAnswerPartInformation != null)
                {
                    questionAnswerPart.QuestionPartAnswer_AnswerPartResponseInformations.Add(questionAnswerPartInformation);
                }

                question.DataShareRequestQuestion_QuestionParts.Add(new DataShareRequestQuestionPartModelData
                {
                    DataShareRequestQuestionPart_Question = questionPart,
                    DataShareRequestQuestionPart_Answer = questionAnswerPart
                });
                return question;
            },
            $"{nameof(QuestionPartModelData.QuestionPart_Id)}, " +
            $"{nameof(QuestionPartPromptsModelData.QuestionPartPrompt_QuestionPartId)}, " +
            $"{nameof(QuestionPartMultipleAnswerItemControlModelData.QuestionPartMultipleAnswerItemControl_QuestionPartId)}, " +
            $"{nameof(QuestionPartResponseTypeInformationModelData.QuestionPartResponseTypeInformation_QuestionPartId)}, " +
            $"{nameof(QuestionPartAnswerModelData.QuestionPartAnswer_Id)}, " +
            $"{nameof(QuestionPartAnswerResponseInformationModelData.QuestionPartAnswerResponse_Id)}",
            new
            {
                DataShareRequestId = dataShareRequestId,
                QuestionId = questionId
            }).ConfigureAwait(false)).ToList();

        return await DoBuildGroupedQuestionModelDataAsync(dbConnection, dbTransaction, dataShareRequestQuestionModelDataFlattened);
    }

    private async Task<DataShareRequestQuestionModelData> DoBuildGroupedQuestionModelDataAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        IReadOnlyList<DataShareRequestQuestionModelData> dataShareRequestQuestionModelDataFlattened)
    {
        var firstRecordInGroup = dataShareRequestQuestionModelDataFlattened[0];

        var allQuestionParts = dataShareRequestQuestionModelDataFlattened.SelectMany(x => x.DataShareRequestQuestion_QuestionParts).ToList();

        var questionPartsGroupedByQuestionPartId = allQuestionParts.GroupBy(x => x.DataShareRequestQuestionPart_Answer!.QuestionPartAnswer_QuestionPartId);

        var groupedQuestionParts = await Task.Run(() =>
        {
            return questionPartsGroupedByQuestionPartId
                .Select(async questionPartsInGroup => await DoBuildGroupedDataShareRequestQuestionPartModelDataAsync(dbConnection, dbTransaction, [.. questionPartsInGroup]))
                .Select(x => x.Result)
                .ToList();
        });

        firstRecordInGroup.DataShareRequestQuestion_QuestionParts = [.. groupedQuestionParts];

        return firstRecordInGroup;
    }

    private async Task<DataShareRequestQuestionPartModelData> DoBuildGroupedDataShareRequestQuestionPartModelDataAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        IReadOnlyList<DataShareRequestQuestionPartModelData> questionPartsInGroup)
    {
        var firstQuestionPartInGroup = questionPartsInGroup[0];

        firstQuestionPartInGroup.DataShareRequestQuestionPart_Question.QuestionPart_ResponseFormat =
            await DoBuildQuestionPartResponseFormatAsync(firstQuestionPartInGroup.DataShareRequestQuestionPart_Question, dbConnection, dbTransaction);

        if (firstQuestionPartInGroup.DataShareRequestQuestionPart_Answer != null)
        {
            var allAnswerPartResponseInformations = questionPartsInGroup.SelectMany(x =>
                x.DataShareRequestQuestionPart_Answer!.QuestionPartAnswer_AnswerPartResponseInformations).ToList();

            var answerPartResponseInformations = DoFilterDuplicatedQuestionAnswerResponseInformations(
                allAnswerPartResponseInformations, firstQuestionPartInGroup);

            firstQuestionPartInGroup.DataShareRequestQuestionPart_Answer.QuestionPartAnswer_AnswerPartResponses = answerPartResponseInformations
                .Select(async answerPartResponseInformation => await DoBuildQuestionAnswerPartResponseAsync(answerPartResponseInformation, dbConnection, dbTransaction))
                .Select(x => x.Result)
                .ToList();
        }

        return firstQuestionPartInGroup;
    }

    /// <remarks>
    /// There should only ever be exactly one response item per response.  However, in early development there was an issue whereby
    /// selection option responses were being reported incorrectly, and if N options were selected then N responses were received, with
    /// each containing the full N selected options.
    /// Therefore, to cater for some of the bad data that may remain from those early stages, a list is returned but only
    /// the first response is actually used
    /// </remarks>>
    private static IEnumerable<QuestionPartAnswerResponseInformationModelData> DoFilterDuplicatedQuestionAnswerResponseInformations(
        IReadOnlyCollection<QuestionPartAnswerResponseInformationModelData> allAnswerPartResponseInformations,
        DataShareRequestQuestionPartModelData firstQuestionPartInGroup)
    {
        var multipleAnswerItemsAreAllowed =
            firstQuestionPartInGroup.DataShareRequestQuestionPart_Question.QuestionPart_MultipleAnswerItemControl.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed;

        if (!multipleAnswerItemsAreAllowed && allAnswerPartResponseInformations.Count > 1) return [allAnswerPartResponseInformations.First(x => x.QuestionPartAnswerItem_OrderWithinAnswerPart == 1)];

        return allAnswerPartResponseInformations;
    }

    private async Task<DataShareRequestQuestionFooterModelData?> DoBuildQuestionFooterDataAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid questionId)
    {
        var questionFooterDatasFlattened = (await databaseCommandRunner.DbQueryAsync<
            DataShareRequestQuestionFooterModelData?,
            DataShareRequestQuestionFooterItemModelData?,
            DataShareRequestQuestionFooterModelData?>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetDataShareRequestQuestionFooterData,
            (footer, footerItem) =>
            {
                if (footerItem != null)
                {
                    footer!.DataShareRequestQuestionFooter_Items.Add(footerItem);
                }
                return footer;
            },
            nameof(DataShareRequestQuestionFooterItemModelData.DataShareRequestQuestionFooterItem_Id),
            new
            {
                QuestionId = questionId
            }).ConfigureAwait(false)).ToList();

        return DoBuildQuestionGroupedFooterModel(questionFooterDatasFlattened);
    }

    private static DataShareRequestQuestionFooterModelData? DoBuildQuestionGroupedFooterModel(
        IReadOnlyCollection<DataShareRequestQuestionFooterModelData?> questionFooterDatasFlattened)
    {
        var firstRecordInGroup = questionFooterDatasFlattened.FirstOrDefault();
        if (firstRecordInGroup == null) return null;

        firstRecordInGroup.DataShareRequestQuestionFooter_Items = questionFooterDatasFlattened
            .SelectMany(x => x!.DataShareRequestQuestionFooter_Items).ToList();

        return firstRecordInGroup;
    }
    #endregion

    #region Get Question Status Informations
    async Task<DataShareRequestQuestionStatusInformationSetModelData> IAcquirerDataShareRequestRepository.GetDataShareRequestQuestionStatusInformationsAsync(
        Guid dataShareRequestId)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestId);

        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dataShareRequestQuestionStatusesFlattened = (await databaseCommandRunner.DbQueryAsync<
                DataShareRequestQuestionStatusInformationSetModelData,
                DataShareRequestQuestionStatusInformationModelData,
                QuestionSetQuestionInformationModelData,
                QuestionResponseInformationDataModel,
                QuestionPartResponseDataModel,
                QuestionPreRequisiteDataModel?,
                QuestionSetQuestionApplicabilityOverride?,
                DataShareRequestQuestionStatusInformationSetModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                acquirerDataShareRequestSqlQueries.GetDataShareRequestQuestionStatusInformationsModelData,
                (set, question, questionSetQuestionInformation, responseInformation, partResponse, prerequisite,
                    applicabilityOverride) =>
                {
                    if (applicabilityOverride != null)
                    {
                        question.SelectionOptionQuestionSetQuestionApplicabilityOverrides.Add(applicabilityOverride);
                    }

                    if (prerequisite != null)
                    {
                        question.QuestionPreRequisites.Add(prerequisite);
                    }

                    responseInformation.QuestionResponseInformation_QuestionPartResponses.Add(partResponse);
                    question.QuestionSetQuestionInformation = questionSetQuestionInformation;
                    question.QuestionResponseInformation = responseInformation;

                    set.DataShareRequestQuestionStatuses.Add(question);

                    return set;
                },
                $"{nameof(DataShareRequestQuestionStatusInformationModelData.DataShareRequestQuestionStatus_QuestionId)}, " +
                $"{nameof(QuestionSetQuestionInformationModelData.QuestionSet_SectionNumber)}, " +
                $"{nameof(QuestionResponseInformationDataModel.QuestionResponseInformation_QuestionId)}, " +
                $"{nameof(QuestionPartResponseDataModel.QuestionPartResponse_ResponseInputType)}, " +
                $"{nameof(QuestionPreRequisiteDataModel.QuestionPreRequisite_QuestionId)}, " +
                $"{nameof(QuestionSetQuestionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControlledQuestionId)}",
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false)).ToList();

            var groupedModelData = BuildGroupedModelData();
            return groupedModelData;

            DataShareRequestQuestionStatusInformationSetModelData BuildGroupedModelData()
            {
                var firstRecordInGroup = dataShareRequestQuestionStatusesFlattened[0];

                firstRecordInGroup.DataShareRequestQuestionStatuses = [.. BuildGroupedDataShareRequestStatuses()];

                return firstRecordInGroup;
            }

            IEnumerable<DataShareRequestQuestionStatusInformationModelData> BuildGroupedDataShareRequestStatuses()
            {
                var dataShareRequestQuestionStatusesGroupedByQuestion = dataShareRequestQuestionStatusesFlattened
                    .SelectMany(x => x.DataShareRequestQuestionStatuses).GroupBy(x => x.DataShareRequestQuestionStatus_QuestionId);

                foreach (var dataShareRequestQuestionStatus in dataShareRequestQuestionStatusesGroupedByQuestion)
                {
                    var firstRecordInGroup = dataShareRequestQuestionStatus.First();

                    firstRecordInGroup.QuestionResponseInformation.QuestionResponseInformation_QuestionPartResponses =
                        dataShareRequestQuestionStatus.SelectMany(x => x.QuestionResponseInformation.QuestionResponseInformation_QuestionPartResponses).ToList();

                    firstRecordInGroup.QuestionPreRequisites =
                        dataShareRequestQuestionStatus.SelectMany(x => x.QuestionPreRequisites).ToList();

                    firstRecordInGroup.SelectionOptionQuestionSetQuestionApplicabilityOverrides =
                        dataShareRequestQuestionStatus.SelectMany(x => x.SelectionOptionQuestionSetQuestionApplicabilityOverrides).ToList();

                    yield return firstRecordInGroup;
                }
            }
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetDataShareRequestQuestionStatusInformations from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region Set Question Answer
    async Task IAcquirerDataShareRequestRepository.SetDataShareRequestQuestionAnswerAsync(
        DataShareRequestQuestionAnswerWriteModelData questionAnswerWriteData)
    {
        ArgumentNullException.ThrowIfNull(questionAnswerWriteData);

        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!;
            
            var answerId = await databaseCommandRunner.DbQuerySingleAsync<Guid>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetDataShareRequestQuestionAnswerId,
                new
                {
                    DataShareRequestId = questionAnswerWriteData.DataShareRequestId,
                    QuestionId = questionAnswerWriteData.QuestionId
                });

            foreach (var answerPart in questionAnswerWriteData.AnswerParts)
            {
                await DoWriteQuestionAnswerPartAsync(dbConnection, dbTransaction, answerId, answerPart);
            }

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to SetDataShareRequestQuestionAnswerAsync in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    private async Task<Guid> DoWriteQuestionAnswerPartAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid answerId,
        DataShareRequestQuestionAnswerPartWriteModelData answerPart)
    {
        var answerPartId = await databaseCommandRunner.DbQuerySingleAsync<Guid>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetQuestionAnswerPartId,
            new
            {
                AnswerId = answerId,
                QuestionPartId = answerPart.QuestionPartId
            });

        // Delete any existing answer part responses for this answer part - all answer part responses will be of the same type, so just pass in the first
        await DoDeleteExistingQuestionAnswerPartResponsesAsync(dbConnection, dbTransaction, answerPartId, answerPart.AnswerPartResponses[0].InputType);

        foreach (var answerPartResponse in answerPart.AnswerPartResponses)
        {
            await DoWriteQuestionAnswerPartResponseAsync(dbConnection, dbTransaction, answerId, answerPartId, answerPartResponse);
        }

        return answerPartId;
    }

    private async Task DoWriteQuestionAnswerPartResponseFreeFormAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid answerPartResponseItemId,
        DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData answerPartResponseFreeForm)
    {
        await databaseCommandRunner.DbExecuteAsync(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.CreateAnswerPartResponseItemFreeForm,
            new
            {
                Id = answerPartResponseItemId,
                OrderWithinAnswerPartResponse = 1,
                EnteredValue = answerPartResponseFreeForm.EnteredValue,
                ValueEntryDeclined = answerPartResponseFreeForm.ValueEntryDeclined
            });
    }

    private async Task DoWriteQuestionAnswerPartResponseSelectionOptionAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid answerId,
        Guid answerPartResponseItemId,
        DataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData answerPartResponseSelectionOption)
    {
        foreach (var selectionOption in answerPartResponseSelectionOption.SelectionOptions)
        {
            var supplementaryQuestionPartAnswerId = selectionOption.SupplementaryQuestionAnswerPart == null
                ? (Guid?)null
                : await DoWriteQuestionAnswerPartAsync(dbConnection, dbTransaction, answerId, selectionOption.SupplementaryQuestionAnswerPart);

            await databaseCommandRunner.DbExecuteAsync(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.CreateAnswerPartResponseItemSelectionOption,
                new
                {
                    AnswerPartResponseItemId = answerPartResponseItemId,
                    SelectionOptionId = selectionOption.OptionSelectionId,
                    SupplementaryQuestionPartAnswerId = supplementaryQuestionPartAnswerId
                });
        }
    }

    private async Task DoWriteQuestionAnswerPartResponseAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid answerId,
        Guid answerPartId,
        DataShareRequestQuestionAnswerPartResponseWriteModelData answerPartResponse)
    {
        var answerPartResponseId = await databaseCommandRunner.DbExecuteScalarAsync<Guid>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.CreateAnswerPartResponse,
            new
            {
                AnswerPartId = answerPartId,
                OrderWithinAnswerPart = answerPartResponse.OrderWithinAnswerPart
            });

        var answerPartResponseItemId = await databaseCommandRunner.DbExecuteScalarAsync<Guid>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.CreateAnswerPartResponseItem,
            new
            {
                AnswerPartResponseId = answerPartResponseId
            });

        await (answerPartResponse.InputType switch
        {
            QuestionPartResponseInputType.FreeForm => DoWriteQuestionAnswerPartResponseFreeFormAsync(
                dbConnection,
                dbTransaction,
                answerPartResponseItemId,
                (DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData)answerPartResponse),

            QuestionPartResponseInputType.OptionSelection => DoWriteQuestionAnswerPartResponseSelectionOptionAsync(
                dbConnection,
                dbTransaction,
                answerId,
                answerPartResponseItemId,
                (DataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData)answerPartResponse),

            _ => throw new InvalidEnumValueException("AnswerPart has unsupported InputType")
        });
    }

    private async Task DoDeleteExistingQuestionAnswerPartResponsesAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid answerPartId,
        QuestionPartResponseInputType responseInputType)
    {
        var existingAnswerPartResponseIds = await databaseCommandRunner.DbQueryAsync<Guid>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetAnswerPartResponseIds,
            new
            {
                AnswerPartId = answerPartId
            });

        foreach (var answerPartResponseId in existingAnswerPartResponseIds)
        {
            var existingAnswerPartResponseItemIds = await databaseCommandRunner.DbQueryAsync<Guid>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetAnswerPartResponseItemId,
                new
                {
                    AnswerPartResponseId = answerPartResponseId
                });

            if (responseInputType == QuestionPartResponseInputType.FreeForm)
            {
                await DoDeleteExistingQuestionAnswerPartFreeFormResponseItemsAsync(dbConnection, dbTransaction,
                    existingAnswerPartResponseItemIds);
            }
            else
            {
                await DoDeleteExistingQuestionAnswerPartSelectionOptionResponseItemsAsync(dbConnection, dbTransaction,
                    existingAnswerPartResponseItemIds);
            }

            await databaseCommandRunner.DbExecuteAsync(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.DeleteAnswerPartResponse,
                new
                {
                    AnswerPartResponseId = answerPartResponseId
                });
        }
    }

    private async Task DoDeleteExistingQuestionAnswerPartSelectionOptionResponseItemsAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        IEnumerable<Guid> existingAnswerPartResponseItemIds)
    {
        foreach (var answerPartResponseItemId in existingAnswerPartResponseItemIds)
        {
            var answerPartSelectionOptionsIds = (await databaseCommandRunner.DbQueryAsync<Guid>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetAnswerPartResponseItemSelectionOptionIds,
                new
                {
                    AnswerPartResponseItemId = answerPartResponseItemId
                })).ToList();

            var supplementaryQuestionAnswerPartModelDatas = new List<SupplementaryQuestionAnswerPartModelData>();
            foreach (var answerPartSelectionOptionsId in answerPartSelectionOptionsIds)
            {
                var supplementaryQuestionAnswerPart = await databaseCommandRunner.DbQuerySingleOrDefaultAsync<SupplementaryQuestionAnswerPartModelData>(
                    dbConnection,
                    dbTransaction,
                    acquirerDataShareRequestSqlQueries.GetAnswerPartResponseItemSelectionOptionSupplementaryAnswerPartModelData,
                    new
                    {
                        Id = answerPartSelectionOptionsId
                    });

                if (supplementaryQuestionAnswerPart != null)
                {
                    supplementaryQuestionAnswerPartModelDatas.Add(supplementaryQuestionAnswerPart);
                }

                await databaseCommandRunner.DbExecuteAsync(
                    dbConnection,
                    dbTransaction,
                    acquirerDataShareRequestSqlQueries.DeleteAnswerPartResponseItemOptionSelect,
                    new
                    {
                        Id = answerPartSelectionOptionsId
                    });
            }

            await databaseCommandRunner.DbExecuteAsync(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.DeleteAnswerPartResponseItem,
                new
                {
                    AnswerPartResponseItemId = answerPartResponseItemId
                });

            foreach (var supplementaryQuestionAnswerPart in supplementaryQuestionAnswerPartModelDatas)
            {
                await DoDeleteExistingQuestionAnswerPartResponsesAsync(dbConnection, dbTransaction,
                    supplementaryQuestionAnswerPart.SupplementaryAnswerPart_AnswerPartId, supplementaryQuestionAnswerPart.SupplementaryAnswerPart_InputType);
            }
        }
    }
    private async Task DoDeleteExistingQuestionAnswerPartFreeFormResponseItemsAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        IEnumerable<Guid> existingAnswerPartResponseItemIds)
    {
        foreach (var answerPartResponseItemId in existingAnswerPartResponseItemIds)
        {
            await databaseCommandRunner.DbExecuteAsync(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.DeleteAnswerPartResponseItemFreeForm,
                new
                {
                    AnswerPartResponseItemId = answerPartResponseItemId
                });

            await databaseCommandRunner.DbExecuteAsync(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.DeleteAnswerPartResponseItem,
                new
                {
                    AnswerPartResponseItemId = answerPartResponseItemId
                });
        }
    }
    #endregion

    #region Submit Data Share Request
    async Task<DataShareRequestSubmissionResultModelData> IAcquirerDataShareRequestRepository.SubmitDataShareRequestAsync(
        IUserIdSet acquirerUserIdSet,
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!;

            var dataShareRequestPreviousStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            await DoSubmitDataShareRequestAsync(dbConnection, dbTransaction);

            await DoRecordDataShareRequestStatusChangeInAuditLogAsync(dbConnection, dbTransaction,
                dataShareRequestId, acquirerUserIdSet, dataShareRequestPreviousStatus.DataShareRequestStatus_RequestStatus);

            await DoCreateSubmissionIfRequiredAsync(dbConnection, dbTransaction, dataShareRequestPreviousStatus.DataShareRequestStatus_RequestStatus);

            var dataShareRequestRequestId = await DoGetDataShareRequestRequestIdAsync(dbConnection, dbTransaction);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);

            return new DataShareRequestSubmissionResultModelData
            {
                DataShareRequest_Id = dataShareRequestId,
                DataShareRequest_RequestId = dataShareRequestRequestId
            };
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to SubmitDataShareRequest in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }

        async Task DoSubmitDataShareRequestAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            try
            {
                await databaseCommandRunner.DbExecuteScalarAsync(
                    dbConnection,
                    dbTransaction,
                    acquirerDataShareRequestSqlQueries.SubmitDataShareRequest,
                    new
                    {
                        DataShareRequestId = dataShareRequestId
                    });
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to SubmitDataShareRequest as failed to set submitted in database";

                logger.LogError(ex, errorMessage);

                throw new DatabaseAccessGeneralException(errorMessage, ex);
            }
        }

        async Task DoCreateSubmissionIfRequiredAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction,
            DataShareRequestStatusType dataShareRequestStatusRequestStatus)
        {
            var existingSubmissionIds = (await databaseCommandRunner.DbQueryAsync<Guid>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetSubmissionIdsForDataShareRequest,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false)).ToList();

            switch (dataShareRequestStatusRequestStatus)
            {
                case DataShareRequestStatusType.Returned:
                {
                    if (!existingSubmissionIds.Any())
                    {
                        throw new InconsistentDataException("No Submission found for Data Share Request in Returned status");
                    }
                } break;

                case DataShareRequestStatusType.Draft:
                {
                    if (existingSubmissionIds.Any())
                    {
                        throw new InconsistentDataException("Unable to create Submission for Data Share Request in Draft status as one already exists");
                    }

                    await databaseCommandRunner.DbExecuteScalarAsync(
                        dbConnection,
                        dbTransaction,
                        acquirerDataShareRequestSqlQueries.CreateSubmissionForDataShareRequest,
                        new
                        {
                            DataShareRequestId = dataShareRequestId
                        }).ConfigureAwait(false);
                    } break;

                default:
                {
                    throw new InvalidEnumValueException(
                        $"Attempt made to create Submission for Data Share Request with unsupported status: {dataShareRequestStatusRequestStatus}");
                }
            }
        }

        async Task<string> DoGetDataShareRequestRequestIdAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            return await databaseCommandRunner.DbQuerySingleAsync<string>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetDataShareRequestRequestId,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false);
        }
    }
    #endregion

    #region Get Data Share Request Answers Summary
    async Task<DataShareRequestAnswersSummaryModelData> IAcquirerDataShareRequestRepository.GetDataShareRequestAnswersSummaryAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!;

            var dataShareRequestAnswersSummaryModelData = await DoGetDataShareRequestAnswersSummaryQuestionGroupsModelDataAsync(dataShareRequestId, dbConnection, dbTransaction);

            var allQuestionGroups = dataShareRequestAnswersSummaryModelData
                .DataShareRequestAnswersSummary_SummarySections.SelectMany(x => x.DataShareRequestAnswersSummarySection_QuestionGroups);

            foreach (var questionGroup in allQuestionGroups)
            {
                await DoPopulateDataShareRequestAnswersQuestionGroupAsync(questionGroup, dataShareRequestId, dbConnection, dbTransaction);
            }

            return dataShareRequestAnswersSummaryModelData;
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetDataShareRequestAnswersSummary from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    private async Task<DataShareRequestAnswersSummaryModelData> DoGetDataShareRequestAnswersSummaryQuestionGroupsModelDataAsync(
        Guid dataShareRequestId,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        var dataShareRequestAnswersSummaryModelDataFlattened = (await databaseCommandRunner.DbQueryAsync<
            DataShareRequestAnswersSummaryModelData,
            DataShareRequestAnswersSummarySectionModelData,
            DataShareRequestAnswersSummaryQuestionGroupModelData,
            Guid?,
            DataShareRequestAnswersSummaryModelData>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetDataShareRequestAnswersSummaryQuestionGroups,
            (answers, section, questionGroup, backingQuestionId) =>
            {
                if (backingQuestionId != null)
                {
                    questionGroup.DataShareRequestAnswersSummaryQuestionGroup_BackingQuestionIds.Add(backingQuestionId.Value);
                }

                section.DataShareRequestAnswersSummarySection_QuestionGroups.Add(questionGroup);
                answers.DataShareRequestAnswersSummary_SummarySections.Add(section);
                return answers;
            },
            $"{(nameof(DataShareRequestAnswersSummarySectionModelData.DataShareRequestAnswersSummarySection_SectionId))}, " +
            $"{(nameof(DataShareRequestAnswersSummaryQuestionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_MainQuestionId))}, " +
            "DataShareRequestAnswersSummaryQuestionGroup_BackingQuestionId",
            new
            {
                DataShareRequestId = dataShareRequestId
            }).ConfigureAwait(false)).ToList();

        return BuildGroupedData(dataShareRequestAnswersSummaryModelDataFlattened);

        static DataShareRequestAnswersSummaryModelData BuildGroupedData(IReadOnlyCollection<DataShareRequestAnswersSummaryModelData> dataShareRequestAnswersSummaryModelDataFlattened)
        {
            var firstRecordInGroup = dataShareRequestAnswersSummaryModelDataFlattened.First();

            firstRecordInGroup.DataShareRequestAnswersSummary_SummarySections = [.. BuildGroupedAnswerSummarySections(dataShareRequestAnswersSummaryModelDataFlattened)];

            return firstRecordInGroup;
        }

        static IEnumerable<DataShareRequestAnswersSummarySectionModelData> BuildGroupedAnswerSummarySections(IEnumerable<DataShareRequestAnswersSummaryModelData> answersSummaryModelDatas)
        {
            var allAnswerSummariesSections = answersSummaryModelDatas.SelectMany(x => x.DataShareRequestAnswersSummary_SummarySections);

            var answerSummarySectionsGroupedBySection = allAnswerSummariesSections.GroupBy(x => x.DataShareRequestAnswersSummarySection_SectionId);

            foreach (var answerSummarySectionGroup in answerSummarySectionsGroupedBySection)
            {
                var firstRecordInGroup = answerSummarySectionGroup.First();

                var allQuestionGroupsInSection =
                    answerSummarySectionGroup.SelectMany(x => x.DataShareRequestAnswersSummarySection_QuestionGroups).ToList();

                firstRecordInGroup.DataShareRequestAnswersSummarySection_QuestionGroups = [.. BuildGroupedQuestionGroups(allQuestionGroupsInSection)];

                yield return firstRecordInGroup;
            }
        }

        static IEnumerable<DataShareRequestAnswersSummaryQuestionGroupModelData> BuildGroupedQuestionGroups(IEnumerable<DataShareRequestAnswersSummaryQuestionGroupModelData> questionGroups)
        {
            var questionGroupsGroupedByMainQuestionId = questionGroups.GroupBy(x => x.DataShareRequestAnswersSummaryQuestionGroup_MainQuestionId);

            foreach (var questionGroup in questionGroupsGroupedByMainQuestionId)
            {
                var firstRecordInGroup = questionGroup.First();

                firstRecordInGroup.DataShareRequestAnswersSummaryQuestionGroup_BackingQuestionIds =
                    questionGroup.SelectMany(x => x.DataShareRequestAnswersSummaryQuestionGroup_BackingQuestionIds).ToList();

                yield return firstRecordInGroup;
            }
        }
    }

    private async Task DoPopulateDataShareRequestAnswersQuestionGroupAsync(
        DataShareRequestAnswersSummaryQuestionGroupModelData questionGroupModelData,
        Guid dataShareRequestId,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        var questionId = questionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_MainQuestionId;

        questionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion = await DoGetDataShareRequestAnswerSummaryQuestionModelDataAsync(
            dbConnection, dbTransaction, dataShareRequestId, questionId);

        foreach (var backingQuestionId in questionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_BackingQuestionIds)
        {
            var backingQuestion = await DoGetDataShareRequestAnswerSummaryQuestionModelDataAsync(
                dbConnection, dbTransaction, dataShareRequestId, backingQuestionId);

            questionGroupModelData.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions.Add(backingQuestion);
        }
    }

    private async Task<DataShareRequestAnswersSummaryQuestionModelData> DoGetDataShareRequestAnswerSummaryQuestionModelDataAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid dataShareRequestId,
        Guid questionId)
    {
        var dataShareRequestAnswersSummaryQuestionModelDataFlattened = (await databaseCommandRunner.DbQueryAsync<
            DataShareRequestAnswersSummaryQuestionModelData,
            DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData,
            DataShareRequestAnswersSummaryQuestionModelData>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetDataShareRequestAnswersSummaryQuestionAnswer,
            (question, questionPartId) =>
            {
                question.DataShareRequestAnswersSummaryQuestion_QuestionPartIds.Add(questionPartId);

                return question;
            },
            nameof(DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData.DataShareRequestAnswersSummaryQuestion_QuestionPartId),
            new
            {
                DataShareRequestId = dataShareRequestId,
                QuestionId = questionId
            }).ConfigureAwait(false)).ToList();

        return await DoBuildGroupedAnswersSummaryQuestionModelData(
            dbConnection, dbTransaction, dataShareRequestId, dataShareRequestAnswersSummaryQuestionModelDataFlattened);
    }

    private async Task<DataShareRequestAnswersSummaryQuestionModelData> DoBuildGroupedAnswersSummaryQuestionModelData(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid dataShareRequestId,
        IReadOnlyList<DataShareRequestAnswersSummaryQuestionModelData> dataShareRequestAnswersSummaryQuestionModelDataFlattened)
    {
        var firstEntryInGroup = dataShareRequestAnswersSummaryQuestionModelDataFlattened[0];

        var questionPartIds = dataShareRequestAnswersSummaryQuestionModelDataFlattened
            .SelectMany(x => x.DataShareRequestAnswersSummaryQuestion_QuestionPartIds)
            .Select(x => x.DataShareRequestAnswersSummaryQuestion_QuestionPartId);

        var summaryQuestionPartsWithResponses = await Task.Run(() =>
            questionPartIds
                .Select(async questionPartId => await DoGetAnswersSummaryQuestionPartWithResponsesModelDataAsync(dbConnection, dbTransaction, dataShareRequestId, questionPartId))
                .Select(x => x.Result)
                .ToList());

        firstEntryInGroup.DataShareRequestAnswersSummaryQuestion_QuestionParts = summaryQuestionPartsWithResponses;

        return firstEntryInGroup;
    }

    private async Task<DataShareRequestAnswersSummaryQuestionPartModelData> DoGetAnswersSummaryQuestionPartWithResponsesModelDataAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid dataShareRequestId,
        Guid questionPartId)
    {
        var answersSummaryQuestionPartModelDatasFlattened = (await databaseCommandRunner.DbQueryAsync<
                DataShareRequestAnswersSummaryQuestionPartModelData,
                DataShareRequestAnswersSummaryQuestionPartResponseModelData?,
                DataShareRequestAnswersSummaryQuestionPartModelData>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetDataShareRequestAnswersSummaryQuestionAnswerPartWithResponseIds,
                (questionPart, response) =>
                {
                    if (response != null)
                    {
                        questionPart.DataShareRequestAnswersSummaryQuestionPart_Responses.Add(response);
                    }
                    return questionPart;
                },
                nameof(DataShareRequestAnswersSummaryQuestionPartResponseModelData.DataShareRequestAnswersSummaryQuestionPartResponse_ResponseId),
                new
                {
                    DataShareRequestId = dataShareRequestId,
                    QuestionPartId = questionPartId
                })
            .ConfigureAwait(false)).ToList();

        return await DoBuildGroupedAnswersSummaryQuestionPartModelDataAsync(
            dbConnection, dbTransaction, answersSummaryQuestionPartModelDatasFlattened);
    }

    private async Task<DataShareRequestAnswersSummaryQuestionPartModelData> DoBuildGroupedAnswersSummaryQuestionPartModelDataAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        IReadOnlyList<DataShareRequestAnswersSummaryQuestionPartModelData> answersSummaryQuestionPartModelDatasFlattened)
    {
        var firstEntryInGroup = answersSummaryQuestionPartModelDatasFlattened[0];

        var responses = answersSummaryQuestionPartModelDatasFlattened
            .SelectMany(x => x.DataShareRequestAnswersSummaryQuestionPart_Responses).ToList();

        foreach (var response in responses)
        {
            var responseItem = await DoGetAnswerResponseItemAsync(
                dbConnection,
                dbTransaction,
                firstEntryInGroup.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType,
                response.DataShareRequestAnswersSummaryQuestionPartResponse_ResponseId);

            response.DataShareRequestAnswersSummaryQuestionPart_ResponseItem = responseItem;
        }

        // Note: There should only ever be exactly one response item per response.  However, in early development there was an issue whereby
        // selection option responses were being reported incorrectly, and if N options were selected then N responses were received, with
        // each containing the full N selected options.
        // Therefore, to cater for some of the bad data that may remain from those early stages, a list is returned but only
        // the first response is actually used

        if (!firstEntryInGroup.DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesAllowed &&
            responses.Count > 1)
        {
            responses = responses.Take(1).ToList();
        }
        firstEntryInGroup.DataShareRequestAnswersSummaryQuestionPart_Responses = responses;

        return firstEntryInGroup;
    }

    private async Task<DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData?> DoGetAnswerResponseItemAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        QuestionPartResponseInputType responseInputType,
        Guid answerPartResponseId)
    {
        return responseInputType switch
        {
            QuestionPartResponseInputType.FreeForm => await DoGetAnswerResponseFreeFormItemAsync(dbConnection, dbTransaction, answerPartResponseId),
            QuestionPartResponseInputType.OptionSelection => await DoGetAnswerResponseOptionSelectionItemAsync(dbConnection, dbTransaction, answerPartResponseId),
            QuestionPartResponseInputType.None => throw new InvalidEnumValueException("Cannot get answer response for No Input ResponseInputType"),
            _ => throw new InvalidEnumValueException("Unknown ResponseInputType"),
        };
    }

    private async Task<DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData?> DoGetAnswerResponseFreeFormItemAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid answerPartResponseId)
    {
        // There should only ever be at most one free form response item per response
        return await databaseCommandRunner.DbQuerySingleOrDefaultAsync<DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetAnswerResponseFreeFormItems,
            new
            {
                AnswerPartResponseId = answerPartResponseId
            }).ConfigureAwait(false);
    }

    private async Task<DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData?> DoGetAnswerResponseOptionSelectionItemAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid answerPartResponseId)
    {
        var optionSelectionResponseItemsFlattened = await databaseCommandRunner.DbQueryAsync<
            DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData,
            DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData?,
            DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetAnswerResponseOptionSelectionItems,
            (responseItem, optionSelection) =>
            {
                if (optionSelection != null)
                {
                    responseItem.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelection_SelectedOptions.Add(optionSelection);
                }

                return responseItem;
            },
            nameof(DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerItemOptionSelectionItem_ItemId),
            new
            {
                AnswerPartResponseId = answerPartResponseId
            }).ConfigureAwait(false);

        return BuildGroupedData();

        DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData? BuildGroupedData()
        {
            var firstEntryInGroup = optionSelectionResponseItemsFlattened.FirstOrDefault();
            if (firstEntryInGroup == null) return firstEntryInGroup;

            firstEntryInGroup.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelection_SelectedOptions =
                optionSelectionResponseItemsFlattened.SelectMany(x =>
                    x.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelection_SelectedOptions).ToList();

            return firstEntryInGroup;
        }
    }
    #endregion

    #region Cancel Data share Request
    async Task IAcquirerDataShareRequestRepository.CancelDataShareRequestAsync(
        IUserIdSet acquirerUserIdSet,
        Guid dataShareRequestId,
        string reasonsForCancellation)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!;

            var dataShareRequestPreviousStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            await DoCancelDataShareRequestAsync(dbConnection, dbTransaction);

            await DoRecordDataShareRequestStatusChangeWithCommentsInAuditLogAsync(dbConnection, dbTransaction,
                dataShareRequestId, acquirerUserIdSet, dataShareRequestPreviousStatus.DataShareRequestStatus_RequestStatus, [reasonsForCancellation]);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to CancelDataShareRequest in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }

        async Task DoCancelDataShareRequestAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            try
            {
                await databaseCommandRunner.DbExecuteScalarAsync(
                    dbConnection,
                    dbTransaction,
                    acquirerDataShareRequestSqlQueries.CancelDataShareRequest,
                    new
                    {
                        DataShareRequestId = dataShareRequestId
                    });
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to CancelDataShareRequest as failed to set cancelled in database";

                logger.LogError(ex, errorMessage);

                throw new DatabaseAccessGeneralException(errorMessage, ex);
            }
        }
    }
    #endregion

    #region Delete Data Share Request
    async Task IAcquirerDataShareRequestRepository.DeleteDataShareRequestAsync(
        IUserIdSet acquirerUserIdSet,
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dbConnection = databaseChannel.Connection;
            var dbTransaction = databaseChannel.Transaction!;

            var dataShareRequestPreviousStatusModelData = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction, dataShareRequestId);

            await DoDeleteDataShareRequestAsync(dbConnection, dbTransaction);

            await DoRecordDataShareRequestStatusChangeInAuditLogAsync(dbConnection, dbTransaction,
                dataShareRequestId, acquirerUserIdSet, dataShareRequestPreviousStatusModelData.DataShareRequestStatus_RequestStatus);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to DeleteDataShareRequest in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }

        async Task DoDeleteDataShareRequestAsync(
            IDbConnection dbConnection,
            IDbTransaction dbTransaction)
        {
            try
            {
                await databaseCommandRunner.DbExecuteScalarAsync(
                    dbConnection,
                    dbTransaction,
                    acquirerDataShareRequestSqlQueries.DeleteDataShareRequest,
                    new
                    {
                        DataShareRequestId = dataShareRequestId
                    });
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to DeleteDataShareRequest as failed to set deleted in database";

                logger.LogError(ex, errorMessage);

                throw new DatabaseAccessGeneralException(errorMessage, ex);
            }
        }
    }
    #endregion

    #region Get Whether Questions Remain That Require A Response
    async Task<bool> IAcquirerDataShareRequestRepository.GetWhetherQuestionsRemainThatRequireAResponseAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await databaseCommandRunner.DbQuerySingleAsync<bool>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                acquirerDataShareRequestSqlQueries.GetWhetherQuestionsRemainThatRequireAResponse,
                new
                {
                    DataShareRequestId = dataShareRequestId
                });
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetWhetherQuestionsRemainThatRequireAResponse from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region Check Whether A Data Share Request Exists
    async Task<bool> IAcquirerDataShareRequestRepository.CheckIfDataShareRequestExistsAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dataShareRequestExists =  await databaseCommandRunner.DbQuerySingleAsync<int>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                acquirerDataShareRequestSqlQueries.CheckIfDataShareRequestExists,
                new
                {
                    DataShareRequestId = dataShareRequestId
                });

            return dataShareRequestExists == 1;
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to CheckIfDataShareRequestExistAsync in database";

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
    async Task<DataShareRequestNotificationInformationModelData> IAcquirerDataShareRequestRepository.GetDataShareRequestNotificationInformationAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await databaseCommandRunner.DbQuerySingleAsync<DataShareRequestNotificationInformationModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                acquirerDataShareRequestSqlQueries.GetDataShareRequestNotificationInformation,
                new
                {
                    DataShareRequestId = dataShareRequestId
                });
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetDataShareRequestNotificationInformation from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
    #endregion

    #region Build Question Part Response Format
    private async Task<QuestionPartResponseFormatModelData> DoBuildQuestionPartResponseFormatAsync(
        QuestionPartModelData questionPartModelData,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        var responseInputType = questionPartModelData.QuestionPart_ResponseTypeInformation.QuestionPartResponseTypeInformation_InputType;

        return responseInputType switch
        {
            QuestionPartResponseInputType.FreeForm => await DoBuildQuestionPartFreeFormResponseFormatAsync(questionPartModelData, dbConnection, dbTransaction),
            QuestionPartResponseInputType.OptionSelection => await DoBuildQuestionPartOptionSelectionResponseFormatAsync(questionPartModelData, dbConnection, dbTransaction),
            QuestionPartResponseInputType.None => await DoBuildQuestionPartNoInputResponseFormatAsync(questionPartModelData),

            _ => throw new InvalidEnumValueException("QuestionPart has unknown QuestionPartResponseInputType")
        };
    }

    private async Task<QuestionPartResponseFormatFreeFormModelData> DoBuildQuestionPartFreeFormResponseFormatAsync(
        QuestionPartModelData questionPartModelData,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        var responseFormatType = questionPartModelData.QuestionPart_ResponseTypeInformation.QuestionPartResponseTypeInformation_FormatType;

        QuestionPartResponseFormatFreeFormModelData questionPartResponseFormat = responseFormatType switch
        {
            QuestionPartResponseFormatType.Text => await BuildQuestionPartResponseFormatFreeFormTextForResponseFormatAsync(),
            QuestionPartResponseFormatType.Numeric => await BuildQuestionPartFreeFormResponseFormatForResponseFormatAsync<QuestionPartResponseFormatFreeFormNumericModelData>(),
            QuestionPartResponseFormatType.Date => await BuildQuestionPartFreeFormResponseFormatForResponseFormatAsync<QuestionPartResponseFormatFreeFormDateModelData>(),
            QuestionPartResponseFormatType.Time => await BuildQuestionPartFreeFormResponseFormatForResponseFormatAsync<QuestionPartResponseFormatFreeFormTimeModelData>(),
            QuestionPartResponseFormatType.DateTime => await BuildQuestionPartFreeFormResponseFormatForResponseFormatAsync<QuestionPartResponseFormatFreeFormDateTimeModelData>(),
            QuestionPartResponseFormatType.Country => await BuildQuestionPartFreeFormResponseFormatForResponseFormatAsync<QuestionPartResponseFormatFreeFormCountryModelData>(),
            _ => throw new InvalidEnumValueException("QuestionPart with FreeForm response Input type has expected Format type")
        };

        return questionPartResponseFormat;

        async Task<QuestionPartResponseFormatFreeFormModelData> BuildQuestionPartResponseFormatFreeFormTextForResponseFormatAsync()
        {
            var questionPartResponseFormatFreeFormText = await BuildQuestionPartFreeFormResponseFormatForResponseFormatAsync<QuestionPartResponseFormatFreeFormTextModelData>();

            questionPartResponseFormatFreeFormText.QuestionPartResponseFormatFreeFormText_MaximumResponseLength =
                DoDetermineMaximumResponseLength();

            return questionPartResponseFormatFreeFormText;

            int DoDetermineMaximumResponseLength()
            {
                if (questionPartModelData.QuestionPart_MultipleAnswerItemControl.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed)
                {
                    return inputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormMultiResponseTextResponse();
                }

                var questionPartType = questionPartModelData.QuestionPart_QuestionPartType;

                return questionPartType switch
                {
                    QuestionPartType.MainQuestionPart => inputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormTextResponse(),

                    QuestionPartType.SupplementaryQuestionPart => inputConstraintConfigurationPresenter.GetMaximumLengthOfSupplementaryTextResponse(),

                    _ => throw new InvalidEnumValueException($"Unable to determine maximum response length for question part of unknown type: '{questionPartType}'")
                };
            }
        }

        async Task<T> BuildQuestionPartFreeFormResponseFormatForResponseFormatAsync<T>() where T : QuestionPartResponseFormatFreeFormModelData, new()
        {
            var freeFormOptionModelData = await databaseCommandRunner.DbQuerySingleAsync<QuestionPartResponseFormatFreeFormOptionsModelData>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetFreeFormOptionsModelData,
                new
                {
                    QuestionPartId = questionPartModelData.QuestionPart_Id,
                }).ConfigureAwait(false);

            var questionPartFreeFormResponseFormatForResponseFormat = new T
            {
                FreeFormOptions = freeFormOptionModelData
            };
            return questionPartFreeFormResponseFormatForResponseFormat;
        }
    }

    private async Task<QuestionPartResponseFormatOptionSelectModelData> DoBuildQuestionPartOptionSelectionResponseFormatAsync(
        QuestionPartModelData questionPartModelData,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        var responseFormatType = questionPartModelData.QuestionPart_ResponseTypeInformation.QuestionPartResponseTypeInformation_FormatType;

        QuestionPartResponseFormatOptionSelectModelData questionPartResponseFormat = responseFormatType switch
        {
            QuestionPartResponseFormatType.SelectSingle => await DoBuildQuestionPartOptionSelectionSingleValueResponseFormatAsync(questionPartModelData, dbConnection, dbTransaction),
            QuestionPartResponseFormatType.SelectMulti => await DoBuildQuestionPartOptionSelectionMultiValueResponseFormatAsync(questionPartModelData, dbConnection, dbTransaction),
            _ => throw new InvalidEnumValueException("QuestionPart with OptionSelection response Input type has expected Format type")
        };

        var optionSelectionItems = questionPartResponseFormat.GetResponseFormatOptionSelectOptionSelectionItems();

        foreach (var optionSelectionItem in optionSelectionItems)
        {
            optionSelectionItem.OptionSelectionItem_SupplementaryQuestionPart =
                await DoBuildOptionSelectionSupplementaryQuestionPartAsync(optionSelectionItem.OptionSelectionItem_SupplementaryQuestionPartId, dbConnection, dbTransaction);
        }

        return questionPartResponseFormat;
    }

    private async Task<QuestionPartModelData?> DoBuildOptionSelectionSupplementaryQuestionPartAsync(
        Guid? supplementaryQuestionPartId,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        if (supplementaryQuestionPartId == null) return await Task.FromResult((QuestionPartModelData?)null);

        var questionPartModelData = await databaseCommandRunner.DbQueryAsync<
            QuestionPartModelData,
            QuestionPartPromptsModelData,
            QuestionPartMultipleAnswerItemControlModelData,
            QuestionPartResponseTypeInformationModelData,
            QuestionPartModelData>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetSupplementaryQuestionPartModelData,
            (questionPart, prompts, multipleAnswerItemControl, responseTypeInformation) =>
            {
                questionPart.QuestionPart_Prompts = prompts;
                questionPart.QuestionPart_MultipleAnswerItemControl = multipleAnswerItemControl;
                questionPart.QuestionPart_ResponseTypeInformation = responseTypeInformation;

                return questionPart;
            },
            $"{nameof(QuestionPartPromptsModelData.QuestionPartPrompt_QuestionPartId)}, " +
            $"{nameof(QuestionPartMultipleAnswerItemControlModelData.QuestionPartMultipleAnswerItemControl_QuestionPartId)}, " +
            $"{nameof(QuestionPartResponseTypeInformationModelData.QuestionPartResponseTypeInformation_QuestionPartId)}",
            new
            {
                QuestionPartId = supplementaryQuestionPartId
            }).ConfigureAwait(false);

        var firstAndOnlyRecordInGroup = questionPartModelData.Single();

        firstAndOnlyRecordInGroup.QuestionPart_ResponseFormat = await DoBuildQuestionPartResponseFormatAsync(firstAndOnlyRecordInGroup, dbConnection, dbTransaction);

        return firstAndOnlyRecordInGroup;
    }

    private async Task<QuestionPartResponseFormatOptionSelectSingleValueModelData> DoBuildQuestionPartOptionSelectionSingleValueResponseFormatAsync(
        QuestionPartModelData questionPartModelData,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        var singleSelectionOptionItemModelDatas =
            await databaseCommandRunner.DbQueryAsync<QuestionPartOptionSelectionItemForSingleSelectionModelData>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetSelectionOptionSingleValueModelData,
                new
                {
                    QuestionPartId = questionPartModelData.QuestionPart_Id
                }).ConfigureAwait(false);

        return new QuestionPartResponseFormatOptionSelectSingleValueModelData
        {
            ResponseFormatOptionSelectSingleValue_SingleSelectionOptions = [.. singleSelectionOptionItemModelDatas]
        };
    }

    private async Task<QuestionPartResponseFormatOptionSelectMultiValueModelData> DoBuildQuestionPartOptionSelectionMultiValueResponseFormatAsync(
        QuestionPartModelData questionPartModelData,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        var multiSelectionOptionItemModelDatas = await databaseCommandRunner.DbQueryAsync<QuestionPartOptionSelectionItemForMultiSelectionModelData>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetSelectionOptionMultiValueModelData,
            new
            {
                QuestionPartId = questionPartModelData.QuestionPart_Id
            }).ConfigureAwait(false);

        return new QuestionPartResponseFormatOptionSelectMultiValueModelData
        {
            ResponseFormatOptionSelectMultiValue_MultiSelectionOptions = [.. multiSelectionOptionItemModelDatas]
        };
    }

    private static async Task<QuestionPartResponseFormatNoInputModelData> DoBuildQuestionPartNoInputResponseFormatAsync(
        QuestionPartModelData questionPartModelData)
    {
        var responseFormatType = questionPartModelData.QuestionPart_ResponseTypeInformation.QuestionPartResponseTypeInformation_FormatType;

        QuestionPartResponseFormatNoInputModelData questionPartResponseFormat = responseFormatType switch
        {
            QuestionPartResponseFormatType.ReadOnly => new QuestionPartResponseFormatReadOnlyModelData(),
            _ => throw new InvalidEnumValueException("QuestionPart with NoInput response Input type has expected Format type")
        };

        return await Task.FromResult(questionPartResponseFormat);
    }
    #endregion

    #region Question Part Answer
    private async Task<QuestionPartAnswerResponseModelData> DoBuildQuestionAnswerPartResponseAsync(
        QuestionPartAnswerResponseInformationModelData answerPartResponseInformation,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        var answerResponseItemId = await databaseCommandRunner.DbQuerySingleOrDefaultAsync<Guid?>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetAnswerPartResponseItemId,
            new
            {
                AnswerPartResponseId = answerPartResponseInformation.QuestionPartAnswerResponse_Id
            });

        var answerResponseItem = await DoBuildQuestionAnswerPartResponseItemAsync();

        return new QuestionPartAnswerResponseModelData
        {
            QuestionPartAnswerResponse_Id = answerPartResponseInformation.QuestionPartAnswerResponse_Id,
            QuestionPartAnswerResponse_OrderWithinAnswerPart = answerPartResponseInformation.QuestionPartAnswerItem_OrderWithinAnswerPart,
            QuestionPartAnswerResponse_InputType = answerPartResponseInformation.QuestionPartAnswerItem_InputType,
            QuestionPartAnswerResponse_ResponseItem = answerResponseItem
        };

        async Task<QuestionPartAnswerResponseItemModelData?> DoBuildQuestionAnswerPartResponseItemAsync()
        {
            if (answerResponseItemId == null) return null;

            return answerPartResponseInformation.QuestionPartAnswerItem_InputType == QuestionPartResponseInputType.FreeForm
                ? await DoBuildQuestionAnswerPartResponseFreeFormModel(answerResponseItemId.Value, dbConnection, dbTransaction)
                : await DoBuildQuestionAnswerPartResponseOptionSelection(answerResponseItemId.Value, dbConnection, dbTransaction);
        }
    }

    private async Task<QuestionPartAnswerResponseItemFreeFormModelData> DoBuildQuestionAnswerPartResponseFreeFormModel(
        Guid answerResponseItemId,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        return await databaseCommandRunner.DbQuerySingleAsync<QuestionPartAnswerResponseItemFreeFormModelData>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetQuestionPartAnswerResponseItemFreeFormModelData,
            new
            {
                AnswerPartResponseItemId = answerResponseItemId
            }).ConfigureAwait(false);
    }

    private async Task<QuestionPartAnswerResponseItemOptionSelectionModelData> DoBuildQuestionAnswerPartResponseOptionSelection(
        Guid answerResponseItemId,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        var questionPartAnswerItemOptionSelectionModelDataFlattened = (
            await databaseCommandRunner.DbQueryAsync<
                QuestionPartAnswerResponseItemOptionSelectionModelData,
                QuestionPartAnswerItemSelectionOptionItemModelData,
                QuestionPartAnswerResponseItemOptionSelectionModelData>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetQuestionPartAnswerResponseItemOptionSelectionModelData,
                (answerItem, selectedOption) =>
                {
                    answerItem.QuestionPartAnswerItem_SelectedOptionItems.Add(selectedOption);

                    return answerItem;
                },
                nameof(QuestionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_OptionSelectionItemId),
                new
                {
                    AnswerPartResponseItemId = answerResponseItemId
                }).ConfigureAwait(false)).ToList();

        var firstRecordInGroup = questionPartAnswerItemOptionSelectionModelDataFlattened[0];

        var allSelectedOptionItems = questionPartAnswerItemOptionSelectionModelDataFlattened.SelectMany(x => x.QuestionPartAnswerItem_SelectedOptionItems).ToList();

        foreach (var selectedOptionItem in allSelectedOptionItems)
        {
            selectedOptionItem.QuestionPartAnswerItem_SupplementaryQuestionPartAnswer =
                await DoBuildSupplementaryQuestionAnswerPartAnswer(selectedOptionItem.QuestionPartAnswerItem_SupplementaryQuestionPartAnswerId, dbConnection, dbTransaction);
        }

        firstRecordInGroup.QuestionPartAnswerItem_SelectedOptionItems = allSelectedOptionItems;

        return firstRecordInGroup;
    }

    private async Task<QuestionPartAnswerModelData?> DoBuildSupplementaryQuestionAnswerPartAnswer(
        Guid? supplementaryQuestionPartAnswerId,
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        if (supplementaryQuestionPartAnswerId == null) return null;

        var answerPartsFlattened = (await databaseCommandRunner.DbQueryAsync<
            QuestionPartAnswerModelData,
            QuestionPartAnswerResponseInformationModelData,
            QuestionPartAnswerModelData>(
            dbConnection,
            dbTransaction,
            acquirerDataShareRequestSqlQueries.GetSupplementaryQuestionAnswerPartAnswerModelData,
            (answer, answerItem) =>
            { 
                answer.QuestionPartAnswer_AnswerPartResponseInformations.Add(answerItem);

                return answer;
            },
            nameof(QuestionPartAnswerResponseInformationModelData.QuestionPartAnswerResponse_Id),
            new
            {
                SupplementaryQuestionPartAnswerId = supplementaryQuestionPartAnswerId
            }).ConfigureAwait(false)).ToList();

        var firstRecordInGroup = answerPartsFlattened[0];

        var allAnswerPartResponseInformations = answerPartsFlattened.SelectMany(x => x.QuestionPartAnswer_AnswerPartResponseInformations);
        firstRecordInGroup.QuestionPartAnswer_AnswerPartResponseInformations = [.. allAnswerPartResponseInformations];

        firstRecordInGroup.QuestionPartAnswer_AnswerPartResponses = firstRecordInGroup.QuestionPartAnswer_AnswerPartResponseInformations
            .Select(async answerPartResponseInformation => await DoBuildQuestionAnswerPartResponseAsync(answerPartResponseInformation, dbConnection, dbTransaction))
            .Select(x => x.Result).ToList();

        return firstRecordInGroup;
    }
    #endregion

    #region Helpers
    private async Task<DataShareRequestStatusTypeModelData> DoGetDataShareRequestStatusAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid dataShareRequestId)
    {
        try
        {
            return await databaseCommandRunner.DbQuerySingleAsync<DataShareRequestStatusTypeModelData>(
                dbConnection,
                dbTransaction,
                acquirerDataShareRequestSqlQueries.GetDataShareRequestStatus,
                new
                {
                    DataShareRequestId = dataShareRequestId
                }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetDataShareRequestStatus from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
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
            var dataShareRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction,
                dataShareRequestId);

            var recordDataShareRequestStatusChangeParameters = new RecordDataShareRequestStatusChangeParameters
            {
                DbConnection = dbConnection,
                DbTransaction = dbTransaction,
                DataShareRequestId = dataShareRequestId,
                FromStatus = previousStatus,
                ToStatus = dataShareRequestStatus.DataShareRequestStatus_RequestStatus,
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
            var dataShareRequestStatus = await DoGetDataShareRequestStatusAsync(dbConnection, dbTransaction,
                dataShareRequestId);

            var recordDataShareRequestStatusChangeWithCommentsParameters = new RecordDataShareRequestStatusChangeWithCommentsParameters
            {
                DbConnection = dbConnection,
                DbTransaction = dbTransaction,
                DataShareRequestId = dataShareRequestId,
                FromStatus = previousStatus,
                ToStatus = dataShareRequestStatus.DataShareRequestStatus_RequestStatus,
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