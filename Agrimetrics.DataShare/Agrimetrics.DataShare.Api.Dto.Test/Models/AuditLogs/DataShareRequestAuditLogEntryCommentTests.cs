using Agrimetrics.DataShare.Api.Dto.Models.AuditLogs;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.AuditLogs;

[TestFixture]
public class DataShareRequestAuditLogEntryCommentTests
{
    [Test]
    public void GivenADataShareRequestAuditLogEntryComment_WhenISetCommentOrder_ThenCommentOrderIsSet(
        [Values(-1, 0, 999)] int testCommentOrder)
    {
        var testDataShareRequestAuditLogEntryComment = new DataShareRequestAuditLogEntryComment();

        testDataShareRequestAuditLogEntryComment.CommentOrder = testCommentOrder;

        var result = testDataShareRequestAuditLogEntryComment.CommentOrder;

        Assert.That(result, Is.EqualTo(testCommentOrder));
    }

    [Test]
    public void GivenADataShareRequestAuditLogEntryComment_WhenISetComment_ThenCommentIsSet(
        [Values("", "  ", "abc")] string testComment)
    {
        var testDataShareRequestAuditLogEntryComment = new DataShareRequestAuditLogEntryComment();

        testDataShareRequestAuditLogEntryComment.Comment = testComment;

        var result = testDataShareRequestAuditLogEntryComment.Comment;

        Assert.That(result, Is.EqualTo(testComment));
    }
}