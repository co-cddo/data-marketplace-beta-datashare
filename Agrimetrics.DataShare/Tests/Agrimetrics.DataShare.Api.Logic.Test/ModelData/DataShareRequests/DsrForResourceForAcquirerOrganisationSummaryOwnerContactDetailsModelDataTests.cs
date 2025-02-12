using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests;

[TestFixture]
public class DataShareRequestForResourceForAcquirerOrganisationSummaryOwnerContactDetailsModelDataTests
{
    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummaryOwnerContactDetailsModelData_WhenISetEmailAddress_ThenEmailAddressIsSet(
        [Values("", "  ", "abc")] string testEmailAddress)
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummaryOwnerContactDetailsModelData = new DataShareRequestForResourceForAcquirerOrganisationSummaryOwnerContactDetailsModelData();

        testDataShareRequestForResourceForAcquirerOrganisationSummaryOwnerContactDetailsModelData.DataShareRequestForResourceForAcquirerOrganisationSummaryOwnerContactDetails_EmailAddress = testEmailAddress;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummaryOwnerContactDetailsModelData.DataShareRequestForResourceForAcquirerOrganisationSummaryOwnerContactDetails_EmailAddress;

        Assert.That(result, Is.EqualTo(testEmailAddress));
    }
}