using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;

namespace Agrimetrics.DataShare.Api.Logic.Services.AuditLog;

internal class AuditLogService(
    IAuditLogModelDataFactory auditLogModelDataFactory,
    IAuditLogRepository auditLogRepository,
    IServiceOperationResultFactory serviceOperationResultFactory) : IAuditLogService
{
    async Task<IServiceOperationDataResult<IAuditLogDataShareRequestStatusChangesResult>> IAuditLogService.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
        Guid dataShareRequestId,
        IEnumerable<DataShareRequestStatus> toStatuses)
    {
        var dataShareRequestStatusTypes = toStatuses.Select(
            auditLogModelDataFactory.ConvertDataShareRequestStatusToDataShareRequestStatusType);

        var auditLogsForDataShareRequestStatusChangesToStatus = await DoGetAuditLogsForDataShareRequestStatusChangeToStatusAsync(dataShareRequestId, dataShareRequestStatusTypes);

        var auditLogDataShareRequestStatusChangesResult = auditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            dataShareRequestId, 
            auditLogsForDataShareRequestStatusChangesToStatus);

        return await Task.Run(() => serviceOperationResultFactory.CreateSuccessfulDataResult(auditLogDataShareRequestStatusChangesResult));
    }

    async Task<IEnumerable<AuditLogDataShareRequestStatusChangeModelData?>> IAuditLogService.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
        Guid dataShareRequestId,
        IEnumerable<DataShareRequestStatusType> toStatuses)
    {
        return await DoGetAuditLogsForDataShareRequestStatusChangeToStatusAsync(dataShareRequestId, toStatuses);
    }

    private async Task<IEnumerable<AuditLogDataShareRequestStatusChangeModelData>> DoGetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
        Guid dataShareRequestId,
        IEnumerable<DataShareRequestStatusType> toStatuses)
    {
        var auditLogsForDataShareRequestStatusChanges = await auditLogRepository.GetAuditLogsForDataShareRequestStatusChangesSetAsync(
            dataShareRequestId,
            fromStatuses: null,
            toStatuses: toStatuses);

        return auditLogsForDataShareRequestStatusChanges
            .OrderBy(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc);
    }

    async Task<AuditLogDataShareRequestStatusChangeModelData?> IAuditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeAsync(
        Guid dataShareRequestId)
    {
        var auditLogsForDataShareRequestStatusChanges = await auditLogRepository.GetAuditLogsForDataShareRequestStatusChangesAsync(
            dataShareRequestId,
            fromStatus: null,
            toStatus: null);

        return auditLogsForDataShareRequestStatusChanges
            .MaxBy(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc);
    }

    async Task<AuditLogDataShareRequestStatusChangeModelData?> IAuditLogService.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
        Guid dataShareRequestId,
        DataShareRequestStatusType? toStatus)
    {
        var auditLogsForDataShareRequestStatusChanges = await auditLogRepository.GetAuditLogsForDataShareRequestStatusChangesAsync(
            dataShareRequestId,
            fromStatus: null,
            toStatus: toStatus);

        return auditLogsForDataShareRequestStatusChanges
            .MaxBy(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc);
    }
}