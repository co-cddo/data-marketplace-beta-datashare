namespace Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

public class QuestionPartAnswerValidationRuleModelData
{
    public Guid QuestionPartAnswerValidationRule_RuleId { get; set; }

    public QuestionPartAnswerValidationRuleId QuestionPartAnswerValidationRule_Rule { get; set; }

    public string? QuestionPartAnswerValidationRule_QuestionErrorText { get; set; }

    public string QuestionPartAnswerValidationRule_RuleErrorText { get; set; } = string.Empty;
}