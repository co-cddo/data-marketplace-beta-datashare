using Agrimetrics.DataShare.Api.Logic.Services.Notification.Notifications.Core.Personalisation;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Notification.Notifications.Core.Personalisation;

[TestFixture]
public class NotificationPersonalisationItemTests
{
    [Test]
    public void GivenPropertyValues_WhenIConstructAnInstanceOfNotificationPersonalisationItem_ThenPropertiesAreSetToTheGivenValues()
    {
        var notificationPersonalisationItem = new NotificationPersonalisationItem
        {
            FieldName = "test field name",
            Value = "test value"
        };

        Assert.Multiple(() =>
        {
            Assert.That(notificationPersonalisationItem.FieldName, Is.EqualTo("test field name"));
            Assert.That(notificationPersonalisationItem.Value, Is.EqualTo("test value"));
        });
    }
}