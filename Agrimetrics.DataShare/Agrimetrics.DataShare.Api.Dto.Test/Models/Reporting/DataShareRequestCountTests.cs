using Agrimetrics.DataShare.Api.Dto.Models.AuditLogs;
using Agrimetrics.DataShare.Api.Dto.Models.Reporting;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Reporting;

[TestFixture]
public class DataShareRequestCountTests
{
    [Test]
    public void GivenADataShareRequestCount_WhenISetDataShareRequestCountQuery_ThenDataShareRequestCountQueryIsSet()
    {
        var testDataShareRequestCountQuery = new DataShareRequestCountQuery();

        var testDataShareRequestCount = new DataShareRequestCount
        {
            DataShareRequestCountQuery = testDataShareRequestCountQuery,
            NumberOfDataShareRequests = 0
        };

        var result = testDataShareRequestCount.DataShareRequestCountQuery;

        Assert.That(result, Is.SameAs(testDataShareRequestCountQuery));
    }

    [Test]
    public void GivenADataShareRequestCount_WhenISetNumberOfDataShareRequests_ThenNumberOfDataShareRequestsIsSet(
        [Values(-1, 0, 999)] int testNumberOfDataShareRequests)
    {
        var testDataShareRequestCount = new DataShareRequestCount
        {
            DataShareRequestCountQuery = new DataShareRequestCountQuery(),
            NumberOfDataShareRequests = testNumberOfDataShareRequests
        };

        var result = testDataShareRequestCount.NumberOfDataShareRequests;

        Assert.That(result, Is.EqualTo(testNumberOfDataShareRequests));
    }
}
