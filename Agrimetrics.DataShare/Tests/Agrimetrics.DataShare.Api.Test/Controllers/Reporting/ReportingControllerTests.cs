using Agrimetrics.DataShare.Api.Controllers.Reporting;
using Agrimetrics.DataShare.Api.Dto.Requests.Reporting;
using Agrimetrics.DataShare.Api.Dto.Responses.Reporting;
using Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;
using Agrimetrics.DataShare.Api.Logic.Services.Reporting;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Agrimetrics.DataShare.Api.Test.TestHelpers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Test.Controllers.Reporting;

[TestFixture]
public class ReportingControllerTests
{
    #region QueryDataShareRequestCounts() Tests
    [Test]
    public void GivenANullQueryDataShareRequestCountsRequest_WhenIQueryDataShareRequestCounts_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.ReportingController.QueryDataShareRequestCounts(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("queryDataShareRequestCountsRequest"));
    }

    [Test]
    public async Task GivenAQueryDataShareRequestsCountsRequest_WhenIQueryDataShareRequestCounts_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheReportingService()
    {
        var testItems = CreateTestItems();

        var testQueryDataShareRequestCountsRequest = testItems.Fixture.Create<QueryDataShareRequestCountsRequest>();

        var testQueryDataShareRequestCountsResult = testItems.Fixture.Create<IQueryDataShareRequestCountsResult>();

        testItems.MockReportingService.Setup(x => x.QueryDataShareRequestCountsAsync(
                testQueryDataShareRequestCountsRequest.DataShareRequestCountQueries))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testQueryDataShareRequestCountsResult));

        var testQueryDataShareRequestsCountsResponse = testItems.Fixture.Create<QueryDataShareRequestsCountsResponse>();

        testItems.MockReportingResponseFactory.Setup(x =>
                x.CreateQueryDataShareRequestsCountsResponse(testQueryDataShareRequestCountsResult.DataShareRequestCounts))
            .Returns(() => testQueryDataShareRequestsCountsResponse);

        var result = await testItems.ReportingController.QueryDataShareRequestCounts(testQueryDataShareRequestCountsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testQueryDataShareRequestsCountsResponse));
        });
    }

    [Test]
    public async Task GivenTheReportingServiceWillReturnFailure_WhenQueryDataShareRequestCounts_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testQueryDataShareRequestCountsRequest = testItems.Fixture.Create<QueryDataShareRequestCountsRequest>();

        testItems.MockReportingService.Setup(x => x.QueryDataShareRequestCountsAsync(
                testQueryDataShareRequestCountsRequest.DataShareRequestCountQueries))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<IQueryDataShareRequestCountsResult>(success: false, error: "test error message"));

        var result = await testItems.ReportingController.QueryDataShareRequestCounts(testQueryDataShareRequestCountsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheReportingServiceWillReturnFailure_WhenQueryDataShareRequestCounts_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testQueryDataShareRequestCountsRequest = testItems.Fixture.Create<QueryDataShareRequestCountsRequest>();

        testItems.MockReportingService.Setup(x => x.QueryDataShareRequestCountsAsync(
                testQueryDataShareRequestCountsRequest.DataShareRequestCountQueries))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<IQueryDataShareRequestCountsResult>(success: false, error: "test error message"));

        await testItems.ReportingController.QueryDataShareRequestCounts(testQueryDataShareRequestCountsRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to QueryDataShareRequestCounts with ReportingService: test error message");
    }

    [Test]
    public async Task GivenTheReportingServiceWillThrowAnException_WhenQueryDataShareRequestCounts_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testQueryDataShareRequestCountsRequest = testItems.Fixture.Create<QueryDataShareRequestCountsRequest>();

        testItems.MockReportingService.Setup(x => x.QueryDataShareRequestCountsAsync(
                testQueryDataShareRequestCountsRequest.DataShareRequestCountQueries))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.ReportingController.QueryDataShareRequestCounts(testQueryDataShareRequestCountsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheReportingServiceWillThrowAnException_WhenQueryDataShareRequestCounts_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testQueryDataShareRequestCountsRequest = testItems.Fixture.Create<QueryDataShareRequestCountsRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockReportingService.Setup(x => x.QueryDataShareRequestCountsAsync(
                testQueryDataShareRequestCountsRequest.DataShareRequestCountQueries))
            .Throws(testException);

        await testItems.ReportingController.QueryDataShareRequestCounts(testQueryDataShareRequestCountsRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region Test Data Creation
    private static IServiceOperationDataResult<T> CreateTestServiceOperationDataResult<T>(
        bool? success = null,
        string? error = null,
        T? data = default)
    {
        var mockServiceOperationDataResult = new Mock<IServiceOperationDataResult<T>>();

        mockServiceOperationDataResult.SetupGet(x => x.Success).Returns(success ?? true);
        mockServiceOperationDataResult.SetupGet(x => x.Error).Returns(error);
        mockServiceOperationDataResult.SetupGet(x => x.Data).Returns(data);

        return mockServiceOperationDataResult.Object;
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<ReportingController>>());
        var mockReportingService = Mock.Get(fixture.Freeze<IReportingService>());
        var mockReportingResponseFactory = Mock.Get(fixture.Freeze<IReportingResponseFactory>());

        var reportingController = new ReportingController(
            mockLogger.Object,
            mockReportingService.Object,
            mockReportingResponseFactory.Object);

        return new TestItems(fixture, reportingController,
            mockLogger, mockReportingService, mockReportingResponseFactory);
    }

    private class TestItems(
        IFixture fixture,
        ReportingController reportingController,
        Mock<ILogger<ReportingController>> mockLogger,
        Mock<IReportingService> mockReportingService,
        Mock<IReportingResponseFactory> mockReportingResponseFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public ReportingController ReportingController { get; } = reportingController;
        public Mock<ILogger<ReportingController>> MockLogger { get; } = mockLogger;
        public Mock<IReportingService> MockReportingService { get; } = mockReportingService;
        public Mock<IReportingResponseFactory> MockReportingResponseFactory { get; } = mockReportingResponseFactory;
    }
    #endregion
}