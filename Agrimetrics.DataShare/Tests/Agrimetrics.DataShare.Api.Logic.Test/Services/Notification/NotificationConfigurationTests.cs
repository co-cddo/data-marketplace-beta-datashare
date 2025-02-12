using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification;

[TestFixture]
public class NotificationConfigurationTests
{
    [Test]
    public void GivenADependencyNotificationsConfigurationPresenter_WhenIQueryGovNotifyApiKey_ThenTheKeyProvidedTheConfigurationPresenterIsReturned()
    {
        var testItems = CreateTestItems();

        var testGovNotifyApiKey = testItems.Fixture.Create<string>();

        testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetGovNotifyApiKey())
            .Returns(testGovNotifyApiKey);

        var result = testItems.NotificationConfiguration.GovNotifyApiKey;

        Assert.That(result, Is.EqualTo(testGovNotifyApiKey));
    }

    [Test]
    public void GivenADependencyNotificationTemplateIdSet_WhenIQueryNotificationTemplateIdSet_ThenTheDependencyNotificationTemplateIdSetIsReturned()
    {
        var testItems = CreateTestItems();

        var result = testItems.NotificationConfiguration.TemplateIdSet;

        Assert.That(result, Is.SameAs(testItems.MockNotificationTemplateIdSet.Object));
    }

    #region Test Items
    private static TestItems CreateTestItems()
    { 
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockNotificationTemplateIdSet = Mock.Get(fixture.Create<INotificationTemplateIdSet>());
        var mockNotificationsConfigurationPresenter = Mock.Get(fixture.Create<INotificationsConfigurationPresenter>());

        var notificationConfiguration = new NotificationConfiguration(
            mockNotificationTemplateIdSet.Object,
            mockNotificationsConfigurationPresenter.Object);

        return new TestItems(
            fixture,
            notificationConfiguration,
            mockNotificationTemplateIdSet,
            mockNotificationsConfigurationPresenter);
    }

    private class TestItems(
        IFixture fixture,
        INotificationConfiguration notificationConfiguration,
        Mock<INotificationTemplateIdSet> mockNotificationTemplateIdSet,
        Mock<INotificationsConfigurationPresenter> mockNotificationsConfigurationPresenter)
    {
        public IFixture Fixture { get; } = fixture;
        public INotificationConfiguration NotificationConfiguration { get; } = notificationConfiguration;
        public Mock<INotificationTemplateIdSet> MockNotificationTemplateIdSet { get; } = mockNotificationTemplateIdSet;
        public Mock<INotificationsConfigurationPresenter> MockNotificationsConfigurationPresenter { get; } = mockNotificationsConfigurationPresenter;
    }

    #endregion
}