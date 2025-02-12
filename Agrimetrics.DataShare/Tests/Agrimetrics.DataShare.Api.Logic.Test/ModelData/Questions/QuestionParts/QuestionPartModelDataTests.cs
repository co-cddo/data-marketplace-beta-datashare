using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts;

[TestFixture]
public class QuestionPartModelDataTests
{
    [Test]
    public void GivenAQuestionPartModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionPartModelData = new QuestionPartModelData();

        var testId = new Guid("403090D5-21BD-4546-B1D7-A1227D29FC51");

        testQuestionPartModelData.QuestionPart_Id = testId;

        var result = testQuestionPartModelData.QuestionPart_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionPartModelData_WhenISetQuestionPartOrderWithinQuestion_ThenQuestionPartOrderWithinQuestionIsSet(
        [Values(-1, 0, 999)] int testQuestionPartOrderWithinQuestion)
    {
        var testQuestionPartModelData = new QuestionPartModelData();

        testQuestionPartModelData.QuestionPart_QuestionPartOrderWithinQuestion = testQuestionPartOrderWithinQuestion;

        var result = testQuestionPartModelData.QuestionPart_QuestionPartOrderWithinQuestion;

        Assert.That(result, Is.EqualTo(testQuestionPartOrderWithinQuestion));
    }

    [Theory]
    public void GivenAQuestionPartModelData_WhenISetQuestionPartType_ThenQuestionPartTypeIsSet(
        QuestionPartType testQuestionPartType)
    {
        var testQuestionPartModelData = new QuestionPartModelData();

        testQuestionPartModelData.QuestionPart_QuestionPartType = testQuestionPartType;

        var result = testQuestionPartModelData.QuestionPart_QuestionPartType;

        Assert.That(result, Is.EqualTo(testQuestionPartType));
    }

    [Test]
    public void GivenAQuestionPartModelData_WhenISetPrompts_ThenPromptsIsSet()
    {
        var testQuestionPartModelData = new QuestionPartModelData();

        var testPrompts = new QuestionPartPromptsModelData();

        testQuestionPartModelData.QuestionPart_Prompts = testPrompts;

        var result = testQuestionPartModelData.QuestionPart_Prompts;

        Assert.That(result, Is.EqualTo(testPrompts));
    }

    [Test]
    public void GivenAQuestionPartModelData_WhenISetMultipleAnswerItemControl_ThenMultipleAnswerItemControlIsSet()
    {
        var testQuestionPartModelData = new QuestionPartModelData();

        var testMultipleAnswerItemControl = new QuestionPartMultipleAnswerItemControlModelData();

        testQuestionPartModelData.QuestionPart_MultipleAnswerItemControl = testMultipleAnswerItemControl;

        var result = testQuestionPartModelData.QuestionPart_MultipleAnswerItemControl;

        Assert.That(result, Is.EqualTo(testMultipleAnswerItemControl));
    }

    [Test]
    public void GivenAQuestionPartModelData_WhenISetResponseTypeInformation_ThenResponseTypeInformationIsSet()
    {
        var testQuestionPartModelData = new QuestionPartModelData();

        var testResponseTypeInformation = new QuestionPartResponseTypeInformationModelData();

        testQuestionPartModelData.QuestionPart_ResponseTypeInformation = testResponseTypeInformation;

        var result = testQuestionPartModelData.QuestionPart_ResponseTypeInformation;

        Assert.That(result, Is.EqualTo(testResponseTypeInformation));
    }

    [Test]
    public void GivenAQuestionPartModelData_WhenISetResponseFormat_ThenResponseFormatIsSet()
    {
        var testQuestionPartModelData = new QuestionPartModelData();

        var testResponseFormat = new TestQuestionPartResponseFormatFreeFormModelData();

        testQuestionPartModelData.QuestionPart_ResponseFormat = testResponseFormat;

        var result = testQuestionPartModelData.QuestionPart_ResponseFormat;

        Assert.That(result, Is.EqualTo(testResponseFormat));
    }

    private class TestQuestionPartResponseFormatFreeFormModelData : QuestionPartResponseFormatFreeFormModelData;
}
