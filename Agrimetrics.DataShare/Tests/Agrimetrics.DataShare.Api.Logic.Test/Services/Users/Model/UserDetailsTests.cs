using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Users.Model;

[TestFixture]
public class UserDetailsTests
{
    [Test]
    public void GivenAUserDetails_WhenISetUserIdSet_ThenUserIdSetIsSet()
    {
        var testUserIdSet = Mock.Of<IUserIdSet>();

        var testUserDetails = new UserDetails
        {
            UserIdSet = testUserIdSet,
            UserContactDetails = Mock.Of<IUserContactDetails>()
        };

        var result = testUserDetails.UserIdSet;

        Assert.That(result, Is.SameAs(testUserIdSet));
    }

    [Test]
    public void GivenAUserDetails_WhenISetUserContactDetails_ThenUserContactDetailsIsSet()
    {
        var testUserContactDetails = Mock.Of<IUserContactDetails>();

        var testUserDetails = new UserDetails
        {
            UserIdSet = Mock.Of<IUserIdSet>(),
            UserContactDetails = testUserContactDetails
        };

        var result = testUserDetails.UserContactDetails;

        Assert.That(result, Is.SameAs(testUserContactDetails));
    }
}