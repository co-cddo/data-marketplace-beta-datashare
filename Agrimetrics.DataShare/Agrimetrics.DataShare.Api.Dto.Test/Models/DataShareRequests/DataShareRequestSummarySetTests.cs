using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests;

[TestFixture]
public class DataShareRequestSummarySetTests
{
    [Test]
    public void GivenADataShareRequestSummarySet_WhenISetAnEmptySetOfDataShareRequestSummaries_ThenDataShareRequestSummariesIsSet()
    {
        var testDataShareRequestSummarySet = new DataShareRequestSummarySet();

        var testDataShareRequestSummaries = new List<DataShareRequestSummary>();

        testDataShareRequestSummarySet.DataShareRequestSummaries = testDataShareRequestSummaries;

        var result = testDataShareRequestSummarySet.DataShareRequestSummaries;

        Assert.That(result, Is.EqualTo(testDataShareRequestSummaries));
    }

    [Test]
    public void GivenADataShareRequestSummarySet_WhenISetDataShareRequestSummaries_ThenDataShareRequestSummariesIsSet()
    {
        var testDataShareRequestSummarySet = new DataShareRequestSummarySet();

        var testDataShareRequestSummaries = new List<DataShareRequestSummary> {new(), new(), new()};

        testDataShareRequestSummarySet.DataShareRequestSummaries = testDataShareRequestSummaries;

        var result = testDataShareRequestSummarySet.DataShareRequestSummaries;

        Assert.That(result, Is.EqualTo(testDataShareRequestSummaries));
    }
}