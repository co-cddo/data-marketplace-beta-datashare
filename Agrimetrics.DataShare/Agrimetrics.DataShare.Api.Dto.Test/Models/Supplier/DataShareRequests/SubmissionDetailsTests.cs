using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionDetailsTests
{
    [Test]
    public void GivenASubmissionDetails_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testSubmissionDetails = new SubmissionDetails();

        var testDataShareRequestId = new Guid("FE92F3CF-26D0-47B9-B77E-46731B83899D");

        testSubmissionDetails.DataShareRequestId = testDataShareRequestId;

        var result = testSubmissionDetails.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenASubmissionDetails_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testSubmissionDetails = new SubmissionDetails();

        testSubmissionDetails.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testSubmissionDetails.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenASubmissionDetails_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatus testRequestStatus)
    {
        var testSubmissionDetails = new SubmissionDetails();

        testSubmissionDetails.RequestStatus = testRequestStatus;

        var result = testSubmissionDetails.RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenASubmissionDetails_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testSubmissionDetails = new SubmissionDetails();

        testSubmissionDetails.EsdaName = testEsdaName;

        var result = testSubmissionDetails.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenASubmissionDetails_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testSubmissionDetails = new SubmissionDetails();

        testSubmissionDetails.AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testSubmissionDetails.AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenASubmissionDetails_WhenISetAnEmptySetOfSections_ThenSectionsIsSet()
    {
        var testSubmissionDetails = new SubmissionDetails();

        var testSections = new List<SubmissionDetailsSection>();

        testSubmissionDetails.Sections = testSections;

        var result = testSubmissionDetails.Sections;

        Assert.That(result, Is.EqualTo(testSections));
    }

    [Test]
    public void GivenASubmissionDetails_WhenISetSections_ThenSectionsIsSet()
    {
        var testSubmissionDetails = new SubmissionDetails();

        var testSections = new List<SubmissionDetailsSection> {new(), new(), new()};

        testSubmissionDetails.Sections = testSections;

        var result = testSubmissionDetails.Sections;

        Assert.That(result, Is.EqualTo(testSections));
    }

    [Test]
    public void GivenASubmissionDetails_WhenISetSubmissionReturnDetailsSet_ThenSubmissionReturnDetailsSetIsSet()
    {
        var testSubmissionDetails = new SubmissionDetails();

        var testSubmissionReturnDetailsSet = new SubmissionReturnDetailsSet();

        testSubmissionDetails.SubmissionReturnDetailsSet = testSubmissionReturnDetailsSet;

        var result = testSubmissionDetails.SubmissionReturnDetailsSet;

        Assert.That(result, Is.SameAs(testSubmissionReturnDetailsSet));
    }
}