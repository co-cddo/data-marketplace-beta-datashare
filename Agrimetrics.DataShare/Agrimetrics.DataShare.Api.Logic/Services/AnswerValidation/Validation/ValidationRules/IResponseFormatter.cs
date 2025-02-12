namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;

public interface IResponseFormatter
{
    bool TryFormatNumericResponse(string numericValue, out int? parsedNumber);

    bool TryFormatDateResponse(string dateValue, out DateTime? parsedDate);

    bool TryFormatTimeResponse(string timeValue, out TimeSpan? parsedTime);

    bool TryFormatDateTimeResponse(string dateTimeValue, out DateTime? parsedDateTime);
}