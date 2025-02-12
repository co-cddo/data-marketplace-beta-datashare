using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionSets;

[TestFixture]
public class QuestionSetSectionOutlineTests
{
    [Test]
    public void GivenAQuestionSetSectionOutline_WhenISetOrderWithinQuestionSetOutline_ThenOrderWithinQuestionSetOutlineIsSet(
        [Values(-1, 0, 999)] int testOrderWithinQuestionSetOutline)
    {
        var testQuestionSetSectionOutline = new QuestionSetSectionOutline();

        testQuestionSetSectionOutline.OrderWithinQuestionSetOutline = testOrderWithinQuestionSetOutline;

        var result = testQuestionSetSectionOutline.OrderWithinQuestionSetOutline;

        Assert.That(result, Is.EqualTo(testOrderWithinQuestionSetOutline));
    }

    [Test]
    public void GivenAQuestionSetSectionOutline_WhenISetHeader_ThenHeaderIsSet(
        [Values("", "  ", "abc")] string testHeader)
    {
        var testQuestionSetSectionOutline = new QuestionSetSectionOutline();

        testQuestionSetSectionOutline.Header = testHeader;

        var result = testQuestionSetSectionOutline.Header;

        Assert.That(result, Is.EqualTo(testHeader));
    }

    [Test]
    public void GivenAQuestionSetSectionOutline_WhenISetAnEmptySetOfQuestions_ThenQuestionsIsSet()
    {
        var testQuestionSetSectionOutline = new QuestionSetSectionOutline();

        var testQuestions = new List<QuestionSetQuestionOutline>();

        testQuestionSetSectionOutline.Questions = testQuestions;

        var result = testQuestionSetSectionOutline.Questions;

        Assert.That(result, Is.EqualTo(testQuestions));
    }

    [Test]
    public void GivenAQuestionSetSectionOutline_WhenISetQuestions_ThenQuestionsIsSet()
    {
        var testQuestionSetSectionOutline = new QuestionSetSectionOutline();

        var testQuestions = new List<QuestionSetQuestionOutline> {new(), new(), new()};

        testQuestionSetSectionOutline.Questions = testQuestions;

        var result = testQuestionSetSectionOutline.Questions;

        Assert.That(result, Is.EqualTo(testQuestions));
    }
}