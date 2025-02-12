using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class ReturnedDecisionSummaryModelDataTests
{
    [Test]
    public void GivenAReturnedDecisionSummaryModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testReturnedDecisionSummaryModelData = new ReturnedDecisionSummaryModelData();

        var testDataShareRequestId = new Guid("F8936010-F3F4-4AC4-9B05-37F835793D04");

        testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId = testDataShareRequestId;

        var result = testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenAReturnedDecisionSummaryModelData_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testReturnedDecisionSummaryModelData = new ReturnedDecisionSummaryModelData();

        testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Theory]
    public void GivenAReturnedDecisionSummaryModelData_WhenISetRequestStatus_ThenRequestStatusIsSet(
        DataShareRequestStatusType testRequestStatus)
    {
        var testReturnedDecisionSummaryModelData = new ReturnedDecisionSummaryModelData();

        testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_RequestStatus = testRequestStatus;

        var result = testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_RequestStatus;

        Assert.That(result, Is.EqualTo(testRequestStatus));
    }

    [Test]
    public void GivenAReturnedDecisionSummaryModelData_WhenISetAcquirerOrganisationId_ThenAcquirerOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testAcquirerOrganisationId)
    {
        var testReturnedDecisionSummaryModelData = new ReturnedDecisionSummaryModelData();

        testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationId = testAcquirerOrganisationId;

        var result = testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationId;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationId));
    }

    [Test]
    public void GivenAReturnedDecisionSummaryModelData_WhenISetAcquirerOrganisationName_ThenAcquirerOrganisationNameIsSet(
        [Values("", "  ", "abc")] string testAcquirerOrganisationName)
    {
        var testReturnedDecisionSummaryModelData = new ReturnedDecisionSummaryModelData();

        testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationName = testAcquirerOrganisationName;

        var result = testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationName;

        Assert.That(result, Is.EqualTo(testAcquirerOrganisationName));
    }
}