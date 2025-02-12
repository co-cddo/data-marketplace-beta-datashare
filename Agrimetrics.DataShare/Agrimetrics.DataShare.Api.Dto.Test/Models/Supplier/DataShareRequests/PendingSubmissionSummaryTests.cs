using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class PendingSubmissionSummaryTests
{
    [Test]
    public void GivenAPendingSubmissionSummary_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testPendingSubmissionSummary = new PendingSubmissionSummary();

        var testDataShareRequestId = new Guid("6FE8E103-F074-4BDE-93E9-5ADD06DEDEBD");

        testPendingSubmissionSummary.DataShareRequestId = testDataShareRequestId;

        var result = testPendingSubmissionSummary.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenAPendingSubmissionSummary_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testPendingSubmissionSummary = new PendingSubmissionSummary();

        testPendingSubmissionSummary.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testPendingSubmissionSummary.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Test]
    public void GivenAPendingSubmissionSummary_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testPendingSubmissionSummary = new PendingSubmissionSummary();

        testPendingSubmissionSummary.AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testPendingSubmissionSummary.AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenAPendingSubmissionSummary_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testPendingSubmissionSummary = new PendingSubmissionSummary();

        testPendingSubmissionSummary.EsdaName = testEsdaName;

        var result = testPendingSubmissionSummary.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenAPendingSubmissionSummary_WhenISetSubmittedOn_ThenSubmittedOnIsSet()
    {
        var testPendingSubmissionSummary = new PendingSubmissionSummary();

        var testSubmittedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testPendingSubmissionSummary.SubmittedOn = testSubmittedOn;

        var result = testPendingSubmissionSummary.SubmittedOn;

        Assert.That(result, Is.EqualTo(testSubmittedOn));
    }

    [Test]
    public void GivenAPendingSubmissionSummary_WhenISetANullWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testPendingSubmissionSummary = new PendingSubmissionSummary();

        var testWhenNeededBy = (DateTime?)null;

        testPendingSubmissionSummary.WhenNeededBy = testWhenNeededBy;

        var result = testPendingSubmissionSummary.WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenAPendingSubmissionSummary_WhenISetWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testPendingSubmissionSummary = new PendingSubmissionSummary();

        var testWhenNeededBy = new DateTime(2025, 12, 25, 14, 45, 59);

        testPendingSubmissionSummary.WhenNeededBy = testWhenNeededBy;

        var result = testPendingSubmissionSummary.WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Theory]
    public void GivenAPendingSubmissionSummary_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatus testRequestStatus)
    {
        var testPendingSubmissionSummary = new PendingSubmissionSummary();

        testPendingSubmissionSummary.RequestStatus = testRequestStatus;

        var result = testPendingSubmissionSummary.RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }
}