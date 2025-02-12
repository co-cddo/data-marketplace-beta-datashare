using Agrimetrics.DataShare.Api.Core.Configuration.Model;
using Agrimetrics.DataShare.Api.Db.Configuration;
using Agrimetrics.DataShare.Api.Logic.Configuration;
using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.Admin;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Configuration;
using Moq;
using System.Net;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Admin;

[TestFixture]
public class AdminServiceTests
{
    #region GetAllSettingsAsync() Tests
    [Test]
    public async Task GivenAnAdminService_WhenIGetAllSettingsAsync_ThenAllConfiguredSettingsValuesAreReturned()
    {
        var testItems = CreateTestItems();
        
        var testDatabaseConnectionSettingValues = testItems.Fixture.CreateMany<SettingValue>().ToList();
        var testNotificationSettingValues = testItems.Fixture.CreateMany<SettingValue>().ToList();
        var testUserServiceSettingValues = testItems.Fixture.CreateMany<SettingValue>().ToList();
        var testDatabaseInformationSettingValues = testItems.Fixture.CreateMany<SettingValue>().ToList();
        var testPageLinksSettingValues = testItems.Fixture.CreateMany<SettingValue>().ToList();

        testItems.MockDatabaseConnectionsConfigurationPresenter.Setup(x => x.GetAllSettingValues()).Returns(testDatabaseConnectionSettingValues);
        testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetAllSettingValues()).Returns(testNotificationSettingValues);
        testItems.MockUsersServiceConfigurationPresenter.Setup(x => x.GetAllSettings()).Returns(testUserServiceSettingValues);
        testItems.MockDataAssetInformationServiceConfigurationPresenter.Setup(x => x.GetAllSettings()).Returns(testDatabaseInformationSettingValues);
        testItems.MockPageLinksConfigurationPresenter.Setup(x => x.GetAllSettings()).Returns(testPageLinksSettingValues);

        await testItems.AdminService.GetAllSettingsAsync();

        testItems.MockServiceOperationResultFactory.Verify(x => x.CreateSuccessfulDataResult(
                It.Is<SettingValueSet>(settingValueSet =>
                    SettingsValueListsAreEqual(settingValueSet.DatabaseConnectionSettingValues, testDatabaseConnectionSettingValues) &&
                    SettingsValueListsAreEqual(settingValueSet.NotificationsSettingValues, testNotificationSettingValues) &&
                    SettingsValueListsAreEqual(settingValueSet.UserServiceSettingValues, testUserServiceSettingValues) &&
                    SettingsValueListsAreEqual(settingValueSet.DatasetInformationSettingValues, testDatabaseInformationSettingValues) &&
                    SettingsValueListsAreEqual(settingValueSet.PageLinksSettingValues, testPageLinksSettingValues)),
                It.IsAny<HttpStatusCode?>()),
            Times.Once);
    }

    private static bool SettingsValueListsAreEqual(
        IEnumerable<SettingValue> set1,
        IEnumerable<SettingValue> set2)
    {
        return set1.Select(x => x.Value)
            .OrderBy(x => x)
            .SequenceEqual(set2.Select(x => x.Value).OrderBy(x => x));
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockDatabaseConnectionsConfigurationPresenter = Mock.Get(fixture.Freeze<IDatabaseConnectionsConfigurationPresenter>());
        var mockNotificationsConfigurationPresenter = Mock.Get(fixture.Freeze<INotificationsConfigurationPresenter>());
        var mockUsersServiceConfigurationPresenter = Mock.Get(fixture.Freeze<IUsersServiceConfigurationPresenter>());
        var mockDataAssetInformationServiceConfigurationPresenter = Mock.Get(fixture.Freeze<IDataAssetInformationServiceConfigurationPresenter>());
        var mockPageLinksConfigurationPresenter = Mock.Get(fixture.Freeze<IPageLinksConfigurationPresenter>());
        var mockServiceOperationResultFactory = Mock.Get(fixture.Freeze<IServiceOperationResultFactory>());

        var adminService = new AdminService(
            mockDatabaseConnectionsConfigurationPresenter.Object,
            mockNotificationsConfigurationPresenter.Object,
            mockUsersServiceConfigurationPresenter.Object,
            mockDataAssetInformationServiceConfigurationPresenter.Object,
            mockPageLinksConfigurationPresenter.Object,
            mockServiceOperationResultFactory.Object);

        return new TestItems(
            fixture,
            adminService,
            mockDatabaseConnectionsConfigurationPresenter,
            mockNotificationsConfigurationPresenter,
            mockUsersServiceConfigurationPresenter,
            mockDataAssetInformationServiceConfigurationPresenter,
            mockPageLinksConfigurationPresenter,
            mockServiceOperationResultFactory);
    }

    private class TestItems(
        IFixture fixture,
        IAdminService adminService,
        Mock<IDatabaseConnectionsConfigurationPresenter> mockDatabaseConnectionsConfigurationPresenter,
        Mock<INotificationsConfigurationPresenter> mockNotificationsConfigurationPresenter,
        Mock<IUsersServiceConfigurationPresenter> mockUsersServiceConfigurationPresenter,
        Mock<IDataAssetInformationServiceConfigurationPresenter> mockDataAssetInformationServiceConfigurationPresenter,
        Mock<IPageLinksConfigurationPresenter> mockPageLinksConfigurationPresenter,
        Mock<IServiceOperationResultFactory> mockServiceOperationResultFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public IAdminService AdminService { get; } = adminService;
        public Mock<IDatabaseConnectionsConfigurationPresenter> MockDatabaseConnectionsConfigurationPresenter { get; } = mockDatabaseConnectionsConfigurationPresenter;
        public Mock<INotificationsConfigurationPresenter> MockNotificationsConfigurationPresenter { get; } = mockNotificationsConfigurationPresenter;
        public Mock<IUsersServiceConfigurationPresenter> MockUsersServiceConfigurationPresenter { get; } = mockUsersServiceConfigurationPresenter;
        public Mock<IDataAssetInformationServiceConfigurationPresenter> MockDataAssetInformationServiceConfigurationPresenter { get; } = mockDataAssetInformationServiceConfigurationPresenter;
        public Mock<IPageLinksConfigurationPresenter> MockPageLinksConfigurationPresenter { get; } = mockPageLinksConfigurationPresenter;
        public Mock<IServiceOperationResultFactory> MockServiceOperationResultFactory { get; } = mockServiceOperationResultFactory;
    }
    #endregion
}
