using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class DataShareRequestReturnResultTests
{
    [Test]
    public void GivenADataShareRequestReturnResult_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestReturnResult = new DataShareRequestReturnResult();

        var testDataShareRequestId = new Guid("776B7465-FB5C-474F-9960-0FC9FD777362");

        testDataShareRequestReturnResult.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestReturnResult.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestReturnResult_WhenISetReturnedDecisionSummary_ThenReturnedDecisionSummaryIsSet()
    {
        var testDataShareRequestReturnResult = new DataShareRequestReturnResult();

        var testReturnedDecisionSummary = new ReturnedDecisionSummary();

        testDataShareRequestReturnResult.ReturnedDecisionSummary = testReturnedDecisionSummary;

        var result = testDataShareRequestReturnResult.ReturnedDecisionSummary;

        Assert.That(result, Is.SameAs(testReturnedDecisionSummary));
    }

    [Theory]
    public void GivenADataShareRequestReturnResult_WhenISetNotificationSuccess_ThenNotificationSuccessIsSet(
        NotificationSuccess testNotificationSuccess)
    {
        var testDataShareRequestReturnResult = new DataShareRequestReturnResult();

        testDataShareRequestReturnResult.NotificationSuccess = testNotificationSuccess;

        var result = testDataShareRequestReturnResult.NotificationSuccess;

        Assert.That(result, Is.EqualTo(testNotificationSuccess));
    }
}