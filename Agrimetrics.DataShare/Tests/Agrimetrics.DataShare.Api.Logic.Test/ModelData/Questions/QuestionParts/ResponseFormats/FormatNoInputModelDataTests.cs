using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatNoInputModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatNoInputModelData_WhenIGetInputType_ThenInputTypeIsNone()
    {
        var testQuestionPartResponseFormatNoInputModelData = new TestQuestionPartResponseFormatNoInputModelData();

        var result = testQuestionPartResponseFormatNoInputModelData.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.None));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatNoInputModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartResponseFormatNoInputModelData = new TestQuestionPartResponseFormatNoInputModelData();

        testQuestionPartResponseFormatNoInputModelData.InputType = testInputType;

        var result = testQuestionPartResponseFormatNoInputModelData.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    private class TestQuestionPartResponseFormatNoInputModelData : QuestionPartResponseFormatNoInputModelData;
}