using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class ReturnedSubmissionInformationTests
{
    [Test]
    public void GivenAReturnedSubmissionInformation_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testReturnedSubmissionInformation = new ReturnedSubmissionInformation();

        var testDataShareRequestId = new Guid("E7C78424-E02D-44E0-817D-DA92DD37453E");

        testReturnedSubmissionInformation.DataShareRequestId = testDataShareRequestId;

        var result = testReturnedSubmissionInformation.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenAReturnedSubmissionInformation_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testReturnedSubmissionInformation = new ReturnedSubmissionInformation();

        testReturnedSubmissionInformation.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testReturnedSubmissionInformation.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenAReturnedSubmissionInformation_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatus testRequestStatus)
    {
        var testReturnedSubmissionInformation = new ReturnedSubmissionInformation();

        testReturnedSubmissionInformation.RequestStatus = testRequestStatus;

        var result = testReturnedSubmissionInformation.RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenAReturnedSubmissionInformation_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testReturnedSubmissionInformation = new ReturnedSubmissionInformation();

        testReturnedSubmissionInformation.AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testReturnedSubmissionInformation.AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenAReturnedSubmissionInformation_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testReturnedSubmissionInformation = new ReturnedSubmissionInformation();

        testReturnedSubmissionInformation.EsdaName = testEsdaName;

        var result = testReturnedSubmissionInformation.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenAReturnedSubmissionInformation_WhenISetSubmittedOn_ThenSubmittedOnIsSet()
    {
        var testReturnedSubmissionInformation = new ReturnedSubmissionInformation();

        var testSubmittedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testReturnedSubmissionInformation.SubmittedOn = testSubmittedOn;

        var result = testReturnedSubmissionInformation.SubmittedOn;

        Assert.That(result, Is.EqualTo(testSubmittedOn));
    }

    [Test]
    public void GivenAReturnedSubmissionInformation_WhenISetReturnedOn_ThenReturnedOnIsSet()
    {
        var testReturnedSubmissionInformation = new ReturnedSubmissionInformation();

        var testReturnedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testReturnedSubmissionInformation.ReturnedOn = testReturnedOn;

        var result = testReturnedSubmissionInformation.ReturnedOn;

        Assert.That(result, Is.EqualTo(testReturnedOn));
    }

    [Test]
    public void GivenAReturnedSubmissionInformation_WhenISetANullWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testReturnedSubmissionInformation = new ReturnedSubmissionInformation();

        var testWhenNeededBy = (DateTime?) null;

        testReturnedSubmissionInformation.WhenNeededBy = testWhenNeededBy;

        var result = testReturnedSubmissionInformation.WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenAReturnedSubmissionInformation_WhenISetWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testReturnedSubmissionInformation = new ReturnedSubmissionInformation();

        var testWhenNeededBy = new DateTime(2025, 12, 25, 14, 45, 59);

        testReturnedSubmissionInformation.WhenNeededBy = testWhenNeededBy;

        var result = testReturnedSubmissionInformation.WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenAReturnedSubmissionInformation_WhenISetSupplierNotes_ThenSupplierNotesIsSet(
        [Values("", "  ", "abc")] string testSupplierNotes)
    {
        var testReturnedSubmissionInformation = new ReturnedSubmissionInformation();

        testReturnedSubmissionInformation.SupplierNotes = testSupplierNotes;

        var result = testReturnedSubmissionInformation.SupplierNotes;

        Assert.That(result, Is.EqualTo(testSupplierNotes));
    }

    [Test]
    public void GivenAReturnedSubmissionInformation_WhenISetFeedbackProvided_ThenFeedbackProvidedIsSet(
        [Values("", "  ", "abc")] string testFeedbackProvided)
    {
        var testReturnedSubmissionInformation = new ReturnedSubmissionInformation();

        testReturnedSubmissionInformation.FeedbackProvided = testFeedbackProvided;

        var result = testReturnedSubmissionInformation.FeedbackProvided;

        Assert.That(result, Is.EqualTo(testFeedbackProvided));
    }
}