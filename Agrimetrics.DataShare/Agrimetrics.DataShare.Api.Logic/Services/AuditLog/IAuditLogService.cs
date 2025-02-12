using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;

namespace Agrimetrics.DataShare.Api.Logic.Services.AuditLog
{
    public interface IAuditLogService
    {
        Task<IServiceOperationDataResult<IAuditLogDataShareRequestStatusChangesResult>> GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
            Guid dataShareRequestId,
            IEnumerable<DataShareRequestStatus> toStatuses);

        Task<IEnumerable<AuditLogDataShareRequestStatusChangeModelData?>> GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
            Guid dataShareRequestId,
            IEnumerable<DataShareRequestStatusType> toStatuses);

        Task<AuditLogDataShareRequestStatusChangeModelData?> GetMostRecentAuditLogForDataShareRequestStatusChangeAsync(
            Guid dataShareRequestId);

        Task<AuditLogDataShareRequestStatusChangeModelData?> GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
            Guid dataShareRequestId,
            DataShareRequestStatusType? toStatus);
    }
}
