using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class DataShareRequestRejectionResultTests
{
    [Test]
    public void GivenADataShareRequestRejectionResult_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestRejectionResult = new DataShareRequestRejectionResult();

        var testDataShareRequestId = new Guid("1A7C7234-6F17-4A53-971A-843E0663A1F7");

        testDataShareRequestRejectionResult.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestRejectionResult.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestRejectionResult_WhenISetRejectedDecisionSummary_ThenRejectedDecisionSummaryIsSet()
    {
        var testDataShareRequestRejectionResult = new DataShareRequestRejectionResult();

        var testRejectedDecisionSummary = new RejectedDecisionSummary();

        testDataShareRequestRejectionResult.RejectedDecisionSummary = testRejectedDecisionSummary;

        var result = testDataShareRequestRejectionResult.RejectedDecisionSummary;

        Assert.That(result, Is.SameAs(testRejectedDecisionSummary));
    }

    [Theory]
    public void GivenADataShareRequestRejectionResult_WhenISetNotificationSuccess_ThenNotificationSuccessIsSet(
        NotificationSuccess testNotificationSuccess)
    {
        var testDataShareRequestRejectionResult = new DataShareRequestRejectionResult();

        testDataShareRequestRejectionResult.NotificationSuccess = testNotificationSuccess;

        var result = testDataShareRequestRejectionResult.NotificationSuccess;

        Assert.That(result, Is.EqualTo(testNotificationSuccess));
    }
}