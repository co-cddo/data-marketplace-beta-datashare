using Agrimetrics.DataShare.Api.Controllers.AuditLogs;
using Agrimetrics.DataShare.Api.Dto.Requests.AuditLogs;
using Agrimetrics.DataShare.Api.Dto.Responses.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.Services.AuditLog;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Agrimetrics.DataShare.Api.Test.TestHelpers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Test.Controllers.AuditLogs;

[TestFixture]
public class AuditLogControllerTests
{
    #region GetDataShareRequestAuditLog() Tests
    [Test]
    public void GivenANullGetDataShareRequestAuditLogRequest_WhenIGetDataShareRequestAuditLog_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AuditLogController.GetDataShareRequestAuditLog(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestAuditLogRequest"));
    }

    [Test]
    public async Task GivenAGetSubmissionSummariesRequest_WhenIGetDataShareRequestAuditLog_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAuditLogService()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAuditLogRequest = testItems.Fixture.Create<GetDataShareRequestAuditLogRequest>();

        var testAuditLogDataShareRequestStatusChangesResult = testItems.Fixture.Create<IAuditLogDataShareRequestStatusChangesResult>();

        testItems.MockAuditLogService.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                testGetDataShareRequestAuditLogRequest.DataShareRequestId,
                testGetDataShareRequestAuditLogRequest.ToStatuses))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testAuditLogDataShareRequestStatusChangesResult));

        var testGetDataShareRequestAuditLogResponse = testItems.Fixture.Create<GetDataShareRequestAuditLogResponse>();

        testItems.MockAuditLogResponseFactory.Setup(x => x.CreateGetDataShareRequestAuditLogResponse(
                testGetDataShareRequestAuditLogRequest,
                testAuditLogDataShareRequestStatusChangesResult))
            .Returns(() => testGetDataShareRequestAuditLogResponse);

        var result = await testItems.AuditLogController.GetDataShareRequestAuditLog(testGetDataShareRequestAuditLogRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetDataShareRequestAuditLogResponse));
        });
    }

    [Test]
    public async Task GivenTheAuditLogServiceWillReturnFailure_WhenIGetDataShareRequestAuditLog_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAuditLogRequest = testItems.Fixture.Create<GetDataShareRequestAuditLogRequest>();

        testItems.MockAuditLogService.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                testGetDataShareRequestAuditLogRequest.DataShareRequestId,
                testGetDataShareRequestAuditLogRequest.ToStatuses))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<IAuditLogDataShareRequestStatusChangesResult>(success: false, error: "test error message"));

        var result = await testItems.AuditLogController.GetDataShareRequestAuditLog(testGetDataShareRequestAuditLogRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAuditLogServiceWillReturnFailure_WhenIGetDataShareRequestAuditLog_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAuditLogRequest = testItems.Fixture.Create<GetDataShareRequestAuditLogRequest>();

        testItems.MockAuditLogService.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                testGetDataShareRequestAuditLogRequest.DataShareRequestId,
                testGetDataShareRequestAuditLogRequest.ToStatuses))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<IAuditLogDataShareRequestStatusChangesResult>(success: false, error: "test error message"));

        await testItems.AuditLogController.GetDataShareRequestAuditLog(testGetDataShareRequestAuditLogRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetAuditLogsForDataShareRequestStatusChangeToStatus from AuditLogService: test error message");
    }

    [Test]
    public async Task GivenTheAuditLogServiceWillThrowAnException_WhenIGetSubmissionSummaries_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAuditLogRequest = testItems.Fixture.Create<GetDataShareRequestAuditLogRequest>();

        testItems.MockAuditLogService.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                testGetDataShareRequestAuditLogRequest.DataShareRequestId,
                testGetDataShareRequestAuditLogRequest.ToStatuses))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AuditLogController.GetDataShareRequestAuditLog(testGetDataShareRequestAuditLogRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAuditLogServiceWillThrowAnException_WhenIGetDataShareRequestAuditLog_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAuditLogRequest = testItems.Fixture.Create<GetDataShareRequestAuditLogRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAuditLogService.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                testGetDataShareRequestAuditLogRequest.DataShareRequestId,
                testGetDataShareRequestAuditLogRequest.ToStatuses))
            .Throws(testException);

        await testItems.AuditLogController.GetDataShareRequestAuditLog(testGetDataShareRequestAuditLogRequest);

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

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<AuditLogController>>());
        var mockAuditLogService = Mock.Get(fixture.Freeze<IAuditLogService>());
        var mockAuditLogResponseFactory = Mock.Get(fixture.Freeze<IAuditLogResponseFactory>());

        var auditLogController = new AuditLogController(
            mockLogger.Object,
            mockAuditLogService.Object,
            mockAuditLogResponseFactory.Object);

        return new TestItems(fixture, auditLogController,
            mockLogger, mockAuditLogService, mockAuditLogResponseFactory);
    }

    private class TestItems(
        IFixture fixture,
        AuditLogController auditLogController,
        Mock<ILogger<AuditLogController>> mockLogger,
        Mock<IAuditLogService> mockAuditLogService,
        Mock<IAuditLogResponseFactory> mockAuditLogResponseFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public AuditLogController AuditLogController { get; } = auditLogController;
        public Mock<ILogger<AuditLogController>> MockLogger { get; } = mockLogger;
        public Mock<IAuditLogService> MockAuditLogService { get; } = mockAuditLogService;
        public Mock<IAuditLogResponseFactory> MockAuditLogResponseFactory { get; } = mockAuditLogResponseFactory;
    }
    #endregion
}