using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests;

[TestFixture]
public class DataShareRequestSummaryTests
{
    [Test]
    public void GivenADataShareRequestSummary_WhenISetId_ThenIdIsSet()
    {
        var testDataShareRequestSummary = new DataShareRequestSummary();

        var testId = new Guid("0029796F-8232-4FCC-B9C5-786793AD7EC0");

        testDataShareRequestSummary.Id = testId;

        var result = testDataShareRequestSummary.Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenADataShareRequestSummary_WhenISetRequestId_ThenRequestIdIsSet(
        [Values("", "  ", "abc")] string testRequestId)
    {
        var testDataShareRequestSummary = new DataShareRequestSummary();

        testDataShareRequestSummary.RequestId = testRequestId;

        var result = testDataShareRequestSummary.RequestId;

        Assert.That(result, Is.EqualTo(testRequestId));
    }

    [Test]
    public void GivenADataShareRequestSummary_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testDataShareRequestSummary = new DataShareRequestSummary();

        testDataShareRequestSummary.EsdaName = testEsdaName;

        var result = testDataShareRequestSummary.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Theory]
    public void GivenADataShareRequestSummary_WhenISetDataShareRequestStatus_ThenDataShareRequestStatusIsSet(
        DataShareRequestStatus testDataShareRequestStatus)
    {
        var testDataShareRequestSummary = new DataShareRequestSummary();

        testDataShareRequestSummary.DataShareRequestStatus = testDataShareRequestStatus;

        var result = testDataShareRequestSummary.DataShareRequestStatus;

        Assert.That(result, Is.EqualTo(testDataShareRequestStatus));
    }
}