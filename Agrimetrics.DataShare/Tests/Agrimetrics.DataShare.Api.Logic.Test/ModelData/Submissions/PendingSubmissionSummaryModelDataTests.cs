using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class PendingSubmissionSummaryModelDataTests
{
    [Test]
    public void GivenAPendingSubmissionSummaryModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testPendingSubmissionSummaryModelData = new PendingSubmissionSummaryModelData();

        var testDataShareRequestId = new Guid("39EB3E3D-89A2-42F0-B3E3-D6FCC09B241D");

        testPendingSubmissionSummaryModelData.PendingSubmissionSummary_DataShareRequestId = testDataShareRequestId;

        var result = testPendingSubmissionSummaryModelData.PendingSubmissionSummary_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenAPendingSubmissionSummaryModelData_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testPendingSubmissionSummaryModelData = new PendingSubmissionSummaryModelData();

        testPendingSubmissionSummaryModelData.PendingSubmissionSummary_DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testPendingSubmissionSummaryModelData.PendingSubmissionSummary_DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Test]
    public void GivenAPendingSubmissionSummaryModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testPendingSubmissionSummaryModelData = new PendingSubmissionSummaryModelData();

        testPendingSubmissionSummaryModelData.PendingSubmissionSummary_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testPendingSubmissionSummaryModelData.PendingSubmissionSummary_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenAPendingSubmissionSummaryModelData_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testPendingSubmissionSummaryModelData = new PendingSubmissionSummaryModelData();

        testPendingSubmissionSummaryModelData.PendingSubmissionSummary_AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testPendingSubmissionSummaryModelData.PendingSubmissionSummary_AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenAPendingSubmissionSummaryModelData_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testPendingSubmissionSummaryModelData = new PendingSubmissionSummaryModelData();

        testPendingSubmissionSummaryModelData.PendingSubmissionSummary_EsdaName = testEsdaName;

        var result = testPendingSubmissionSummaryModelData.PendingSubmissionSummary_EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenAPendingSubmissionSummaryModelData_WhenISetSubmittedOn_ThenSubmittedOnIsSet()
    {
        var testPendingSubmissionSummaryModelData = new PendingSubmissionSummaryModelData();

        var testSubmittedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testPendingSubmissionSummaryModelData.PendingSubmissionSummary_SubmittedOn = testSubmittedOn;

        var result = testPendingSubmissionSummaryModelData.PendingSubmissionSummary_SubmittedOn;

        Assert.That(result, Is.EqualTo(testSubmittedOn));
    }

    [Test]
    public void GivenAPendingSubmissionSummaryModelData_WhenISetWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testPendingSubmissionSummaryModelData = new PendingSubmissionSummaryModelData();

        var testWhenNeededBy = new DateTime(2025, 12, 25, 14, 45, 59);

        testPendingSubmissionSummaryModelData.PendingSubmissionSummary_WhenNeededBy = testWhenNeededBy;

        var result = testPendingSubmissionSummaryModelData.PendingSubmissionSummary_WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Theory]
    public void GivenAPendingSubmissionSummaryModelData_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatusType testRequestStatus)
    {
        var testPendingSubmissionSummaryModelData = new PendingSubmissionSummaryModelData();

        testPendingSubmissionSummaryModelData.PendingSubmissionSummary_RequestStatus = testRequestStatus;

        var result = testPendingSubmissionSummaryModelData.PendingSubmissionSummary_RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }
}