using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Acquirer.DataShareRequests;

[TestFixture]
public class DataShareRequestDeletionResultTests
{
    [Test]
    public void GivenADataShareRequestDeletionResult_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestId = new Guid("D373A58B-0176-4497-9E8C-B246B75C3B50");

        var testDataShareRequestDeletionResult = new DataShareRequestDeletionResult
        {
            DataShareRequestId = testDataShareRequestId
        };
        
        var result = testDataShareRequestDeletionResult.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }
}