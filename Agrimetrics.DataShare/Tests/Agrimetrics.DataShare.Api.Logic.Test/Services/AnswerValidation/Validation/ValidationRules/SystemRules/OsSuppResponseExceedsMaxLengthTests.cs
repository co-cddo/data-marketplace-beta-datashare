using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;
using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

[TestFixture]
public class OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRuleTests
{
    #region Accepts() Tests
    [Test]
    public void GivenANullDataShareRequestQuestionAnswerPart_WhenICallAccepts_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule.Accepts(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionAnswerPart"));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithOptionSelectionInputType_WhenICallAccepts_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPartResponse = CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
            [testQuestionAnswerPartResponse]);

        var result = testItems.OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule.Accepts(
            testQuestionAnswerPart);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithNonOptionSelectionInputType_WhenICallAccepts_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
            [new DataShareRequestQuestionAnswerPartResponseFreeForm()]);

        var result = testItems.OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule.Accepts(
            testQuestionAnswerPart);

        Assert.That(result, Is.False);
    }
    #endregion

    #region ResponseFailsValidation() Tests
    [Test]
    public void GivenANullDataShareRequestQuestionAnswerPart_WhenICallResponseFailsValidation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
                null!,
                out _),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionAnswerPart"));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithResponseOfNonOptionSelectionInputType_WhenICallResponseFailsValidation_ThenAnInvalidOperationExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(),
            new DataShareRequestQuestionAnswerPartResponseFreeForm(),
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(),
        ]);

        Assert.That(() => testItems.OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
                testQuestionAnswerPart,
                out _),
            Throws.InvalidOperationException.With.Message.EqualTo("Unable to validate whether OptionSelection Supplementary Response Exceeds Maximum Length for response of incorrect type"));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithNoSelectedOptionItems_WhenICallResponseFailsValidation_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:1, selectedOptionItems:[]),
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:2, selectedOptionItems:[]),
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:3, selectedOptionItems:[])
        ]);

        var result = testItems.OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithNoSelectedOptionItems_WhenICallResponseFailsValidation_ThenAnEmptyValidationErrorSetIsCreated()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:1, selectedOptionItems:[]),
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:2, selectedOptionItems:[]),
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:3, selectedOptionItems:[])
        ]);

        testItems.OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out var validationErrorSet);

        Assert.That(validationErrorSet.ResponseValidationErrors, Is.Empty);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithNoSelectedOptionsItemsWithSupplementaryAnswerPartsThatExceedTheMaximumLength_WhenICallResponseFailsValidation_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter
            .Setup(x => x.GetMaximumLengthOfSupplementaryTextResponse())
            .Returns(20);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:1, selectedOptionItems:
                [
                    // No supplementary answer
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: null),

                    // Supplementary answer with no entered value
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue:null)])),
                ]),

            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:2, selectedOptionItems:
                [
                    // Supplementary answer with empty entered value
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue:string.Empty)])),

                    // Supplementary answer with non-freeform answer
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption()]))
                ]),

            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:3, selectedOptionItems:
            [
                // Supplementary answer with short enough entered value
                CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                    supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                        answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue: "01234567890123456789")])),
            ]),
        ]);

        var result = testItems.OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithNoDuplicatedResponses_WhenICallResponseFailsValidation_ThenAnEmptyValidationErrorSetIsCreated()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter
            .Setup(x => x.GetMaximumLengthOfSupplementaryTextResponse())
            .Returns(20);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:1, selectedOptionItems:
                [
                    // No supplementary answer
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: null),

                    // Supplementary answer with no entered value
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue:null)])),
                ]),

            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:2, selectedOptionItems:
                [
                    // Supplementary answer with empty entered value
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue:string.Empty)])),

                    // Supplementary answer with non-freeform answer
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption()]))
                ]),

            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:3, selectedOptionItems:
            [
                // Supplementary answer with short enough entered value
                CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                    supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                        answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue: "01234567890123456789")])),
            ]),
        ]);

        testItems.OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out var validationErrorSet);

        Assert.That(validationErrorSet.ResponseValidationErrors, Is.Empty);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithSelectedOptionsItemsWithSupplementaryAnswerPartsThatExceedTheMaximumLength_WhenICallResponseFailsValidation_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter
            .Setup(x => x.GetMaximumLengthOfSupplementaryTextResponse())
            .Returns(20);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:1, selectedOptionItems:
                [
                    // No supplementary answer
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: null),

                    // Supplementary answer with no entered value
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue:null)])),

                    // Supplementary answer with entered value that is too long
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue: "01234567890123456789Z")])),
                ]),

            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:2, selectedOptionItems:
                [
                    // Supplementary answer with empty entered value
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue:string.Empty)])),

                    // Supplementary answer with non-freeform answer
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption()]))
                ]),

            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:3, selectedOptionItems:
            [
                // Supplementary answer with short enough entered value
                CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                    supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                        answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue: "01234567890123456789")])),
            ]),
        ]);

        var result = testItems.OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithNoDuplicatedResponses_WhenICallResponseFailsValidation_ThenASingleValidationErrorIsCreatedForEachDuplicatedResponse()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter
            .Setup(x => x.GetMaximumLengthOfSupplementaryTextResponse())
            .Returns(20);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:1, selectedOptionItems:
                [
                    // No supplementary answer
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: null),

                    // Supplementary answer with no entered value
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue:null)])),

                    // Supplementary answer with entered value that is too long
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue: "01234567890123456789X")]))
                ]),

            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:2, selectedOptionItems:
                [
                    // Supplementary answer with empty entered value
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue:string.Empty)])),

                    // Supplementary answer with non-freeform answer
                    CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                        supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                            answerPartResponses: [ CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption()]))
                ]),

            CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(orderWithinAnswerPart:3, selectedOptionItems:
            [
                // Supplementary answer with short enough entered value
                CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                    supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                        answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue: "01234567890123456789")])),

                // Supplementary answer with entered value that is too long
                CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
                    supplementaryQuestionAnswerPart: CreateTestSupplementaryAnswerPart(
                        answerPartResponses: [ CreateTestSupplementaryFreeFormAnswer(enteredValue: "01234567890123456789Z")])),
            ]),
        ]);

        testItems.OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out var validationErrorSet);

        Assert.Multiple(() =>
        {
            Assert.That(validationErrorSet.ResponseValidationErrors, Has.Exactly(2).Items);

            Assert.That(validationErrorSet.ResponseValidationErrors.Any(x => x.ResponseOrderWithinAnswerPart == 1), Is.True);
            Assert.That(validationErrorSet.ResponseValidationErrors.Any(x => x.ResponseOrderWithinAnswerPart == 3), Is.True);

            Assert.That(validationErrorSet.ResponseValidationErrors.All(x => x.ValidationErrors.Single().Equals(
                $"Supplementary Answer exceeds maximum length (20 characters)")), Is.True);
        });
    }
    #endregion

    #region Test Data Creation
    private static DataShareRequestQuestionAnswerPartResponseFreeForm CreateTestSupplementaryFreeFormAnswer(
        string? enteredValue = null)
    {
        return new DataShareRequestQuestionAnswerPartResponseFreeForm
        {
            EnteredValue = enteredValue ?? string.Empty
        };
    }

    private static DataShareRequestQuestionAnswerPart CreateTestSupplementaryAnswerPart(
        List<DataShareRequestQuestionAnswerPartResponseBase>? answerPartResponses = null)
    {
        return new DataShareRequestQuestionAnswerPart
        {
            AnswerPartResponses = answerPartResponses ?? []
        };
    }

    private static DataShareRequestQuestionAnswerPartResponseSelectionOptionItem CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOptionItem(
            DataShareRequestQuestionAnswerPart? supplementaryQuestionAnswerPart = null)
    {
        return new DataShareRequestQuestionAnswerPartResponseSelectionOptionItem
        {
            SupplementaryQuestionAnswerPart = supplementaryQuestionAnswerPart
        };
    }

    private static DataShareRequestQuestionAnswerPartResponseSelectionOption CreateTestDataShareRequestQuestionAnswerPartResponseSelectionOption(
        int? orderWithinAnswerPart = null,
        List<DataShareRequestQuestionAnswerPartResponseSelectionOptionItem>? selectedOptionItems = null)
    {
        return new DataShareRequestQuestionAnswerPartResponseSelectionOption
        {
            OrderWithinAnswerPart = orderWithinAnswerPart ?? 0,
            SelectedOptionItems = selectedOptionItems ?? []
        };
    }

    private static DataShareRequestQuestionAnswerPart CreateTestDataShareRequestQuestionAnswerPart(
        List<DataShareRequestQuestionAnswerPartResponseBase>? answerPartResponses = null)
    {
        return new DataShareRequestQuestionAnswerPart
        {
            AnswerPartResponses = answerPartResponses ?? []
        };
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockInputConstraintConfigurationPresenter = Mock.Get(fixture.Freeze<IInputConstraintConfigurationPresenter>());

        var optionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule = new OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule(
            mockInputConstraintConfigurationPresenter.Object);

        return new TestItems(
            optionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule,
            mockInputConstraintConfigurationPresenter);
    }

    private class TestItems(
        IOptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule optionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule,
        Mock<IInputConstraintConfigurationPresenter> mockInputConstraintConfigurationPresenter)
    {
        public IOptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule OptionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule { get; } = optionSelectionSupplementaryResponseExceedsMaximumLengthValidationRule;
        public Mock<IInputConstraintConfigurationPresenter> MockInputConstraintConfigurationPresenter { get; } = mockInputConstraintConfigurationPresenter;
    }
    #endregion
}