using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs
{
    public class AuditLogDataShareRequestStatusChangeModelData
    {
        public Guid AuditLogDataShareRequestStatusChange_Id { get; set; }

        public Guid AuditLogDataShareRequestStatusChange_DataShareRequestId { get; set; }

        public DataShareRequestStatusType AuditLogDataShareRequestStatusChange_FromStatus { get; set; }

        public DataShareRequestStatusType AuditLogDataShareRequestStatusChange_ToStatus { get; set; }

        public int AuditLogDataShareRequestStatusChange_ChangedByUserId { get; set; }

        public int AuditLogDataShareRequestStatusChange_ChangedByUserDomainId { get; set; }

        public int AuditLogDataShareRequestStatusChange_ChangedByUserOrganisationId { get; set; }

        public DateTime AuditLogDataShareRequestStatusChange_ChangedAtUtc { get; set; }

        public List<AuditLogDataShareRequestStatusChangeCommentModelData> AuditLogDataShareRequestStatusChange_Comments { get; set; } = [];
    }
}
