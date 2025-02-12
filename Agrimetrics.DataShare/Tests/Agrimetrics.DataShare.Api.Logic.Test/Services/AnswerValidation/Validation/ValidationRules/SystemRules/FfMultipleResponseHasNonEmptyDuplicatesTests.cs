using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

[TestFixture]
public class FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRuleTests
{
    #region Accepts() Tests
    [Test]
    public void GivenANullDataShareRequestQuestionAnswerPart_WhenICallAccepts_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule.Accepts(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionAnswerPart"));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithFreeFormInputTypeAndWithMultipleResponsesAllowed_WhenICallAccepts_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPartResponse = CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(
            multipleResponsesAreAllowed: true);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
            [testQuestionAnswerPartResponse]);

        var result = testItems.FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule.Accepts(
            testQuestionAnswerPart);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithNonFreeFormInputType_WhenICallAccepts_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
            [new DataShareRequestQuestionAnswerPartResponseSelectionOption()]);

        var result = testItems.FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule.Accepts(
            testQuestionAnswerPart);

        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartThatDoesNotAllowMultipleResponses_WhenICallAccepts_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPartResponse = CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(
            multipleResponsesAreAllowed:false);

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
            [testQuestionAnswerPartResponse]);

        var result = testItems.FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule.Accepts(
            testQuestionAnswerPart);

        Assert.That(result, Is.False);
    }
    #endregion

    #region ResponseFailsValidation() Tests
    [Test]
    public void GivenANullDataShareRequestQuestionAnswerPart_WhenICallResponseFailsValidation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule.ResponseFailsValidation(
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

        Assert.That(() => testItems.FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule.ResponseFailsValidation(
                testQuestionAnswerPart,
                out _),
            Throws.InvalidOperationException.With.Message.EqualTo("Unable to validate whether FreeForm Multi Response Has Non Empty Duplicates for response of incorrect type"));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithNoDuplicatedResponses_WhenICallResponseFailsValidation_ThenFalseIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, enteredValue:"test value 1"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:2, enteredValue:"test value 2"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:3, enteredValue:"test value 3")
        ]);

        var result = testItems.FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithNoDuplicatedResponses_WhenICallResponseFailsValidation_ThenAnEmptyValidationErrorSetIsCreated()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, enteredValue:"test value 1"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:2, enteredValue:"test value 2"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:3, enteredValue:"test value 3")
        ]);

        testItems.FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out var validationErrorSet);

        Assert.That(validationErrorSet.ResponseValidationErrors, Is.Empty);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithDuplicatedResponses_WhenICallResponseFailsValidation_ThenTrueIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, enteredValue:"test value"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:2, enteredValue:"another test value"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:3, enteredValue:"  test value  ")
        ]);

        var result = testItems.FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartWithDuplicatedResponses_WhenICallResponseFailsValidation_ThenASingleValidationErrorIsCreatedForEachDuplicatedResponse()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPart = CreateTestDataShareRequestQuestionAnswerPart(
        [
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:1, enteredValue:"test value 1"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:2, enteredValue:"test value 2"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:3, enteredValue:"test value 3"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:4, enteredValue:"test value 1"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:5, enteredValue:"test value 1"),
            CreateTestDataShareRequestQuestionAnswerPartResponseFreeForm(orderWithinAnswerPart:6, enteredValue:"test value 3")
        ]);

        testItems.FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule.ResponseFailsValidation(
            testQuestionAnswerPart,
            out var validationErrorSet);

        Assert.Multiple(() =>
        {
            Assert.That(validationErrorSet.ResponseValidationErrors, Has.Exactly(5).Items);

            Assert.That(validationErrorSet.ResponseValidationErrors.Any(x => x.ResponseOrderWithinAnswerPart == 1), Is.True);
            Assert.That(validationErrorSet.ResponseValidationErrors.Any(x => x.ResponseOrderWithinAnswerPart == 3), Is.True);
            Assert.That(validationErrorSet.ResponseValidationErrors.Any(x => x.ResponseOrderWithinAnswerPart == 4), Is.True);
            Assert.That(validationErrorSet.ResponseValidationErrors.Any(x => x.ResponseOrderWithinAnswerPart == 5), Is.True);
            Assert.That(validationErrorSet.ResponseValidationErrors.Any(x => x.ResponseOrderWithinAnswerPart == 6), Is.True);

            Assert.That(validationErrorSet.ResponseValidationErrors.All(x => x.ValidationErrors.Single().Equals(
                "This value has been entered more than once")), Is.True);
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
        var freeFormMultipleResponseHasNonEmptyDuplicatesValidationRule = new FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule();

        return new TestItems(
            freeFormMultipleResponseHasNonEmptyDuplicatesValidationRule);
    }

    private class TestItems(
        IFreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule freeFormMultipleResponseHasNonEmptyDuplicatesValidationRule)
    {
        public IFreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule FreeFormMultipleResponseHasNonEmptyDuplicatesValidationRule { get; } = freeFormMultipleResponseHasNonEmptyDuplicatesValidationRule;
    }
    #endregion
}