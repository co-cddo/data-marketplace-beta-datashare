using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests;

[TestFixture]
public class DataShareRequestForResourceForAcquirerOrganisationSummarySetModelDataTests
{
    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummarySetModelData_WhenISetAnEmptySetOfDataShareRequestForResourceForAcquirerOrganisationSummaries_ThenDataShareRequestForResourceForAcquirerOrganisationSummariesIsSet()
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummarySetModelData = new DataShareRequestForResourceForAcquirerOrganisationSummarySetModelData();

        var testDataShareRequestForResourceForAcquirerOrganisationSummaries = new List<DataShareRequestForResourceForAcquirerOrganisationSummaryModelData>();

        testDataShareRequestForResourceForAcquirerOrganisationSummarySetModelData.DataShareRequestForResourceForAcquirerOrganisationSummaries = testDataShareRequestForResourceForAcquirerOrganisationSummaries;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummarySetModelData.DataShareRequestForResourceForAcquirerOrganisationSummaries;

        Assert.That(result, Is.EqualTo(testDataShareRequestForResourceForAcquirerOrganisationSummaries));
    }

    [Test]
    public void GivenADataShareRequestForResourceForAcquirerOrganisationSummarySetModelData_WhenISetDataShareRequestForResourceForAcquirerOrganisationSummaries_ThenDataShareRequestForResourceForAcquirerOrganisationSummariesIsSet()
    {
        var testDataShareRequestForResourceForAcquirerOrganisationSummarySetModelData = new DataShareRequestForResourceForAcquirerOrganisationSummarySetModelData();

        var testDataShareRequestForResourceForAcquirerOrganisationSummaries = new List<DataShareRequestForResourceForAcquirerOrganisationSummaryModelData> {new(), new(), new()};

        testDataShareRequestForResourceForAcquirerOrganisationSummarySetModelData.DataShareRequestForResourceForAcquirerOrganisationSummaries = testDataShareRequestForResourceForAcquirerOrganisationSummaries;

        var result = testDataShareRequestForResourceForAcquirerOrganisationSummarySetModelData.DataShareRequestForResourceForAcquirerOrganisationSummaries;

        Assert.That(result, Is.EqualTo(testDataShareRequestForResourceForAcquirerOrganisationSummaries));
    }
}