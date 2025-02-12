using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification.Notifications.Core
{
    [TestFixture]
    public class NotificationTemplateIdSetTests
    {
        [Test]
        public void GivenAConfiguredSupplierNewDataShareRequestReceivedId_WhenIGetSupplierNewDataShareRequestReceivedId_ThenTheConfiguredTemplateIdIsReturned()
        {
            var testItems = CreateTestItems();

            var testSupplierNewDataShareRequestReceivedId = testItems.Fixture.Create<Guid>();

            testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetSupplierNewDataShareRequestReceivedTemplateId())
                .Returns(testSupplierNewDataShareRequestReceivedId);

            var result = testItems.NotificationTemplateIdSet.SupplierNewDataShareRequestReceivedId;

            Assert.That(result, Is.EqualTo(testSupplierNewDataShareRequestReceivedId));
        }

        [Test]
        public void GivenAConfiguredSupplierDataShareRequestCancelledId_WhenIGetSupplierDataShareRequestCancelledId_ThenTheConfiguredTemplateIdIsReturned()
        {
            var testItems = CreateTestItems();

            var testSupplierDataShareRequestCancelledId = testItems.Fixture.Create<Guid>();

            testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetSupplierDataShareRequestCancelledTemplateId())
                .Returns(testSupplierDataShareRequestCancelledId);

            var result = testItems.NotificationTemplateIdSet.SupplierDataShareRequestCancelledId;

            Assert.That(result, Is.EqualTo(testSupplierDataShareRequestCancelledId));
        }

        [Test]
        public void GivenAConfiguredAcquirerDataShareRequestAcceptedId_WhenIGetAcquirerDataShareRequestAcceptedId_ThenTheConfiguredTemplateIdIsReturned()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestAcceptedId = testItems.Fixture.Create<Guid>();

            testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetAcquirerDataShareRequestAcceptedTemplateId())
                .Returns(testAcquirerDataShareRequestAcceptedId);

            var result = testItems.NotificationTemplateIdSet.AcquirerDataShareRequestAcceptedId;

            Assert.That(result, Is.EqualTo(testAcquirerDataShareRequestAcceptedId));
        }

        [Test]
        public void GivenAConfiguredAcquirerDataShareRequestRejectedId_WhenIGetAcquirerDataShareRequestRejectedId_ThenTheConfiguredTemplateIdIsReturned()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestRejectedId = testItems.Fixture.Create<Guid>();

            testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetAcquirerDataShareRequestRejectedTemplateId())
                .Returns(testAcquirerDataShareRequestRejectedId);

            var result = testItems.NotificationTemplateIdSet.AcquirerDataShareRequestRejectedId;

            Assert.That(result, Is.EqualTo(testAcquirerDataShareRequestRejectedId));
        }

        [Test]
        public void GivenAConfiguredAcquirerDataShareRequestReturnedWithCommentsId_WhenIGetAcquirerDataShareRequestReturnedWithCommentsId_ThenTheConfiguredTemplateIdIsReturned()
        {
            var testItems = CreateTestItems();

            var testAcquirerDataShareRequestReturnedWithCommentsId = testItems.Fixture.Create<Guid>();

            testItems.MockNotificationsConfigurationPresenter.Setup(x => x.GetAcquirerDataShareRequestReturnedWithCommentsTemplateId())
                .Returns(testAcquirerDataShareRequestReturnedWithCommentsId);

            var result = testItems.NotificationTemplateIdSet.AcquirerDataShareRequestReturnedWithCommentsId;

            Assert.That(result, Is.EqualTo(testAcquirerDataShareRequestReturnedWithCommentsId));
        }
        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            
            var mockNotificationsConfigurationPresenter = Mock.Get(fixture.Create<INotificationsConfigurationPresenter>());

            var notificationTemplateIdSet = new NotificationTemplateIdSet(
                    mockNotificationsConfigurationPresenter.Object);

            return new TestItems(
                fixture,
                notificationTemplateIdSet,
                mockNotificationsConfigurationPresenter);
        }

        private class TestItems(
            IFixture fixture,
            INotificationTemplateIdSet notificationTemplateIdSet,
            Mock<INotificationsConfigurationPresenter> mockNotificationsConfigurationPresenter)
        {
            public IFixture Fixture { get; } = fixture;
            public INotificationTemplateIdSet NotificationTemplateIdSet { get; } = notificationTemplateIdSet;
            public Mock<INotificationsConfigurationPresenter> MockNotificationsConfigurationPresenter { get; } = mockNotificationsConfigurationPresenter;
        }
        #endregion
    }
}
