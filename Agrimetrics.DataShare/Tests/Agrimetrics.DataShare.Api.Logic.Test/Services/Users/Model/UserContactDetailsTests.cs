using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Users.Model;

[TestFixture]
public class UserContactDetailsTests
{
    [Test]
    public void GivenAUserContactDetails_WhenISetUserName_ThenUserNameIsSet()
    {
        const string testUserName = "test user name";

        var testUserContactDetails = new UserContactDetails
        {
            UserName = testUserName,
            EmailAddress = "_",
            EmailNotification = It.IsAny<bool>()
        };

        var result = testUserContactDetails.UserName;

        Assert.That(result, Is.EqualTo(testUserName));
    }

    [Test]
    public void GivenAUserContactDetails_WhenISetEmailAddress_ThenEmailAddressIsSet()
    {
        const string testEmailAddress = "test email address";

        var testUserContactDetails = new UserContactDetails
        {
            UserName = "_",
            EmailAddress = testEmailAddress,
            EmailNotification = It.IsAny<bool>()
        };

        var result = testUserContactDetails.EmailAddress;

        Assert.That(result, Is.EqualTo(testEmailAddress));
    }

    [Theory]
    public void GivenAUserContactDetails_WhenISetEmailNotification_ThenEmailNotificationIsSet(
        bool testEmailNotification)
    {
        var testUserContactDetails = new UserContactDetails
        {
            UserName = "_",
            EmailAddress = "_",
            EmailNotification = testEmailNotification
        };

        var result = testUserContactDetails.EmailNotification;

        Assert.That(result, Is.EqualTo(testEmailNotification));
    }
}