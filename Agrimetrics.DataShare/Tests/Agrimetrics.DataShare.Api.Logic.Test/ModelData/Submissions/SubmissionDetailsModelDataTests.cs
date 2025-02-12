using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionDetailsModelDataTests
{
    [Test]
    public void GivenASubmissionDetailsModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testSubmissionDetailsModelData = new SubmissionDetailsModelData();

        var testDataShareRequestId = new Guid("B9506E1F-68EE-40D4-9404-0ECE07B47E9E");

        testSubmissionDetailsModelData.SubmissionDetails_DataShareRequestId = testDataShareRequestId;

        var result = testSubmissionDetailsModelData.SubmissionDetails_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenASubmissionDetailsModelData_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testSubmissionDetailsModelData = new SubmissionDetailsModelData();

        testSubmissionDetailsModelData.SubmissionDetails_DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testSubmissionDetailsModelData.SubmissionDetails_DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenASubmissionDetailsModelData_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatusType testRequestStatus)
    {
        var testSubmissionDetailsModelData = new SubmissionDetailsModelData();

        testSubmissionDetailsModelData.SubmissionDetails_RequestStatus = testRequestStatus;

        var result = testSubmissionDetailsModelData.SubmissionDetails_RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenASubmissionDetailsModelData_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testSubmissionDetailsModelData = new SubmissionDetailsModelData();

        testSubmissionDetailsModelData.SubmissionDetails_EsdaName = testEsdaName;

        var result = testSubmissionDetailsModelData.SubmissionDetails_EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenASubmissionDetailsModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testSubmissionDetailsModelData = new SubmissionDetailsModelData();

        testSubmissionDetailsModelData.SubmissionDetails_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testSubmissionDetailsModelData.SubmissionDetails_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenASubmissionDetailsModelData_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testSubmissionDetailsModelData = new SubmissionDetailsModelData();

        testSubmissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testSubmissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }

    [Test]
    public void GivenASubmissionDetailsModelData_WhenISetAnEmptySetOfSections_ThenSectionsIsSet()
    {
        var testSubmissionDetailsModelData = new SubmissionDetailsModelData();

        var testSections = new List<SubmissionDetailsSectionModelData>();

        testSubmissionDetailsModelData.SubmissionDetails_Sections = testSections;

        var result = testSubmissionDetailsModelData.SubmissionDetails_Sections;

        Assert.That(result, Is.EqualTo(testSections));
    }

    [Test]
    public void GivenASubmissionDetailsModelData_WhenISetSections_ThenSectionsIsSet()
    {
        var testSubmissionDetailsModelData = new SubmissionDetailsModelData();

        var testSections = new List<SubmissionDetailsSectionModelData> {new(), new(), new()};

        testSubmissionDetailsModelData.SubmissionDetails_Sections = testSections;

        var result = testSubmissionDetailsModelData.SubmissionDetails_Sections;

        Assert.That(result, Is.EqualTo(testSections));
    }

    [Test]
    public void GivenASubmissionDetailsModelData_WhenISetAnEmptySetOfSubmissionReturnComments_ThenSubmissionReturnCommentsIsSet()
    {
        var testSubmissionDetailsModelData = new SubmissionDetailsModelData();

        var testSubmissionReturnComments = new List<SubmissionReturnCommentsModelData>();

        testSubmissionDetailsModelData.SubmissionDetails_SubmissionReturnComments = testSubmissionReturnComments;

        var result = testSubmissionDetailsModelData.SubmissionDetails_SubmissionReturnComments;

        Assert.That(result, Is.EqualTo(testSubmissionReturnComments));
    }

    [Test]
    public void GivenASubmissionDetailsModelData_WhenISetSubmissionReturnComments_ThenSubmissionReturnCommentsIsSet()
    {
        var testSubmissionDetailsModelData = new SubmissionDetailsModelData();

        var testSubmissionReturnComments = new List<SubmissionReturnCommentsModelData> {new(), new(), new()};

        testSubmissionDetailsModelData.SubmissionDetails_SubmissionReturnComments = testSubmissionReturnComments;

        var result = testSubmissionDetailsModelData.SubmissionDetails_SubmissionReturnComments;

        Assert.That(result, Is.EqualTo(testSubmissionReturnComments));
    }
}