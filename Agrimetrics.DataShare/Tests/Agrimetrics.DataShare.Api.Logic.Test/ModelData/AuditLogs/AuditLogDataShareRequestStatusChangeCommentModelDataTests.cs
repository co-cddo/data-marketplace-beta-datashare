using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.AuditLogs;

[TestFixture]
public class AuditLogDataShareRequestStatusChangeCommentModelDataTests
{
    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeCommentModelData_WhenISetId_ThenIdIsSet()
    {
        var testAuditLogDataShareRequestStatusChangeCommentModelData = new AuditLogDataShareRequestStatusChangeCommentModelData();

        var testId = new Guid("B3484E49-9276-48BB-A35F-0B8CDA7A54D3");

        testAuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Id = testId;

        var result = testAuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeCommentModelData_WhenISetStatusChangeId_ThenStatusChangeIdIsSet()
    {
        var testAuditLogDataShareRequestStatusChangeCommentModelData = new AuditLogDataShareRequestStatusChangeCommentModelData();

        var testStatusChangeId = new Guid("B3484E49-9276-48BB-A35F-0B8CDA7A54D3");

        testAuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_StatusChangeId = testStatusChangeId;

        var result = testAuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_StatusChangeId;

        Assert.That(result, Is.EqualTo(testStatusChangeId));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeCommentModelData_WhenISetComment_ThenCommentIsSet(
        [Values("", "  ", "abc")] string testComment)
    {
        var testAuditLogDataShareRequestStatusChangeCommentModelData = new AuditLogDataShareRequestStatusChangeCommentModelData();

        testAuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Comment = testComment;

        var result = testAuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_Comment;

        Assert.That(result, Is.EqualTo(testComment));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeCommentModelData_WhenISetCommentOrder_ThenCommentOrderIsSet(
        [Values(-1, 0, 999)] int testCommentOrder)
    {
        var testAuditLogDataShareRequestStatusChangeCommentModelData = new AuditLogDataShareRequestStatusChangeCommentModelData();

        testAuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_CommentOrder = testCommentOrder;

        var result = testAuditLogDataShareRequestStatusChangeCommentModelData.AuditLogDataShareRequestStatusChangeComment_CommentOrder;

        Assert.That(result, Is.EqualTo(testCommentOrder));
    }
}
