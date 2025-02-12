using Agrimetrics.DataShare.Api.Dto.Models.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.AuditLogs;

[TestFixture]
public class AuditLogDataShareRequestStatusChangesResultTests
{
    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangesResult_WhenISetDataShareRequestAuditLog_ThenDataShareRequestAuditLogIsSet()
    {
        var testDataShareRequestAuditLog = new DataShareRequestAuditLog();

        var testAuditLogDataShareRequestStatusChangesResult = new AuditLogDataShareRequestStatusChangesResult
        {
            DataShareRequestAuditLog = testDataShareRequestAuditLog
        };

        var result = testAuditLogDataShareRequestStatusChangesResult.DataShareRequestAuditLog;

        Assert.That(result, Is.EqualTo(testDataShareRequestAuditLog));
    }
}