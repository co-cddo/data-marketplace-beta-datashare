using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Reporting;

[TestFixture]
public class ReportingDataShareRequestInformationModelDataTests
{
    [Test]
    public void GivenAReportingDataShareRequestInformationModelData_WhenISetId_ThenIdIsSet()
    {
        var testReportingDataShareRequestInformationModelData = new ReportingDataShareRequestInformationModelData();

        var testId = new Guid("A5A015DF-29A7-40A6-8428-0C9680D8ED87");

        testReportingDataShareRequestInformationModelData.DataShareRequest_Id = testId;

        var result = testReportingDataShareRequestInformationModelData.DataShareRequest_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAReportingDataShareRequestInformationModelData_WhenISetRequestId_ThenRequestIdIsSet(
        [Values("", "  ", "abc")] string testRequestId)
    {
        var testReportingDataShareRequestInformationModelData = new ReportingDataShareRequestInformationModelData();

        testReportingDataShareRequestInformationModelData.DataShareRequest_RequestId = testRequestId;

        var result = testReportingDataShareRequestInformationModelData.DataShareRequest_RequestId;

        Assert.That(result, Is.EqualTo(testRequestId));
    }

    [Theory]
    public void GivenAReportingDataShareRequestInformationModelData_WhenISetCurrentStatus_ThenCurrentStatusIsSet(
        DataShareRequestStatusType testCurrentStatus)
    {
        var testReportingDataShareRequestInformationModelData = new ReportingDataShareRequestInformationModelData();

        testReportingDataShareRequestInformationModelData.DataShareRequest_CurrentStatus = testCurrentStatus;

        var result = testReportingDataShareRequestInformationModelData.DataShareRequest_CurrentStatus;

        Assert.That(result, Is.EqualTo(testCurrentStatus));
    }

    [Test]
    public void GivenAReportingDataShareRequestInformationModelData_WhenISetPublisherOrganisationId_ThenPublisherOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testPublisherOrganisationId)
    {
        var testReportingDataShareRequestInformationModelData = new ReportingDataShareRequestInformationModelData();

        testReportingDataShareRequestInformationModelData.DataShareRequest_PublisherOrganisationId = testPublisherOrganisationId;

        var result = testReportingDataShareRequestInformationModelData.DataShareRequest_PublisherOrganisationId;

        Assert.That(result, Is.EqualTo(testPublisherOrganisationId));
    }

    [Test]
    public void GivenAReportingDataShareRequestInformationModelData_WhenISetPublisherDomainId_ThenPublisherDomainIdIsSet(
        [Values(-1, 0, 999)] int testPublisherDomainId)
    {
        var testReportingDataShareRequestInformationModelData = new ReportingDataShareRequestInformationModelData();

        testReportingDataShareRequestInformationModelData.DataShareRequest_PublisherDomainId = testPublisherDomainId;

        var result = testReportingDataShareRequestInformationModelData.DataShareRequest_PublisherDomainId;

        Assert.That(result, Is.EqualTo(testPublisherDomainId));
    }

    [Test]
    public void GivenAReportingDataShareRequestInformationModelData_WhenISetAnEmptySetOfStatuses_ThenStatusesIsSet()
    {
        var testReportingDataShareRequestInformationModelData = new ReportingDataShareRequestInformationModelData();

        var testStatuses = new List<ReportingDataShareRequestStatusModelData>();

        testReportingDataShareRequestInformationModelData.DataShareRequest_Statuses = testStatuses;

        var result = testReportingDataShareRequestInformationModelData.DataShareRequest_Statuses;

        Assert.That(result, Is.EqualTo(testStatuses));
    }

    [Test]
    public void GivenAReportingDataShareRequestInformationModelData_WhenISetStatuses_ThenStatusesIsSet()
    {
        var testReportingDataShareRequestInformationModelData = new ReportingDataShareRequestInformationModelData();

        var testStatuses = new List<ReportingDataShareRequestStatusModelData> {new(), new(), new()};

        testReportingDataShareRequestInformationModelData.DataShareRequest_Statuses = testStatuses;

        var result = testReportingDataShareRequestInformationModelData.DataShareRequest_Statuses;

        Assert.That(result, Is.EqualTo(testStatuses));
    }
}