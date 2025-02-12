using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class RejectedDecisionSummaryModelDataTests
{
    [Test]
    public void GivenARejectedDecisionSummaryModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testRejectedDecisionSummaryModelData = new RejectedDecisionSummaryModelData();

        var testDataShareRequestId = new Guid("0DD3F5E4-F787-4952-BE25-1425EE441AAC");

        testRejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId = testDataShareRequestId;

        var result = testRejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenARejectedDecisionSummaryModelData_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testRejectedDecisionSummaryModelData = new RejectedDecisionSummaryModelData();

        testRejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testRejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenARejectedDecisionSummaryModelData_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatusType testRequestStatus)
    {
        var testRejectedDecisionSummaryModelData = new RejectedDecisionSummaryModelData();

        testRejectedDecisionSummaryModelData.RejectedDecisionSummary_RequestStatus = testRequestStatus;

        var result = testRejectedDecisionSummaryModelData.RejectedDecisionSummary_RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenARejectedDecisionSummaryModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testRejectedDecisionSummaryModelData = new RejectedDecisionSummaryModelData();

        testRejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testRejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenARejectedDecisionSummaryModelData_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testRejectedDecisionSummaryModelData = new RejectedDecisionSummaryModelData();

        testRejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testRejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }
}