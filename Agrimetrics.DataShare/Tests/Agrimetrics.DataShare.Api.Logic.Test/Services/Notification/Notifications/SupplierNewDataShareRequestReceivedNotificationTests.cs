using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification.Notifications
{
    [TestFixture]
    public class SupplierNewDataShareRequestReceivedNotificationTests
    {
        #region Construction Tests
        [Test]
        public void GivenAConfiguredNewDataShareRequestReceivedTemplateId_WhenIConstructAnInstanceOfNewDataShareRequestReceivedNotification_ThenTemplateIdIsConfiguredToTheValueInConfiguration()
        {
            var testItems = CreateTestItems();

            Mock.Get(testItems.MockNotificationConfiguration.Object.TemplateIdSet).SetupGet(x => x.SupplierNewDataShareRequestReceivedId)
                .Returns(() => Guid.Parse("ABE3A3AE-A468-4EB9-BA65-ADB5CFDF4E63"));

            var newDataShareRequestReceivedNotification = new SupplierNewDataShareRequestReceivedNotification(
                testItems.MockNotificationConfiguration.Object)
            {
                SupplierOrganisationEmailAddress = "_",
                SupplierOrganisationName = "_",
                AcquirerOrganisationName = "_",
                EsdaName = "_",
                DataMarketPlaceSignInAddress = "_"
            };

            Assert.That(newDataShareRequestReceivedNotification.TemplateId, Is.EqualTo(Guid.Parse("ABE3A3AE-A468-4EB9-BA65-ADB5CFDF4E63")));
        }

        [Test]
        public void GivenPropertyValues_WhenIConstructAnInstanceOfNewDataShareRequestReceivedNotification_ThenPropertiesAreConfiguredToTheGivenValues()
        {
            var testItems = CreateTestItems();

            var newDataShareRequestReceivedNotification = new SupplierNewDataShareRequestReceivedNotification(
                testItems.MockNotificationConfiguration.Object)
            {
                SupplierOrganisationEmailAddress = "test supplier organisation email address",
                SupplierOrganisationName = "test supplier organisation name",
                AcquirerOrganisationName = "test acquirer organisation name",
                EsdaName = "test esda name",
                DataMarketPlaceSignInAddress = "test data marketplace sign in address"
            };

            Assert.Multiple(() =>
            {
                Assert.That(newDataShareRequestReceivedNotification.SupplierOrganisationEmailAddress, Is.EqualTo("test supplier organisation email address"));
                Assert.That(newDataShareRequestReceivedNotification.AcquirerOrganisationName, Is.EqualTo("test acquirer organisation name"));
                Assert.That(newDataShareRequestReceivedNotification.SupplierOrganisationName, Is.EqualTo("test supplier organisation name"));
                Assert.That(newDataShareRequestReceivedNotification.EsdaName, Is.EqualTo("test esda name"));
                Assert.That(newDataShareRequestReceivedNotification.DataMarketPlaceSignInAddress, Is.EqualTo("test data marketplace sign in address"));
            });
        }

        [Test]
        public void GivenASupplierOrganisationEmailAddress_WhenIConstructAnInstanceOfNewDataShareRequestReceivedNotification_ThenRecipientEmailAddressIsConfiguredWithSupplierOrganisationEmailAddress()
        {
            var testItems = CreateTestItems();

            var newDataShareRequestReceivedNotification = new SupplierNewDataShareRequestReceivedNotification(
                testItems.MockNotificationConfiguration.Object)
            {
                SupplierOrganisationEmailAddress = "test supplier organisation email address",
                SupplierOrganisationName = "_",
                AcquirerOrganisationName = "_",
                EsdaName = "_",
                DataMarketPlaceSignInAddress = "_"
            };

            Assert.That(newDataShareRequestReceivedNotification.RecipientEmailAddress, Is.EqualTo("test supplier organisation email address"));
        }

        [Test]
        public void GivenPropertyValues_WhenIConstructAnInstanceOfNewDataShareRequestReceivedNotification_ThenPersonalisationIsConfiguredToTheGivenValues()
        {
            var testItems = CreateTestItems();

            var newDataShareRequestReceivedNotification = new SupplierNewDataShareRequestReceivedNotification(
                testItems.MockNotificationConfiguration.Object)
            {
                SupplierOrganisationEmailAddress = "_",
                SupplierOrganisationName = "test supplier organisation name",
                AcquirerOrganisationName = "test acquirer organisation name",
                EsdaName = "test esda name",
                DataMarketPlaceSignInAddress = "test data marketplace sign in address"
            };

            var result = newDataShareRequestReceivedNotification.Personalisation.PersonalisationItems.ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result.Any(x => x is {FieldName:"acquirer-organisation", Value:"test acquirer organisation name"}));
                Assert.That(result.Any(x => x is {FieldName:"supplier-name", Value:"test supplier organisation name"}));
                Assert.That(result.Any(x => x is {FieldName:"resource-name", Value:"test esda name"}));
                Assert.That(result.Any(x => x is {FieldName:"sign-in", Value:"[sign in](test data marketplace sign in address)"}));
            });
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockNotificationConfiguration = Mock.Get(fixture.Create<INotificationConfiguration>());

            return new TestItems(
                mockNotificationConfiguration);
        }

        private class TestItems(
            Mock<INotificationConfiguration> mockNotificationConfiguration)
        {
            public Mock<INotificationConfiguration> MockNotificationConfiguration { get; } = mockNotificationConfiguration;
        }
        #endregion
    }
}
