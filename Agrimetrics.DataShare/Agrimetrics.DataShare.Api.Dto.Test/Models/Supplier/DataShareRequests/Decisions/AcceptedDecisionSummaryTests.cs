using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests.Decisions;

[TestFixture]
public class AcceptedDecisionSummaryTests
{
    [Test]
    public void GivenAnAcceptedDecisionSummary_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testAcceptedDecisionSummary = new AcceptedDecisionSummary();

        var testDataShareRequestId = new Guid("E97FE3CF-07C2-44D2-A412-FFFF8818C550");

        testAcceptedDecisionSummary.DataShareRequestId = testDataShareRequestId;

        var result = testAcceptedDecisionSummary.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenAnAcceptedDecisionSummary_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testAcceptedDecisionSummary = new AcceptedDecisionSummary();

        testAcceptedDecisionSummary.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testAcceptedDecisionSummary.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenAnAcceptedDecisionSummary_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatus testRequestStatus)
    {
        var testAcceptedDecisionSummary = new AcceptedDecisionSummary();

        testAcceptedDecisionSummary.RequestStatus = testRequestStatus;

        var result = testAcceptedDecisionSummary.RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenAnAcceptedDecisionSummary_WhenISetAcquirerUserEmailAddress_ThenAcquirerUserEmailAddressIsSet(
        [Values("", "  ", "abc")] string testAcquirerUserEmailAddress)
    {
        var testAcceptedDecisionSummary = new AcceptedDecisionSummary();

        testAcceptedDecisionSummary.AcquirerUserEmailAddress = testAcquirerUserEmailAddress;

        var result = testAcceptedDecisionSummary.AcquirerUserEmailAddress;

        Assert.That(result, Is.EqualTo(testAcquirerUserEmailAddress));
    }

    [Test]
    public void GivenAnAcceptedDecisionSummary_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testAcceptedDecisionSummary = new AcceptedDecisionSummary();

        testAcceptedDecisionSummary.AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testAcceptedDecisionSummary.AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }
}
