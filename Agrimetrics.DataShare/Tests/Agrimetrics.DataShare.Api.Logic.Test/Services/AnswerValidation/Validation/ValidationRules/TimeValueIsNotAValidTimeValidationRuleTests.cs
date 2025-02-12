using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation.ValidationRules;

[TestFixture]
public class TimeValueIsNotAValidTimeValidationRuleTests
{
    #region Accepts() Tests
    [Test]
    public void GivenANullValidationRule_WhenICheckAccepts_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.TimeValueIsNotAValidTimeValidationRule.Accepts(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("validationRule"));
    }

    [Test]
    public void GivenAValidationRuleWithTimeValueIsNotAValidTimeRuleId_WhenICheckAccepts_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        var testValidationRule = testItems.Fixture.Build<QuestionPartAnswerValidationRuleModelData>()
            .With(x => x.QuestionPartAnswerValidationRule_Rule, QuestionPartAnswerValidationRuleId.FreeForm_Time_NotAValidTime)
            .Create();

        var result = testItems.TimeValueIsNotAValidTimeValidationRule.Accepts(testValidationRule);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenAValidationRuleNotWithTimeValueIsNotAValidTimeRuleId_WhenICheckAccepts_ThenFalseIsReturned(
        [ValueSource(nameof(NonApplicableValidationRuleIds))] QuestionPartAnswerValidationRuleId validationRuleId)
    {
        var testItems = CreateTestItems();

        var testValidationRule = testItems.Fixture.Build<QuestionPartAnswerValidationRuleModelData>()
            .With(x => x.QuestionPartAnswerValidationRule_Rule, validationRuleId)
            .Create();

        var result = testItems.TimeValueIsNotAValidTimeValidationRule.Accepts(testValidationRule);

        Assert.That(result, Is.False);
    }

    private static IEnumerable<QuestionPartAnswerValidationRuleId> NonApplicableValidationRuleIds =>
        Enum.GetValues<QuestionPartAnswerValidationRuleId>().Except([QuestionPartAnswerValidationRuleId.FreeForm_Time_NotAValidTime]);
    #endregion

    #region ResponseFailsValidation() Tests
    [Test]
    public void GivenANullQuestionAnswerPart_WhenICheckWhetherResponseFailsValidation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.TimeValueIsNotAValidTimeValidationRule.ResponseFailsValidation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionAnswerPartResponseForValidation"));
    }

    [Test]
    public void GivenANonFreeFormOptionResponse_WhenICheckWhetherResponseFailsValidation_ThenAnInvalidOperationExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var questionAnswerPartResponse = testItems.Fixture.Create<IQuestionAnswerPartResponseForValidation>();

        Assert.That(() => testItems.TimeValueIsNotAValidTimeValidationRule.ResponseFailsValidation(questionAnswerPartResponse),
            Throws.InvalidOperationException.With.Message.EqualTo("QuestionAnswerPartResponse is not of expected type for validation rule TimeValueIsNotAValidTimeValidationRule"));
    }

    [Test]
    public void GivenAFreeFormOptionResponseWithNoValueSupplied_WhenICheckWhetherResponseFailsValidation_ThenFalseIsReturned(
        [Values("", "  ")] string enteredValue)
    {
        var testItems = CreateTestItems();

        var freeFormResponse = testItems.Fixture.Build<DataShareRequestQuestionAnswerPartResponseFreeForm>()
            .With(x => x.EnteredValue, enteredValue)
            .Create();

        var questionAnswerPartResponse = testItems.Fixture.Build<QuestionAnswerPartResponseForValidation>()
            .With(x => x.QuestionAnswerPartResponse, freeFormResponse)
            .Create();

        var result = testItems.TimeValueIsNotAValidTimeValidationRule.ResponseFailsValidation(questionAnswerPartResponse);

        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenAFreeFormOptionResponseWithANonTimeValueSupplied_WhenICheckWhetherResponseFailsValidation_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        TimeSpan? parsedValue;
        testItems.MockResponseFormatter.Setup(x => x.TryFormatTimeResponse("test non time response", out parsedValue))
            .Returns(false);

        var freeFormResponse = testItems.Fixture.Build<DataShareRequestQuestionAnswerPartResponseFreeForm>()
            .With(x => x.EnteredValue, "test non time response")
            .Create();

        var questionAnswerPartResponse = testItems.Fixture.Build<QuestionAnswerPartResponseForValidation>()
            .With(x => x.QuestionAnswerPartResponse, freeFormResponse)
            .Create();

        var result = testItems.TimeValueIsNotAValidTimeValidationRule.ResponseFailsValidation(questionAnswerPartResponse);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenAFreeFormOptionResponseWithATimeValueSupplied_WhenICheckWhetherResponseFailsValidation_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        TimeSpan? parsedValue;
        testItems.MockResponseFormatter.Setup(x => x.TryFormatTimeResponse("test time response", out parsedValue))
            .Returns(true);

        var freeFormResponse = testItems.Fixture.Build<DataShareRequestQuestionAnswerPartResponseFreeForm>()
            .With(x => x.EnteredValue, "test time response")
            .Create();

        var questionAnswerPartResponse = testItems.Fixture.Build<QuestionAnswerPartResponseForValidation>()
            .With(x => x.QuestionAnswerPartResponse, freeFormResponse)
            .Create();

        var result = testItems.TimeValueIsNotAValidTimeValidationRule.ResponseFailsValidation(questionAnswerPartResponse);

        Assert.That(result, Is.False);
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockResponseFormatter = Mock.Get(fixture.Create<IResponseFormatter>());

        var timeValueIsNotAValidTimeValidationRule = new TimeValueIsNotAValidTimeValidationRule(
            mockResponseFormatter.Object);

        return new TestItems(
            fixture,
            timeValueIsNotAValidTimeValidationRule,
            mockResponseFormatter);
    }

    private class TestItems(
        IFixture fixture,
        IValidationRule timeValueIsNotAValidTimeValidationRule,
        Mock<IResponseFormatter> mockResponseFormatter)
    {
        public IFixture Fixture { get; } = fixture;
        public IValidationRule TimeValueIsNotAValidTimeValidationRule { get; } = timeValueIsNotAValidTimeValidationRule;
        public Mock<IResponseFormatter> MockResponseFormatter { get; } = mockResponseFormatter;
    }
    #endregion
}