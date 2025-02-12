using Agrimetrics.DataShare.Api.Dto.Models.Acquirer;
using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Acquirer;

[TestFixture]
public class AcquirerUserDetailsTests
{
    [Test]
    public void GivenAcquirerUserDetails_WhenIOrganisationId_ThenOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testOrganisationId)
    {
        var testAcquirerUserDetails = new AcquirerUserDetails
        {
            OrganisationId = testOrganisationId,
            DomainId = 0,
            UserId = 0
        };

        var result = testAcquirerUserDetails.OrganisationId;

        Assert.That(result, Is.EqualTo(testOrganisationId));
    }

    [Test]
    public void GivenAcquirerUserDetails_WhenIDomainId_ThenDomainIdIsSet(
        [Values(-1, 0, 999)] int testDomainId)
    {
        var testAcquirerUserDetails = new AcquirerUserDetails
        {
            OrganisationId = 0,
            DomainId = testDomainId,
            UserId = 0
        };

        var result = testAcquirerUserDetails.DomainId;

        Assert.That(result, Is.EqualTo(testDomainId));
    }

    [Test]
    public void GivenAcquirerUserDetails_WhenIDomainId_ThenUserIdIsSet(
        [Values(-1, 0, 999)] int testUserId)
    {
        var testAcquirerUserDetails = new AcquirerUserDetails
        {
            OrganisationId = 0,
            DomainId = 0,
            UserId = testUserId
        };

        var result = testAcquirerUserDetails.UserId;

        Assert.That(result, Is.EqualTo(testUserId));
    }
}
