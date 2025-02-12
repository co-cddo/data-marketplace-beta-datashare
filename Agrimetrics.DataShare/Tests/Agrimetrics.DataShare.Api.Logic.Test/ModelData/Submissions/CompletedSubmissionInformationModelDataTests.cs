using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class CompletedSubmissionInformationModelDataTests
{
    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        var testDataShareRequestId = new Guid("5A41E193-937A-40BC-9DA1-423B8654C962");

        testCompletedSubmissionInformationModelData.CompletedSubmission_DataShareRequestId = testDataShareRequestId;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        testCompletedSubmissionInformationModelData.CompletedSubmission_DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenACompletedSubmissionInformationModelData_WhenISetDataShareRequestStatus_ThenDataShareRequestStatusIsSet(
        DataShareRequestStatusType testDataShareRequestStatus)
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        testCompletedSubmissionInformationModelData.CompletedSubmission_DataShareRequestStatus = testDataShareRequestStatus;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_DataShareRequestStatus;

        Assert.That(result, Is.EqualTo(testDataShareRequestStatus));
    }

    [Theory]
    public void GivenACompletedSubmissionInformationModelData_WhenISetDecision_ThenDecisionIsSet(
        SubmissionDecisionType testDecision)
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        testCompletedSubmissionInformationModelData.CompletedSubmission_Decision = testDecision;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_Decision;

        Assert.That(result, Is.EqualTo(testDecision));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetAcquirerUserId_ThenAcquirerUserIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerUserId)
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerUserId = testAcquirerUserId;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerUserId;

        Assert.That(result, Is.EqualTo(testAcquirerUserId));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetAcquirerUserEmailAddress_ThenAcquirerUserEmailAddressIsSet(
        [Values("", "  ", "abc")] string testAcquirerUserEmailAddress)
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerUserEmailAddress = testAcquirerUserEmailAddress;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerUserEmailAddress;

        Assert.That(result, Is.EqualTo(testAcquirerUserEmailAddress));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        testCompletedSubmissionInformationModelData.CompletedSubmission_EsdaName = testEsdaName;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetSubmittedOn_ThenSubmittedOnIsSet()
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        var testSubmittedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testCompletedSubmissionInformationModelData.CompletedSubmission_SubmittedOn = testSubmittedOn;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_SubmittedOn;

        Assert.That(result, Is.EqualTo(testSubmittedOn));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetCompletedOn_ThenCompletedOnIsSet()
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        var testCompletedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testCompletedSubmissionInformationModelData.CompletedSubmission_CompletedOn = testCompletedOn;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_CompletedOn;

        Assert.That(result, Is.EqualTo(testCompletedOn));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetANullWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        var testWhenNeededBy = (DateTime?) null;

        testCompletedSubmissionInformationModelData.CompletedSubmission_WhenNeededBy = testWhenNeededBy;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        var testWhenNeededBy = new DateTime(2025, 12, 25, 14, 45, 59);

        testCompletedSubmissionInformationModelData.CompletedSubmission_WhenNeededBy = testWhenNeededBy;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetSupplierNotes_ThenSupplierNotesIsSet(
        [Values("", "  ", "abc")] string testSupplierNotes)
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        testCompletedSubmissionInformationModelData.CompletedSubmission_SupplierNotes = testSupplierNotes;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_SupplierNotes;

        Assert.That(result, Is.EqualTo(testSupplierNotes));
    }

    [Test]
    public void GivenACompletedSubmissionInformationModelData_WhenISetFeedbackProvided_ThenFeedbackProvidedIsSet(
        [Values("", "  ", "abc")] string testFeedbackProvided)
    {
        var testCompletedSubmissionInformationModelData = new CompletedSubmissionInformationModelData();

        testCompletedSubmissionInformationModelData.CompletedSubmission_FeedbackProvided = testFeedbackProvided;

        var result = testCompletedSubmissionInformationModelData.CompletedSubmission_FeedbackProvided;

        Assert.That(result, Is.EqualTo(testFeedbackProvided));
    }
}