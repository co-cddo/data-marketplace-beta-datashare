using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatModelDataTests
{
    [Theory]
    public void GivenAQuestionPartResponseFormatModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartResponseFormatModelData = new TestQuestionPartResponseFormatModelData();

        testQuestionPartResponseFormatModelData.InputType = testInputType;

        var result = testQuestionPartResponseFormatModelData.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatModelData = new TestQuestionPartResponseFormatModelData();

        testQuestionPartResponseFormatModelData.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatModelData.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }

    private class TestQuestionPartResponseFormatModelData : QuestionPartResponseFormatModelData;
}