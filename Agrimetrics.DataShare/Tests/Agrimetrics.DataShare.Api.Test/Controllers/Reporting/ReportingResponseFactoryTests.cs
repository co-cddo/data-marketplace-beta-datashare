using Agrimetrics.DataShare.Api.Controllers.Reporting;
using Agrimetrics.DataShare.Api.Dto.Models.Reporting;
using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Test.Controllers.Reporting;

[TestFixture]
public class ReportingResponseFactoryTests
{
    #region CreateQueryDataShareRequestsCountsResponse() Tests
    [Test]
    public void GivenANullSetOfDataShareRequestCountResults_WhenICreateQueryDataShareRequestsCountsResponse_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.ReportingResponseFactory.CreateQueryDataShareRequestsCountsResponse(
                null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestCountResults"));
    }

    [Test]
    public void GivenAnEmptySetOfDataShareRequestCountResults_WhenICreateQueryDataShareRequestsCountsResponse_ThenAQueryDataShareRequestsCountsResponseIsCreatedWithAnEmptySetOfDataShareRequestCounts()
    {
        var testItems = CreateTestItems();

        var result = testItems.ReportingResponseFactory.CreateQueryDataShareRequestsCountsResponse(
            Enumerable.Empty<IDataShareRequestCount>());

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestCounts, Is.Empty);
        });
    }

    [Test]
    public void GivenANonEmptySetOfDataShareRequestCountResults_WhenICreateQueryDataShareRequestsCountsResponse_ThenAQueryDataShareRequestsCountsResponseIsCreatedWithTheGivenSetOfDataShareRequestCounts()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestCounts = testItems.Fixture.CreateMany<DataShareRequestCount>();

        var result = testItems.ReportingResponseFactory.CreateQueryDataShareRequestsCountsResponse(
            testDataShareRequestCounts);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestCounts, Is.EqualTo(testDataShareRequestCounts));
        });
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var reportingResponseFactory = new ReportingResponseFactory();

        return new TestItems(fixture, reportingResponseFactory);
    }

    private class TestItems(
        IFixture fixture,
        IReportingResponseFactory reportingResponseFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public IReportingResponseFactory ReportingResponseFactory { get; } = reportingResponseFactory;
    }
    #endregion
}