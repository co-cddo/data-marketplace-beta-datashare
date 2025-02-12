using Agrimetrics.DataShare.Api.Dto.Models.AuditLogs;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.AuditLogs;

[TestFixture]
public class DataShareRequestAuditLogEntryTests
{
    [Test]
    public void GivenADataShareRequestAuditLogEntry_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestAuditLogEntry = new DataShareRequestAuditLogEntry();

        var testDataShareRequestId = new Guid("B039C152-35A2-4100-9F18-6D49B85C3B47");

        testDataShareRequestAuditLogEntry.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestAuditLogEntry.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Theory]
    public void GivenADataShareRequestAuditLogEntry_WhenISetFromStatus_ThenFromStatusIsSet(
        DataShareRequestStatus? testFromStatus)
    {
        var testDataShareRequestAuditLogEntry = new DataShareRequestAuditLogEntry();

        testDataShareRequestAuditLogEntry.FromStatus = testFromStatus;

        var result = testDataShareRequestAuditLogEntry.FromStatus;

        Assert.That(result, Is.EqualTo(testFromStatus));
    }

    [Theory]
    public void GivenADataShareRequestAuditLogEntry_WhenISetToStatus_ThenToStatusIsSet(
        DataShareRequestStatus? testToStatus)
    {
        var testDataShareRequestAuditLogEntry = new DataShareRequestAuditLogEntry();

        testDataShareRequestAuditLogEntry.ToStatus = testToStatus;

        var result = testDataShareRequestAuditLogEntry.ToStatus;

        Assert.That(result, Is.EqualTo(testToStatus));
    }

    [Test]
    public void GivenADataShareRequestAuditLogEntry_WhenISetChangedByOrganisationId_ThenChangedByOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testChangedByOrganisationId)
    {
        var testDataShareRequestAuditLogEntry = new DataShareRequestAuditLogEntry();

        testDataShareRequestAuditLogEntry.ChangedByOrganisationId = testChangedByOrganisationId;

        var result = testDataShareRequestAuditLogEntry.ChangedByOrganisationId;

        Assert.That(result, Is.EqualTo(testChangedByOrganisationId));
    }

    [Test]
    public void GivenADataShareRequestAuditLogEntry_WhenISetChangedByDomainId_ThenChangedByDomainIdIsSet(
        [Values(-1, 0, 999)] int testChangedByDomainId)
    {
        var testDataShareRequestAuditLogEntry = new DataShareRequestAuditLogEntry();

        testDataShareRequestAuditLogEntry.ChangedByDomainId = testChangedByDomainId;

        var result = testDataShareRequestAuditLogEntry.ChangedByDomainId;

        Assert.That(result, Is.EqualTo(testChangedByDomainId));
    }

    [Test]
    public void GivenADataShareRequestAuditLogEntry_WhenISetChangedByUserId_ThenChangedByUserIdIsSet(
        [Values(-1, 0, 999)] int testChangedByUserId)
    {
        var testDataShareRequestAuditLogEntry = new DataShareRequestAuditLogEntry();

        testDataShareRequestAuditLogEntry.ChangedByUserId = testChangedByUserId;

        var result = testDataShareRequestAuditLogEntry.ChangedByUserId;

        Assert.That(result, Is.EqualTo(testChangedByUserId));
    }

    [Test]
    public void GivenADataShareRequestAuditLogEntry_WhenISetChangedOnUtc_ThenChangedOnUtcIsSet()
    {
        var testDataShareRequestAuditLogEntry = new DataShareRequestAuditLogEntry();

        var testChangedOnUtc = new DateTime(2025, 12, 25, 14, 45, 59);

        testDataShareRequestAuditLogEntry.ChangedOnUtc = testChangedOnUtc;

        var result = testDataShareRequestAuditLogEntry.ChangedOnUtc;

        Assert.That(result, Is.EqualTo(testChangedOnUtc));
    }

    [Test]
    public void GivenADataShareRequestAuditLogEntry_WhenISetAnEmptySetOfComments_ThenCommentsIsSet()
    {
        var testDataShareRequestAuditLogEntry = new DataShareRequestAuditLogEntry();

        testDataShareRequestAuditLogEntry.Comments = [];

        var result = testDataShareRequestAuditLogEntry.Comments;

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GivenADataShareRequestAuditLogEntry_WhenISetComments_ThenCommentsIsSet()
    {
        var testDataShareRequestAuditLogEntry = new DataShareRequestAuditLogEntry();

        var testComments = new List<DataShareRequestAuditLogEntryComment>
        {
            new(), new(), new()
        };

        testDataShareRequestAuditLogEntry.Comments = testComments;

        var result = testDataShareRequestAuditLogEntry.Comments;

        Assert.That(result, Is.EqualTo(testComments));
    }
}