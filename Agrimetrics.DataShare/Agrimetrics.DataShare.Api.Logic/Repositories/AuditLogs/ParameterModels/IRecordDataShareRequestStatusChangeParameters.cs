using System.Data;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AuditLogs.ParameterModels;

public interface IRecordDataShareRequestStatusChangeParameters
{
    IDbConnection DbConnection { get; }

    IDbTransaction DbTransaction { get; }

    Guid DataShareRequestId { get; }

    DataShareRequestStatusType FromStatus { get; }

    DataShareRequestStatusType ToStatus { get; }

    IUserIdSet ChangedByUser { get; }

    DateTime ChangedAtLocalTime { get; }
}