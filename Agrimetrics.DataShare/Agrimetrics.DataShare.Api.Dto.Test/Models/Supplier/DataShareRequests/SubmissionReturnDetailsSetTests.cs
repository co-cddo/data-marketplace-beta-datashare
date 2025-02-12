using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Supplier.DataShareRequests;

[TestFixture]
public class SubmissionReturnDetailsSetTests
{
    [Test]
    public void GivenASubmissionReturnDetailsSet_WhenISetAnEmptySetOfSubmissionReturns_ThenSubmissionReturnsIsSet()
    {
        var testSubmissionReturnDetailsSet = new SubmissionReturnDetailsSet();

        var testSubmissionReturns = new List<SubmissionReturnDetails>();

        testSubmissionReturnDetailsSet.SubmissionReturns = testSubmissionReturns;

        var result = testSubmissionReturnDetailsSet.SubmissionReturns;

        Assert.That(result, Is.EqualTo(testSubmissionReturns));
    }

    [Test]
    public void GivenASubmissionReturnDetailsSet_WhenISetSubmissionReturns_ThenSubmissionReturnsIsSet()
    {
        var testSubmissionReturnDetailsSet = new SubmissionReturnDetailsSet();

        var testSubmissionReturns = new List<SubmissionReturnDetails> {new(), new(), new()};

        testSubmissionReturnDetailsSet.SubmissionReturns = testSubmissionReturns;

        var result = testSubmissionReturnDetailsSet.SubmissionReturns;

        Assert.That(result, Is.EqualTo(testSubmissionReturns));
    }
}