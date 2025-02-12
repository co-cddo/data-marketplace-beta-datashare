using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class AcceptedDecisionSummaryModelDataTests
{
    [Test]
    public void GivenAAcceptedDecisionSummaryModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testAcceptedDecisionSummaryModelData = new AcceptedDecisionSummaryModelData();

        var testDataShareRequestId = new Guid("F8936010-F3F4-4AC4-9B05-37F835793D04");

        testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId = testDataShareRequestId;

        var result = testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenAAcceptedDecisionSummaryModelData_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testAcceptedDecisionSummaryModelData = new AcceptedDecisionSummaryModelData();

        testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenAAcceptedDecisionSummaryModelData_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatusType testRequestStatus)
    {
        var testAcceptedDecisionSummaryModelData = new AcceptedDecisionSummaryModelData();

        testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_RequestStatus = testRequestStatus;

        var result = testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenAAcceptedDecisionSummaryModelData_WhenISetAcquirerUserId_ThenAcquirerUserIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerUserId)
    {
        var testAcceptedDecisionSummaryModelData = new AcceptedDecisionSummaryModelData();

        testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId = testAcquirerUserId;

        var result = testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId;

        Assert.That(result, Is.EqualTo(testAcquirerUserId));
    }

    [Test]
    public void GivenAAcceptedDecisionSummaryModelData_WhenISetAcquirerUserEmailAddress_ThenAcquirerUserEmailAddressIsSet(
        [Values("", "  ", "abc")] string testAcquirerUserEmailAddress)
    {
        var testAcceptedDecisionSummaryModelData = new AcceptedDecisionSummaryModelData();

        testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserEmailAddress = testAcquirerUserEmailAddress;

        var result = testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserEmailAddress;

        Assert.That(result, Is.EqualTo(testAcquirerUserEmailAddress));
    }

    [Test]
    public void GivenAAcceptedDecisionSummaryModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testAcceptedDecisionSummaryModelData = new AcceptedDecisionSummaryModelData();

        testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenAAcceptedDecisionSummaryModelData_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testAcceptedDecisionSummaryModelData = new AcceptedDecisionSummaryModelData();

        testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }
}

