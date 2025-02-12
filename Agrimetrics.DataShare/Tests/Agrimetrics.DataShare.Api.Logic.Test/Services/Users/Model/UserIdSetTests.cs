using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Users.Model;

[TestFixture]
public class UserIdSetTests
{
    [Test]
    public void GivenAUserIdSet_WhenISetUserId_ThenUserIdIsSet()
    {
        const int testUserId = 123;

        var testUserIdSet = new UserIdSet
        {
            UserId = testUserId,
            DomainId = 0,
            OrganisationId = 0,
            EmailNotification = It.IsAny<bool>()
        };

        var result = testUserIdSet.UserId;

        Assert.That(result, Is.EqualTo(testUserId));
    }

    [Test]
    public void GivenAUserIdSet_WhenISetDomainId_ThenDomainIdIsSet()
    {
        const int testDomainId = 123;

        var testUserIdSet = new UserIdSet
        {
            UserId = 0,
            DomainId = testDomainId,
            OrganisationId = 0,
            EmailNotification = It.IsAny<bool>()
        };

        var result = testUserIdSet.DomainId;

        Assert.That(result, Is.EqualTo(testDomainId));
    }

    [Test]
    public void GivenAUserIdSet_WhenISetOrganisationId_ThenOrganisationIdIsSet()
    {
        const int testOrganisationId = 123;

        var testUserIdSet = new UserIdSet
        {
            UserId = 0,
            DomainId = 0,
            OrganisationId = testOrganisationId,
            EmailNotification = It.IsAny<bool>()
        };

        var result = testUserIdSet.OrganisationId;

        Assert.That(result, Is.EqualTo(testOrganisationId));
    }

    [Theory]
    public void GivenAUserIdSet_WhenISetEmailNotification_ThenEmailNotificationIsSet(
        bool testEmailNotification)
    {
        var testUserIdSet = new UserIdSet
        {
            UserId = 0,
            DomainId = 0,
            OrganisationId = 0,
            EmailNotification = testEmailNotification
        };

        var result = testUserIdSet.EmailNotification;

        Assert.That(result, Is.EqualTo(testEmailNotification));
    }
}