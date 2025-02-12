using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionInformationModelDataTests
{
    [Test]
    public void GivenASubmissionInformationModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        var testDataShareRequestId = new Guid("F8936010-F3F4-4AC4-9B05-37F835793D04");

        testSubmissionInformationModelData.SubmissionInformation_DataShareRequestId = testDataShareRequestId;

        var result = testSubmissionInformationModelData.SubmissionInformation_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        testSubmissionInformationModelData.SubmissionInformation_DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testSubmissionInformationModelData.SubmissionInformation_DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenASubmissionInformationModelData_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatusType testRequestStatus)
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        testSubmissionInformationModelData.SubmissionInformation_RequestStatus = testRequestStatus;

        var result = testSubmissionInformationModelData.SubmissionInformation_RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        testSubmissionInformationModelData.SubmissionInformation_EsdaName = testEsdaName;

        var result = testSubmissionInformationModelData.SubmissionInformation_EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetAcquirerUserId_ThenAcquirerUserIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerUserId)
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        testSubmissionInformationModelData.SubmissionInformation_AcquirerUserId = testAcquirerUserId;

        var result = testSubmissionInformationModelData.SubmissionInformation_AcquirerUserId;

        Assert.That(result, Is.EqualTo(testAcquirerUserId));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        testSubmissionInformationModelData.SubmissionInformation_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testSubmissionInformationModelData.SubmissionInformation_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        testSubmissionInformationModelData.SubmissionInformation_AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testSubmissionInformationModelData.SubmissionInformation_AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetAnEmptySetOfDataTypes_ThenDataTypesIsSet()
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        var testDataTypes = new List<string>();

        testSubmissionInformationModelData.SubmissionInformation_DataTypes = testDataTypes;

        var result = testSubmissionInformationModelData.SubmissionInformation_DataTypes;

        Assert.That(result, Is.EqualTo(testDataTypes));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetDataTypes_ThenDataTypesIsSet()
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        var testDataTypes = new List<string> {"aaa", "bbb", "ccc"};

        testSubmissionInformationModelData.SubmissionInformation_DataTypes = testDataTypes;

        var result = testSubmissionInformationModelData.SubmissionInformation_DataTypes;

        Assert.That(result, Is.EqualTo(testDataTypes));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetProjectAims_ThenProjectAimsIsSet(
        [Values("", "  ", "abc")] string testProjectAims)
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        testSubmissionInformationModelData.SubmissionInformation_ProjectAims = testProjectAims;

        var result = testSubmissionInformationModelData.SubmissionInformation_ProjectAims;

        Assert.That(result, Is.EqualTo(testProjectAims));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetANullWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        var testWhenNeededBy = (DateTime?) null;

        testSubmissionInformationModelData.SubmissionInformation_WhenNeededBy = testWhenNeededBy;

        var result = testSubmissionInformationModelData.SubmissionInformation_WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        var testWhenNeededBy = new DateTime(2025, 12, 25, 13, 45, 59);

        testSubmissionInformationModelData.SubmissionInformation_WhenNeededBy = testWhenNeededBy;

        var result = testSubmissionInformationModelData.SubmissionInformation_WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetSubmittedOn_ThenSubmittedOnIsSet()
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        var testSubmittedOn = new DateTime(2025, 12, 25, 13, 45, 59);

        testSubmissionInformationModelData.SubmissionInformation_SubmittedOn = testSubmittedOn;

        var result = testSubmissionInformationModelData.SubmissionInformation_SubmittedOn;

        Assert.That(result, Is.EqualTo(testSubmittedOn));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetAcquirerEmailAddress_ThenAcquirerEmailAddressIsSet(
        [Values("", "  ", "abc")] string testAcquirerEmailAddress)
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        testSubmissionInformationModelData.SubmissionInformation_AcquirerEmailAddress = testAcquirerEmailAddress;

        var result = testSubmissionInformationModelData.SubmissionInformation_AcquirerEmailAddress;

        Assert.That(result, Is.EqualTo(testAcquirerEmailAddress));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetAnEmptySetOfAnswerHighlights_ThenAnswerHighlightsIsSet()
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        var testAnswerHighlights = new List<string>();

        testSubmissionInformationModelData.SubmissionInformation_AnswerHighlights = testAnswerHighlights;

        var result = testSubmissionInformationModelData.SubmissionInformation_AnswerHighlights;

        Assert.That(result, Is.EqualTo(testAnswerHighlights));
    }

    [Test]
    public void GivenASubmissionInformationModelData_WhenISetAnswerHighlights_ThenAnswerHighlightsIsSet()
    {
        var testSubmissionInformationModelData = new SubmissionInformationModelData();

        var testAnswerHighlights = new List<string> { "aaa", "bbb", "ccc" };

        testSubmissionInformationModelData.SubmissionInformation_AnswerHighlights = testAnswerHighlights;

        var result = testSubmissionInformationModelData.SubmissionInformation_AnswerHighlights;

        Assert.That(result, Is.EqualTo(testAnswerHighlights));
    }
}