using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.AnswerValidation;

[TestFixture]
public class QuestionPartAnswerValidationRuleModelDataTests
{
    [Test]
    public void GivenAQuestionPartAnswerValidationRuleModelData_WhenISetRuleId_ThenRuleIdIsSet()
    {
        var testQuestionPartAnswerValidationRuleModelData = new QuestionPartAnswerValidationRuleModelData();

        var testRuleId = new Guid("A1643121-6F20-48F4-8E76-F123AA5FEC16");

        testQuestionPartAnswerValidationRuleModelData.QuestionPartAnswerValidationRule_RuleId = testRuleId;

        var result = testQuestionPartAnswerValidationRuleModelData.QuestionPartAnswerValidationRule_RuleId;

        Assert.That(result, Is.EqualTo(testRuleId));
    }

    [Theory]
    public void GivenAQuestionPartAnswerValidationRuleModelData_WhenISetRule_ThenRuleIsSet(
        QuestionPartAnswerValidationRuleId testRule)
    {
        var testQuestionPartAnswerValidationRuleModelData = new QuestionPartAnswerValidationRuleModelData();

        testQuestionPartAnswerValidationRuleModelData.QuestionPartAnswerValidationRule_Rule = testRule;

        var result = testQuestionPartAnswerValidationRuleModelData.QuestionPartAnswerValidationRule_Rule;

        Assert.That(result, Is.EqualTo(testRule));
    }

    [Test]
    public void GivenAQuestionPartAnswerValidationRuleModelData_WhenISetQuestionErrorText_ThenQuestionErrorTextIsSet(
        [Values(null, "", "  ", "abc")] string? testQuestionErrorText)
    {
        var testQuestionPartAnswerValidationRuleModelData = new QuestionPartAnswerValidationRuleModelData();

        testQuestionPartAnswerValidationRuleModelData.QuestionPartAnswerValidationRule_QuestionErrorText = testQuestionErrorText;

        var result = testQuestionPartAnswerValidationRuleModelData.QuestionPartAnswerValidationRule_QuestionErrorText;

        Assert.That(result, Is.EqualTo(testQuestionErrorText));
    }

    [Test]
    public void GivenAQuestionPartAnswerValidationRuleModelData_WhenISetRuleErrorText_ThenRuleErrorTextIsSet(
        [Values(null, "", "  ", "abc")] string testRuleErrorText)
    {
        var testQuestionPartAnswerValidationRuleModelData = new QuestionPartAnswerValidationRuleModelData();

        testQuestionPartAnswerValidationRuleModelData.QuestionPartAnswerValidationRule_RuleErrorText = testRuleErrorText;

        var result = testQuestionPartAnswerValidationRuleModelData.QuestionPartAnswerValidationRule_RuleErrorText;

        Assert.That(result, Is.EqualTo(testRuleErrorText));
    }
}