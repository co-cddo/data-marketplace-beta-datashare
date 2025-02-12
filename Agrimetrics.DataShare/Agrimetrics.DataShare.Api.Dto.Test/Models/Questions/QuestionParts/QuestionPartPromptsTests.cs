using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts;

[TestFixture]
public class QuestionPartPromptsTests
{
    [Test]
    public void GivenAQuestionPartPrompts_WhenISetQuestionText_ThenQuestionTextIsSet(
        [Values(null, "", "  ", "abc")] string? testQuestionText)
    {
        var testQuestionPartPrompts = new QuestionPartPrompts();

        testQuestionPartPrompts.QuestionText = testQuestionText;

        var result = testQuestionPartPrompts.QuestionText;

        Assert.That(result, Is.EqualTo(testQuestionText));
    }

    [Test]
    public void GivenAQuestionPartPrompts_WhenISetHintText_ThenHintTextIsSet(
        [Values(null, "", "  ", "abc")] string? testHintText)
    {
        var testQuestionPartPrompts = new QuestionPartPrompts();

        testQuestionPartPrompts.HintText = testHintText;

        var result = testQuestionPartPrompts.HintText;

        Assert.That(result, Is.EqualTo(testHintText));
    }
}