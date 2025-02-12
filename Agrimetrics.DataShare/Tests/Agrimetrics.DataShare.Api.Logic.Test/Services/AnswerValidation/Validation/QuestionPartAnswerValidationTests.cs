using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation;
using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation;

[TestFixture]
public class QuestionPartAnswerValidationTests
{
    #region ValidateQuestionPartAnswer() Tests
    [Test]
    public void GivenANullQuestionPartAnswer_WhenIValidateQuestionPartAnswer_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.QuestionPartAnswerValidation.ValidateQuestionPartAnswer(
                null!,
                testItems.Fixture.Create<QuestionPartAnswerValidationRuleSetModelData>()),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionPartAnswer"));
    }

    [Test]
    public void GivenANullValidationRuleSet_WhenIValidateQuestionPartAnswer_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.QuestionPartAnswerValidation.ValidateQuestionPartAnswer(
                testItems.Fixture.Create<DataShareRequestQuestionAnswerPart>(),
                null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("validationRuleSet"));
    }

    [Test]
    public void GivenAValidationRuleSetContainingARuleThatCannotBeMatched_WhenIValidateQuestionPartAnswer_ThenAnExceptionIsThrown()
    {
        var testItems = CreateTestItems();


        Assert.That(() => testItems.QuestionPartAnswerValidation.ValidateQuestionPartAnswer(
                testItems.Fixture.Create<DataShareRequestQuestionAnswerPart>(),
                testItems.Fixture.Create<QuestionPartAnswerValidationRuleSetModelData>()),
            Throws.Exception.With.Message.EqualTo("Failed to identify validation rule"));
    }

    [Test]
    public void GivenAValidationRuleSetContainingARuleThatHasMultipleMatches_WhenIValidateQuestionPartAnswer_ThenAnExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        testItems.MockValidationRule1.Setup(x => x.Accepts(It.IsAny<QuestionPartAnswerValidationRuleModelData>())).Returns(true);
        testItems.MockValidationRule3.Setup(x => x.Accepts(It.IsAny<QuestionPartAnswerValidationRuleModelData>())).Returns(true);

        Assert.That(() => testItems.QuestionPartAnswerValidation.ValidateQuestionPartAnswer(
                testItems.Fixture.Create<DataShareRequestQuestionAnswerPart>(),
                testItems.Fixture.Create<QuestionPartAnswerValidationRuleSetModelData>()),
            Throws.Exception.With.Message.EqualTo("Failed to identify unique validation rule"));
    }

    [Theory]
    public void GivenADataShareRequestQuestionAnswerPart_WhenIValidateQuestionPartAnswer_ThenTheOptionalityOfThatQuestionIsProvidesToTheValidationRuleForEachResponseInTheAnswer(
        bool answerIsOptional)
    {
        var testItems = CreateTestItems();

        testItems.MockValidationRule1.Setup(x => x.Accepts(It.IsAny<QuestionPartAnswerValidationRuleModelData>())).Returns(true);

        var testAnswerPartResponses = testItems.Fixture.CreateMany<DataShareRequestQuestionAnswerPartResponseBase>(5).ToList();

        var testQuestionPartAnswer = testItems.Fixture.Build<DataShareRequestQuestionAnswerPart>()
            .With(x => x.AnswerPartResponses, testAnswerPartResponses)
            .Create();

        var testQuestionPartAnswerValidationRuleSetModelData = testItems.Fixture.Build<QuestionPartAnswerValidationRuleSetModelData>()
            .With(x => x.QuestionPartAnswerValidationRuleSet_AnswerIsOptional, answerIsOptional)
            .Create();

        testItems.QuestionPartAnswerValidation.ValidateQuestionPartAnswer(
            testQuestionPartAnswer,
            testQuestionPartAnswerValidationRuleSetModelData);

        Assert.Multiple(() =>
        {
            testItems.MockValidationRule1.Verify(x => x.ResponseFailsValidation(
                    It.Is<IQuestionAnswerPartResponseForValidation>(response => response.QuestionAnswerPartIsOptional == answerIsOptional)),
                Times.AtLeastOnce);

            testItems.MockValidationRule1.Verify(x => x.ResponseFailsValidation(
                    It.Is<IQuestionAnswerPartResponseForValidation>(response => response.QuestionAnswerPartIsOptional != answerIsOptional)),
                Times.Never);
        });
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithResponsesThatFailValidationRules_WhenIValidateQuestionPartAnswer_ThenTheValidationErrorsOfThoseRulesAreReturned()
    {
        var testItems = CreateTestItems();

        var testValidationRule1 = testItems.Fixture.Build<QuestionPartAnswerValidationRuleModelData>()
            .With(x => x.QuestionPartAnswerValidationRule_QuestionErrorText, (string?)null)
            .With(x => x.QuestionPartAnswerValidationRule_RuleErrorText, "rule 1 error").Create();

        var testValidationRule2 = testItems.Fixture.Build<QuestionPartAnswerValidationRuleModelData>()
            .With(x => x.QuestionPartAnswerValidationRule_QuestionErrorText, (string?)null)
            .With(x => x.QuestionPartAnswerValidationRule_QuestionErrorText, "rule 2 error").Create();

        var testValidationRule3 = testItems.Fixture.Build<QuestionPartAnswerValidationRuleModelData>()
            .With(x => x.QuestionPartAnswerValidationRule_QuestionErrorText, "question 3 error")
            .With(x => x.QuestionPartAnswerValidationRule_RuleErrorText, "rule 3 error").Create();

        var testValidationRuleSet = testItems.Fixture.Build<QuestionPartAnswerValidationRuleSetModelData>()
            .With(x => x.QuestionPartAnswerValidationRuleSet_ValidationRules, [testValidationRule1, testValidationRule2, testValidationRule3])
            .Create();

        testItems.MockValidationRule1.Setup(x => x.Accepts(testValidationRule1)).Returns(true);
        testItems.MockValidationRule2.Setup(x => x.Accepts(testValidationRule2)).Returns(true);
        testItems.MockValidationRule3.Setup(x => x.Accepts(testValidationRule3)).Returns(true);

        var testAnswerPartResponse1 = testItems.Fixture.Build<DataShareRequestQuestionAnswerPartResponseBase>().With(x => x.OrderWithinAnswerPart, 10).Create();
        var testAnswerPartResponse2 = testItems.Fixture.Build<DataShareRequestQuestionAnswerPartResponseBase>().With(x => x.OrderWithinAnswerPart, 20).Create();
        var testAnswerPartResponse3 = testItems.Fixture.Build<DataShareRequestQuestionAnswerPartResponseBase>().With(x => x.OrderWithinAnswerPart, 30).Create();

        var testQuestionPartId = Guid.Parse("B212828C-912D-4E47-91BE-E3B220B43D3B");

        var testQuestionPartAnswer = testItems.Fixture.Build<DataShareRequestQuestionAnswerPart>()
            .With(x => x.AnswerPartResponses, [testAnswerPartResponse1, testAnswerPartResponse2, testAnswerPartResponse3])
            .With(x => x.QuestionPartId, testQuestionPartId)
            .Create();

        // Response 1 fails rules 1 and 3
        testItems.MockValidationRule1.Setup(x => x.ResponseFailsValidation(
            It.Is<IQuestionAnswerPartResponseForValidation>(response => response.QuestionAnswerPartResponse == testAnswerPartResponse1))).Returns(true);
        testItems.MockValidationRule3.Setup(x => x.ResponseFailsValidation(
            It.Is<IQuestionAnswerPartResponseForValidation>(response => response.QuestionAnswerPartResponse == testAnswerPartResponse1))).Returns(true);

        // Response 2 does not fail any rules

        // Response 3 fails rules 1, 2 and 3
        testItems.MockValidationRule1.Setup(x => x.ResponseFailsValidation(
            It.Is<IQuestionAnswerPartResponseForValidation>(response => response.QuestionAnswerPartResponse == testAnswerPartResponse3))).Returns(true);
        testItems.MockValidationRule2.Setup(x => x.ResponseFailsValidation(
            It.Is<IQuestionAnswerPartResponseForValidation>(response => response.QuestionAnswerPartResponse == testAnswerPartResponse3))).Returns(true);
        testItems.MockValidationRule3.Setup(x => x.ResponseFailsValidation(
            It.Is<IQuestionAnswerPartResponseForValidation>(response => response.QuestionAnswerPartResponse == testAnswerPartResponse3))).Returns(true);

        var result = testItems.QuestionPartAnswerValidation.ValidateQuestionPartAnswer(
            testQuestionPartAnswer,
            testValidationRuleSet).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(2).Items);

            Assert.That(result.Any(x => x.QuestionPartId == testQuestionPartId &&
                                        x is {ResponseOrderWithinAnswerPart: 10, ValidationErrors.Count: 2} &&
                                        x.ValidationErrors.Contains("rule 1 error") &&
                                        x.ValidationErrors.Contains("question 3 error")));

            Assert.That(result.Any(x => x.QuestionPartId == testQuestionPartId &&
                                        x is {ResponseOrderWithinAnswerPart: 30, ValidationErrors.Count: 3} &&
                                        x.ValidationErrors.Contains("rule 1 error") &&
                                        x.ValidationErrors.Contains("rule 2 error") &&
                                        x.ValidationErrors.Contains("question 3 error")));
        });
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithResponsesThatFailSystemValidationRules_WhenIValidateQuestionPartAnswer_ThenTheValidationErrorsOfThoseRulesAreReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionPartId = Guid.Parse("B212828C-912D-4E47-91BE-E3B220B43D3B");

        var testQuestionPartAnswer = testItems.Fixture.Build<DataShareRequestQuestionAnswerPart>()
            .With(x => x.QuestionPartId, testQuestionPartId)
            .Create();

        testItems.MockSystemValidationRule1.Setup(x => x.Accepts(testQuestionPartAnswer)).Returns(true);
        testItems.MockSystemValidationRule2.Setup(x => x.Accepts(testQuestionPartAnswer)).Returns(true);
        testItems.MockSystemValidationRule3.Setup(x => x.Accepts(testQuestionPartAnswer)).Returns(true);

        // Answer fails rules 1 and 3
        var validationErrorSetA = CreateTestQuestionAnswerPartValidationErrorSet(
            responseValidationErrors:
            [
                CreateTestQuestionAnswerPartResponseValidationErrorSet(
                    responseOrderWithinAnswerPart: 10,
                    validationErrors: ["error A", "error B"])
            ]);
        testItems.MockSystemValidationRule1.Setup(x => x.ResponseFailsValidation(
            testQuestionPartAnswer, out validationErrorSetA)).Returns(true);

        var validationErrorSetB = CreateTestQuestionAnswerPartValidationErrorSet(
            responseValidationErrors:
            [
                CreateTestQuestionAnswerPartResponseValidationErrorSet(
                    responseOrderWithinAnswerPart: 30,
                    validationErrors: ["error C"])
            ]);
        testItems.MockSystemValidationRule3.Setup(x => x.ResponseFailsValidation(
            testQuestionPartAnswer, out validationErrorSetB)).Returns(true);
        
        var emptyValidationRuleSetModelData = new QuestionPartAnswerValidationRuleSetModelData
        {
            QuestionPartAnswerValidationRuleSet_ValidationRules = []
        };

        var result = testItems.QuestionPartAnswerValidation.ValidateQuestionPartAnswer(
            testQuestionPartAnswer,
            emptyValidationRuleSetModelData).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(2).Items);

            Assert.That(result.Any(x => x.QuestionPartId == testQuestionPartId &&
                                        x is {ResponseOrderWithinAnswerPart: 10, ValidationErrors.Count: 2} &&
                                        x.ValidationErrors.Contains("error A") &&
                                        x.ValidationErrors.Contains("error B")));

            Assert.That(result.Any(x => x.QuestionPartId == testQuestionPartId &&
                                        x is {ResponseOrderWithinAnswerPart: 30, ValidationErrors.Count: 1} &&
                                        x.ValidationErrors.Contains("error C")));
        });
    }
    #endregion

    #region Test Data Creation
    private static IQuestionAnswerPartResponseValidationErrorSet CreateTestQuestionAnswerPartResponseValidationErrorSet(
        int? responseOrderWithinAnswerPart = null,
        IEnumerable<string>? validationErrors = null)
    {
        var mockQuestionAnswerPartResponseValidationErrorSet = new Mock<IQuestionAnswerPartResponseValidationErrorSet>();

        mockQuestionAnswerPartResponseValidationErrorSet.Setup(x => x.ResponseOrderWithinAnswerPart)
            .Returns(responseOrderWithinAnswerPart ?? 0);

        mockQuestionAnswerPartResponseValidationErrorSet.Setup(x => x.ValidationErrors)
            .Returns(validationErrors ?? []);

        return mockQuestionAnswerPartResponseValidationErrorSet.Object;
    }

    private static IQuestionAnswerPartValidationErrorSet CreateTestQuestionAnswerPartValidationErrorSet(
        IEnumerable<IQuestionAnswerPartResponseValidationErrorSet>? responseValidationErrors = null)
    {
        var mockQuestionAnswerPartValidationErrorSet = new Mock<IQuestionAnswerPartValidationErrorSet>();

        mockQuestionAnswerPartValidationErrorSet.SetupGet(x => x.ResponseValidationErrors)
            .Returns(responseValidationErrors ?? []);

        return mockQuestionAnswerPartValidationErrorSet.Object;
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockAnswerValidationRule1 = Mock.Get(fixture.Create<IValidationRule>());
        var mockAnswerValidationRule2 = Mock.Get(fixture.Create<IValidationRule>());
        var mockAnswerValidationRule3 = Mock.Get(fixture.Create<IValidationRule>());

        var mockSystemValidationRule1 = Mock.Get(fixture.Create<ISystemValidationRule>());
        var mockSystemValidationRule2 = Mock.Get(fixture.Create<ISystemValidationRule>());
        var mockSystemValidationRule3 = Mock.Get(fixture.Create<ISystemValidationRule>());

        var questionPartAnswerValidation = new QuestionPartAnswerValidation(
            [ mockAnswerValidationRule1.Object, mockAnswerValidationRule2.Object, mockAnswerValidationRule3.Object],
            [ mockSystemValidationRule1.Object, mockSystemValidationRule2.Object, mockSystemValidationRule3.Object]);

        return new TestItems(
            fixture,
            questionPartAnswerValidation,
            mockAnswerValidationRule1,
            mockAnswerValidationRule2,
            mockAnswerValidationRule3,
            mockSystemValidationRule1,
            mockSystemValidationRule2,
            mockSystemValidationRule3);
    }

    private class TestItems(
        IFixture fixture,
        IQuestionPartAnswerValidation questionPartAnswerValidation,
        Mock<IValidationRule> mockValidationRule1,
        Mock<IValidationRule> mockValidationRule2,
        Mock<IValidationRule> mockValidationRule3,
        Mock<ISystemValidationRule> mockSystemValidationRule1,
        Mock<ISystemValidationRule> mockSystemValidationRule2,
        Mock<ISystemValidationRule> mockSystemValidationRule3)
    {
        public IFixture Fixture { get; } = fixture;
        public IQuestionPartAnswerValidation QuestionPartAnswerValidation { get; } = questionPartAnswerValidation;
        public Mock<IValidationRule> MockValidationRule1 { get; } = mockValidationRule1;
        public Mock<IValidationRule> MockValidationRule2 { get; } = mockValidationRule2;
        public Mock<IValidationRule> MockValidationRule3 { get; } = mockValidationRule3;
        public Mock<ISystemValidationRule> MockSystemValidationRule1 { get; } = mockSystemValidationRule1;
        public Mock<ISystemValidationRule> MockSystemValidationRule2 { get; } = mockSystemValidationRule2;
        public Mock<ISystemValidationRule> MockSystemValidationRule3 { get; } = mockSystemValidationRule3;
    }
    #endregion
}