using Agrimetrics.DataShare.Api.Logic.Services.Notification.Client;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification.Client
{
    [TestFixture]
    public class NotificationClientProxyFactoryTest
    {
        #region Create() Tests
        [Test]
        public void GivenAnEmptyGovNotifyApiKey_WhenICreate_ThenAnArgumentExceptionIsThrown(
            [Values("", "  ")] string govNotifyApiKey)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.NotificationClientProxyFactory.Create(govNotifyApiKey),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("govNotifyApiKey"));
        }

        [Test]
        public void GivenValidParameters_WhenICreate_ThenAnInstanceOfNotificationClientProxyIsReturned()
        {
            var testItems = CreateTestItems();

            const string testGovNotifyApiKey = "test_api_key-11111111-1111-1111-1111-111111111111-11111111-1111-1111-1111-111111111111";

            Assert.That(() => testItems.NotificationClientProxyFactory.Create(testGovNotifyApiKey),
                Is.Not.Null);
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var notificationClientProxyFactory = new NotificationClientProxyFactory();

            return new TestItems(
                notificationClientProxyFactory);
        }

        private class TestItems(
            INotificationClientProxyFactory notificationClientProxyFactory)
        {
            public INotificationClientProxyFactory NotificationClientProxyFactory { get; } = notificationClientProxyFactory;
        }
        #endregion
    }
}
