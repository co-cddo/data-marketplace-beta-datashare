using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.Users.Model;

[TestFixture]
public class OrganisationInformationTests
{
    [Test]
    public void GivenAOrganisationInformation_WhenISetOrganisationId_ThenOrganisationIdIsSet()
    {
        const int testOrganisationId = 123;

        var testOrganisationInformation = new OrganisationInformation
        {
            OrganisationId = testOrganisationId,
            OrganisationName = "_",
            Domains = []
        };

        var result = testOrganisationInformation.OrganisationId;

        Assert.That(result, Is.EqualTo(testOrganisationId));
    }

    [Test]
    public void GivenAOrganisationInformation_WhenISetOrganisationName_ThenOrganisationNameIsSet()
    {
        const string testOrganisationName = "test domain name";

        var testOrganisationInformation = new OrganisationInformation
        {
            OrganisationId = 0,
            OrganisationName = testOrganisationName,
            Domains = []
        };

        var result = testOrganisationInformation.OrganisationName;

        Assert.That(result, Is.EqualTo(testOrganisationName));
    }

    [Test]
    public void GivenAOrganisationInformation_WhenISetDomains_ThenDomainsIsSet()
    {
        var testDomains = new List<IDomainInformation>
        {
            Mock.Of<IDomainInformation>(),
            Mock.Of<IDomainInformation>(),
            Mock.Of<IDomainInformation>()
        };

        var testOrganisationInformation = new OrganisationInformation
        {
            OrganisationId = 0,
            OrganisationName = "_",
            Domains = testDomains
        };

        var result = testOrganisationInformation.Domains;

        Assert.That(result, Is.EqualTo(testDomains));
    }
}