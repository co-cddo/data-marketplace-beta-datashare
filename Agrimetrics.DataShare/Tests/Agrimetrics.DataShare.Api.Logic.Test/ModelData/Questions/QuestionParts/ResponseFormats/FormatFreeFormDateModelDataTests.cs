using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormDateModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormDateModelData_WhenIGetFormatType_ThenFormatTypeIsDate()
    {
        var testQuestionPartResponseFormatFreeFormDateModelData = new QuestionPartResponseFormatFreeFormDateModelData();

        var result = testQuestionPartResponseFormatFreeFormDateModelData.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.Date));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormDateModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormDateModelData = new QuestionPartResponseFormatFreeFormDateModelData();

        testQuestionPartResponseFormatFreeFormDateModelData.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormDateModelData.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}

