using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Acquirer.DataShareRequests;

[TestFixture]
public class AcquirerContactDetailsTests
{
    [Test]
    public void GivenAcquirerContactDetails_WhenISetUserName_ThenUserNameIsSet(
        [Values("", "  ", "abc")] string testUserName)
    {
        var testAcquirerContactDetails = new AcquirerContactDetails();

        testAcquirerContactDetails.UserName = testUserName;

        var result = testAcquirerContactDetails.UserName;

        Assert.That(result, Is.EqualTo(testUserName));
    }

    [Test]
    public void GivenAcquirerContactDetails_WhenISetEmailAddress_ThenEmailAddressIsSet(
        [Values("", "  ", "abc")] string testEmailAddress)
    {
        var testAcquirerContactDetails = new AcquirerContactDetails();

        testAcquirerContactDetails.EmailAddress = testEmailAddress;

        var result = testAcquirerContactDetails.EmailAddress;

        Assert.That(result, Is.EqualTo(testEmailAddress));
    }
}
