using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionSets;

[TestFixture]
public class QuestionSetQuestionOutlineTests
{
    [Test]
    public void GivenAQuestionSetQuestionOutline_WhenISetOrderWithinQuestionSetSection_ThenOrderWithinQuestionSetSectionIsSet(
        [Values(-1, 0, 999)] int testOrderWithinQuestionSetSection)
    {
        var testQuestionSetQuestionOutline = new QuestionSetQuestionOutline();

        testQuestionSetQuestionOutline.OrderWithinQuestionSetSection = testOrderWithinQuestionSetSection;

        var result = testQuestionSetQuestionOutline.OrderWithinQuestionSetSection;

        Assert.That(result, Is.EqualTo(testOrderWithinQuestionSetSection));
    }

    [Test]
    public void GivenAQuestionSetQuestionOutline_WhenISetQuestionText_ThenQuestionTextIsSet(
        [Values("", "  ", "abc")] string testQuestionText)
    {
        var testQuestionSetQuestionOutline = new QuestionSetQuestionOutline();

        testQuestionSetQuestionOutline.QuestionText = testQuestionText;

        var result = testQuestionSetQuestionOutline.QuestionText;

        Assert.That(result, Is.EqualTo(testQuestionText));
    }
}