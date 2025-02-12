using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests;

[TestFixture]
public class QuestionSetQuestionOutlineModelDataTests
{
    [Test]
    public void GivenAQuestionSetQuestionOutlineModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionSetQuestionOutlineModelData = new QuestionSetQuestionOutlineModelData();

        var testId = new Guid("39A40D9D-37D4-4AAA-A1CE-4BBA1C9FDD32");

        testQuestionSetQuestionOutlineModelData.QuestionSetQuestionOutline_Id = testId;

        var result = testQuestionSetQuestionOutlineModelData.QuestionSetQuestionOutline_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionSetQuestionOutlineModelData_WhenISetOrderWithinSection_ThenOrderWithinSectionIsSet(
        [Values(-1, 0, 999)] int testOrderWithinSection)
    {
        var testQuestionSetQuestionOutlineModelData = new QuestionSetQuestionOutlineModelData();

        testQuestionSetQuestionOutlineModelData.QuestionSetQuestionOutline_OrderWithinSection = testOrderWithinSection;

        var result = testQuestionSetQuestionOutlineModelData.QuestionSetQuestionOutline_OrderWithinSection;

        Assert.That(result, Is.EqualTo(testOrderWithinSection));
    }

    [Test]
    public void GivenAQuestionSetQuestionOutlineModelData_WhenISetQuestionText_ThenQuestionTextIsSet(
        [Values("", "  ", "abc")] string testQuestionText)
    {
        var testQuestionSetQuestionOutlineModelData = new QuestionSetQuestionOutlineModelData();

        testQuestionSetQuestionOutlineModelData.QuestionSetQuestionOutline_QuestionText = testQuestionText;

        var result = testQuestionSetQuestionOutlineModelData.QuestionSetQuestionOutline_QuestionText;

        Assert.That(result, Is.EqualTo(testQuestionText));
    }
}