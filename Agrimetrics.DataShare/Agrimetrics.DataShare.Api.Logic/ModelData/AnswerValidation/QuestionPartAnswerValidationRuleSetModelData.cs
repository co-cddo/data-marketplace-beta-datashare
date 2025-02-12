using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

public class QuestionPartAnswerValidationRuleSetModelData
{
    public Guid QuestionPartAnswerValidationRuleSet_QuestionPartId { get; set; }

    public bool QuestionPartAnswerValidationRuleSet_AnswerIsOptional { get; set; }

    public QuestionPartResponseFormatType QuestionPartAnswerValidationRuleSet_ResponseFormatType { get; set; }

    public List<QuestionPartAnswerValidationRuleModelData> QuestionPartAnswerValidationRuleSet_ValidationRules { get; set; } = [];
}