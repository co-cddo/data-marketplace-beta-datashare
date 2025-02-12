using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionDetailsAnswerPartResponseModelDataTests
{
    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseModelData_WhenISetId_ThenIdIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseModelData = new SubmissionDetailsAnswerPartResponseModelData();

        var testId = new Guid("F8936010-F3F4-4AC4-9B05-37F835793D04");

        testSubmissionDetailsAnswerPartResponseModelData.SubmissionDetailsAnswerPartResponse_Id = testId;

        var result = testSubmissionDetailsAnswerPartResponseModelData.SubmissionDetailsAnswerPartResponse_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseModelData_WhenISetOrderWithinAnswerPart_ThenOrderWithinAnswerPartIsSet(
        [Values(-1, 0, 999)] int testOrderWithinAnswerPart)
    {
        var testSubmissionDetailsAnswerPartResponseModelData = new SubmissionDetailsAnswerPartResponseModelData();

        testSubmissionDetailsAnswerPartResponseModelData.SubmissionDetailsAnswerPartResponse_OrderWithinAnswerPart = testOrderWithinAnswerPart;

        var result = testSubmissionDetailsAnswerPartResponseModelData.SubmissionDetailsAnswerPartResponse_OrderWithinAnswerPart;

        Assert.That(result, Is.EqualTo(testOrderWithinAnswerPart));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseModelData_WhenISetAnEmptySetOfResponseItems_ThenResponseItemsIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseModelData = new SubmissionDetailsAnswerPartResponseModelData();

        var testResponseItems = new List<SubmissionDetailsAnswerPartResponseItemModelData>();

        testSubmissionDetailsAnswerPartResponseModelData.SubmissionDetailsAnswerPartResponse_ResponseItems = testResponseItems;

        var result = testSubmissionDetailsAnswerPartResponseModelData.SubmissionDetailsAnswerPartResponse_ResponseItems;

        Assert.That(result, Is.EqualTo(testResponseItems));
    }

    [Test]
    public void GivenASubmissionDetailsAnswerPartResponseModelData_WhenISetResponseItems_ThenResponseItemsIsSet()
    {
        var testSubmissionDetailsAnswerPartResponseModelData = new SubmissionDetailsAnswerPartResponseModelData();

        var testResponseItems = new List<SubmissionDetailsAnswerPartResponseItemModelData> {new(), new(), new()};

        testSubmissionDetailsAnswerPartResponseModelData.SubmissionDetailsAnswerPartResponse_ResponseItems = testResponseItems;

        var result = testSubmissionDetailsAnswerPartResponseModelData.SubmissionDetailsAnswerPartResponse_ResponseItems;

        Assert.That(result, Is.EqualTo(testResponseItems));
    }
}