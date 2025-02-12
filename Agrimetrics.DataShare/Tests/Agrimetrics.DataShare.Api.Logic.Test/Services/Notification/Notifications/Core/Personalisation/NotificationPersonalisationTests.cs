using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core.Personalisation;
using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification.Notifications.Core.Personalisation
{
    [TestFixture]
    public class NotificationPersonalisationTests
    {
        [Test]
        public void GivenAnEmptySetOfPersonalisationItems_WhenIConstructAnInstanceOfNotificationPersonalisation_ThenPersonalisationItemsIsEmpty()
        {
            var notificationPersonalisation = new NotificationPersonalisation
            {
                PersonalisationItems = Enumerable.Empty<INotificationPersonalisationItem>()
            };

            Assert.That(notificationPersonalisation.PersonalisationItems, Is.Empty);
        }

        [Test]
        public void GivenASetOfPersonalisationItems_WhenIConstructAnInstanceOfNotificationPersonalisation_ThenPersonalisationItemsIsSetToTheGivenValue()
        {
            var testItems = CreateTestItems();

            var testNotificationPersonalisationItems = testItems.Fixture.CreateMany<INotificationPersonalisationItem>();

            var notificationPersonalisation = new NotificationPersonalisation
            {
                PersonalisationItems = testNotificationPersonalisationItems
            };

            Assert.That(notificationPersonalisation.PersonalisationItems, Is.EqualTo(testNotificationPersonalisationItems));
        }

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            return new TestItems(
                fixture);
        }

        private class TestItems(
            IFixture fixture)
        {
            public IFixture Fixture { get; } = fixture;
        }
        #endregion
    }
}
