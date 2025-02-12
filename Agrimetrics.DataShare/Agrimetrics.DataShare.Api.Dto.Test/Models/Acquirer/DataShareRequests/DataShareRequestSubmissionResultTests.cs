using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Acquirer.DataShareRequests;

[TestFixture]
public class DataShareRequestSubmissionResultTests
{
    [Test]
    public void GivenADataShareRequestSubmissionResult_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestSubmissionResult = new DataShareRequestSubmissionResult();

        var testDataShareRequestId = new Guid("D373A58B-0176-4497-9E8C-B246B75C3B50");

        testDataShareRequestSubmissionResult.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestSubmissionResult.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestSubmissionResult_WhenISetReasonsForCancellation_ThenReasonsForCancellationIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testDataShareRequestSubmissionResult = new DataShareRequestSubmissionResult();

        testDataShareRequestSubmissionResult.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testDataShareRequestSubmissionResult.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenADataShareRequestSubmissionResult_WhenISetNotificationSuccess_ThenNotificationSuccessIsSet(
        NotificationSuccess testNotificationSuccess)
    {
        var testDataShareRequestSubmissionResult = new DataShareRequestSubmissionResult();

        testDataShareRequestSubmissionResult.NotificationSuccess = testNotificationSuccess;

        var result = testDataShareRequestSubmissionResult.NotificationSuccess;

        Assert.That(result, Is.EqualTo(testNotificationSuccess));
    }
}