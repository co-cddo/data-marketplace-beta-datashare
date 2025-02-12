using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Acquirer.DataShareRequests;

[TestFixture]
public class DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySetTests
{
    [Test]
    public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet_WhenISetEsdaId_ThenEsdaIdIsSet()
    {
        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet();

        var testEsdaId = new Guid("D373A58B-0176-4497-9E8C-B246B75C3B50");

        testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet.EsdaId = testEsdaId;

        var result = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet.EsdaId;

        Assert.That(result, Is.EqualTo(testEsdaId));
    }

    [Test]
    public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet();

        testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet.EsdaName = testEsdaName;

        var result = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet_WhenISetAnEmptyListOfDataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries_ThenDataShareRequestRaisedForEsdaByAcquirerOrganisationSummariesIsSet()
    {
        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet();

        testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet.DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries = [];

        var result = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet.DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries;

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet_WhenISetDataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries_ThenDataShareRequestRaisedForEsdaByAcquirerOrganisationSummariesIsSet()
    {
        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet = new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet();

        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries = new List<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary>
        {
            new(), new(), new()
        };

        testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet.DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries;

        var result = testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet.DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries;

        Assert.That(result, Is.SameAs(testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries));
    }
}