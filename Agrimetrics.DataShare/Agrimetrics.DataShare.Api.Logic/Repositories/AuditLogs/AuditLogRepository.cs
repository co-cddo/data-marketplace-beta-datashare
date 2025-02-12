using System.ComponentModel;
using System.Data;
using System.Data.Common;
using Agrimetrics.DataShare.Api.Core.Utilities;
using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs.ParameterModels;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;

internal class AuditLogRepository(
    ILogger<AuditLogRepository> logger,
    IDatabaseChannelCreation databaseChannelCreation,
    IDatabaseCommandRunner databaseCommandRunner,
    IAuditLogSqlQueries auditLogSqlQueries) : IAuditLogRepository
{
    #region Data Share Request Status Changes
    #region Recording Status Changes
    async Task<Guid> IAuditLogRepository.RecordDataShareRequestStatusChangeAsync(
        IRecordDataShareRequestStatusChangeParameters recordDataShareRequestStatusChangeParameters)
    {
        if (!Enum.IsDefined(typeof(DataShareRequestStatusType), recordDataShareRequestStatusChangeParameters.FromStatus))
            throw new InvalidEnumArgumentException(nameof(recordDataShareRequestStatusChangeParameters.FromStatus), (int)recordDataShareRequestStatusChangeParameters.FromStatus, typeof(DataShareRequestStatusType));

        if (!Enum.IsDefined(typeof(DataShareRequestStatusType), recordDataShareRequestStatusChangeParameters.ToStatus))
            throw new InvalidEnumArgumentException(nameof(recordDataShareRequestStatusChangeParameters.ToStatus), (int)recordDataShareRequestStatusChangeParameters.ToStatus, typeof(DataShareRequestStatusType));

        try
        {
            var doRecordDataShareRequestStatusChangeWithCommentsParams = new DoRecordDataShareRequestStatusChangeWithCommentsParams
            {
                DbConnection = recordDataShareRequestStatusChangeParameters.DbConnection,
                DbTransaction = recordDataShareRequestStatusChangeParameters.DbTransaction,
                DataShareRequestId = recordDataShareRequestStatusChangeParameters.DataShareRequestId,
                FromStatus = recordDataShareRequestStatusChangeParameters.FromStatus,
                ToStatus = recordDataShareRequestStatusChangeParameters.ToStatus,
                ChangedByUser = recordDataShareRequestStatusChangeParameters.ChangedByUser,
                ChangedAtLocalTime = recordDataShareRequestStatusChangeParameters.ChangedAtLocalTime,
                Comments = []
            };

            return await DoRecordDataShareRequestStatusChangeWithCommentsAsync(doRecordDataShareRequestStatusChangeWithCommentsParams);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to RecordDataShareRequestStatusChange in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
    }

    async Task<Guid> IAuditLogRepository.RecordDataShareRequestStatusChangeWithCommentsAsync(
        IRecordDataShareRequestStatusChangeWithCommentsParameters recordDataShareRequestStatusChangeWithCommentsParameters)
    {
        if (!Enum.IsDefined(typeof(DataShareRequestStatusType), recordDataShareRequestStatusChangeWithCommentsParameters.FromStatus))
            throw new InvalidEnumArgumentException(nameof(recordDataShareRequestStatusChangeWithCommentsParameters.FromStatus), (int)recordDataShareRequestStatusChangeWithCommentsParameters.FromStatus, typeof(DataShareRequestStatusType));

        if (!Enum.IsDefined(typeof(DataShareRequestStatusType), recordDataShareRequestStatusChangeWithCommentsParameters.ToStatus))
            throw new InvalidEnumArgumentException(nameof(recordDataShareRequestStatusChangeWithCommentsParameters.ToStatus), (int)recordDataShareRequestStatusChangeWithCommentsParameters.ToStatus, typeof(DataShareRequestStatusType));

        if (recordDataShareRequestStatusChangeWithCommentsParameters.Comments == null)
            throw new ArgumentNullException(nameof(recordDataShareRequestStatusChangeWithCommentsParameters.Comments));

        try
        {
            var doRecordDataShareRequestStatusChangeWithCommentsParams = new DoRecordDataShareRequestStatusChangeWithCommentsParams
            {
                DbConnection = recordDataShareRequestStatusChangeWithCommentsParameters.DbConnection,
                DbTransaction = recordDataShareRequestStatusChangeWithCommentsParameters.DbTransaction,
                DataShareRequestId = recordDataShareRequestStatusChangeWithCommentsParameters.DataShareRequestId,
                FromStatus = recordDataShareRequestStatusChangeWithCommentsParameters.FromStatus,
                ToStatus = recordDataShareRequestStatusChangeWithCommentsParameters.ToStatus,
                ChangedByUser = recordDataShareRequestStatusChangeWithCommentsParameters.ChangedByUser,
                ChangedAtLocalTime = recordDataShareRequestStatusChangeWithCommentsParameters.ChangedAtLocalTime,
                Comments = recordDataShareRequestStatusChangeWithCommentsParameters.Comments
            };

            return await DoRecordDataShareRequestStatusChangeWithCommentsAsync(doRecordDataShareRequestStatusChangeWithCommentsParams);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to RecordDataShareRequestStatusChangeWithComments in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
    }

    // Providing parameters as a structure is clearer than a long list of individual parameters (sonarcloud rule csharpsquid:S107)
    private sealed class DoRecordDataShareRequestStatusChangeWithCommentsParams
    {
        public required IDbConnection DbConnection { get; init; }
        public required IDbTransaction DbTransaction { get; init; }
        public required Guid DataShareRequestId { get; init; }
        public required DataShareRequestStatusType FromStatus { get; init; }
        public required DataShareRequestStatusType ToStatus { get; init; }
        public required IUserIdSet ChangedByUser { get; init; }
        public required DateTime ChangedAtLocalTime { get; init; }
        public required IEnumerable<string> Comments { get; init; }
    }

    private async Task<Guid> DoRecordDataShareRequestStatusChangeWithCommentsAsync(
        DoRecordDataShareRequestStatusChangeWithCommentsParams parameters)
    {
        var dbConnection = parameters.DbConnection;
        var dbTransaction = parameters.DbTransaction;
        var dataShareRequestId = parameters.DataShareRequestId;
        var fromStatus = parameters.FromStatus;
        var toStatus = parameters.ToStatus;
        var changedByUser = parameters.ChangedByUser;
        var changedAtLocalTime = parameters.ChangedAtLocalTime;
        var comments = parameters.Comments;

        try
        {
            var statusChangeLogEntryId = await RecordStatusChangeAsync();

            foreach (var indexedComment in comments.Select((comment, index) => new { Comment = comment, Index = index }))
            {
                await RecordStatusChangeCommentAsync(
                    statusChangeLogEntryId, indexedComment.Comment, indexedComment.Index + 1);
            }

            return statusChangeLogEntryId;
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to RecordStatusChange in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        

        async Task<Guid> RecordStatusChangeAsync()
        {
            return await databaseCommandRunner.DbExecuteScalarAsync<Guid>(
                dbConnection,
                dbTransaction,
                auditLogSqlQueries.RecordDataShareRequestStatusChange,
                new
                {
                    DataShareRequestId = dataShareRequestId,
                    FromStatus = fromStatus.ToString(),
                    ToStatus = toStatus.ToString(),
                    ChangedByUserId = changedByUser.UserId,
                    ChangedByUserDomainId = changedByUser.DomainId,
                    ChangedByUserOrganisationId = changedByUser.OrganisationId,
                    ChangedAtUtc = changedAtLocalTime.ProvisionApiTimestampToDatabaseTimestamp()
                }).ConfigureAwait(false);
        }

        async Task RecordStatusChangeCommentAsync(
            Guid statusChangeLogEntryId,
            string comment,
            int commentOrder)
        {
            try
            {
                await databaseCommandRunner.DbExecuteScalarAsync(
                    dbConnection,
                    dbTransaction,
                    auditLogSqlQueries.RecordDataShareRequestStatusChangeComment,
                    new
                    {
                        StatusChangeId = statusChangeLogEntryId,
                        Comment = comment,
                        CommentOrder = commentOrder
                    }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to RecordStatusChangeComment in database";

                logger.LogError(ex, errorMessage);

                throw new DatabaseAccessGeneralException(errorMessage, ex);
            }
        }
    }
    #endregion

    #region Reading Status Changes
    async Task<IEnumerable<AuditLogDataShareRequestStatusChangeModelData>> IAuditLogRepository.GetAuditLogsForDataShareRequestStatusChangesAsync(
        Guid dataShareRequestId,
        DataShareRequestStatusType? fromStatus,
        DataShareRequestStatusType? toStatus)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await DoGetAuditLogsForDataShareRequestStatusChangesAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                dataShareRequestId,
                fromStatus,
                toStatus);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetAuditLogsForDataShareRequestStatusChanges in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    async Task<IEnumerable<AuditLogDataShareRequestStatusChangeModelData>> IAuditLogRepository.GetAuditLogsForDataShareRequestStatusChangesSetAsync(
        Guid dataShareRequestId,
        IEnumerable<DataShareRequestStatusType>? fromStatuses,
        IEnumerable<DataShareRequestStatusType>? toStatuses)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await DoGetAuditLogsForDataShareRequestStatusChangesAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                dataShareRequestId,
                fromStatuses,
                toStatuses);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetAuditLogsForDataShareRequestStatusChanges in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    private async Task<IEnumerable<AuditLogDataShareRequestStatusChangeModelData>> DoGetAuditLogsForDataShareRequestStatusChangesAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid dataShareRequestId,
        DataShareRequestStatusType? fromStatus,
        DataShareRequestStatusType? toStatus)
    {
        return await DoGetAuditLogsForDataShareRequestStatusChangesAsync(
            dbConnection,
            dbTransaction,
            dataShareRequestId,
            fromStatus.HasValue ? new List<DataShareRequestStatusType>{ fromStatus.Value} : null,
            toStatus.HasValue ? new List<DataShareRequestStatusType> { toStatus.Value } : null);
    }

    private async Task<IEnumerable<AuditLogDataShareRequestStatusChangeModelData>> DoGetAuditLogsForDataShareRequestStatusChangesAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        Guid dataShareRequestId,
        IEnumerable<DataShareRequestStatusType>? fromStatuses,
        IEnumerable<DataShareRequestStatusType>? toStatuses)
    {
        var fromStatusQuerySet = BuildStatusQuerySet(fromStatuses).ToList();
        var toStatusQuerySet = BuildStatusQuerySet(toStatuses).ToList();

        var auditLogDataShareRequestStatusChangeModelDatasFlattened = (await databaseCommandRunner.DbQueryAsync<
            AuditLogDataShareRequestStatusChangeModelData,
            AuditLogDataShareRequestStatusChangeCommentModelData,
            AuditLogDataShareRequestStatusChangeModelData>(
            dbConnection,
            dbTransaction,
            auditLogSqlQueries.GetDataShareRequestStatusChanges,
            (statusChange, comment) =>
            {
                statusChange.AuditLogDataShareRequestStatusChange_Comments.Add(comment);
                return statusChange;
            },
            nameof(AuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Id),
            new
            {
                DataShareRequestId = dataShareRequestId,
                HasFromStatuses = fromStatusQuerySet.Any(),
                FromStatuses = fromStatusQuerySet,
                HasToStatuses = toStatusQuerySet.Any(),
                ToStatuses = toStatusQuerySet
            }).ConfigureAwait(false)).ToList();

        return BuildGroupedData();

        IEnumerable<string> BuildStatusQuerySet(IEnumerable<DataShareRequestStatusType>? statuses)
        {
            return statuses == null
                ? Enumerable.Empty<string>()
                : statuses.Select(x => x.ToString());
        }

        IEnumerable<AuditLogDataShareRequestStatusChangeModelData> BuildGroupedData()
        {
            var auditLogDataShareRequestStatusChangesGroupedByChangeId = auditLogDataShareRequestStatusChangeModelDatasFlattened
                .GroupBy(x => x.AuditLogDataShareRequestStatusChange_Id).ToList();

            foreach (var auditLogDataShareRequestStatusChangesInGroup in auditLogDataShareRequestStatusChangesGroupedByChangeId)
            {
                var firstRecordInGroup = auditLogDataShareRequestStatusChangesInGroup.First();

                firstRecordInGroup.AuditLogDataShareRequestStatusChange_Comments = auditLogDataShareRequestStatusChangesInGroup
                    .SelectMany(x => x.AuditLogDataShareRequestStatusChange_Comments).ToList();

                yield return firstRecordInGroup;
            }
        }
    }
    #endregion
    #endregion
}