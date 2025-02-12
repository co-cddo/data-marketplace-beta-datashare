using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation;

internal class QuestionPartAnswerValidation(
    IEnumerable<IValidationRule> validationRules,
    IEnumerable<ISystemValidationRule> systemValidationRules) : IQuestionPartAnswerValidation
{
    #region Types
    private sealed class ValidationRuleWithErrorText
    {
        public required IValidationRule ValidationRule { get; init; }
        public required string ErrorText { get; init; }
    }
    #endregion

    IEnumerable<SetDataShareRequestQuestionAnswerPartResponseValidationError> IQuestionPartAnswerValidation.ValidateQuestionPartAnswer(
        DataShareRequestQuestionAnswerPart questionPartAnswer, 
        QuestionPartAnswerValidationRuleSetModelData validationRuleSet)
    {
        ArgumentNullException.ThrowIfNull(questionPartAnswer);
        ArgumentNullException.ThrowIfNull(validationRuleSet);

        var questionRuleValidationErrors = DoValidateQuestionPartAnswerForConfiguredRules(
            questionPartAnswer, validationRuleSet);

        var systemRuleValidationErrors = DoValidateQuestionPartAnswerForSystemRules(
            questionPartAnswer);

        return questionRuleValidationErrors.Concat(systemRuleValidationErrors);
    }

    private IEnumerable<SetDataShareRequestQuestionAnswerPartResponseValidationError> DoValidateQuestionPartAnswerForConfiguredRules(
            DataShareRequestQuestionAnswerPart questionPartAnswer,
            QuestionPartAnswerValidationRuleSetModelData validationRuleSet)
    {

        var applicableValidationRules = validationRuleSet.QuestionPartAnswerValidationRuleSet_ValidationRules
            .Select(FindMatchingValidationRule);

        return questionPartAnswer.AnswerPartResponses
            .Select(answerPartResponse => IdentifyValidationErrorsForAnswerPartResponse(
                new QuestionAnswerPartResponseForValidation
                {
                    QuestionAnswerPartResponse = answerPartResponse,
                    QuestionAnswerPartIsOptional = validationRuleSet.QuestionPartAnswerValidationRuleSet_AnswerIsOptional
                },
                applicableValidationRules,
                questionPartAnswer.QuestionPartId))
            .Where(x => x.ValidationErrors.Any())
            .ToList();

        ValidationRuleWithErrorText FindMatchingValidationRule(QuestionPartAnswerValidationRuleModelData validationRuleModelData)
        {
            var matchingValidationRules = validationRules.Where(x => x.Accepts(validationRuleModelData)).ToList();

            if (matchingValidationRules.Count == 0) throw new InconsistentDataException("Failed to identify validation rule");
            if (matchingValidationRules.Count > 1) throw new InconsistentDataException("Failed to identify unique validation rule");

            var validationRule = matchingValidationRules[0];

            return new ValidationRuleWithErrorText
            {
                ValidationRule = validationRule,
                ErrorText = validationRuleModelData.QuestionPartAnswerValidationRule_QuestionErrorText ??
                            validationRuleModelData.QuestionPartAnswerValidationRule_RuleErrorText
            };
        }

        SetDataShareRequestQuestionAnswerPartResponseValidationError IdentifyValidationErrorsForAnswerPartResponse(
            IQuestionAnswerPartResponseForValidation questionAnswerPartResponseForValidation,
            IEnumerable<ValidationRuleWithErrorText> validationRulesWithErrorText,
            Guid questionPartId)
        {
            
            var validationRulesThatFailValidation = 
                validationRulesWithErrorText.Where(x => x.ValidationRule.ResponseFailsValidation(questionAnswerPartResponseForValidation));
            
            var validationErrors = validationRulesThatFailValidation.Select(x => x.ErrorText).ToList();

            return new SetDataShareRequestQuestionAnswerPartResponseValidationError
            {
                QuestionPartId = questionPartId,
                ResponseOrderWithinAnswerPart = questionAnswerPartResponseForValidation.QuestionAnswerPartResponse.OrderWithinAnswerPart,
                ValidationErrors = validationErrors
            };
        }
    }

    private IEnumerable<SetDataShareRequestQuestionAnswerPartResponseValidationError> DoValidateQuestionPartAnswerForSystemRules(
            DataShareRequestQuestionAnswerPart questionPartAnswer)
    {
        var applicableSystemRules = systemValidationRules.Where(x => x.Accepts(questionPartAnswer));

        foreach (var systemRule in applicableSystemRules)
        {
            if (systemRule.ResponseFailsValidation(questionPartAnswer, out var validationErrorSet))
            {
                foreach (var responseValidationError in validationErrorSet.ResponseValidationErrors)
                {
                    yield return new SetDataShareRequestQuestionAnswerPartResponseValidationError
                    {
                        QuestionPartId = questionPartAnswer.QuestionPartId,
                        ResponseOrderWithinAnswerPart = responseValidationError.ResponseOrderWithinAnswerPart,
                        ValidationErrors = responseValidationError.ValidationErrors.ToList()
                    };
                }
            }
        }
    }
}