using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts;

[TestFixture]
public class QuestionPartTests
{
    [Test]
    public void GivenAQuestionPart_WhenISetId_ThenIdIsSet()
    {
        var testQuestionPart = new QuestionPart();

        var testId = new Guid("9E648100-639A-4863-9427-44554D0DD520");

        testQuestionPart.Id = testId;

        var result = testQuestionPart.Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionPart_WhenISetQuestionPartOrderWithinQuestion_ThenQuestionPartOrderWithinQuestionIsSet(
        [Values(-1, 0, 999)] int testQuestionPartOrderWithinQuestion)
    {
        var testQuestionPart = new QuestionPart();

        testQuestionPart.QuestionPartOrderWithinQuestion = testQuestionPartOrderWithinQuestion;

        var result = testQuestionPart.QuestionPartOrderWithinQuestion;

        Assert.That(result, Is.EqualTo(testQuestionPartOrderWithinQuestion));
    }

    [Test]
    public void GivenAQuestionPart_WhenISetPrompts_ThenPromptsIsSet()
    {
        var testQuestionPart = new QuestionPart();

        var testPrompts = new QuestionPartPrompts();

        testQuestionPart.Prompts = testPrompts;

        var result = testQuestionPart.Prompts;

        Assert.That(result, Is.SameAs(testPrompts));
    }

    [Test]
    public void GivenAQuestionPart_WhenISetMultipleAnswerItemControl_ThenMultipleAnswerItemControlIsSet()
    {
        var testQuestionPart = new QuestionPart();

        var testMultipleAnswerItemControl = new QuestionPartMultipleAnswerItemControl();

        testQuestionPart.MultipleAnswerItemControl = testMultipleAnswerItemControl;

        var result = testQuestionPart.MultipleAnswerItemControl;

        Assert.That(result, Is.SameAs(testMultipleAnswerItemControl));
    }

    [Test]
    public void GivenAQuestionPart_WhenISetResponseFormat_ThenResponseFormatIsSet()
    {
        var testQuestionPart = new QuestionPart();

        var testResponseFormat = new TestQuestionPartResponseFormat();

        testQuestionPart.ResponseFormat = testResponseFormat;

        var result = testQuestionPart.ResponseFormat;

        Assert.That(result, Is.EqualTo(testResponseFormat));
    }

    private class TestQuestionPartResponseFormat : QuestionPartResponseFormatBase;
}

