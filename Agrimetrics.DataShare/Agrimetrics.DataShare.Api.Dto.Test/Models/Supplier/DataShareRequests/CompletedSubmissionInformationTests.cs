using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class CompletedSubmissionInformationTests
{
    [Test]
    public void GivenACompletedSubmissionInformation_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        var testDataShareRequestId = new Guid("80372CFC-285F-489A-965C-F75C5164B2EE");

        testCompletedSubmissionInformation.DataShareRequestId = testDataShareRequestId;

        var result = testCompletedSubmissionInformation.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenACompletedSubmissionInformation_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        testCompletedSubmissionInformation.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testCompletedSubmissionInformation.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenACompletedSubmissionInformation_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatus testRequestStatus)
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        testCompletedSubmissionInformation.RequestStatus = testRequestStatus;

        var result = testCompletedSubmissionInformation.RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Theory]
    public void GivenACompletedSubmissionInformation_WhenISetDecision_ThenDecisionIsSet(
        SubmissionDecision testDecision)
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        testCompletedSubmissionInformation.Decision = testDecision;

        var result = testCompletedSubmissionInformation.Decision;

        Assert.That(result, Is.EqualTo(testDecision));
    }

    [Test]
    public void GivenACompletedSubmissionInformation_WhenISetAcquirerUserEmail_ThenAcquirerUserEmailIsSet(
        [Values("", "  ", "abc")] string testAcquirerUserEmail)
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        testCompletedSubmissionInformation.AcquirerUserEmail = testAcquirerUserEmail;

        var result = testCompletedSubmissionInformation.AcquirerUserEmail;

        Assert.That(result, Is.EqualTo(testAcquirerUserEmail));
    }

    [Test]
    public void GivenACompletedSubmissionInformation_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        testCompletedSubmissionInformation.AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testCompletedSubmissionInformation.AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenACompletedSubmissionInformation_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        testCompletedSubmissionInformation.EsdaName = testEsdaName;

        var result = testCompletedSubmissionInformation.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenACompletedSubmissionInformation_WhenISetSubmittedOn_ThenSubmittedOnIsSet()
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        var testSubmittedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testCompletedSubmissionInformation.SubmittedOn = testSubmittedOn;

        var result = testCompletedSubmissionInformation.SubmittedOn;

        Assert.That(result, Is.EqualTo(testSubmittedOn));
    }

    [Test]
    public void GivenACompletedSubmissionInformation_WhenISetCompletedOn_ThenCompletedOnIsSet()
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        var testCompletedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testCompletedSubmissionInformation.CompletedOn = testCompletedOn;

        var result = testCompletedSubmissionInformation.CompletedOn;

        Assert.That(result, Is.EqualTo(testCompletedOn));
    }

    [Test]
    public void GivenACompletedSubmissionInformation_WhenISetANullWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        var testWhenNeededBy = (DateTime?) null;

        testCompletedSubmissionInformation.WhenNeededBy = testWhenNeededBy;

        var result = testCompletedSubmissionInformation.WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenACompletedSubmissionInformation_WhenISetWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        var testWhenNeededBy = new DateTime(2025, 12, 25, 14, 45, 59);

        testCompletedSubmissionInformation.WhenNeededBy = testWhenNeededBy;

        var result = testCompletedSubmissionInformation.WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenACompletedSubmissionInformation_WhenISetSupplierNotes_ThenSupplierNotesIsSet(
        [Values("", "  ", "abc")] string testSupplierNotes)
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        testCompletedSubmissionInformation.SupplierNotes = testSupplierNotes;

        var result = testCompletedSubmissionInformation.SupplierNotes;

        Assert.That(result, Is.EqualTo(testSupplierNotes));
    }

    [Test]
    public void GivenACompletedSubmissionInformation_WhenISetFeedbackProvided_ThenFeedbackProvidedIsSet(
        [Values("", "  ", "abc")] string testFeedbackProvided)
    {
        var testCompletedSubmissionInformation = new CompletedSubmissionInformation();

        testCompletedSubmissionInformation.FeedbackProvided = testFeedbackProvided;

        var result = testCompletedSubmissionInformation.FeedbackProvided;

        Assert.That(result, Is.EqualTo(testFeedbackProvided));
    }
}

