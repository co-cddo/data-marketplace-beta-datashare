using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class CompletedSubmissionSummaryTests
{
    [Test]
    public void GivenACompletedSubmissionSummary_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testCompletedSubmissionSummary = new CompletedSubmissionSummary();

        var testDataShareRequestId = new Guid("2A6332D6-B58E-4C62-9338-7F4013E0F806");

        testCompletedSubmissionSummary.DataShareRequestId = testDataShareRequestId;

        var result = testCompletedSubmissionSummary.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenACompletedSubmissionSummary_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testCompletedSubmissionSummary = new CompletedSubmissionSummary();

        testCompletedSubmissionSummary.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testCompletedSubmissionSummary.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Test]
    public void GivenACompletedSubmissionSummary_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testCompletedSubmissionSummary = new CompletedSubmissionSummary();

        testCompletedSubmissionSummary.AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testCompletedSubmissionSummary.AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenACompletedSubmissionSummary_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testCompletedSubmissionSummary = new CompletedSubmissionSummary();

        testCompletedSubmissionSummary.EsdaName = testEsdaName;

        var result = testCompletedSubmissionSummary.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenACompletedSubmissionSummary_WhenISetSubmittedOn_ThenSubmittedOnIsSet()
    {
        var testCompletedSubmissionSummary = new CompletedSubmissionSummary();

        var testSubmittedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testCompletedSubmissionSummary.SubmittedOn = testSubmittedOn;

        var result = testCompletedSubmissionSummary.SubmittedOn;

        Assert.That(result, Is.EqualTo(testSubmittedOn));
    }

    [Test]
    public void GivenACompletedSubmissionSummary_WhenISetCompletedOn_ThenCompletedOnIsSet()
    {
        var testCompletedSubmissionSummary = new CompletedSubmissionSummary();

        var testCompletedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testCompletedSubmissionSummary.CompletedOn = testCompletedOn;

        var result = testCompletedSubmissionSummary.CompletedOn;

        Assert.That(result, Is.EqualTo(testCompletedOn));
    }

    [Theory]
    public void GivenACompletedSubmissionSummary_WhenISetDecision_ThenDecisionIsSet(
        SubmissionDecision testDecision)
    {
        var testCompletedSubmissionSummary = new CompletedSubmissionSummary();

        testCompletedSubmissionSummary.Decision = testDecision;

        var result = testCompletedSubmissionSummary.Decision;

        Assert.That(result, Is.EqualTo(testDecision));
    }
}