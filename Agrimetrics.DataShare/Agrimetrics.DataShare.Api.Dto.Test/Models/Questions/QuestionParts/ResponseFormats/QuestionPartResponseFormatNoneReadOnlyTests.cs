using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatNoneReadOnlyTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatNoneReadOnly_WhenIGetFormatType_ThenFormatTypeIsReadOnly()
    {
        var testQuestionPartResponseFormatNoneReadOnly = new QuestionPartResponseFormatNoneReadOnly();

        var result = testQuestionPartResponseFormatNoneReadOnly.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.ReadOnly));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatNoneReadOnly_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatNoneReadOnly = new QuestionPartResponseFormatNoneReadOnly();

        testQuestionPartResponseFormatNoneReadOnly.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatNoneReadOnly.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}