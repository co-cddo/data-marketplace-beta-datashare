using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Reporting;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Reporting;

[TestFixture]
public class DataShareRequestCountQueryTests
{
    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetId_ThenIdIsSet(
        [Values(-1, 0, 999)] int testId)
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var result = testDataShareRequestCount.Id = testId;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetAnEmptySetOfCurrentStatuses_ThenCurrentStatusesIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testCurrentStatuses = new List<DataShareRequestStatus>();

        var result = testDataShareRequestCount.CurrentStatuses = testCurrentStatuses;

        Assert.That(result, Is.EqualTo(testCurrentStatuses));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetCurrentStatuses_ThenCurrentStatusesIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testCurrentStatuses = Enum.GetValues<DataShareRequestStatus>();

        var result = testDataShareRequestCount.CurrentStatuses = testCurrentStatuses;

        Assert.That(result, Is.EqualTo(testCurrentStatuses));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetAnEmptySetOfIntermediateStatuses_ThenIntermediateStatusesIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testIntermediateStatuses = new List<DataShareRequestStatus>();

        var result = testDataShareRequestCount.IntermediateStatuses = testIntermediateStatuses;

        Assert.That(result, Is.EqualTo(testIntermediateStatuses));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetIntermediateStatuses_ThenIntermediateStatusesIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testIntermediateStatuses = Enum.GetValues<DataShareRequestStatus>();

        var result = testDataShareRequestCount.IntermediateStatuses = testIntermediateStatuses;

        Assert.That(result, Is.EqualTo(testIntermediateStatuses));
    }

    [Theory]
    public void GivenADataShareRequestCountQuery_WhenISetUseOnlyTheMostRecentPeriodSpentInIntermediateStatuses_ThenUseOnlyTheMostRecentPeriodSpentInIntermediateStatusesIsSet(
        bool testUseOnlyTheMostRecentPeriodSpentInIntermediateStatuses)
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var result = testDataShareRequestCount.UseOnlyTheMostRecentPeriodSpentInIntermediateStatuses = testUseOnlyTheMostRecentPeriodSpentInIntermediateStatuses;

        Assert.That(result, Is.EqualTo(testUseOnlyTheMostRecentPeriodSpentInIntermediateStatuses));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetANullMinimumDuration_ThenMinimumDurationIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testMinimumDuration = (TimeSpan?) null;

        var result = testDataShareRequestCount.MinimumDuration = testMinimumDuration;

        Assert.That(result, Is.EqualTo(testMinimumDuration));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetMinimumDuration_ThenMinimumDurationIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testMinimumDuration = new TimeSpan();

        var result = testDataShareRequestCount.MinimumDuration = testMinimumDuration;

        Assert.That(result, Is.EqualTo(testMinimumDuration));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetANullMaximumDuration_ThenMaximumDurationIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testMaximumDuration = (TimeSpan?)null;

        var result = testDataShareRequestCount.MaximumDuration = testMaximumDuration;

        Assert.That(result, Is.EqualTo(testMaximumDuration));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetMaximumDuration_ThenMaximumDurationIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testMaximumDuration = new TimeSpan();

        var result = testDataShareRequestCount.MaximumDuration = testMaximumDuration;

        Assert.That(result, Is.EqualTo(testMaximumDuration));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetANullFrom_ThenFromIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testFrom = (DateTime?) null;

        var result = testDataShareRequestCount.From = testFrom;

        Assert.That(result, Is.EqualTo(testFrom));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetFrom_ThenFromIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testFrom = new DateTime(2025, 12, 25, 14, 45, 59);

        var result = testDataShareRequestCount.From = testFrom;

        Assert.That(result, Is.EqualTo(testFrom));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetANullTo_ThenToIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testTo = (DateTime?)null;

        var result = testDataShareRequestCount.To = testTo;

        Assert.That(result, Is.EqualTo(testTo));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetTo_ThenToIsSet()
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var testTo = new DateTime(2025, 12, 25, 14, 45, 59);

        var result = testDataShareRequestCount.To = testTo;

        Assert.That(result, Is.EqualTo(testTo));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetPublisherOrganisationId_ThenPublisherOrganisationIdIsSet(
        [Values(null, -1, 0, 999)] int? testPublisherOrganisationId)
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var result = testDataShareRequestCount.PublisherOrganisationId = testPublisherOrganisationId;

        Assert.That(result, Is.EqualTo(testPublisherOrganisationId));
    }

    [Test]
    public void GivenADataShareRequestCountQuery_WhenISetPublisherDomainId_ThenPublisherDomainIdIsSet(
        [Values(null, -1, 0, 999)] int? testPublisherDomainId)
    {
        var testDataShareRequestCount = new DataShareRequestCountQuery();

        var result = testDataShareRequestCount.PublisherDomainId = testPublisherDomainId;

        Assert.That(result, Is.EqualTo(testPublisherDomainId));
    }
}