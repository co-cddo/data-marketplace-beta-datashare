namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

/// <summary>
/// The values in this enum need to match those in the table dbo.ResponseFormat
/// </summary>
public enum QuestionPartResponseFormatType
{
    Text,
    Numeric,
    Date,
    Time,
    DateTime,

    SelectSingle,
    SelectMulti,

    ReadOnly,

    Country
}