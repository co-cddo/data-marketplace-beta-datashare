using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests;

[TestFixture]
public class DataShareRequestAdminSummarySetTests
{
    [Test]
    public void GivenADataShareRequestAdminSummarySet_WhenISetAnEmptySetOfDataShareRequestAdminSummaries_ThenDataShareRequestAdminSummariesIsSet()
    {
        var testDataShareRequestAdminSummaries = new List<DataShareRequestAdminSummary>();

        var testDataShareRequestAdminSummarySet = new DataShareRequestAdminSummarySet
        {
            DataShareRequestAdminSummaries = testDataShareRequestAdminSummaries
        };

        var result = testDataShareRequestAdminSummarySet.DataShareRequestAdminSummaries;

        Assert.That(result, Is.EqualTo(testDataShareRequestAdminSummaries));
    }

    [Test]
    public void GivenADataShareRequestAdminSummarySet_WhenISetDataShareRequestAdminSummaries_ThenDataShareRequestAdminSummariesIsSet()
    {
        var testDataShareRequestAdminSummaries = new List<DataShareRequestAdminSummary>
        {
            new()
            {
                Id = Guid.Empty,
                RequestId = "_",
                EsdaName = "_",
                WhenCreatedUtc = new DateTime(),
                WhenSubmittedUtc = null,
                CreatedByUserEmailAddress = "_",
                WhenNeededByUtc = null,
                DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
            },
            new()
            {
                Id = Guid.Empty,
                RequestId = "_",
                EsdaName = "_",
                WhenCreatedUtc = new DateTime(),
                WhenSubmittedUtc = null,
                CreatedByUserEmailAddress = "_",
                WhenNeededByUtc = null,
                DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
            },
            new()
            {
                Id = Guid.Empty,
                RequestId = "_",
                EsdaName = "_",
                WhenCreatedUtc = new DateTime(),
                WhenSubmittedUtc = null,
                CreatedByUserEmailAddress = "_",
                WhenNeededByUtc = null,
                DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
            }
        };

        var testDataShareRequestAdminSummarySet = new DataShareRequestAdminSummarySet
        {
            DataShareRequestAdminSummaries = testDataShareRequestAdminSummaries
        };

        var result = testDataShareRequestAdminSummarySet.DataShareRequestAdminSummaries;

        Assert.That(result, Is.SameAs(testDataShareRequestAdminSummaries));
    }
}