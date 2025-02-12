using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Acquirer.DataShareRequests;

[TestFixture]
public class DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaryTests
{
    [Test]
    public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummary_WhenISetId_ThenIdIsSet()
    {
        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary();

        var testId = new Guid("D373A58B-0176-4497-9E8C-B246B75C3B50");

        testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.Id = testId;

        var result = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummary_WhenISetRequestId_ThenRequestIdIsSet(
        [Values("", "  ", "abc")] string testRequestId)
    {
        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary();

        testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.RequestId = testRequestId;

        var result = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.RequestId;

        Assert.That(result, Is.EqualTo(testRequestId));
    }

    [Theory]
    public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummary_WhenISetStatus_ThenStatusIsSet(
        DataShareRequestStatus testStatus)
    {
        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary();

        testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.Status = testStatus;

        var result = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.Status;

        Assert.That(result, Is.EqualTo(testStatus));
    }

    [Test]
    public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummary_WhenISetDateStarted_ThenDateStartedIsSet()
    {
        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary();

        var testDateStarted = new DateTime(2025, 12, 25, 15, 45, 59);

        testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.DateStarted = testDateStarted;

        var result = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.DateStarted;

        Assert.That(result, Is.EqualTo(testDateStarted));
    }

    [Test]
    public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummary_WhenISetDateSubmitted_ThenDateSubmittedIsSet()
    {
        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary();

        var testDateSubmitted = new DateTime(2025, 12, 25, 15, 45, 59);

        testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.DateSubmitted = testDateSubmitted;

        var result = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.DateSubmitted;

        Assert.That(result, Is.EqualTo(testDateSubmitted));
    }

    [Test]
    public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummary_WhenISetNullDateSubmitted_ThenDateSubmittedIsSet()
    {
        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary();

        testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.DateSubmitted = null;

        var result = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.DateSubmitted;

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummary_WhenISetOriginatingAcquirerContactDetails_ThenOriginatingAcquirerContactDetailsIsSet()
    {
        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary();

        var testOriginatingAcquirerContactDetails = new AcquirerContactDetails();

        testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.OriginatingAcquirerContactDetails = testOriginatingAcquirerContactDetails;

        var result = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary.OriginatingAcquirerContactDetails;

        Assert.That(result, Is.SameAs(testOriginatingAcquirerContactDetails));
    }
}