using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class DataShareRequestAcceptanceResultTests
{
    [Test]
    public void GivenADataShareRequestAcceptanceResult_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestAcceptanceResult = new DataShareRequestAcceptanceResult();

        var testDataShareRequestId = new Guid("DD11D172-58C3-4FE0-B66A-79CC03F0FE68");

        testDataShareRequestAcceptanceResult.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestAcceptanceResult.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestAcceptanceResult_WhenISetAcceptedDecisionSummary_ThenAcceptedDecisionSummaryIsSet()
    {
        var testDataShareRequestAcceptanceResult = new DataShareRequestAcceptanceResult();

        var testAcceptedDecisionSummary = new AcceptedDecisionSummary();

        testDataShareRequestAcceptanceResult.AcceptedDecisionSummary = testAcceptedDecisionSummary;

        var result = testDataShareRequestAcceptanceResult.AcceptedDecisionSummary;

        Assert.That(result, Is.EqualTo(testAcceptedDecisionSummary));
    }

    [Theory]
    public void GivenADataShareRequestAcceptanceResult_WhenISetNotificationSuccess_ThenNotificationSuccessIsSet(
        NotificationSuccess testNotificationSuccess)
    {
        var testDataShareRequestAcceptanceResult = new DataShareRequestAcceptanceResult();

        testDataShareRequestAcceptanceResult.NotificationSuccess = testNotificationSuccess;

        var result = testDataShareRequestAcceptanceResult.NotificationSuccess;

        Assert.That(result, Is.EqualTo(testNotificationSuccess));
    }
}