using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs.ParameterModels;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs;

public interface IAuditLogRepository
{
    Task<Guid> RecordDataShareRequestStatusChangeAsync(
        IRecordDataShareRequestStatusChangeParameters recordDataShareRequestStatusChangeParameters);
    
    Task<Guid> RecordDataShareRequestStatusChangeWithCommentsAsync(
        IRecordDataShareRequestStatusChangeWithCommentsParameters recordDataShareRequestStatusChangeWithCommentsParameters);

    Task<IEnumerable<AuditLogDataShareRequestStatusChangeModelData>> GetAuditLogsForDataShareRequestStatusChangesAsync(
        Guid dataShareRequestId,
        DataShareRequestStatusType? fromStatus,
        DataShareRequestStatusType? toStatus);

    Task<IEnumerable<AuditLogDataShareRequestStatusChangeModelData>> GetAuditLogsForDataShareRequestStatusChangesSetAsync(
        Guid dataShareRequestId,
        IEnumerable<DataShareRequestStatusType>? fromStatuses,
        IEnumerable<DataShareRequestStatusType>? toStatuses);
}