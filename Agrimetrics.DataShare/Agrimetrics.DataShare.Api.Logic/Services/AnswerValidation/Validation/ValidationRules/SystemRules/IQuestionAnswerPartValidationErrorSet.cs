﻿namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules.SystemValidationRules;

public interface IQuestionAnswerPartValidationErrorSet
{
    IEnumerable<IQuestionAnswerPartResponseValidationErrorSet> ResponseValidationErrors { get; }
}