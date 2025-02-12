namespace Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;

public class AuditLogDataShareRequestStatusChangeCommentModelData
{
    public Guid AuditLogDataShareRequestStatusChangeComment_Id { get; set; }

    public Guid AuditLogDataShareRequestStatusChangeComment_StatusChangeId { get; set; }

    public string AuditLogDataShareRequestStatusChangeComment_Comment { get; set; } = string.Empty;

    public int AuditLogDataShareRequestStatusChangeComment_CommentOrder { get; set; }
}