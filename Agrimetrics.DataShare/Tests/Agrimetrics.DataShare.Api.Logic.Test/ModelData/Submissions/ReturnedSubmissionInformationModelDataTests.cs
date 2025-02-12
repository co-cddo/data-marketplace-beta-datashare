using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class ReturnedSubmissionInformationModelDataTests
{
    [Test]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        var testDataShareRequestId = new Guid("F8936010-F3F4-4AC4-9B05-37F835793D04");

        testReturnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestId = testDataShareRequestId;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        testReturnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatusType testRequestStatus)
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        testReturnedSubmissionInformationModelData.ReturnedSubmission_RequestStatus = testRequestStatus;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        testReturnedSubmissionInformationModelData.ReturnedSubmission_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        testReturnedSubmissionInformationModelData.ReturnedSubmission_AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        testReturnedSubmissionInformationModelData.ReturnedSubmission_EsdaName = testEsdaName;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetSubmittedOn_ThenSubmittedOnIsSet()
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        var testSubmittedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testReturnedSubmissionInformationModelData.ReturnedSubmission_SubmittedOn = testSubmittedOn;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_SubmittedOn;

        Assert.That(result, Is.EqualTo(testSubmittedOn));
    }

    [Test]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetReturnedOn_ThenReturnedOnIsSet()
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        var testReturnedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testReturnedSubmissionInformationModelData.ReturnedSubmission_ReturnedOn = testReturnedOn;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_ReturnedOn;

        Assert.That(result, Is.EqualTo(testReturnedOn));
    }

    [Test]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetANullWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        var testWhenNeededBy = (DateTime?) null;

        testReturnedSubmissionInformationModelData.ReturnedSubmission_WhenNeededBy = testWhenNeededBy;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        var testWhenNeededBy = new DateTime(2025, 12, 25, 14, 45, 59);

        testReturnedSubmissionInformationModelData.ReturnedSubmission_WhenNeededBy = testWhenNeededBy;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetSupplierNotes_ThenSupplierNotesIsSet(
        [Values("", "  ", "abc")] string testSupplierNotes)
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        testReturnedSubmissionInformationModelData.ReturnedSubmission_SupplierNotes = testSupplierNotes;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_SupplierNotes;

        Assert.That(result, Is.EqualTo(testSupplierNotes));
    }

    [Test]
    public void GivenAReturnedSubmissionInformationModelData_WhenISetFeedbackProvided_ThenFeedbackProvidedIsSet(
        [Values("", "  ", "abc")] string testFeedbackProvided)
    {
        var testReturnedSubmissionInformationModelData = new ReturnedSubmissionInformationModelData();

        testReturnedSubmissionInformationModelData.ReturnedSubmission_FeedbackProvided = testFeedbackProvided;

        var result = testReturnedSubmissionInformationModelData.ReturnedSubmission_FeedbackProvided;

        Assert.That(result, Is.EqualTo(testFeedbackProvided));
    }
}