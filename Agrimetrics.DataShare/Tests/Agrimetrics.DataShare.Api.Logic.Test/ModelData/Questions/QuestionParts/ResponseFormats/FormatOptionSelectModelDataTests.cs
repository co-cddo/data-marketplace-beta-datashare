using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatOptionSelectModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectModelData_WhenIGetInputType_ThenInputTypeIsOptionSelection()
    {
        var testQuestionPartResponseFormatOptionSelectModelData = new TestQuestionPartResponseFormatOptionSelectModelData();

        var result = testQuestionPartResponseFormatOptionSelectModelData.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.OptionSelection));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatOptionSelectModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartResponseFormatOptionSelectModelData = new TestQuestionPartResponseFormatOptionSelectModelData();

        testQuestionPartResponseFormatOptionSelectModelData.InputType = testInputType;

        var result = testQuestionPartResponseFormatOptionSelectModelData.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    private class TestQuestionPartResponseFormatOptionSelectModelData : QuestionPartResponseFormatOptionSelectModelData
    {
        public override List<QuestionPartOptionSelectionItemModelData> GetResponseFormatOptionSelectOptionSelectionItems() => [];
    }
}