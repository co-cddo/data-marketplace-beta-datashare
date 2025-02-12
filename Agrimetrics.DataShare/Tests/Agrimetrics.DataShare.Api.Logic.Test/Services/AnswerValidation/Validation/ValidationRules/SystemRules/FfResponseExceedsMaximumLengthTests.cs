using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.Configuration;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

[TestFixture]
public class FreeFormResponseExceedsMaximumLengthValidationRuleTests
{
    #region Accepts() Tests
    [Test]
    public void GivenANullDataShareRequestQuestionAnswerPart_WhenICallAccepts_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.FreeFormResponseExceedsMaximumLengthValidationRule.Accepts(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionAnswerPart"));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithFreeFormInputType_WhenICallAccepts_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPartResponse = CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
            [testQuestionAnswerPartResponse]);

        var result = testItems.FreeFormResponseExceedsMaximumLengthValidationRule.Accepts(
            testQuestionAnswerPart);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithNonFreeFormInputType_WhenICallAccepts_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
            [new DataShareRequestQuestionAnswerPartResponseSelectionOption()]);

        var result = testItems.FreeFormResponseExceedsMaximumLengthValidationRule.Accepts(
            testQuestionAnswerPart);

        Assert.That(result, Is.False);
    }
    #endregion

    #region ResponseFailsValidation() Tests
    [Test]
    public void GivenANullDataShareRequestQuestionAnswerPart_WhenICallResponseFailsValidation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.FreeFormResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
                null!,
                out _),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionAnswerPart"));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithResponseOfNonFreeFormInputType_WhenICallResponseFailsValidation_ThenAnInvalidOperationExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(),
            new DataShareRequestQuestionAnswerPartResponseSelectionOption(),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(),
        ]);

        Assert.That(() => testItems.FreeFormResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
                testQuestionAnswerPart,
                out _),
            Throws.InvalidOperationException.With.Message.EqualTo("Unable to validate whether FreeForm Response Exceeds Maximum Length for response of incorrect type"));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithSingleResponseThatIsWithinTheConfiguredMaximumLength_WhenICallResponseFailsValidation_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter.Setup(x => x.GetMaximumLengthOfFreeFormTextResponse())
            .Returns(10);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, multipleResponsesAreAllowed:false, enteredValue:"0123456789")
        ]);

        var result = testItems.FreeFormResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithSingleResponseThatIsWithinTheConfiguredMaximumLength_WhenICallResponseFailsValidation_ThenAnEmptyValidationErrorSetIsCreated()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter.Setup(x => x.GetMaximumLengthOfFreeFormTextResponse())
            .Returns(10);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, multipleResponsesAreAllowed:false, enteredValue:"0123456789")
        ]);

        testItems.FreeFormResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out var validationErrorSet);

        Assert.That(validationErrorSet.ResponseValidationErrors, Is.Empty);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithSingleResponseThatIsLongerThanTheConfiguredMaximumLength_WhenICallResponseFailsValidation_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter.Setup(x => x.GetMaximumLengthOfFreeFormTextResponse())
            .Returns(10);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, multipleResponsesAreAllowed:false, enteredValue:"0123456789Z")
        ]);

        var result = testItems.FreeFormResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithSingleResponseThatIsLongerThanTheConfiguredMaximumLength_WhenICallResponseFailsValidation_ThenASingleValidationErrorIsCreatedForResponseThatIsTooLong()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter.Setup(x => x.GetMaximumLengthOfFreeFormTextResponse())
            .Returns(10);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, multipleResponsesAreAllowed:false, enteredValue:"0123456789Z")
        ]);

        testItems.FreeFormResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out var validationErrorSet);

        Assert.Multiple(() =>
        {
            Assert.That(validationErrorSet.ResponseValidationErrors, Has.Exactly(1).Items);

            Assert.That(validationErrorSet.ResponseValidationErrors.Any(x => x.ResponseOrderWithinAnswerPart == 1), Is.True);

            Assert.That(validationErrorSet.ResponseValidationErrors.All(x => x.ValidationErrors.Single().Equals(
                "Value exceeds maximum length (10 characters)")), Is.True);
        });
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithMultipleResponsesThatAreAllWithinTheConfiguredMaximumLength_WhenICallResponseFailsValidation_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter.Setup(x => x.GetMaximumLengthOfFreeFormMultiResponseTextResponse())
            .Returns(10);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, multipleResponsesAreAllowed:true, enteredValue:""),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:2, multipleResponsesAreAllowed:true, enteredValue:"X"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:3, multipleResponsesAreAllowed:true, enteredValue:"0123456789"),
        ]);

        var result = testItems.FreeFormResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithMultipleResponsesThatAreAllWithinTheConfiguredMaximumLength_WhenICallResponseFailsValidation_ThenAnEmptyValidationErrorSetIsCreated()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter.Setup(x => x.GetMaximumLengthOfFreeFormMultiResponseTextResponse())
            .Returns(10);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, multipleResponsesAreAllowed:true, enteredValue:""),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:2, multipleResponsesAreAllowed:true, enteredValue:"X"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:3, multipleResponsesAreAllowed:true, enteredValue:"0123456789"),
        ]);

        testItems.FreeFormResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out var validationErrorSet);

        Assert.That(validationErrorSet.ResponseValidationErrors, Is.Empty);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithMultipleResponsesAndAtLeastOneIsLongerThanTheConfiguredMaximumLength_WhenICallResponseFailsValidation_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter.Setup(x => x.GetMaximumLengthOfFreeFormMultiResponseTextResponse())
            .Returns(10);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, multipleResponsesAreAllowed:true, enteredValue:""),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:2, multipleResponsesAreAllowed:true, enteredValue:"X"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:3, multipleResponsesAreAllowed:true, enteredValue:"0123456789Z"),
        ]);

        var result = testItems.FreeFormResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithMultipleResponsesAndAtLeastOneIsLongerThanTheConfiguredMaximumLength_WhenICallResponseFailsValidation_ThenASingleValidationErrorIsCreatedForResponseThatIsTooLong()
    {
        var testItems = CreateTestItems();

        testItems.MockInputConstraintConfigurationPresenter.Setup(x => x.GetMaximumLengthOfFreeFormMultiResponseTextResponse())
            .Returns(10);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, multipleResponsesAreAllowed:true, enteredValue:""),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:2, multipleResponsesAreAllowed:true, enteredValue:"abc def    "),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:3, multipleResponsesAreAllowed:true, enteredValue:"X"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:4, multipleResponsesAreAllowed:true, enteredValue:"0123456789Z"),
        ]);

        testItems.FreeFormResponseExceedsMaximumLengthValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out var validationErrorSet);

        Assert.Multiple(() =>
        {
            Assert.That(validationErrorSet.ResponseValidationErrors, Has.Exactly(2).Items);

            Assert.That(validationErrorSet.ResponseValidationErrors.Any(x => x.ResponseOrderWithinAnswerPart == 2), Is.True);
            Assert.That(validationErrorSet.ResponseValidationErrors.Any(x => x.ResponseOrderWithinAnswerPart == 4), Is.True);

            Assert.That(validationErrorSet.ResponseValidationErrors.All(x => x.ValidationErrors.Single().Equals(
                "Value exceeds maximum length (10 characters)")), Is.True);
        });
    }
    #endregion

    #region Test Data Creation
    private static DataShareRequestQuestionAnswerPartResponseFreeForm CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(
        string? enteredValue = null,
        int? orderWithinAnswerPart = null,
        bool? multipleResponsesAreAllowed = null)
    {
        return new DataShareRequestQuestionAnswerPartResponseFreeForm
        {
            EnteredValue = enteredValue ?? string.Empty,
            OrderWithinAnswerPart = orderWithinAnswerPart ?? 0,
            MultipleResponsesAreAllowed = multipleResponsesAreAllowed ?? false
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

        var freeFormResponseExceedsMaximumLengthValidationRule = new FreeFormResponseExceedsMaximumLengthValidationRule(
            mockInputConstraintConfigurationPresenter.Object);

        return new TestItems(
            freeFormResponseExceedsMaximumLengthValidationRule,
            mockInputConstraintConfigurationPresenter);
    }

    private class TestItems(
        IFreeFormResponseExceedsMaximumLengthValidationRule freeFormResponseExceedsMaximumLengthValidationRule,
        Mock<IInputConstraintConfigurationPresenter> mockInputConstraintConfigurationPresenter)
    {
        public IFreeFormResponseExceedsMaximumLengthValidationRule FreeFormResponseExceedsMaximumLengthValidationRule { get; } = freeFormResponseExceedsMaximumLengthValidationRule;
        public Mock<IInputConstraintConfigurationPresenter> MockInputConstraintConfigurationPresenter { get; } = mockInputConstraintConfigurationPresenter;
    }
    #endregion
}