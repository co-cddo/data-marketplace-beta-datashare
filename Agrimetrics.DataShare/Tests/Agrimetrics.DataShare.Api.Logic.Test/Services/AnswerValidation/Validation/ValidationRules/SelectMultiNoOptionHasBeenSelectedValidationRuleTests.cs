using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation.ValidationRules;

[TestFixture]
public class SelectMultiNoOptionHasBeenSelectedValidationRuleTests
{
    #region Accepts() Tests
    [Test]
    public void GivenANullValidationRule_WhenICheckAccepts_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SelectMultiNoOptionHasBeenSelectedValidationRule.Accepts(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("validationRule"));
    }

    [Test]
    public void GivenAValidationRuleWithSelectMultiNoOptionIsSelectedRuleId_WhenICheckAccepts_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        var testValidationRule = testItems.Fixture.Build<QuestionPartAnswerValidationRuleModelData>()
            .With(x => x.QuestionPartAnswerValidationRule_Rule, QuestionPartAnswerValidationRuleId.OptionSelection_SelectMulti_NoOptionIsSelected)
            .Create();

        var result = testItems.SelectMultiNoOptionHasBeenSelectedValidationRule.Accepts(testValidationRule);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenAValidationRuleNotWithSelectMultiNoOptionIsSelectedRuleId_WhenICheckAccepts_ThenFalseIsReturned(
        [ValueSource(nameof(NonApplicableValidationRuleIds))] QuestionPartAnswerValidationRuleId validationRuleId)
    {
        var testItems = CreateTestItems();

        var testValidationRule = testItems.Fixture.Build<QuestionPartAnswerValidationRuleModelData>()
            .With(x => x.QuestionPartAnswerValidationRule_Rule, validationRuleId)
            .Create();

        var result = testItems.SelectMultiNoOptionHasBeenSelectedValidationRule.Accepts(testValidationRule);

        Assert.That(result, Is.False);
    }

    private static IEnumerable<QuestionPartAnswerValidationRuleId> NonApplicableValidationRuleIds =>
        Enum.GetValues<QuestionPartAnswerValidationRuleId>().Except([QuestionPartAnswerValidationRuleId.OptionSelection_SelectMulti_NoOptionIsSelected]);
    #endregion

    #region ResponseFailsValidation() Tests
    [Test]
    public void GivenANullQuestionAnswerPart_WhenICheckWhetherResponseFailsValidation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SelectMultiNoOptionHasBeenSelectedValidationRule.ResponseFailsValidation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionAnswerPartResponseForValidation"));
    }

    [Test]
    public void GivenANonSelectionOptionResponse_WhenICheckWhetherResponseFailsValidation_ThenAnInvalidOperationExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var questionAnswerPartResponse = testItems.Fixture.Create<IQuestionAnswerPartResponseForValidation>();

        Assert.That(() => testItems.SelectMultiNoOptionHasBeenSelectedValidationRule.ResponseFailsValidation(questionAnswerPartResponse),
            Throws.InvalidOperationException.With.Message.EqualTo("QuestionAnswerPartResponse is not of expected type for validation rule SelectMultiNoOptionHasBeenSelectedValidationRule"));
    }

    [Test]
    public void GivenASelectionOptionResponseAndNoOptionsAreSelected_WhenICheckWhetherResponseFailsValidation_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        var selectionOptionResponse = testItems.Fixture.Build<DataShareRequestQuestionAnswerPartResponseSelectionOption>()
            .With(x => x.SelectedOptionItems, [])
            .Create();

        var questionAnswerPartResponse = testItems.Fixture.Build<QuestionAnswerPartResponseForValidation>()
            .With(x => x.QuestionAnswerPartResponse, selectionOptionResponse)
            .Create();

        var result = testItems.SelectMultiNoOptionHasBeenSelectedValidationRule.ResponseFailsValidation(questionAnswerPartResponse);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenASelectionOptionResponseAndOptionsAreSelected_WhenICheckWhetherResponseFailsValidation_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        var selectedOptionItems = testItems.Fixture.CreateMany<DataShareRequestQuestionAnswerPartResponseSelectionOptionItem>().ToList();

        var selectionOptionResponse = testItems.Fixture.Build<DataShareRequestQuestionAnswerPartResponseSelectionOption>()
            .With(x => x.SelectedOptionItems, selectedOptionItems)
            .Create();

        var questionAnswerPartResponse = testItems.Fixture.Build<QuestionAnswerPartResponseForValidation>()
            .With(x => x.QuestionAnswerPartResponse, selectionOptionResponse)
            .Create();

        var result = testItems.SelectMultiNoOptionHasBeenSelectedValidationRule.ResponseFailsValidation(questionAnswerPartResponse);

        Assert.That(result, Is.False);
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var selectMultiNoOptionHasBeenSelectedValidationRule = new SelectMultiNoOptionHasBeenSelectedValidationRule();

        return new TestItems(
            fixture,
            selectMultiNoOptionHasBeenSelectedValidationRule);
    }

    private class TestItems(
        IFixture fixture,
        IValidationRule selectMultiNoOptionHasBeenSelectedValidationRule)
    {
        public IFixture Fixture { get; } = fixture;
        public IValidationRule SelectMultiNoOptionHasBeenSelectedValidationRule { get; } = selectMultiNoOptionHasBeenSelectedValidationRule;
    }
    #endregion
}