using System.ComponentModel;
using Agrimetrics.DataShare.Api.Dto.Models.AuditLogs;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;

internal class AuditLogModelDataFactory : IAuditLogModelDataFactory
{
    IAuditLogDataShareRequestStatusChangesResult IAuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
        Guid dataShareRequestId,
        IEnumerable<AuditLogDataShareRequestStatusChangeModelData> auditLogsForDataShareRequestStatusChangesModelDatas)
    {
        return new AuditLogDataShareRequestStatusChangesResult
        {
            DataShareRequestAuditLog = new DataShareRequestAuditLog
            {
                DataShareRequestId = dataShareRequestId,
                AuditLogEntries = auditLogsForDataShareRequestStatusChangesModelDatas.Select(ConvertAuditLogForDataShareRequestStatusChangesModelData).ToList()
            }
        };

        DataShareRequestAuditLogEntry ConvertAuditLogForDataShareRequestStatusChangesModelData(
            AuditLogDataShareRequestStatusChangeModelData auditLogDataShareRequestStatusChangeModelData)
        {
            return new DataShareRequestAuditLogEntry
            {
                DataShareRequestId = auditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_DataShareRequestId,
                FromStatus = DoConvertDataShareRequestStatusTypeToDataShareRequestStatus(auditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_FromStatus),
                ToStatus = DoConvertDataShareRequestStatusTypeToDataShareRequestStatus(auditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ToStatus),
                ChangedByOrganisationId = auditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedByUserOrganisationId,
                ChangedByDomainId = auditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedByUserDomainId,
                ChangedByUserId = auditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedByUserId,
                ChangedOnUtc = auditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedAtUtc,
                Comments = auditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_Comments.Select(ConvertAuditLogDataShareRequestStatusChangeComment).OfType<DataShareRequestAuditLogEntryComment>().ToList()
            };
        }

        DataShareRequestAuditLogEntryComment? ConvertAuditLogDataShareRequestStatusChangeComment(
            AuditLogDataShareRequestStatusChangeCommentModelData? auditLogDataShareRequestStatusChangeCommentModelData)
        {
            if (auditLogDataShareRequestStatusChangeCommentModelData == null) return null;

            return new DataShareRequestAuditLogEntryComment
            {
                CommentOrder = auditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_CommentOrder,
                Comment = auditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Comment
            };
        }
    }

    private static DataShareRequestStatus? DoConvertDataShareRequestStatusTypeToDataShareRequestStatus(
        DataShareRequestStatusType? dataShareRequestStatusType)
    {
        return dataShareRequestStatusType switch
        {
            DataShareRequestStatusType.None => null,
            DataShareRequestStatusType.Draft => DataShareRequestStatus.Draft,
            DataShareRequestStatusType.Submitted => DataShareRequestStatus.Submitted,
            DataShareRequestStatusType.Accepted => DataShareRequestStatus.Accepted,
            DataShareRequestStatusType.Rejected => DataShareRequestStatus.Rejected,
            DataShareRequestStatusType.Returned => DataShareRequestStatus.Returned,
            DataShareRequestStatusType.Cancelled => DataShareRequestStatus.Cancelled,
            DataShareRequestStatusType.InReview => DataShareRequestStatus.InReview,
            DataShareRequestStatusType.Deleted => DataShareRequestStatus.Deleted,
            _ => null
        };
    }

    DataShareRequestStatusType IAuditLogModelDataFactory.ConvertDataShareRequestStatusToDataShareRequestStatusType(
        DataShareRequestStatus dataShareRequestStatus)
    {
        return dataShareRequestStatus switch
        {
            DataShareRequestStatus.Draft => DataShareRequestStatusType.Draft,
            DataShareRequestStatus.Submitted => DataShareRequestStatusType.Submitted,
            DataShareRequestStatus.Rejected => DataShareRequestStatusType.Rejected,
            DataShareRequestStatus.Accepted => DataShareRequestStatusType.Accepted,
            DataShareRequestStatus.Cancelled => DataShareRequestStatusType.Cancelled,
            DataShareRequestStatus.Returned => DataShareRequestStatusType.Returned,
            DataShareRequestStatus.InReview => DataShareRequestStatusType.InReview,
            DataShareRequestStatus.Deleted => DataShareRequestStatusType.Deleted,

            _ => throw new InvalidEnumArgumentException(
                nameof(dataShareRequestStatus), (int)dataShareRequestStatus, typeof(DataShareRequestStatus))
        };
    }
}