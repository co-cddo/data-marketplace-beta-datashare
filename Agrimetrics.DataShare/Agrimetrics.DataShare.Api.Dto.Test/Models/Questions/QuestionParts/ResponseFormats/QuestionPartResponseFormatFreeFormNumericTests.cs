using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormNumericTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormNumeric_WhenIGetFormatType_ThenFormatTypeIsNumeric()
    {
        var testQuestionPartResponseFormatFreeFormNumeric = new QuestionPartResponseFormatFreeFormNumeric();

        var result = testQuestionPartResponseFormatFreeFormNumeric.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.Numeric));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormNumeric_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormNumeric = new QuestionPartResponseFormatFreeFormNumeric();

        testQuestionPartResponseFormatFreeFormNumeric.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormNumeric.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}