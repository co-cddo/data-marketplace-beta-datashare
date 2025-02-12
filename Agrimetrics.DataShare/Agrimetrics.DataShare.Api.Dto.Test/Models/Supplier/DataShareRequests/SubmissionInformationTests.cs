using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionInformationTests
{
    [Test]
    public void GivenASubmissionInformation_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testSubmissionInformation = new SubmissionInformation();

        var testDataShareRequestId = new Guid("540383BE-D5EB-4073-9814-A87DD2153F11");

        testSubmissionInformation.DataShareRequestId = testDataShareRequestId;

        var result = testSubmissionInformation.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testSubmissionInformation = new SubmissionInformation();

        testSubmissionInformation.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testSubmissionInformation.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenASubmissionInformation_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatus testRequestStatus)
    {
        var testSubmissionInformation = new SubmissionInformation();

        testSubmissionInformation.RequestStatus = testRequestStatus;

        var result = testSubmissionInformation.RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testSubmissionInformation = new SubmissionInformation();

        testSubmissionInformation.EsdaName = testEsdaName;

        var result = testSubmissionInformation.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testSubmissionInformation = new SubmissionInformation();

        testSubmissionInformation.AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testSubmissionInformation.AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetAnEmptySetOfDataTypes_ThenDataTypesIsSet()
    {
        var testSubmissionInformation = new SubmissionInformation();

        var testDataTypes = new List<string>();

        testSubmissionInformation.DataTypes = testDataTypes;

        var result = testSubmissionInformation.DataTypes;

        Assert.That(result, Is.EqualTo(testDataTypes));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetDataTypes_ThenDataTypesIsSet()
    {
        var testSubmissionInformation = new SubmissionInformation();

        var testDataTypes = new List<string> {"aaa", "bbb", "ccc"};

        testSubmissionInformation.DataTypes = testDataTypes;

        var result = testSubmissionInformation.DataTypes;

        Assert.That(result, Is.EqualTo(testDataTypes));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetProjectAims_ThenProjectAimsIsSet(
        [Values("", "  ", "abc")] string testProjectAims)
    {
        var testSubmissionInformation = new SubmissionInformation();

        testSubmissionInformation.ProjectAims = testProjectAims;

        var result = testSubmissionInformation.ProjectAims;

        Assert.That(result, Is.EqualTo(testProjectAims));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetANullWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testSubmissionInformation = new SubmissionInformation();

        var testWhenNeededBy = (DateTime?) null;

        testSubmissionInformation.WhenNeededBy = testWhenNeededBy;

        var result = testSubmissionInformation.WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetWhenNeededBy_ThenWhenNeededByIsSet()
    {
        var testSubmissionInformation = new SubmissionInformation();

        var testWhenNeededBy = new DateTime(2025, 12, 25, 14, 45, 59);

        testSubmissionInformation.WhenNeededBy = testWhenNeededBy;

        var result = testSubmissionInformation.WhenNeededBy;

        Assert.That(result, Is.EqualTo(testWhenNeededBy));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetSubmittedOn_ThenSubmittedOnIsSet()
    {
        var testSubmissionInformation = new SubmissionInformation();

        var testSubmittedOn = new DateTime(2025, 12, 25, 14, 45, 59);

        testSubmissionInformation.SubmittedOn = testSubmittedOn;

        var result = testSubmissionInformation.SubmittedOn;

        Assert.That(result, Is.EqualTo(testSubmittedOn));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetAcquirerEmailAddress_ThenAcquirerEmailAddressIsSet(
        [Values("", "  ", "abc")] string testAcquirerEmailAddress)
    {
        var testSubmissionInformation = new SubmissionInformation();

        testSubmissionInformation.AcquirerEmailAddress = testAcquirerEmailAddress;

        var result = testSubmissionInformation.AcquirerEmailAddress;

        Assert.That(result, Is.EqualTo(testAcquirerEmailAddress));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetAnEmptySetOfAnswerHighlights_ThenAnswerHighlightsIsSet()
    {
        var testSubmissionInformation = new SubmissionInformation();

        var testAnswerHighlights = new List<string>();

        testSubmissionInformation.AnswerHighlights = testAnswerHighlights;

        var result = testSubmissionInformation.AnswerHighlights;

        Assert.That(result, Is.EqualTo(testAnswerHighlights));
    }

    [Test]
    public void GivenASubmissionInformation_WhenISetAnswerHighlights_ThenAnswerHighlightsIsSet()
    {
        var testSubmissionInformation = new SubmissionInformation();

        var testAnswerHighlights = new List<string> {"aaa", "bbb", "ccc"};

        testSubmissionInformation.AnswerHighlights = testAnswerHighlights;

        var result = testSubmissionInformation.AnswerHighlights;

        Assert.That(result, Is.EqualTo(testAnswerHighlights));
    }
}