using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules
{
    public interface IValidationRule
    {
        bool Accepts(QuestionPartAnswerValidationRuleModelData validationRule);

        bool ResponseFailsValidation(IQuestionAnswerPartResponseForValidation questionAnswerPartResponseForValidation);
    }
}
