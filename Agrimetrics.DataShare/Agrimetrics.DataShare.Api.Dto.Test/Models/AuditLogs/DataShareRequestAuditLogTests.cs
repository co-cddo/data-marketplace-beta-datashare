using Agrimetrics.DataShare.Api.Dto.Models.AuditLogs;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.AuditLogs;

[TestFixture]
public class DataShareRequestAuditLogTests
{
    [Test]
    public void GivenADataShareRequestAuditLog_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestAuditLog = new DataShareRequestAuditLog();

        var testDataShareRequestId = new Guid("D373A58B-0176-4497-9E8C-B246B75C3B50");

        testDataShareRequestAuditLog.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestAuditLog.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestAuditLog_WhenISetAnEmptySetOfAuditLogEntries_ThenAuditLogEntriesIsSet()
    {
        var testDataShareRequestAuditLog = new DataShareRequestAuditLog();

        testDataShareRequestAuditLog.AuditLogEntries = [];

        var result = testDataShareRequestAuditLog.AuditLogEntries;

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GivenADataShareRequestAuditLog_WhenISetAuditLogEntries_ThenAuditLogEntriesIsSet()
    {
        var testDataShareRequestAuditLog = new DataShareRequestAuditLog();

        var testAuditLogEntries = new List<DataShareRequestAuditLogEntry>
        {
            new(), new(), new()
        };

        testDataShareRequestAuditLog.AuditLogEntries = testAuditLogEntries;

        var result = testDataShareRequestAuditLog.AuditLogEntries;

        Assert.That(result, Is.SameAs(testAuditLogEntries));
    }
}
