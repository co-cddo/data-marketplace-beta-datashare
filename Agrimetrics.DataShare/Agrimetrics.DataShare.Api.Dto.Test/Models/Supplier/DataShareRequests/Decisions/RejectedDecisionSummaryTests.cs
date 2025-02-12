using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests.Decisions;

[TestFixture]
public class RejectedDecisionSummaryTests
{
    [Test]
    public void GivenARejectedDecisionSummary_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testRejectedDecisionSummary = new RejectedDecisionSummary();

        var testDataShareRequestId = new Guid("7AB5DFF3-B871-49FB-81F0-52093BF2713A");

        testRejectedDecisionSummary.DataShareRequestId = testDataShareRequestId;

        var result = testRejectedDecisionSummary.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenARejectedDecisionSummary_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testRejectedDecisionSummary = new RejectedDecisionSummary();

        testRejectedDecisionSummary.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testRejectedDecisionSummary.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenARejectedDecisionSummary_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatus testRequestStatus)
    {
        var testRejectedDecisionSummary = new RejectedDecisionSummary();

        testRejectedDecisionSummary.RequestStatus = testRequestStatus;

        var result = testRejectedDecisionSummary.RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenARejectedDecisionSummary_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testRejectedDecisionSummary = new RejectedDecisionSummary();

        testRejectedDecisionSummary.AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testRejectedDecisionSummary.AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }
}