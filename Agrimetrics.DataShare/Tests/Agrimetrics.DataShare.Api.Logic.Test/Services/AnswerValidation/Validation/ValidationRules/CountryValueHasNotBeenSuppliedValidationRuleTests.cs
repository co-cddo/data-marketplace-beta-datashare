using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation.ValidationRules;

[TestFixture]
public class CountryValueHasNotBeenSuppliedValidationRuleTests
{
    #region Accepts() Tests
    [Test]
    public void GivenANullValidationRule_WhenICheckAccepts_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.CountryValueHasNotBeenSuppliedValidationRule.Accepts(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("validationRule"));
    }

    [Test]
    public void GivenAValidationRuleWithCountryValueHasNotBeenSuppliedRuleId_WhenICheckAccepts_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        var testValidationRule = testItems.Fixture.Build<QuestionPartAnswerValidationRuleModelData>()
            .With(x => x.QuestionPartAnswerValidationRule_Rule, QuestionPartAnswerValidationRuleId.FreeForm_Country_NoValueSupplied)
            .Create();

        var result = testItems.CountryValueHasNotBeenSuppliedValidationRule.Accepts(testValidationRule);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenAValidationRuleNotWithCountryValueHasNotBeenSuppliedRuleId_WhenICheckAccepts_ThenFalseIsReturned(
        [ValueSource(nameof(NonApplicableValidationRuleIds))] QuestionPartAnswerValidationRuleId validationRuleId)
    {
        var testItems = CreateTestItems();

        var testValidationRule = testItems.Fixture.Build<QuestionPartAnswerValidationRuleModelData>()
            .With(x => x.QuestionPartAnswerValidationRule_Rule, validationRuleId)
            .Create();

        var result = testItems.CountryValueHasNotBeenSuppliedValidationRule.Accepts(testValidationRule);

        Assert.That(result, Is.False);
    }

    private static IEnumerable<QuestionPartAnswerValidationRuleId> NonApplicableValidationRuleIds =>
        Enum.GetValues<QuestionPartAnswerValidationRuleId>().Except([QuestionPartAnswerValidationRuleId.FreeForm_Country_NoValueSupplied]);
    #endregion

    #region ResponseFailsValidation() Tests
    [Test]
    public void GivenANullQuestionAnswerPart_WhenICheckWhetherResponseFailsValidation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.CountryValueHasNotBeenSuppliedValidationRule.ResponseFailsValidation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionAnswerPartResponseForValidation"));
    }

    [Test]
    public void GivenANonFreeFormOptionResponse_WhenICheckWhetherResponseFailsValidation_ThenAnInvalidOperationExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var questionAnswerPartResponse = testItems.Fixture.Create<IQuestionAnswerPartResponseForValidation>();

        Assert.That(() => testItems.CountryValueHasNotBeenSuppliedValidationRule.ResponseFailsValidation(questionAnswerPartResponse),
            Throws.InvalidOperationException.With.Message.EqualTo("QuestionAnswerPartResponse is not of expected type for validation rule CountryValueHasNotBeenSuppliedValidationRule"));
    }

    [Test]
    public void GivenAFreeFormOptionResponseForANonOptionalResponseAndNoValueIsSupplied_WhenICheckWhetherResponseFailsValidation_ThenTrueIsReturned(
        [Values("", "  ")] string enteredValue)
    {
        var testItems = CreateTestItems();

        var freeFormResponse = testItems.Fixture.Build<DataShareRequestQuestionAnswerPartResponseFreeForm>()
            .With(x => x.EnteredValue, enteredValue)
            .Create();

        var questionAnswerPartResponse = testItems.Fixture.Build<QuestionAnswerPartResponseForValidation>()
            .With(x => x.QuestionAnswerPartIsOptional, false)
            .With(x => x.QuestionAnswerPartResponse, freeFormResponse)
            .Create();

        var result = testItems.CountryValueHasNotBeenSuppliedValidationRule.ResponseFailsValidation(questionAnswerPartResponse);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenAFreeFormOptionResponseForAnOptionalResponseAndNoValueIsSupplied_WhenICheckWhetherResponseFailsValidation_ThenFalseIsReturned(
        [Values("", "  ")] string enteredValue)
    {
        var testItems = CreateTestItems();

        var freeFormResponse = testItems.Fixture.Build<DataShareRequestQuestionAnswerPartResponseFreeForm>()
            .With(x => x.EnteredValue, enteredValue)
            .Create();

        var questionAnswerPartResponse = testItems.Fixture.Build<QuestionAnswerPartResponseForValidation>()
            .With(x => x.QuestionAnswerPartIsOptional, true)
            .With(x => x.QuestionAnswerPartResponse, freeFormResponse)
            .Create();

        var result = testItems.CountryValueHasNotBeenSuppliedValidationRule.ResponseFailsValidation(questionAnswerPartResponse);

        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenAFreeFormOptionResponseAndValueIsSupplied_WhenICheckWhetherResponseFailsValidation_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        var freeFormResponse = testItems.Fixture.Build<DataShareRequestQuestionAnswerPartResponseFreeForm>()
            .With(x => x.EnteredValue, "some response")
            .Create();

        var questionAnswerPartResponse = testItems.Fixture.Build<QuestionAnswerPartResponseForValidation>()
            .With(x => x.QuestionAnswerPartResponse, freeFormResponse)
            .Create();

        var result = testItems.CountryValueHasNotBeenSuppliedValidationRule.ResponseFailsValidation(questionAnswerPartResponse);

        Assert.That(result, Is.False);
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var countryValueHasNotBeenSuppliedValidationRule = new CountryValueHasNotBeenSuppliedValidationRule();

        return new TestItems(
            fixture,
            countryValueHasNotBeenSuppliedValidationRule);
    }

    private class TestItems(
        IFixture fixture,
        IValidationRule CountryValueHasNotBeenSuppliedValidationRule)
    {
        public IFixture Fixture { get; } = fixture;
        public IValidationRule CountryValueHasNotBeenSuppliedValidationRule { get; } = CountryValueHasNotBeenSuppliedValidationRule;
    }
    #endregion
}