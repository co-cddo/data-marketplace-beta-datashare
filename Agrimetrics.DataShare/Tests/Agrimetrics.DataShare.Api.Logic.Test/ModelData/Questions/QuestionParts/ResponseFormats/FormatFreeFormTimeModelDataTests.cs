using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatFreeFormTimeModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormTimeModelData_WhenIGetFormatType_ThenFormatTypeIsTime()
    {
        var testQuestionPartResponseFormatFreeFormTimeModelData = new QuestionPartResponseFormatFreeFormTimeModelData();

        var result = testQuestionPartResponseFormatFreeFormTimeModelData.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.Time));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormTimeModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatFreeFormTimeModelData = new QuestionPartResponseFormatFreeFormTimeModelData();

        testQuestionPartResponseFormatFreeFormTimeModelData.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatFreeFormTimeModelData.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }
}