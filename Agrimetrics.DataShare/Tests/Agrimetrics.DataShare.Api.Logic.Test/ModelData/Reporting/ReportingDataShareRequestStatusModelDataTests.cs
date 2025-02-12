using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Reporting;

[TestFixture]
public class ReportingDataShareRequestStatusModelDataTests
{
    [Test]
    public void GivenAReportingDataShareRequestStatusModelData_WhenISetId_ThenIdIsSet()
    {
        var testReportingDataShareRequestStatusModelData = new ReportingDataShareRequestStatusModelData();

        var testId = new Guid("7E502CF6-BA1F-4377-968A-0795F9810C46");

        testReportingDataShareRequestStatusModelData.Status_Id = testId;

        var result = testReportingDataShareRequestStatusModelData.Status_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAReportingDataShareRequestStatusModelData_WhenISetOrder_ThenOrderIsSet(
        [Values(-1, 0, 999)] int testOrder)
    {
        var testReportingDataShareRequestStatusModelData = new ReportingDataShareRequestStatusModelData();

        testReportingDataShareRequestStatusModelData.Status_Order = testOrder;

        var result = testReportingDataShareRequestStatusModelData.Status_Order;

        Assert.That(result, Is.EqualTo(testOrder));
    }

    [Theory]
    public void GivenAReportingDataShareRequestStatusModelData_WhenISetStatus_ThenStatusIsSet(
        DataShareRequestStatusType testStatus)
    {
        var testReportingDataShareRequestStatusModelData = new ReportingDataShareRequestStatusModelData();

        testReportingDataShareRequestStatusModelData.Status_Status = testStatus;

        var result = testReportingDataShareRequestStatusModelData.Status_Status;

        Assert.That(result, Is.EqualTo(testStatus));
    }

    [Test]
    public void GivenAReportingDataShareRequestStatusModelData_WhenISetEnteredAtUtc_ThenEnteredAtUtcIsSet()
    {
        var testReportingDataShareRequestStatusModelData = new ReportingDataShareRequestStatusModelData();

        var testEnteredAtUtc = new DateTime(2025, 12, 25, 14, 45, 59);

        testReportingDataShareRequestStatusModelData.Status_EnteredAtUtc = testEnteredAtUtc;

        var result = testReportingDataShareRequestStatusModelData.Status_EnteredAtUtc;

        Assert.That(result, Is.EqualTo(testEnteredAtUtc));
    }

    [Test]
    public void GivenAReportingDataShareRequestStatusModelData_WhenISetANullLeftAtUtc_ThenLeftAtUtcIsSet()
    {
        var testReportingDataShareRequestStatusModelData = new ReportingDataShareRequestStatusModelData();

        var testLeftAtUtc = (DateTime?) null;

        testReportingDataShareRequestStatusModelData.Status_LeftAtUtc = testLeftAtUtc;

        var result = testReportingDataShareRequestStatusModelData.Status_LeftAtUtc;

        Assert.That(result, Is.EqualTo(testLeftAtUtc));
    }

    [Test]
    public void GivenAReportingDataShareRequestStatusModelData_WhenISetLeftAtUtc_ThenLeftAtUtcIsSet()
    {
        var testReportingDataShareRequestStatusModelData = new ReportingDataShareRequestStatusModelData();

        var testLeftAtUtc = new DateTime(2025, 12, 25, 14, 45, 59);

        testReportingDataShareRequestStatusModelData.Status_LeftAtUtc = testLeftAtUtc;

        var result = testReportingDataShareRequestStatusModelData.Status_LeftAtUtc;

        Assert.That(result, Is.EqualTo(testLeftAtUtc));
    }
}