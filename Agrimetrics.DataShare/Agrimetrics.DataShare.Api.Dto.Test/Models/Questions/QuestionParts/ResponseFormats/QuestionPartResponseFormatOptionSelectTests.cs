using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatOptionSelectTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelect_WhenIGetInputType_ThenInputTypeIsOptionSelection()
    {
        var testQuestionPartResponseFormatOptionSelect = new QuestionPartResponseFormatOptionSelect();

        var result = testQuestionPartResponseFormatOptionSelect.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.OptionSelection));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatOptionSelect_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartResponseFormatOptionSelect = new QuestionPartResponseFormatOptionSelect();

        testQuestionPartResponseFormatOptionSelect.InputType = testInputType;

        var result = testQuestionPartResponseFormatOptionSelect.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }
}