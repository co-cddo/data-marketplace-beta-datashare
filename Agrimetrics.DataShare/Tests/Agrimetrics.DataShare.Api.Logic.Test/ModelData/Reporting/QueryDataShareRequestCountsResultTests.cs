using Agrimetrics.DataShare.Api.Dto.Models.Reporting;
using Agrimetrics.DataShare.Api.Logic.ModelData.Reporting;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Reporting;

[TestFixture]
public class QueryDataShareRequestCountsResultTests
{
    [Test]
    public void GivenAQueryDataShareRequestCountsResult_WhenISetAnEmptySetOfDataShareRequestCounts_ThenDataShareRequestCountsIsSet()
    {
        var testDataShareRequestCounts = new List<IDataShareRequestCount>();

        var testQueryDataShareRequestCountsResult = new QueryDataShareRequestCountsResult
        {
            DataShareRequestCounts = testDataShareRequestCounts
        };

        var result = testQueryDataShareRequestCountsResult.DataShareRequestCounts;

        Assert.That(result, Is.EqualTo(testDataShareRequestCounts));
    }

    [Test]
    public void GivenAQueryDataShareRequestCountsResult_WhenISetDataShareRequestCounts_ThenDataShareRequestCountsIsSet()
    {
        var testDataShareRequestCounts = new List<IDataShareRequestCount>
        {
            new DataShareRequestCount
            {
                NumberOfDataShareRequests = 1,
                DataShareRequestCountQuery = new DataShareRequestCountQuery()
            },
            new DataShareRequestCount
            {
                NumberOfDataShareRequests = 2,
                DataShareRequestCountQuery = new DataShareRequestCountQuery()
            },
            new DataShareRequestCount
            {
                NumberOfDataShareRequests = 3,
                DataShareRequestCountQuery = new DataShareRequestCountQuery()
            }
        };

        var testQueryDataShareRequestCountsResult = new QueryDataShareRequestCountsResult
        {
            DataShareRequestCounts = testDataShareRequestCounts
        };

        var result = testQueryDataShareRequestCountsResult.DataShareRequestCounts;

        Assert.That(result, Is.EqualTo(testDataShareRequestCounts));
    }
}

