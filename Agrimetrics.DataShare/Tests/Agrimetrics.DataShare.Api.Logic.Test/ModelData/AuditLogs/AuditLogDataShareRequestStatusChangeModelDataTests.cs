using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.AuditLogs;

[TestFixture]
public class AuditLogDataShareRequestStatusChangeModelDataTests
{
    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelData_WhenISetId_ThenIdIsSet()
    {
        var testAuditLogDataShareRequestStatusChangeModelData = new AuditLogDataShareRequestStatusChangeModelData();

        var testId = new Guid("E201159F-04E8-4EFE-BA04-8D913F366C21");

        testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_Id = testId;

        var result = testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testAuditLogDataShareRequestStatusChangeModelData = new AuditLogDataShareRequestStatusChangeModelData();

        var testDataShareRequestId = new Guid("E201159F-04E8-4EFE-BA04-8D913F366C21");

        testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_DataShareRequestId = testDataShareRequestId;

        var result = testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Theory]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelData_WhenISetFromStatus_ThenFromStatusIsSet(
        DataShareRequestStatusType testFromStatus)
    {
        var testAuditLogDataShareRequestStatusChangeModelData = new AuditLogDataShareRequestStatusChangeModelData();

        testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_FromStatus = testFromStatus;

        var result = testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_FromStatus;

        Assert.That(result, Is.EqualTo(testFromStatus));
    }

    [Theory]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelData_WhenISetToStatus_ThenToStatusIsSet(
        DataShareRequestStatusType testToStatus)
    {
        var testAuditLogDataShareRequestStatusChangeModelData = new AuditLogDataShareRequestStatusChangeModelData();

        testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ToStatus = testToStatus;

        var result = testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ToStatus;

        Assert.That(result, Is.EqualTo(testToStatus));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelData_WhenISetChangedByUserId_ThenChangedByUserIdIsSet(
        [Values(-1, 0, 999)] int testChangedByUserId)
    {
        var testAuditLogDataShareRequestStatusChangeModelData = new AuditLogDataShareRequestStatusChangeModelData();

        testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedByUserId = testChangedByUserId;

        var result = testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedByUserId;

        Assert.That(result, Is.EqualTo(testChangedByUserId));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelData_WhenISetChangedByUserDomainId_ThenChangedByUserDomainIdIsSet(
        [Values(-1, 0, 999)] int testChangedByUserDomainId)
    {
        var testAuditLogDataShareRequestStatusChangeModelData = new AuditLogDataShareRequestStatusChangeModelData();

        testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedByUserDomainId = testChangedByUserDomainId;

        var result = testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedByUserDomainId;

        Assert.That(result, Is.EqualTo(testChangedByUserDomainId));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelData_WhenISetChangedByUserOrganisationId_ThenChangedByUserOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testChangedByUserOrganisationId)
    {
        var testAuditLogDataShareRequestStatusChangeModelData = new AuditLogDataShareRequestStatusChangeModelData();

        testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedByUserOrganisationId = testChangedByUserOrganisationId;

        var result = testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedByUserOrganisationId;

        Assert.That(result, Is.EqualTo(testChangedByUserOrganisationId));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelData_WhenISetChangedAtUtc_ThenChangedAtUtcIsSet()
    {
        var testAuditLogDataShareRequestStatusChangeModelData = new AuditLogDataShareRequestStatusChangeModelData();

        var testChangedAtUtc = new DateTime(2025, 12, 25, 14, 45, 59);

        testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedAtUtc = testChangedAtUtc;

        var result = testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_ChangedAtUtc;

        Assert.That(result, Is.EqualTo(testChangedAtUtc));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelData_WhenISetAnEmptySetOfComments_ThenCommentsIsSet()
    {
        var testAuditLogDataShareRequestStatusChangeModelData = new AuditLogDataShareRequestStatusChangeModelData();

        var testComments = new List<AuditLogDataShareRequestStatusChangeCommentModelData>();

        testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_Comments = testComments;

        var result = testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_Comments;

        Assert.That(result, Is.EqualTo(testComments));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelData_WhenISetComments_ThenCommentsIsSet()
    {
        var testAuditLogDataShareRequestStatusChangeModelData = new AuditLogDataShareRequestStatusChangeModelData();

        var testComments = new List<AuditLogDataShareRequestStatusChangeCommentModelData> {new(), new(), new()};

        testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_Comments = testComments;

        var result = testAuditLogDataShareRequestStatusChangeModelData.AuditLogDataShareRequestStatusChange_Comments;

        Assert.That(result, Is.EqualTo(testComments));
    }
}