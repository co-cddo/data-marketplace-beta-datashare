using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class CompletedSubmissionSummaryModelDataTests
{
    [Test]
    public void GivenACompletedSubmissionSummaryModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testCompletedSubmissionSummaryModelData = new CompletedSubmissionSummaryModelData();

        var testDataShareRequestId = new Guid("C60D33D2-6DFB-4929-820A-C4C1B8BCBF47");

        testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_DataShareRequestId = testDataShareRequestId;

        var result = testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenACompletedSubmissionSummaryModelData_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testCompletedSubmissionSummaryModelData = new CompletedSubmissionSummaryModelData();

        testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Test]
    public void GivenACompletedSubmissionSummaryModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testCompletedSubmissionSummaryModelData = new CompletedSubmissionSummaryModelData();

        testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenACompletedSubmissionSummaryModelData_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testCompletedSubmissionSummaryModelData = new CompletedSubmissionSummaryModelData();

        testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenACompletedSubmissionSummaryModelData_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testCompletedSubmissionSummaryModelData = new CompletedSubmissionSummaryModelData();

        testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_EsdaName = testEsdaName;

        var result = testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Theory]
    public void GivenACompletedSubmissionSummaryModelData_WhenISetStatus_ThenStatusIsSet(
        DataShareRequestStatusType testStatus)
    {
        var testCompletedSubmissionSummaryModelData = new CompletedSubmissionSummaryModelData();

        testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_Status = testStatus;

        var result = testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_Status;

        Assert.That(result, Is.EqualTo(testStatus));
    }

    [Theory]
    public void GivenACompletedSubmissionSummaryModelData_WhenISetDecision_ThenDecisionIsSet(
        SubmissionDecisionType testDecision)
    {
        var testCompletedSubmissionSummaryModelData = new CompletedSubmissionSummaryModelData();

        testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_Decision = testDecision;

        var result = testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_Decision;

        Assert.That(result, Is.EqualTo(testDecision));
    }

    [Test]
    public void GivenACompletedSubmissionSummaryModelData_WhenISetSubmittedOn_ThenSubmittedOnIsSet()
    {
        var testCompletedSubmissionSummaryModelData = new CompletedSubmissionSummaryModelData();

        var testSubmittedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_SubmittedOn = testSubmittedOn;

        var result = testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_SubmittedOn;

        Assert.That(result, Is.EqualTo(testSubmittedOn));
    }

    [Test]
    public void GivenACompletedSubmissionSummaryModelData_WhenISetCompletedOn_ThenCompletedOnIsSet()
    {
        var testCompletedSubmissionSummaryModelData = new CompletedSubmissionSummaryModelData();

        var testCompletedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_CompletedOn = testCompletedOn;

        var result = testCompletedSubmissionSummaryModelData.CompletedSubmissionSummary_CompletedOn;

        Assert.That(result, Is.EqualTo(testCompletedOn));
    }
}