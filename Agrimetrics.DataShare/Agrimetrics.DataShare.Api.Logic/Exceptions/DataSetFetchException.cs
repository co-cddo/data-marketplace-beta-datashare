namespace Agrimetrics.DataShare.Api.Logic.Exceptions;

public class DataSetFetchException : Exception
{
    public required int? StatusCode { get; init; }

    public required string? ResponseText { get; init; }

    public required string ExceptionText { get; init; }

    public override string Message => "DataSetFetchException thrown: " + ToString();

    public override string ToString()
    {
        return $"{nameof(StatusCode)}: '{StatusCode}', " +
               $"{nameof(ResponseText)}: '{ResponseText}', " +
               $"{nameof(ExceptionText)}: '{ExceptionText}'";
    }
}