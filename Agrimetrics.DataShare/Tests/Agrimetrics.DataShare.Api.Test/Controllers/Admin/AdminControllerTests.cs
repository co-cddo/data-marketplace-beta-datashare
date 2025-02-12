using Agrimetrics.DataShare.Api.Controllers.Admin;
using Agrimetrics.DataShare.Api.Core.Configuration.Model;
using Agrimetrics.DataShare.Api.Dto.Requests.Admin;
using Agrimetrics.DataShare.Api.Dto.Responses.Admin;
using Agrimetrics.DataShare.Api.Logic.Services.Admin;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Agrimetrics.DataShare.Api.Test.TestHelpers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Test.Controllers.Admin;

[TestFixture]
public class AdminControllerTests
{
    #region GetAllSettings() Tests
    [Test]
    public void GivenANullGetAllSettingsRequest_WhenIGetAllSettings_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AdminController.GetAllSettings(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getAllSettingsRequest"));
    }

    [Test]
    public async Task GivenAGetAllSettingsRequest_WhenIGetAllSettings_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAdminService()
    {
        var testItems = CreateTestItems();

        var testGetAllSettingsRequest = testItems.Fixture.Create<GetAllSettingsRequest>();

        var testSettingValueSet = testItems.Fixture.Create<SettingValueSet>();

        testItems.MockAdminService.Setup(x => x.GetAllSettingsAsync())
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testSettingValueSet));

        var testGetAllSettingsResponse = testItems.Fixture.Create<GetAllSettingsResponse>();

        testItems.MockAdminResponseFactory.Setup(x => x.CreateGetAllSettingsResponse(
                testGetAllSettingsRequest,
                testSettingValueSet))
            .Returns(() => testGetAllSettingsResponse);

        var result = await testItems.AdminController.GetAllSettings(testGetAllSettingsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetAllSettingsResponse));
        });
    }

    [Test]
    public async Task GivenTheAdminServiceWillReturnFailure_WhenIGetAllSettings_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetAllSettingsRequest = testItems.Fixture.Create<GetAllSettingsRequest>();

        testItems.MockAdminService.Setup(x => x.GetAllSettingsAsync())
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SettingValueSet>(success: false, error: "test error message"));

        var result = await testItems.AdminController.GetAllSettings(testGetAllSettingsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAdminServiceWillReturnFailure_WhenIGetAllSettings_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetAllSettingsRequest = testItems.Fixture.Create<GetAllSettingsRequest>();

        testItems.MockAdminService.Setup(x => x.GetAllSettingsAsync())
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SettingValueSet>(success: false, error: "test error message"));

        await testItems.AdminController.GetAllSettings(testGetAllSettingsRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetAllSettings from AdminService: test error message");
    }

    [Test]
    public async Task GivenTheAdminServiceWillThrowAnException_WhenIGetAllSettings_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetAllSettingsRequest = testItems.Fixture.Create<GetAllSettingsRequest>();

        testItems.MockAdminService.Setup(x => x.GetAllSettingsAsync())
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AdminController.GetAllSettings(testGetAllSettingsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAdminServiceWillThrowAnException_WhenIGetAllSettings_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetAllSettingsRequest = testItems.Fixture.Create<GetAllSettingsRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAdminService.Setup(x => x.GetAllSettingsAsync())
            .Throws(testException);

        await testItems.AdminController.GetAllSettings(testGetAllSettingsRequest);

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

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<AdminController>>());
        var mockAdminService = Mock.Get(fixture.Freeze<IAdminService>());
        var mockAdminResponseFactory = Mock.Get(fixture.Freeze<IAdminResponseFactory>());

        var adminController = new AdminController(
            mockLogger.Object,
            mockAdminService.Object,
            mockAdminResponseFactory.Object);

        return new TestItems(fixture, adminController,
            mockLogger, mockAdminService, mockAdminResponseFactory);
    }

    private class TestItems(
        IFixture fixture,
        AdminController adminController,
        Mock<ILogger<AdminController>> mockLogger,
        Mock<IAdminService> mockAdminService,
        Mock<IAdminResponseFactory> mockAdminResponseFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public AdminController AdminController { get; } = adminController;
        public Mock<ILogger<AdminController>> MockLogger { get; } = mockLogger;
        public Mock<IAdminService> MockAdminService { get; } = mockAdminService;
        public Mock<IAdminResponseFactory> MockAdminResponseFactory { get; } = mockAdminResponseFactory;
    }
    #endregion
}