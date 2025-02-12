using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatBaseTests
{
    [Theory]
    public void GivenAQuestionPartResponseFormatBase_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartResponseFormatBase = new QuestionPartResponseFormatBase();

        testQuestionPartResponseFormatBase.InputType = testInputType;

        var result = testQuestionPartResponseFormatBase.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatBase_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatBase = new QuestionPartResponseFormatBase();

        testQuestionPartResponseFormatBase.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatBase.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}
