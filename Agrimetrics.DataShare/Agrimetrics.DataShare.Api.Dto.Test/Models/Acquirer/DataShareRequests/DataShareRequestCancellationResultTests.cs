using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Acquirer.DataShareRequests;

[TestFixture]
public class DataShareRequestCancellationResultTests
{
    [Test]
    public void GivenDataShareRequestCancellationResult_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestCancellationResult = new DataShareRequestCancellationResult();

        var testDataShareRequestId = new Guid("D373A58B-0176-4497-9E8C-B246B75C3B50");

        testDataShareRequestCancellationResult.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestCancellationResult.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenDataShareRequestCancellationResult_WhenISetReasonsForCancellation_ThenReasonsForCancellationIsSet(
        [Values("", "  ", "abc")] string testReasonsForCancellation)
    {
        var testDataShareRequestCancellationResult = new DataShareRequestCancellationResult();

        testDataShareRequestCancellationResult.ReasonsForCancellation = testReasonsForCancellation;

        var result = testDataShareRequestCancellationResult.ReasonsForCancellation;

        Assert.That(result, Is.EqualTo(testReasonsForCancellation));
    }

    [Theory]
    public void GivenDataShareRequestCancellationResult_WhenISetNotificationSuccess_ThenNotificationSuccessIsSet(
        NotificationSuccess testNotificationSuccess)
    {
        var testDataShareRequestCancellationResult = new DataShareRequestCancellationResult();

        testDataShareRequestCancellationResult.NotificationSuccess = testNotificationSuccess;

        var result = testDataShareRequestCancellationResult.NotificationSuccess;

        Assert.That(result, Is.EqualTo(testNotificationSuccess));
    }
}