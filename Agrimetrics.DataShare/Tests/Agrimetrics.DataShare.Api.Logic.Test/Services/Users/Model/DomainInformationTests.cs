using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Users.Model;

[TestFixture]
public class DomainInformationTests
{
    [Test]
    public void GivenADomainInformation_WhenISetDomainId_ThenDomainIdIsSet()
    {
        const int testDomainId = 123;

        var testDomainInformation = new DomainInformation
        {
            DomainId = testDomainId,
            DomainName = "_",
            DataShareRequestMailboxAddress = null
        };

        var result = testDomainInformation.DomainId;

        Assert.That(result, Is.EqualTo(testDomainId));
    }

    [Test]
    public void GivenADomainInformation_WhenISetDomainName_ThenDomainNameIsSet()
    {
        const string testDomainName = "test domain name";

        var testDomainInformation = new DomainInformation
        {
            DomainId = 0,
            DomainName = testDomainName,
            DataShareRequestMailboxAddress = null
        };

        var result = testDomainInformation.DomainName;

        Assert.That(result, Is.EqualTo(testDomainName));
    }

    [Test]
    public void GivenADomainInformation_WhenISetDataShareRequestMailboxAddress_ThenDataShareRequestMailboxAddressIsSet()
    {
        const string testDataShareRequestMailboxAddress = "test mailbox address";

        var testDomainInformation = new DomainInformation
        {
            DomainId = 0,
            DomainName = "_",
            DataShareRequestMailboxAddress = testDataShareRequestMailboxAddress
        };

        var result = testDomainInformation.DataShareRequestMailboxAddress;

        Assert.That(result, Is.EqualTo(testDataShareRequestMailboxAddress));
    }
}
