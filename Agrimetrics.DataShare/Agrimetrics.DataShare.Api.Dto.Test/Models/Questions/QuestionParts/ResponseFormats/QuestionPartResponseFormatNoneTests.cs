using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatNoneTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatNone_WhenIGetInputType_ThenInputTypeIsNone()
    {
        var testQuestionPartResponseFormatNone = new QuestionPartResponseFormatNone();

        var result = testQuestionPartResponseFormatNone.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.None));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatNone_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartResponseFormatNone = new QuestionPartResponseFormatNone();

        testQuestionPartResponseFormatNone.InputType = testInputType;

        var result = testQuestionPartResponseFormatNone.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }
}