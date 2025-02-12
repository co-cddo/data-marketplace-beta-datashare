using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestNotificationRecipientDeterminations;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AcquirerDataShareRequest.DataShareRequestNotificationRecipientDeterminations
{
    [TestFixture]
    public class DataShareRequestNotificationRecipientTests
    {
        [Test]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase("test email address")]
        public void GivenAnyEmailAddress_WhenIConstructAnInstanceOfDataShareRequestNotificationRecipient_ThenEmailAddressIsConfiguredToTheGivenValue(
            string testRecipientName)
        {
            var dataShareRequestNotificationRecipient = new DataShareRequestNotificationRecipient
            {
                EmailAddress = testRecipientName,
                RecipientName = It.IsAny<string>()
            };

            Assert.That(dataShareRequestNotificationRecipient.EmailAddress, Is.EqualTo(testRecipientName));
        }

        [Test]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase("test recipient name")]
        public void GivenAnyRecipientName_WhenIConstructAnInstanceOfDataShareRequestNotificationRecipient_ThenRecipientNameIsConfiguredToTheGivenValue(
            string testRecipientName)
        {
            var dataShareRequestNotificationRecipient = new DataShareRequestNotificationRecipient
            {
                EmailAddress = It.IsAny<string>(),
                RecipientName = testRecipientName
            };

            Assert.That(dataShareRequestNotificationRecipient.RecipientName, Is.EqualTo(testRecipientName));
        }
    }
}
