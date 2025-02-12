using System.Globalization;

namespace Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;

internal class ResponseFormatter : IResponseFormatter
{
    bool IResponseFormatter.TryFormatNumericResponse(
        string numericValue,
        out int? parsedNumber)
    {
        var parsedOk = int.TryParse(
        numericValue,
            out var parsedValue);

        parsedNumber = parsedOk ? parsedValue : null;

        return parsedOk;
    }

    bool IResponseFormatter.TryFormatDateResponse(string dateValue, out DateTime? parsedDate)
    {
        var parsedOk = DateTime.TryParseExact(
            dateValue,
            "yyyyMMdd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var parsedValue);

        parsedDate = parsedOk ? parsedValue : null;

        return parsedOk;
    }

    bool IResponseFormatter.TryFormatTimeResponse(string timeValue, out TimeSpan? parsedTime)
    {
        var parsedOk = DateTime.TryParseExact(
            timeValue,
            "HH:mm:ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var parsedValue);

        parsedTime = parsedOk ? parsedValue.TimeOfDay : null;

        return parsedOk;
    }

    bool IResponseFormatter.TryFormatDateTimeResponse(string dateTimeValue, out DateTime? parsedDateTime)
    {
        var parsedOk = DateTime.TryParseExact(
            dateTimeValue,
            "yyyyMMdd HH:mm:ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var parsedValue);

        parsedDateTime = parsedOk ? parsedValue : null;

        return parsedOk;
    }
}