using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts;

[TestFixture]
public class QuestionPartPromptsModelDataTests
{
    [Test]
    public void GivenAQuestionPartPromptsModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testQuestionPartPromptsModelData = new QuestionPartPromptsModelData();

        var testQuestionPartId = new Guid("73E9E7F4-ACB6-410F-BD0E-82B654A8ECA1");

        testQuestionPartPromptsModelData.QuestionPartPrompt_QuestionPartId = testQuestionPartId;

        var result = testQuestionPartPromptsModelData.QuestionPartPrompt_QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Test]
    public void GivenAQuestionPartPromptsModelData_WhenISetQuestionText_ThenQuestionTextIsSet(
        [Values(null, "", "  ", "abc")] string? testQuestionText)
    {
        var testQuestionPartPromptsModelData = new QuestionPartPromptsModelData();

        testQuestionPartPromptsModelData.QuestionPartPrompt_QuestionText = testQuestionText;

        var result = testQuestionPartPromptsModelData.QuestionPartPrompt_QuestionText;

        Assert.That(result, Is.EqualTo(testQuestionText));
    }

    [Test]
    public void GivenAQuestionPartPromptsModelData_WhenISetHintText_ThenHintTextIsSet(
        [Values(null, "", "  ", "abc")] string? testHintText)
    {
        var testQuestionPartPromptsModelData = new QuestionPartPromptsModelData();

        testQuestionPartPromptsModelData.QuestionPartPrompt_HintText = testHintText;

        var result = testQuestionPartPromptsModelData.QuestionPartPrompt_HintText;

        Assert.That(result, Is.EqualTo(testHintText));
    }
}