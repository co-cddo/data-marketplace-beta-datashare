﻿using Agrimetrics.DataShare.Api.Core.SystemProxies;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation.ValidationRules;

[TestFixture]
public class DateTimeCannotBeInTheFutureValidationRuleTests
{
    #region Accepts() Tests
    [Test]
    public void GivenANullValidationRule_WhenICheckAccepts_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.DateTimeCannotBeInTheFutureValidationRule.Accepts(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("validationRule"));
    }

    [Test]
    public void GivenAValidationRuleWithDateTimeCannotBeInTheFutureRuleId_WhenICheckAccepts_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        var testValidationRule = testItems.Fixture.Build<QuestionPartAnswerValidationRuleModelData>()
            .With(x => x.QuestionPartAnswerValidationRule_Rule, QuestionPartAnswerValidationRuleId.FreeForm_DateTime_DateTimeCannotBeInTheFuture)
            .Create();

        var result = testItems.DateTimeCannotBeInTheFutureValidationRule.Accepts(testValidationRule);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenAValidationRuleNotWithDateTimeCannotBeInTheFutureRuleId_WhenICheckAccepts_ThenFalseIsReturned(
        [ValueSource(nameof(NonApplicableValidationRuleIds))] QuestionPartAnswerValidationRuleId validationRuleId)
    {
        var testItems = CreateTestItems();

        var testValidationRule = testItems.Fixture.Build<QuestionPartAnswerValidationRuleModelData>()
            .With(x => x.QuestionPartAnswerValidationRule_Rule, validationRuleId)
            .Create();

        var result = testItems.DateTimeCannotBeInTheFutureValidationRule.Accepts(testValidationRule);

        Assert.That(result, Is.False);
    }

    private static IEnumerable<QuestionPartAnswerValidationRuleId> NonApplicableValidationRuleIds =>
        Enum.GetValues<QuestionPartAnswerValidationRuleId>().Except([QuestionPartAnswerValidationRuleId.FreeForm_DateTime_DateTimeCannotBeInTheFuture]);
    #endregion

    #region ResponseFailsValidation() Tests
    [Test]
    public void GivenANullQuestionAnswerPart_WhenICheckWhetherResponseFailsValidation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.DateTimeCannotBeInTheFutureValidationRule.ResponseFailsValidation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionAnswerPartResponseForValidation"));
    }

    [Test]
    public void GivenANonFreeFormOptionResponse_WhenICheckWhetherResponseFailsValidation_ThenAnInvalidOperationExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var questionAnswerPartResponse = testItems.Fixture.Create<IQuestionAnswerPartResponseForValidation>();

        Assert.That(() => testItems.DateTimeCannotBeInTheFutureValidationRule.ResponseFailsValidation(questionAnswerPartResponse),
            Throws.InvalidOperationException.With.Message.EqualTo("QuestionAnswerPartResponse is not of expected type for validation rule DateTimeCannotBeInTheFutureValidationRule"));
    }

    [Test]
    public void GivenAFreeFormOptionResponseWithAnInvalidDateTime_WhenICheckWhetherResponseFailsValidation_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        DateTime? parsedValue;
        testItems.MockResponseFormatter.Setup(x => x.TryFormatDateTimeResponse(It.IsAny<string>(), out parsedValue))
            .Returns(false);

        var freeFormResponse = testItems.Fixture.Create<DataShareRequestQuestionAnswerPartResponseFreeForm>();

        var questionAnswerPartResponse = testItems.Fixture.Build<QuestionAnswerPartResponseForValidation>()
            .With(x => x.QuestionAnswerPartResponse, freeFormResponse)
            .Create();

        var result = testItems.DateTimeCannotBeInTheFutureValidationRule.ResponseFailsValidation(questionAnswerPartResponse);

        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenAFreeFormOptionResponseWithDateTimeValueThatIsInTheFuture_WhenICheckWhetherResponseFailsValidation_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        var testTimeNow = new DateTime(2024, 12, 25, 13, 45, 59, 999);
        testItems.MockClock.SetupGet(x => x.LocalNow).Returns(testTimeNow);

        DateTime? parsedValue = testTimeNow.AddSeconds(1);
        testItems.MockResponseFormatter.Setup(x => x.TryFormatDateTimeResponse(It.IsAny<string>(), out parsedValue))
            .Returns(true);

        var freeFormResponse = testItems.Fixture.Create<DataShareRequestQuestionAnswerPartResponseFreeForm>();

        var questionAnswerPartResponse = testItems.Fixture.Build<QuestionAnswerPartResponseForValidation>()
            .With(x => x.QuestionAnswerPartResponse, freeFormResponse)
            .Create();

        var result = testItems.DateTimeCannotBeInTheFutureValidationRule.ResponseFailsValidation(questionAnswerPartResponse);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenAFreeFormOptionResponseWithDateTimeValueThatIsNotInTheFuture_WhenICheckWhetherResponseFailsValidation_ThenTrueIsReturned(
        [Values(0, 1)] int numberOfSecondsFromNow)
    {
        var testItems = CreateTestItems();

        var testTimeNow = new DateTime(2024, 12, 25, 13, 45, 59, 999);
        testItems.MockClock.SetupGet(x => x.LocalNow).Returns(testTimeNow);

        DateTime? parsedValue = testTimeNow.AddSeconds(-numberOfSecondsFromNow);
        testItems.MockResponseFormatter.Setup(x => x.TryFormatDateTimeResponse(It.IsAny<string>(), out parsedValue))
            .Returns(true);

        var freeFormResponse = testItems.Fixture.Create<DataShareRequestQuestionAnswerPartResponseFreeForm>();

        var questionAnswerPartResponse = testItems.Fixture.Build<QuestionAnswerPartResponseForValidation>()
            .With(x => x.QuestionAnswerPartResponse, freeFormResponse)
            .Create();

        var result = testItems.DateTimeCannotBeInTheFutureValidationRule.ResponseFailsValidation(questionAnswerPartResponse);

        Assert.That(result, Is.False);
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockResponseFormatter = Mock.Get(fixture.Create<IResponseFormatter>());
        var mockClock = Mock.Get(fixture.Create<IClock>());

        var dateTimeValueIsNotAValidDateTimeValidationRule = new DateTimeCannotBeInTheFutureValidationRule(
            mockResponseFormatter.Object,
            mockClock.Object);

        return new TestItems(
            fixture,
            dateTimeValueIsNotAValidDateTimeValidationRule,
            mockResponseFormatter,
            mockClock);
    }

    private class TestItems(
        IFixture fixture,
        IValidationRule dateTimeValueIsNotAValidDateTimeValidationRule,
        Mock<IResponseFormatter> mockResponseFormatter,
        Mock<IClock> mockClock)
    {
        public IFixture Fixture { get; } = fixture;
        public IValidationRule DateTimeCannotBeInTheFutureValidationRule { get; } = dateTimeValueIsNotAValidDateTimeValidationRule;
        public Mock<IResponseFormatter> MockResponseFormatter { get; } = mockResponseFormatter;
        public Mock<IClock> MockClock { get; } = mockClock;
    }
    #endregion
}