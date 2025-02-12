using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests.Decisions;

[TestFixture]
public class ReturnedDecisionSummaryTests
{
    [Test]
    public void GivenAReturnedDecisionSummary_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testReturnedDecisionSummary = new ReturnedDecisionSummary();

        var testDataShareRequestId = new Guid("4E4C0498-BC4A-4A67-872E-07C3C9A94917");

        testReturnedDecisionSummary.DataShareRequestId = testDataShareRequestId;

        var result = testReturnedDecisionSummary.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenAReturnedDecisionSummary_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testReturnedDecisionSummary = new ReturnedDecisionSummary();

        testReturnedDecisionSummary.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testReturnedDecisionSummary.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenAReturnedDecisionSummary_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatus testRequestStatus)
    {
        var testReturnedDecisionSummary = new ReturnedDecisionSummary();

        testReturnedDecisionSummary.RequestStatus = testRequestStatus;

        var result = testReturnedDecisionSummary.RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenAReturnedDecisionSummary_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testReturnedDecisionSummary = new ReturnedDecisionSummary();

        testReturnedDecisionSummary.AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testReturnedDecisionSummary.AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }
}